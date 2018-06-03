namespace taskt.UI.Forms
{
    partial class frmSequenceRecorder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSequenceRecorder));
            this.chkGroupMovesIntoSequences = new System.Windows.Forms.CheckBox();
            this.chkCaptureMouse = new System.Windows.Forms.CheckBox();
            this.chkCaptureKeyboard = new System.Windows.Forms.CheckBox();
            this.lblRecording = new System.Windows.Forms.Label();
            this.pnlOptions = new System.Windows.Forms.Panel();
            this.chkTrackWindowSize = new System.Windows.Forms.CheckBox();
            this.chkActivateTopLeft = new System.Windows.Forms.CheckBox();
            this.lblMainLogo = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtHookResolution = new System.Windows.Forms.TextBox();
            this.chkCaptureWindowEvents = new System.Windows.Forms.CheckBox();
            this.chkGroupIntoSequence = new System.Windows.Forms.CheckBox();
            this.chkCaptureClicks = new System.Windows.Forms.CheckBox();
            this.uiBtnRecord = new taskt.UI.CustomControls.UIPictureButton();
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
            this.chkGroupMovesIntoSequences.Location = new System.Drawing.Point(161, 40);
            this.chkGroupMovesIntoSequences.Name = "chkGroupMovesIntoSequences";
            this.chkGroupMovesIntoSequences.Size = new System.Drawing.Size(281, 19);
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
            this.chkCaptureMouse.Location = new System.Drawing.Point(3, 76);
            this.chkCaptureMouse.Name = "chkCaptureMouse";
            this.chkCaptureMouse.Size = new System.Drawing.Size(145, 19);
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
            this.chkCaptureKeyboard.Location = new System.Drawing.Point(3, 95);
            this.chkCaptureKeyboard.Name = "chkCaptureKeyboard";
            this.chkCaptureKeyboard.Size = new System.Drawing.Size(152, 19);
            this.chkCaptureKeyboard.TabIndex = 5;
            this.chkCaptureKeyboard.Text = "Capture Keyboard Input";
            this.chkCaptureKeyboard.UseVisualStyleBackColor = true;
            // 
            // lblRecording
            // 
            this.lblRecording.AutoSize = true;
            this.lblRecording.BackColor = System.Drawing.Color.Transparent;
            this.lblRecording.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecording.ForeColor = System.Drawing.Color.DarkOrange;
            this.lblRecording.Location = new System.Drawing.Point(9, 53);
            this.lblRecording.Name = "lblRecording";
            this.lblRecording.Size = new System.Drawing.Size(505, 45);
            this.lblRecording.TabIndex = 6;
            this.lblRecording.Text = "Press \'END\' Key to stop recording!";
            this.lblRecording.Visible = false;
            // 
            // pnlOptions
            // 
            this.pnlOptions.BackColor = System.Drawing.Color.Transparent;
            this.pnlOptions.Controls.Add(this.chkTrackWindowSize);
            this.pnlOptions.Controls.Add(this.chkActivateTopLeft);
            this.pnlOptions.Controls.Add(this.lblMainLogo);
            this.pnlOptions.Controls.Add(this.label1);
            this.pnlOptions.Controls.Add(this.txtHookResolution);
            this.pnlOptions.Controls.Add(this.chkCaptureWindowEvents);
            this.pnlOptions.Controls.Add(this.chkGroupIntoSequence);
            this.pnlOptions.Controls.Add(this.chkCaptureClicks);
            this.pnlOptions.Controls.Add(this.chkGroupMovesIntoSequences);
            this.pnlOptions.Controls.Add(this.chkCaptureMouse);
            this.pnlOptions.Controls.Add(this.chkCaptureKeyboard);
            this.pnlOptions.Location = new System.Drawing.Point(5, 3);
            this.pnlOptions.Name = "pnlOptions";
            this.pnlOptions.Size = new System.Drawing.Size(498, 142);
            this.pnlOptions.TabIndex = 7;
            // 
            // chkTrackWindowSize
            // 
            this.chkTrackWindowSize.AutoSize = true;
            this.chkTrackWindowSize.Checked = true;
            this.chkTrackWindowSize.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTrackWindowSize.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkTrackWindowSize.Location = new System.Drawing.Point(161, 95);
            this.chkTrackWindowSize.Name = "chkTrackWindowSize";
            this.chkTrackWindowSize.Size = new System.Drawing.Size(182, 19);
            this.chkTrackWindowSize.TabIndex = 11;
            this.chkTrackWindowSize.Text = "Track Activated Window Sizes";
            this.chkTrackWindowSize.UseVisualStyleBackColor = true;
            // 
            // chkActivateTopLeft
            // 
            this.chkActivateTopLeft.AutoSize = true;
            this.chkActivateTopLeft.Checked = true;
            this.chkActivateTopLeft.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkActivateTopLeft.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkActivateTopLeft.Location = new System.Drawing.Point(161, 76);
            this.chkActivateTopLeft.Name = "chkActivateTopLeft";
            this.chkActivateTopLeft.Size = new System.Drawing.Size(153, 19);
            this.chkActivateTopLeft.TabIndex = 10;
            this.chkActivateTopLeft.Text = "Open Windows Top Left";
            this.chkActivateTopLeft.UseVisualStyleBackColor = true;
            // 
            // lblMainLogo
            // 
            this.lblMainLogo.AutoSize = true;
            this.lblMainLogo.BackColor = System.Drawing.Color.Transparent;
            this.lblMainLogo.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMainLogo.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblMainLogo.Location = new System.Drawing.Point(4, 2);
            this.lblMainLogo.Name = "lblMainLogo";
            this.lblMainLogo.Size = new System.Drawing.Size(255, 37);
            this.lblMainLogo.TabIndex = 9;
            this.lblMainLogo.Text = "screen recorder";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 119);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Mouse Move Sampling (ms)";
            // 
            // txtHookResolution
            // 
            this.txtHookResolution.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHookResolution.Location = new System.Drawing.Point(159, 115);
            this.txtHookResolution.Name = "txtHookResolution";
            this.txtHookResolution.Size = new System.Drawing.Size(52, 22);
            this.txtHookResolution.TabIndex = 9;
            this.txtHookResolution.Text = "0";
            // 
            // chkCaptureWindowEvents
            // 
            this.chkCaptureWindowEvents.AutoSize = true;
            this.chkCaptureWindowEvents.Checked = true;
            this.chkCaptureWindowEvents.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCaptureWindowEvents.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCaptureWindowEvents.Location = new System.Drawing.Point(3, 40);
            this.chkCaptureWindowEvents.Name = "chkCaptureWindowEvents";
            this.chkCaptureWindowEvents.Size = new System.Drawing.Size(152, 19);
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
            this.chkGroupIntoSequence.Location = new System.Drawing.Point(161, 58);
            this.chkGroupIntoSequence.Name = "chkGroupIntoSequence";
            this.chkGroupIntoSequence.Size = new System.Drawing.Size(154, 19);
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
            this.chkCaptureClicks.Location = new System.Drawing.Point(3, 58);
            this.chkCaptureClicks.Name = "chkCaptureClicks";
            this.chkCaptureClicks.Size = new System.Drawing.Size(141, 19);
            this.chkCaptureClicks.TabIndex = 6;
            this.chkCaptureClicks.Text = "Capture Mouse Clicks";
            this.chkCaptureClicks.UseVisualStyleBackColor = true;
            // 
            // uiBtnRecord
            // 
            this.uiBtnRecord.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnRecord.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnRecord.DisplayText = "Start";
            this.uiBtnRecord.DisplayTextBrush = System.Drawing.Color.SteelBlue;
            this.uiBtnRecord.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.uiBtnRecord.Image = global::taskt.Properties.Resources.various_record_button;
            this.uiBtnRecord.IsMouseOver = false;
            this.uiBtnRecord.Location = new System.Drawing.Point(6, 144);
            this.uiBtnRecord.Name = "uiBtnRecord";
            this.uiBtnRecord.Size = new System.Drawing.Size(48, 48);
            this.uiBtnRecord.TabIndex = 8;
            this.uiBtnRecord.TabStop = false;
            this.uiBtnRecord.Click += new System.EventHandler(this.uiBtnRecord_Click);
            // 
            // frmSequenceRecorder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundChangeIndex = 1000;
            this.ClientSize = new System.Drawing.Size(559, 192);
            this.Controls.Add(this.uiBtnRecord);
            this.Controls.Add(this.pnlOptions);
            this.Controls.Add(this.lblRecording);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSequenceRecorder";
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
        private CustomControls.UIPictureButton uiBtnRecord;
        private System.Windows.Forms.CheckBox chkCaptureClicks;
        private System.Windows.Forms.CheckBox chkGroupIntoSequence;
        private System.Windows.Forms.CheckBox chkCaptureWindowEvents;
        private System.Windows.Forms.TextBox txtHookResolution;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblMainLogo;
        private System.Windows.Forms.CheckBox chkActivateTopLeft;
        private System.Windows.Forms.CheckBox chkTrackWindowSize;
    }
}