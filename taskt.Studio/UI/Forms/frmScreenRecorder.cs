using System;
using System.Collections.Generic;
using taskt.Commands;
using taskt.Core.Command;
using taskt.Properties;
using taskt.UI.Forms.ScriptBuilder_Forms;
using taskt.Utilities;

namespace taskt.UI.Forms
{
    public partial class frmScreenRecorder : UIForm
    {
        public frmScriptBuilder CallBackForm { get; set; }
        private List<ScriptCommand> _scriptCommandList;

        public frmScreenRecorder()
        {
            InitializeComponent();
        }

        private void OnHookStopped(object sender, EventArgs e)
        {
            //isRecording = false;
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
            _scriptCommandList = GlobalHook.GeneratedCommands;
            var outputList = new List<ScriptCommand>();

            if (chkGroupIntoSequence.Checked)
            {
                var newSequence = new SequenceCommand();

                foreach (ScriptCommand cmd in _scriptCommandList)
                {
                    newSequence.v_scriptActions.Add(cmd);
                }

                if (newSequence.v_scriptActions.Count > 0)
                    outputList.Add(newSequence);
            }
            else if (chkGroupMovesIntoSequences.Checked)
            {
                var newSequence = new SequenceCommand();

                foreach (ScriptCommand cmd in _scriptCommandList)
                {

                    if (cmd is SendMouseMoveCommand)
                    {
                        var sendMouseCmd = (SendMouseMoveCommand)cmd;
                        if (sendMouseCmd.v_MouseClick != "None")
                        {
                            outputList.Add(newSequence);
                            newSequence = new SequenceCommand();
                            outputList.Add(cmd);
                        }
                        else
                        {
                            newSequence.v_scriptActions.Add(cmd);
                        }
                    }
                    else if (cmd is SendKeystrokesCommand)
                    {
                        outputList.Add(newSequence);
                        newSequence = new SequenceCommand();
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
                outputList = _scriptCommandList;
            }

            var commentCommand = new AddCodeCommentCommand();
            commentCommand.v_Comment = "Sequence Recorded " + DateTime.Now.ToString();
            outputList.Insert(0, commentCommand);

            foreach (var cmd in outputList)
            {
                CallBackForm.AddCommandToListView(cmd);
            }
            Close();
        }

        private void btnStartRecording_Click(object sender, EventArgs e)
        {

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
                Height = 150;
                BringToFront();
                MoveFormToBottomRight(this);
                TopMost = true;
                uiBtnRecord.Top = lblRecording.Top + 50;
                pnlOptions.Hide();
                lblRecording.Show();

                int.TryParse(txtHookResolution.Text, out int samplingResolution);

                GlobalHook.HookStopped += new EventHandler(OnHookStopped);
                GlobalHook.StartScreenRecordingHook(chkCaptureClicks.Checked, chkCaptureMouse.Checked,
                    chkGroupMovesIntoSequences.Checked, chkCaptureKeyboard.Checked, chkCaptureWindowEvents.Checked,
                    chkActivateTopLeft.Checked, chkTrackWindowSize.Checked, chkTrackWindowsOpenLocation.Checked,
                    samplingResolution, txtHookStop.Text);
                lblRecording.Text = "Press '" + txtHookStop.Text + "' key to stop recording!";
                // WindowHook.StartHook();

                _scriptCommandList = new List<ScriptCommand>();

                uiBtnRecord.DisplayText = "Stop";
                uiBtnRecord.Image = Resources.various_stop_button;
            }
            else
            {
                GlobalHook.StopHook();
            }
        }

        private void chkActivateTopLeft_CheckedChanged(object sender, EventArgs e)
        {
            if (chkActivateTopLeft.Checked)
                chkTrackWindowsOpenLocation.Checked = false;
        }

        private void chkTrackWindowsOpenLocation_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTrackWindowsOpenLocation.Checked)
                chkActivateTopLeft.Checked = false;
        }
    }
}











