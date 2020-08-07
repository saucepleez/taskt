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
            this.uiBtnStepInto = new taskt.UI.CustomControls.CustomUIControls.UIPictureButton();
            this.uiBtnStepOver = new taskt.UI.CustomControls.CustomUIControls.UIPictureButton();
            this.uiBtnCancel = new taskt.UI.CustomControls.CustomUIControls.UIPictureButton();
            this.uiBtnPause = new taskt.UI.CustomControls.CustomUIControls.UIPictureButton();
            this.pbBotIcon = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnStepInto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnStepOver)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnPause)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBotIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // lstSteppingCommands
            // 
            this.lstSteppingCommands.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstSteppingCommands.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstSteppingCommands.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lstSteppingCommands.FormattingEnabled = true;
            this.lstSteppingCommands.ItemHeight = 21;
            this.lstSteppingCommands.Location = new System.Drawing.Point(9, 69);
            this.lstSteppingCommands.Margin = new System.Windows.Forms.Padding(4);
            this.lstSteppingCommands.Name = "lstSteppingCommands";
            this.lstSteppingCommands.Size = new System.Drawing.Size(899, 151);
            this.lstSteppingCommands.TabIndex = 0;
            this.lstSteppingCommands.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstSteppingCommands_DrawItem);
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
            this.lblCloseTimer.Location = new System.Drawing.Point(16, 217);
            this.lblCloseTimer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCloseTimer.Name = "lblCloseTimer";
            this.lblCloseTimer.Size = new System.Drawing.Size(0, 23);
            this.lblCloseTimer.TabIndex = 3;
            // 
            // lblMainLogo
            // 
            this.lblMainLogo.AutoSize = true;
            this.lblMainLogo.BackColor = System.Drawing.Color.Transparent;
            this.lblMainLogo.Font = new System.Drawing.Font("Segoe UI Semilight", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMainLogo.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblMainLogo.Location = new System.Drawing.Point(1, 6);
            this.lblMainLogo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMainLogo.Name = "lblMainLogo";
            this.lblMainLogo.Size = new System.Drawing.Size(318, 54);
            this.lblMainLogo.TabIndex = 4;
            this.lblMainLogo.Text = "taskt is executing";
            // 
            // lblKillProcNote
            // 
            this.lblKillProcNote.AutoSize = true;
            this.lblKillProcNote.BackColor = System.Drawing.Color.Transparent;
            this.lblKillProcNote.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKillProcNote.ForeColor = System.Drawing.Color.White;
            this.lblKillProcNote.Location = new System.Drawing.Point(5, 227);
            this.lblKillProcNote.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblKillProcNote.Name = "lblKillProcNote";
            this.lblKillProcNote.Size = new System.Drawing.Size(419, 25);
            this.lblKillProcNote.TabIndex = 17;
            this.lblKillProcNote.Text = "Press the \'Pause/Break\' key to stop automation.";
            // 
            // lblAction
            // 
            this.lblAction.AutoSize = true;
            this.lblAction.BackColor = System.Drawing.Color.Transparent;
            this.lblAction.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAction.ForeColor = System.Drawing.Color.White;
            this.lblAction.Location = new System.Drawing.Point(7, 257);
            this.lblAction.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAction.Name = "lblAction";
            this.lblAction.Size = new System.Drawing.Size(83, 25);
            this.lblAction.TabIndex = 19;
            this.lblAction.Text = "Action...";
            this.lblAction.Visible = false;
            // 
            // uiBtnStepInto
            // 
            this.uiBtnStepInto.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnStepInto.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnStepInto.DisplayText = "Step Into";
            this.uiBtnStepInto.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnStepInto.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnStepInto.Image = global::taskt.Properties.Resources.command_step_into;
            this.uiBtnStepInto.IsMouseOver = false;
            this.uiBtnStepInto.Location = new System.Drawing.Point(700, 2);
            this.uiBtnStepInto.Margin = new System.Windows.Forms.Padding(4);
            this.uiBtnStepInto.Name = "uiBtnStepInto";
            this.uiBtnStepInto.Size = new System.Drawing.Size(64, 59);
            this.uiBtnStepInto.TabIndex = 23;
            this.uiBtnStepInto.TabStop = false;
            this.uiBtnStepInto.Text = "Step Into";
            this.uiBtnStepInto.Visible = false;
            this.uiBtnStepInto.Click += new System.EventHandler(this.uiBtnStepInto_Click);
            // 
            // uiBtnStepOver
            // 
            this.uiBtnStepOver.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnStepOver.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnStepOver.DisplayText = "Step Over";
            this.uiBtnStepOver.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnStepOver.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnStepOver.Image = global::taskt.Properties.Resources.command_step_over;
            this.uiBtnStepOver.IsMouseOver = false;
            this.uiBtnStepOver.Location = new System.Drawing.Point(628, 2);
            this.uiBtnStepOver.Margin = new System.Windows.Forms.Padding(4);
            this.uiBtnStepOver.Name = "uiBtnStepOver";
            this.uiBtnStepOver.Size = new System.Drawing.Size(64, 59);
            this.uiBtnStepOver.TabIndex = 22;
            this.uiBtnStepOver.TabStop = false;
            this.uiBtnStepOver.Text = "Step Over";
            this.uiBtnStepOver.Visible = false;
            this.uiBtnStepOver.Click += new System.EventHandler(this.uiBtnStepOver_Click);
            // 
            // uiBtnCancel
            // 
            this.uiBtnCancel.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnCancel.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnCancel.DisplayText = "Cancel";
            this.uiBtnCancel.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnCancel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnCancel.Image = global::taskt.Properties.Resources.command_cancel;
            this.uiBtnCancel.IsMouseOver = false;
            this.uiBtnCancel.Location = new System.Drawing.Point(844, 2);
            this.uiBtnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.uiBtnCancel.Name = "uiBtnCancel";
            this.uiBtnCancel.Size = new System.Drawing.Size(64, 59);
            this.uiBtnCancel.TabIndex = 14;
            this.uiBtnCancel.TabStop = false;
            this.uiBtnCancel.Text = "Cancel";
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
            this.uiBtnPause.Location = new System.Drawing.Point(772, 2);
            this.uiBtnPause.Margin = new System.Windows.Forms.Padding(4);
            this.uiBtnPause.Name = "uiBtnPause";
            this.uiBtnPause.Size = new System.Drawing.Size(64, 59);
            this.uiBtnPause.TabIndex = 15;
            this.uiBtnPause.TabStop = false;
            this.uiBtnPause.Text = "Pause";
            this.uiBtnPause.Click += new System.EventHandler(this.uiBtnPause_Click);
            // 
            // pbBotIcon
            // 
            this.pbBotIcon.BackColor = System.Drawing.Color.Transparent;
            this.pbBotIcon.Image = global::taskt.Properties.Resources.executing;
            this.pbBotIcon.Location = new System.Drawing.Point(9, 69);
            this.pbBotIcon.Margin = new System.Windows.Forms.Padding(4);
            this.pbBotIcon.Name = "pbBotIcon";
            this.pbBotIcon.Size = new System.Drawing.Size(675, 135);
            this.pbBotIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbBotIcon.TabIndex = 18;
            this.pbBotIcon.TabStop = false;
            this.pbBotIcon.Click += new System.EventHandler(this.pbBotIcon_Click);
            // 
            // frmScriptEngine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.ClientSize = new System.Drawing.Size(925, 304);
            this.Controls.Add(this.uiBtnStepInto);
            this.Controls.Add(this.uiBtnStepOver);
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
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "frmScriptEngine";
            this.Text = "Bot Engine";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmProcessingStatus_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnStepInto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnStepOver)).EndInit();
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
        private taskt.UI.CustomControls.CustomUIControls.UIPictureButton uiBtnCancel;
        private taskt.UI.CustomControls.CustomUIControls.UIPictureButton uiBtnPause;
        private System.Windows.Forms.Label lblKillProcNote;
        private System.Windows.Forms.PictureBox pbBotIcon;
        private System.Windows.Forms.Label lblAction;
        private CustomControls.CustomUIControls.UIPictureButton uiBtnStepInto;
        private CustomControls.CustomUIControls.UIPictureButton uiBtnStepOver;
    }
}