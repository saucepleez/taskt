using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using taskt.Core.AutomationCommands;
using static taskt.Core.AutomationCommands.User32Functions;

namespace taskt.UI.Forms
{
    public partial class frmSequenceRecorder : UIForm
    {

        public static List<Core.AutomationCommands.ScriptCommand> commandList;
        public frmScriptBuilder callBackForm { get; set; }
        private bool isRecording { get; set; }
        public frmSequenceRecorder()
        {
            InitializeComponent();
        }

        private void btnStartRecording_Click(object sender, EventArgs e)
        {



        }
        void OnHookStopped(object sender, EventArgs e)
        {

          //  isRecording = false;

            GlobalHook.HookStopped -= new EventHandler(OnHookStopped);

            //if (!isRecording)
            //{
            //    return;
            //}


            pnlOptions.Show();
            lblRecording.Hide();

            FinalizeRecording();
        }

        private void FinalizeRecording()
        {

            var commandList = GlobalHook.generatedCommands;


            var outputList = new List<Core.AutomationCommands.ScriptCommand>();


            if (chkGroupIntoSequence.Checked)
            {
                var newSequence = new Core.AutomationCommands.SequenceCommand();

                foreach (Core.AutomationCommands.ScriptCommand cmd in commandList)
                {
                    newSequence.v_scriptActions.Add(cmd);
                }


                if (newSequence.v_scriptActions.Count > 0)
                    outputList.Add(newSequence);


            }
            else if (chkGroupMovesIntoSequences.Checked)
            {
                var newSequence = new Core.AutomationCommands.SequenceCommand();

                foreach (Core.AutomationCommands.ScriptCommand cmd in commandList)
                {

                    if (cmd is Core.AutomationCommands.SendMouseMoveCommand)
                    {
                        var sendMouseCmd = (Core.AutomationCommands.SendMouseMoveCommand)cmd;
                        if (sendMouseCmd.v_MouseClick != "None")
                        {
                            outputList.Add(newSequence);
                            newSequence = new Core.AutomationCommands.SequenceCommand();
                            outputList.Add(cmd);
                        }
                        else
                        {
                            newSequence.v_scriptActions.Add(cmd);
                        }
                    }
                    else if (cmd is SendKeysCommand)
                    {
                        outputList.Add(newSequence);
                        newSequence = new Core.AutomationCommands.SequenceCommand();
                        outputList.Add(cmd);
                    }
                    else
                    {
                        newSequence.v_scriptActions.Add(cmd);
                    }


                }

                if (newSequence.v_scriptActions.Count > 0)
                    outputList.Add(newSequence);


            }

            else
            {
                outputList = commandList;
            }






            var commentCommand = new Core.AutomationCommands.CommentCommand();
            commentCommand.v_Comment = "Sequence Recorded " + DateTime.Now.ToString();
            outputList.Insert(0, commentCommand);

            foreach (var cmd in outputList)
            {
                callBackForm.AddCommandToListView(cmd);
            }

            this.Close();
        }

        private void btnStopRecording_Click(object sender, EventArgs e)
        {



            GlobalHook.StopHook();
            //FinalizeRecording();


        }

        private void frmSequenceRecorder_Load(object sender, EventArgs e)
        {
           
        }

        private void chkGroupIntoSequences_CheckedChanged(object sender, EventArgs e)
        {


        }

        private void chkCaptureMouse_CheckedChanged(object sender, EventArgs e)
        {
            chkGroupMovesIntoSequences.Checked = chkCaptureMouse.Checked;
            chkGroupMovesIntoSequences.Enabled = chkGroupMovesIntoSequences.Checked;
        }

        private void uiBtnRecord_Click(object sender, EventArgs e)
        {
            if (uiBtnRecord.DisplayText == "Start")
            {
                this.Height = 150;
                this.BringToFront();
                MoveFormToBottomRight(this);
                this.TopMost = true;
                uiBtnRecord.Top = lblRecording.Top + 50;

                pnlOptions.Hide();

                lblRecording.Show();


                int samplingResolution;
                int.TryParse(txtHookResolution.Text, out samplingResolution);


                GlobalHook.HookStopped += new EventHandler(OnHookStopped);
                GlobalHook.StartScreenRecordingHook(chkCaptureClicks.Checked, chkCaptureMouse.Checked, chkGroupMovesIntoSequences.Checked, chkCaptureKeyboard.Checked, chkCaptureWindowEvents.Checked, chkActivateTopLeft.Checked, chkTrackWindowSize.Checked, samplingResolution);

               // WindowHook.StartHook();



                commandList = new List<Core.AutomationCommands.ScriptCommand>();


                uiBtnRecord.DisplayText = "Stop";
                uiBtnRecord.Image = taskt.Properties.Resources.various_stop_button;

            }
            else
            {
                GlobalHook.StopHook();
            }
        }


    }
}


    


    





