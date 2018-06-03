//Copyright (c) 2017 Jason Bayldon
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace sharpRPA.UI.Forms
{
    public partial class frmScriptEngine : UIForm
    {
        #region Form Events

        public string filePath { get; set; }
        public string xmlInfo { get; set; }
        private System.Diagnostics.Stopwatch sw;
        public frmScriptBuilder callBackForm { get; set; }
        public List<Core.Script.ScriptVariable> variableList { get; set; }

        public Dictionary<string, object> appInstances { get; set; }
        private bool isPaused { get; set; }
        public Core.AutomationCommands.ErrorHandlingCommand errorHandling;
        public frmScriptEngine(string pathToFile, frmScriptBuilder builderForm)
        {
            filePath = pathToFile;
            callBackForm = builderForm;
            InitializeComponent();
        }

        private void frmProcessingStatus_Load(object sender, EventArgs e)
        {
            // callBackForm.websocket.Send("Running");
            this.BringToFront();
            MoveFormToBottomRight(this);
            bgwRunScript.RunWorkerAsync();
        }

        public bool ShowMessage(string message, string title, UI.Forms.Supplemental.frmDialog.DialogType dialogType, int closeAfter)
        {
            var confirmationForm = new UI.Forms.Supplemental.frmDialog(message, title, dialogType, closeAfter);
            return confirmationForm.ShowDialog() == DialogResult.OK;
        }

        #endregion Form Events

        #region BackgroundWorker

        private void bgwRunScript_DoWork(object sender, DoWorkEventArgs e)
        {
            sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            //bgwRunScript.ReportProgress(0, new object[] { 0, "Bot Engine Started: " + DateTime.Now.ToString() });
            bgwRunScript.ReportProgress(0, new ProgressUpdate() { LineNumber = 0, UpdateText = "Bot Engine Started: " + DateTime.Now.ToString() });

            Core.Script.Script automationScript;

            //parse file or streamed XML from sharpRPAServer
            if (filePath != "")
            {
                automationScript = Core.Script.Script.DeserializeFile(filePath);
            }
            else
            {
                automationScript = Core.Script.Script.DeserializeXML(xmlInfo);
            }

            //track variables and app instances
            variableList = automationScript.Variables;

            appInstances = new Dictionary<string, object>();



            //create execution
            foreach (var executionCommand in automationScript.Commands)
            {
                if (bgwRunScript.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                ExecuteCommand(executionCommand, bgwRunScript);
            }
        }

        public void ExecuteCommand(Core.Script.ScriptAction command, BackgroundWorker bgw)
        {
            //get command
            Core.AutomationCommands.ScriptCommand parentCommand = command.ScriptCommand;

            //handle pause request
            if (parentCommand.PauseBeforeExeucution)
            {
                bgwRunScript.ReportProgress(0, new PauseRequest());
                isPaused = true;
            }

            //handle pause
            bool isFirstWait = true;
            while (isPaused)
            {
                //only show pause first loop
                if (isFirstWait)
                {
                    bgwRunScript.ReportProgress(0, new ProgressUpdate() { LineNumber = parentCommand.LineNumber, UpdateText = "Paused on Line " + parentCommand.LineNumber + ": " + parentCommand.GetDisplayValue() });
                    bgwRunScript.ReportProgress(0, new ProgressUpdate() { LineNumber = parentCommand.LineNumber, UpdateText = "[Please select 'Resume' when ready]" });
                    isFirstWait = false;
                }

                //wait
                System.Threading.Thread.Sleep(2000);
            }






            //bypass comments
            if (parentCommand is Core.AutomationCommands.CommentCommand || parentCommand.IsCommented)
            {
               // bgwRunScript.ReportProgress(0, new object[] { parentCommand.LineNumber, "Skipping Line " + parentCommand.LineNumber + ": " + parentCommand.GetDisplayValue() });
                bgwRunScript.ReportProgress(0, new ProgressUpdate() { LineNumber = parentCommand.LineNumber, UpdateText = "Skipping Line " + parentCommand.LineNumber + ": " + parentCommand.GetDisplayValue() });
                return;
            }

            //update listbox
            //bgwRunScript.ReportProgress(0, new object[] { parentCommand.LineNumber, "Running Line " + parentCommand.LineNumber + ": " + parentCommand.GetDisplayValue() });
            bgwRunScript.ReportProgress(0, new ProgressUpdate() { LineNumber = parentCommand.LineNumber, UpdateText = "Running Line " + parentCommand.LineNumber + ": " + parentCommand.GetDisplayValue() });




            //handle any errors
            try
            {
                //determine type of command
                if ((parentCommand is Core.AutomationCommands.BeginLoopCommand) || (parentCommand is Core.AutomationCommands.BeginIfCommand) || parentCommand is Core.AutomationCommands.BeginExcelDatasetLoopCommand)
                {
                    //run the command and pass bgw/command as this command will recursively call this method for sub commands
                    parentCommand.RunCommand(this, command, bgw);
                }
                else
                {
                    //run the command
                    parentCommand.RunCommand(this);
                }
            }
            catch (Exception ex)
            {
                //error occuured so decide what user selected
                if (errorHandling != null)
                {
                    switch (errorHandling.v_ErrorHandlingAction)
                    {
                        case "Continue Processing":
                            //bgwRunScript.ReportProgress(0, new object[] { parentCommand.LineNumber, "Error Occured at Line " + parentCommand.LineNumber + ":" + ex.ToString() });
                            //bgwRunScript.ReportProgress(0, new object[] { parentCommand.LineNumber, "Continuing Per Error Handling" });
                            bgwRunScript.ReportProgress(0, new ProgressUpdate() { LineNumber = parentCommand.LineNumber, UpdateText = "Error Occured at Line " + parentCommand.LineNumber + ":" + ex.ToString() });
                            bgwRunScript.ReportProgress(0, new ProgressUpdate() { LineNumber = parentCommand.LineNumber, UpdateText = "Continuing Per Error Handling" });

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

        private void bgwRunScript_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            if (e.UserState is ProgressUpdate)
            {
                ProgressUpdate currentProgress = (ProgressUpdate)e.UserState;

                if (callBackForm != null)
                {
                    callBackForm.DebugLine = currentProgress.LineNumber;
                }

                AddStatus(currentProgress.UpdateText);

            }
            else if (e.UserState is PauseRequest)
            {
                lstSteppingCommands.Items.Add("[Script Requested Pause]");
                uiBtnPause.Image = Properties.Resources.restart;
                uiBtnPause.DisplayText = "Resume";
            }

            //object[] progressUpdate = (object[])e.UserState;

            //if ((callBackForm != null) && (progressUpdate[0] != null))
            //    callBackForm.DebugLine = (int)progressUpdate[0];


        }

        private void AddStatus(string text)
        {
            lstSteppingCommands.Items.Add(text + "..");
            lstSteppingCommands.SelectedIndex = lstSteppingCommands.Items.Count - 1;

            if (callBackForm != null)
            {
                callBackForm.SendMessage(text);
            }

        }

        private void bgwRunScript_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            sw.Stop();

            //dispose of any unnecessary application instances
            CleanUpMemory();

            //reset debug line
            if (callBackForm != null)
                callBackForm.DebugLine = 0;
            uiBtnPause.Visible = false;
            uiBtnCancel.DisplayText = "Close";
            uiBtnCancel.Visible = true;

            if ((e.Error == null) && (!e.Cancelled))
            {
                lblMainLogo.Text = "debug info (success)";
                lstSteppingCommands.Items.Add("Bot Finished Task Successfully");
                if (callBackForm != null)
                    callBackForm.Notify("Script Execution Completed Successfully!");
                tmrNotify.Enabled = true;
            }
            else if ((e.Error == null) && (e.Cancelled))
            {
                lblMainLogo.Text = "debug info (cancelled)";
                lstSteppingCommands.Items.Add("Operation Cancelled by User");
                if (callBackForm != null)
                    callBackForm.Notify("Script Execution Cancelled!");
            }
            else
            {
                lblMainLogo.Text = "debug info (error)";
                lstSteppingCommands.Items.Add("Bot Had A Problem: " + e.Error.Message);
                if (callBackForm != null)
                    callBackForm.Notify("Script Execution Encountered an Error!");
            }

            AddStatus("Bot Engine Finished: " + DateTime.Now.ToString());
            AddStatus("Total Run Time: " + sw.Elapsed.ToString());
            lstSteppingCommands.SelectedIndex = lstSteppingCommands.Items.Count - 1;

            if (callBackForm != null)
            {
                callBackForm.SendMessage("Available, Previous Run-Time: " + sw.Elapsed.ToString());
            }


        }

        private void CleanUpMemory()
        {
            //close out app sessions here eventually

            //foreach (var kvp in appInstances)
            //{
            //    try
            //    {
            //        if (kvp.Value is OpenQA.Selenium.Chrome.ChromeDriver)
            //        {
            //            OpenQA.Selenium.Chrome.ChromeDriver driver = (OpenQA.Selenium.Chrome.ChromeDriver)kvp.Value;
            //            driver.Quit();
            //            driver.Dispose();
            //        }
            //    }
            //    catch (Exception)
            //    {
            //        //should we throw an exception?
            //    }

            //}
        }

        public void ReportProgress(string progressToReport)
        {
            //bgwRunScript.ReportProgress(0, new object[] { null, "Command Report: " + progressToReport });
            bgwRunScript.ReportProgress(0, new ProgressUpdate() { LineNumber =  0, UpdateText = "Command Report: " + progressToReport });
        }

        #endregion BackgroundWorker

        #region UI Elements

        private void lblClose_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void lblClose_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void autoCloseTimer_Tick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            if (uiBtnCancel.DisplayText == "Close")
            {
                this.Close();
                return;
            }

            uiBtnPause.Visible = false;
            uiBtnCancel.Visible = false;
            isPaused = false;
            lstSteppingCommands.Items.Add("[User Requested Cancellation]");
            lstSteppingCommands.SelectedIndex = lstSteppingCommands.Items.Count - 1;
            lblMainLogo.Text = "debug info (cancelling)";
            bgwRunScript.CancelAsync();
        }

        private void uiBtnPause_Click(object sender, EventArgs e)
        {
            isPaused = !isPaused;

            if (isPaused)
            {
                lstSteppingCommands.Items.Add("[User Requested Pause]");
                uiBtnPause.Image = Properties.Resources.restart;
                uiBtnPause.DisplayText = "Resume";
            }
            else
            {
                lstSteppingCommands.Items.Add("[User Requested Resume]");
                uiBtnPause.Image = Properties.Resources.pause;
                uiBtnPause.DisplayText = "Pause";
            }

            lstSteppingCommands.SelectedIndex = lstSteppingCommands.Items.Count - 1;
        }

        #endregion UI Elements

        private void frmScriptEngine_FormClosing(object sender, FormClosingEventArgs e)
        {
            //dispose of any unnecessary application instances
            CleanUpMemory();
        }
    }

   public class ProgressUpdate
    {
        public int LineNumber { get; set; }
        public string UpdateText { get; set; }
    }

    public class PauseRequest { }


}