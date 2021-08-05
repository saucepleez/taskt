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
            this.chkSlimActionBar = new System.Windows.Forms.CheckBox();
            this.chkPreloadCommands = new System.Windows.Forms.CheckBox();
            this.btnLaunchDisplayManager = new System.Windows.Forms.Button();
            this.lblStartupMode = new System.Windows.Forms.Label();
            this.cboStartUpMode = new System.Windows.Forms.ComboBox();
            this.btnSelectAttendedTaskFolder = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtAttendedTaskFolder = new System.Windows.Forms.TextBox();
            this.btnLaunchAttendedMode = new System.Windows.Forms.Button();
            this.chkMinimizeToTray = new System.Windows.Forms.CheckBox();
            this.btnGenerateWikiDocs = new System.Windows.Forms.Button();
            this.btnClearMetrics = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.lblMetrics = new System.Windows.Forms.Label();
            this.tvExecutionTimes = new System.Windows.Forms.TreeView();
            this.btnSelectFolder = new System.Windows.Forms.Button();
            this.lblRootFolder = new System.Windows.Forms.Label();
            this.txtAppFolderPath = new System.Windows.Forms.TextBox();
            this.tabDebugSettings = new System.Windows.Forms.TabPage();
            this.txtCurrentWindow = new System.Windows.Forms.TextBox();
            this.lblCurrentWindow = new System.Windows.Forms.Label();
            this.chkAutoCalcVariables = new System.Windows.Forms.CheckBox();
            this.label16 = new System.Windows.Forms.Label();
            this.cboCancellationKey = new System.Windows.Forms.ComboBox();
            this.chkOverrideInstances = new System.Windows.Forms.CheckBox();
            this.lblDelay = new System.Windows.Forms.Label();
            this.txtCommandDelay = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblVariableDisplay = new System.Windows.Forms.Label();
            this.txtVariableEndMarker = new System.Windows.Forms.TextBox();
            this.txtVariableStartMarker = new System.Windows.Forms.TextBox();
            this.chkTrackMetrics = new System.Windows.Forms.CheckBox();
            this.tabServerSettings = new System.Windows.Forms.TabPage();
            this.btnTaskPublish = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.btnGetGUID = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.txtGUID = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtHttpsAddress = new System.Windows.Forms.TextBox();
            this.chkBypassValidation = new System.Windows.Forms.CheckBox();
            this.btnCloseConnection = new System.Windows.Forms.Button();
            this.lblSocketException = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.tabLocalListener = new System.Windows.Forms.TabPage();
            this.txtWhiteList = new System.Windows.Forms.TextBox();
            this.chkEnableWhitelist = new System.Windows.Forms.CheckBox();
            this.chkAutoStartListener = new System.Windows.Forms.CheckBox();
            this.label19 = new System.Windows.Forms.Label();
            this.txtListeningPort = new System.Windows.Forms.TextBox();
            this.lblListeningStatus = new System.Windows.Forms.Label();
            this.btnStopListening = new System.Windows.Forms.Button();
            this.btnStartListening = new System.Windows.Forms.Button();
            this.chkRequireListenerKey = new System.Windows.Forms.CheckBox();
            this.label20 = new System.Windows.Forms.Label();
            this.txtAuthListeningKey = new System.Windows.Forms.TextBox();
            this.chkEnableListening = new System.Windows.Forms.CheckBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.tabEditorSettings = new System.Windows.Forms.TabPage();
            this.chkGruopingBySubgruop = new System.Windows.Forms.CheckBox();
            this.chkInsertCommentIfLoop = new System.Windows.Forms.CheckBox();
            this.txtDefaultDBInstanceName = new System.Windows.Forms.TextBox();
            this.lblDBInstance = new System.Windows.Forms.Label();
            this.chkInsertElse = new System.Windows.Forms.CheckBox();
            this.chkInsertVariablePosition = new System.Windows.Forms.CheckBox();
            this.txtDefaultWordInstanceName = new System.Windows.Forms.TextBox();
            this.lblWordInstance = new System.Windows.Forms.Label();
            this.txtDefaultExcelInstanceName = new System.Windows.Forms.TextBox();
            this.lblExcelInstance = new System.Windows.Forms.Label();
            this.txtDefaultStopwatchInstanceName = new System.Windows.Forms.TextBox();
            this.lblStopwatchInstance = new System.Windows.Forms.Label();
            this.txtDefaultBrowserInstanceName = new System.Windows.Forms.TextBox();
            this.lblBrowserInstance = new System.Windows.Forms.Label();
            this.chkSequenceDragDrop = new System.Windows.Forms.CheckBox();
            this.chkInsertCommandsInline = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tmrGetSocketStatus = new System.Windows.Forms.Timer(this.components);
            this.bgwMetrics = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOpen)).BeginInit();
            this.uiSettingTabs.SuspendLayout();
            this.tabAppSettings.SuspendLayout();
            this.tabDebugSettings.SuspendLayout();
            this.tabServerSettings.SuspendLayout();
            this.tabLocalListener.SuspendLayout();
            this.tabEditorSettings.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnConnect.Location = new System.Drawing.Point(312, 323);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 30);
            this.btnConnect.TabIndex = 17;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Visible = false;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // chkRetryOnDisconnect
            // 
            this.chkRetryOnDisconnect.AutoSize = true;
            this.chkRetryOnDisconnect.BackColor = System.Drawing.Color.Transparent;
            this.chkRetryOnDisconnect.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRetryOnDisconnect.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkRetryOnDisconnect.Location = new System.Drawing.Point(302, 336);
            this.chkRetryOnDisconnect.Name = "chkRetryOnDisconnect";
            this.chkRetryOnDisconnect.Size = new System.Drawing.Size(154, 19);
            this.chkRetryOnDisconnect.TabIndex = 15;
            this.chkRetryOnDisconnect.Text = "Retry If Connection Fails";
            this.chkRetryOnDisconnect.UseVisualStyleBackColor = false;
            this.chkRetryOnDisconnect.Visible = false;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(6, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(452, 26);
            this.label1.TabIndex = 14;
            this.label1.Text = "Manage settings used by the application";
            // 
            // chkAutomaticallyConnect
            // 
            this.chkAutomaticallyConnect.AutoSize = true;
            this.chkAutomaticallyConnect.BackColor = System.Drawing.Color.Transparent;
            this.chkAutomaticallyConnect.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutomaticallyConnect.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkAutomaticallyConnect.Location = new System.Drawing.Point(12, 117);
            this.chkAutomaticallyConnect.Name = "chkAutomaticallyConnect";
            this.chkAutomaticallyConnect.Size = new System.Drawing.Size(158, 24);
            this.chkAutomaticallyConnect.TabIndex = 3;
            this.chkAutomaticallyConnect.Text = "Check In On Startup";
            this.chkAutomaticallyConnect.UseVisualStyleBackColor = false;
            // 
            // chkServerEnabled
            // 
            this.chkServerEnabled.AutoSize = true;
            this.chkServerEnabled.BackColor = System.Drawing.Color.Transparent;
            this.chkServerEnabled.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkServerEnabled.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkServerEnabled.Location = new System.Drawing.Point(12, 94);
            this.chkServerEnabled.Name = "chkServerEnabled";
            this.chkServerEnabled.Size = new System.Drawing.Size(205, 24);
            this.chkServerEnabled.TabIndex = 2;
            this.chkServerEnabled.Text = "Server Connection Enabled";
            this.chkServerEnabled.UseVisualStyleBackColor = false;
            // 
            // txtServerURL
            // 
            this.txtServerURL.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtServerURL.Location = new System.Drawing.Point(275, 332);
            this.txtServerURL.Name = "txtServerURL";
            this.txtServerURL.Size = new System.Drawing.Size(371, 29);
            this.txtServerURL.TabIndex = 11;
            this.txtServerURL.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.SlateGray;
            this.label3.Location = new System.Drawing.Point(290, 336);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(232, 16);
            this.label3.TabIndex = 9;
            this.label3.Text = "Server URL ex. ws://localhost:port/ws)\r";
            this.label3.Visible = false;
            // 
            // txtPublicKey
            // 
            this.txtPublicKey.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtPublicKey.Location = new System.Drawing.Point(275, 327);
            this.txtPublicKey.Name = "txtPublicKey";
            this.txtPublicKey.Size = new System.Drawing.Size(371, 29);
            this.txtPublicKey.TabIndex = 12;
            this.txtPublicKey.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.SlateGray;
            this.label2.Location = new System.Drawing.Point(310, 339);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 16);
            this.label2.TabIndex = 18;
            this.label2.Text = "Connect Key:";
            this.label2.Visible = false;
            // 
            // chkEnableLogging
            // 
            this.chkEnableLogging.AutoSize = true;
            this.chkEnableLogging.BackColor = System.Drawing.Color.Transparent;
            this.chkEnableLogging.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkEnableLogging.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkEnableLogging.Location = new System.Drawing.Point(10, 74);
            this.chkEnableLogging.Name = "chkEnableLogging";
            this.chkEnableLogging.Size = new System.Drawing.Size(212, 25);
            this.chkEnableLogging.TabIndex = 3;
            this.chkEnableLogging.Text = "Enable Diagnostic Logging";
            this.chkEnableLogging.UseVisualStyleBackColor = false;
            // 
            // chkAutoCloseWindow
            // 
            this.chkAutoCloseWindow.AutoSize = true;
            this.chkAutoCloseWindow.BackColor = System.Drawing.Color.Transparent;
            this.chkAutoCloseWindow.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutoCloseWindow.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkAutoCloseWindow.Location = new System.Drawing.Point(10, 54);
            this.chkAutoCloseWindow.Name = "chkAutoCloseWindow";
            this.chkAutoCloseWindow.Size = new System.Drawing.Size(275, 25);
            this.chkAutoCloseWindow.TabIndex = 2;
            this.chkAutoCloseWindow.Text = "Automatically Close Debug Window";
            this.chkAutoCloseWindow.UseVisualStyleBackColor = false;
            // 
            // chkShowDebug
            // 
            this.chkShowDebug.AutoSize = true;
            this.chkShowDebug.BackColor = System.Drawing.Color.Transparent;
            this.chkShowDebug.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkShowDebug.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkShowDebug.Location = new System.Drawing.Point(10, 34);
            this.chkShowDebug.Name = "chkShowDebug";
            this.chkShowDebug.Size = new System.Drawing.Size(177, 25);
            this.chkShowDebug.TabIndex = 1;
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
            this.uiBtnOpen.Image = global::taskt.Properties.Resources.various_ok_button;
            this.uiBtnOpen.IsMouseOver = false;
            this.uiBtnOpen.Location = new System.Drawing.Point(3, 532);
            this.uiBtnOpen.Name = "uiBtnOpen";
            this.uiBtnOpen.Size = new System.Drawing.Size(48, 44);
            this.uiBtnOpen.TabIndex = 13;
            this.uiBtnOpen.TabStop = false;
            this.uiBtnOpen.Text = "Ok";
            this.uiBtnOpen.Click += new System.EventHandler(this.uiBtnOpen_Click);
            // 
            // lblMainLogo
            // 
            this.lblMainLogo.AutoSize = true;
            this.lblMainLogo.BackColor = System.Drawing.Color.Transparent;
            this.lblMainLogo.Font = new System.Drawing.Font("Segoe UI Semilight", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMainLogo.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblMainLogo.Location = new System.Drawing.Point(3, 1);
            this.lblMainLogo.Name = "lblMainLogo";
            this.lblMainLogo.Size = new System.Drawing.Size(125, 45);
            this.lblMainLogo.TabIndex = 14;
            this.lblMainLogo.Text = "settings";
            // 
            // lblOptions
            // 
            this.lblOptions.AutoSize = true;
            this.lblOptions.BackColor = System.Drawing.Color.Transparent;
            this.lblOptions.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOptions.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblOptions.Location = new System.Drawing.Point(6, 6);
            this.lblOptions.Name = "lblOptions";
            this.lblOptions.Size = new System.Drawing.Size(181, 30);
            this.lblOptions.TabIndex = 0;
            this.lblOptions.Text = "Automation Engine";
            // 
            // lblApplicationSettings
            // 
            this.lblApplicationSettings.AutoSize = true;
            this.lblApplicationSettings.BackColor = System.Drawing.Color.Transparent;
            this.lblApplicationSettings.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApplicationSettings.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblApplicationSettings.Location = new System.Drawing.Point(6, 6);
            this.lblApplicationSettings.Name = "lblApplicationSettings";
            this.lblApplicationSettings.Size = new System.Drawing.Size(184, 30);
            this.lblApplicationSettings.TabIndex = 0;
            this.lblApplicationSettings.Text = "Application Settings";
            // 
            // chkAntiIdle
            // 
            this.chkAntiIdle.AutoSize = true;
            this.chkAntiIdle.BackColor = System.Drawing.Color.Transparent;
            this.chkAntiIdle.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAntiIdle.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkAntiIdle.Location = new System.Drawing.Point(10, 39);
            this.chkAntiIdle.Name = "chkAntiIdle";
            this.chkAntiIdle.Size = new System.Drawing.Size(209, 24);
            this.chkAntiIdle.TabIndex = 1;
            this.chkAntiIdle.Text = "Anti-Idle (while app is open)";
            this.chkAntiIdle.UseVisualStyleBackColor = false;
            // 
            // btnUpdates
            // 
            this.btnUpdates.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdates.Location = new System.Drawing.Point(12, 306);
            this.btnUpdates.Name = "btnUpdates";
            this.btnUpdates.Size = new System.Drawing.Size(207, 25);
            this.btnUpdates.TabIndex = 15;
            this.btnUpdates.Text = "Check For Updates";
            this.btnUpdates.UseVisualStyleBackColor = true;
            this.btnUpdates.Click += new System.EventHandler(this.btnUpdateCheck_Click);
            // 
            // chkAdvancedDebug
            // 
            this.chkAdvancedDebug.AutoSize = true;
            this.chkAdvancedDebug.BackColor = System.Drawing.Color.Transparent;
            this.chkAdvancedDebug.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAdvancedDebug.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkAdvancedDebug.Location = new System.Drawing.Point(10, 97);
            this.chkAdvancedDebug.Name = "chkAdvancedDebug";
            this.chkAdvancedDebug.Size = new System.Drawing.Size(344, 25);
            this.chkAdvancedDebug.TabIndex = 4;
            this.chkAdvancedDebug.Text = "Show Advanced Debug Logs During Execution";
            this.chkAdvancedDebug.UseVisualStyleBackColor = false;
            // 
            // chkCreateMissingVariables
            // 
            this.chkCreateMissingVariables.AutoSize = true;
            this.chkCreateMissingVariables.BackColor = System.Drawing.Color.Transparent;
            this.chkCreateMissingVariables.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCreateMissingVariables.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkCreateMissingVariables.Location = new System.Drawing.Point(10, 119);
            this.chkCreateMissingVariables.Name = "chkCreateMissingVariables";
            this.chkCreateMissingVariables.Size = new System.Drawing.Size(318, 25);
            this.chkCreateMissingVariables.TabIndex = 5;
            this.chkCreateMissingVariables.Text = "Create Missing Variables During Execution";
            this.chkCreateMissingVariables.UseVisualStyleBackColor = false;
            // 
            // uiSettingTabs
            // 
            this.uiSettingTabs.Controls.Add(this.tabAppSettings);
            this.uiSettingTabs.Controls.Add(this.tabDebugSettings);
            this.uiSettingTabs.Controls.Add(this.tabServerSettings);
            this.uiSettingTabs.Controls.Add(this.tabLocalListener);
            this.uiSettingTabs.Controls.Add(this.tabEditorSettings);
            this.uiSettingTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiSettingTabs.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uiSettingTabs.Location = new System.Drawing.Point(3, 68);
            this.uiSettingTabs.Name = "uiSettingTabs";
            this.uiSettingTabs.SelectedIndex = 0;
            this.uiSettingTabs.Size = new System.Drawing.Size(632, 458);
            this.uiSettingTabs.TabIndex = 1;
            // 
            // tabAppSettings
            // 
            this.tabAppSettings.AutoScroll = true;
            this.tabAppSettings.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabAppSettings.Controls.Add(this.chkSlimActionBar);
            this.tabAppSettings.Controls.Add(this.chkPreloadCommands);
            this.tabAppSettings.Controls.Add(this.btnLaunchDisplayManager);
            this.tabAppSettings.Controls.Add(this.lblStartupMode);
            this.tabAppSettings.Controls.Add(this.cboStartUpMode);
            this.tabAppSettings.Controls.Add(this.btnSelectAttendedTaskFolder);
            this.tabAppSettings.Controls.Add(this.label6);
            this.tabAppSettings.Controls.Add(this.txtAttendedTaskFolder);
            this.tabAppSettings.Controls.Add(this.btnLaunchAttendedMode);
            this.tabAppSettings.Controls.Add(this.chkMinimizeToTray);
            this.tabAppSettings.Controls.Add(this.btnGenerateWikiDocs);
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
            this.tabAppSettings.Size = new System.Drawing.Size(624, 424);
            this.tabAppSettings.TabIndex = 0;
            this.tabAppSettings.Text = "Application";
            // 
            // chkSlimActionBar
            // 
            this.chkSlimActionBar.AutoSize = true;
            this.chkSlimActionBar.BackColor = System.Drawing.Color.Transparent;
            this.chkSlimActionBar.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSlimActionBar.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkSlimActionBar.Location = new System.Drawing.Point(10, 83);
            this.chkSlimActionBar.Name = "chkSlimActionBar";
            this.chkSlimActionBar.Size = new System.Drawing.Size(154, 24);
            this.chkSlimActionBar.TabIndex = 5;
            this.chkSlimActionBar.Text = "Use Slim Action Bar";
            this.chkSlimActionBar.UseVisualStyleBackColor = false;
            // 
            // chkPreloadCommands
            // 
            this.chkPreloadCommands.AutoSize = true;
            this.chkPreloadCommands.BackColor = System.Drawing.Color.Transparent;
            this.chkPreloadCommands.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPreloadCommands.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkPreloadCommands.Location = new System.Drawing.Point(10, 105);
            this.chkPreloadCommands.Name = "chkPreloadCommands";
            this.chkPreloadCommands.Size = new System.Drawing.Size(320, 24);
            this.chkPreloadCommands.TabIndex = 6;
            this.chkPreloadCommands.Text = "Load Commands at Startup (Reduces Flicker)";
            this.chkPreloadCommands.UseVisualStyleBackColor = false;
            this.chkPreloadCommands.Visible = false;
            // 
            // btnLaunchDisplayManager
            // 
            this.btnLaunchDisplayManager.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLaunchDisplayManager.Location = new System.Drawing.Point(225, 336);
            this.btnLaunchDisplayManager.Name = "btnLaunchDisplayManager";
            this.btnLaunchDisplayManager.Size = new System.Drawing.Size(207, 25);
            this.btnLaunchDisplayManager.TabIndex = 18;
            this.btnLaunchDisplayManager.Text = "Display Manager for VMs";
            this.btnLaunchDisplayManager.UseVisualStyleBackColor = true;
            this.btnLaunchDisplayManager.Click += new System.EventHandler(this.btnLaunchDisplayManager_Click);
            // 
            // lblStartupMode
            // 
            this.lblStartupMode.AutoSize = true;
            this.lblStartupMode.BackColor = System.Drawing.Color.Transparent;
            this.lblStartupMode.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStartupMode.ForeColor = System.Drawing.Color.SlateGray;
            this.lblStartupMode.Location = new System.Drawing.Point(10, 253);
            this.lblStartupMode.Name = "lblStartupMode";
            this.lblStartupMode.Size = new System.Drawing.Size(70, 17);
            this.lblStartupMode.TabIndex = 13;
            this.lblStartupMode.Text = "Start Mode";
            // 
            // cboStartUpMode
            // 
            this.cboStartUpMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStartUpMode.FormattingEnabled = true;
            this.cboStartUpMode.Items.AddRange(new object[] {
            "Builder Mode",
            "Attended Task Mode"});
            this.cboStartUpMode.Location = new System.Drawing.Point(13, 270);
            this.cboStartUpMode.Name = "cboStartUpMode";
            this.cboStartUpMode.Size = new System.Drawing.Size(219, 29);
            this.cboStartUpMode.TabIndex = 14;
            // 
            // btnSelectAttendedTaskFolder
            // 
            this.btnSelectAttendedTaskFolder.Location = new System.Drawing.Point(504, 222);
            this.btnSelectAttendedTaskFolder.Name = "btnSelectAttendedTaskFolder";
            this.btnSelectAttendedTaskFolder.Size = new System.Drawing.Size(42, 28);
            this.btnSelectAttendedTaskFolder.TabIndex = 12;
            this.btnSelectAttendedTaskFolder.Text = "...";
            this.btnSelectAttendedTaskFolder.UseVisualStyleBackColor = true;
            this.btnSelectAttendedTaskFolder.Click += new System.EventHandler(this.btnSelectAttendedTaskFolder_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.SlateGray;
            this.label6.Location = new System.Drawing.Point(9, 206);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(130, 17);
            this.label6.TabIndex = 10;
            this.label6.Text = "Attended Tasks Folder";
            // 
            // txtAttendedTaskFolder
            // 
            this.txtAttendedTaskFolder.Location = new System.Drawing.Point(12, 223);
            this.txtAttendedTaskFolder.Name = "txtAttendedTaskFolder";
            this.txtAttendedTaskFolder.Size = new System.Drawing.Size(490, 29);
            this.txtAttendedTaskFolder.TabIndex = 11;
            // 
            // btnLaunchAttendedMode
            // 
            this.btnLaunchAttendedMode.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLaunchAttendedMode.Location = new System.Drawing.Point(225, 306);
            this.btnLaunchAttendedMode.Name = "btnLaunchAttendedMode";
            this.btnLaunchAttendedMode.Size = new System.Drawing.Size(207, 25);
            this.btnLaunchAttendedMode.TabIndex = 16;
            this.btnLaunchAttendedMode.Text = "Launch Attended Mode";
            this.btnLaunchAttendedMode.UseVisualStyleBackColor = true;
            this.btnLaunchAttendedMode.Click += new System.EventHandler(this.btnLaunchAttendedMode_Click);
            // 
            // chkMinimizeToTray
            // 
            this.chkMinimizeToTray.AutoSize = true;
            this.chkMinimizeToTray.BackColor = System.Drawing.Color.Transparent;
            this.chkMinimizeToTray.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkMinimizeToTray.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkMinimizeToTray.Location = new System.Drawing.Point(10, 61);
            this.chkMinimizeToTray.Name = "chkMinimizeToTray";
            this.chkMinimizeToTray.Size = new System.Drawing.Size(186, 24);
            this.chkMinimizeToTray.TabIndex = 4;
            this.chkMinimizeToTray.Text = "Minimize to System Tray";
            this.chkMinimizeToTray.UseVisualStyleBackColor = false;
            // 
            // btnGenerateWikiDocs
            // 
            this.btnGenerateWikiDocs.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerateWikiDocs.Location = new System.Drawing.Point(12, 336);
            this.btnGenerateWikiDocs.Name = "btnGenerateWikiDocs";
            this.btnGenerateWikiDocs.Size = new System.Drawing.Size(207, 25);
            this.btnGenerateWikiDocs.TabIndex = 17;
            this.btnGenerateWikiDocs.Text = "Generate Documentation";
            this.btnGenerateWikiDocs.UseVisualStyleBackColor = true;
            this.btnGenerateWikiDocs.Click += new System.EventHandler(this.btnGenerateWikiDocs_Click);
            // 
            // btnClearMetrics
            // 
            this.btnClearMetrics.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearMetrics.Location = new System.Drawing.Point(11, 509);
            this.btnClearMetrics.Name = "btnClearMetrics";
            this.btnClearMetrics.Size = new System.Drawing.Size(108, 23);
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
            this.label5.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.SlateGray;
            this.label5.Location = new System.Drawing.Point(13, 370);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(248, 17);
            this.label5.TabIndex = 19;
            this.label5.Text = "Script Execution Metrics (Last 10 per Script)";
            // 
            // lblMetrics
            // 
            this.lblMetrics.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDownGrid;
            this.lblMetrics.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMetrics.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblMetrics.Location = new System.Drawing.Point(12, 388);
            this.lblMetrics.Name = "lblMetrics";
            this.lblMetrics.Size = new System.Drawing.Size(534, 117);
            this.lblMetrics.TabIndex = 20;
            this.lblMetrics.Text = "Getting Metrics...";
            // 
            // tvExecutionTimes
            // 
            this.tvExecutionTimes.Location = new System.Drawing.Point(12, 388);
            this.tvExecutionTimes.Name = "tvExecutionTimes";
            this.tvExecutionTimes.Size = new System.Drawing.Size(534, 118);
            this.tvExecutionTimes.TabIndex = 26;
            this.tvExecutionTimes.Visible = false;
            // 
            // btnSelectFolder
            // 
            this.btnSelectFolder.Location = new System.Drawing.Point(504, 175);
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.Size = new System.Drawing.Size(42, 28);
            this.btnSelectFolder.TabIndex = 9;
            this.btnSelectFolder.Text = "...";
            this.btnSelectFolder.UseVisualStyleBackColor = true;
            this.btnSelectFolder.Click += new System.EventHandler(this.btnSelectFolder_Click);
            // 
            // lblRootFolder
            // 
            this.lblRootFolder.AutoSize = true;
            this.lblRootFolder.BackColor = System.Drawing.Color.Transparent;
            this.lblRootFolder.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRootFolder.ForeColor = System.Drawing.Color.SlateGray;
            this.lblRootFolder.Location = new System.Drawing.Point(9, 158);
            this.lblRootFolder.Name = "lblRootFolder";
            this.lblRootFolder.Size = new System.Drawing.Size(101, 17);
            this.lblRootFolder.TabIndex = 7;
            this.lblRootFolder.Text = "taskt Root Folder";
            // 
            // txtAppFolderPath
            // 
            this.txtAppFolderPath.Location = new System.Drawing.Point(12, 175);
            this.txtAppFolderPath.Name = "txtAppFolderPath";
            this.txtAppFolderPath.Size = new System.Drawing.Size(490, 29);
            this.txtAppFolderPath.TabIndex = 8;
            // 
            // tabDebugSettings
            // 
            this.tabDebugSettings.AutoScroll = true;
            this.tabDebugSettings.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabDebugSettings.Controls.Add(this.txtCurrentWindow);
            this.tabDebugSettings.Controls.Add(this.lblCurrentWindow);
            this.tabDebugSettings.Controls.Add(this.chkAutoCalcVariables);
            this.tabDebugSettings.Controls.Add(this.label16);
            this.tabDebugSettings.Controls.Add(this.cboCancellationKey);
            this.tabDebugSettings.Controls.Add(this.chkOverrideInstances);
            this.tabDebugSettings.Controls.Add(this.lblDelay);
            this.tabDebugSettings.Controls.Add(this.txtCommandDelay);
            this.tabDebugSettings.Controls.Add(this.label10);
            this.tabDebugSettings.Controls.Add(this.label9);
            this.tabDebugSettings.Controls.Add(this.label8);
            this.tabDebugSettings.Controls.Add(this.label7);
            this.tabDebugSettings.Controls.Add(this.lblVariableDisplay);
            this.tabDebugSettings.Controls.Add(this.txtVariableEndMarker);
            this.tabDebugSettings.Controls.Add(this.txtVariableStartMarker);
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
            this.tabDebugSettings.Size = new System.Drawing.Size(624, 424);
            this.tabDebugSettings.TabIndex = 1;
            this.tabDebugSettings.Text = "Automation Engine";
            // 
            // txtCurrentWindow
            // 
            this.txtCurrentWindow.Location = new System.Drawing.Point(185, 445);
            this.txtCurrentWindow.Name = "txtCurrentWindow";
            this.txtCurrentWindow.Size = new System.Drawing.Size(204, 29);
            this.txtCurrentWindow.TabIndex = 21;
            // 
            // lblCurrentWindow
            // 
            this.lblCurrentWindow.AutoSize = true;
            this.lblCurrentWindow.BackColor = System.Drawing.Color.Transparent;
            this.lblCurrentWindow.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentWindow.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblCurrentWindow.Location = new System.Drawing.Point(6, 448);
            this.lblCurrentWindow.Name = "lblCurrentWindow";
            this.lblCurrentWindow.Size = new System.Drawing.Size(183, 21);
            this.lblCurrentWindow.TabIndex = 20;
            this.lblCurrentWindow.Text = "Current window keyword:";
            // 
            // chkAutoCalcVariables
            // 
            this.chkAutoCalcVariables.AutoSize = true;
            this.chkAutoCalcVariables.BackColor = System.Drawing.Color.Transparent;
            this.chkAutoCalcVariables.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutoCalcVariables.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkAutoCalcVariables.Location = new System.Drawing.Point(10, 183);
            this.chkAutoCalcVariables.Name = "chkAutoCalcVariables";
            this.chkAutoCalcVariables.Size = new System.Drawing.Size(255, 25);
            this.chkAutoCalcVariables.TabIndex = 10;
            this.chkAutoCalcVariables.Text = "Calculate Variables Automatically";
            this.chkAutoCalcVariables.UseVisualStyleBackColor = false;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.SteelBlue;
            this.label16.Location = new System.Drawing.Point(6, 406);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(134, 21);
            this.label16.TabIndex = 0;
            this.label16.Text = "End Script Hotkey:";
            // 
            // cboCancellationKey
            // 
            this.cboCancellationKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCancellationKey.FormattingEnabled = true;
            this.cboCancellationKey.Location = new System.Drawing.Point(142, 405);
            this.cboCancellationKey.Name = "cboCancellationKey";
            this.cboCancellationKey.Size = new System.Drawing.Size(154, 29);
            this.cboCancellationKey.TabIndex = 1;
            // 
            // chkOverrideInstances
            // 
            this.chkOverrideInstances.AutoSize = true;
            this.chkOverrideInstances.BackColor = System.Drawing.Color.Transparent;
            this.chkOverrideInstances.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkOverrideInstances.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkOverrideInstances.Location = new System.Drawing.Point(10, 161);
            this.chkOverrideInstances.Name = "chkOverrideInstances";
            this.chkOverrideInstances.Size = new System.Drawing.Size(187, 25);
            this.chkOverrideInstances.TabIndex = 9;
            this.chkOverrideInstances.Text = "Override App Instances";
            this.chkOverrideInstances.UseVisualStyleBackColor = false;
            this.chkOverrideInstances.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // lblDelay
            // 
            this.lblDelay.AutoSize = true;
            this.lblDelay.BackColor = System.Drawing.Color.Transparent;
            this.lblDelay.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDelay.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblDelay.Location = new System.Drawing.Point(7, 218);
            this.lblDelay.Name = "lblDelay";
            this.lblDelay.Size = new System.Drawing.Size(349, 21);
            this.lblDelay.TabIndex = 11;
            this.lblDelay.Text = "Default delay between executing commands (ms):";
            // 
            // txtCommandDelay
            // 
            this.txtCommandDelay.Location = new System.Drawing.Point(357, 215);
            this.txtCommandDelay.Name = "txtCommandDelay";
            this.txtCommandDelay.Size = new System.Drawing.Size(77, 29);
            this.txtCommandDelay.TabIndex = 12;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.SteelBlue;
            this.label10.Location = new System.Drawing.Point(20, 367);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(98, 21);
            this.label10.TabIndex = 17;
            this.label10.Text = "End Marker:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.SteelBlue;
            this.label9.Location = new System.Drawing.Point(12, 336);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(106, 21);
            this.label9.TabIndex = 15;
            this.label9.Text = "Start Marker:";
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.SteelBlue;
            this.label8.Location = new System.Drawing.Point(10, 265);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(410, 66);
            this.label8.TabIndex = 14;
            this.label8.Text = "Indicate the start and end markers for variables.  When the engine runs, it will " +
    "automatically replace the variable with the stored value.";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.SteelBlue;
            this.label7.Location = new System.Drawing.Point(6, 240);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(149, 30);
            this.label7.TabIndex = 13;
            this.label7.Text = "Variable Pattern";
            // 
            // lblVariableDisplay
            // 
            this.lblVariableDisplay.AutoSize = true;
            this.lblVariableDisplay.BackColor = System.Drawing.Color.Transparent;
            this.lblVariableDisplay.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVariableDisplay.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblVariableDisplay.Location = new System.Drawing.Point(163, 350);
            this.lblVariableDisplay.Name = "lblVariableDisplay";
            this.lblVariableDisplay.Size = new System.Drawing.Size(133, 25);
            this.lblVariableDisplay.TabIndex = 19;
            this.lblVariableDisplay.Text = "VariableName";
            // 
            // txtVariableEndMarker
            // 
            this.txtVariableEndMarker.Location = new System.Drawing.Point(119, 364);
            this.txtVariableEndMarker.Name = "txtVariableEndMarker";
            this.txtVariableEndMarker.Size = new System.Drawing.Size(26, 29);
            this.txtVariableEndMarker.TabIndex = 18;
            this.txtVariableEndMarker.TextChanged += new System.EventHandler(this.txtVariableStartMarker_TextChanged);
            // 
            // txtVariableStartMarker
            // 
            this.txtVariableStartMarker.Location = new System.Drawing.Point(119, 333);
            this.txtVariableStartMarker.Name = "txtVariableStartMarker";
            this.txtVariableStartMarker.Size = new System.Drawing.Size(26, 29);
            this.txtVariableStartMarker.TabIndex = 16;
            this.txtVariableStartMarker.TextChanged += new System.EventHandler(this.txtVariableStartMarker_TextChanged);
            // 
            // chkTrackMetrics
            // 
            this.chkTrackMetrics.AutoSize = true;
            this.chkTrackMetrics.BackColor = System.Drawing.Color.Transparent;
            this.chkTrackMetrics.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkTrackMetrics.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkTrackMetrics.Location = new System.Drawing.Point(10, 141);
            this.chkTrackMetrics.Name = "chkTrackMetrics";
            this.chkTrackMetrics.Size = new System.Drawing.Size(188, 25);
            this.chkTrackMetrics.TabIndex = 6;
            this.chkTrackMetrics.Text = "Track Execution Metrics";
            this.chkTrackMetrics.UseVisualStyleBackColor = false;
            // 
            // tabServerSettings
            // 
            this.tabServerSettings.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabServerSettings.Controls.Add(this.btnTaskPublish);
            this.tabServerSettings.Controls.Add(this.label15);
            this.tabServerSettings.Controls.Add(this.label14);
            this.tabServerSettings.Controls.Add(this.label13);
            this.tabServerSettings.Controls.Add(this.btnGetGUID);
            this.tabServerSettings.Controls.Add(this.label12);
            this.tabServerSettings.Controls.Add(this.txtGUID);
            this.tabServerSettings.Controls.Add(this.label11);
            this.tabServerSettings.Controls.Add(this.txtHttpsAddress);
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
            this.tabServerSettings.Size = new System.Drawing.Size(624, 424);
            this.tabServerSettings.TabIndex = 2;
            this.tabServerSettings.Text = "Server";
            // 
            // btnTaskPublish
            // 
            this.btnTaskPublish.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnTaskPublish.Location = new System.Drawing.Point(14, 317);
            this.btnTaskPublish.Name = "btnTaskPublish";
            this.btnTaskPublish.Size = new System.Drawing.Size(147, 30);
            this.btnTaskPublish.TabIndex = 11;
            this.btnTaskPublish.Text = "Publish Task";
            this.btnTaskPublish.UseVisualStyleBackColor = true;
            this.btnTaskPublish.Click += new System.EventHandler(this.btnTaskPublish_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.SteelBlue;
            this.label15.Location = new System.Drawing.Point(14, 261);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(453, 20);
            this.label15.TabIndex = 9;
            this.label15.Text = "Indicates the GUID the client will use when connecting to taskt server";
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.SlateGray;
            this.label14.Location = new System.Drawing.Point(10, 33);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(587, 62);
            this.label14.TabIndex = 1;
            this.label14.Text = resources.GetString("label14.Text");
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.SteelBlue;
            this.label13.Location = new System.Drawing.Point(10, 162);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(418, 20);
            this.label13.TabIndex = 5;
            this.label13.Text = "Enter the location of the taskt server ex. https://localhost:60281";
            // 
            // btnGetGUID
            // 
            this.btnGetGUID.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnGetGUID.Location = new System.Drawing.Point(12, 213);
            this.btnGetGUID.Name = "btnGetGUID";
            this.btnGetGUID.Size = new System.Drawing.Size(147, 30);
            this.btnGetGUID.TabIndex = 7;
            this.btnGetGUID.Text = "Test Connection";
            this.btnGetGUID.UseVisualStyleBackColor = true;
            this.btnGetGUID.Click += new System.EventHandler(this.btnGetGUID_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.SlateGray;
            this.label12.Location = new System.Drawing.Point(14, 247);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(74, 17);
            this.label12.TabIndex = 8;
            this.label12.Text = "Client GUID";
            // 
            // txtGUID
            // 
            this.txtGUID.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtGUID.Location = new System.Drawing.Point(15, 284);
            this.txtGUID.Name = "txtGUID";
            this.txtGUID.Size = new System.Drawing.Size(371, 29);
            this.txtGUID.TabIndex = 10;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.SlateGray;
            this.label11.Location = new System.Drawing.Point(11, 142);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(111, 17);
            this.label11.TabIndex = 4;
            this.label11.Text = "HTTPS Server URL";
            // 
            // txtHttpsAddress
            // 
            this.txtHttpsAddress.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtHttpsAddress.Location = new System.Drawing.Point(13, 183);
            this.txtHttpsAddress.Name = "txtHttpsAddress";
            this.txtHttpsAddress.Size = new System.Drawing.Size(371, 29);
            this.txtHttpsAddress.TabIndex = 6;
            // 
            // chkBypassValidation
            // 
            this.chkBypassValidation.AutoSize = true;
            this.chkBypassValidation.BackColor = System.Drawing.Color.Transparent;
            this.chkBypassValidation.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBypassValidation.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkBypassValidation.Location = new System.Drawing.Point(337, 332);
            this.chkBypassValidation.Name = "chkBypassValidation";
            this.chkBypassValidation.Size = new System.Drawing.Size(138, 19);
            this.chkBypassValidation.TabIndex = 23;
            this.chkBypassValidation.Text = "Bypass SSL Validation";
            this.chkBypassValidation.UseVisualStyleBackColor = false;
            this.chkBypassValidation.Visible = false;
            this.chkBypassValidation.CheckedChanged += new System.EventHandler(this.chkBypassValidation_CheckedChanged);
            // 
            // btnCloseConnection
            // 
            this.btnCloseConnection.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnCloseConnection.Location = new System.Drawing.Point(313, 323);
            this.btnCloseConnection.Name = "btnCloseConnection";
            this.btnCloseConnection.Size = new System.Drawing.Size(75, 30);
            this.btnCloseConnection.TabIndex = 22;
            this.btnCloseConnection.Text = "Close";
            this.btnCloseConnection.UseVisualStyleBackColor = true;
            this.btnCloseConnection.Visible = false;
            this.btnCloseConnection.Click += new System.EventHandler(this.btnCloseConnection_Click);
            // 
            // lblSocketException
            // 
            this.lblSocketException.AutoSize = true;
            this.lblSocketException.BackColor = System.Drawing.Color.Transparent;
            this.lblSocketException.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSocketException.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblSocketException.Location = new System.Drawing.Point(78, 405);
            this.lblSocketException.Name = "lblSocketException";
            this.lblSocketException.Size = new System.Drawing.Size(0, 20);
            this.lblSocketException.TabIndex = 21;
            this.lblSocketException.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.SteelBlue;
            this.label4.Location = new System.Drawing.Point(7, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(144, 30);
            this.label4.TabIndex = 0;
            this.label4.Text = "Server Settings";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblStatus.Location = new System.Drawing.Point(78, 374);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 20);
            this.lblStatus.TabIndex = 16;
            this.lblStatus.Visible = false;
            // 
            // tabLocalListener
            // 
            this.tabLocalListener.Controls.Add(this.txtWhiteList);
            this.tabLocalListener.Controls.Add(this.chkEnableWhitelist);
            this.tabLocalListener.Controls.Add(this.chkAutoStartListener);
            this.tabLocalListener.Controls.Add(this.label19);
            this.tabLocalListener.Controls.Add(this.txtListeningPort);
            this.tabLocalListener.Controls.Add(this.lblListeningStatus);
            this.tabLocalListener.Controls.Add(this.btnStopListening);
            this.tabLocalListener.Controls.Add(this.btnStartListening);
            this.tabLocalListener.Controls.Add(this.chkRequireListenerKey);
            this.tabLocalListener.Controls.Add(this.label20);
            this.tabLocalListener.Controls.Add(this.txtAuthListeningKey);
            this.tabLocalListener.Controls.Add(this.chkEnableListening);
            this.tabLocalListener.Controls.Add(this.label17);
            this.tabLocalListener.Controls.Add(this.label18);
            this.tabLocalListener.Location = new System.Drawing.Point(4, 30);
            this.tabLocalListener.Name = "tabLocalListener";
            this.tabLocalListener.Padding = new System.Windows.Forms.Padding(3);
            this.tabLocalListener.Size = new System.Drawing.Size(624, 424);
            this.tabLocalListener.TabIndex = 3;
            this.tabLocalListener.Text = "Local Listener";
            this.tabLocalListener.UseVisualStyleBackColor = true;
            // 
            // txtWhiteList
            // 
            this.txtWhiteList.Location = new System.Drawing.Point(15, 269);
            this.txtWhiteList.Multiline = true;
            this.txtWhiteList.Name = "txtWhiteList";
            this.txtWhiteList.Size = new System.Drawing.Size(371, 86);
            this.txtWhiteList.TabIndex = 10;
            // 
            // chkEnableWhitelist
            // 
            this.chkEnableWhitelist.AutoSize = true;
            this.chkEnableWhitelist.BackColor = System.Drawing.Color.Transparent;
            this.chkEnableWhitelist.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkEnableWhitelist.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkEnableWhitelist.Location = new System.Drawing.Point(14, 241);
            this.chkEnableWhitelist.Name = "chkEnableWhitelist";
            this.chkEnableWhitelist.Size = new System.Drawing.Size(318, 24);
            this.chkEnableWhitelist.TabIndex = 9;
            this.chkEnableWhitelist.Text = "Enable IP Verification (Seperate with comma)";
            this.chkEnableWhitelist.UseVisualStyleBackColor = false;
            // 
            // chkAutoStartListener
            // 
            this.chkAutoStartListener.AutoSize = true;
            this.chkAutoStartListener.BackColor = System.Drawing.Color.Transparent;
            this.chkAutoStartListener.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutoStartListener.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkAutoStartListener.Location = new System.Drawing.Point(14, 76);
            this.chkAutoStartListener.Name = "chkAutoStartListener";
            this.chkAutoStartListener.Size = new System.Drawing.Size(193, 24);
            this.chkAutoStartListener.TabIndex = 2;
            this.chkAutoStartListener.Text = "Start Listening on Startup";
            this.chkAutoStartListener.UseVisualStyleBackColor = false;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.BackColor = System.Drawing.Color.Transparent;
            this.label19.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.SlateGray;
            this.label19.Location = new System.Drawing.Point(11, 118);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(83, 17);
            this.label19.TabIndex = 4;
            this.label19.Text = "Listening Port";
            // 
            // txtListeningPort
            // 
            this.txtListeningPort.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtListeningPort.Location = new System.Drawing.Point(14, 137);
            this.txtListeningPort.Name = "txtListeningPort";
            this.txtListeningPort.Size = new System.Drawing.Size(132, 29);
            this.txtListeningPort.TabIndex = 5;
            // 
            // lblListeningStatus
            // 
            this.lblListeningStatus.AutoSize = true;
            this.lblListeningStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblListeningStatus.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblListeningStatus.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblListeningStatus.Location = new System.Drawing.Point(10, 394);
            this.lblListeningStatus.Name = "lblListeningStatus";
            this.lblListeningStatus.Size = new System.Drawing.Size(137, 30);
            this.lblListeningStatus.TabIndex = 13;
            this.lblListeningStatus.Text = "Listening on {}";
            this.lblListeningStatus.Visible = false;
            // 
            // btnStopListening
            // 
            this.btnStopListening.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnStopListening.Location = new System.Drawing.Point(167, 361);
            this.btnStopListening.Name = "btnStopListening";
            this.btnStopListening.Size = new System.Drawing.Size(147, 30);
            this.btnStopListening.TabIndex = 12;
            this.btnStopListening.Text = "Stop Listening";
            this.btnStopListening.UseVisualStyleBackColor = true;
            this.btnStopListening.Click += new System.EventHandler(this.btnStopListening_Click);
            // 
            // btnStartListening
            // 
            this.btnStartListening.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnStartListening.Location = new System.Drawing.Point(14, 361);
            this.btnStartListening.Name = "btnStartListening";
            this.btnStartListening.Size = new System.Drawing.Size(147, 30);
            this.btnStartListening.TabIndex = 11;
            this.btnStartListening.Text = "Start Listening";
            this.btnStartListening.UseVisualStyleBackColor = true;
            this.btnStartListening.Click += new System.EventHandler(this.btnStartListening_Click);
            // 
            // chkRequireListenerKey
            // 
            this.chkRequireListenerKey.AutoSize = true;
            this.chkRequireListenerKey.BackColor = System.Drawing.Color.Transparent;
            this.chkRequireListenerKey.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRequireListenerKey.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkRequireListenerKey.Location = new System.Drawing.Point(14, 169);
            this.chkRequireListenerKey.Name = "chkRequireListenerKey";
            this.chkRequireListenerKey.Size = new System.Drawing.Size(204, 24);
            this.chkRequireListenerKey.TabIndex = 6;
            this.chkRequireListenerKey.Text = "Require Authentication Key";
            this.chkRequireListenerKey.UseVisualStyleBackColor = false;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.BackColor = System.Drawing.Color.Transparent;
            this.label20.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.ForeColor = System.Drawing.Color.SlateGray;
            this.label20.Location = new System.Drawing.Point(11, 193);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(113, 17);
            this.label20.TabIndex = 7;
            this.label20.Text = "Authentication Key";
            // 
            // txtAuthListeningKey
            // 
            this.txtAuthListeningKey.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtAuthListeningKey.Location = new System.Drawing.Point(14, 211);
            this.txtAuthListeningKey.Name = "txtAuthListeningKey";
            this.txtAuthListeningKey.Size = new System.Drawing.Size(371, 29);
            this.txtAuthListeningKey.TabIndex = 8;
            // 
            // chkEnableListening
            // 
            this.chkEnableListening.AutoSize = true;
            this.chkEnableListening.BackColor = System.Drawing.Color.Transparent;
            this.chkEnableListening.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkEnableListening.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkEnableListening.Location = new System.Drawing.Point(14, 97);
            this.chkEnableListening.Name = "chkEnableListening";
            this.chkEnableListening.Size = new System.Drawing.Size(180, 24);
            this.chkEnableListening.TabIndex = 3;
            this.chkEnableListening.Text = "Local Listening Enabled";
            this.chkEnableListening.UseVisualStyleBackColor = false;
            // 
            // label17
            // 
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.SlateGray;
            this.label17.Location = new System.Drawing.Point(10, 33);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(587, 62);
            this.label17.TabIndex = 1;
            this.label17.Text = "Enable this functionality to allow this computer to accept script execution reque" +
    "sts from other taskt or REST-capable clients.";
            this.label17.Click += new System.EventHandler(this.label17_Click);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.BackColor = System.Drawing.Color.Transparent;
            this.label18.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.SteelBlue;
            this.label18.Location = new System.Drawing.Point(7, 6);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(268, 30);
            this.label18.TabIndex = 0;
            this.label18.Text = "Local Listener Settings (BETA)";
            // 
            // tabEditorSettings
            // 
            this.tabEditorSettings.AutoScroll = true;
            this.tabEditorSettings.Controls.Add(this.chkGruopingBySubgruop);
            this.tabEditorSettings.Controls.Add(this.chkInsertCommentIfLoop);
            this.tabEditorSettings.Controls.Add(this.txtDefaultDBInstanceName);
            this.tabEditorSettings.Controls.Add(this.lblDBInstance);
            this.tabEditorSettings.Controls.Add(this.chkInsertElse);
            this.tabEditorSettings.Controls.Add(this.chkInsertVariablePosition);
            this.tabEditorSettings.Controls.Add(this.txtDefaultWordInstanceName);
            this.tabEditorSettings.Controls.Add(this.lblWordInstance);
            this.tabEditorSettings.Controls.Add(this.txtDefaultExcelInstanceName);
            this.tabEditorSettings.Controls.Add(this.lblExcelInstance);
            this.tabEditorSettings.Controls.Add(this.txtDefaultStopwatchInstanceName);
            this.tabEditorSettings.Controls.Add(this.lblStopwatchInstance);
            this.tabEditorSettings.Controls.Add(this.txtDefaultBrowserInstanceName);
            this.tabEditorSettings.Controls.Add(this.lblBrowserInstance);
            this.tabEditorSettings.Controls.Add(this.chkSequenceDragDrop);
            this.tabEditorSettings.Controls.Add(this.chkInsertCommandsInline);
            this.tabEditorSettings.Location = new System.Drawing.Point(4, 30);
            this.tabEditorSettings.Name = "tabEditorSettings";
            this.tabEditorSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabEditorSettings.Size = new System.Drawing.Size(624, 424);
            this.tabEditorSettings.TabIndex = 4;
            this.tabEditorSettings.Text = "Editor";
            this.tabEditorSettings.UseVisualStyleBackColor = true;
            // 
            // chkGruopingBySubgruop
            // 
            this.chkGruopingBySubgruop.AutoSize = true;
            this.chkGruopingBySubgruop.BackColor = System.Drawing.Color.Transparent;
            this.chkGruopingBySubgruop.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkGruopingBySubgruop.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkGruopingBySubgruop.Location = new System.Drawing.Point(10, 161);
            this.chkGruopingBySubgruop.Name = "chkGruopingBySubgruop";
            this.chkGruopingBySubgruop.Size = new System.Drawing.Size(176, 24);
            this.chkGruopingBySubgruop.TabIndex = 21;
            this.chkGruopingBySubgruop.Text = "Gruoping by subgroup";
            this.chkGruopingBySubgruop.UseVisualStyleBackColor = false;
            // 
            // chkInsertCommentIfLoop
            // 
            this.chkInsertCommentIfLoop.AutoSize = true;
            this.chkInsertCommentIfLoop.BackColor = System.Drawing.Color.Transparent;
            this.chkInsertCommentIfLoop.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkInsertCommentIfLoop.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkInsertCommentIfLoop.Location = new System.Drawing.Point(10, 131);
            this.chkInsertCommentIfLoop.Name = "chkInsertCommentIfLoop";
            this.chkInsertCommentIfLoop.Size = new System.Drawing.Size(257, 24);
            this.chkInsertCommentIfLoop.TabIndex = 20;
            this.chkInsertCommentIfLoop.Text = "Insert Comment above If, Loop, Try";
            this.chkInsertCommentIfLoop.UseVisualStyleBackColor = false;
            // 
            // txtDefaultDBInstanceName
            // 
            this.txtDefaultDBInstanceName.Location = new System.Drawing.Point(10, 438);
            this.txtDefaultDBInstanceName.Name = "txtDefaultDBInstanceName";
            this.txtDefaultDBInstanceName.Size = new System.Drawing.Size(490, 29);
            this.txtDefaultDBInstanceName.TabIndex = 19;
            // 
            // lblDBInstance
            // 
            this.lblDBInstance.AutoSize = true;
            this.lblDBInstance.BackColor = System.Drawing.Color.Transparent;
            this.lblDBInstance.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDBInstance.ForeColor = System.Drawing.Color.SlateGray;
            this.lblDBInstance.Location = new System.Drawing.Point(7, 418);
            this.lblDBInstance.Name = "lblDBInstance";
            this.lblDBInstance.Size = new System.Drawing.Size(192, 17);
            this.lblDBInstance.TabIndex = 18;
            this.lblDBInstance.Text = "Default Database instance name";
            // 
            // chkInsertElse
            // 
            this.chkInsertElse.AutoSize = true;
            this.chkInsertElse.BackColor = System.Drawing.Color.Transparent;
            this.chkInsertElse.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkInsertElse.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkInsertElse.Location = new System.Drawing.Point(10, 101);
            this.chkInsertElse.Name = "chkInsertElse";
            this.chkInsertElse.Size = new System.Drawing.Size(306, 24);
            this.chkInsertElse.TabIndex = 17;
            this.chkInsertElse.Text = "Insert Else when BeginIf command inserted";
            this.chkInsertElse.UseVisualStyleBackColor = false;
            // 
            // chkInsertVariablePosition
            // 
            this.chkInsertVariablePosition.AutoSize = true;
            this.chkInsertVariablePosition.BackColor = System.Drawing.Color.Transparent;
            this.chkInsertVariablePosition.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkInsertVariablePosition.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkInsertVariablePosition.Location = new System.Drawing.Point(10, 71);
            this.chkInsertVariablePosition.Name = "chkInsertVariablePosition";
            this.chkInsertVariablePosition.Size = new System.Drawing.Size(379, 24);
            this.chkInsertVariablePosition.TabIndex = 16;
            this.chkInsertVariablePosition.Text = "Insert variable at cursor position (Textbox/Combobox)";
            this.chkInsertVariablePosition.UseVisualStyleBackColor = false;
            // 
            // txtDefaultWordInstanceName
            // 
            this.txtDefaultWordInstanceName.Location = new System.Drawing.Point(10, 387);
            this.txtDefaultWordInstanceName.Name = "txtDefaultWordInstanceName";
            this.txtDefaultWordInstanceName.Size = new System.Drawing.Size(490, 29);
            this.txtDefaultWordInstanceName.TabIndex = 15;
            // 
            // lblWordInstance
            // 
            this.lblWordInstance.AutoSize = true;
            this.lblWordInstance.BackColor = System.Drawing.Color.Transparent;
            this.lblWordInstance.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWordInstance.ForeColor = System.Drawing.Color.SlateGray;
            this.lblWordInstance.Location = new System.Drawing.Point(7, 367);
            this.lblWordInstance.Name = "lblWordInstance";
            this.lblWordInstance.Size = new System.Drawing.Size(169, 17);
            this.lblWordInstance.TabIndex = 14;
            this.lblWordInstance.Text = "Default Word instance name";
            // 
            // txtDefaultExcelInstanceName
            // 
            this.txtDefaultExcelInstanceName.Location = new System.Drawing.Point(10, 337);
            this.txtDefaultExcelInstanceName.Name = "txtDefaultExcelInstanceName";
            this.txtDefaultExcelInstanceName.Size = new System.Drawing.Size(490, 29);
            this.txtDefaultExcelInstanceName.TabIndex = 13;
            // 
            // lblExcelInstance
            // 
            this.lblExcelInstance.AutoSize = true;
            this.lblExcelInstance.BackColor = System.Drawing.Color.Transparent;
            this.lblExcelInstance.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExcelInstance.ForeColor = System.Drawing.Color.SlateGray;
            this.lblExcelInstance.Location = new System.Drawing.Point(7, 317);
            this.lblExcelInstance.Name = "lblExcelInstance";
            this.lblExcelInstance.Size = new System.Drawing.Size(168, 17);
            this.lblExcelInstance.TabIndex = 12;
            this.lblExcelInstance.Text = "Default Excel instance name";
            // 
            // txtDefaultStopwatchInstanceName
            // 
            this.txtDefaultStopwatchInstanceName.Location = new System.Drawing.Point(10, 284);
            this.txtDefaultStopwatchInstanceName.Name = "txtDefaultStopwatchInstanceName";
            this.txtDefaultStopwatchInstanceName.Size = new System.Drawing.Size(490, 29);
            this.txtDefaultStopwatchInstanceName.TabIndex = 11;
            // 
            // lblStopwatchInstance
            // 
            this.lblStopwatchInstance.AutoSize = true;
            this.lblStopwatchInstance.BackColor = System.Drawing.Color.Transparent;
            this.lblStopwatchInstance.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStopwatchInstance.ForeColor = System.Drawing.Color.SlateGray;
            this.lblStopwatchInstance.Location = new System.Drawing.Point(7, 264);
            this.lblStopwatchInstance.Name = "lblStopwatchInstance";
            this.lblStopwatchInstance.Size = new System.Drawing.Size(193, 17);
            this.lblStopwatchInstance.TabIndex = 10;
            this.lblStopwatchInstance.Text = "Default Stopwach instance name";
            // 
            // txtDefaultBrowserInstanceName
            // 
            this.txtDefaultBrowserInstanceName.Location = new System.Drawing.Point(10, 232);
            this.txtDefaultBrowserInstanceName.Name = "txtDefaultBrowserInstanceName";
            this.txtDefaultBrowserInstanceName.Size = new System.Drawing.Size(490, 29);
            this.txtDefaultBrowserInstanceName.TabIndex = 9;
            // 
            // lblBrowserInstance
            // 
            this.lblBrowserInstance.AutoSize = true;
            this.lblBrowserInstance.BackColor = System.Drawing.Color.Transparent;
            this.lblBrowserInstance.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBrowserInstance.ForeColor = System.Drawing.Color.SlateGray;
            this.lblBrowserInstance.Location = new System.Drawing.Point(7, 212);
            this.lblBrowserInstance.Name = "lblBrowserInstance";
            this.lblBrowserInstance.Size = new System.Drawing.Size(182, 17);
            this.lblBrowserInstance.TabIndex = 8;
            this.lblBrowserInstance.Text = "Default browser instance name";
            // 
            // chkSequenceDragDrop
            // 
            this.chkSequenceDragDrop.AutoSize = true;
            this.chkSequenceDragDrop.BackColor = System.Drawing.Color.Transparent;
            this.chkSequenceDragDrop.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSequenceDragDrop.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkSequenceDragDrop.Location = new System.Drawing.Point(10, 41);
            this.chkSequenceDragDrop.Name = "chkSequenceDragDrop";
            this.chkSequenceDragDrop.Size = new System.Drawing.Size(341, 24);
            this.chkSequenceDragDrop.TabIndex = 5;
            this.chkSequenceDragDrop.Text = "Allow Drag and Drop into Sequence Commands";
            this.chkSequenceDragDrop.UseVisualStyleBackColor = false;
            // 
            // chkInsertCommandsInline
            // 
            this.chkInsertCommandsInline.AutoSize = true;
            this.chkInsertCommandsInline.BackColor = System.Drawing.Color.Transparent;
            this.chkInsertCommandsInline.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkInsertCommandsInline.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkInsertCommandsInline.Location = new System.Drawing.Point(10, 11);
            this.chkInsertCommandsInline.Name = "chkInsertCommandsInline";
            this.chkInsertCommandsInline.Size = new System.Drawing.Size(351, 24);
            this.chkInsertCommandsInline.TabIndex = 4;
            this.chkInsertCommandsInline.Text = "New Commands Insert Below Selected Command";
            this.chkInsertCommandsInline.UseVisualStyleBackColor = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.uiSettingTabs, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.uiBtnOpen, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(638, 584);
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
            this.panel1.Size = new System.Drawing.Size(638, 65);
            this.panel1.TabIndex = 0;
            // 
            // tmrGetSocketStatus
            // 
            this.tmrGetSocketStatus.Interval = 250;
            this.tmrGetSocketStatus.Tick += new System.EventHandler(this.tmrGetSocketStatus_Tick);
            // 
            // bgwMetrics
            // 
            this.bgwMetrics.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwMetrics_DoWork);
            this.bgwMetrics.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwMetrics_RunWorkerCompleted);
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 584);
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
            this.tabLocalListener.ResumeLayout(false);
            this.tabLocalListener.PerformLayout();
            this.tabEditorSettings.ResumeLayout(false);
            this.tabEditorSettings.PerformLayout();
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
        private System.Windows.Forms.Button btnGenerateWikiDocs;
        private System.Windows.Forms.TextBox txtVariableStartMarker;
        private System.Windows.Forms.Label lblVariableDisplay;
        private System.Windows.Forms.TextBox txtVariableEndMarker;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblDelay;
        private System.Windows.Forms.TextBox txtCommandDelay;
        internal System.Windows.Forms.CheckBox chkOverrideInstances;
        private System.Windows.Forms.CheckBox chkMinimizeToTray;
        private System.Windows.Forms.Button btnLaunchAttendedMode;
        private System.Windows.Forms.Button btnSelectAttendedTaskFolder;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtAttendedTaskFolder;
        private System.Windows.Forms.Label lblStartupMode;
        private System.Windows.Forms.ComboBox cboStartUpMode;
        private System.Windows.Forms.Button btnLaunchDisplayManager;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtGUID;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtHttpsAddress;
        private System.Windows.Forms.Button btnGetGUID;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnTaskPublish;
        private System.Windows.Forms.CheckBox chkPreloadCommands;
        private System.Windows.Forms.CheckBox chkSlimActionBar;
        private System.Windows.Forms.ComboBox cboCancellationKey;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TabPage tabLocalListener;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtAuthListeningKey;
        private System.Windows.Forms.CheckBox chkEnableListening;
        private System.Windows.Forms.CheckBox chkRequireListenerKey;
        private System.Windows.Forms.Button btnStopListening;
        private System.Windows.Forms.Button btnStartListening;
        private System.Windows.Forms.Label lblListeningStatus;
        private System.Windows.Forms.CheckBox chkAutoStartListener;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtListeningPort;
        private System.Windows.Forms.CheckBox chkEnableWhitelist;
        private System.Windows.Forms.TextBox txtWhiteList;
        internal System.Windows.Forms.CheckBox chkAutoCalcVariables;
        private System.Windows.Forms.TabPage tabEditorSettings;
        private System.Windows.Forms.CheckBox chkSequenceDragDrop;
        private System.Windows.Forms.CheckBox chkInsertCommandsInline;
        private System.Windows.Forms.TextBox txtDefaultBrowserInstanceName;
        private System.Windows.Forms.Label lblBrowserInstance;
        private System.Windows.Forms.TextBox txtDefaultWordInstanceName;
        private System.Windows.Forms.Label lblWordInstance;
        private System.Windows.Forms.TextBox txtDefaultExcelInstanceName;
        private System.Windows.Forms.Label lblExcelInstance;
        private System.Windows.Forms.TextBox txtDefaultStopwatchInstanceName;
        private System.Windows.Forms.Label lblStopwatchInstance;
        private System.Windows.Forms.CheckBox chkInsertVariablePosition;
        private System.Windows.Forms.CheckBox chkInsertElse;
        private System.Windows.Forms.TextBox txtCurrentWindow;
        private System.Windows.Forms.Label lblCurrentWindow;
        private System.Windows.Forms.TextBox txtDefaultDBInstanceName;
        private System.Windows.Forms.Label lblDBInstance;
        private System.Windows.Forms.CheckBox chkInsertCommentIfLoop;
        private System.Windows.Forms.CheckBox chkGruopingBySubgruop;
    }
}