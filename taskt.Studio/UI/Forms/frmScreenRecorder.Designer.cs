namespace taskt.UI.Forms
{
    partial class frmScreenRecorder
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmScreenRecorder));
            this.chkGroupMovesIntoSequences = new System.Windows.Forms.CheckBox();
            this.chkCaptureMouse = new System.Windows.Forms.CheckBox();
            this.chkCaptureKeyboard = new System.Windows.Forms.CheckBox();
            this.lblRecording = new System.Windows.Forms.Label();
            this.pnlOptions = new System.Windows.Forms.Panel();
            this.lblStopCaptureHotKey = new System.Windows.Forms.Label();
            this.txtHookStop = new System.Windows.Forms.TextBox();
            this.chkTrackWindowsOpenLocation = new System.Windows.Forms.CheckBox();
            this.lblOtherOptions = new System.Windows.Forms.Label();
            this.lblCommandGroupingOptions = new System.Windows.Forms.Label();
            this.lblCaptureOptions = new System.Windows.Forms.Label();
            this.lblScreenRecorder = new System.Windows.Forms.Label();
            this.chkTrackWindowSize = new System.Windows.Forms.CheckBox();
            this.chkActivateTopLeft = new System.Windows.Forms.CheckBox();
            this.lblMouseMoveSampling = new System.Windows.Forms.Label();
            this.txtHookResolution = new System.Windows.Forms.TextBox();
            this.chkCaptureWindowEvents = new System.Windows.Forms.CheckBox();
            this.chkGroupIntoSequence = new System.Windows.Forms.CheckBox();
            this.chkCaptureClicks = new System.Windows.Forms.CheckBox();
            this.uiBtnRecord = new taskt.UI.CustomControls.CustomUIControls.UIPictureButton();
            this.pnlOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnRecord)).BeginInit();
            this.SuspendLayout();
            // 
            // chkGroupMovesIntoSequences
            // 
            this.chkGroupMovesIntoSequences.AutoSize = true;
            this.chkGroupMovesIntoSequences.Checked = true;
            this.chkGroupMovesIntoSequences.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGroupMovesIntoSequences.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkGroupMovesIntoSequences.ForeColor = System.Drawing.Color.AliceBlue;
            this.chkGroupMovesIntoSequences.Location = new System.Drawing.Point(16, 337);
            this.chkGroupMovesIntoSequences.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.chkGroupMovesIntoSequences.Name = "chkGroupMovesIntoSequences";
            this.chkGroupMovesIntoSequences.Size = new System.Drawing.Size(348, 24);
            this.chkGroupMovesIntoSequences.TabIndex = 2;
            this.chkGroupMovesIntoSequences.Text = "Group Mouse Moves as Sequences (Less Clutter)";
            this.chkGroupMovesIntoSequences.UseVisualStyleBackColor = true;
            this.chkGroupMovesIntoSequences.CheckedChanged += new System.EventHandler(this.chkGroupIntoSequences_CheckedChanged);
            // 
            // chkCaptureMouse
            // 
            this.chkCaptureMouse.AutoSize = true;
            this.chkCaptureMouse.Checked = true;
            this.chkCaptureMouse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCaptureMouse.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCaptureMouse.ForeColor = System.Drawing.Color.AliceBlue;
            this.chkCaptureMouse.Location = new System.Drawing.Point(20, 151);
            this.chkCaptureMouse.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.chkCaptureMouse.Name = "chkCaptureMouse";
            this.chkCaptureMouse.Size = new System.Drawing.Size(178, 24);
            this.chkCaptureMouse.TabIndex = 4;
            this.chkCaptureMouse.Text = "Capture Mouse Moves";
            this.chkCaptureMouse.UseVisualStyleBackColor = true;
            this.chkCaptureMouse.CheckedChanged += new System.EventHandler(this.chkCaptureMouse_CheckedChanged);
            // 
            // chkCaptureKeyboard
            // 
            this.chkCaptureKeyboard.AutoSize = true;
            this.chkCaptureKeyboard.Checked = true;
            this.chkCaptureKeyboard.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCaptureKeyboard.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCaptureKeyboard.ForeColor = System.Drawing.Color.AliceBlue;
            this.chkCaptureKeyboard.Location = new System.Drawing.Point(20, 180);
            this.chkCaptureKeyboard.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.chkCaptureKeyboard.Name = "chkCaptureKeyboard";
            this.chkCaptureKeyboard.Size = new System.Drawing.Size(189, 24);
            this.chkCaptureKeyboard.TabIndex = 5;
            this.chkCaptureKeyboard.Text = "Capture Keyboard Input";
            this.chkCaptureKeyboard.UseVisualStyleBackColor = true;
            // 
            // lblRecording
            // 
            this.lblRecording.AutoSize = true;
            this.lblRecording.BackColor = System.Drawing.Color.Transparent;
            this.lblRecording.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecording.ForeColor = System.Drawing.Color.DarkOrange;
            this.lblRecording.Location = new System.Drawing.Point(21, 14);
            this.lblRecording.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblRecording.Name = "lblRecording";
            this.lblRecording.Size = new System.Drawing.Size(500, 46);
            this.lblRecording.TabIndex = 6;
            this.lblRecording.Text = "Press \'F2\' Key to stop recording!";
            this.lblRecording.Visible = false;
            // 
            // pnlOptions
            // 
            this.pnlOptions.BackColor = System.Drawing.Color.Transparent;
            this.pnlOptions.Controls.Add(this.lblStopCaptureHotKey);
            this.pnlOptions.Controls.Add(this.txtHookStop);
            this.pnlOptions.Controls.Add(this.chkTrackWindowsOpenLocation);
            this.pnlOptions.Controls.Add(this.lblOtherOptions);
            this.pnlOptions.Controls.Add(this.lblCommandGroupingOptions);
            this.pnlOptions.Controls.Add(this.lblCaptureOptions);
            this.pnlOptions.Controls.Add(this.lblScreenRecorder);
            this.pnlOptions.Controls.Add(this.chkTrackWindowSize);
            this.pnlOptions.Controls.Add(this.chkActivateTopLeft);
            this.pnlOptions.Controls.Add(this.lblMouseMoveSampling);
            this.pnlOptions.Controls.Add(this.txtHookResolution);
            this.pnlOptions.Controls.Add(this.chkCaptureWindowEvents);
            this.pnlOptions.Controls.Add(this.chkGroupIntoSequence);
            this.pnlOptions.Controls.Add(this.chkCaptureClicks);
            this.pnlOptions.Controls.Add(this.chkGroupMovesIntoSequences);
            this.pnlOptions.Controls.Add(this.chkCaptureMouse);
            this.pnlOptions.Controls.Add(this.chkCaptureKeyboard);
            this.pnlOptions.Location = new System.Drawing.Point(23, 9);
            this.pnlOptions.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.pnlOptions.Name = "pnlOptions";
            this.pnlOptions.Size = new System.Drawing.Size(675, 495);
            this.pnlOptions.TabIndex = 7;
            // 
            // lblStopCaptureHotKey
            // 
            this.lblStopCaptureHotKey.AutoSize = true;
            this.lblStopCaptureHotKey.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStopCaptureHotKey.ForeColor = System.Drawing.Color.AliceBlue;
            this.lblStopCaptureHotKey.Location = new System.Drawing.Point(17, 256);
            this.lblStopCaptureHotKey.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblStopCaptureHotKey.Name = "lblStopCaptureHotKey";
            this.lblStopCaptureHotKey.Size = new System.Drawing.Size(139, 19);
            this.lblStopCaptureHotKey.TabIndex = 18;
            this.lblStopCaptureHotKey.Text = "Stop Capture HotKey";
            // 
            // txtHookStop
            // 
            this.txtHookStop.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHookStop.Location = new System.Drawing.Point(284, 251);
            this.txtHookStop.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.txtHookStop.Name = "txtHookStop";
            this.txtHookStop.Size = new System.Drawing.Size(89, 26);
            this.txtHookStop.TabIndex = 17;
            this.txtHookStop.Text = "F2";
            // 
            // chkTrackWindowsOpenLocation
            // 
            this.chkTrackWindowsOpenLocation.AutoSize = true;
            this.chkTrackWindowsOpenLocation.Checked = true;
            this.chkTrackWindowsOpenLocation.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTrackWindowsOpenLocation.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkTrackWindowsOpenLocation.ForeColor = System.Drawing.Color.White;
            this.chkTrackWindowsOpenLocation.Location = new System.Drawing.Point(16, 431);
            this.chkTrackWindowsOpenLocation.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.chkTrackWindowsOpenLocation.Name = "chkTrackWindowsOpenLocation";
            this.chkTrackWindowsOpenLocation.Size = new System.Drawing.Size(247, 24);
            this.chkTrackWindowsOpenLocation.TabIndex = 16;
            this.chkTrackWindowsOpenLocation.Text = "Track Activated Window Position";
            this.chkTrackWindowsOpenLocation.UseVisualStyleBackColor = true;
            this.chkTrackWindowsOpenLocation.CheckedChanged += new System.EventHandler(this.chkTrackWindowsOpenLocation_CheckedChanged);
            // 
            // lblOtherOptions
            // 
            this.lblOtherOptions.AutoSize = true;
            this.lblOtherOptions.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOtherOptions.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.lblOtherOptions.Location = new System.Drawing.Point(11, 400);
            this.lblOtherOptions.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblOtherOptions.Name = "lblOtherOptions";
            this.lblOtherOptions.Size = new System.Drawing.Size(137, 28);
            this.lblOtherOptions.TabIndex = 15;
            this.lblOtherOptions.Text = "Other Options";
            // 
            // lblCommandGroupingOptions
            // 
            this.lblCommandGroupingOptions.AutoSize = true;
            this.lblCommandGroupingOptions.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCommandGroupingOptions.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.lblCommandGroupingOptions.Location = new System.Drawing.Point(7, 302);
            this.lblCommandGroupingOptions.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblCommandGroupingOptions.Name = "lblCommandGroupingOptions";
            this.lblCommandGroupingOptions.Size = new System.Drawing.Size(267, 28);
            this.lblCommandGroupingOptions.TabIndex = 14;
            this.lblCommandGroupingOptions.Text = "Command Grouping Options";
            // 
            // lblCaptureOptions
            // 
            this.lblCaptureOptions.AutoSize = true;
            this.lblCaptureOptions.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCaptureOptions.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.lblCaptureOptions.Location = new System.Drawing.Point(11, 60);
            this.lblCaptureOptions.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblCaptureOptions.Name = "lblCaptureOptions";
            this.lblCaptureOptions.Size = new System.Drawing.Size(156, 28);
            this.lblCaptureOptions.TabIndex = 13;
            this.lblCaptureOptions.Text = "Capture Options";
            // 
            // lblScreenRecorder
            // 
            this.lblScreenRecorder.AutoSize = true;
            this.lblScreenRecorder.BackColor = System.Drawing.Color.Transparent;
            this.lblScreenRecorder.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScreenRecorder.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblScreenRecorder.Location = new System.Drawing.Point(7, 9);
            this.lblScreenRecorder.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblScreenRecorder.Name = "lblScreenRecorder";
            this.lblScreenRecorder.Size = new System.Drawing.Size(315, 46);
            this.lblScreenRecorder.TabIndex = 12;
            this.lblScreenRecorder.Text = "screen recorder";
            // 
            // chkTrackWindowSize
            // 
            this.chkTrackWindowSize.AutoSize = true;
            this.chkTrackWindowSize.Checked = true;
            this.chkTrackWindowSize.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTrackWindowSize.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkTrackWindowSize.ForeColor = System.Drawing.Color.White;
            this.chkTrackWindowSize.Location = new System.Drawing.Point(16, 491);
            this.chkTrackWindowSize.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.chkTrackWindowSize.Name = "chkTrackWindowSize";
            this.chkTrackWindowSize.Size = new System.Drawing.Size(228, 24);
            this.chkTrackWindowSize.TabIndex = 11;
            this.chkTrackWindowSize.Text = "Track Activated Window Sizes";
            this.chkTrackWindowSize.UseVisualStyleBackColor = true;
            // 
            // chkActivateTopLeft
            // 
            this.chkActivateTopLeft.AutoSize = true;
            this.chkActivateTopLeft.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkActivateTopLeft.ForeColor = System.Drawing.Color.White;
            this.chkActivateTopLeft.Location = new System.Drawing.Point(16, 460);
            this.chkActivateTopLeft.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.chkActivateTopLeft.Name = "chkActivateTopLeft";
            this.chkActivateTopLeft.Size = new System.Drawing.Size(257, 24);
            this.chkActivateTopLeft.TabIndex = 10;
            this.chkActivateTopLeft.Text = "Open Activated Windows Top Left";
            this.chkActivateTopLeft.UseVisualStyleBackColor = true;
            this.chkActivateTopLeft.CheckedChanged += new System.EventHandler(this.chkActivateTopLeft_CheckedChanged);
            // 
            // lblMouseMoveSampling
            // 
            this.lblMouseMoveSampling.AutoSize = true;
            this.lblMouseMoveSampling.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMouseMoveSampling.ForeColor = System.Drawing.Color.AliceBlue;
            this.lblMouseMoveSampling.Location = new System.Drawing.Point(16, 214);
            this.lblMouseMoveSampling.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblMouseMoveSampling.Name = "lblMouseMoveSampling";
            this.lblMouseMoveSampling.Size = new System.Drawing.Size(180, 19);
            this.lblMouseMoveSampling.TabIndex = 9;
            this.lblMouseMoveSampling.Text = "Mouse Move Sampling (ms)";
            // 
            // txtHookResolution
            // 
            this.txtHookResolution.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHookResolution.Location = new System.Drawing.Point(284, 209);
            this.txtHookResolution.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.txtHookResolution.Name = "txtHookResolution";
            this.txtHookResolution.Size = new System.Drawing.Size(68, 26);
            this.txtHookResolution.TabIndex = 9;
            this.txtHookResolution.Text = "0";
            // 
            // chkCaptureWindowEvents
            // 
            this.chkCaptureWindowEvents.AutoSize = true;
            this.chkCaptureWindowEvents.Checked = true;
            this.chkCaptureWindowEvents.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCaptureWindowEvents.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCaptureWindowEvents.ForeColor = System.Drawing.Color.AliceBlue;
            this.chkCaptureWindowEvents.Location = new System.Drawing.Point(20, 97);
            this.chkCaptureWindowEvents.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.chkCaptureWindowEvents.Name = "chkCaptureWindowEvents";
            this.chkCaptureWindowEvents.Size = new System.Drawing.Size(188, 24);
            this.chkCaptureWindowEvents.TabIndex = 8;
            this.chkCaptureWindowEvents.Text = "Capture Window Events";
            this.chkCaptureWindowEvents.UseVisualStyleBackColor = true;
            // 
            // chkGroupIntoSequence
            // 
            this.chkGroupIntoSequence.AutoSize = true;
            this.chkGroupIntoSequence.Checked = true;
            this.chkGroupIntoSequence.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGroupIntoSequence.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkGroupIntoSequence.ForeColor = System.Drawing.Color.AliceBlue;
            this.chkGroupIntoSequence.Location = new System.Drawing.Point(16, 366);
            this.chkGroupIntoSequence.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.chkGroupIntoSequence.Name = "chkGroupIntoSequence";
            this.chkGroupIntoSequence.Size = new System.Drawing.Size(192, 24);
            this.chkGroupIntoSequence.TabIndex = 7;
            this.chkGroupIntoSequence.Text = "Group All Into Sequence";
            this.chkGroupIntoSequence.UseVisualStyleBackColor = true;
            // 
            // chkCaptureClicks
            // 
            this.chkCaptureClicks.AutoSize = true;
            this.chkCaptureClicks.Checked = true;
            this.chkCaptureClicks.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCaptureClicks.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCaptureClicks.ForeColor = System.Drawing.Color.AliceBlue;
            this.chkCaptureClicks.Location = new System.Drawing.Point(20, 124);
            this.chkCaptureClicks.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.chkCaptureClicks.Name = "chkCaptureClicks";
            this.chkCaptureClicks.Size = new System.Drawing.Size(172, 24);
            this.chkCaptureClicks.TabIndex = 6;
            this.chkCaptureClicks.Text = "Capture Mouse Clicks";
            this.chkCaptureClicks.UseVisualStyleBackColor = true;
            // 
            // uiBtnRecord
            // 
            this.uiBtnRecord.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnRecord.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnRecord.DisplayText = "Start";
            this.uiBtnRecord.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnRecord.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.uiBtnRecord.Image = global::taskt.Properties.Resources.various_record_button;
            this.uiBtnRecord.IsMouseOver = false;
            this.uiBtnRecord.Location = new System.Drawing.Point(20, 546);
            this.uiBtnRecord.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.uiBtnRecord.Name = "uiBtnRecord";
            this.uiBtnRecord.Size = new System.Drawing.Size(64, 59);
            this.uiBtnRecord.TabIndex = 8;
            this.uiBtnRecord.TabStop = false;
            this.uiBtnRecord.Text = "Start";
            this.uiBtnRecord.Click += new System.EventHandler(this.uiBtnRecord_Click);
            // 
            // frmScreenRecorder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundChangeIndex = 1000;
            this.ClientSize = new System.Drawing.Size(741, 518);
            this.Controls.Add(this.uiBtnRecord);
            this.Controls.Add(this.pnlOptions);
            this.Controls.Add(this.lblRecording);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.Name = "frmScreenRecorder";
            this.Text = "Screen Recorder";
            this.Load += new System.EventHandler(this.frmSequenceRecorder_Load);
            this.pnlOptions.ResumeLayout(false);
            this.pnlOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnRecord)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox chkGroupMovesIntoSequences;
        private System.Windows.Forms.CheckBox chkCaptureMouse;
        private System.Windows.Forms.CheckBox chkCaptureKeyboard;
        private System.Windows.Forms.Label lblRecording;
        private System.Windows.Forms.Panel pnlOptions;
        private CustomControls.CustomUIControls.UIPictureButton uiBtnRecord;
        private System.Windows.Forms.CheckBox chkCaptureClicks;
        private System.Windows.Forms.CheckBox chkGroupIntoSequence;
        private System.Windows.Forms.CheckBox chkCaptureWindowEvents;
        private System.Windows.Forms.TextBox txtHookResolution;
        private System.Windows.Forms.Label lblMouseMoveSampling;
        private System.Windows.Forms.CheckBox chkActivateTopLeft;
        private System.Windows.Forms.CheckBox chkTrackWindowSize;
        private System.Windows.Forms.Label lblScreenRecorder;
        private System.Windows.Forms.Label lblOtherOptions;
        private System.Windows.Forms.Label lblCommandGroupingOptions;
        private System.Windows.Forms.Label lblCaptureOptions;
        private System.Windows.Forms.CheckBox chkTrackWindowsOpenLocation;
        private System.Windows.Forms.Label lblStopCaptureHotKey;
        private System.Windows.Forms.TextBox txtHookStop;
    }
}