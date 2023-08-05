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
            this.attendedScriptWatcher = new System.IO.FileSystemWatcher();
            this.attededMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectFolderAttendedMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectFileAttendedMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.attendetMenuSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.closeAttendedMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uiBtnMenu = new taskt.UI.CustomControls.UIPictureButton();
            this.uiBtnRun = new taskt.UI.CustomControls.UIPictureButton();
            ((System.ComponentModel.ISupportInitialize)(this.attendedScriptWatcher)).BeginInit();
            this.attededMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnRun)).BeginInit();
            this.SuspendLayout();
            // 
            // cboSelectedScript
            // 
            this.cboSelectedScript.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSelectedScript.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboSelectedScript.FormattingEnabled = true;
            this.cboSelectedScript.Location = new System.Drawing.Point(59, 12);
            this.cboSelectedScript.Name = "cboSelectedScript";
            this.cboSelectedScript.Size = new System.Drawing.Size(334, 28);
            this.cboSelectedScript.TabIndex = 0;
            // 
            // tmrBackColorFlash
            // 
            this.tmrBackColorFlash.Interval = 250;
            this.tmrBackColorFlash.Tick += new System.EventHandler(this.tmrBackColorFlash_Tick);
            // 
            // attendedScriptWatcher
            // 
            this.attendedScriptWatcher.EnableRaisingEvents = true;
            this.attendedScriptWatcher.SynchronizingObject = this;
            this.attendedScriptWatcher.Created += new System.IO.FileSystemEventHandler(this.attendedScriptWatcher_Created);
            // 
            // attededMenuStrip
            // 
            this.attededMenuStrip.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.attededMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectFolderAttendedMenuItem,
            this.selectFileAttendedMenuItem,
            this.attendetMenuSep1,
            this.closeAttendedMenuItem});
            this.attededMenuStrip.Name = "attededMenuStrip";
            this.attededMenuStrip.Size = new System.Drawing.Size(214, 104);
            // 
            // selectFolderAttendedMenuItem
            // 
            this.selectFolderAttendedMenuItem.Name = "selectFolderAttendedMenuItem";
            this.selectFolderAttendedMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.selectFolderAttendedMenuItem.Size = new System.Drawing.Size(213, 24);
            this.selectFolderAttendedMenuItem.Text = "Select &Folder";
            this.selectFolderAttendedMenuItem.ToolTipText = "Change Script File Folder";
            this.selectFolderAttendedMenuItem.Click += new System.EventHandler(this.selectFolderAttendedMenuItem_Click);
            // 
            // selectFileAttendedMenuItem
            // 
            this.selectFileAttendedMenuItem.Name = "selectFileAttendedMenuItem";
            this.selectFileAttendedMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.selectFileAttendedMenuItem.Size = new System.Drawing.Size(213, 24);
            this.selectFileAttendedMenuItem.Text = "&Select File";
            this.selectFileAttendedMenuItem.ToolTipText = "Select Script File";
            this.selectFileAttendedMenuItem.Click += new System.EventHandler(this.selectFileAttendedMenuItem_Click);
            // 
            // attendetMenuSep1
            // 
            this.attendetMenuSep1.Name = "attendetMenuSep1";
            this.attendetMenuSep1.Size = new System.Drawing.Size(210, 6);
            // 
            // closeAttendedMenuItem
            // 
            this.closeAttendedMenuItem.Image = global::taskt.Properties.Resources.command_exitloop;
            this.closeAttendedMenuItem.Name = "closeAttendedMenuItem";
            this.closeAttendedMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.closeAttendedMenuItem.Size = new System.Drawing.Size(213, 24);
            this.closeAttendedMenuItem.Text = "&Close";
            this.closeAttendedMenuItem.Click += new System.EventHandler(this.closeAttendedMenuItem_Click);
            // 
            // uiBtnMenu
            // 
            this.uiBtnMenu.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnMenu.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnMenu.DisplayText = "Menu";
            this.uiBtnMenu.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnMenu.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnMenu.Image = global::taskt.Properties.Resources.action_bar_options;
            this.uiBtnMenu.IsMouseOver = false;
            this.uiBtnMenu.Location = new System.Drawing.Point(4, 2);
            this.uiBtnMenu.Name = "uiBtnMenu";
            this.uiBtnMenu.Size = new System.Drawing.Size(48, 46);
            this.uiBtnMenu.TabIndex = 14;
            this.uiBtnMenu.TabStop = false;
            this.uiBtnMenu.Text = "Menu";
            this.uiBtnMenu.Click += new System.EventHandler(this.uiBtnMenu_Click);
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
            this.uiBtnRun.Size = new System.Drawing.Size(48, 46);
            this.uiBtnRun.TabIndex = 13;
            this.uiBtnRun.TabStop = false;
            this.uiBtnRun.Text = "Run";
            this.uiBtnRun.Click += new System.EventHandler(this.uiBtnRun_Click);
            // 
            // frmAttendedMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(59)))), ((int)(((byte)(59)))));
            this.ClientSize = new System.Drawing.Size(445, 53);
            this.Controls.Add(this.uiBtnMenu);
            this.Controls.Add(this.uiBtnRun);
            this.Controls.Add(this.cboSelectedScript);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmAttendedMode";
            this.Text = "taskt Attended Mode";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmAttendedMode_Load);
            this.Shown += new System.EventHandler(this.frmAttendedMode_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmAttendedMode_KeyDown);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.frmAttendedMode_MouseDoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmAttendedMode_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmAttendedMode_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmAttendedMode_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.attendedScriptWatcher)).EndInit();
            this.attededMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnRun)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cboSelectedScript;
        private CustomControls.UIPictureButton uiBtnRun;
        private CustomControls.UIPictureButton uiBtnMenu;
        private System.Windows.Forms.Timer tmrBackColorFlash;
        private System.IO.FileSystemWatcher attendedScriptWatcher;
        private System.Windows.Forms.ContextMenuStrip attededMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem selectFolderAttendedMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectFileAttendedMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeAttendedMenuItem;
        private System.Windows.Forms.ToolStripSeparator attendetMenuSep1;
    }
}