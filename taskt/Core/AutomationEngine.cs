using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace taskt.Core
{
    public class AutomationEngineInstance
    {

        //engine variables
        public List<Core.Script.ScriptVariable> VariableList { get; set; }
        public Dictionary<string, object> AppInstances { get; set; }
        public Core.AutomationCommands.ErrorHandlingCommand ErrorHandler;
        public List<ScriptError> ErrorsOccured { get; set; }
        public bool IsCancellationPending { get; set; }
        public bool CurrentLoopCancelled { get; set; }
        private bool IsScriptPaused { get; set; }
        [JsonIgnore]
        public UI.Forms.frmScriptEngine tasktEngineUI { get; set; }
        private System.Diagnostics.Stopwatch sw { get; set; }
        public EngineStatus CurrentStatus { get; set; }
        public EngineSettings engineSettings { get; set; }
        public string FileName { get; set; }
        //events
        public event EventHandler<ReportProgressEventArgs> ReportProgressEvent;
        public event EventHandler<ScriptFinishedEventArgs> ScriptFinishedEvent;
        public event EventHandler<LineNumberChangedEventArgs> LineNumberChangedEvent;

        public Serilog.Core.Logger engineLogger;
        public AutomationEngineInstance()
        {
            //initialize logger
            engineLogger = new Logging().CreateLogger("Engine", Serilog.RollingInterval.Day);
            engineLogger.Information("Engine Class has been initialized");

            //initialize error tracking list
            ErrorsOccured = new List<ScriptError>();

            //set to initialized
            CurrentStatus = EngineStatus.Loaded;

            //get engine settings
            engineSettings = new Core.ApplicationSettings().GetOrCreateApplicationSettings().EngineSettings;
        }

        public void ExecuteScriptAsync(UI.Forms.frmScriptEngine scriptEngine, string filePath)
        {
            engineLogger.Information("Client requesting to execute script using frmEngine");

            tasktEngineUI = scriptEngine;
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                ExecuteScript(filePath, true);
            }).Start();
        }
        public void ExecuteScriptAsync(string filePath)
        {
            engineLogger.Information("Client requesting to execute script independently");

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                ExecuteScript(filePath, true);
            }).Start();
        }
        public void ExecuteScriptXML(string xmlData)
        {
            engineLogger.Information("Client requesting to execute script independently");

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                ExecuteScript(xmlData, false);
            }).Start();
        }

        public void ExecuteScript(string data, bool dataIsFile)
        {
            Core.Client.EngineBusy = true;


            try
            {
 
                CurrentStatus = EngineStatus.Running;

                //create stopwatch for metrics tracking
                sw = new System.Diagnostics.Stopwatch();
                sw.Start();

                //log starting
                ReportProgress("Bot Engine Started: " + DateTime.Now.ToString());

                //get automation script
                Core.Script.Script automationScript;
                if (dataIsFile)
                {
                    ReportProgress("Deserializing File");
                    engineLogger.Information("Script Path: " + data);
                    FileName = data;
                    automationScript = Core.Script.Script.DeserializeFile(data);
                }
                else
                {
                    ReportProgress("Deserializing XML");
                    automationScript = Core.Script.Script.DeserializeXML(data);
                }




                //track variables and app instances
                ReportProgress("Creating Variable List");
                VariableList = automationScript.Variables;
                ReportProgress("Creating App Instance Tracking List");

                //create app instances and merge in global instances
                this.AppInstances = new Dictionary<string, object>();
                var GlobalInstances = GlobalAppInstances.GetInstances();
                foreach (var instance in GlobalInstances)
                {
                    this.AppInstances.Add(instance.Key, instance.Value);
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

            Core.Client.EngineBusy = false;

        }
        public void ExecuteCommand(Core.Script.ScriptAction command)
        {
            //get command
            Core.AutomationCommands.ScriptCommand parentCommand = command.ScriptCommand;

           //update execution line numbers
            LineNumberChanged(parentCommand.LineNumber);

            //handle pause request
            if (parentCommand.PauseBeforeExeucution)
            {
                ReportProgress("Pausing Before Execution");
                IsScriptPaused = true;
            }

            //handle pause
            bool isFirstWait = true;
            while (IsScriptPaused)
            {
                //only show pause first loop
                if (isFirstWait)
                {
                    CurrentStatus = EngineStatus.Paused;
                    ReportProgress("Paused at Line " + parentCommand.LineNumber + " - " + parentCommand.GetDisplayValue());
                    ReportProgress("Paused on Line " + parentCommand.LineNumber + ": " + parentCommand.GetDisplayValue());
                    ReportProgress("[Please select 'Resume' when ready]");
                    isFirstWait = false;
                }

                //wait
                System.Threading.Thread.Sleep(2000);
            }

            CurrentStatus = EngineStatus.Running;

            //handle if cancellation was requested
            if (IsCancellationPending)
            {
                return;
            }


            //bypass comments
            if (parentCommand is Core.AutomationCommands.CommentCommand || parentCommand.IsCommented)
            {
                ReportProgress("Skipping Line " + parentCommand.LineNumber + ": " + parentCommand.GetDisplayValue().ConvertToUserVariable(this));
                return;
            }

            //report intended execution
            ReportProgress("Running Line " + parentCommand.LineNumber + ": " + parentCommand.GetDisplayValue());
        

            //handle any errors
            try
            {
                //determine type of command
                if ((parentCommand is Core.AutomationCommands.BeginNumberOfTimesLoopCommand) || (parentCommand is Core.AutomationCommands.BeginContinousLoopCommand) || (parentCommand is Core.AutomationCommands.BeginListLoopCommand) || (parentCommand is Core.AutomationCommands.BeginIfCommand) || parentCommand is Core.AutomationCommands.BeginExcelDatasetLoopCommand)
                {
                    //run the command and pass bgw/command as this command will recursively call this method for sub commands
                    parentCommand.RunCommand(this, command);
                }
                else if (parentCommand is Core.AutomationCommands.SequenceCommand)
                {
                    parentCommand.RunCommand(this, command);
                }
                else if (parentCommand is Core.AutomationCommands.StopTaskCommand)
                {
                    IsCancellationPending = true;
                    return;
                }
                else if (parentCommand is Core.AutomationCommands.ExitLoopCommand)
                {
                    CurrentLoopCancelled = true;
                }
                else if(parentCommand is Core.AutomationCommands.SetEngineDelayCommand)
                {
                    //get variable
                    var setEngineCommand = (Core.AutomationCommands.SetEngineDelayCommand)parentCommand;                    
                    var engineDelay = setEngineCommand.v_EngineSpeed.ConvertToUserVariable(this);
                    var delay = int.Parse(engineDelay);

                    //update delay setting
                    this.engineSettings.DelayBetweenCommands = delay;
                }
                else
                {
                    //sleep required time
                    System.Threading.Thread.Sleep(engineSettings.DelayBetweenCommands);

                    //run the command
                    parentCommand.RunCommand(this);
                }
            }
            catch (Exception ex)
            {
                ErrorsOccured.Add(new ScriptError() { LineNumber = parentCommand.LineNumber, ErrorMessage = ex.Message, StackTrace = ex.ToString() });

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
                        
                            throw new Exception(ex.ToString());
                    }
                }
                else
                {
                
                    throw new Exception(ex.ToString());
                }
            }
        }
        public void AddAppInstance(string instanceName, object appObject) {

            if (AppInstances.ContainsKey(instanceName) && engineSettings.OverrideExistingAppInstances)
            {
                ReportProgress("Overriding Existing Instance: " + instanceName);
                AppInstances.Remove(instanceName);
            }
            else if (AppInstances.ContainsKey(instanceName) && !engineSettings.OverrideExistingAppInstances)
            {
                throw new Exception("App Instance already exists and override has been disabled in engine settings! Enable override existing app instances or use unique instance names!");
            }

            try
            {
                this.AppInstances.Add(instanceName, appObject);
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

        public void CancelScript()
        {
            IsCancellationPending = true;
        }
        public void PauseScript()
        {
            IsScriptPaused = true;
        }
        public void ResumeScript()
        {
            IsScriptPaused = false;
        }

        

        public virtual void ReportProgress(string progress)
        {
            engineLogger.Information(progress);
            ReportProgressEventArgs args = new ReportProgressEventArgs();
            args.ProgressUpdate = progress;
            //send log to server
            Core.Sockets.SocketClient.SendExecutionLog(progress);

            //invoke event
            ReportProgressEvent?.Invoke(this, args);
        }
        public virtual void ScriptFinished(ScriptFinishedEventArgs.ScriptFinishedResult result, string error = null)
        {
            engineLogger.Information("Result Code: " + result.ToString());

            if (error == null)
            {
                engineLogger.Information("Error: None");
            }
            else
            {
                engineLogger.Information("Error: " + error);
            }

            engineLogger.Dispose();


            CurrentStatus = EngineStatus.Finished;
            ScriptFinishedEventArgs args = new ScriptFinishedEventArgs();
            args.LoggedOn = DateTime.Now;
            args.Result = result;
            args.Error = error;
            args.ExecutionTime = sw.Elapsed;
            args.FileName = FileName;

            //convert to json
            var serializedArguments = Newtonsoft.Json.JsonConvert.SerializeObject(args);

            //write execution metrics
            if ((engineSettings.TrackExecutionMetrics) && (FileName != null))
            {
                var summaryLogger = new Logging().CreateJsonLogger("Execution Summary", Serilog.RollingInterval.Infinite);
                summaryLogger.Information(serializedArguments);
                summaryLogger.Dispose();
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

    }
    public class ReportProgressEventArgs : EventArgs
    {
        public string ProgressUpdate { get; set; }
    }
    public class ScriptFinishedEventArgs : EventArgs
    {
        public DateTime LoggedOn { get; set; }
        public ScriptFinishedResult Result { get; set; }
        public string Error { get; set; }
        public TimeSpan ExecutionTime { get; set; }
        public string FileName { get; set; }
        public enum ScriptFinishedResult
        {
            Successful, Error, Cancelled
        }
    }
    public class LineNumberChangedEventArgs : EventArgs
    {
       public int CurrentLineNumber { get; set; }
    }

    public class ScriptError
    {
        public int LineNumber { get; set; }
        public string StackTrace { get; set; }
        public string ErrorMessage { get; set; }
    }


}
