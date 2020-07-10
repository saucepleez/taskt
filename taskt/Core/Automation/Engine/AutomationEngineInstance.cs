using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using taskt.Core.Server;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Core.App;
using taskt.UI.Forms;
using taskt.Core.Script;
using taskt.Core.Automation.Commands;
using taskt.Core.Automation.Engine.EngineEventArgs;
using Serilog.Core;
using taskt.Core.Settings;
using taskt.Core.Server.Models;

namespace taskt.Core.Automation.Engine
{
    public class AutomationEngineInstance
    {
        //engine variables
        public List<ScriptVariable> VariableList { get; set; }
        public Dictionary<string, object> AppInstances { get; set; }
        public ErrorHandlingCommand ErrorHandler;
        public List<ScriptError> ErrorsOccured { get; set; }
        public bool ChildScriptFailed { get; set; }
        public bool ChildScriptErrorCaught { get; set; }
        public ScriptCommand LastExecutedCommand { get; set; }
        public bool IsCancellationPending { get; set; }
        public bool CurrentLoopCancelled { get; set; }
        public bool CurrentLoopContinuing { get; set; }
        public bool _isScriptPaused { get; private set; }
        private bool _isScriptSteppedOver { get; set; }
        private bool _isScriptSteppedInto { get; set; }
        [JsonIgnore]
        public frmScriptEngine TasktEngineUI { get; set; }
        private Stopwatch _stopWatch { get; set; }
        private EngineStatus _currentStatus { get; set; }
        public EngineSettings EngineSettings { get; set; }
        private ServerSettings _serverSettings { get; set; }
        public List<DataTable> DataTables { get; set; }
        public string FileName { get; set; }
        public Task TaskModel { get; set; }
        public bool ServerExecution { get; set; }
        public List<IRestResponse> ServiceResponses { get; set; }
        public bool AutoCalculateVariables { get; set; }
        public string TasktResult { get; set; } = "";
        //events
        public event EventHandler<ReportProgressEventArgs> ReportProgressEvent;
        public event EventHandler<ScriptFinishedEventArgs> ScriptFinishedEvent;
        public event EventHandler<LineNumberChangedEventArgs> LineNumberChangedEvent;
        public Logger EngineLogger;

        public AutomationEngineInstance()
        {
            //initialize logger
            EngineLogger = new Logging().CreateLogger("Engine", Serilog.RollingInterval.Day);
            EngineLogger.Information("Engine Class has been initialized");

            //initialize error tracking list
            ErrorsOccured = new List<ScriptError>();

            //set to initialized
            _currentStatus = EngineStatus.Loaded;

            //get engine settings
            var settings = new ApplicationSettings().GetOrCreateApplicationSettings();
            EngineSettings = settings.EngineSettings;
            _serverSettings = settings.ServerSettings;

            VariableList = new List<ScriptVariable>();
            AppInstances = new Dictionary<string, object>();
            ServiceResponses = new List<IRestResponse>();
            DataTables = new List<DataTable>();

            //this value can be later overriden by script
            AutoCalculateVariables = EngineSettings.AutoCalcVariables;
        }

        public void ExecuteScriptAsync(frmScriptEngine scriptEngine, string filePath, List<ScriptVariable> variables = null)
        {
            EngineLogger.Information("Client requesting to execute script using frmEngine");

            TasktEngineUI = scriptEngine;

            if (variables != null)
            {
                VariableList = variables;
            }

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                ExecuteScript(filePath, true);
            }).Start();
        }

        public void ExecuteScriptAsync(string filePath)
        {
            EngineLogger.Information("Client requesting to execute script independently");

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                ExecuteScript(filePath, true);
            }).Start();
        }

        public void ExecuteScriptJson(string jsonData)
        {
            EngineLogger.Information("Client requesting to execute script independently");

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                ExecuteScript(jsonData, false);
            }).Start();
        }

        private void ExecuteScript(string data, bool dataIsFile)
        {
            Client.EngineBusy = true;

            try
            {
                _currentStatus = EngineStatus.Running;

                //create stopwatch for metrics tracking
                _stopWatch = new Stopwatch();
                _stopWatch.Start();

                //log starting
                ReportProgress("Bot Engine Started: " + DateTime.Now.ToString());

                //get automation script
                Script.Script automationScript;
                if (dataIsFile)
                {
                    ReportProgress("Deserializing File");
                    EngineLogger.Information("Script Path: " + data);
                    FileName = data;
                    automationScript = Script.Script.DeserializeFile(data);
                }
                else
                {
                    ReportProgress("Deserializing JSON");
                    automationScript = Script.Script.DeserializeJsonString(data);
                }

                if (_serverSettings.ServerConnectionEnabled && TaskModel == null)
                {
                    TaskModel = HttpServerClient.AddTask(data);
                }
                else if (_serverSettings.ServerConnectionEnabled && TaskModel != null)
                {
                    TaskModel = HttpServerClient.UpdateTask(TaskModel.TaskID, "Running", "Running Server Assignment");
                }

                //track variables and app instances
                ReportProgress("Creating Variable List");

                //set variables if they were passed in
                if (VariableList != null)
                {
                    foreach (var var in VariableList)
                    {
                        var variableFound = automationScript.Variables.Where(f => f.VariableName == var.VariableName).FirstOrDefault();

                        if (variableFound != null)
                        {
                            variableFound.VariableValue = var.VariableValue;
                        }
                    }
                }

                VariableList = automationScript.Variables;

                ReportProgress("Creating App Instance Tracking List");
                //create app instances and merge in global instances
                AppInstances = new Dictionary<string, object>();
                var GlobalInstances = GlobalAppInstances.GetInstances();
                foreach (var instance in GlobalInstances)
                {
                    AppInstances.Add(instance.Key, instance.Value);
                }

                //execute commands
                foreach (var executionCommand in automationScript.Commands)
                {
                    if (IsCancellationPending)
                    {
                        ReportProgress("Cancelling Script");
                        ScriptFinished(ScriptFinishedEventArgs.ScriptFinishedResult.Cancelled);
                        return;
                    }

                    ExecuteCommand(executionCommand);
                }

                if (IsCancellationPending)
                {
                    //mark cancelled - handles when cancelling and user defines 1 parent command or else it will show successful
                    ScriptFinished(ScriptFinishedEventArgs.ScriptFinishedResult.Cancelled);
                }
                else
                {
                    //mark finished
                    ScriptFinished(ScriptFinishedEventArgs.ScriptFinishedResult.Successful);
                }
            }
            catch (Exception ex)
            {
                ScriptFinished(ScriptFinishedEventArgs.ScriptFinishedResult.Error, ex.ToString());
            }
        }

        public void ExecuteCommand(ScriptAction command)
        {
            //get command
            ScriptCommand parentCommand = command.ScriptCommand;

            //set LastCommadExecuted
            LastExecutedCommand = command.ScriptCommand;

            //update execution line numbers
            LineNumberChanged(parentCommand.LineNumber);

            //handle pause request
            if (parentCommand.PauseBeforeExecution)
            {
                ReportProgress("Pausing Before Execution");
                _isScriptPaused = true;
            }

            //handle pause
            bool isFirstWait = true;
            while (_isScriptPaused)
            {
                //only show pause first loop
                if (isFirstWait)
                {
                    _currentStatus = EngineStatus.Paused;
                    ReportProgress("Paused on Line " + parentCommand.LineNumber + ": " + parentCommand.GetDisplayValue());
                    ReportProgress("[Please select 'Resume' when ready]");
                    isFirstWait = false;
                }

                if (_isScriptSteppedInto && parentCommand.CommandName == "RunTaskCommand")
                {
                    parentCommand.IsSteppedInto = true;
                    //TODO: Studio Step Into
                    //((RunTaskCommand)parentCommand).CurrentScriptBuilder = TasktEngineUI.CallBackForm;
                    _isScriptSteppedInto = false;
                    break;
                }
                else if (_isScriptSteppedOver || _isScriptSteppedInto)
                {
                    _isScriptSteppedOver = false;
                    _isScriptSteppedInto = false;
                    break;
                }
               
                //wait
                Thread.Sleep(1000);
            }

            _currentStatus = EngineStatus.Running;

            //handle if cancellation was requested
            if (IsCancellationPending)
            {
                return;
            }

            //If Child Script Failed and Child Script Error not Caught, next command should not be executed
            if (ChildScriptFailed && !ChildScriptErrorCaught)
                throw new Exception("Child Script Failed");

            //bypass comments
            if (parentCommand is AddCodeCommentCommand || parentCommand.IsCommented)
            {
                ReportProgress("Skipping Line " + parentCommand.LineNumber + ": "
                    + parentCommand.GetDisplayValue().ConvertToUserVariable(this));
                return;
            }

            //report intended execution
            ReportProgress("Running Line " + parentCommand.LineNumber + ": " + parentCommand.GetDisplayValue());

            //handle any errors
            try
            {
                //determine type of command
                if ((parentCommand is LoopNumberOfTimesCommand) || (parentCommand is LoopContinuouslyCommand) ||
                    (parentCommand is LoopCollectionCommand) || (parentCommand is BeginIfCommand) ||
                    (parentCommand is BeginMultiIfCommand) || (parentCommand is TryCommand) ||
                    (parentCommand is BeginLoopCommand) || (parentCommand is BeginMultiLoopCommand) ||
                    (parentCommand is BeginRetryCommand))
                {
                    //run the command and pass bgw/command as this command will recursively call this method for sub commands
                    parentCommand.RunCommand(this, command);
                }
                else if (parentCommand is SequenceCommand)
                {
                    parentCommand.RunCommand(this, command);
                }
                else if (parentCommand is StopCurrentTaskCommand)
                {
                    IsCancellationPending = true;
                    return;
                }
                else if (parentCommand is ExitLoopCommand)
                {
                    CurrentLoopCancelled = true;
                }
                else if (parentCommand is NextLoopCommand)
                {
                    CurrentLoopContinuing = true;
                }
                else if(parentCommand is SetEngineDelayCommand)
                {
                    //get variable
                    var setEngineCommand = (SetEngineDelayCommand)parentCommand;
                    var engineDelay = setEngineCommand.v_EngineSpeed.ConvertToUserVariable(this);
                    var delay = int.Parse(engineDelay);

                    //update delay setting
                    EngineSettings.DelayBetweenCommands = delay;
                }
                else
                {
                    //sleep required time
                    Thread.Sleep(EngineSettings.DelayBetweenCommands);

                        //run the command
                        parentCommand.RunCommand(this);
                }
            }
            catch (Exception ex)
            {
                if (!(LastExecutedCommand is RethrowCommand))
                {
                    if (ChildScriptFailed)
                    {
                        ChildScriptFailed = false;
                        ErrorsOccured.Clear();
                    }

                    ErrorsOccured.Add(new ScriptError()
                    {
                        SourceFile = FileName,
                        LineNumber = parentCommand.LineNumber,
                        StackTrace = ex.ToString(),
                        ErrorType = ex.GetType().Name,
                        ErrorMessage = ex.Message
                    });
                }

                //error occuured so decide what user selected
                if (ErrorHandler != null)
                {
                    switch (ErrorHandler.v_ErrorHandlingAction)
                    {
                        case "Continue Processing":
                            ReportProgress("Error Occured at Line " + parentCommand.LineNumber + ":" + ex.ToString());
                            ReportProgress("Continuing Per Error Handling");
                            break;

                        default:
                            throw ex;
                    }
                }
                else
                {
                    throw ex;
                }
            }
        }
        public void AddAppInstance(string instanceName, object appObject) {

            if (AppInstances.ContainsKey(instanceName) && EngineSettings.OverrideExistingAppInstances)
            {
                ReportProgress("Overriding Existing Instance: " + instanceName);
                AppInstances.Remove(instanceName);
            }
            else if (AppInstances.ContainsKey(instanceName) && !EngineSettings.OverrideExistingAppInstances)
            {
                throw new Exception("App Instance already exists and override has been disabled in engine settings! " +
                    "Enable override existing app instances or use unique instance names!");
            }

            try
            {
                AppInstances.Add(instanceName, appObject);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object GetAppInstance(string instanceName)
        {
            try
            {
                if (AppInstances.TryGetValue(instanceName, out object appObject))
                {
                    return appObject;
                }
                else
                {
                    throw new Exception("App Instance '" + instanceName + "' not found!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void RemoveAppInstance(string instanceName)
        {
            try
            {
                if (AppInstances.ContainsKey(instanceName))
                {
                    AppInstances.Remove(instanceName);
                }
                else
                {
                    throw new Exception("App Instance '" + instanceName + "' not found!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddVariable(string variableName, object variableValue)
        {
            if (VariableList.Any(f => f.VariableName == variableName))
            {
                //update existing variable
                var existingVariable = VariableList.FirstOrDefault(f => f.VariableName == variableName);
                existingVariable.VariableName = variableName;
                existingVariable.VariableValue = variableValue;
            }
            else if (VariableList.Any(f => "{" + f.VariableName + "}" == variableName))
            {
                //update existing edge-case variable due to user error
                var existingVariable = VariableList.FirstOrDefault(f => "{" + f.VariableName + "}" == variableName);
                existingVariable.VariableName = variableName;
                existingVariable.VariableValue = variableValue;
            }
            else
            {
                //add new variable
                var newVariable = new ScriptVariable();
                newVariable.VariableName = variableName;
                newVariable.VariableValue = variableValue;
                VariableList.Add(newVariable);
            }
        }

        public void CancelScript()
        {
            IsCancellationPending = true;
        }

        public void PauseScript()
        {
            _isScriptPaused = true;
        }

        public void ResumeScript()
        {
            _isScriptPaused = false;
        }

        public void StepOverScript()
        {
            _isScriptSteppedOver = true;
        }

        public void StepIntoScript()
        {
            _isScriptSteppedInto = true;
        }

        public virtual void ReportProgress(string progress)
        {
            EngineLogger.Information(progress);
            ReportProgressEventArgs args = new ReportProgressEventArgs();
            args.ProgressUpdate = progress;

            //send log to server
            SocketClient.SendExecutionLog(progress);

            //invoke event
            ReportProgressEvent?.Invoke(this, args);
        }

        public virtual void ScriptFinished(ScriptFinishedEventArgs.ScriptFinishedResult result, string error = null)
        {
            if (ChildScriptFailed && !ChildScriptErrorCaught)
            {
                error = "Terminate with failure";
                result = ScriptFinishedEventArgs.ScriptFinishedResult.Error;
            }
            EngineLogger.Information("Result Code: " + result.ToString());

            //add result variable if missing
            var resultVar = VariableList.Where(f => f.VariableName == "taskt.Result").FirstOrDefault();

            //handle if variable is missing
            if (resultVar == null)
            {
                resultVar = new ScriptVariable() { VariableName = "taskt.Result", VariableValue = "" };
            }

            //check value
            var resultValue = resultVar.VariableValue.ToString();

            if (error == null)
            {
                EngineLogger.Information("Error: None");

                if (TaskModel != null && _serverSettings.ServerConnectionEnabled)
                {
                    HttpServerClient.UpdateTask(TaskModel.TaskID, "Completed", "Script Completed Successfully");
                }

                if (string.IsNullOrEmpty(resultValue))
                {
                    TasktResult = "Successfully Completed Script";
                }
                else
                {
                    TasktResult = resultValue;
                }
            }

            else
            {
                error = ErrorsOccured.OrderByDescending(x => x.LineNumber).FirstOrDefault().StackTrace;
                EngineLogger.Information("Error: " + error);

                if (TaskModel != null)
                {
                    HttpServerClient.UpdateTask(TaskModel.TaskID, "Error", error);
                }

                TasktResult = error;
            }

            EngineLogger.Dispose();

            _currentStatus = EngineStatus.Finished;
            ScriptFinishedEventArgs args = new ScriptFinishedEventArgs();
            args.LoggedOn = DateTime.Now;
            args.Result = result;
            args.Error = error;
            args.ExecutionTime = _stopWatch.Elapsed;
            args.FileName = FileName;

            SocketClient.SendExecutionLog("Result Code: " + result.ToString());
            SocketClient.SendExecutionLog("Total Execution Time: " + _stopWatch.Elapsed);

            //convert to json
            var serializedArguments = JsonConvert.SerializeObject(args);

            //write execution metrics
            if ((EngineSettings.TrackExecutionMetrics) && (FileName != null))
            {
                var summaryLogger = new Logging().CreateJsonLogger("Execution Summary", Serilog.RollingInterval.Infinite);
                summaryLogger.Information(serializedArguments);
                summaryLogger.Dispose();
            }

            Client.EngineBusy = false;

            if (_serverSettings.ServerConnectionEnabled)
            {
                HttpServerClient.CheckIn();
            }

            ScriptFinishedEvent?.Invoke(this, args);
        }

        public virtual void LineNumberChanged(int lineNumber)
        {
            LineNumberChangedEventArgs args = new LineNumberChangedEventArgs();
            args.CurrentLineNumber = lineNumber;
            LineNumberChangedEvent?.Invoke(this, args);
        }

        public enum EngineStatus
        {
            Loaded, Running, Paused, Finished
        }

        public string GetEngineContext()
        {
            //set json settings
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Error = (serializer, err) =>
            {
                err.ErrorContext.Handled = true;
            };
            settings.Formatting = Formatting.Indented;

            return  JsonConvert.SerializeObject(this, settings);
        }
    }
}
