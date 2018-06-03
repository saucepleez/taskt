namespace sharpRPA.UI.Forms
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
            this.tlpControls = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainer1 = new sharpRPA.UI.CustomControls.UISplitContainer();
            this.tvCommands = new sharpRPA.UI.CustomControls.UITreeView();
            this.pnlCommandHelper = new System.Windows.Forms.Panel();
            this.flwRecentFiles = new sharpRPA.UI.CustomControls.UIFlowLayoutPanel();
            this.lblCurrentVersion = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lnkGitWiki = new System.Windows.Forms.LinkLabel();
            this.lnkGitIssue = new System.Windows.Forms.LinkLabel();
            this.lnkGitLatestReleases = new System.Windows.Forms.LinkLabel();
            this.lnkGitProject = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblNote = new System.Windows.Forms.Label();
            this.lstScriptActions = new sharpRPA.UI.CustomControls.UIListView();
            this.commandColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblCoordinatorInfo = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.lblMainLogo = new System.Windows.Forms.Label();
            this.pnlStatus = new System.Windows.Forms.Panel();
            this.pnlControlContainer = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.uiGroupBox3 = new sharpRPA.UI.CustomControls.UIGroupBox();
            this.uiBtnRunScript = new sharpRPA.UI.CustomControls.UIPictureButton();
            this.uiBtnScheduleManagement = new sharpRPA.UI.CustomControls.UIPictureButton();
            this.uiGroupBox1 = new sharpRPA.UI.CustomControls.UIGroupBox();
            this.uiBtnCommandExplorer = new sharpRPA.UI.CustomControls.UIPictureButton();
            this.uiBtnAddCommand = new sharpRPA.UI.CustomControls.UIPictureButton();
            this.uiBtnAddVariable = new sharpRPA.UI.CustomControls.UIPictureButton();
            this.uiGroupBox2 = new sharpRPA.UI.CustomControls.UIGroupBox();
            this.uiBtnSettings = new sharpRPA.UI.CustomControls.UIPictureButton();
            this.uiBtnSave = new sharpRPA.UI.CustomControls.UIPictureButton();
            this.uiBtnNew = new sharpRPA.UI.CustomControls.UIPictureButton();
            this.uiBtnOpen = new sharpRPA.UI.CustomControls.UIPictureButton();
            this.tmrNotify = new System.Windows.Forms.Timer(this.components);
            this.lstContextStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.enableSelectedCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableSelectedCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pauseBeforeExecutionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copySelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tlpControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.pnlCommandHelper.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.pnlControlContainer.SuspendLayout();
            this.uiGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnRunScript)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnScheduleManagement)).BeginInit();
            this.uiGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCommandExplorer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnAddCommand)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnAddVariable)).BeginInit();
            this.uiGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnSettings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnNew)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOpen)).BeginInit();
            this.lstContextStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpControls
            // 
            this.tlpControls.BackColor = System.Drawing.Color.White;
            this.tlpControls.ColumnCount = 3;
            this.tlpControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 255F));
            this.tlpControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 524F));
            this.tlpControls.Controls.Add(this.splitContainer1, 0, 2);
            this.tlpControls.Controls.Add(this.pnlHeader, 0, 0);
            this.tlpControls.Controls.Add(this.pnlStatus, 0, 3);
            this.tlpControls.Controls.Add(this.pnlControlContainer, 0, 1);
            this.tlpControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpControls.Location = new System.Drawing.Point(0, 0);
            this.tlpControls.Margin = new System.Windows.Forms.Padding(0);
            this.tlpControls.Name = "tlpControls";
            this.tlpControls.RowCount = 4;
            this.tlpControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tlpControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 82F));
            this.tlpControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tlpControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tlpControls.Size = new System.Drawing.Size(1029, 631);
            this.tlpControls.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.tlpControls.SetColumnSpan(this.splitContainer1, 3);
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(3, 134);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Panel1.Controls.Add(this.tvCommands);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Panel2.Controls.Add(this.pnlCommandHelper);
            this.splitContainer1.Panel2.Controls.Add(this.lstScriptActions);
            this.splitContainer1.Size = new System.Drawing.Size(1023, 462);
            this.splitContainer1.SplitterDistance = 238;
            this.splitContainer1.TabIndex = 4;
            // 
            // tvCommands
            // 
            this.tvCommands.BackColor = System.Drawing.Color.White;
            this.tvCommands.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tvCommands.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvCommands.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tvCommands.ForeColor = System.Drawing.Color.SteelBlue;
            this.tvCommands.Location = new System.Drawing.Point(0, 0);
            this.tvCommands.Name = "tvCommands";
            this.tvCommands.ShowLines = false;
            this.tvCommands.Size = new System.Drawing.Size(238, 462);
            this.tvCommands.TabIndex = 8;
            this.tvCommands.DoubleClick += new System.EventHandler(this.tvCommands_DoubleClick);
            // 
            // pnlCommandHelper
            // 
            this.pnlCommandHelper.Controls.Add(this.flwRecentFiles);
            this.pnlCommandHelper.Controls.Add(this.lblCurrentVersion);
            this.pnlCommandHelper.Controls.Add(this.label4);
            this.pnlCommandHelper.Controls.Add(this.label3);
            this.pnlCommandHelper.Controls.Add(this.label2);
            this.pnlCommandHelper.Controls.Add(this.lnkGitWiki);
            this.pnlCommandHelper.Controls.Add(this.lnkGitIssue);
            this.pnlCommandHelper.Controls.Add(this.lnkGitLatestReleases);
            this.pnlCommandHelper.Controls.Add(this.lnkGitProject);
            this.pnlCommandHelper.Controls.Add(this.label1);
            this.pnlCommandHelper.Controls.Add(this.lblNote);
            this.pnlCommandHelper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCommandHelper.Location = new System.Drawing.Point(0, 0);
            this.pnlCommandHelper.Name = "pnlCommandHelper";
            this.pnlCommandHelper.Size = new System.Drawing.Size(781, 462);
            this.pnlCommandHelper.TabIndex = 7;
            // 
            // flwRecentFiles
            // 
            this.flwRecentFiles.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flwRecentFiles.Location = new System.Drawing.Point(7, 234);
            this.flwRecentFiles.Name = "flwRecentFiles";
            this.flwRecentFiles.Size = new System.Drawing.Size(397, 156);
            this.flwRecentFiles.TabIndex = 12;
            this.flwRecentFiles.WrapContents = false;
            // 
            // lblCurrentVersion
            // 
            this.lblCurrentVersion.AutoSize = true;
            this.lblCurrentVersion.Font = new System.Drawing.Font("Segoe UI Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentVersion.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblCurrentVersion.Location = new System.Drawing.Point(11, 422);
            this.lblCurrentVersion.Name = "lblCurrentVersion";
            this.lblCurrentVersion.Size = new System.Drawing.Size(55, 21);
            this.lblCurrentVersion.TabIndex = 11;
            this.lblCurrentVersion.Text = "v. 0.0.0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Black", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.SteelBlue;
            this.label4.Location = new System.Drawing.Point(5, 392);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(139, 30);
            this.label4.TabIndex = 10;
            this.label4.Text = "Version Info";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Black", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.SteelBlue;
            this.label3.Location = new System.Drawing.Point(2, 200);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 30);
            this.label3.TabIndex = 8;
            this.label3.Text = "Recent Files";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Black", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.SteelBlue;
            this.label2.Location = new System.Drawing.Point(5, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(147, 30);
            this.label2.TabIndex = 7;
            this.label2.Text = "Helpful Links";
            // 
            // lnkGitWiki
            // 
            this.lnkGitWiki.AutoSize = true;
            this.lnkGitWiki.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkGitWiki.ForeColor = System.Drawing.Color.SteelBlue;
            this.lnkGitWiki.LinkColor = System.Drawing.Color.SteelBlue;
            this.lnkGitWiki.Location = new System.Drawing.Point(7, 183);
            this.lnkGitWiki.Name = "lnkGitWiki";
            this.lnkGitWiki.Size = new System.Drawing.Size(156, 17);
            this.lnkGitWiki.TabIndex = 6;
            this.lnkGitWiki.TabStop = true;
            this.lnkGitWiki.Text = "View Documentation/Wiki";
            this.lnkGitWiki.VisitedLinkColor = System.Drawing.Color.LightSteelBlue;
            this.lnkGitWiki.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkGitWiki_LinkClicked);
            // 
            // lnkGitIssue
            // 
            this.lnkGitIssue.AutoSize = true;
            this.lnkGitIssue.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkGitIssue.ForeColor = System.Drawing.Color.SteelBlue;
            this.lnkGitIssue.LinkColor = System.Drawing.Color.SteelBlue;
            this.lnkGitIssue.Location = new System.Drawing.Point(7, 166);
            this.lnkGitIssue.Name = "lnkGitIssue";
            this.lnkGitIssue.Size = new System.Drawing.Size(235, 17);
            this.lnkGitIssue.TabIndex = 5;
            this.lnkGitIssue.TabStop = true;
            this.lnkGitIssue.Text = "Request Enhancement or Report a bug";
            this.lnkGitIssue.VisitedLinkColor = System.Drawing.Color.LightSteelBlue;
            this.lnkGitIssue.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkGitIssue_LinkClicked);
            // 
            // lnkGitLatestReleases
            // 
            this.lnkGitLatestReleases.AutoSize = true;
            this.lnkGitLatestReleases.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkGitLatestReleases.ForeColor = System.Drawing.Color.SteelBlue;
            this.lnkGitLatestReleases.LinkColor = System.Drawing.Color.SteelBlue;
            this.lnkGitLatestReleases.Location = new System.Drawing.Point(7, 149);
            this.lnkGitLatestReleases.Name = "lnkGitLatestReleases";
            this.lnkGitLatestReleases.Size = new System.Drawing.Size(128, 17);
            this.lnkGitLatestReleases.TabIndex = 4;
            this.lnkGitLatestReleases.TabStop = true;
            this.lnkGitLatestReleases.Text = "View Latest Releases";
            this.lnkGitLatestReleases.VisitedLinkColor = System.Drawing.Color.LightSteelBlue;
            this.lnkGitLatestReleases.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkGitLatestReleases_LinkClicked);
            // 
            // lnkGitProject
            // 
            this.lnkGitProject.AutoSize = true;
            this.lnkGitProject.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkGitProject.ForeColor = System.Drawing.Color.SteelBlue;
            this.lnkGitProject.LinkColor = System.Drawing.Color.SteelBlue;
            this.lnkGitProject.Location = new System.Drawing.Point(7, 132);
            this.lnkGitProject.Name = "lnkGitProject";
            this.lnkGitProject.Size = new System.Drawing.Size(142, 17);
            this.lnkGitProject.TabIndex = 3;
            this.lnkGitProject.TabStop = true;
            this.lnkGitProject.Text = "View Project on GitHub";
            this.lnkGitProject.VisitedLinkColor = System.Drawing.Color.LightSteelBlue;
            this.lnkGitProject.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkGitProject_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Black", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.SteelBlue;
            this.label1.Location = new System.Drawing.Point(4, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(251, 30);
            this.label1.TabIndex = 2;
            this.label1.Text = "Welcome to sharpRPA!";
            // 
            // lblNote
            // 
            this.lblNote.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNote.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblNote.Location = new System.Drawing.Point(5, 36);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(358, 66);
            this.lblNote.TabIndex = 1;
            this.lblNote.Text = "Start building automation by selecting \"Add\" at the top or double-clicking a comm" +
    "and from the list to the left.";
            // 
            // lstScriptActions
            // 
            this.lstScriptActions.AllowDrop = true;
            this.lstScriptActions.BackColor = System.Drawing.Color.White;
            this.lstScriptActions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstScriptActions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.commandColumn});
            this.lstScriptActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstScriptActions.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstScriptActions.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstScriptActions.Location = new System.Drawing.Point(0, 0);
            this.lstScriptActions.Name = "lstScriptActions";
            this.lstScriptActions.Size = new System.Drawing.Size(781, 462);
            this.lstScriptActions.TabIndex = 6;
            this.lstScriptActions.UseCompatibleStateImageBehavior = false;
            this.lstScriptActions.View = System.Windows.Forms.View.Details;
            this.lstScriptActions.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.lstScriptActions_ItemDrag);
            this.lstScriptActions.DragDrop += new System.Windows.Forms.DragEventHandler(this.lstScriptActions_DragDrop);
            this.lstScriptActions.DragEnter += new System.Windows.Forms.DragEventHandler(this.lstScriptActions_DragEnter);
            this.lstScriptActions.DoubleClick += new System.EventHandler(this.lstScriptActions_DoubleClick);
            this.lstScriptActions.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstScriptActions_KeyDown);
            this.lstScriptActions.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lstScriptActions_MouseClick);
            // 
            // commandColumn
            // 
            this.commandColumn.Text = "Script Commands";
            this.commandColumn.Width = 771;
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.tlpControls.SetColumnSpan(this.pnlHeader, 3);
            this.pnlHeader.Controls.Add(this.lblCoordinatorInfo);
            this.pnlHeader.Controls.Add(this.pictureBox2);
            this.pnlHeader.Controls.Add(this.lblMainLogo);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Margin = new System.Windows.Forms.Padding(0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1029, 49);
            this.pnlHeader.TabIndex = 2;
            this.pnlHeader.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // lblCoordinatorInfo
            // 
            this.lblCoordinatorInfo.AutoSize = true;
            this.lblCoordinatorInfo.BackColor = System.Drawing.Color.Transparent;
            this.lblCoordinatorInfo.Font = new System.Drawing.Font("Raleway SemiBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCoordinatorInfo.ForeColor = System.Drawing.Color.White;
            this.lblCoordinatorInfo.Location = new System.Drawing.Point(228, 16);
            this.lblCoordinatorInfo.Name = "lblCoordinatorInfo";
            this.lblCoordinatorInfo.Size = new System.Drawing.Size(0, 19);
            this.lblCoordinatorInfo.TabIndex = 3;
            this.lblCoordinatorInfo.Visible = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::sharpRPA.Properties.Resources.RPALogo;
            this.pictureBox2.Location = new System.Drawing.Point(11, 2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(45, 45);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            // 
            // lblMainLogo
            // 
            this.lblMainLogo.AutoSize = true;
            this.lblMainLogo.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMainLogo.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblMainLogo.Location = new System.Drawing.Point(51, 2);
            this.lblMainLogo.Margin = new System.Windows.Forms.Padding(0);
            this.lblMainLogo.Name = "lblMainLogo";
            this.lblMainLogo.Size = new System.Drawing.Size(165, 45);
            this.lblMainLogo.TabIndex = 0;
            this.lblMainLogo.Text = "sharpRPA";
            // 
            // pnlStatus
            // 
            this.pnlStatus.BackColor = System.Drawing.Color.SteelBlue;
            this.tlpControls.SetColumnSpan(this.pnlStatus, 3);
            this.pnlStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlStatus.Font = new System.Drawing.Font("Eras Demi ITC", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlStatus.Location = new System.Drawing.Point(0, 599);
            this.pnlStatus.Margin = new System.Windows.Forms.Padding(0);
            this.pnlStatus.Name = "pnlStatus";
            this.pnlStatus.Size = new System.Drawing.Size(1029, 32);
            this.pnlStatus.TabIndex = 3;
            this.pnlStatus.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlStatus_Paint);
            // 
            // pnlControlContainer
            // 
            this.pnlControlContainer.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tlpControls.SetColumnSpan(this.pnlControlContainer, 3);
            this.pnlControlContainer.Controls.Add(this.label7);
            this.pnlControlContainer.Controls.Add(this.label6);
            this.pnlControlContainer.Controls.Add(this.label5);
            this.pnlControlContainer.Controls.Add(this.uiGroupBox3);
            this.pnlControlContainer.Controls.Add(this.uiGroupBox1);
            this.pnlControlContainer.Controls.Add(this.uiGroupBox2);
            this.pnlControlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlControlContainer.Location = new System.Drawing.Point(0, 49);
            this.pnlControlContainer.Margin = new System.Windows.Forms.Padding(0);
            this.pnlControlContainer.Name = "pnlControlContainer";
            this.pnlControlContainer.Size = new System.Drawing.Size(1029, 82);
            this.pnlControlContainer.TabIndex = 7;
            this.pnlControlContainer.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlControlContainer_Paint);
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Gainsboro;
            this.label7.ForeColor = System.Drawing.Color.DimGray;
            this.label7.Location = new System.Drawing.Point(510, 5);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(2, 70);
            this.label7.TabIndex = 21;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Gainsboro;
            this.label6.ForeColor = System.Drawing.Color.DimGray;
            this.label6.Location = new System.Drawing.Point(217, 5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(2, 70);
            this.label6.TabIndex = 20;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Gainsboro;
            this.label5.ForeColor = System.Drawing.Color.DimGray;
            this.label5.Location = new System.Drawing.Point(390, 5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(2, 70);
            this.label5.TabIndex = 19;
            // 
            // uiGroupBox3
            // 
            this.uiGroupBox3.BackColor = System.Drawing.Color.Transparent;
            this.uiGroupBox3.Controls.Add(this.uiBtnRunScript);
            this.uiGroupBox3.Controls.Add(this.uiBtnScheduleManagement);
            this.uiGroupBox3.Location = new System.Drawing.Point(394, 3);
            this.uiGroupBox3.Name = "uiGroupBox3";
            this.uiGroupBox3.Size = new System.Drawing.Size(115, 76);
            this.uiGroupBox3.TabIndex = 18;
            this.uiGroupBox3.TabStop = false;
            this.uiGroupBox3.Text = "Execute";
            this.uiGroupBox3.TitleBackColor = System.Drawing.Color.Transparent;
            this.uiGroupBox3.TitleFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uiGroupBox3.TitleForeColor = System.Drawing.Color.SteelBlue;
            this.uiGroupBox3.TitleHatchStyle = System.Drawing.Drawing2D.HatchStyle.Horizontal;
            // 
            // uiBtnRunScript
            // 
            this.uiBtnRunScript.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnRunScript.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnRunScript.DisplayText = "Run";
            this.uiBtnRunScript.DisplayTextBrush = System.Drawing.Color.Black;
            this.uiBtnRunScript.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnRunScript.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnRunScript.Image")));
            this.uiBtnRunScript.IsMouseOver = false;
            this.uiBtnRunScript.Location = new System.Drawing.Point(62, 20);
            this.uiBtnRunScript.Name = "uiBtnRunScript";
            this.uiBtnRunScript.Size = new System.Drawing.Size(48, 52);
            this.uiBtnRunScript.TabIndex = 12;
            this.uiBtnRunScript.TabStop = false;
            this.uiBtnRunScript.Click += new System.EventHandler(this.uiBtnRunScript_Click);
            // 
            // uiBtnScheduleManagement
            // 
            this.uiBtnScheduleManagement.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnScheduleManagement.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnScheduleManagement.DisplayText = "Schedule";
            this.uiBtnScheduleManagement.DisplayTextBrush = System.Drawing.Color.Black;
            this.uiBtnScheduleManagement.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnScheduleManagement.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnScheduleManagement.Image")));
            this.uiBtnScheduleManagement.IsMouseOver = false;
            this.uiBtnScheduleManagement.Location = new System.Drawing.Point(4, 20);
            this.uiBtnScheduleManagement.Name = "uiBtnScheduleManagement";
            this.uiBtnScheduleManagement.Size = new System.Drawing.Size(52, 52);
            this.uiBtnScheduleManagement.TabIndex = 13;
            this.uiBtnScheduleManagement.TabStop = false;
            this.uiBtnScheduleManagement.Click += new System.EventHandler(this.uiBtnScheduleManagement_Click);
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.BackColor = System.Drawing.Color.Transparent;
            this.uiGroupBox1.Controls.Add(this.uiBtnCommandExplorer);
            this.uiGroupBox1.Controls.Add(this.uiBtnAddCommand);
            this.uiGroupBox1.Controls.Add(this.uiBtnAddVariable);
            this.uiGroupBox1.Location = new System.Drawing.Point(221, 3);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.Size = new System.Drawing.Size(168, 76);
            this.uiGroupBox1.TabIndex = 17;
            this.uiGroupBox1.TabStop = false;
            this.uiGroupBox1.Text = "Commands and Variables";
            this.uiGroupBox1.TitleBackColor = System.Drawing.Color.Transparent;
            this.uiGroupBox1.TitleFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uiGroupBox1.TitleForeColor = System.Drawing.Color.SteelBlue;
            this.uiGroupBox1.TitleHatchStyle = System.Drawing.Drawing2D.HatchStyle.Horizontal;
            // 
            // uiBtnCommandExplorer
            // 
            this.uiBtnCommandExplorer.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnCommandExplorer.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnCommandExplorer.DisplayText = "Explore";
            this.uiBtnCommandExplorer.DisplayTextBrush = System.Drawing.Color.Black;
            this.uiBtnCommandExplorer.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnCommandExplorer.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnCommandExplorer.Image")));
            this.uiBtnCommandExplorer.IsMouseOver = false;
            this.uiBtnCommandExplorer.Location = new System.Drawing.Point(111, 20);
            this.uiBtnCommandExplorer.Name = "uiBtnCommandExplorer";
            this.uiBtnCommandExplorer.Size = new System.Drawing.Size(52, 52);
            this.uiBtnCommandExplorer.TabIndex = 14;
            this.uiBtnCommandExplorer.TabStop = false;
            this.uiBtnCommandExplorer.Click += new System.EventHandler(this.uiBtnCommandExplorer_Click);
            // 
            // uiBtnAddCommand
            // 
            this.uiBtnAddCommand.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnAddCommand.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnAddCommand.DisplayText = "Add";
            this.uiBtnAddCommand.DisplayTextBrush = System.Drawing.Color.Black;
            this.uiBtnAddCommand.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnAddCommand.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnAddCommand.Image")));
            this.uiBtnAddCommand.IsMouseOver = false;
            this.uiBtnAddCommand.Location = new System.Drawing.Point(3, 20);
            this.uiBtnAddCommand.Name = "uiBtnAddCommand";
            this.uiBtnAddCommand.Size = new System.Drawing.Size(48, 52);
            this.uiBtnAddCommand.TabIndex = 12;
            this.uiBtnAddCommand.TabStop = false;
            this.uiBtnAddCommand.Click += new System.EventHandler(this.uiBtnAddCommand_Click);
            // 
            // uiBtnAddVariable
            // 
            this.uiBtnAddVariable.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnAddVariable.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnAddVariable.DisplayText = "Variables";
            this.uiBtnAddVariable.DisplayTextBrush = System.Drawing.Color.Black;
            this.uiBtnAddVariable.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnAddVariable.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnAddVariable.Image")));
            this.uiBtnAddVariable.IsMouseOver = false;
            this.uiBtnAddVariable.Location = new System.Drawing.Point(55, 20);
            this.uiBtnAddVariable.Name = "uiBtnAddVariable";
            this.uiBtnAddVariable.Size = new System.Drawing.Size(52, 52);
            this.uiBtnAddVariable.TabIndex = 13;
            this.uiBtnAddVariable.TabStop = false;
            this.uiBtnAddVariable.Click += new System.EventHandler(this.uiBtnAddVariable_Click);
            // 
            // uiGroupBox2
            // 
            this.uiGroupBox2.BackColor = System.Drawing.Color.Transparent;
            this.uiGroupBox2.Controls.Add(this.uiBtnSettings);
            this.uiGroupBox2.Controls.Add(this.uiBtnSave);
            this.uiGroupBox2.Controls.Add(this.uiBtnNew);
            this.uiGroupBox2.Controls.Add(this.uiBtnOpen);
            this.uiGroupBox2.Location = new System.Drawing.Point(3, 3);
            this.uiGroupBox2.Name = "uiGroupBox2";
            this.uiGroupBox2.Size = new System.Drawing.Size(213, 76);
            this.uiGroupBox2.TabIndex = 16;
            this.uiGroupBox2.TabStop = false;
            this.uiGroupBox2.Text = "File Actions and Settings";
            this.uiGroupBox2.TitleBackColor = System.Drawing.Color.Transparent;
            this.uiGroupBox2.TitleFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uiGroupBox2.TitleForeColor = System.Drawing.Color.SteelBlue;
            this.uiGroupBox2.TitleHatchStyle = System.Drawing.Drawing2D.HatchStyle.Horizontal;
            // 
            // uiBtnSettings
            // 
            this.uiBtnSettings.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnSettings.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnSettings.DisplayText = "Settings";
            this.uiBtnSettings.DisplayTextBrush = System.Drawing.Color.Black;
            this.uiBtnSettings.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnSettings.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnSettings.Image")));
            this.uiBtnSettings.IsMouseOver = false;
            this.uiBtnSettings.Location = new System.Drawing.Point(160, 20);
            this.uiBtnSettings.Name = "uiBtnSettings";
            this.uiBtnSettings.Size = new System.Drawing.Size(48, 52);
            this.uiBtnSettings.TabIndex = 12;
            this.uiBtnSettings.TabStop = false;
            this.uiBtnSettings.Click += new System.EventHandler(this.uiBtnSettings_Click);
            // 
            // uiBtnSave
            // 
            this.uiBtnSave.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnSave.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnSave.DisplayText = "Save";
            this.uiBtnSave.DisplayTextBrush = System.Drawing.Color.Black;
            this.uiBtnSave.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnSave.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnSave.Image")));
            this.uiBtnSave.IsMouseOver = false;
            this.uiBtnSave.Location = new System.Drawing.Point(108, 20);
            this.uiBtnSave.Name = "uiBtnSave";
            this.uiBtnSave.Size = new System.Drawing.Size(48, 52);
            this.uiBtnSave.TabIndex = 11;
            this.uiBtnSave.TabStop = false;
            this.uiBtnSave.Click += new System.EventHandler(this.uiBtnSave_Click);
            // 
            // uiBtnNew
            // 
            this.uiBtnNew.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnNew.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnNew.DisplayText = "New";
            this.uiBtnNew.DisplayTextBrush = System.Drawing.Color.Black;
            this.uiBtnNew.Font = new System.Drawing.Font("Eras Medium ITC", 8F);
            this.uiBtnNew.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnNew.Image")));
            this.uiBtnNew.IsMouseOver = false;
            this.uiBtnNew.Location = new System.Drawing.Point(3, 20);
            this.uiBtnNew.Name = "uiBtnNew";
            this.uiBtnNew.Size = new System.Drawing.Size(48, 52);
            this.uiBtnNew.TabIndex = 12;
            this.uiBtnNew.TabStop = false;
            this.uiBtnNew.Click += new System.EventHandler(this.uiBtnNew_Click);
            // 
            // uiBtnOpen
            // 
            this.uiBtnOpen.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnOpen.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnOpen.DisplayText = "Open";
            this.uiBtnOpen.DisplayTextBrush = System.Drawing.Color.Black;
            this.uiBtnOpen.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnOpen.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnOpen.Image")));
            this.uiBtnOpen.IsMouseOver = false;
            this.uiBtnOpen.Location = new System.Drawing.Point(55, 20);
            this.uiBtnOpen.Name = "uiBtnOpen";
            this.uiBtnOpen.Size = new System.Drawing.Size(48, 52);
            this.uiBtnOpen.TabIndex = 10;
            this.uiBtnOpen.TabStop = false;
            this.uiBtnOpen.Click += new System.EventHandler(this.uiBtnOpen_Click);
            // 
            // tmrNotify
            // 
            this.tmrNotify.Enabled = true;
            this.tmrNotify.Interval = 500;
            this.tmrNotify.Tick += new System.EventHandler(this.tmrNotify_Tick);
            // 
            // lstContextStrip
            // 
            this.lstContextStrip.Font = new System.Drawing.Font("Eras Light ITC", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstContextStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enableSelectedCodeToolStripMenuItem,
            this.disableSelectedCodeToolStripMenuItem,
            this.pauseBeforeExecutionToolStripMenuItem,
            this.copySelectedToolStripMenuItem,
            this.pasteSelectedToolStripMenuItem});
            this.lstContextStrip.Name = "lstContextStrip";
            this.lstContextStrip.Size = new System.Drawing.Size(233, 124);
            // 
            // enableSelectedCodeToolStripMenuItem
            // 
            this.enableSelectedCodeToolStripMenuItem.Name = "enableSelectedCodeToolStripMenuItem";
            this.enableSelectedCodeToolStripMenuItem.Size = new System.Drawing.Size(232, 24);
            this.enableSelectedCodeToolStripMenuItem.Text = "Enable Selected Code";
            this.enableSelectedCodeToolStripMenuItem.Click += new System.EventHandler(this.enableSelectedCodeToolStripMenuItem_Click);
            // 
            // disableSelectedCodeToolStripMenuItem
            // 
            this.disableSelectedCodeToolStripMenuItem.Name = "disableSelectedCodeToolStripMenuItem";
            this.disableSelectedCodeToolStripMenuItem.Size = new System.Drawing.Size(232, 24);
            this.disableSelectedCodeToolStripMenuItem.Text = "Disable Selected Code";
            this.disableSelectedCodeToolStripMenuItem.Click += new System.EventHandler(this.disableSelectedCodeToolStripMenuItem_Click);
            // 
            // pauseBeforeExecutionToolStripMenuItem
            // 
            this.pauseBeforeExecutionToolStripMenuItem.Name = "pauseBeforeExecutionToolStripMenuItem";
            this.pauseBeforeExecutionToolStripMenuItem.Size = new System.Drawing.Size(232, 24);
            this.pauseBeforeExecutionToolStripMenuItem.Text = "Pause Before Execution";
            this.pauseBeforeExecutionToolStripMenuItem.Click += new System.EventHandler(this.pauseBeforeExecutionToolStripMenuItem_Click);
            // 
            // copySelectedToolStripMenuItem
            // 
            this.copySelectedToolStripMenuItem.Name = "copySelectedToolStripMenuItem";
            this.copySelectedToolStripMenuItem.Size = new System.Drawing.Size(232, 24);
            this.copySelectedToolStripMenuItem.Text = "Copy Selected Action";
            this.copySelectedToolStripMenuItem.Click += new System.EventHandler(this.copySelectedToolStripMenuItem_Click);
            // 
            // pasteSelectedToolStripMenuItem
            // 
            this.pasteSelectedToolStripMenuItem.Name = "pasteSelectedToolStripMenuItem";
            this.pasteSelectedToolStripMenuItem.Size = new System.Drawing.Size(232, 24);
            this.pasteSelectedToolStripMenuItem.Text = "Paste Selected Action";
            this.pasteSelectedToolStripMenuItem.Click += new System.EventHandler(this.pasteSelectedToolStripMenuItem_Click);
            // 
            // frmScriptBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1029, 631);
            this.Controls.Add(this.tlpControls);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmScriptBuilder";
            this.Text = "sharpRPA - Script Builder";
            this.Load += new System.EventHandler(this.frmScriptBuilder_Load);
            this.Shown += new System.EventHandler(this.frmScriptBuilder_Shown);
            this.SizeChanged += new System.EventHandler(this.frmScriptBuilder_SizeChanged);
            this.tlpControls.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.pnlCommandHelper.ResumeLayout(false);
            this.pnlCommandHelper.PerformLayout();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.pnlControlContainer.ResumeLayout(false);
            this.uiGroupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnRunScript)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnScheduleManagement)).EndInit();
            this.uiGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCommandExplorer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnAddCommand)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnAddVariable)).EndInit();
            this.uiGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnSettings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnNew)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOpen)).EndInit();
            this.lstContextStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tlpControls;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel pnlStatus;
        private System.Windows.Forms.Label lblMainLogo;
        private sharpRPA.UI.CustomControls.UIListView lstScriptActions;
        private System.Windows.Forms.ColumnHeader commandColumn;
        private System.Windows.Forms.Timer tmrNotify;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ContextMenuStrip lstContextStrip;
        private System.Windows.Forms.ToolStripMenuItem enableSelectedCodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disableSelectedCodeToolStripMenuItem;
        private System.Windows.Forms.Panel pnlControlContainer;
        private sharpRPA.UI.CustomControls.UIPictureButton uiBtnRunScript;
        private sharpRPA.UI.CustomControls.UIPictureButton uiBtnAddVariable;
        private sharpRPA.UI.CustomControls.UIPictureButton uiBtnAddCommand;
        private sharpRPA.UI.CustomControls.UIPictureButton uiBtnSave;
        private sharpRPA.UI.CustomControls.UIPictureButton uiBtnOpen;
        private sharpRPA.UI.CustomControls.UIPictureButton uiBtnNew;
        private sharpRPA.UI.CustomControls.UIPictureButton uiBtnScheduleManagement;
        private sharpRPA.UI.CustomControls.UIPictureButton uiBtnCommandExplorer;
        private System.Windows.Forms.ToolStripMenuItem pauseBeforeExecutionToolStripMenuItem;
        private System.Windows.Forms.Label lblCoordinatorInfo;
        private CustomControls.UIPictureButton uiBtnSettings;
        private UI.CustomControls.UITreeView tvCommands;
        private UI.CustomControls.UISplitContainer splitContainer1;
        private CustomControls.UIGroupBox uiGroupBox2;
        private CustomControls.UIGroupBox uiGroupBox1;
        private CustomControls.UIGroupBox uiGroupBox3;
        private System.Windows.Forms.Panel pnlCommandHelper;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.ToolStripMenuItem copySelectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteSelectedToolStripMenuItem;
        private System.Windows.Forms.LinkLabel lnkGitProject;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel lnkGitIssue;
        private System.Windows.Forms.LinkLabel lnkGitLatestReleases;
        private System.Windows.Forms.LinkLabel lnkGitWiki;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblCurrentVersion;
        private CustomControls.UIFlowLayoutPanel flwRecentFiles;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
    }
}

