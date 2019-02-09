namespace taskt.UI.Forms
{
    partial class frmScriptEngine
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmScriptEngine));
            this.lstSteppingCommands = new System.Windows.Forms.ListBox();
            this.tmrNotify = new System.Windows.Forms.Timer(this.components);
            this.lblCloseTimer = new System.Windows.Forms.Label();
            this.lblMainLogo = new System.Windows.Forms.Label();
            this.lblKillProcNote = new System.Windows.Forms.Label();
            this.lblAction = new System.Windows.Forms.Label();
            this.uiBtnCancel = new taskt.UI.CustomControls.UIPictureButton();
            this.uiBtnPause = new taskt.UI.CustomControls.UIPictureButton();
            this.pbBotIcon = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnPause)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBotIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // lstSteppingCommands
            // 
            this.lstSteppingCommands.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstSteppingCommands.ForeColor = System.Drawing.Color.SteelBlue;
            this.lstSteppingCommands.FormattingEnabled = true;
            this.lstSteppingCommands.ItemHeight = 17;
            this.lstSteppingCommands.Location = new System.Drawing.Point(7, 56);
            this.lstSteppingCommands.Name = "lstSteppingCommands";
            this.lstSteppingCommands.Size = new System.Drawing.Size(675, 140);
            this.lstSteppingCommands.TabIndex = 0;
            this.lstSteppingCommands.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstSteppingCommands_MouseDoubleClick);
            // 
            // tmrNotify
            // 
            this.tmrNotify.Interval = 5000;
            this.tmrNotify.Tick += new System.EventHandler(this.autoCloseTimer_Tick);
            // 
            // lblCloseTimer
            // 
            this.lblCloseTimer.AutoSize = true;
            this.lblCloseTimer.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCloseTimer.ForeColor = System.Drawing.Color.White;
            this.lblCloseTimer.Location = new System.Drawing.Point(12, 176);
            this.lblCloseTimer.Name = "lblCloseTimer";
            this.lblCloseTimer.Size = new System.Drawing.Size(0, 17);
            this.lblCloseTimer.TabIndex = 3;
            // 
            // lblMainLogo
            // 
            this.lblMainLogo.AutoSize = true;
            this.lblMainLogo.BackColor = System.Drawing.Color.Transparent;
            this.lblMainLogo.Font = new System.Drawing.Font("Segoe UI Semilight", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMainLogo.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblMainLogo.Location = new System.Drawing.Point(1, 5);
            this.lblMainLogo.Name = "lblMainLogo";
            this.lblMainLogo.Size = new System.Drawing.Size(253, 45);
            this.lblMainLogo.TabIndex = 4;
            this.lblMainLogo.Text = "taskt is executing";
            // 
            // lblKillProcNote
            // 
            this.lblKillProcNote.AutoSize = true;
            this.lblKillProcNote.BackColor = System.Drawing.Color.Transparent;
            this.lblKillProcNote.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKillProcNote.ForeColor = System.Drawing.Color.White;
            this.lblKillProcNote.Location = new System.Drawing.Point(4, 196);
            this.lblKillProcNote.Name = "lblKillProcNote";
            this.lblKillProcNote.Size = new System.Drawing.Size(333, 20);
            this.lblKillProcNote.TabIndex = 17;
            this.lblKillProcNote.Text = "Press the \'Pause/Break\' key to stop automation.";
            // 
            // lblAction
            // 
            this.lblAction.AutoSize = true;
            this.lblAction.BackColor = System.Drawing.Color.Transparent;
            this.lblAction.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAction.ForeColor = System.Drawing.Color.White;
            this.lblAction.Location = new System.Drawing.Point(5, 219);
            this.lblAction.Name = "lblAction";
            this.lblAction.Size = new System.Drawing.Size(65, 20);
            this.lblAction.TabIndex = 19;
            this.lblAction.Text = "Action...";
            this.lblAction.Visible = false;
            // 
            // uiBtnCancel
            // 
            this.uiBtnCancel.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnCancel.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnCancel.DisplayText = "Cancel";
            this.uiBtnCancel.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnCancel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnCancel.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnCancel.Image")));
            this.uiBtnCancel.IsMouseOver = false;
            this.uiBtnCancel.Location = new System.Drawing.Point(633, 2);
            this.uiBtnCancel.Name = "uiBtnCancel";
            this.uiBtnCancel.Size = new System.Drawing.Size(48, 48);
            this.uiBtnCancel.TabIndex = 14;
            this.uiBtnCancel.TabStop = false;
            this.uiBtnCancel.Click += new System.EventHandler(this.uiBtnCancel_Click);
            // 
            // uiBtnPause
            // 
            this.uiBtnPause.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnPause.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnPause.DisplayText = "Pause";
            this.uiBtnPause.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnPause.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnPause.Image = global::taskt.Properties.Resources.command_pause;
            this.uiBtnPause.IsMouseOver = false;
            this.uiBtnPause.Location = new System.Drawing.Point(579, 2);
            this.uiBtnPause.Name = "uiBtnPause";
            this.uiBtnPause.Size = new System.Drawing.Size(48, 48);
            this.uiBtnPause.TabIndex = 15;
            this.uiBtnPause.TabStop = false;
            this.uiBtnPause.Click += new System.EventHandler(this.uiBtnPause_Click);
            // 
            // pbBotIcon
            // 
            this.pbBotIcon.BackColor = System.Drawing.Color.Transparent;
            this.pbBotIcon.Image = global::taskt.Properties.Resources.executing;
            this.pbBotIcon.Location = new System.Drawing.Point(7, 56);
            this.pbBotIcon.Name = "pbBotIcon";
            this.pbBotIcon.Size = new System.Drawing.Size(675, 135);
            this.pbBotIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbBotIcon.TabIndex = 18;
            this.pbBotIcon.TabStop = false;
            this.pbBotIcon.Click += new System.EventHandler(this.pbBotIcon_Click);
            // 
            // frmScriptEngine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.ClientSize = new System.Drawing.Size(694, 247);
            this.Controls.Add(this.lblAction);
            this.Controls.Add(this.lblKillProcNote);
            this.Controls.Add(this.uiBtnCancel);
            this.Controls.Add(this.uiBtnPause);
            this.Controls.Add(this.lblMainLogo);
            this.Controls.Add(this.lblCloseTimer);
            this.Controls.Add(this.lstSteppingCommands);
            this.Controls.Add(this.pbBotIcon);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmScriptEngine";
            this.Text = "Bot Engine";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmProcessingStatus_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnPause)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBotIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox lstSteppingCommands;
        private System.Windows.Forms.Timer tmrNotify;
        private System.Windows.Forms.Label lblCloseTimer;
        private System.Windows.Forms.Label lblMainLogo;
        private taskt.UI.CustomControls.UIPictureButton uiBtnCancel;
        private taskt.UI.CustomControls.UIPictureButton uiBtnPause;
        private System.Windows.Forms.Label lblKillProcNote;
        private System.Windows.Forms.PictureBox pbBotIcon;
        private System.Windows.Forms.Label lblAction;
    }
}