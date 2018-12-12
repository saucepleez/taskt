//Copyright (c) 2018 Jason Bayldon
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
using taskt.Core;
using static taskt.Core.AutomationCommands.User32Functions;

namespace taskt.UI.Forms
{
    public partial class frmScriptEngine : UIForm
    {
        //all variables used by this form
        #region Form Variables
        public Core.EngineSettings engineSettings;
        public string filePath { get; set; }
        public string xmlData { get; set; }
        public frmScriptBuilder callBackForm { get; set; }
        private bool advancedDebug { get; set; }
        private Core.AutomationEngineInstance engineInstance { get; set; }

        #endregion

        //events and methods
        #region Form Events/Methods
        public frmScriptEngine(string pathToFile, frmScriptBuilder builderForm)
        {
            InitializeComponent();

            //set callback form
            callBackForm = builderForm;

            //set file
            this.filePath = pathToFile;

            //get engine settings
            engineSettings = new Core.ApplicationSettings().GetOrCreateApplicationSettings().EngineSettings;

            //determine whether to show listbox or not
            advancedDebug = engineSettings.ShowAdvancedDebugOutput;

            //if listbox should be shown
            if (advancedDebug)
            {
                lstSteppingCommands.Show();
                lblMainLogo.Show();
                pbBotIcon.Hide();
                lblAction.Hide();
            }
            else
            {
                lstSteppingCommands.Hide();
                lblMainLogo.Hide();
                pbBotIcon.Show();
                lblAction.Show();
            }


            //apply debug window setting
            if (!engineSettings.ShowDebugWindow)
            {
                this.Visible = false;
                this.Opacity = 0;
            }

            //add hooks for hot key cancellation
            GlobalHook.HookStopped += new EventHandler(OnHookStopped);
            GlobalHook.StartEngineCancellationHook();



        }
        public frmScriptEngine()
        {
            InitializeComponent();

           
            //set file
            this.filePath = null;

            //get engine settings
            engineSettings = new Core.ApplicationSettings().GetOrCreateApplicationSettings().EngineSettings;

            //determine whether to show listbox or not
            advancedDebug = engineSettings.ShowAdvancedDebugOutput;

            //if listbox should be shown
            if (advancedDebug)
            {
                lstSteppingCommands.Show();
                lblMainLogo.Show();
                pbBotIcon.Hide();
                lblAction.Hide();
            }
            else
            {
                lstSteppingCommands.Hide();
                lblMainLogo.Hide();
                pbBotIcon.Show();
                lblAction.Show();
            }


            //apply debug window setting
            if (!engineSettings.ShowDebugWindow)
            {
                this.Visible = false;
                this.Opacity = 0;
            }

            //add hooks for hot key cancellation
            GlobalHook.HookStopped += new EventHandler(OnHookStopped);
            GlobalHook.StartEngineCancellationHook();

    

        }
        private void frmProcessingStatus_Load(object sender, EventArgs e)
        {
            //move engine form to bottom right and bring to front
            if (engineSettings.ShowDebugWindow)
            {
                this.BringToFront();
                MoveFormToBottomRight(this);
            }

            //start running
            engineInstance = new Core.AutomationEngineInstance();
            engineInstance.ReportProgressEvent += Engine_ReportProgress;
            engineInstance.ScriptFinishedEvent += Engine_ScriptFinishedEvent;
            engineInstance.LineNumberChangedEvent += EngineInstance_LineNumberChangedEvent;

            if (xmlData == null)
            {
                engineInstance.ExecuteScriptAsync(this, filePath);
            }
            else
            {
                engineInstance.ExecuteScriptXML(xmlData);
            }

   
        }

     

        /// <summary>
        /// Triggers the automation engine to stop based on a hooked key press
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnHookStopped(object sender, EventArgs e)
        {
            uiBtnCancel_Click(null, null);
            GlobalHook.HookStopped -= new EventHandler(OnHookStopped);
            engineInstance.CancelScript();

        }
      
        #endregion

        //engine event handlers
        #region Engine Event Handlers
        /// <summary>
        /// Handles Progress Updates raised by Automation Engine
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Engine_ReportProgress(object sender, ReportProgressEventArgs e)
        {
            AddStatus(e.ProgressUpdate);
        }
        /// <summary>
        /// Handles Script Finished Event raised by Automation Engine
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Engine_ScriptFinishedEvent(object sender, ScriptFinishedEventArgs e)
        {

            switch (e.Result)
            {
                case ScriptFinishedEventArgs.ScriptFinishedResult.Successful:
                    AddStatus("Script Completed Successfully");
                    UpdateUI("debug info (success)");
                
                    break;
                case ScriptFinishedEventArgs.ScriptFinishedResult.Error:
                    AddStatus("Error: " + e.Error);
                    AddStatus("Script Completed With Errors!");
                    UpdateUI("debug info (error)");

                    if (!advancedDebug)
                    {
                        pbBotIcon.Image = Properties.Resources.robot_error;
                    }

                    break;
                case ScriptFinishedEventArgs.ScriptFinishedResult.Cancelled:
                    AddStatus("Script Cancelled By User");
                    UpdateUI("debug info (cancelled)");
                    break;
                default:
                    break;
            }


       
            AddStatus("Total Execution Time: " + e.ExecutionTime.ToString());

        }

        private void EngineInstance_LineNumberChangedEvent(object sender, LineNumberChangedEventArgs e)
        {
            UpdateLineNumber(e.CurrentLineNumber);
        }
        #endregion

        //delegates to marshal changes to UI
        #region Engine Delegates
        /// <summary>
        /// Delegate for adding progress reports
        /// </summary>
        /// <param name="message">The progress report string from Automation Engine</param>
        public delegate void AddStatusDelegate(string message);
        /// <summary>
        /// Adds a status to the listbox for debugging and display purposes
        /// </summary>
        /// <param name="text"></param>
        private void AddStatus(string text)
        {
            if (InvokeRequired)
            {
                var d = new AddStatusDelegate(AddStatus);
                Invoke(d, new object[] { text });
            }
            else
            {
               //update status
                lblAction.Text = text + "..";
                lstSteppingCommands.Items.Add(DateTime.Now.ToString("MM/dd/yy hh:mm:ss.fff") + " | " + text + "..");
                lstSteppingCommands.SelectedIndex = lstSteppingCommands.Items.Count - 1;

           
            }

        }
        /// <summary>
        /// Delegate for updating UI after Automation Engine finishes
        /// </summary>
        /// <param name="message"></param>
        public delegate void UpdateUIDelegate(string message);
        /// <summary>
        /// Standard UI updates after automation is finished running
        /// </summary>
        /// <param name="mainLogoText"></param>
        private void UpdateUI(string mainLogoText)
        {

            if (InvokeRequired)
            {
                var d = new UpdateUIDelegate(UpdateUI);
                Invoke(d, new object[] { mainLogoText });
            }
            else
            {

                //set main logo text
                lblMainLogo.Text = mainLogoText;

                //hide and change buttons not required
                uiBtnPause.Visible = false;
                uiBtnCancel.DisplayText = "Close";
                uiBtnCancel.Visible = true;

                //reset debug line
                if (callBackForm != null)
                    callBackForm.DebugLine = 0;

                //begin auto close
                if (engineSettings.AutoCloseDebugWindow)
                    tmrNotify.Enabled = true;

            }

        }
        /// <summary>
        /// Delegate for showing message box
        /// </summary>
        /// <param name="message"></param>
        public delegate void ShowMessageDelegate(string message, string title, UI.Forms.Supplemental.frmDialog.DialogType dialogType, int closeAfter);
        /// <summary>
        /// Used by the automation engine to show a message to the user on-screen. If UI is not available, a standard messagebox will be invoked instead.
        /// </summary>
        public void ShowMessage(string message, string title, UI.Forms.Supplemental.frmDialog.DialogType dialogType, int closeAfter)
        {
            if (InvokeRequired)
            {
                var d = new ShowMessageDelegate(ShowMessage);
                Invoke(d, new object[] { message, title, dialogType, closeAfter });
            }
            else
            {
                var confirmationForm = new UI.Forms.Supplemental.frmDialog(message, title, dialogType, closeAfter);
                confirmationForm.ShowDialog();
            }
         
        }

        public delegate List<string> ShowInputDelegate(Core.AutomationCommands.UserInputCommand inputs);
        public List<string> ShowInput(Core.AutomationCommands.UserInputCommand inputs)
        {
            if (InvokeRequired)
            {
                var d = new ShowInputDelegate(ShowInput);
                Invoke(d, new object[] { inputs });
                return null;
            }
            else
            {
                var inputForm = new Supplemental.frmUserInput();
                inputForm.InputCommand = inputs;


                var dialogResult = inputForm.ShowDialog();

                if (dialogResult == DialogResult.OK)
                {

                    var responses = new List<string>();
                    foreach (var ctrl in inputForm.InputControls)
                    {
                        if (ctrl is CheckBox)
                        {
                            var checkboxCtrl = (CheckBox)ctrl;
                            responses.Add(checkboxCtrl.Checked.ToString());
                        }
                        else
                        {
                            responses.Add(ctrl.Text);
                        }


                    }

                    return responses;
                }
                else
                {
                    return null;
                }
        

           
            }

        }
        public delegate List<Core.Script.ScriptVariable> ShowHTMLInputDelegate(string htmlTemplate);
        public List<Core.Script.ScriptVariable> ShowHTMLInput(string htmlTemplate)
        {
            if (InvokeRequired)
            {
                var d = new ShowHTMLInputDelegate(ShowHTMLInput);
                Invoke(d, new object[] { htmlTemplate });
                return null;
            }
            else
            {
                var inputForm = new Supplemental.frmHTMLDisplayForm();
                inputForm.TemplateHTML = htmlTemplate;


                var dialogResult = inputForm.ShowDialog();

                if (inputForm.Result == DialogResult.OK)
                {
                    var variables = inputForm.GetVariablesFromHTML("input");

                    variables.AddRange(inputForm.GetVariablesFromHTML("select"));

                    return variables;
                }
                else
                {
                    return null;
                }



            }

        }

        public delegate void SetLineNumber(int lineNumber);
        public void UpdateLineNumber(int lineNumber)
        {
            if (InvokeRequired)
            {
                var d = new SetLineNumber(UpdateLineNumber);
                Invoke(d, new object[] { lineNumber });
            }
            else
            {
                if (callBackForm != null)
                {
                    callBackForm.DebugLine = lineNumber;
                }
                        
            }
        }
        #endregion

        //various small UI methods
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
            lblKillProcNote.Text = "Cancelling...";
            engineInstance.ResumeScript();
            lstSteppingCommands.Items.Add("[User Requested Cancellation]");
            lstSteppingCommands.SelectedIndex = lstSteppingCommands.Items.Count - 1;
            lblMainLogo.Text = "debug info (cancelling)";
            engineInstance.CancelScript();
        }

        private void uiBtnPause_Click(object sender, EventArgs e)
        {
           

            if (uiBtnPause.DisplayText == "Pause")
            {
                lstSteppingCommands.Items.Add("[User Requested Pause]");
                uiBtnPause.Image = Properties.Resources.action_bar_run;
                uiBtnPause.DisplayText = "Resume";
                engineInstance.PauseScript();
            }
            else
            {
                lstSteppingCommands.Items.Add("[User Requested Resume]");
                uiBtnPause.Image = Properties.Resources.command_pause;
                uiBtnPause.DisplayText = "Pause";
                engineInstance.ResumeScript();
            }

            lstSteppingCommands.SelectedIndex = lstSteppingCommands.Items.Count - 1;
        }

        private void pbBotIcon_Click(object sender, EventArgs e)
        {
            //show debug if user clicks
            lstSteppingCommands.Visible = !lstSteppingCommands.Visible;
        }
        private void lstSteppingCommands_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show(lstSteppingCommands.SelectedItem.ToString(), "Item Status");
        }
        #endregion UI Elements


    }



}