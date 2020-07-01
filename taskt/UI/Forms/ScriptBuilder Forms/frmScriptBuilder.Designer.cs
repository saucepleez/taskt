using System;
using System.Windows.Forms;

namespace taskt.UI.Forms.ScriptBuilder_Forms
{
    partial class frmScriptBuilder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmScriptBuilder));
            taskt.Core.Theme theme1 = new taskt.Core.Theme();
            this.cmsProjectFolderActions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiCopyFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDeleteFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiNewFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiNewScriptFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiPasteFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRenameFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.tmrNotify = new System.Windows.Forms.Timer(this.components);
            this.cmsScriptActions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.enableSelectedCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableSelectedCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pauseBeforeExecutionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cutSelectedActionssToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copySelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToParentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyTray = new System.Windows.Forms.NotifyIcon(this.components);
            this.pnlControlContainer = new System.Windows.Forms.Panel();
            this.grpSearch = new taskt.UI.CustomControls.UIGroupBox();
            this.pbSearch = new System.Windows.Forms.PictureBox();
            this.lblCurrentlyViewing = new System.Windows.Forms.Label();
            this.lblTotalResults = new System.Windows.Forms.Label();
            this.txtCommandSearch = new System.Windows.Forms.TextBox();
            this.grpSaveClose = new taskt.UI.CustomControls.UIGroupBox();
            this.btnSequenceImport = new taskt.UI.CustomControls.UIPictureButton();
            this.uiBtnKeep = new taskt.UI.CustomControls.UIPictureButton();
            this.uiBtnClose = new taskt.UI.CustomControls.UIPictureButton();
            this.grpFileActions = new taskt.UI.CustomControls.UIGroupBox();
            this.uiBtnProject = new taskt.UI.CustomControls.UIPictureButton();
            this.uiBtnImport = new taskt.UI.CustomControls.UIPictureButton();
            this.uiBtnSaveAs = new taskt.UI.CustomControls.UIPictureButton();
            this.uiBtnSave = new taskt.UI.CustomControls.UIPictureButton();
            this.uiBtnNew = new taskt.UI.CustomControls.UIPictureButton();
            this.uiBtnOpen = new taskt.UI.CustomControls.UIPictureButton();
            this.grpRecordRun = new taskt.UI.CustomControls.UIGroupBox();
            this.uiBtnRecordSequence = new taskt.UI.CustomControls.UIPictureButton();
            this.uiBtnRunScript = new taskt.UI.CustomControls.UIPictureButton();
            this.uiBtnScheduleManagement = new taskt.UI.CustomControls.UIPictureButton();
            this.grpVariable = new taskt.UI.CustomControls.UIGroupBox();
            this.uiBtnClearAll = new taskt.UI.CustomControls.UIPictureButton();
            this.uiBtnSettings = new taskt.UI.CustomControls.UIPictureButton();
            this.uiBtnAddVariable = new taskt.UI.CustomControls.UIPictureButton();
            this.pnlStatus = new System.Windows.Forms.Panel();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.pnlMain = new taskt.UI.CustomControls.UIPanel();
            this.lblMainLogo = new System.Windows.Forms.Label();
            this.lblCoordinatorInfo = new System.Windows.Forms.Label();
            this.splitContainer1 = new taskt.UI.CustomControls.UISplitContainer();
            this.uiPaneTabs = new taskt.UI.CustomControls.UITabControl();
            this.tpProject = new System.Windows.Forms.TabPage();
            this.tvProject = new taskt.UI.CustomControls.UITreeView();
            this.imgListProjectPane = new System.Windows.Forms.ImageList(this.components);
            this.tpCommands = new System.Windows.Forms.TabPage();
            this.tvCommands = new taskt.UI.CustomControls.UITreeView();
            this.uiScriptTabControl = new taskt.UI.CustomControls.UITabControl();
            this.pnlCommandHelper = new System.Windows.Forms.Panel();
            this.flwRecentFiles = new taskt.UI.CustomControls.UIFlowLayoutPanel();
            this.lblFilesMissing = new System.Windows.Forms.Label();
            this.pbRecentFiles = new System.Windows.Forms.PictureBox();
            this.pbLinks = new System.Windows.Forms.PictureBox();
            this.pbTasktLogo = new System.Windows.Forms.PictureBox();
            this.lblRecentFiles = new System.Windows.Forms.Label();
            this.lnkGitWiki = new System.Windows.Forms.LinkLabel();
            this.lnkGitIssue = new System.Windows.Forms.LinkLabel();
            this.lnkGitLatestReleases = new System.Windows.Forms.LinkLabel();
            this.lnkGitProject = new System.Windows.Forms.LinkLabel();
            this.lblWelcomeToTaskt = new System.Windows.Forms.Label();
            this.lblWelcomeDescription = new System.Windows.Forms.Label();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmCommand = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlDivider = new System.Windows.Forms.Panel();
            this.msTasktMenu = new taskt.UI.CustomControls.UIMenuStrip();
            this.fileActionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restartApplicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeApplicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modifyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.variablesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showSearchBarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutTasktToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scriptActionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scheduleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAndRunToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsSearchBox = new System.Windows.Forms.ToolStripTextBox();
            this.tsSearchButton = new System.Windows.Forms.ToolStripMenuItem();
            this.tsSearchResult = new System.Windows.Forms.ToolStripMenuItem();
            this.tlpControls = new System.Windows.Forms.TableLayoutPanel();
            this.cmsProjectFileActions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiCopyFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDeleteFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRenameFile = new System.Windows.Forms.ToolStripMenuItem();
            this.imgListTabControl = new System.Windows.Forms.ImageList(this.components);
            this.cmsScriptTabActions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiCloseTab = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCloseAllButThis = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsProjectFolderActions.SuspendLayout();
            this.cmsScriptActions.SuspendLayout();
            this.pnlControlContainer.SuspendLayout();
            this.grpSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSearch)).BeginInit();
            this.grpSaveClose.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnSequenceImport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnKeep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnClose)).BeginInit();
            this.grpFileActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnProject)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnImport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnSaveAs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnNew)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOpen)).BeginInit();
            this.grpRecordRun.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnRecordSequence)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnRunScript)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnScheduleManagement)).BeginInit();
            this.grpVariable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnClearAll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnSettings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnAddVariable)).BeginInit();
            this.pnlHeader.SuspendLayout();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.uiPaneTabs.SuspendLayout();
            this.tpProject.SuspendLayout();
            this.tpCommands.SuspendLayout();
            this.pnlCommandHelper.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbRecentFiles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLinks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTasktLogo)).BeginInit();
            this.msTasktMenu.SuspendLayout();
            this.tlpControls.SuspendLayout();
            this.cmsProjectFileActions.SuspendLayout();
            this.cmsScriptTabActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmsProjectFolderActions
            // 
            this.cmsProjectFolderActions.AllowDrop = true;
            this.cmsProjectFolderActions.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.cmsProjectFolderActions.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsProjectFolderActions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCopyFolder,
            this.tsmiDeleteFolder,
            this.tsmiNewFolder,
            this.tsmiNewScriptFile,
            this.tsmiPasteFolder,
            this.tsmiRenameFolder});
            this.cmsProjectFolderActions.Name = "cmsProjectActions";
            this.cmsProjectFolderActions.Size = new System.Drawing.Size(199, 172);
            // 
            // tsmiCopyFolder
            // 
            this.tsmiCopyFolder.Image = ((System.Drawing.Image)(resources.GetObject("tsmiCopyFolder.Image")));
            this.tsmiCopyFolder.Name = "tsmiCopyFolder";
            this.tsmiCopyFolder.Size = new System.Drawing.Size(198, 28);
            this.tsmiCopyFolder.Text = "Copy";
            this.tsmiCopyFolder.Click += new System.EventHandler(this.tsmiCopyFolder_Click);
            // 
            // tsmiDeleteFolder
            // 
            this.tsmiDeleteFolder.Image = ((System.Drawing.Image)(resources.GetObject("tsmiDeleteFolder.Image")));
            this.tsmiDeleteFolder.Name = "tsmiDeleteFolder";
            this.tsmiDeleteFolder.Size = new System.Drawing.Size(198, 28);
            this.tsmiDeleteFolder.Text = "Delete";
            this.tsmiDeleteFolder.Click += new System.EventHandler(this.tsmiDeleteFolder_Click);
            // 
            // tsmiNewFolder
            // 
            this.tsmiNewFolder.Image = ((System.Drawing.Image)(resources.GetObject("tsmiNewFolder.Image")));
            this.tsmiNewFolder.Name = "tsmiNewFolder";
            this.tsmiNewFolder.Size = new System.Drawing.Size(198, 28);
            this.tsmiNewFolder.Text = "New Folder";
            this.tsmiNewFolder.Click += new System.EventHandler(this.tsmiNewFolder_Click);
            // 
            // tsmiNewScriptFile
            // 
            this.tsmiNewScriptFile.Image = ((System.Drawing.Image)(resources.GetObject("tsmiNewScriptFile.Image")));
            this.tsmiNewScriptFile.Name = "tsmiNewScriptFile";
            this.tsmiNewScriptFile.Size = new System.Drawing.Size(198, 28);
            this.tsmiNewScriptFile.Text = "New Script File";
            this.tsmiNewScriptFile.Click += new System.EventHandler(this.tsmiNewScriptFile_Click);
            // 
            // tsmiPasteFolder
            // 
            this.tsmiPasteFolder.Image = ((System.Drawing.Image)(resources.GetObject("tsmiPasteFolder.Image")));
            this.tsmiPasteFolder.Name = "tsmiPasteFolder";
            this.tsmiPasteFolder.Size = new System.Drawing.Size(198, 28);
            this.tsmiPasteFolder.Text = "Paste";
            this.tsmiPasteFolder.Click += new System.EventHandler(this.tsmiPasteFolder_Click);
            // 
            // tsmiRenameFolder
            // 
            this.tsmiRenameFolder.Image = ((System.Drawing.Image)(resources.GetObject("tsmiRenameFolder.Image")));
            this.tsmiRenameFolder.Name = "tsmiRenameFolder";
            this.tsmiRenameFolder.Size = new System.Drawing.Size(198, 28);
            this.tsmiRenameFolder.Text = "Rename";
            this.tsmiRenameFolder.Click += new System.EventHandler(this.tsmiRenameFolder_Click);
            // 
            // tmrNotify
            // 
            this.tmrNotify.Enabled = true;
            this.tmrNotify.Interval = 500;
            this.tmrNotify.Tick += new System.EventHandler(this.tmrNotify_Tick);
            // 
            // cmsScriptActions
            // 
            this.cmsScriptActions.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.cmsScriptActions.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.cmsScriptActions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enableSelectedCodeToolStripMenuItem,
            this.disableSelectedCodeToolStripMenuItem,
            this.pauseBeforeExecutionToolStripMenuItem,
            this.cutSelectedActionssToolStripMenuItem,
            this.copySelectedToolStripMenuItem,
            this.pasteSelectedToolStripMenuItem,
            this.moveToParentToolStripMenuItem,
            this.viewCodeToolStripMenuItem});
            this.cmsScriptActions.Name = "cmsScriptActions";
            this.cmsScriptActions.Size = new System.Drawing.Size(264, 228);
            // 
            // enableSelectedCodeToolStripMenuItem
            // 
            this.enableSelectedCodeToolStripMenuItem.Name = "enableSelectedCodeToolStripMenuItem";
            this.enableSelectedCodeToolStripMenuItem.Size = new System.Drawing.Size(263, 28);
            this.enableSelectedCodeToolStripMenuItem.Text = "Enable Selected Code";
            this.enableSelectedCodeToolStripMenuItem.Click += new System.EventHandler(this.enableSelectedCodeToolStripMenuItem_Click);
            // 
            // disableSelectedCodeToolStripMenuItem
            // 
            this.disableSelectedCodeToolStripMenuItem.Name = "disableSelectedCodeToolStripMenuItem";
            this.disableSelectedCodeToolStripMenuItem.Size = new System.Drawing.Size(263, 28);
            this.disableSelectedCodeToolStripMenuItem.Text = "Disable Selected Code";
            this.disableSelectedCodeToolStripMenuItem.Click += new System.EventHandler(this.disableSelectedCodeToolStripMenuItem_Click);
            // 
            // pauseBeforeExecutionToolStripMenuItem
            // 
            this.pauseBeforeExecutionToolStripMenuItem.Name = "pauseBeforeExecutionToolStripMenuItem";
            this.pauseBeforeExecutionToolStripMenuItem.Size = new System.Drawing.Size(263, 28);
            this.pauseBeforeExecutionToolStripMenuItem.Text = "Pause Before Execution";
            this.pauseBeforeExecutionToolStripMenuItem.Click += new System.EventHandler(this.pauseBeforeExecutionToolStripMenuItem_Click);
            // 
            // cutSelectedActionssToolStripMenuItem
            // 
            this.cutSelectedActionssToolStripMenuItem.Name = "cutSelectedActionssToolStripMenuItem";
            this.cutSelectedActionssToolStripMenuItem.Size = new System.Drawing.Size(263, 28);
            this.cutSelectedActionssToolStripMenuItem.Text = "Cut Selected Actions(s)";
            this.cutSelectedActionssToolStripMenuItem.Click += new System.EventHandler(this.cutSelectedActionssToolStripMenuItem_Click);
            // 
            // copySelectedToolStripMenuItem
            // 
            this.copySelectedToolStripMenuItem.Name = "copySelectedToolStripMenuItem";
            this.copySelectedToolStripMenuItem.Size = new System.Drawing.Size(263, 28);
            this.copySelectedToolStripMenuItem.Text = "Copy Selected Action(s)";
            this.copySelectedToolStripMenuItem.Click += new System.EventHandler(this.copySelectedToolStripMenuItem_Click);
            // 
            // pasteSelectedToolStripMenuItem
            // 
            this.pasteSelectedToolStripMenuItem.Name = "pasteSelectedToolStripMenuItem";
            this.pasteSelectedToolStripMenuItem.Size = new System.Drawing.Size(263, 28);
            this.pasteSelectedToolStripMenuItem.Text = "Paste Selected Action(s)";
            this.pasteSelectedToolStripMenuItem.Click += new System.EventHandler(this.pasteSelectedToolStripMenuItem_Click);
            // 
            // moveToParentToolStripMenuItem
            // 
            this.moveToParentToolStripMenuItem.Name = "moveToParentToolStripMenuItem";
            this.moveToParentToolStripMenuItem.Size = new System.Drawing.Size(263, 28);
            this.moveToParentToolStripMenuItem.Text = "Move Out To Parent";
            this.moveToParentToolStripMenuItem.Visible = false;
            this.moveToParentToolStripMenuItem.Click += new System.EventHandler(this.moveToParentToolStripMenuItem_Click);
            // 
            // viewCodeToolStripMenuItem
            // 
            this.viewCodeToolStripMenuItem.Name = "viewCodeToolStripMenuItem";
            this.viewCodeToolStripMenuItem.Size = new System.Drawing.Size(263, 28);
            this.viewCodeToolStripMenuItem.Text = "View Code";
            this.viewCodeToolStripMenuItem.Click += new System.EventHandler(this.viewCodeToolStripMenuItem_Click);
            // 
            // notifyTray
            // 
            this.notifyTray.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyTray.BalloonTipText = "taskt is still running in your system tray. Double-click to restore taskt to full" +
    " size!\r\n";
            this.notifyTray.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyTray.Icon")));
            this.notifyTray.Text = "taskt, free and open-source process automation";
            this.notifyTray.Visible = true;
            this.notifyTray.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyTray_MouseDoubleClick);
            // 
            // pnlControlContainer
            // 
            this.pnlControlContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.tlpControls.SetColumnSpan(this.pnlControlContainer, 3);
            this.pnlControlContainer.Controls.Add(this.grpSearch);
            this.pnlControlContainer.Controls.Add(this.grpSaveClose);
            this.pnlControlContainer.Controls.Add(this.grpFileActions);
            this.pnlControlContainer.Controls.Add(this.grpRecordRun);
            this.pnlControlContainer.Controls.Add(this.grpVariable);
            this.pnlControlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlControlContainer.Location = new System.Drawing.Point(0, 89);
            this.pnlControlContainer.Margin = new System.Windows.Forms.Padding(0);
            this.pnlControlContainer.Name = "pnlControlContainer";
            this.pnlControlContainer.Size = new System.Drawing.Size(1328, 101);
            this.pnlControlContainer.TabIndex = 7;
            this.pnlControlContainer.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlControlContainer_Paint);
            // 
            // grpSearch
            // 
            this.grpSearch.BackColor = System.Drawing.Color.Transparent;
            this.grpSearch.Controls.Add(this.pbSearch);
            this.grpSearch.Controls.Add(this.lblCurrentlyViewing);
            this.grpSearch.Controls.Add(this.lblTotalResults);
            this.grpSearch.Controls.Add(this.txtCommandSearch);
            this.grpSearch.Location = new System.Drawing.Point(802, 8);
            this.grpSearch.Margin = new System.Windows.Forms.Padding(4);
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.Padding = new System.Windows.Forms.Padding(4);
            this.grpSearch.Size = new System.Drawing.Size(248, 91);
            this.grpSearch.TabIndex = 20;
            this.grpSearch.TabStop = false;
            this.grpSearch.Text = "Search";
            this.grpSearch.TitleBackColor = System.Drawing.Color.Transparent;
            this.grpSearch.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpSearch.TitleForeColor = System.Drawing.Color.GhostWhite;
            this.grpSearch.TitleHatchStyle = System.Drawing.Drawing2D.HatchStyle.Horizontal;
            // 
            // pbSearch
            // 
            this.pbSearch.Image = global::taskt.Properties.Resources.command_search;
            this.pbSearch.Location = new System.Drawing.Point(198, 22);
            this.pbSearch.Margin = new System.Windows.Forms.Padding(4);
            this.pbSearch.Name = "pbSearch";
            this.pbSearch.Size = new System.Drawing.Size(20, 20);
            this.pbSearch.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbSearch.TabIndex = 17;
            this.pbSearch.TabStop = false;
            this.pbSearch.Click += new System.EventHandler(this.pbSearch_Click);
            this.pbSearch.MouseEnter += new System.EventHandler(this.pbSearch_MouseEnter);
            this.pbSearch.MouseLeave += new System.EventHandler(this.pbSearch_MouseLeave);
            // 
            // lblCurrentlyViewing
            // 
            this.lblCurrentlyViewing.AutoSize = true;
            this.lblCurrentlyViewing.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentlyViewing.ForeColor = System.Drawing.Color.DimGray;
            this.lblCurrentlyViewing.Location = new System.Drawing.Point(6, 70);
            this.lblCurrentlyViewing.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurrentlyViewing.Name = "lblCurrentlyViewing";
            this.lblCurrentlyViewing.Size = new System.Drawing.Size(123, 19);
            this.lblCurrentlyViewing.TabIndex = 3;
            this.lblCurrentlyViewing.Text = "Viewing Result X/Y";
            this.lblCurrentlyViewing.Visible = false;
            // 
            // lblTotalResults
            // 
            this.lblTotalResults.AutoSize = true;
            this.lblTotalResults.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalResults.ForeColor = System.Drawing.Color.DimGray;
            this.lblTotalResults.Location = new System.Drawing.Point(6, 52);
            this.lblTotalResults.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTotalResults.Name = "lblTotalResults";
            this.lblTotalResults.Size = new System.Drawing.Size(140, 19);
            this.lblTotalResults.TabIndex = 2;
            this.lblTotalResults.Text = "X Total Results Found";
            this.lblTotalResults.Visible = false;
            // 
            // txtCommandSearch
            // 
            this.txtCommandSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCommandSearch.Location = new System.Drawing.Point(6, 20);
            this.txtCommandSearch.Margin = new System.Windows.Forms.Padding(4);
            this.txtCommandSearch.Name = "txtCommandSearch";
            this.txtCommandSearch.Size = new System.Drawing.Size(188, 27);
            this.txtCommandSearch.TabIndex = 0;
            this.txtCommandSearch.TextChanged += new System.EventHandler(this.txtCommandSearch_TextChanged);
            // 
            // grpSaveClose
            // 
            this.grpSaveClose.BackColor = System.Drawing.Color.Transparent;
            this.grpSaveClose.Controls.Add(this.btnSequenceImport);
            this.grpSaveClose.Controls.Add(this.uiBtnKeep);
            this.grpSaveClose.Controls.Add(this.uiBtnClose);
            this.grpSaveClose.Location = new System.Drawing.Point(1045, 4);
            this.grpSaveClose.Margin = new System.Windows.Forms.Padding(4);
            this.grpSaveClose.Name = "grpSaveClose";
            this.grpSaveClose.Padding = new System.Windows.Forms.Padding(4);
            this.grpSaveClose.Size = new System.Drawing.Size(206, 91);
            this.grpSaveClose.TabIndex = 19;
            this.grpSaveClose.TabStop = false;
            this.grpSaveClose.Text = "Save and Close";
            this.grpSaveClose.TitleBackColor = System.Drawing.Color.Transparent;
            this.grpSaveClose.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpSaveClose.TitleForeColor = System.Drawing.Color.GhostWhite;
            this.grpSaveClose.TitleHatchStyle = System.Drawing.Drawing2D.HatchStyle.Horizontal;
            this.grpSaveClose.Visible = false;
            // 
            // btnSequenceImport
            // 
            this.btnSequenceImport.BackColor = System.Drawing.Color.Transparent;
            this.btnSequenceImport.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btnSequenceImport.DisplayText = "Import";
            this.btnSequenceImport.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.btnSequenceImport.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnSequenceImport.Image = global::taskt.Properties.Resources.action_bar_import;
            this.btnSequenceImport.IsMouseOver = false;
            this.btnSequenceImport.Location = new System.Drawing.Point(138, 22);
            this.btnSequenceImport.Margin = new System.Windows.Forms.Padding(4);
            this.btnSequenceImport.Name = "btnSequenceImport";
            this.btnSequenceImport.Size = new System.Drawing.Size(60, 62);
            this.btnSequenceImport.TabIndex = 20;
            this.btnSequenceImport.TabStop = false;
            this.btnSequenceImport.Text = "Import";
            this.btnSequenceImport.Click += new System.EventHandler(this.btnSequenceImport_Click);
            // 
            // uiBtnKeep
            // 
            this.uiBtnKeep.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnKeep.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnKeep.DisplayText = "Keep";
            this.uiBtnKeep.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnKeep.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnKeep.Image = global::taskt.Properties.Resources.action_bar_record;
            this.uiBtnKeep.IsMouseOver = false;
            this.uiBtnKeep.Location = new System.Drawing.Point(2, 22);
            this.uiBtnKeep.Margin = new System.Windows.Forms.Padding(4);
            this.uiBtnKeep.Name = "uiBtnKeep";
            this.uiBtnKeep.Size = new System.Drawing.Size(60, 62);
            this.uiBtnKeep.TabIndex = 19;
            this.uiBtnKeep.TabStop = false;
            this.uiBtnKeep.Text = "Keep";
            this.uiBtnKeep.Click += new System.EventHandler(this.uiBtnKeep_Click);
            // 
            // uiBtnClose
            // 
            this.uiBtnClose.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnClose.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnClose.DisplayText = "Close";
            this.uiBtnClose.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnClose.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnClose.Image = global::taskt.Properties.Resources.action_bar_schedule;
            this.uiBtnClose.IsMouseOver = false;
            this.uiBtnClose.Location = new System.Drawing.Point(68, 22);
            this.uiBtnClose.Margin = new System.Windows.Forms.Padding(4);
            this.uiBtnClose.Name = "uiBtnClose";
            this.uiBtnClose.Size = new System.Drawing.Size(65, 62);
            this.uiBtnClose.TabIndex = 13;
            this.uiBtnClose.TabStop = false;
            this.uiBtnClose.Text = "Close";
            this.uiBtnClose.Click += new System.EventHandler(this.uiBtnClose_Click);
            // 
            // grpFileActions
            // 
            this.grpFileActions.BackColor = System.Drawing.Color.Transparent;
            this.grpFileActions.Controls.Add(this.uiBtnProject);
            this.grpFileActions.Controls.Add(this.uiBtnImport);
            this.grpFileActions.Controls.Add(this.uiBtnSaveAs);
            this.grpFileActions.Controls.Add(this.uiBtnSave);
            this.grpFileActions.Controls.Add(this.uiBtnNew);
            this.grpFileActions.Controls.Add(this.uiBtnOpen);
            this.grpFileActions.Location = new System.Drawing.Point(11, 6);
            this.grpFileActions.Margin = new System.Windows.Forms.Padding(4);
            this.grpFileActions.Name = "grpFileActions";
            this.grpFileActions.Padding = new System.Windows.Forms.Padding(4);
            this.grpFileActions.Size = new System.Drawing.Size(371, 91);
            this.grpFileActions.TabIndex = 16;
            this.grpFileActions.TabStop = false;
            this.grpFileActions.Text = "File Actions";
            this.grpFileActions.TitleBackColor = System.Drawing.Color.Transparent;
            this.grpFileActions.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpFileActions.TitleForeColor = System.Drawing.Color.GhostWhite;
            this.grpFileActions.TitleHatchStyle = System.Drawing.Drawing2D.HatchStyle.Horizontal;
            // 
            // uiBtnProject
            // 
            this.uiBtnProject.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnProject.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnProject.DisplayText = "Project";
            this.uiBtnProject.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnProject.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.uiBtnProject.Image = global::taskt.Properties.Resources.action_bar_run;
            this.uiBtnProject.IsMouseOver = false;
            this.uiBtnProject.Location = new System.Drawing.Point(0, 24);
            this.uiBtnProject.Margin = new System.Windows.Forms.Padding(4);
            this.uiBtnProject.Name = "uiBtnProject";
            this.uiBtnProject.Size = new System.Drawing.Size(60, 62);
            this.uiBtnProject.TabIndex = 15;
            this.uiBtnProject.TabStop = false;
            this.uiBtnProject.Text = "Project";
            this.uiBtnProject.Click += new System.EventHandler(this.uiBtnProject_Click);
            // 
            // uiBtnImport
            // 
            this.uiBtnImport.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnImport.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnImport.DisplayText = "Import";
            this.uiBtnImport.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnImport.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnImport.Image = global::taskt.Properties.Resources.action_bar_import;
            this.uiBtnImport.IsMouseOver = false;
            this.uiBtnImport.Location = new System.Drawing.Point(183, 24);
            this.uiBtnImport.Margin = new System.Windows.Forms.Padding(4);
            this.uiBtnImport.Name = "uiBtnImport";
            this.uiBtnImport.Size = new System.Drawing.Size(60, 62);
            this.uiBtnImport.TabIndex = 14;
            this.uiBtnImport.TabStop = false;
            this.uiBtnImport.Text = "Import";
            this.uiBtnImport.Click += new System.EventHandler(this.uiBtnImport_Click);
            // 
            // uiBtnSaveAs
            // 
            this.uiBtnSaveAs.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnSaveAs.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnSaveAs.DisplayText = "Save As";
            this.uiBtnSaveAs.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnSaveAs.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnSaveAs.Image = global::taskt.Properties.Resources.action_bar_saveas;
            this.uiBtnSaveAs.IsMouseOver = false;
            this.uiBtnSaveAs.Location = new System.Drawing.Point(306, 24);
            this.uiBtnSaveAs.Margin = new System.Windows.Forms.Padding(4);
            this.uiBtnSaveAs.Name = "uiBtnSaveAs";
            this.uiBtnSaveAs.Size = new System.Drawing.Size(60, 62);
            this.uiBtnSaveAs.TabIndex = 13;
            this.uiBtnSaveAs.TabStop = false;
            this.uiBtnSaveAs.Text = "Save As";
            this.uiBtnSaveAs.Click += new System.EventHandler(this.uiBtnSaveAs_Click);
            // 
            // uiBtnSave
            // 
            this.uiBtnSave.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnSave.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnSave.DisplayText = "Save";
            this.uiBtnSave.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnSave.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnSave.Image = global::taskt.Properties.Resources.action_bar_save;
            this.uiBtnSave.IsMouseOver = false;
            this.uiBtnSave.Location = new System.Drawing.Point(244, 24);
            this.uiBtnSave.Margin = new System.Windows.Forms.Padding(4);
            this.uiBtnSave.Name = "uiBtnSave";
            this.uiBtnSave.Size = new System.Drawing.Size(60, 62);
            this.uiBtnSave.TabIndex = 11;
            this.uiBtnSave.TabStop = false;
            this.uiBtnSave.Text = "Save";
            this.uiBtnSave.Click += new System.EventHandler(this.uiBtnSave_Click);
            // 
            // uiBtnNew
            // 
            this.uiBtnNew.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnNew.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnNew.DisplayText = "New";
            this.uiBtnNew.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.uiBtnNew.Image = global::taskt.Properties.Resources.action_bar_new;
            this.uiBtnNew.IsMouseOver = false;
            this.uiBtnNew.Location = new System.Drawing.Point(60, 24);
            this.uiBtnNew.Margin = new System.Windows.Forms.Padding(4);
            this.uiBtnNew.Name = "uiBtnNew";
            this.uiBtnNew.Size = new System.Drawing.Size(60, 62);
            this.uiBtnNew.TabIndex = 12;
            this.uiBtnNew.TabStop = false;
            this.uiBtnNew.Text = "New";
            this.uiBtnNew.Click += new System.EventHandler(this.uiBtnNew_Click);
            // 
            // uiBtnOpen
            // 
            this.uiBtnOpen.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnOpen.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnOpen.DisplayText = "Open";
            this.uiBtnOpen.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnOpen.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnOpen.Image = global::taskt.Properties.Resources.action_bar_open;
            this.uiBtnOpen.IsMouseOver = false;
            this.uiBtnOpen.Location = new System.Drawing.Point(122, 24);
            this.uiBtnOpen.Margin = new System.Windows.Forms.Padding(4);
            this.uiBtnOpen.Name = "uiBtnOpen";
            this.uiBtnOpen.Size = new System.Drawing.Size(60, 62);
            this.uiBtnOpen.TabIndex = 10;
            this.uiBtnOpen.TabStop = false;
            this.uiBtnOpen.Text = "Open";
            this.uiBtnOpen.Click += new System.EventHandler(this.uiBtnOpen_Click);
            // 
            // grpRecordRun
            // 
            this.grpRecordRun.BackColor = System.Drawing.Color.Transparent;
            this.grpRecordRun.Controls.Add(this.uiBtnRecordSequence);
            this.grpRecordRun.Controls.Add(this.uiBtnRunScript);
            this.grpRecordRun.Controls.Add(this.uiBtnScheduleManagement);
            this.grpRecordRun.Location = new System.Drawing.Point(602, 8);
            this.grpRecordRun.Margin = new System.Windows.Forms.Padding(4);
            this.grpRecordRun.Name = "grpRecordRun";
            this.grpRecordRun.Padding = new System.Windows.Forms.Padding(4);
            this.grpRecordRun.Size = new System.Drawing.Size(202, 91);
            this.grpRecordRun.TabIndex = 18;
            this.grpRecordRun.TabStop = false;
            this.grpRecordRun.Text = "Record and Run";
            this.grpRecordRun.TitleBackColor = System.Drawing.Color.Transparent;
            this.grpRecordRun.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpRecordRun.TitleForeColor = System.Drawing.Color.GhostWhite;
            this.grpRecordRun.TitleHatchStyle = System.Drawing.Drawing2D.HatchStyle.Horizontal;
            // 
            // uiBtnRecordSequence
            // 
            this.uiBtnRecordSequence.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnRecordSequence.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnRecordSequence.DisplayText = "Record";
            this.uiBtnRecordSequence.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnRecordSequence.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnRecordSequence.Image = global::taskt.Properties.Resources.action_bar_record;
            this.uiBtnRecordSequence.IsMouseOver = false;
            this.uiBtnRecordSequence.Location = new System.Drawing.Point(2, 22);
            this.uiBtnRecordSequence.Margin = new System.Windows.Forms.Padding(4);
            this.uiBtnRecordSequence.Name = "uiBtnRecordSequence";
            this.uiBtnRecordSequence.Size = new System.Drawing.Size(60, 62);
            this.uiBtnRecordSequence.TabIndex = 19;
            this.uiBtnRecordSequence.TabStop = false;
            this.uiBtnRecordSequence.Text = "Record";
            this.uiBtnRecordSequence.Click += new System.EventHandler(this.uiBtnRecordSequence_Click);
            // 
            // uiBtnRunScript
            // 
            this.uiBtnRunScript.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnRunScript.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnRunScript.DisplayText = "Run";
            this.uiBtnRunScript.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnRunScript.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnRunScript.Image = global::taskt.Properties.Resources.action_bar_run;
            this.uiBtnRunScript.IsMouseOver = false;
            this.uiBtnRunScript.Location = new System.Drawing.Point(130, 22);
            this.uiBtnRunScript.Margin = new System.Windows.Forms.Padding(4);
            this.uiBtnRunScript.Name = "uiBtnRunScript";
            this.uiBtnRunScript.Size = new System.Drawing.Size(60, 62);
            this.uiBtnRunScript.TabIndex = 12;
            this.uiBtnRunScript.TabStop = false;
            this.uiBtnRunScript.Text = "Run";
            this.uiBtnRunScript.Click += new System.EventHandler(this.uiBtnRunScript_Click);
            // 
            // uiBtnScheduleManagement
            // 
            this.uiBtnScheduleManagement.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnScheduleManagement.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnScheduleManagement.DisplayText = "Schedule";
            this.uiBtnScheduleManagement.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnScheduleManagement.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnScheduleManagement.Image = global::taskt.Properties.Resources.action_bar_schedule;
            this.uiBtnScheduleManagement.IsMouseOver = false;
            this.uiBtnScheduleManagement.Location = new System.Drawing.Point(64, 22);
            this.uiBtnScheduleManagement.Margin = new System.Windows.Forms.Padding(4);
            this.uiBtnScheduleManagement.Name = "uiBtnScheduleManagement";
            this.uiBtnScheduleManagement.Size = new System.Drawing.Size(65, 62);
            this.uiBtnScheduleManagement.TabIndex = 13;
            this.uiBtnScheduleManagement.TabStop = false;
            this.uiBtnScheduleManagement.Text = "Schedule";
            this.uiBtnScheduleManagement.Click += new System.EventHandler(this.uiBtnScheduleManagement_Click);
            // 
            // grpVariable
            // 
            this.grpVariable.BackColor = System.Drawing.Color.Transparent;
            this.grpVariable.Controls.Add(this.uiBtnClearAll);
            this.grpVariable.Controls.Add(this.uiBtnSettings);
            this.grpVariable.Controls.Add(this.uiBtnAddVariable);
            this.grpVariable.Location = new System.Drawing.Point(382, 8);
            this.grpVariable.Margin = new System.Windows.Forms.Padding(4);
            this.grpVariable.Name = "grpVariable";
            this.grpVariable.Padding = new System.Windows.Forms.Padding(4);
            this.grpVariable.Size = new System.Drawing.Size(214, 91);
            this.grpVariable.TabIndex = 17;
            this.grpVariable.TabStop = false;
            this.grpVariable.Text = "Variables and Settings";
            this.grpVariable.TitleBackColor = System.Drawing.Color.Transparent;
            this.grpVariable.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpVariable.TitleForeColor = System.Drawing.Color.GhostWhite;
            this.grpVariable.TitleHatchStyle = System.Drawing.Drawing2D.HatchStyle.Horizontal;
            // 
            // uiBtnClearAll
            // 
            this.uiBtnClearAll.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnClearAll.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnClearAll.DisplayText = "Clear";
            this.uiBtnClearAll.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnClearAll.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnClearAll.Image = global::taskt.Properties.Resources.action_bar_clear;
            this.uiBtnClearAll.IsMouseOver = false;
            this.uiBtnClearAll.Location = new System.Drawing.Point(135, 24);
            this.uiBtnClearAll.Margin = new System.Windows.Forms.Padding(4);
            this.uiBtnClearAll.Name = "uiBtnClearAll";
            this.uiBtnClearAll.Size = new System.Drawing.Size(60, 62);
            this.uiBtnClearAll.TabIndex = 14;
            this.uiBtnClearAll.TabStop = false;
            this.uiBtnClearAll.Text = "Clear";
            this.uiBtnClearAll.Click += new System.EventHandler(this.uiBtnClearAll_Click);
            // 
            // uiBtnSettings
            // 
            this.uiBtnSettings.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnSettings.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnSettings.DisplayText = "Settings";
            this.uiBtnSettings.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnSettings.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnSettings.Image = global::taskt.Properties.Resources.action_bar_options;
            this.uiBtnSettings.IsMouseOver = false;
            this.uiBtnSettings.Location = new System.Drawing.Point(74, 24);
            this.uiBtnSettings.Margin = new System.Windows.Forms.Padding(4);
            this.uiBtnSettings.Name = "uiBtnSettings";
            this.uiBtnSettings.Size = new System.Drawing.Size(60, 62);
            this.uiBtnSettings.TabIndex = 12;
            this.uiBtnSettings.TabStop = false;
            this.uiBtnSettings.Text = "Settings";
            this.uiBtnSettings.Click += new System.EventHandler(this.uiBtnSettings_Click);
            // 
            // uiBtnAddVariable
            // 
            this.uiBtnAddVariable.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnAddVariable.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnAddVariable.DisplayText = "Variables";
            this.uiBtnAddVariable.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnAddVariable.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnAddVariable.Image = global::taskt.Properties.Resources.action_bar_variable;
            this.uiBtnAddVariable.IsMouseOver = false;
            this.uiBtnAddVariable.Location = new System.Drawing.Point(8, 24);
            this.uiBtnAddVariable.Margin = new System.Windows.Forms.Padding(4);
            this.uiBtnAddVariable.Name = "uiBtnAddVariable";
            this.uiBtnAddVariable.Size = new System.Drawing.Size(65, 62);
            this.uiBtnAddVariable.TabIndex = 13;
            this.uiBtnAddVariable.TabStop = false;
            this.uiBtnAddVariable.Text = "Variables";
            this.uiBtnAddVariable.Click += new System.EventHandler(this.uiBtnAddVariable_Click);
            // 
            // pnlStatus
            // 
            this.pnlStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(59)))), ((int)(((byte)(59)))));
            this.tlpControls.SetColumnSpan(this.pnlStatus, 3);
            this.pnlStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlStatus.Location = new System.Drawing.Point(0, 657);
            this.pnlStatus.Margin = new System.Windows.Forms.Padding(0);
            this.pnlStatus.Name = "pnlStatus";
            this.pnlStatus.Size = new System.Drawing.Size(1328, 39);
            this.pnlStatus.TabIndex = 3;
            this.pnlStatus.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlStatus_Paint);
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.tlpControls.SetColumnSpan(this.pnlHeader, 3);
            this.pnlHeader.Controls.Add(this.pnlMain);
            this.pnlHeader.Controls.Add(this.lblCoordinatorInfo);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Margin = new System.Windows.Forms.Padding(0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1328, 51);
            this.pnlHeader.TabIndex = 2;
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.lblMainLogo);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Margin = new System.Windows.Forms.Padding(0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(1328, 51);
            this.pnlMain.TabIndex = 2;
            theme1.BgGradientEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(50)))), ((int)(((byte)(178)))));
            theme1.BgGradientStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(136)))), ((int)(((byte)(204)))));
            this.pnlMain.Theme = theme1;
            // 
            // lblMainLogo
            // 
            this.lblMainLogo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblMainLogo.AutoSize = true;
            this.lblMainLogo.BackColor = System.Drawing.Color.Transparent;
            this.lblMainLogo.Font = new System.Drawing.Font("Segoe UI Light", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMainLogo.ForeColor = System.Drawing.Color.AliceBlue;
            this.lblMainLogo.Location = new System.Drawing.Point(2, -8);
            this.lblMainLogo.Margin = new System.Windows.Forms.Padding(0);
            this.lblMainLogo.Name = "lblMainLogo";
            this.lblMainLogo.Size = new System.Drawing.Size(101, 54);
            this.lblMainLogo.TabIndex = 0;
            this.lblMainLogo.Text = "taskt";
            this.lblMainLogo.Click += new System.EventHandler(this.lblMainLogo_Click);
            // 
            // lblCoordinatorInfo
            // 
            this.lblCoordinatorInfo.AutoSize = true;
            this.lblCoordinatorInfo.BackColor = System.Drawing.Color.Transparent;
            this.lblCoordinatorInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCoordinatorInfo.ForeColor = System.Drawing.Color.White;
            this.lblCoordinatorInfo.Location = new System.Drawing.Point(285, 20);
            this.lblCoordinatorInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCoordinatorInfo.Name = "lblCoordinatorInfo";
            this.lblCoordinatorInfo.Size = new System.Drawing.Size(0, 25);
            this.lblCoordinatorInfo.TabIndex = 3;
            this.lblCoordinatorInfo.Visible = false;
            // 
            // splitContainer1
            // 
            this.tlpControls.SetColumnSpan(this.splitContainer1, 3);
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(4, 200);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Panel1.Controls.Add(this.uiPaneTabs);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Panel2.Controls.Add(this.uiScriptTabControl);
            this.splitContainer1.Size = new System.Drawing.Size(1320, 453);
            this.splitContainer1.SplitterDistance = 238;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 4;
            // 
            // uiPaneTabs
            // 
            this.uiPaneTabs.Controls.Add(this.tpProject);
            this.uiPaneTabs.Controls.Add(this.tpCommands);
            this.uiPaneTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiPaneTabs.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.uiPaneTabs.Location = new System.Drawing.Point(0, 0);
            this.uiPaneTabs.Margin = new System.Windows.Forms.Padding(4);
            this.uiPaneTabs.Name = "uiPaneTabs";
            this.uiPaneTabs.SelectedIndex = 0;
            this.uiPaneTabs.Size = new System.Drawing.Size(238, 453);
            this.uiPaneTabs.TabIndex = 26;
            // 
            // tpProject
            // 
            this.tpProject.Controls.Add(this.tvProject);
            this.tpProject.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.tpProject.ForeColor = System.Drawing.Color.SteelBlue;
            this.tpProject.Location = new System.Drawing.Point(4, 32);
            this.tpProject.Name = "tpProject";
            this.tpProject.Padding = new System.Windows.Forms.Padding(3);
            this.tpProject.Size = new System.Drawing.Size(230, 417);
            this.tpProject.TabIndex = 5;
            this.tpProject.Text = "Project";
            this.tpProject.UseVisualStyleBackColor = true;
            // 
            // tvProject
            // 
            this.tvProject.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(59)))), ((int)(((byte)(59)))));
            this.tvProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvProject.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.tvProject.ForeColor = System.Drawing.Color.White;
            this.tvProject.ImageIndex = 0;
            this.tvProject.ImageList = this.imgListProjectPane;
            this.tvProject.Location = new System.Drawing.Point(3, 3);
            this.tvProject.Name = "tvProject";
            this.tvProject.SelectedImageIndex = 0;
            this.tvProject.ShowLines = false;
            this.tvProject.Size = new System.Drawing.Size(224, 411);
            this.tvProject.TabIndex = 0;
            this.tvProject.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvProject_BeforeExpand);
            this.tvProject.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvProject_NodeMouseClick);
            this.tvProject.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvProject_DoubleClick);
            this.tvProject.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tvProject_KeyDown);
            // 
            // imgListProjectPane
            // 
            this.imgListProjectPane.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgListProjectPane.ImageStream")));
            this.imgListProjectPane.TransparentColor = System.Drawing.Color.Transparent;
            this.imgListProjectPane.Images.SetKeyName(0, "ProjectFolderIcon2.png");
            this.imgListProjectPane.Images.SetKeyName(1, "ProjectScriptFileIcon2.png");
            this.imgListProjectPane.Images.SetKeyName(2, "ProjectGenericFileIcon2.png");
            this.imgListProjectPane.Images.SetKeyName(3, "microsoft-excel-icon.png");
            this.imgListProjectPane.Images.SetKeyName(4, "microsoft-word-icon.png");
            this.imgListProjectPane.Images.SetKeyName(5, "pdf-icon.png");
            // 
            // tpCommands
            // 
            this.tpCommands.Controls.Add(this.tvCommands);
            this.tpCommands.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.tpCommands.ForeColor = System.Drawing.Color.SteelBlue;
            this.tpCommands.Location = new System.Drawing.Point(4, 32);
            this.tpCommands.Name = "tpCommands";
            this.tpCommands.Padding = new System.Windows.Forms.Padding(3);
            this.tpCommands.Size = new System.Drawing.Size(230, 417);
            this.tpCommands.TabIndex = 4;
            this.tpCommands.Text = "Commands";
            this.tpCommands.UseVisualStyleBackColor = true;
            // 
            // tvCommands
            // 
            this.tvCommands.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(59)))), ((int)(((byte)(59)))));
            this.tvCommands.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tvCommands.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvCommands.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.tvCommands.ForeColor = System.Drawing.Color.White;
            this.tvCommands.Location = new System.Drawing.Point(3, 3);
            this.tvCommands.Margin = new System.Windows.Forms.Padding(4);
            this.tvCommands.Name = "tvCommands";
            this.tvCommands.ShowLines = false;
            this.tvCommands.Size = new System.Drawing.Size(224, 411);
            this.tvCommands.TabIndex = 9;
            this.tvCommands.DoubleClick += new System.EventHandler(this.tvCommands_DoubleClick);
            this.tvCommands.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tvCommands_KeyPress);
            // 
            // uiScriptTabControl
            // 
            this.uiScriptTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiScriptTabControl.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.uiScriptTabControl.Location = new System.Drawing.Point(0, 0);
            this.uiScriptTabControl.Name = "uiScriptTabControl";
            this.uiScriptTabControl.SelectedIndex = 0;
            this.uiScriptTabControl.ShowToolTips = true;
            this.uiScriptTabControl.Size = new System.Drawing.Size(1077, 453);
            this.uiScriptTabControl.TabIndex = 3;
            this.uiScriptTabControl.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.uiScriptTabControl_DrawItem);
            this.uiScriptTabControl.SelectedIndexChanged += new System.EventHandler(this.uiScriptTabControl_SelectedIndexChanged);
            this.uiScriptTabControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.uiScriptTabControl_MouseClick);
            // 
            // pnlCommandHelper
            // 
            this.pnlCommandHelper.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(59)))), ((int)(((byte)(59)))));
            this.pnlCommandHelper.Controls.Add(this.flwRecentFiles);
            this.pnlCommandHelper.Controls.Add(this.lblFilesMissing);
            this.pnlCommandHelper.Controls.Add(this.pbRecentFiles);
            this.pnlCommandHelper.Controls.Add(this.pbLinks);
            this.pnlCommandHelper.Controls.Add(this.pbTasktLogo);
            this.pnlCommandHelper.Controls.Add(this.lblRecentFiles);
            this.pnlCommandHelper.Controls.Add(this.lnkGitWiki);
            this.pnlCommandHelper.Controls.Add(this.lnkGitIssue);
            this.pnlCommandHelper.Controls.Add(this.lnkGitLatestReleases);
            this.pnlCommandHelper.Controls.Add(this.lnkGitProject);
            this.pnlCommandHelper.Controls.Add(this.lblWelcomeToTaskt);
            this.pnlCommandHelper.Controls.Add(this.lblWelcomeDescription);
            this.pnlCommandHelper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCommandHelper.Location = new System.Drawing.Point(3, 3);
            this.pnlCommandHelper.Margin = new System.Windows.Forms.Padding(4);
            this.pnlCommandHelper.Name = "pnlCommandHelper";
            this.pnlCommandHelper.Size = new System.Drawing.Size(1063, 411);
            this.pnlCommandHelper.TabIndex = 7;
            // 
            // flwRecentFiles
            // 
            this.flwRecentFiles.AutoScroll = true;
            this.flwRecentFiles.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flwRecentFiles.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flwRecentFiles.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.flwRecentFiles.Location = new System.Drawing.Point(145, 291);
            this.flwRecentFiles.Margin = new System.Windows.Forms.Padding(4);
            this.flwRecentFiles.Name = "flwRecentFiles";
            this.flwRecentFiles.Size = new System.Drawing.Size(496, 180);
            this.flwRecentFiles.TabIndex = 12;
            this.flwRecentFiles.WrapContents = false;
            // 
            // lblFilesMissing
            // 
            this.lblFilesMissing.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFilesMissing.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblFilesMissing.Location = new System.Drawing.Point(144, 289);
            this.lblFilesMissing.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFilesMissing.Name = "lblFilesMissing";
            this.lblFilesMissing.Size = new System.Drawing.Size(448, 80);
            this.lblFilesMissing.TabIndex = 16;
            this.lblFilesMissing.Text = "there were no script files found in your script directory.";
            this.lblFilesMissing.Visible = false;
            // 
            // pbRecentFiles
            // 
            this.pbRecentFiles.Image = global::taskt.Properties.Resources.recent_files;
            this.pbRecentFiles.Location = new System.Drawing.Point(15, 262);
            this.pbRecentFiles.Margin = new System.Windows.Forms.Padding(4);
            this.pbRecentFiles.Name = "pbRecentFiles";
            this.pbRecentFiles.Size = new System.Drawing.Size(105, 105);
            this.pbRecentFiles.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbRecentFiles.TabIndex = 15;
            this.pbRecentFiles.TabStop = false;
            // 
            // pbLinks
            // 
            this.pbLinks.Image = global::taskt.Properties.Resources.links_header;
            this.pbLinks.Location = new System.Drawing.Point(15, 135);
            this.pbLinks.Margin = new System.Windows.Forms.Padding(4);
            this.pbLinks.Name = "pbLinks";
            this.pbLinks.Size = new System.Drawing.Size(105, 105);
            this.pbLinks.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbLinks.TabIndex = 14;
            this.pbLinks.TabStop = false;
            // 
            // pbTasktLogo
            // 
            this.pbTasktLogo.Image = global::taskt.Properties.Resources.item_header;
            this.pbTasktLogo.Location = new System.Drawing.Point(15, 10);
            this.pbTasktLogo.Margin = new System.Windows.Forms.Padding(4);
            this.pbTasktLogo.Name = "pbTasktLogo";
            this.pbTasktLogo.Size = new System.Drawing.Size(105, 105);
            this.pbTasktLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbTasktLogo.TabIndex = 13;
            this.pbTasktLogo.TabStop = false;
            // 
            // lblRecentFiles
            // 
            this.lblRecentFiles.AutoSize = true;
            this.lblRecentFiles.Font = new System.Drawing.Font("Segoe UI Semilight", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecentFiles.ForeColor = System.Drawing.Color.AliceBlue;
            this.lblRecentFiles.Location = new System.Drawing.Point(138, 251);
            this.lblRecentFiles.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRecentFiles.Name = "lblRecentFiles";
            this.lblRecentFiles.Size = new System.Drawing.Size(153, 37);
            this.lblRecentFiles.TabIndex = 8;
            this.lblRecentFiles.Text = "Recent Files";
            // 
            // lnkGitWiki
            // 
            this.lnkGitWiki.AutoSize = true;
            this.lnkGitWiki.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkGitWiki.ForeColor = System.Drawing.Color.White;
            this.lnkGitWiki.LinkColor = System.Drawing.Color.AliceBlue;
            this.lnkGitWiki.Location = new System.Drawing.Point(145, 211);
            this.lnkGitWiki.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lnkGitWiki.Name = "lnkGitWiki";
            this.lnkGitWiki.Size = new System.Drawing.Size(219, 25);
            this.lnkGitWiki.TabIndex = 6;
            this.lnkGitWiki.TabStop = true;
            this.lnkGitWiki.Text = "View Documentation/Wiki";
            this.lnkGitWiki.VisitedLinkColor = System.Drawing.Color.LightSteelBlue;
            this.lnkGitWiki.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkGitWiki_LinkClicked);
            // 
            // lnkGitIssue
            // 
            this.lnkGitIssue.AutoSize = true;
            this.lnkGitIssue.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkGitIssue.ForeColor = System.Drawing.Color.White;
            this.lnkGitIssue.LinkColor = System.Drawing.Color.AliceBlue;
            this.lnkGitIssue.Location = new System.Drawing.Point(145, 186);
            this.lnkGitIssue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lnkGitIssue.Name = "lnkGitIssue";
            this.lnkGitIssue.Size = new System.Drawing.Size(325, 25);
            this.lnkGitIssue.TabIndex = 5;
            this.lnkGitIssue.TabStop = true;
            this.lnkGitIssue.Text = "Request Enhancement or Report a bug";
            this.lnkGitIssue.VisitedLinkColor = System.Drawing.Color.LightSteelBlue;
            this.lnkGitIssue.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkGitIssue_LinkClicked);
            // 
            // lnkGitLatestReleases
            // 
            this.lnkGitLatestReleases.AutoSize = true;
            this.lnkGitLatestReleases.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkGitLatestReleases.ForeColor = System.Drawing.Color.White;
            this.lnkGitLatestReleases.LinkColor = System.Drawing.Color.AliceBlue;
            this.lnkGitLatestReleases.Location = new System.Drawing.Point(145, 161);
            this.lnkGitLatestReleases.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lnkGitLatestReleases.Name = "lnkGitLatestReleases";
            this.lnkGitLatestReleases.Size = new System.Drawing.Size(175, 25);
            this.lnkGitLatestReleases.TabIndex = 4;
            this.lnkGitLatestReleases.TabStop = true;
            this.lnkGitLatestReleases.Text = "View Latest Releases";
            this.lnkGitLatestReleases.VisitedLinkColor = System.Drawing.Color.LightSteelBlue;
            this.lnkGitLatestReleases.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkGitLatestReleases_LinkClicked);
            // 
            // lnkGitProject
            // 
            this.lnkGitProject.AutoSize = true;
            this.lnkGitProject.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkGitProject.ForeColor = System.Drawing.Color.White;
            this.lnkGitProject.LinkColor = System.Drawing.Color.AliceBlue;
            this.lnkGitProject.Location = new System.Drawing.Point(145, 136);
            this.lnkGitProject.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lnkGitProject.Name = "lnkGitProject";
            this.lnkGitProject.Size = new System.Drawing.Size(198, 25);
            this.lnkGitProject.TabIndex = 3;
            this.lnkGitProject.TabStop = true;
            this.lnkGitProject.Text = "View Project on GitHub";
            this.lnkGitProject.VisitedLinkColor = System.Drawing.Color.LightSteelBlue;
            this.lnkGitProject.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkGitProject_LinkClicked);
            // 
            // lblWelcomeToTaskt
            // 
            this.lblWelcomeToTaskt.AutoSize = true;
            this.lblWelcomeToTaskt.Font = new System.Drawing.Font("Segoe UI Semilight", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWelcomeToTaskt.ForeColor = System.Drawing.Color.AliceBlue;
            this.lblWelcomeToTaskt.Location = new System.Drawing.Point(139, 5);
            this.lblWelcomeToTaskt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWelcomeToTaskt.Name = "lblWelcomeToTaskt";
            this.lblWelcomeToTaskt.Size = new System.Drawing.Size(227, 37);
            this.lblWelcomeToTaskt.TabIndex = 2;
            this.lblWelcomeToTaskt.Text = "Welcome to taskt!";
            // 
            // lblWelcomeDescription
            // 
            this.lblWelcomeDescription.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWelcomeDescription.ForeColor = System.Drawing.Color.White;
            this.lblWelcomeDescription.Location = new System.Drawing.Point(142, 40);
            this.lblWelcomeDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWelcomeDescription.Name = "lblWelcomeDescription";
            this.lblWelcomeDescription.Size = new System.Drawing.Size(350, 94);
            this.lblWelcomeDescription.TabIndex = 1;
            this.lblWelcomeDescription.Text = "Start building automation by double-clicking a command from the list to the left." +
    "";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Width = 20;
            // 
            // clmCommand
            // 
            this.clmCommand.Text = "Script Commands";
            this.clmCommand.Width = 800;
            // 
            // pnlDivider
            // 
            this.pnlDivider.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(59)))), ((int)(((byte)(59)))));
            this.tlpControls.SetColumnSpan(this.pnlDivider, 4);
            this.pnlDivider.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDivider.Location = new System.Drawing.Point(0, 190);
            this.pnlDivider.Margin = new System.Windows.Forms.Padding(0);
            this.pnlDivider.Name = "pnlDivider";
            this.pnlDivider.Size = new System.Drawing.Size(1328, 6);
            this.pnlDivider.TabIndex = 13;
            // 
            // msTasktMenu
            // 
            this.msTasktMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.tlpControls.SetColumnSpan(this.msTasktMenu, 3);
            this.msTasktMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.msTasktMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.msTasktMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileActionsToolStripMenuItem,
            this.modifyToolStripMenuItem,
            this.scriptActionsToolStripMenuItem,
            this.saveAndRunToolStripMenuItem,
            this.tsSearchBox,
            this.tsSearchButton,
            this.tsSearchResult});
            this.msTasktMenu.Location = new System.Drawing.Point(0, 51);
            this.msTasktMenu.Name = "msTasktMenu";
            this.msTasktMenu.Size = new System.Drawing.Size(1328, 38);
            this.msTasktMenu.TabIndex = 1;
            this.msTasktMenu.Text = "menuStrip1";
            // 
            // fileActionsToolStripMenuItem
            // 
            this.fileActionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addProjectToolStripMenuItem,
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.importFileToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.restartApplicationToolStripMenuItem,
            this.closeApplicationToolStripMenuItem});
            this.fileActionsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.fileActionsToolStripMenuItem.Image = global::taskt.Properties.Resources.action_bar_new;
            this.fileActionsToolStripMenuItem.Name = "fileActionsToolStripMenuItem";
            this.fileActionsToolStripMenuItem.Size = new System.Drawing.Size(119, 34);
            this.fileActionsToolStripMenuItem.Text = "File Actions";
            // 
            // addProjectToolStripMenuItem
            // 
            this.addProjectToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.addProjectToolStripMenuItem.Image = global::taskt.Properties.Resources.action_bar_run;
            this.addProjectToolStripMenuItem.Name = "addProjectToolStripMenuItem";
            this.addProjectToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.addProjectToolStripMenuItem.Text = "Project Manager";
            this.addProjectToolStripMenuItem.Click += new System.EventHandler(this.addProjectToolStripMenuItem_Click);
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.newToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.newToolStripMenuItem.Image = global::taskt.Properties.Resources.action_bar_new;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.newToolStripMenuItem.Text = "New File";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.openToolStripMenuItem.Image = global::taskt.Properties.Resources.action_bar_open;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.openToolStripMenuItem.Text = "Open Existing File";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // importFileToolStripMenuItem
            // 
            this.importFileToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.importFileToolStripMenuItem.Image = global::taskt.Properties.Resources.action_bar_import;
            this.importFileToolStripMenuItem.Name = "importFileToolStripMenuItem";
            this.importFileToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.importFileToolStripMenuItem.Text = "Import File";
            this.importFileToolStripMenuItem.Click += new System.EventHandler(this.importFileToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.saveToolStripMenuItem.Image = global::taskt.Properties.Resources.action_bar_save;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.saveAsToolStripMenuItem.Image = global::taskt.Properties.Resources.action_bar_saveas;
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // restartApplicationToolStripMenuItem
            // 
            this.restartApplicationToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.restartApplicationToolStripMenuItem.Image = global::taskt.Properties.Resources.action_bar_restart;
            this.restartApplicationToolStripMenuItem.Name = "restartApplicationToolStripMenuItem";
            this.restartApplicationToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.restartApplicationToolStripMenuItem.Text = "Restart Application";
            this.restartApplicationToolStripMenuItem.Click += new System.EventHandler(this.restartApplicationToolStripMenuItem_Click);
            // 
            // closeApplicationToolStripMenuItem
            // 
            this.closeApplicationToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.closeApplicationToolStripMenuItem.Image = global::taskt.Properties.Resources.action_bar_close;
            this.closeApplicationToolStripMenuItem.Name = "closeApplicationToolStripMenuItem";
            this.closeApplicationToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.closeApplicationToolStripMenuItem.Text = "Close Application";
            this.closeApplicationToolStripMenuItem.Click += new System.EventHandler(this.closeApplicationToolStripMenuItem_Click);
            // 
            // modifyToolStripMenuItem
            // 
            this.modifyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.variablesToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.showSearchBarToolStripMenuItem,
            this.aboutTasktToolStripMenuItem});
            this.modifyToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.modifyToolStripMenuItem.Image = global::taskt.Properties.Resources.action_bar_options;
            this.modifyToolStripMenuItem.Name = "modifyToolStripMenuItem";
            this.modifyToolStripMenuItem.Size = new System.Drawing.Size(95, 34);
            this.modifyToolStripMenuItem.Text = "Options";
            // 
            // variablesToolStripMenuItem
            // 
            this.variablesToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.variablesToolStripMenuItem.Image = global::taskt.Properties.Resources.action_bar_variable;
            this.variablesToolStripMenuItem.Name = "variablesToolStripMenuItem";
            this.variablesToolStripMenuItem.Size = new System.Drawing.Size(209, 26);
            this.variablesToolStripMenuItem.Text = "Variable Manager";
            this.variablesToolStripMenuItem.Click += new System.EventHandler(this.variablesToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.settingsToolStripMenuItem.Image = global::taskt.Properties.Resources.action_bar_options;
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(209, 26);
            this.settingsToolStripMenuItem.Text = "Settings Manager";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // showSearchBarToolStripMenuItem
            // 
            this.showSearchBarToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.showSearchBarToolStripMenuItem.Image = global::taskt.Properties.Resources.command_search;
            this.showSearchBarToolStripMenuItem.Name = "showSearchBarToolStripMenuItem";
            this.showSearchBarToolStripMenuItem.Size = new System.Drawing.Size(209, 26);
            this.showSearchBarToolStripMenuItem.Text = "Show Search Bar";
            this.showSearchBarToolStripMenuItem.Click += new System.EventHandler(this.showSearchBarToolStripMenuItem_Click);
            // 
            // aboutTasktToolStripMenuItem
            // 
            this.aboutTasktToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.aboutTasktToolStripMenuItem.Image = global::taskt.Properties.Resources.item_header;
            this.aboutTasktToolStripMenuItem.Name = "aboutTasktToolStripMenuItem";
            this.aboutTasktToolStripMenuItem.Size = new System.Drawing.Size(209, 26);
            this.aboutTasktToolStripMenuItem.Text = "About taskt";
            this.aboutTasktToolStripMenuItem.Click += new System.EventHandler(this.aboutTasktToolStripMenuItem_Click);
            // 
            // scriptActionsToolStripMenuItem
            // 
            this.scriptActionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.recordToolStripMenuItem,
            this.scheduleToolStripMenuItem,
            this.runToolStripMenuItem});
            this.scriptActionsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.scriptActionsToolStripMenuItem.Image = global::taskt.Properties.Resources.action_bar_record;
            this.scriptActionsToolStripMenuItem.Name = "scriptActionsToolStripMenuItem";
            this.scriptActionsToolStripMenuItem.Size = new System.Drawing.Size(134, 34);
            this.scriptActionsToolStripMenuItem.Text = "Script Actions";
            // 
            // recordToolStripMenuItem
            // 
            this.recordToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.recordToolStripMenuItem.Image = global::taskt.Properties.Resources.action_bar_record;
            this.recordToolStripMenuItem.Name = "recordToolStripMenuItem";
            this.recordToolStripMenuItem.Size = new System.Drawing.Size(152, 26);
            this.recordToolStripMenuItem.Text = "Record";
            this.recordToolStripMenuItem.Click += new System.EventHandler(this.recordToolStripMenuItem_Click);
            // 
            // scheduleToolStripMenuItem
            // 
            this.scheduleToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.scheduleToolStripMenuItem.Image = global::taskt.Properties.Resources.action_bar_schedule;
            this.scheduleToolStripMenuItem.Name = "scheduleToolStripMenuItem";
            this.scheduleToolStripMenuItem.Size = new System.Drawing.Size(152, 26);
            this.scheduleToolStripMenuItem.Text = "Schedule";
            this.scheduleToolStripMenuItem.Click += new System.EventHandler(this.scheduleToolStripMenuItem_Click);
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.runToolStripMenuItem.Image = global::taskt.Properties.Resources.action_bar_run;
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.Size = new System.Drawing.Size(152, 26);
            this.runToolStripMenuItem.Text = "Run";
            this.runToolStripMenuItem.Click += new System.EventHandler(this.runToolStripMenuItem_Click);
            // 
            // saveAndRunToolStripMenuItem
            // 
            this.saveAndRunToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.saveAndRunToolStripMenuItem.Image = global::taskt.Properties.Resources.action_bar_run;
            this.saveAndRunToolStripMenuItem.Name = "saveAndRunToolStripMenuItem";
            this.saveAndRunToolStripMenuItem.Size = new System.Drawing.Size(132, 34);
            this.saveAndRunToolStripMenuItem.Text = "Save and Run";
            this.saveAndRunToolStripMenuItem.Click += new System.EventHandler(this.saveAndRunToolStripMenuItem_Click);
            // 
            // tsSearchBox
            // 
            this.tsSearchBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tsSearchBox.Name = "tsSearchBox";
            this.tsSearchBox.Size = new System.Drawing.Size(124, 34);
            this.tsSearchBox.Visible = false;
            this.tsSearchBox.TextChanged += new System.EventHandler(this.txtCommandSearch_TextChanged);
            // 
            // tsSearchButton
            // 
            this.tsSearchButton.ForeColor = System.Drawing.Color.White;
            this.tsSearchButton.Image = global::taskt.Properties.Resources.command_search;
            this.tsSearchButton.Name = "tsSearchButton";
            this.tsSearchButton.Size = new System.Drawing.Size(34, 34);
            this.tsSearchButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tsSearchButton.Visible = false;
            this.tsSearchButton.Click += new System.EventHandler(this.pbSearch_Click);
            // 
            // tsSearchResult
            // 
            this.tsSearchResult.ForeColor = System.Drawing.Color.White;
            this.tsSearchResult.Name = "tsSearchResult";
            this.tsSearchResult.Size = new System.Drawing.Size(14, 34);
            this.tsSearchResult.Visible = false;
            // 
            // tlpControls
            // 
            this.tlpControls.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(59)))), ((int)(((byte)(59)))));
            this.tlpControls.ColumnCount = 3;
            this.tlpControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 319F));
            this.tlpControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 655F));
            this.tlpControls.Controls.Add(this.msTasktMenu, 0, 1);
            this.tlpControls.Controls.Add(this.pnlDivider, 0, 3);
            this.tlpControls.Controls.Add(this.splitContainer1, 0, 4);
            this.tlpControls.Controls.Add(this.pnlHeader, 0, 0);
            this.tlpControls.Controls.Add(this.pnlStatus, 0, 5);
            this.tlpControls.Controls.Add(this.pnlControlContainer, 0, 2);
            this.tlpControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpControls.Location = new System.Drawing.Point(0, 0);
            this.tlpControls.Margin = new System.Windows.Forms.Padding(0);
            this.tlpControls.Name = "tlpControls";
            this.tlpControls.RowCount = 6;
            this.tlpControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 51F));
            this.tlpControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tlpControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 101F));
            this.tlpControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.tlpControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tlpControls.Size = new System.Drawing.Size(1328, 696);
            this.tlpControls.TabIndex = 2;
            // 
            // cmsProjectFileActions
            // 
            this.cmsProjectFileActions.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.cmsProjectFileActions.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsProjectFileActions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCopyFile,
            this.tsmiDeleteFile,
            this.tsmiRenameFile});
            this.cmsProjectFileActions.Name = "cmsProjectFileActions";
            this.cmsProjectFileActions.Size = new System.Drawing.Size(148, 88);
            // 
            // tsmiCopyFile
            // 
            this.tsmiCopyFile.Image = ((System.Drawing.Image)(resources.GetObject("tsmiCopyFile.Image")));
            this.tsmiCopyFile.Name = "tsmiCopyFile";
            this.tsmiCopyFile.Size = new System.Drawing.Size(147, 28);
            this.tsmiCopyFile.Text = "Copy";
            this.tsmiCopyFile.Click += new System.EventHandler(this.tsmiCopyFile_Click);
            // 
            // tsmiDeleteFile
            // 
            this.tsmiDeleteFile.Image = ((System.Drawing.Image)(resources.GetObject("tsmiDeleteFile.Image")));
            this.tsmiDeleteFile.Name = "tsmiDeleteFile";
            this.tsmiDeleteFile.Size = new System.Drawing.Size(147, 28);
            this.tsmiDeleteFile.Text = "Delete";
            this.tsmiDeleteFile.Click += new System.EventHandler(this.tsmiDeleteFile_Click);
            // 
            // tsmiRenameFile
            // 
            this.tsmiRenameFile.Image = ((System.Drawing.Image)(resources.GetObject("tsmiRenameFile.Image")));
            this.tsmiRenameFile.Name = "tsmiRenameFile";
            this.tsmiRenameFile.Size = new System.Drawing.Size(147, 28);
            this.tsmiRenameFile.Text = "Rename";
            this.tsmiRenameFile.Click += new System.EventHandler(this.tsmiRenameFile_Click);
            // 
            // imgListTabControl
            // 
            this.imgListTabControl.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgListTabControl.ImageStream")));
            this.imgListTabControl.TransparentColor = System.Drawing.Color.Transparent;
            this.imgListTabControl.Images.SetKeyName(0, "close-button.png");
            // 
            // cmsScriptTabActions
            // 
            this.cmsScriptTabActions.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.cmsScriptTabActions.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsScriptTabActions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCloseTab,
            this.tsmiCloseAllButThis});
            this.cmsScriptTabActions.Name = "cmsScriptTabActions";
            this.cmsScriptTabActions.Size = new System.Drawing.Size(212, 60);
            // 
            // tsmiCloseTab
            // 
            this.tsmiCloseTab.Name = "tsmiCloseTab";
            this.tsmiCloseTab.Size = new System.Drawing.Size(211, 28);
            this.tsmiCloseTab.Text = "Close Tab";
            this.tsmiCloseTab.Click += new System.EventHandler(this.tsmiCloseTab_Click);
            // 
            // tsmiCloseAllButThis
            // 
            this.tsmiCloseAllButThis.Name = "tsmiCloseAllButThis";
            this.tsmiCloseAllButThis.Size = new System.Drawing.Size(211, 28);
            this.tsmiCloseAllButThis.Text = "Close All But This";
            this.tsmiCloseAllButThis.Click += new System.EventHandler(this.tsmiCloseAllButThis_Click);
            // 
            // frmScriptBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1328, 696);
            this.Controls.Add(this.tlpControls);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.msTasktMenu;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmScriptBuilder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "taskt";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmScriptBuilder_FormClosing);
            this.Load += new System.EventHandler(this.frmScriptBuilder_Load);
            this.Shown += new System.EventHandler(this.frmScriptBuilder_Shown);
            this.SizeChanged += new System.EventHandler(this.frmScriptBuilder_SizeChanged);
            this.Resize += new System.EventHandler(this.frmScriptBuilder_Resize);
            this.cmsProjectFolderActions.ResumeLayout(false);
            this.cmsScriptActions.ResumeLayout(false);
            this.pnlControlContainer.ResumeLayout(false);
            this.grpSearch.ResumeLayout(false);
            this.grpSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSearch)).EndInit();
            this.grpSaveClose.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnSequenceImport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnKeep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnClose)).EndInit();
            this.grpFileActions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnProject)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnImport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnSaveAs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnNew)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOpen)).EndInit();
            this.grpRecordRun.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnRecordSequence)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnRunScript)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnScheduleManagement)).EndInit();
            this.grpVariable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnClearAll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnSettings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnAddVariable)).EndInit();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.uiPaneTabs.ResumeLayout(false);
            this.tpProject.ResumeLayout(false);
            this.tpCommands.ResumeLayout(false);
            this.pnlCommandHelper.ResumeLayout(false);
            this.pnlCommandHelper.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbRecentFiles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLinks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTasktLogo)).EndInit();
            this.msTasktMenu.ResumeLayout(false);
            this.msTasktMenu.PerformLayout();
            this.tlpControls.ResumeLayout(false);
            this.tlpControls.PerformLayout();
            this.cmsProjectFileActions.ResumeLayout(false);
            this.cmsScriptTabActions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer tmrNotify;
        private System.Windows.Forms.ContextMenuStrip cmsScriptActions;
        private System.Windows.Forms.ToolStripMenuItem enableSelectedCodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disableSelectedCodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pauseBeforeExecutionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copySelectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteSelectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cutSelectedActionssToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveToParentToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon notifyTray;
        private System.Windows.Forms.ToolStripMenuItem viewCodeToolStripMenuItem;
        private ContextMenuStrip cmsProjectFolderActions;
        private ToolStripMenuItem tsmiDeleteFolder;
        private ToolStripMenuItem tsmiCopyFolder;
        private ToolStripMenuItem tsmiRenameFolder;
        private Panel pnlControlContainer;
        private TableLayoutPanel tlpControls;
        private CustomControls.UIMenuStrip msTasktMenu;
        private ToolStripMenuItem fileActionsToolStripMenuItem;
        private ToolStripMenuItem addProjectToolStripMenuItem;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem importFileToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem restartApplicationToolStripMenuItem;
        private ToolStripMenuItem closeApplicationToolStripMenuItem;
        private ToolStripMenuItem modifyToolStripMenuItem;
        private ToolStripMenuItem variablesToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem showSearchBarToolStripMenuItem;
        private ToolStripMenuItem aboutTasktToolStripMenuItem;
        private ToolStripMenuItem scriptActionsToolStripMenuItem;
        private ToolStripMenuItem recordToolStripMenuItem;
        private ToolStripMenuItem scheduleToolStripMenuItem;
        private ToolStripMenuItem runToolStripMenuItem;
        private ToolStripMenuItem saveAndRunToolStripMenuItem;
        private ToolStripTextBox tsSearchBox;
        private ToolStripMenuItem tsSearchButton;
        private ToolStripMenuItem tsSearchResult;
        private Panel pnlDivider;
        private CustomControls.UISplitContainer splitContainer1;
        private CustomControls.UITabControl uiPaneTabs;
        private TabPage tpProject;
        private CustomControls.UITreeView tvProject;
        private TabPage tpCommands;
        private CustomControls.UITreeView tvCommands;
        private Panel pnlHeader;
        private CustomControls.UIPanel pnlMain;
        private Label lblMainLogo;
        private Label lblCoordinatorInfo;
        private Panel pnlStatus;
        private CustomControls.UIGroupBox grpSearch;
        private PictureBox pbSearch;
        private Label lblCurrentlyViewing;
        private Label lblTotalResults;
        private TextBox txtCommandSearch;
        private CustomControls.UIGroupBox grpSaveClose;
        private CustomControls.UIPictureButton btnSequenceImport;
        private CustomControls.UIPictureButton uiBtnKeep;
        private CustomControls.UIPictureButton uiBtnClose;
        private CustomControls.UIGroupBox grpFileActions;
        private CustomControls.UIPictureButton uiBtnProject;
        private CustomControls.UIPictureButton uiBtnImport;
        private CustomControls.UIPictureButton uiBtnSaveAs;
        private CustomControls.UIPictureButton uiBtnSave;
        private CustomControls.UIPictureButton uiBtnNew;
        private CustomControls.UIPictureButton uiBtnOpen;
        private CustomControls.UIGroupBox grpRecordRun;
        private CustomControls.UIPictureButton uiBtnRecordSequence;
        private CustomControls.UIPictureButton uiBtnRunScript;
        private CustomControls.UIPictureButton uiBtnScheduleManagement;
        private CustomControls.UIGroupBox grpVariable;
        private CustomControls.UIPictureButton uiBtnClearAll;
        private CustomControls.UIPictureButton uiBtnSettings;
        private CustomControls.UIPictureButton uiBtnAddVariable;
        private ToolStripMenuItem tsmiNewFolder;
        private ToolStripMenuItem tsmiNewScriptFile;
        private ContextMenuStrip cmsProjectFileActions;
        private ToolStripMenuItem tsmiDeleteFile;
        private ToolStripMenuItem tsmiCopyFile;
        private ToolStripMenuItem tsmiRenameFile;
        private ToolStripMenuItem tsmiPasteFolder;
        private ImageList imgListProjectPane;
        private CustomControls.UITabControl uiScriptTabControl;
        private Panel pnlCommandHelper;
        private CustomControls.UIFlowLayoutPanel flwRecentFiles;
        private Label lblFilesMissing;
        private PictureBox pbRecentFiles;
        private PictureBox pbLinks;
        private PictureBox pbTasktLogo;
        private Label lblRecentFiles;
        private LinkLabel lnkGitWiki;
        private LinkLabel lnkGitIssue;
        private LinkLabel lnkGitLatestReleases;
        private LinkLabel lnkGitProject;
        private Label lblWelcomeToTaskt;
        private Label lblWelcomeDescription;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader clmCommand;
        private ImageList imgListTabControl;
        private ContextMenuStrip cmsScriptTabActions;
        private ToolStripMenuItem tsmiCloseTab;
        private ToolStripMenuItem tsmiCloseAllButThis;
    }
}

