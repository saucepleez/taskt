namespace taskt.UI.Forms
{
    partial class frmSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSettings));
            this.btnConnect = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.chkRetryOnDisconnect = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkAutomaticallyConnect = new System.Windows.Forms.CheckBox();
            this.chkServerEnabled = new System.Windows.Forms.CheckBox();
            this.txtServerURL = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPublicKey = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkEnableLogging = new System.Windows.Forms.CheckBox();
            this.chkAutoCloseWindow = new System.Windows.Forms.CheckBox();
            this.chkShowDebug = new System.Windows.Forms.CheckBox();
            this.uiBtnOpen = new taskt.UI.CustomControls.UIPictureButton();
            this.lblMainLogo = new System.Windows.Forms.Label();
            this.lblOptions = new System.Windows.Forms.Label();
            this.lblApplicationSettings = new System.Windows.Forms.Label();
            this.chkAntiIdle = new System.Windows.Forms.CheckBox();
            this.btnUpdates = new System.Windows.Forms.Button();
            this.chkAdvancedDebug = new System.Windows.Forms.CheckBox();
            this.chkCreateMissingVariables = new System.Windows.Forms.CheckBox();
            this.uiSettingTabs = new taskt.UI.CustomControls.UITabControl();
            this.tabAppSettings = new System.Windows.Forms.TabPage();
            this.chkInsertCommandsInline = new System.Windows.Forms.CheckBox();
            this.btnClearMetrics = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.lblMetrics = new System.Windows.Forms.Label();
            this.tvExecutionTimes = new System.Windows.Forms.TreeView();
            this.btnSelectFolder = new System.Windows.Forms.Button();
            this.lblRootFolder = new System.Windows.Forms.Label();
            this.txtAppFolderPath = new System.Windows.Forms.TextBox();
            this.tabDebugSettings = new System.Windows.Forms.TabPage();
            this.chkTrackMetrics = new System.Windows.Forms.CheckBox();
            this.tabServerSettings = new System.Windows.Forms.TabPage();
            this.chkBypassValidation = new System.Windows.Forms.CheckBox();
            this.btnCloseConnection = new System.Windows.Forms.Button();
            this.lblSocketException = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tmrGetSocketStatus = new System.Windows.Forms.Timer(this.components);
            this.bgwMetrics = new System.ComponentModel.BackgroundWorker();
            this.btnGenerateWikiDocs = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOpen)).BeginInit();
            this.uiSettingTabs.SuspendLayout();
            this.tabAppSettings.SuspendLayout();
            this.tabDebugSettings.SuspendLayout();
            this.tabServerSettings.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnConnect.Location = new System.Drawing.Point(12, 222);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 33);
            this.btnConnect.TabIndex = 17;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblStatus.Location = new System.Drawing.Point(13, 258);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 20);
            this.lblStatus.TabIndex = 16;
            // 
            // chkRetryOnDisconnect
            // 
            this.chkRetryOnDisconnect.AutoSize = true;
            this.chkRetryOnDisconnect.BackColor = System.Drawing.Color.Transparent;
            this.chkRetryOnDisconnect.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRetryOnDisconnect.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkRetryOnDisconnect.Location = new System.Drawing.Point(11, 75);
            this.chkRetryOnDisconnect.Name = "chkRetryOnDisconnect";
            this.chkRetryOnDisconnect.Size = new System.Drawing.Size(154, 19);
            this.chkRetryOnDisconnect.TabIndex = 15;
            this.chkRetryOnDisconnect.Text = "Retry If Connection Fails";
            this.chkRetryOnDisconnect.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.SlateGray;
            this.label1.Location = new System.Drawing.Point(6, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(452, 28);
            this.label1.TabIndex = 14;
            this.label1.Text = "Manage settings used by the application";
            // 
            // chkAutomaticallyConnect
            // 
            this.chkAutomaticallyConnect.AutoSize = true;
            this.chkAutomaticallyConnect.BackColor = System.Drawing.Color.Transparent;
            this.chkAutomaticallyConnect.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutomaticallyConnect.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkAutomaticallyConnect.Location = new System.Drawing.Point(10, 55);
            this.chkAutomaticallyConnect.Name = "chkAutomaticallyConnect";
            this.chkAutomaticallyConnect.Size = new System.Drawing.Size(206, 19);
            this.chkAutomaticallyConnect.TabIndex = 13;
            this.chkAutomaticallyConnect.Text = "Automatically Connect on Startup";
            this.chkAutomaticallyConnect.UseVisualStyleBackColor = false;
            // 
            // chkServerEnabled
            // 
            this.chkServerEnabled.AutoSize = true;
            this.chkServerEnabled.BackColor = System.Drawing.Color.Transparent;
            this.chkServerEnabled.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkServerEnabled.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkServerEnabled.Location = new System.Drawing.Point(10, 36);
            this.chkServerEnabled.Name = "chkServerEnabled";
            this.chkServerEnabled.Size = new System.Drawing.Size(168, 19);
            this.chkServerEnabled.TabIndex = 12;
            this.chkServerEnabled.Text = "Server Connection Enabled";
            this.chkServerEnabled.UseVisualStyleBackColor = false;
            // 
            // txtServerURL
            // 
            this.txtServerURL.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtServerURL.Location = new System.Drawing.Point(10, 141);
            this.txtServerURL.Name = "txtServerURL";
            this.txtServerURL.Size = new System.Drawing.Size(371, 29);
            this.txtServerURL.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.SlateGray;
            this.label3.Location = new System.Drawing.Point(7, 119);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(232, 16);
            this.label3.TabIndex = 9;
            this.label3.Text = "Server URL ex. ws://localhost:port/ws)\r";
            // 
            // txtPublicKey
            // 
            this.txtPublicKey.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtPublicKey.Location = new System.Drawing.Point(11, 190);
            this.txtPublicKey.Name = "txtPublicKey";
            this.txtPublicKey.Size = new System.Drawing.Size(371, 29);
            this.txtPublicKey.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.SlateGray;
            this.label2.Location = new System.Drawing.Point(9, 172);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 16);
            this.label2.TabIndex = 18;
            this.label2.Text = "Connect Key:";
            // 
            // chkEnableLogging
            // 
            this.chkEnableLogging.AutoSize = true;
            this.chkEnableLogging.BackColor = System.Drawing.Color.Transparent;
            this.chkEnableLogging.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkEnableLogging.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkEnableLogging.Location = new System.Drawing.Point(10, 87);
            this.chkEnableLogging.Name = "chkEnableLogging";
            this.chkEnableLogging.Size = new System.Drawing.Size(207, 24);
            this.chkEnableLogging.TabIndex = 14;
            this.chkEnableLogging.Text = "Enable Diagnostic Logging";
            this.chkEnableLogging.UseVisualStyleBackColor = false;
            // 
            // chkAutoCloseWindow
            // 
            this.chkAutoCloseWindow.AutoSize = true;
            this.chkAutoCloseWindow.BackColor = System.Drawing.Color.Transparent;
            this.chkAutoCloseWindow.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutoCloseWindow.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkAutoCloseWindow.Location = new System.Drawing.Point(10, 62);
            this.chkAutoCloseWindow.Name = "chkAutoCloseWindow";
            this.chkAutoCloseWindow.Size = new System.Drawing.Size(268, 24);
            this.chkAutoCloseWindow.TabIndex = 13;
            this.chkAutoCloseWindow.Text = "Automatically Close Debug Window";
            this.chkAutoCloseWindow.UseVisualStyleBackColor = false;
            // 
            // chkShowDebug
            // 
            this.chkShowDebug.AutoSize = true;
            this.chkShowDebug.BackColor = System.Drawing.Color.Transparent;
            this.chkShowDebug.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkShowDebug.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkShowDebug.Location = new System.Drawing.Point(10, 36);
            this.chkShowDebug.Name = "chkShowDebug";
            this.chkShowDebug.Size = new System.Drawing.Size(172, 24);
            this.chkShowDebug.TabIndex = 12;
            this.chkShowDebug.Text = "Show Debug Window";
            this.chkShowDebug.UseVisualStyleBackColor = false;
            // 
            // uiBtnOpen
            // 
            this.uiBtnOpen.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnOpen.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnOpen.DisplayText = "Ok";
            this.uiBtnOpen.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnOpen.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnOpen.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnOpen.Image")));
            this.uiBtnOpen.IsMouseOver = false;
            this.uiBtnOpen.Location = new System.Drawing.Point(3, 536);
            this.uiBtnOpen.Name = "uiBtnOpen";
            this.uiBtnOpen.Size = new System.Drawing.Size(48, 48);
            this.uiBtnOpen.TabIndex = 13;
            this.uiBtnOpen.TabStop = false;
            this.uiBtnOpen.Click += new System.EventHandler(this.uiBtnOpen_Click);
            // 
            // lblMainLogo
            // 
            this.lblMainLogo.AutoSize = true;
            this.lblMainLogo.BackColor = System.Drawing.Color.Transparent;
            this.lblMainLogo.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMainLogo.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblMainLogo.Location = new System.Drawing.Point(3, 4);
            this.lblMainLogo.Name = "lblMainLogo";
            this.lblMainLogo.Size = new System.Drawing.Size(135, 37);
            this.lblMainLogo.TabIndex = 14;
            this.lblMainLogo.Text = "settings";
            // 
            // lblOptions
            // 
            this.lblOptions.AutoSize = true;
            this.lblOptions.BackColor = System.Drawing.Color.Transparent;
            this.lblOptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOptions.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblOptions.Location = new System.Drawing.Point(6, 9);
            this.lblOptions.Name = "lblOptions";
            this.lblOptions.Size = new System.Drawing.Size(188, 24);
            this.lblOptions.TabIndex = 15;
            this.lblOptions.Text = "Automation Engine";
            // 
            // lblApplicationSettings
            // 
            this.lblApplicationSettings.AutoSize = true;
            this.lblApplicationSettings.BackColor = System.Drawing.Color.Transparent;
            this.lblApplicationSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApplicationSettings.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblApplicationSettings.Location = new System.Drawing.Point(6, 9);
            this.lblApplicationSettings.Name = "lblApplicationSettings";
            this.lblApplicationSettings.Size = new System.Drawing.Size(194, 24);
            this.lblApplicationSettings.TabIndex = 21;
            this.lblApplicationSettings.Text = "Application Settings";
            // 
            // chkAntiIdle
            // 
            this.chkAntiIdle.AutoSize = true;
            this.chkAntiIdle.BackColor = System.Drawing.Color.Transparent;
            this.chkAntiIdle.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAntiIdle.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkAntiIdle.Location = new System.Drawing.Point(10, 36);
            this.chkAntiIdle.Name = "chkAntiIdle";
            this.chkAntiIdle.Size = new System.Drawing.Size(217, 24);
            this.chkAntiIdle.TabIndex = 20;
            this.chkAntiIdle.Text = "Anti-Idle (while app is open)";
            this.chkAntiIdle.UseVisualStyleBackColor = false;
            // 
            // btnUpdates
            // 
            this.btnUpdates.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdates.Location = new System.Drawing.Point(10, 142);
            this.btnUpdates.Name = "btnUpdates";
            this.btnUpdates.Size = new System.Drawing.Size(207, 27);
            this.btnUpdates.TabIndex = 22;
            this.btnUpdates.Text = "Check For Updates";
            this.btnUpdates.UseVisualStyleBackColor = true;
            this.btnUpdates.Click += new System.EventHandler(this.btnUpdateCheck_Click);
            // 
            // chkAdvancedDebug
            // 
            this.chkAdvancedDebug.AutoSize = true;
            this.chkAdvancedDebug.BackColor = System.Drawing.Color.Transparent;
            this.chkAdvancedDebug.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAdvancedDebug.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkAdvancedDebug.Location = new System.Drawing.Point(11, 111);
            this.chkAdvancedDebug.Name = "chkAdvancedDebug";
            this.chkAdvancedDebug.Size = new System.Drawing.Size(335, 24);
            this.chkAdvancedDebug.TabIndex = 23;
            this.chkAdvancedDebug.Text = "Show Advanced Debug Logs During Execution";
            this.chkAdvancedDebug.UseVisualStyleBackColor = false;
            // 
            // chkCreateMissingVariables
            // 
            this.chkCreateMissingVariables.AutoSize = true;
            this.chkCreateMissingVariables.BackColor = System.Drawing.Color.Transparent;
            this.chkCreateMissingVariables.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCreateMissingVariables.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkCreateMissingVariables.Location = new System.Drawing.Point(11, 133);
            this.chkCreateMissingVariables.Name = "chkCreateMissingVariables";
            this.chkCreateMissingVariables.Size = new System.Drawing.Size(306, 24);
            this.chkCreateMissingVariables.TabIndex = 24;
            this.chkCreateMissingVariables.Text = "Create Missing Variables During Execution";
            this.chkCreateMissingVariables.UseVisualStyleBackColor = false;
            // 
            // uiSettingTabs
            // 
            this.uiSettingTabs.Controls.Add(this.tabAppSettings);
            this.uiSettingTabs.Controls.Add(this.tabDebugSettings);
            this.uiSettingTabs.Controls.Add(this.tabServerSettings);
            this.uiSettingTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiSettingTabs.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uiSettingTabs.Location = new System.Drawing.Point(3, 73);
            this.uiSettingTabs.Name = "uiSettingTabs";
            this.uiSettingTabs.SelectedIndex = 0;
            this.uiSettingTabs.Size = new System.Drawing.Size(568, 457);
            this.uiSettingTabs.TabIndex = 25;
            // 
            // tabAppSettings
            // 
            this.tabAppSettings.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabAppSettings.Controls.Add(this.btnGenerateWikiDocs);
            this.tabAppSettings.Controls.Add(this.chkInsertCommandsInline);
            this.tabAppSettings.Controls.Add(this.btnClearMetrics);
            this.tabAppSettings.Controls.Add(this.label5);
            this.tabAppSettings.Controls.Add(this.lblMetrics);
            this.tabAppSettings.Controls.Add(this.tvExecutionTimes);
            this.tabAppSettings.Controls.Add(this.btnSelectFolder);
            this.tabAppSettings.Controls.Add(this.lblRootFolder);
            this.tabAppSettings.Controls.Add(this.txtAppFolderPath);
            this.tabAppSettings.Controls.Add(this.lblApplicationSettings);
            this.tabAppSettings.Controls.Add(this.chkAntiIdle);
            this.tabAppSettings.Controls.Add(this.btnUpdates);
            this.tabAppSettings.Location = new System.Drawing.Point(4, 30);
            this.tabAppSettings.Name = "tabAppSettings";
            this.tabAppSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabAppSettings.Size = new System.Drawing.Size(560, 423);
            this.tabAppSettings.TabIndex = 0;
            this.tabAppSettings.Text = "Application";
            // 
            // chkInsertCommandsInline
            // 
            this.chkInsertCommandsInline.AutoSize = true;
            this.chkInsertCommandsInline.BackColor = System.Drawing.Color.Transparent;
            this.chkInsertCommandsInline.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkInsertCommandsInline.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkInsertCommandsInline.Location = new System.Drawing.Point(9, 61);
            this.chkInsertCommandsInline.Name = "chkInsertCommandsInline";
            this.chkInsertCommandsInline.Size = new System.Drawing.Size(356, 24);
            this.chkInsertCommandsInline.TabIndex = 30;
            this.chkInsertCommandsInline.Text = "New Commands Insert Below Selected Command";
            this.chkInsertCommandsInline.UseVisualStyleBackColor = false;
            // 
            // btnClearMetrics
            // 
            this.btnClearMetrics.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearMetrics.Location = new System.Drawing.Point(9, 370);
            this.btnClearMetrics.Name = "btnClearMetrics";
            this.btnClearMetrics.Size = new System.Drawing.Size(108, 25);
            this.btnClearMetrics.TabIndex = 29;
            this.btnClearMetrics.Text = "Clear Metrics";
            this.btnClearMetrics.UseVisualStyleBackColor = true;
            this.btnClearMetrics.Visible = false;
            this.btnClearMetrics.Click += new System.EventHandler(this.btnClearMetrics_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.SlateGray;
            this.label5.Location = new System.Drawing.Point(11, 219);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(262, 16);
            this.label5.TabIndex = 28;
            this.label5.Text = "Script Execution Metrics (Last 10 per Script)";
            // 
            // lblMetrics
            // 
            this.lblMetrics.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDownGrid;
            this.lblMetrics.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblMetrics.Location = new System.Drawing.Point(10, 238);
            this.lblMetrics.Name = "lblMetrics";
            this.lblMetrics.Size = new System.Drawing.Size(534, 127);
            this.lblMetrics.TabIndex = 27;
            this.lblMetrics.Text = "Getting Metrics...";
            // 
            // tvExecutionTimes
            // 
            this.tvExecutionTimes.Location = new System.Drawing.Point(10, 238);
            this.tvExecutionTimes.Name = "tvExecutionTimes";
            this.tvExecutionTimes.Size = new System.Drawing.Size(534, 127);
            this.tvExecutionTimes.TabIndex = 26;
            this.tvExecutionTimes.Visible = false;
            // 
            // btnSelectFolder
            // 
            this.btnSelectFolder.Location = new System.Drawing.Point(502, 107);
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.Size = new System.Drawing.Size(42, 30);
            this.btnSelectFolder.TabIndex = 25;
            this.btnSelectFolder.Text = "...";
            this.btnSelectFolder.UseVisualStyleBackColor = true;
            this.btnSelectFolder.Click += new System.EventHandler(this.btnSelectFolder_Click);
            // 
            // lblRootFolder
            // 
            this.lblRootFolder.AutoSize = true;
            this.lblRootFolder.BackColor = System.Drawing.Color.Transparent;
            this.lblRootFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRootFolder.ForeColor = System.Drawing.Color.SlateGray;
            this.lblRootFolder.Location = new System.Drawing.Point(7, 88);
            this.lblRootFolder.Name = "lblRootFolder";
            this.lblRootFolder.Size = new System.Drawing.Size(110, 16);
            this.lblRootFolder.TabIndex = 24;
            this.lblRootFolder.Text = "taskt Root Folder";
            // 
            // txtAppFolderPath
            // 
            this.txtAppFolderPath.Location = new System.Drawing.Point(10, 107);
            this.txtAppFolderPath.Name = "txtAppFolderPath";
            this.txtAppFolderPath.Size = new System.Drawing.Size(490, 29);
            this.txtAppFolderPath.TabIndex = 23;
            // 
            // tabDebugSettings
            // 
            this.tabDebugSettings.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabDebugSettings.Controls.Add(this.chkTrackMetrics);
            this.tabDebugSettings.Controls.Add(this.lblOptions);
            this.tabDebugSettings.Controls.Add(this.chkCreateMissingVariables);
            this.tabDebugSettings.Controls.Add(this.chkShowDebug);
            this.tabDebugSettings.Controls.Add(this.chkAdvancedDebug);
            this.tabDebugSettings.Controls.Add(this.chkAutoCloseWindow);
            this.tabDebugSettings.Controls.Add(this.chkEnableLogging);
            this.tabDebugSettings.Location = new System.Drawing.Point(4, 30);
            this.tabDebugSettings.Name = "tabDebugSettings";
            this.tabDebugSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabDebugSettings.Size = new System.Drawing.Size(560, 369);
            this.tabDebugSettings.TabIndex = 1;
            this.tabDebugSettings.Text = "Automation Engine";
            // 
            // chkTrackMetrics
            // 
            this.chkTrackMetrics.AutoSize = true;
            this.chkTrackMetrics.BackColor = System.Drawing.Color.Transparent;
            this.chkTrackMetrics.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkTrackMetrics.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkTrackMetrics.Location = new System.Drawing.Point(11, 157);
            this.chkTrackMetrics.Name = "chkTrackMetrics";
            this.chkTrackMetrics.Size = new System.Drawing.Size(182, 24);
            this.chkTrackMetrics.TabIndex = 25;
            this.chkTrackMetrics.Text = "Track Execution Metrics";
            this.chkTrackMetrics.UseVisualStyleBackColor = false;
            // 
            // tabServerSettings
            // 
            this.tabServerSettings.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabServerSettings.Controls.Add(this.chkBypassValidation);
            this.tabServerSettings.Controls.Add(this.btnCloseConnection);
            this.tabServerSettings.Controls.Add(this.lblSocketException);
            this.tabServerSettings.Controls.Add(this.label4);
            this.tabServerSettings.Controls.Add(this.chkServerEnabled);
            this.tabServerSettings.Controls.Add(this.txtPublicKey);
            this.tabServerSettings.Controls.Add(this.label3);
            this.tabServerSettings.Controls.Add(this.label2);
            this.tabServerSettings.Controls.Add(this.txtServerURL);
            this.tabServerSettings.Controls.Add(this.btnConnect);
            this.tabServerSettings.Controls.Add(this.chkAutomaticallyConnect);
            this.tabServerSettings.Controls.Add(this.chkRetryOnDisconnect);
            this.tabServerSettings.Controls.Add(this.lblStatus);
            this.tabServerSettings.Location = new System.Drawing.Point(4, 30);
            this.tabServerSettings.Name = "tabServerSettings";
            this.tabServerSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabServerSettings.Size = new System.Drawing.Size(560, 369);
            this.tabServerSettings.TabIndex = 2;
            this.tabServerSettings.Text = "Server";
            // 
            // chkBypassValidation
            // 
            this.chkBypassValidation.AutoSize = true;
            this.chkBypassValidation.BackColor = System.Drawing.Color.Transparent;
            this.chkBypassValidation.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBypassValidation.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkBypassValidation.Location = new System.Drawing.Point(11, 97);
            this.chkBypassValidation.Name = "chkBypassValidation";
            this.chkBypassValidation.Size = new System.Drawing.Size(138, 19);
            this.chkBypassValidation.TabIndex = 23;
            this.chkBypassValidation.Text = "Bypass SSL Validation";
            this.chkBypassValidation.UseVisualStyleBackColor = false;
            this.chkBypassValidation.CheckedChanged += new System.EventHandler(this.chkBypassValidation_CheckedChanged);
            // 
            // btnCloseConnection
            // 
            this.btnCloseConnection.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnCloseConnection.Location = new System.Drawing.Point(90, 222);
            this.btnCloseConnection.Name = "btnCloseConnection";
            this.btnCloseConnection.Size = new System.Drawing.Size(75, 33);
            this.btnCloseConnection.TabIndex = 22;
            this.btnCloseConnection.Text = "Close";
            this.btnCloseConnection.UseVisualStyleBackColor = true;
            this.btnCloseConnection.Click += new System.EventHandler(this.btnCloseConnection_Click);
            // 
            // lblSocketException
            // 
            this.lblSocketException.AutoSize = true;
            this.lblSocketException.BackColor = System.Drawing.Color.Transparent;
            this.lblSocketException.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSocketException.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblSocketException.Location = new System.Drawing.Point(13, 278);
            this.lblSocketException.Name = "lblSocketException";
            this.lblSocketException.Size = new System.Drawing.Size(0, 20);
            this.lblSocketException.TabIndex = 21;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.SteelBlue;
            this.label4.Location = new System.Drawing.Point(6, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(151, 24);
            this.label4.TabIndex = 20;
            this.label4.Text = "Server Settings";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.uiSettingTabs, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.uiBtnOpen, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(574, 593);
            this.tableLayoutPanel1.TabIndex = 26;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.lblMainLogo);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(574, 70);
            this.panel1.TabIndex = 26;
            // 
            // tmrGetSocketStatus
            // 
            this.tmrGetSocketStatus.Enabled = true;
            this.tmrGetSocketStatus.Interval = 250;
            this.tmrGetSocketStatus.Tick += new System.EventHandler(this.tmrGetSocketStatus_Tick);
            // 
            // bgwMetrics
            // 
            this.bgwMetrics.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwMetrics_DoWork);
            this.bgwMetrics.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwMetrics_RunWorkerCompleted);
            // 
            // btnGenerateWikiDocs
            // 
            this.btnGenerateWikiDocs.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerateWikiDocs.Location = new System.Drawing.Point(10, 175);
            this.btnGenerateWikiDocs.Name = "btnGenerateWikiDocs";
            this.btnGenerateWikiDocs.Size = new System.Drawing.Size(207, 27);
            this.btnGenerateWikiDocs.TabIndex = 31;
            this.btnGenerateWikiDocs.Text = "Generate Documentation";
            this.btnGenerateWikiDocs.UseVisualStyleBackColor = true;
            this.btnGenerateWikiDocs.Click += new System.EventHandler(this.btnGenerateWikiDocs_Click);
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundChangeIndex = 300;
            this.ClientSize = new System.Drawing.Size(574, 593);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSettings";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOpen)).EndInit();
            this.uiSettingTabs.ResumeLayout(false);
            this.tabAppSettings.ResumeLayout(false);
            this.tabAppSettings.PerformLayout();
            this.tabDebugSettings.ResumeLayout(false);
            this.tabDebugSettings.PerformLayout();
            this.tabServerSettings.ResumeLayout(false);
            this.tabServerSettings.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtServerURL;
        private System.Windows.Forms.CheckBox chkAutomaticallyConnect;
        private System.Windows.Forms.CheckBox chkServerEnabled;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.CheckBox chkRetryOnDisconnect;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtPublicKey;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkAutoCloseWindow;
        private System.Windows.Forms.CheckBox chkShowDebug;
        private CustomControls.UIPictureButton uiBtnOpen;
        private System.Windows.Forms.CheckBox chkEnableLogging;
        private System.Windows.Forms.Label lblMainLogo;
        private System.Windows.Forms.Label lblOptions;
        private System.Windows.Forms.Label lblApplicationSettings;
        private System.Windows.Forms.CheckBox chkAntiIdle;
        private System.Windows.Forms.Button btnUpdates;
        private System.Windows.Forms.CheckBox chkAdvancedDebug;
        private System.Windows.Forms.CheckBox chkCreateMissingVariables;
        private CustomControls.UITabControl uiSettingTabs;
        private System.Windows.Forms.TabPage tabAppSettings;
        private System.Windows.Forms.TabPage tabDebugSettings;
        private System.Windows.Forms.TabPage tabServerSettings;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblSocketException;
        private System.Windows.Forms.Button btnCloseConnection;
        private System.Windows.Forms.Timer tmrGetSocketStatus;
        private System.Windows.Forms.CheckBox chkBypassValidation;
        private System.Windows.Forms.Label lblRootFolder;
        private System.Windows.Forms.TextBox txtAppFolderPath;
        private System.Windows.Forms.Button btnSelectFolder;
        private System.Windows.Forms.TreeView tvExecutionTimes;
        private System.ComponentModel.BackgroundWorker bgwMetrics;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblMetrics;
        private System.Windows.Forms.Button btnClearMetrics;
        private System.Windows.Forms.CheckBox chkTrackMetrics;
        private System.Windows.Forms.CheckBox chkInsertCommandsInline;
        private System.Windows.Forms.Button btnGenerateWikiDocs;
    }
}