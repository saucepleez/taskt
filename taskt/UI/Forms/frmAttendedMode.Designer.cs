namespace taskt.UI.Forms
{
    partial class frmAttendedMode
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAttendedMode));
            this.cboSelectedScript = new System.Windows.Forms.ComboBox();
            this.tmrBackColorFlash = new System.Windows.Forms.Timer(this.components);
            this.uiBtnClose = new taskt.UI.CustomControls.UIPictureButton();
            this.uiBtnRun = new taskt.UI.CustomControls.UIPictureButton();
            this.attendedScriptWatcher = new System.IO.FileSystemWatcher();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnRun)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.attendedScriptWatcher)).BeginInit();
            this.SuspendLayout();
            // 
            // cboSelectedScript
            // 
            this.cboSelectedScript.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSelectedScript.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboSelectedScript.FormattingEnabled = true;
            this.cboSelectedScript.Location = new System.Drawing.Point(59, 13);
            this.cboSelectedScript.Name = "cboSelectedScript";
            this.cboSelectedScript.Size = new System.Drawing.Size(334, 28);
            this.cboSelectedScript.TabIndex = 0;
            // 
            // tmrBackColorFlash
            // 
            this.tmrBackColorFlash.Interval = 250;
            this.tmrBackColorFlash.Tick += new System.EventHandler(this.tmrBackColorFlash_Tick);
            // 
            // uiBtnClose
            // 
            this.uiBtnClose.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnClose.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnClose.DisplayText = "Close";
            this.uiBtnClose.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnClose.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnClose.Image = global::taskt.Properties.Resources.command_exitloop;
            this.uiBtnClose.IsMouseOver = false;
            this.uiBtnClose.Location = new System.Drawing.Point(4, 2);
            this.uiBtnClose.Name = "uiBtnClose";
            this.uiBtnClose.Size = new System.Drawing.Size(48, 50);
            this.uiBtnClose.TabIndex = 14;
            this.uiBtnClose.TabStop = false;
            this.uiBtnClose.Click += new System.EventHandler(this.uiBtnClose_Click);
            // 
            // uiBtnRun
            // 
            this.uiBtnRun.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnRun.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnRun.DisplayText = "Run";
            this.uiBtnRun.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnRun.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnRun.Image = global::taskt.Properties.Resources.action_bar_run;
            this.uiBtnRun.IsMouseOver = false;
            this.uiBtnRun.Location = new System.Drawing.Point(398, 2);
            this.uiBtnRun.Name = "uiBtnRun";
            this.uiBtnRun.Size = new System.Drawing.Size(48, 50);
            this.uiBtnRun.TabIndex = 13;
            this.uiBtnRun.TabStop = false;
            this.uiBtnRun.Click += new System.EventHandler(this.uiBtnRun_Click);
            // 
            // attendedScriptWatcher
            // 
            this.attendedScriptWatcher.EnableRaisingEvents = true;
            this.attendedScriptWatcher.SynchronizingObject = this;
            this.attendedScriptWatcher.Created += new System.IO.FileSystemEventHandler(this.attendedScriptWatcher_Created);
            // 
            // frmAttendedMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(59)))), ((int)(((byte)(59)))));
            this.ClientSize = new System.Drawing.Size(445, 57);
            this.Controls.Add(this.uiBtnClose);
            this.Controls.Add(this.uiBtnRun);
            this.Controls.Add(this.cboSelectedScript);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmAttendedMode";
            this.Text = "taskt Attended Mode";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmAttendedMode_Load);
            this.Shown += new System.EventHandler(this.frmAttendedMode_Shown);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.frmAttendedMode_MouseDoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmAttendedMode_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmAttendedMode_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmAttendedMode_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnRun)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.attendedScriptWatcher)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cboSelectedScript;
        private CustomControls.UIPictureButton uiBtnRun;
        private CustomControls.UIPictureButton uiBtnClose;
        private System.Windows.Forms.Timer tmrBackColorFlash;
        private System.IO.FileSystemWatcher attendedScriptWatcher;
    }
}