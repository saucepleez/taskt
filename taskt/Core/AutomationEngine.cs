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
        public bool IsCancellationPending { get; set; }
        private bool IsScriptPaused { get; set; }
        public UI.Forms.frmScriptEngine tasktEngineUI { get; set; }
        private System.Diagnostics.Stopwatch sw { get; set; }
        public EngineStatus CurrentStatus { get; set; }
        public EngineSettings engineSettings { get; set; }
        //events
        public event EventHandler<ReportProgressEventArgs> ReportProgressEvent;
        public event EventHandler<ScriptFinishedEventArgs> ScriptFinishedEvent;
        public event EventHandler<LineNumberChangedEventArgs> LineNumberChangedEvent;

        public AutomationEngineInstance()
        {
            //set to initialized
            CurrentStatus = EngineStatus.Loaded;

            //get engine settings
            engineSettings = new Core.ApplicationSettings().GetOrCreateApplicationSettings().EngineSettings;
        }

        public void ExecuteScriptAsync(UI.Forms.frmScriptEngine scriptEngine, string filePath)
        {
            tasktEngineUI = scriptEngine;
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                ExecuteScript(filePath);
            }).Start();
        }
        public void ExecuteScriptAsync(string filePath)
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                ExecuteScript(filePath);
            }).Start();
        }

        public void ExecuteScript(string filePath)
        {
            CurrentStatus = EngineStatus.Running;

            try
            {
                //create stopwatch for metrics tracking
                sw = new System.Diagnostics.Stopwatch();
                sw.Start();

                //log starting
                ReportProgress("Bot Engine Started: " + DateTime.Now.ToString());

                //parse file or streamed XML from tasktServer
                ReportProgress("Deserializing File");
                var automationScript = Core.Script.Script.DeserializeFile(filePath);

                //track variables and app instances
                ReportProgress("Creating Variable List");
                VariableList = automationScript.Variables;
                ReportProgress("Creating App Instance Tracking List");
                AppInstances = new Dictionary<string, object>();


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
                ReportProgress("Skipping Line " + parentCommand.LineNumber + ": " + parentCommand.GetDisplayValue());
                return;
            }

            //report intended execution
            ReportProgress("Running Line " + parentCommand.LineNumber + ": " + parentCommand.GetDisplayValue());
        

            //handle any errors
            try
            {
                //determine type of command
                if ((parentCommand is Core.AutomationCommands.BeginNumberOfTimesLoopCommand) || (parentCommand is Core.AutomationCommands.BeginListLoopCommand) || (parentCommand is Core.AutomationCommands.BeginIfCommand) || parentCommand is Core.AutomationCommands.BeginExcelDatasetLoopCommand)
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
                   // bgw.CancelAsync();
                    return;
                }
                else
                {
                    //run the command
                    parentCommand.RunCommand(this);
                }
            }
            catch (Exception ex)
            {
                Logging.log.Error("Exception Occured:" + ex.ToString());
                //error occuured so decide what user selected
                if (ErrorHandler != null)
                {
                    switch (ErrorHandler.v_ErrorHandlingAction)
                    {
                        case "Continue Processing":
                           Logging.log.Warn("User set continue processing");
                           ReportProgress("Error Occured at Line " + parentCommand.LineNumber + ":" + ex.ToString());
                           ReportProgress("Continuing Per Error Handling");

                            break;

                        default:
                            Logging.log.Warn("User did not handle Exception");
                            throw new Exception(ex.ToString());
                    }
                }
                else
                {
                    Logging.log.Error("User did not handle Exception");
                    throw new Exception(ex.ToString());
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
            ReportProgressEventArgs args = new ReportProgressEventArgs();
            args.ProgressUpdate = progress;
            ReportProgressEvent?.Invoke(this, args);
        }
        public virtual void ScriptFinished(ScriptFinishedEventArgs.ScriptFinishedResult result, string error = null)
        {
            CurrentStatus = EngineStatus.Finished;
            ScriptFinishedEventArgs args = new ScriptFinishedEventArgs();
            args.Result = result;
            args.Error = error;
            args.ExecutionTime = sw.Elapsed;
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
        public ScriptFinishedResult Result { get; set; }
        public string Error { get; set; }
        public TimeSpan ExecutionTime { get; set; }
        public enum ScriptFinishedResult
        {
            Successful, Error, Cancelled
        }
    }
    public class LineNumberChangedEventArgs : EventArgs
    {
       public int CurrentLineNumber { get; set; }
    }
}
