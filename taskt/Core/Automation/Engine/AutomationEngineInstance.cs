﻿using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Reflection;
using taskt.Core.Server;
using taskt.Core.Automation.Commands;
using taskt.Core.Script;

namespace taskt.Core.Automation.Engine
{
    public class AutomationEngineInstance : IAppInstancesProperties
    {
        /// <summary>
        /// script variable list
        /// </summary>
        public List<ScriptVariable> VariableList { get; set; }

        /// <summary>
        /// app instances list
        /// </summary>
        public Dictionary<string, object> AppInstances { get; set; }

        public Dictionary<string, Script.Script> PreloadedTasks { get; set; }
        public ErrorHandlingCommand ErrorHandler;
        public List<ScriptError> ErrorsOccured { get; set; }
        public bool IsCancellationPending { get; set; }
        public bool CurrentLoopCancelled { get; set; }
        public bool CurrentLoopContinuing { get; set; }
        private bool IsScriptPaused { get; set; }
        [JsonIgnore]
        public UI.Forms.ScriptEngine.frmScriptEngine tasktEngineUI { get; set; }
        private System.Diagnostics.Stopwatch sw { get; set; }
        public EngineStatus CurrentStatus { get; set; }
        public EngineSettings engineSettings { get; set; }
        public ServerSettings serverSettings { get; set; }
        public List<DataTable> DataTables { get; set; }
        public string FileName { get; set; }
        public Task taskModel { get; set; }
        public bool serverExecution { get; set; }
        public List<RestResponse> ServiceResponses { get; set; }
        //events
        public event EventHandler<ReportProgressEventArgs> ReportProgressEvent;
        public event EventHandler<ScriptFinishedEventArgs> ScriptFinishedEvent;
        public event EventHandler<LineNumberChangedEventArgs> LineNumberChangedEvent;

        public bool AutoCalculateVariables { get; set; }
        public string TasktResult { get; set; } = "";

        private Serilog.Core.Logger engineLogger = null;

        private const int INNER_VARIABLES = 4;

        public AutomationEngineInstance(bool enableLog = true)
        {
            //initialize logger
            if (enableLog)
            {
                engineLogger = new Logging().CreateLogger("Engine", Serilog.RollingInterval.Day);
                WriteLog("Engine Class has been initialized");
            }

            //initialize error tracking list
            ErrorsOccured = new List<ScriptError>();

            //set to initialized
            CurrentStatus = EngineStatus.Loaded;

            //get engine settings
            var settings = new ApplicationSettings().GetOrCreateApplicationSettings();
            engineSettings = settings.EngineSettings;
            serverSettings = settings.ServerSettings;

            VariableList = new List<ScriptVariable>();

            if (PreloadedTasks is null)
            {
                PreloadedTasks = new Dictionary<string, Script.Script>();
            }
      
            AppInstances = new Dictionary<string, object>();
            ServiceResponses = new List<RestResponse>();
            DataTables = new List<DataTable>();

            //this value can be later overriden by script
            AutoCalculateVariables = engineSettings.AutoCalcVariables;
        }

        public void ExecuteScriptAsync(UI.Forms.ScriptEngine.frmScriptEngine scriptEngine, string filePath, List<ScriptVariable> variables = null, Dictionary<string, Script.Script> preloadedTasks = null)
        {
            WriteLog("Client requesting to execute script using frmEngine");

            tasktEngineUI = scriptEngine;

            if (variables != null)
            {
                VariableList = variables;
            }

            if (preloadedTasks != null)
            {
                PreloadedTasks = preloadedTasks;
            }

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                ExecuteScript(filePath, true);
            }).Start();
        }

        public void ExecuteScriptAsync(string filePath)
        {
            WriteLog("Client requesting to execute script independently");

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                ExecuteScript(filePath, true);
            }).Start();
        }

        public void ExecuteScriptXML(string xmlData)
        {
            WriteLog("Client requesting to execute script independently");

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                ExecuteScript(xmlData, false);
            }).Start();
        }

        public void ExecuteScript(string data, bool dataIsFile)
        {
            Client.EngineBusy = true;

            try
            {
                CurrentStatus = EngineStatus.Running;

                //create stopwatch for metrics tracking
                sw = new System.Diagnostics.Stopwatch();
                sw.Start();

                //log starting
                ReportProgress("Bot Engine Started: " + DateTime.Now.ToString());

                //determine if preloaded script exists
                bool preLoadedTask = false;
                if (PreloadedTasks != null)
                {
                    preLoadedTask = PreloadedTasks.Any(f => f.Key == data);
                }

                //get automation script
                Script.Script automationScript;
                if (dataIsFile && (!preLoadedTask))
                {
                    ReportProgress("Deserializing File");
                    WriteLog("Script Path: " + data);
                    FileName = data;              
                    automationScript = Script.Script.DeserializeFile(data, engineSettings);
                }
                else if (dataIsFile && preLoadedTask)
                {
                    ReportProgress("Using Preloaded Task");
                    WriteLog("Preloaded Script Path: " + data);
                    FileName = data;
                    automationScript = PreloadedTasks[data];
                }
                else
                {
                    ReportProgress("Deserializing XML");
                    automationScript = Script.Script.DeserializeXML(data);
                }

                if (serverSettings.ServerConnectionEnabled && taskModel == null)
                {
                    taskModel = HttpServerClient.AddTask(data);
                }
                else if (serverSettings.ServerConnectionEnabled && taskModel != null)
                {
                    taskModel = HttpServerClient.UpdateTask(taskModel.TaskID, "Running", "Running Server Assignment");
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

                // add hidden inner variable
                VariableList.AddRange(
                    new ScriptVariable[]
                    {
                        new ScriptVariable()
                        {
                            VariableName = "__INNER_0",
                            VariableValue = ""
                        },
                        new ScriptVariable()
                        {
                            VariableName = "__INNER_1",
                            VariableValue = ""
                        },
                        new ScriptVariable()
                        {
                            VariableName = "__INNER_2",
                            VariableValue = ""
                        },
                        new ScriptVariable()
                        {
                            VariableName = "__INNER_3",
                            VariableValue = ""
                        }
                    }
                );

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
        }

        public void ExecuteCommand(ScriptAction command)
        {
            //get command
            ScriptCommand parentCommand = command.ScriptCommand;

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
                Thread.Sleep(2000);
            }

            CurrentStatus = EngineStatus.Running;

            //handle if cancellation was requested
            if (IsCancellationPending)
            {
                return;
            }

            //bypass comments
            if (parentCommand is CommentCommand || parentCommand.IsCommented)
            {
                ReportProgress("Skipping Line " + parentCommand.LineNumber + ": " + parentCommand.GetDisplayValue().ExpandValueOrUserVariable(this));
                return;
            }

            //report intended execution
            ReportProgress("Running Line " + parentCommand.LineNumber + ": " + parentCommand.GetDisplayValue());

            //handle any errors
            try
            {
                //determine type of command
                if ((parentCommand is BeginNumberOfTimesLoopCommand) || (parentCommand is BeginContinousLoopCommand) || (parentCommand is BeginListLoopCommand) || (parentCommand is BeginIfCommand) || (parentCommand is BeginMultiIfCommand) || (parentCommand is TryCommand) || (parentCommand is BeginLoopCommand) || (parentCommand is BeginMultiLoopCommand))
                {
                    //run the command and pass bgw/command as this command will recursively call this method for sub commands
                    parentCommand.RunCommand(this, command);
                }
                else if (parentCommand is SequenceCommand)
                {
                    // todo: execute runcommand
                    parentCommand.RunCommand(this, command);
                }
                else
                {
                    //sleep required time
                    Thread.Sleep(engineSettings.DelayBetweenCommands);

                    //run the command
                    parentCommand.RunCommand(this);
                }

                if (IsCancellationPending)
                {
                    return;
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
                            throw ex;
                    }
                }
                else
                {
                    throw ex;
                }
            }
            finally
            {
                ClearInnerVariables();
            }
        }

        /// <summary>
        /// add App Instance in Engine List (wrapper)
        /// </summary>
        /// <param name="instanceName"></param>
        /// <param name="appObject"></param>
        /// <exception cref="Exception"></exception>
        public void AddAppInstance(string instanceName, object appObject) 
        {
            this.AddAppInstance(instanceName, appObject, this);
        }

        /// <summary>
        /// get App Instance from Engine List (wrapper)
        /// </summary>
        /// <param name="instanceName"></param>
        /// <returns></returns>
        public object GetAppInstance(string instanceName)
        {
            return ((IAppInstancesProperties)this).GetAppInstance(instanceName);
        }

        /// <summary>
        /// remove App Instance from Engine List (wrapper)
        /// </summary>
        /// <param name="instanceName"></param>
        public void RemoveAppInstance(string instanceName)
        {
            ((IAppInstancesProperties)this).RemoveAppInstance(instanceName);
        }

        /// <summary>
        /// Get New App Instance name (wrapper)
        /// </summary>
        /// <param name="prefixName"></param>
        /// <returns></returns>
        public string GetNewAppInstanceName(string prefixName = "")
        {
            return ((IAppInstancesProperties)this).GetNewInstanceName(prefixName);
        }

        //public void AddVariable(string variableName, object variableValue)
        //{
        //    if (VariableList.Any(f => f.VariableName == variableName))
        //    {
        //        //update existing variable
        //        var existingVariable = VariableList.FirstOrDefault(f => f.VariableName == variableName);
        //        existingVariable.VariableName = variableName;
        //        existingVariable.VariableValue = variableValue;
        //    }
        //    else if (VariableList.Any(f => "{" + f.VariableName + "}" == variableName))
        //    {
        //        //update existing edge-case variable due to user error
        //        var existingVariable = VariableList.FirstOrDefault(f => "{" + f.VariableName + "}" == variableName);
        //        existingVariable.VariableName = variableName;
        //        existingVariable.VariableValue = variableValue;
        //    }
        //    else
        //    {
        //        //add new variable
        //        var newVariable = new ScriptVariable();
        //        newVariable.VariableName = variableName;
        //        newVariable.VariableValue = variableValue;
        //        VariableList.Add(newVariable);
        //    }
        //}

        //public void StoreComplexObjectInVariable(string variableName, object value)
        //{
        //    ScriptVariable storeVariable = VariableList.Where(x => x.VariableName == variableName).FirstOrDefault();

        //    if (storeVariable == null)
        //    {
        //        //create and set variable
        //        VariableList.Add(new ScriptVariable
        //        {
        //            VariableName = variableName,
        //            VariableValue = value
        //       });            
        //    }
        //    else
        //    {
        //        //set variable
        //        storeVariable.VariableValue = value;
        //    }
        //}

        private void ClearInnerVariables()
        {
            for (int i = 0; i < INNER_VARIABLES; i++)
            {
                ScriptVariable v = VariableList.Where(x => x.VariableName == "__INNER_" + i.ToString()).FirstOrDefault();
                if (v != null)
                {
                    v.VariableValue = "";
                }
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
            WriteLog(progress);
            ReportProgressEventArgs args = new ReportProgressEventArgs();
            args.ProgressUpdate = progress;
            //send log to server
            SocketClient.SendExecutionLog(progress);

            //invoke event
            ReportProgressEvent?.Invoke(this, args);
        }

        public virtual void ScriptFinished(ScriptFinishedEventArgs.ScriptFinishedResult result, string error = null)
        {
            WriteLog("Result Code: " + result.ToString());

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
                WriteLog("Error: None");

                if (taskModel != null && serverSettings.ServerConnectionEnabled)
                {
                    HttpServerClient.UpdateTask(taskModel.TaskID, "Completed", "Script Completed Successfully");
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
                WriteLog("Error: " + error);

                if (taskModel != null)
                {
                    HttpServerClient.UpdateTask(taskModel.TaskID, "Error", error);
                }

                TasktResult = error;
            }

            engineLogger.Dispose();

            CurrentStatus = EngineStatus.Finished;
            ScriptFinishedEventArgs args = new ScriptFinishedEventArgs();
            args.LoggedOn = DateTime.Now;
            args.Result = result;
            args.Error = error;
            args.ExecutionTime = sw.Elapsed;
            args.FileName = FileName;

            SocketClient.SendExecutionLog("Result Code: " + result.ToString());
            SocketClient.SendExecutionLog("Total Execution Time: " + sw.Elapsed);

            //convert to json
            var serializedArguments = JsonConvert.SerializeObject(args);

            //write execution metrics
            if ((engineSettings.TrackExecutionMetrics) && (FileName != null))
            {
                var summaryLogger = new Logging().CreateJsonLogger("Execution Summary", Serilog.RollingInterval.Infinite);
                summaryLogger.Information(serializedArguments);
                summaryLogger.Dispose();
            }

            Client.EngineBusy = false;

            if (serverSettings.ServerConnectionEnabled)
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

        public void WriteLog(string logText)
        {
            if (engineLogger != null)
            {
                engineLogger.Information(logText);
            }
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
