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
            this.lblManageSettings = new System.Windows.Forms.Label();
            this.chkAutomaticallyConnect = new System.Windows.Forms.CheckBox();
            this.chkServerEnabled = new System.Windows.Forms.CheckBox();
            this.txtServerURL = new System.Windows.Forms.TextBox();
            this.lblServerURL = new System.Windows.Forms.Label();
            this.txtPublicKey = new System.Windows.Forms.TextBox();
            this.lblConnectKey = new System.Windows.Forms.Label();
            this.chkEnableLogging = new System.Windows.Forms.CheckBox();
            this.chkAutoCloseWindow = new System.Windows.Forms.CheckBox();
            this.chkShowDebug = new System.Windows.Forms.CheckBox();
            this.uiBtnOpen = new taskt.UI.CustomControls.UIPictureButton();
            this.lblMainLogo = new System.Windows.Forms.Label();
            this.lblAutomationEngine = new System.Windows.Forms.Label();
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
            this.lblAttendedTasksFolder = new System.Windows.Forms.Label();
            this.txtAttendedTaskFolder = new System.Windows.Forms.TextBox();
            this.btnLaunchAttendedMode = new System.Windows.Forms.Button();
            this.chkMinimizeToTray = new System.Windows.Forms.CheckBox();
            this.chkSequenceDragDrop = new System.Windows.Forms.CheckBox();
            this.btnGenerateWikiDocs = new System.Windows.Forms.Button();
            this.chkInsertCommandsInline = new System.Windows.Forms.CheckBox();
            this.btnClearMetrics = new System.Windows.Forms.Button();
            this.lblScriptExecutionMetrics = new System.Windows.Forms.Label();
            this.lblGettingMetrics = new System.Windows.Forms.Label();
            this.tvExecutionTimes = new System.Windows.Forms.TreeView();
            this.btnSelectFolder = new System.Windows.Forms.Button();
            this.lblRootFolder = new System.Windows.Forms.Label();
            this.txtAppFolderPath = new System.Windows.Forms.TextBox();
            this.tabDebugSettings = new System.Windows.Forms.TabPage();
            this.chkAutoCalcVariables = new System.Windows.Forms.CheckBox();
            this.lblEndScriptHotKey = new System.Windows.Forms.Label();
            this.cboCancellationKey = new System.Windows.Forms.ComboBox();
            this.chkOverrideInstances = new System.Windows.Forms.CheckBox();
            this.lblDelay = new System.Windows.Forms.Label();
            this.txtCommandDelay = new System.Windows.Forms.TextBox();
            this.lblEndMarker = new System.Windows.Forms.Label();
            this.lblStartMarker = new System.Windows.Forms.Label();
            this.lblVariablePatternDesc = new System.Windows.Forms.Label();
            this.lblVariablePattern = new System.Windows.Forms.Label();
            this.lblVariableDisplay = new System.Windows.Forms.Label();
            this.txtVariableEndMarker = new System.Windows.Forms.TextBox();
            this.txtVariableStartMarker = new System.Windows.Forms.TextBox();
            this.chkTrackMetrics = new System.Windows.Forms.CheckBox();
            this.tabServerSettings = new System.Windows.Forms.TabPage();
            this.btnTaskPublish = new System.Windows.Forms.Button();
            this.lblClientGUIDDesc = new System.Windows.Forms.Label();
            this.lblServerSettingsDesc = new System.Windows.Forms.Label();
            this.lblHTTPSDesc = new System.Windows.Forms.Label();
            this.btnGetGUID = new System.Windows.Forms.Button();
            this.lblClientGUID = new System.Windows.Forms.Label();
            this.txtGUID = new System.Windows.Forms.TextBox();
            this.lblHTTPSServerURL = new System.Windows.Forms.Label();
            this.txtHttpsAddress = new System.Windows.Forms.TextBox();
            this.chkBypassValidation = new System.Windows.Forms.CheckBox();
            this.btnCloseConnection = new System.Windows.Forms.Button();
            this.lblSocketException = new System.Windows.Forms.Label();
            this.lblServerSettings = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.tabLocalListener = new System.Windows.Forms.TabPage();
            this.txtWhiteList = new System.Windows.Forms.TextBox();
            this.chkEnableWhitelist = new System.Windows.Forms.CheckBox();
            this.chkAutoStartListener = new System.Windows.Forms.CheckBox();
            this.lblListeningPort = new System.Windows.Forms.Label();
            this.txtListeningPort = new System.Windows.Forms.TextBox();
            this.lblListeningStatus = new System.Windows.Forms.Label();
            this.btnStopListening = new System.Windows.Forms.Button();
            this.btnStartListening = new System.Windows.Forms.Button();
            this.chkRequireListenerKey = new System.Windows.Forms.CheckBox();
            this.lblAuthKey = new System.Windows.Forms.Label();
            this.txtAuthListeningKey = new System.Windows.Forms.TextBox();
            this.chkEnableListening = new System.Windows.Forms.CheckBox();
            this.lblLocalListenerSettingsDesc = new System.Windows.Forms.Label();
            this.lblLocalListenerSettings = new System.Windows.Forms.Label();
            this.tlpSettings = new System.Windows.Forms.TableLayoutPanel();
            this.pnlSettings = new System.Windows.Forms.Panel();
            this.tmrGetSocketStatus = new System.Windows.Forms.Timer(this.components);
            this.bgwMetrics = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOpen)).BeginInit();
            this.uiSettingTabs.SuspendLayout();
            this.tabAppSettings.SuspendLayout();
            this.tabDebugSettings.SuspendLayout();
            this.tabServerSettings.SuspendLayout();
            this.tabLocalListener.SuspendLayout();
            this.tlpSettings.SuspendLayout();
            this.pnlSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnConnect.Location = new System.Drawing.Point(416, 431);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(4);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(100, 41);
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
            this.chkRetryOnDisconnect.Location = new System.Drawing.Point(403, 448);
            this.chkRetryOnDisconnect.Margin = new System.Windows.Forms.Padding(4);
            this.chkRetryOnDisconnect.Name = "chkRetryOnDisconnect";
            this.chkRetryOnDisconnect.Size = new System.Drawing.Size(189, 24);
            this.chkRetryOnDisconnect.TabIndex = 15;
            this.chkRetryOnDisconnect.Text = "Retry If Connection Fails";
            this.chkRetryOnDisconnect.UseVisualStyleBackColor = false;
            this.chkRetryOnDisconnect.Visible = false;
            // 
            // lblManageSettings
            // 
            this.lblManageSettings.BackColor = System.Drawing.Color.Transparent;
            this.lblManageSettings.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblManageSettings.ForeColor = System.Drawing.Color.White;
            this.lblManageSettings.Location = new System.Drawing.Point(8, 53);
            this.lblManageSettings.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblManageSettings.Name = "lblManageSettings";
            this.lblManageSettings.Size = new System.Drawing.Size(603, 34);
            this.lblManageSettings.TabIndex = 14;
            this.lblManageSettings.Text = "Manage settings used by the application";
            // 
            // chkAutomaticallyConnect
            // 
            this.chkAutomaticallyConnect.AutoSize = true;
            this.chkAutomaticallyConnect.BackColor = System.Drawing.Color.Transparent;
            this.chkAutomaticallyConnect.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutomaticallyConnect.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkAutomaticallyConnect.Location = new System.Drawing.Point(16, 156);
            this.chkAutomaticallyConnect.Margin = new System.Windows.Forms.Padding(4);
            this.chkAutomaticallyConnect.Name = "chkAutomaticallyConnect";
            this.chkAutomaticallyConnect.Size = new System.Drawing.Size(197, 29);
            this.chkAutomaticallyConnect.TabIndex = 13;
            this.chkAutomaticallyConnect.Text = "Check In On Startup";
            this.chkAutomaticallyConnect.UseVisualStyleBackColor = false;
            // 
            // chkServerEnabled
            // 
            this.chkServerEnabled.AutoSize = true;
            this.chkServerEnabled.BackColor = System.Drawing.Color.Transparent;
            this.chkServerEnabled.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkServerEnabled.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkServerEnabled.Location = new System.Drawing.Point(16, 126);
            this.chkServerEnabled.Margin = new System.Windows.Forms.Padding(4);
            this.chkServerEnabled.Name = "chkServerEnabled";
            this.chkServerEnabled.Size = new System.Drawing.Size(255, 29);
            this.chkServerEnabled.TabIndex = 12;
            this.chkServerEnabled.Text = "Server Connection Enabled";
            this.chkServerEnabled.UseVisualStyleBackColor = false;
            // 
            // txtServerURL
            // 
            this.txtServerURL.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtServerURL.Location = new System.Drawing.Point(367, 443);
            this.txtServerURL.Margin = new System.Windows.Forms.Padding(4);
            this.txtServerURL.Name = "txtServerURL";
            this.txtServerURL.Size = new System.Drawing.Size(493, 34);
            this.txtServerURL.TabIndex = 11;
            this.txtServerURL.Visible = false;
            // 
            // lblServerURL
            // 
            this.lblServerURL.AutoSize = true;
            this.lblServerURL.BackColor = System.Drawing.Color.Transparent;
            this.lblServerURL.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServerURL.ForeColor = System.Drawing.Color.SlateGray;
            this.lblServerURL.Location = new System.Drawing.Point(387, 448);
            this.lblServerURL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblServerURL.Name = "lblServerURL";
            this.lblServerURL.Size = new System.Drawing.Size(297, 20);
            this.lblServerURL.TabIndex = 9;
            this.lblServerURL.Text = "Server URL ex. ws://localhost:port/ws)\r";
            this.lblServerURL.Visible = false;
            // 
            // txtPublicKey
            // 
            this.txtPublicKey.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtPublicKey.Location = new System.Drawing.Point(367, 436);
            this.txtPublicKey.Margin = new System.Windows.Forms.Padding(4);
            this.txtPublicKey.Name = "txtPublicKey";
            this.txtPublicKey.Size = new System.Drawing.Size(493, 34);
            this.txtPublicKey.TabIndex = 19;
            this.txtPublicKey.Visible = false;
            // 
            // lblConnectKey
            // 
            this.lblConnectKey.AutoSize = true;
            this.lblConnectKey.BackColor = System.Drawing.Color.Transparent;
            this.lblConnectKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConnectKey.ForeColor = System.Drawing.Color.SlateGray;
            this.lblConnectKey.Location = new System.Drawing.Point(413, 452);
            this.lblConnectKey.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblConnectKey.Name = "lblConnectKey";
            this.lblConnectKey.Size = new System.Drawing.Size(109, 20);
            this.lblConnectKey.TabIndex = 18;
            this.lblConnectKey.Text = "Connect Key:";
            this.lblConnectKey.Visible = false;
            // 
            // chkEnableLogging
            // 
            this.chkEnableLogging.AutoSize = true;
            this.chkEnableLogging.BackColor = System.Drawing.Color.Transparent;
            this.chkEnableLogging.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkEnableLogging.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkEnableLogging.Location = new System.Drawing.Point(13, 94);
            this.chkEnableLogging.Margin = new System.Windows.Forms.Padding(4);
            this.chkEnableLogging.Name = "chkEnableLogging";
            this.chkEnableLogging.Size = new System.Drawing.Size(256, 32);
            this.chkEnableLogging.TabIndex = 14;
            this.chkEnableLogging.Text = "Enable Diagnostic Logging";
            this.chkEnableLogging.UseVisualStyleBackColor = false;
            // 
            // chkAutoCloseWindow
            // 
            this.chkAutoCloseWindow.AutoSize = true;
            this.chkAutoCloseWindow.BackColor = System.Drawing.Color.Transparent;
            this.chkAutoCloseWindow.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutoCloseWindow.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkAutoCloseWindow.Location = new System.Drawing.Point(13, 69);
            this.chkAutoCloseWindow.Margin = new System.Windows.Forms.Padding(4);
            this.chkAutoCloseWindow.Name = "chkAutoCloseWindow";
            this.chkAutoCloseWindow.Size = new System.Drawing.Size(334, 32);
            this.chkAutoCloseWindow.TabIndex = 13;
            this.chkAutoCloseWindow.Text = "Automatically Close Debug Window";
            this.chkAutoCloseWindow.UseVisualStyleBackColor = false;
            // 
            // chkShowDebug
            // 
            this.chkShowDebug.AutoSize = true;
            this.chkShowDebug.BackColor = System.Drawing.Color.Transparent;
            this.chkShowDebug.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkShowDebug.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkShowDebug.Location = new System.Drawing.Point(13, 44);
            this.chkShowDebug.Margin = new System.Windows.Forms.Padding(4);
            this.chkShowDebug.Name = "chkShowDebug";
            this.chkShowDebug.Size = new System.Drawing.Size(216, 32);
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
            this.uiBtnOpen.Location = new System.Drawing.Point(4, 709);
            this.uiBtnOpen.Margin = new System.Windows.Forms.Padding(4);
            this.uiBtnOpen.Name = "uiBtnOpen";
            this.uiBtnOpen.Size = new System.Drawing.Size(64, 59);
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
            this.lblMainLogo.Location = new System.Drawing.Point(4, 1);
            this.lblMainLogo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMainLogo.Name = "lblMainLogo";
            this.lblMainLogo.Size = new System.Drawing.Size(156, 54);
            this.lblMainLogo.TabIndex = 14;
            this.lblMainLogo.Text = "settings";
            // 
            // lblAutomationEngine
            // 
            this.lblAutomationEngine.AutoSize = true;
            this.lblAutomationEngine.BackColor = System.Drawing.Color.Transparent;
            this.lblAutomationEngine.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAutomationEngine.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblAutomationEngine.Location = new System.Drawing.Point(8, 7);
            this.lblAutomationEngine.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAutomationEngine.Name = "lblAutomationEngine";
            this.lblAutomationEngine.Size = new System.Drawing.Size(233, 37);
            this.lblAutomationEngine.TabIndex = 15;
            this.lblAutomationEngine.Text = "Automation Engine";
            // 
            // lblApplicationSettings
            // 
            this.lblApplicationSettings.AutoSize = true;
            this.lblApplicationSettings.BackColor = System.Drawing.Color.Transparent;
            this.lblApplicationSettings.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApplicationSettings.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblApplicationSettings.Location = new System.Drawing.Point(8, 7);
            this.lblApplicationSettings.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblApplicationSettings.Name = "lblApplicationSettings";
            this.lblApplicationSettings.Size = new System.Drawing.Size(239, 37);
            this.lblApplicationSettings.TabIndex = 21;
            this.lblApplicationSettings.Text = "Application Settings";
            // 
            // chkAntiIdle
            // 
            this.chkAntiIdle.AutoSize = true;
            this.chkAntiIdle.BackColor = System.Drawing.Color.Transparent;
            this.chkAntiIdle.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAntiIdle.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkAntiIdle.Location = new System.Drawing.Point(13, 44);
            this.chkAntiIdle.Margin = new System.Windows.Forms.Padding(4);
            this.chkAntiIdle.Name = "chkAntiIdle";
            this.chkAntiIdle.Size = new System.Drawing.Size(263, 29);
            this.chkAntiIdle.TabIndex = 20;
            this.chkAntiIdle.Text = "Anti-Idle (while app is open)";
            this.chkAntiIdle.UseVisualStyleBackColor = false;
            // 
            // btnUpdates
            // 
            this.btnUpdates.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdates.Location = new System.Drawing.Point(16, 407);
            this.btnUpdates.Margin = new System.Windows.Forms.Padding(4);
            this.btnUpdates.Name = "btnUpdates";
            this.btnUpdates.Size = new System.Drawing.Size(276, 33);
            this.btnUpdates.TabIndex = 22;
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
            this.chkAdvancedDebug.Location = new System.Drawing.Point(13, 118);
            this.chkAdvancedDebug.Margin = new System.Windows.Forms.Padding(4);
            this.chkAdvancedDebug.Name = "chkAdvancedDebug";
            this.chkAdvancedDebug.Size = new System.Drawing.Size(424, 32);
            this.chkAdvancedDebug.TabIndex = 23;
            this.chkAdvancedDebug.Text = "Show Advanced Debug Logs During Execution";
            this.chkAdvancedDebug.UseVisualStyleBackColor = false;
            // 
            // chkCreateMissingVariables
            // 
            this.chkCreateMissingVariables.AutoSize = true;
            this.chkCreateMissingVariables.BackColor = System.Drawing.Color.Transparent;
            this.chkCreateMissingVariables.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCreateMissingVariables.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkCreateMissingVariables.Location = new System.Drawing.Point(13, 143);
            this.chkCreateMissingVariables.Margin = new System.Windows.Forms.Padding(4);
            this.chkCreateMissingVariables.Name = "chkCreateMissingVariables";
            this.chkCreateMissingVariables.Size = new System.Drawing.Size(386, 32);
            this.chkCreateMissingVariables.TabIndex = 24;
            this.chkCreateMissingVariables.Text = "Create Missing Variables During Execution";
            this.chkCreateMissingVariables.UseVisualStyleBackColor = false;
            // 
            // uiSettingTabs
            // 
            this.uiSettingTabs.Controls.Add(this.tabAppSettings);
            this.uiSettingTabs.Controls.Add(this.tabDebugSettings);
            this.uiSettingTabs.Controls.Add(this.tabServerSettings);
            this.uiSettingTabs.Controls.Add(this.tabLocalListener);
            this.uiSettingTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiSettingTabs.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uiSettingTabs.Location = new System.Drawing.Point(4, 90);
            this.uiSettingTabs.Margin = new System.Windows.Forms.Padding(4);
            this.uiSettingTabs.Name = "uiSettingTabs";
            this.uiSettingTabs.SelectedIndex = 0;
            this.uiSettingTabs.Size = new System.Drawing.Size(843, 611);
            this.uiSettingTabs.TabIndex = 25;
            // 
            // tabAppSettings
            // 
            this.tabAppSettings.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabAppSettings.Controls.Add(this.chkSlimActionBar);
            this.tabAppSettings.Controls.Add(this.chkPreloadCommands);
            this.tabAppSettings.Controls.Add(this.btnLaunchDisplayManager);
            this.tabAppSettings.Controls.Add(this.lblStartupMode);
            this.tabAppSettings.Controls.Add(this.cboStartUpMode);
            this.tabAppSettings.Controls.Add(this.btnSelectAttendedTaskFolder);
            this.tabAppSettings.Controls.Add(this.lblAttendedTasksFolder);
            this.tabAppSettings.Controls.Add(this.txtAttendedTaskFolder);
            this.tabAppSettings.Controls.Add(this.btnLaunchAttendedMode);
            this.tabAppSettings.Controls.Add(this.chkMinimizeToTray);
            this.tabAppSettings.Controls.Add(this.chkSequenceDragDrop);
            this.tabAppSettings.Controls.Add(this.btnGenerateWikiDocs);
            this.tabAppSettings.Controls.Add(this.chkInsertCommandsInline);
            this.tabAppSettings.Controls.Add(this.btnClearMetrics);
            this.tabAppSettings.Controls.Add(this.lblScriptExecutionMetrics);
            this.tabAppSettings.Controls.Add(this.lblGettingMetrics);
            this.tabAppSettings.Controls.Add(this.tvExecutionTimes);
            this.tabAppSettings.Controls.Add(this.btnSelectFolder);
            this.tabAppSettings.Controls.Add(this.lblRootFolder);
            this.tabAppSettings.Controls.Add(this.txtAppFolderPath);
            this.tabAppSettings.Controls.Add(this.lblApplicationSettings);
            this.tabAppSettings.Controls.Add(this.chkAntiIdle);
            this.tabAppSettings.Controls.Add(this.btnUpdates);
            this.tabAppSettings.Location = new System.Drawing.Point(4, 37);
            this.tabAppSettings.Margin = new System.Windows.Forms.Padding(4);
            this.tabAppSettings.Name = "tabAppSettings";
            this.tabAppSettings.Padding = new System.Windows.Forms.Padding(4);
            this.tabAppSettings.Size = new System.Drawing.Size(835, 570);
            this.tabAppSettings.TabIndex = 0;
            this.tabAppSettings.Text = "Application";
            // 
            // chkSlimActionBar
            // 
            this.chkSlimActionBar.AutoSize = true;
            this.chkSlimActionBar.BackColor = System.Drawing.Color.Transparent;
            this.chkSlimActionBar.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSlimActionBar.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkSlimActionBar.Location = new System.Drawing.Point(13, 142);
            this.chkSlimActionBar.Margin = new System.Windows.Forms.Padding(4);
            this.chkSlimActionBar.Name = "chkSlimActionBar";
            this.chkSlimActionBar.Size = new System.Drawing.Size(193, 29);
            this.chkSlimActionBar.TabIndex = 42;
            this.chkSlimActionBar.Text = "Use Slim Action Bar";
            this.chkSlimActionBar.UseVisualStyleBackColor = false;
            // 
            // chkPreloadCommands
            // 
            this.chkPreloadCommands.AutoSize = true;
            this.chkPreloadCommands.BackColor = System.Drawing.Color.Transparent;
            this.chkPreloadCommands.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPreloadCommands.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkPreloadCommands.Location = new System.Drawing.Point(13, 166);
            this.chkPreloadCommands.Margin = new System.Windows.Forms.Padding(4);
            this.chkPreloadCommands.Name = "chkPreloadCommands";
            this.chkPreloadCommands.Size = new System.Drawing.Size(400, 29);
            this.chkPreloadCommands.TabIndex = 41;
            this.chkPreloadCommands.Text = "Load Commands at Startup (Reduces Flicker)";
            this.chkPreloadCommands.UseVisualStyleBackColor = false;
            this.chkPreloadCommands.Visible = false;
            // 
            // btnLaunchDisplayManager
            // 
            this.btnLaunchDisplayManager.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLaunchDisplayManager.Location = new System.Drawing.Point(300, 448);
            this.btnLaunchDisplayManager.Margin = new System.Windows.Forms.Padding(4);
            this.btnLaunchDisplayManager.Name = "btnLaunchDisplayManager";
            this.btnLaunchDisplayManager.Size = new System.Drawing.Size(276, 33);
            this.btnLaunchDisplayManager.TabIndex = 40;
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
            this.lblStartupMode.Location = new System.Drawing.Point(13, 337);
            this.lblStartupMode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStartupMode.Name = "lblStartupMode";
            this.lblStartupMode.Size = new System.Drawing.Size(92, 23);
            this.lblStartupMode.TabIndex = 39;
            this.lblStartupMode.Text = "Start Mode";
            // 
            // cboStartUpMode
            // 
            this.cboStartUpMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStartUpMode.FormattingEnabled = true;
            this.cboStartUpMode.Items.AddRange(new object[] {
            "Builder Mode",
            "Attended Task Mode"});
            this.cboStartUpMode.Location = new System.Drawing.Point(17, 361);
            this.cboStartUpMode.Margin = new System.Windows.Forms.Padding(4);
            this.cboStartUpMode.Name = "cboStartUpMode";
            this.cboStartUpMode.Size = new System.Drawing.Size(291, 36);
            this.cboStartUpMode.TabIndex = 38;
            // 
            // btnSelectAttendedTaskFolder
            // 
            this.btnSelectAttendedTaskFolder.Location = new System.Drawing.Point(672, 297);
            this.btnSelectAttendedTaskFolder.Margin = new System.Windows.Forms.Padding(4);
            this.btnSelectAttendedTaskFolder.Name = "btnSelectAttendedTaskFolder";
            this.btnSelectAttendedTaskFolder.Size = new System.Drawing.Size(56, 37);
            this.btnSelectAttendedTaskFolder.TabIndex = 37;
            this.btnSelectAttendedTaskFolder.Text = "...";
            this.btnSelectAttendedTaskFolder.UseVisualStyleBackColor = true;
            this.btnSelectAttendedTaskFolder.Click += new System.EventHandler(this.btnSelectAttendedTaskFolder_Click);
            // 
            // lblAttendedTasksFolder
            // 
            this.lblAttendedTasksFolder.AutoSize = true;
            this.lblAttendedTasksFolder.BackColor = System.Drawing.Color.Transparent;
            this.lblAttendedTasksFolder.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAttendedTasksFolder.ForeColor = System.Drawing.Color.SlateGray;
            this.lblAttendedTasksFolder.Location = new System.Drawing.Point(12, 274);
            this.lblAttendedTasksFolder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAttendedTasksFolder.Name = "lblAttendedTasksFolder";
            this.lblAttendedTasksFolder.Size = new System.Drawing.Size(173, 23);
            this.lblAttendedTasksFolder.TabIndex = 36;
            this.lblAttendedTasksFolder.Text = "Attended Tasks Folder";
            // 
            // txtAttendedTaskFolder
            // 
            this.txtAttendedTaskFolder.Location = new System.Drawing.Point(16, 298);
            this.txtAttendedTaskFolder.Margin = new System.Windows.Forms.Padding(4);
            this.txtAttendedTaskFolder.Name = "txtAttendedTaskFolder";
            this.txtAttendedTaskFolder.Size = new System.Drawing.Size(652, 34);
            this.txtAttendedTaskFolder.TabIndex = 35;
            // 
            // btnLaunchAttendedMode
            // 
            this.btnLaunchAttendedMode.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLaunchAttendedMode.Location = new System.Drawing.Point(300, 407);
            this.btnLaunchAttendedMode.Margin = new System.Windows.Forms.Padding(4);
            this.btnLaunchAttendedMode.Name = "btnLaunchAttendedMode";
            this.btnLaunchAttendedMode.Size = new System.Drawing.Size(276, 33);
            this.btnLaunchAttendedMode.TabIndex = 34;
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
            this.chkMinimizeToTray.Location = new System.Drawing.Point(13, 118);
            this.chkMinimizeToTray.Margin = new System.Windows.Forms.Padding(4);
            this.chkMinimizeToTray.Name = "chkMinimizeToTray";
            this.chkMinimizeToTray.Size = new System.Drawing.Size(231, 29);
            this.chkMinimizeToTray.TabIndex = 33;
            this.chkMinimizeToTray.Text = "Minimize to System Tray";
            this.chkMinimizeToTray.UseVisualStyleBackColor = false;
            // 
            // chkSequenceDragDrop
            // 
            this.chkSequenceDragDrop.AutoSize = true;
            this.chkSequenceDragDrop.BackColor = System.Drawing.Color.Transparent;
            this.chkSequenceDragDrop.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSequenceDragDrop.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkSequenceDragDrop.Location = new System.Drawing.Point(13, 94);
            this.chkSequenceDragDrop.Margin = new System.Windows.Forms.Padding(4);
            this.chkSequenceDragDrop.Name = "chkSequenceDragDrop";
            this.chkSequenceDragDrop.Size = new System.Drawing.Size(425, 29);
            this.chkSequenceDragDrop.TabIndex = 32;
            this.chkSequenceDragDrop.Text = "Allow Drag and Drop into Sequence Commands";
            this.chkSequenceDragDrop.UseVisualStyleBackColor = false;
            // 
            // btnGenerateWikiDocs
            // 
            this.btnGenerateWikiDocs.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerateWikiDocs.Location = new System.Drawing.Point(16, 448);
            this.btnGenerateWikiDocs.Margin = new System.Windows.Forms.Padding(4);
            this.btnGenerateWikiDocs.Name = "btnGenerateWikiDocs";
            this.btnGenerateWikiDocs.Size = new System.Drawing.Size(276, 33);
            this.btnGenerateWikiDocs.TabIndex = 31;
            this.btnGenerateWikiDocs.Text = "Generate Documentation";
            this.btnGenerateWikiDocs.UseVisualStyleBackColor = true;
            this.btnGenerateWikiDocs.Click += new System.EventHandler(this.btnGenerateWikiDocs_Click);
            // 
            // chkInsertCommandsInline
            // 
            this.chkInsertCommandsInline.AutoSize = true;
            this.chkInsertCommandsInline.BackColor = System.Drawing.Color.Transparent;
            this.chkInsertCommandsInline.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkInsertCommandsInline.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkInsertCommandsInline.Location = new System.Drawing.Point(13, 69);
            this.chkInsertCommandsInline.Margin = new System.Windows.Forms.Padding(4);
            this.chkInsertCommandsInline.Name = "chkInsertCommandsInline";
            this.chkInsertCommandsInline.Size = new System.Drawing.Size(441, 29);
            this.chkInsertCommandsInline.TabIndex = 30;
            this.chkInsertCommandsInline.Text = "New Commands Insert Below Selected Command";
            this.chkInsertCommandsInline.UseVisualStyleBackColor = false;
            // 
            // btnClearMetrics
            // 
            this.btnClearMetrics.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearMetrics.Location = new System.Drawing.Point(15, 678);
            this.btnClearMetrics.Margin = new System.Windows.Forms.Padding(4);
            this.btnClearMetrics.Name = "btnClearMetrics";
            this.btnClearMetrics.Size = new System.Drawing.Size(144, 31);
            this.btnClearMetrics.TabIndex = 29;
            this.btnClearMetrics.Text = "Clear Metrics";
            this.btnClearMetrics.UseVisualStyleBackColor = true;
            this.btnClearMetrics.Visible = false;
            this.btnClearMetrics.Click += new System.EventHandler(this.btnClearMetrics_Click);
            // 
            // lblScriptExecutionMetrics
            // 
            this.lblScriptExecutionMetrics.AutoSize = true;
            this.lblScriptExecutionMetrics.BackColor = System.Drawing.Color.Transparent;
            this.lblScriptExecutionMetrics.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScriptExecutionMetrics.ForeColor = System.Drawing.Color.SlateGray;
            this.lblScriptExecutionMetrics.Location = new System.Drawing.Point(17, 494);
            this.lblScriptExecutionMetrics.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblScriptExecutionMetrics.Name = "lblScriptExecutionMetrics";
            this.lblScriptExecutionMetrics.Size = new System.Drawing.Size(327, 23);
            this.lblScriptExecutionMetrics.TabIndex = 28;
            this.lblScriptExecutionMetrics.Text = "Script Execution Metrics (Last 10 per Script)";
            // 
            // lblGettingMetrics
            // 
            this.lblGettingMetrics.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDownGrid;
            this.lblGettingMetrics.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGettingMetrics.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblGettingMetrics.Location = new System.Drawing.Point(16, 517);
            this.lblGettingMetrics.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGettingMetrics.Name = "lblGettingMetrics";
            this.lblGettingMetrics.Size = new System.Drawing.Size(712, 156);
            this.lblGettingMetrics.TabIndex = 27;
            this.lblGettingMetrics.Text = "Getting Metrics...";
            // 
            // tvExecutionTimes
            // 
            this.tvExecutionTimes.Location = new System.Drawing.Point(16, 517);
            this.tvExecutionTimes.Margin = new System.Windows.Forms.Padding(4);
            this.tvExecutionTimes.Name = "tvExecutionTimes";
            this.tvExecutionTimes.Size = new System.Drawing.Size(711, 155);
            this.tvExecutionTimes.TabIndex = 26;
            this.tvExecutionTimes.Visible = false;
            // 
            // btnSelectFolder
            // 
            this.btnSelectFolder.Location = new System.Drawing.Point(672, 234);
            this.btnSelectFolder.Margin = new System.Windows.Forms.Padding(4);
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.Size = new System.Drawing.Size(56, 37);
            this.btnSelectFolder.TabIndex = 25;
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
            this.lblRootFolder.Location = new System.Drawing.Point(12, 210);
            this.lblRootFolder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRootFolder.Name = "lblRootFolder";
            this.lblRootFolder.Size = new System.Drawing.Size(136, 23);
            this.lblRootFolder.TabIndex = 24;
            this.lblRootFolder.Text = "taskt Root Folder";
            // 
            // txtAppFolderPath
            // 
            this.txtAppFolderPath.Location = new System.Drawing.Point(16, 234);
            this.txtAppFolderPath.Margin = new System.Windows.Forms.Padding(4);
            this.txtAppFolderPath.Name = "txtAppFolderPath";
            this.txtAppFolderPath.Size = new System.Drawing.Size(652, 34);
            this.txtAppFolderPath.TabIndex = 23;
            // 
            // tabDebugSettings
            // 
            this.tabDebugSettings.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabDebugSettings.Controls.Add(this.chkAutoCalcVariables);
            this.tabDebugSettings.Controls.Add(this.lblEndScriptHotKey);
            this.tabDebugSettings.Controls.Add(this.cboCancellationKey);
            this.tabDebugSettings.Controls.Add(this.chkOverrideInstances);
            this.tabDebugSettings.Controls.Add(this.lblDelay);
            this.tabDebugSettings.Controls.Add(this.txtCommandDelay);
            this.tabDebugSettings.Controls.Add(this.lblEndMarker);
            this.tabDebugSettings.Controls.Add(this.lblStartMarker);
            this.tabDebugSettings.Controls.Add(this.lblVariablePatternDesc);
            this.tabDebugSettings.Controls.Add(this.lblVariablePattern);
            this.tabDebugSettings.Controls.Add(this.lblVariableDisplay);
            this.tabDebugSettings.Controls.Add(this.txtVariableEndMarker);
            this.tabDebugSettings.Controls.Add(this.txtVariableStartMarker);
            this.tabDebugSettings.Controls.Add(this.chkTrackMetrics);
            this.tabDebugSettings.Controls.Add(this.lblAutomationEngine);
            this.tabDebugSettings.Controls.Add(this.chkCreateMissingVariables);
            this.tabDebugSettings.Controls.Add(this.chkShowDebug);
            this.tabDebugSettings.Controls.Add(this.chkAdvancedDebug);
            this.tabDebugSettings.Controls.Add(this.chkAutoCloseWindow);
            this.tabDebugSettings.Controls.Add(this.chkEnableLogging);
            this.tabDebugSettings.Location = new System.Drawing.Point(4, 37);
            this.tabDebugSettings.Margin = new System.Windows.Forms.Padding(4);
            this.tabDebugSettings.Name = "tabDebugSettings";
            this.tabDebugSettings.Padding = new System.Windows.Forms.Padding(4);
            this.tabDebugSettings.Size = new System.Drawing.Size(835, 570);
            this.tabDebugSettings.TabIndex = 1;
            this.tabDebugSettings.Text = "Automation Engine";
            // 
            // chkAutoCalcVariables
            // 
            this.chkAutoCalcVariables.AutoSize = true;
            this.chkAutoCalcVariables.BackColor = System.Drawing.Color.Transparent;
            this.chkAutoCalcVariables.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutoCalcVariables.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkAutoCalcVariables.Location = new System.Drawing.Point(13, 219);
            this.chkAutoCalcVariables.Margin = new System.Windows.Forms.Padding(4);
            this.chkAutoCalcVariables.Name = "chkAutoCalcVariables";
            this.chkAutoCalcVariables.Size = new System.Drawing.Size(309, 32);
            this.chkAutoCalcVariables.TabIndex = 38;
            this.chkAutoCalcVariables.Text = "Calculate Variables Automatically";
            this.chkAutoCalcVariables.UseVisualStyleBackColor = false;
            // 
            // lblEndScriptHotKey
            // 
            this.lblEndScriptHotKey.AutoSize = true;
            this.lblEndScriptHotKey.BackColor = System.Drawing.Color.Transparent;
            this.lblEndScriptHotKey.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEndScriptHotKey.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblEndScriptHotKey.Location = new System.Drawing.Point(8, 522);
            this.lblEndScriptHotKey.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEndScriptHotKey.Name = "lblEndScriptHotKey";
            this.lblEndScriptHotKey.Size = new System.Drawing.Size(164, 28);
            this.lblEndScriptHotKey.TabIndex = 37;
            this.lblEndScriptHotKey.Text = "End Script Hotkey:";
            // 
            // cboCancellationKey
            // 
            this.cboCancellationKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCancellationKey.FormattingEnabled = true;
            this.cboCancellationKey.Location = new System.Drawing.Point(189, 519);
            this.cboCancellationKey.Margin = new System.Windows.Forms.Padding(4);
            this.cboCancellationKey.Name = "cboCancellationKey";
            this.cboCancellationKey.Size = new System.Drawing.Size(204, 36);
            this.cboCancellationKey.TabIndex = 36;
            // 
            // chkOverrideInstances
            // 
            this.chkOverrideInstances.AutoSize = true;
            this.chkOverrideInstances.BackColor = System.Drawing.Color.Transparent;
            this.chkOverrideInstances.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkOverrideInstances.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkOverrideInstances.Location = new System.Drawing.Point(13, 192);
            this.chkOverrideInstances.Margin = new System.Windows.Forms.Padding(4);
            this.chkOverrideInstances.Name = "chkOverrideInstances";
            this.chkOverrideInstances.Size = new System.Drawing.Size(230, 32);
            this.chkOverrideInstances.TabIndex = 35;
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
            this.lblDelay.Location = new System.Drawing.Point(8, 255);
            this.lblDelay.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDelay.Name = "lblDelay";
            this.lblDelay.Size = new System.Drawing.Size(430, 28);
            this.lblDelay.TabIndex = 34;
            this.lblDelay.Text = "Default delay between executing commands (ms):";
            // 
            // txtCommandDelay
            // 
            this.txtCommandDelay.Location = new System.Drawing.Point(479, 252);
            this.txtCommandDelay.Margin = new System.Windows.Forms.Padding(4);
            this.txtCommandDelay.Name = "txtCommandDelay";
            this.txtCommandDelay.Size = new System.Drawing.Size(101, 34);
            this.txtCommandDelay.TabIndex = 33;
            // 
            // lblEndMarker
            // 
            this.lblEndMarker.AutoSize = true;
            this.lblEndMarker.BackColor = System.Drawing.Color.Transparent;
            this.lblEndMarker.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEndMarker.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblEndMarker.Location = new System.Drawing.Point(20, 458);
            this.lblEndMarker.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEndMarker.Name = "lblEndMarker";
            this.lblEndMarker.Size = new System.Drawing.Size(122, 28);
            this.lblEndMarker.TabIndex = 32;
            this.lblEndMarker.Text = "End Marker:";
            // 
            // lblStartMarker
            // 
            this.lblStartMarker.AutoSize = true;
            this.lblStartMarker.BackColor = System.Drawing.Color.Transparent;
            this.lblStartMarker.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStartMarker.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblStartMarker.Location = new System.Drawing.Point(16, 416);
            this.lblStartMarker.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStartMarker.Name = "lblStartMarker";
            this.lblStartMarker.Size = new System.Drawing.Size(130, 28);
            this.lblStartMarker.TabIndex = 31;
            this.lblStartMarker.Text = "Start Marker:";
            // 
            // lblVariablePatternDesc
            // 
            this.lblVariablePatternDesc.BackColor = System.Drawing.Color.Transparent;
            this.lblVariablePatternDesc.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVariablePatternDesc.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblVariablePatternDesc.Location = new System.Drawing.Point(13, 319);
            this.lblVariablePatternDesc.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVariablePatternDesc.Name = "lblVariablePatternDesc";
            this.lblVariablePatternDesc.Size = new System.Drawing.Size(547, 89);
            this.lblVariablePatternDesc.TabIndex = 30;
            this.lblVariablePatternDesc.Text = "Indicate the start and end markers for variables.  When the engine runs, it will " +
    "automatically replace the variable with the stored value.";
            // 
            // lblVariablePattern
            // 
            this.lblVariablePattern.AutoSize = true;
            this.lblVariablePattern.BackColor = System.Drawing.Color.Transparent;
            this.lblVariablePattern.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVariablePattern.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblVariablePattern.Location = new System.Drawing.Point(8, 286);
            this.lblVariablePattern.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVariablePattern.Name = "lblVariablePattern";
            this.lblVariablePattern.Size = new System.Drawing.Size(193, 37);
            this.lblVariablePattern.TabIndex = 29;
            this.lblVariablePattern.Text = "Variable Pattern";
            // 
            // lblVariableDisplay
            // 
            this.lblVariableDisplay.AutoSize = true;
            this.lblVariableDisplay.BackColor = System.Drawing.Color.Transparent;
            this.lblVariableDisplay.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVariableDisplay.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblVariableDisplay.Location = new System.Drawing.Point(217, 434);
            this.lblVariableDisplay.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVariableDisplay.Name = "lblVariableDisplay";
            this.lblVariableDisplay.Size = new System.Drawing.Size(167, 32);
            this.lblVariableDisplay.TabIndex = 28;
            this.lblVariableDisplay.Text = "VariableName";
            // 
            // txtVariableEndMarker
            // 
            this.txtVariableEndMarker.Location = new System.Drawing.Point(159, 453);
            this.txtVariableEndMarker.Margin = new System.Windows.Forms.Padding(4);
            this.txtVariableEndMarker.Name = "txtVariableEndMarker";
            this.txtVariableEndMarker.Size = new System.Drawing.Size(33, 34);
            this.txtVariableEndMarker.TabIndex = 27;
            this.txtVariableEndMarker.TextChanged += new System.EventHandler(this.txtVariableStartMarker_TextChanged);
            // 
            // txtVariableStartMarker
            // 
            this.txtVariableStartMarker.Location = new System.Drawing.Point(159, 412);
            this.txtVariableStartMarker.Margin = new System.Windows.Forms.Padding(4);
            this.txtVariableStartMarker.Name = "txtVariableStartMarker";
            this.txtVariableStartMarker.Size = new System.Drawing.Size(33, 34);
            this.txtVariableStartMarker.TabIndex = 26;
            this.txtVariableStartMarker.TextChanged += new System.EventHandler(this.txtVariableStartMarker_TextChanged);
            // 
            // chkTrackMetrics
            // 
            this.chkTrackMetrics.AutoSize = true;
            this.chkTrackMetrics.BackColor = System.Drawing.Color.Transparent;
            this.chkTrackMetrics.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkTrackMetrics.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkTrackMetrics.Location = new System.Drawing.Point(13, 167);
            this.chkTrackMetrics.Margin = new System.Windows.Forms.Padding(4);
            this.chkTrackMetrics.Name = "chkTrackMetrics";
            this.chkTrackMetrics.Size = new System.Drawing.Size(229, 32);
            this.chkTrackMetrics.TabIndex = 25;
            this.chkTrackMetrics.Text = "Track Execution Metrics";
            this.chkTrackMetrics.UseVisualStyleBackColor = false;
            // 
            // tabServerSettings
            // 
            this.tabServerSettings.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabServerSettings.Controls.Add(this.btnTaskPublish);
            this.tabServerSettings.Controls.Add(this.lblClientGUIDDesc);
            this.tabServerSettings.Controls.Add(this.lblServerSettingsDesc);
            this.tabServerSettings.Controls.Add(this.lblHTTPSDesc);
            this.tabServerSettings.Controls.Add(this.btnGetGUID);
            this.tabServerSettings.Controls.Add(this.lblClientGUID);
            this.tabServerSettings.Controls.Add(this.txtGUID);
            this.tabServerSettings.Controls.Add(this.lblHTTPSServerURL);
            this.tabServerSettings.Controls.Add(this.txtHttpsAddress);
            this.tabServerSettings.Controls.Add(this.chkBypassValidation);
            this.tabServerSettings.Controls.Add(this.btnCloseConnection);
            this.tabServerSettings.Controls.Add(this.lblSocketException);
            this.tabServerSettings.Controls.Add(this.lblServerSettings);
            this.tabServerSettings.Controls.Add(this.chkServerEnabled);
            this.tabServerSettings.Controls.Add(this.txtPublicKey);
            this.tabServerSettings.Controls.Add(this.lblServerURL);
            this.tabServerSettings.Controls.Add(this.lblConnectKey);
            this.tabServerSettings.Controls.Add(this.txtServerURL);
            this.tabServerSettings.Controls.Add(this.btnConnect);
            this.tabServerSettings.Controls.Add(this.chkAutomaticallyConnect);
            this.tabServerSettings.Controls.Add(this.chkRetryOnDisconnect);
            this.tabServerSettings.Controls.Add(this.lblStatus);
            this.tabServerSettings.Location = new System.Drawing.Point(4, 37);
            this.tabServerSettings.Margin = new System.Windows.Forms.Padding(4);
            this.tabServerSettings.Name = "tabServerSettings";
            this.tabServerSettings.Padding = new System.Windows.Forms.Padding(4);
            this.tabServerSettings.Size = new System.Drawing.Size(835, 570);
            this.tabServerSettings.TabIndex = 2;
            this.tabServerSettings.Text = "Server";
            // 
            // btnTaskPublish
            // 
            this.btnTaskPublish.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnTaskPublish.Location = new System.Drawing.Point(19, 422);
            this.btnTaskPublish.Margin = new System.Windows.Forms.Padding(4);
            this.btnTaskPublish.Name = "btnTaskPublish";
            this.btnTaskPublish.Size = new System.Drawing.Size(196, 41);
            this.btnTaskPublish.TabIndex = 33;
            this.btnTaskPublish.Text = "Publish Task";
            this.btnTaskPublish.UseVisualStyleBackColor = true;
            this.btnTaskPublish.Click += new System.EventHandler(this.btnTaskPublish_Click);
            // 
            // lblClientGUIDDesc
            // 
            this.lblClientGUIDDesc.AutoSize = true;
            this.lblClientGUIDDesc.BackColor = System.Drawing.Color.Transparent;
            this.lblClientGUIDDesc.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClientGUIDDesc.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblClientGUIDDesc.Location = new System.Drawing.Point(19, 348);
            this.lblClientGUIDDesc.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblClientGUIDDesc.Name = "lblClientGUIDDesc";
            this.lblClientGUIDDesc.Size = new System.Drawing.Size(571, 25);
            this.lblClientGUIDDesc.TabIndex = 32;
            this.lblClientGUIDDesc.Text = "Indicates the GUID the client will use when connecting to taskt server";
            // 
            // lblServerSettingsDesc
            // 
            this.lblServerSettingsDesc.BackColor = System.Drawing.Color.Transparent;
            this.lblServerSettingsDesc.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServerSettingsDesc.ForeColor = System.Drawing.Color.SlateGray;
            this.lblServerSettingsDesc.Location = new System.Drawing.Point(13, 44);
            this.lblServerSettingsDesc.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblServerSettingsDesc.Name = "lblServerSettingsDesc";
            this.lblServerSettingsDesc.Size = new System.Drawing.Size(783, 82);
            this.lblServerSettingsDesc.TabIndex = 30;
            this.lblServerSettingsDesc.Text = resources.GetString("lblServerSettingsDesc.Text");
            // 
            // lblHTTPSDesc
            // 
            this.lblHTTPSDesc.AutoSize = true;
            this.lblHTTPSDesc.BackColor = System.Drawing.Color.Transparent;
            this.lblHTTPSDesc.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHTTPSDesc.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblHTTPSDesc.Location = new System.Drawing.Point(13, 217);
            this.lblHTTPSDesc.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHTTPSDesc.Name = "lblHTTPSDesc";
            this.lblHTTPSDesc.Size = new System.Drawing.Size(522, 25);
            this.lblHTTPSDesc.TabIndex = 29;
            this.lblHTTPSDesc.Text = "Enter the location of the taskt server ex. https://localhost:60281";
            // 
            // btnGetGUID
            // 
            this.btnGetGUID.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnGetGUID.Location = new System.Drawing.Point(16, 284);
            this.btnGetGUID.Margin = new System.Windows.Forms.Padding(4);
            this.btnGetGUID.Name = "btnGetGUID";
            this.btnGetGUID.Size = new System.Drawing.Size(196, 41);
            this.btnGetGUID.TabIndex = 28;
            this.btnGetGUID.Text = "Test Connection";
            this.btnGetGUID.UseVisualStyleBackColor = true;
            this.btnGetGUID.Click += new System.EventHandler(this.btnGetGUID_Click);
            // 
            // lblClientGUID
            // 
            this.lblClientGUID.AutoSize = true;
            this.lblClientGUID.BackColor = System.Drawing.Color.Transparent;
            this.lblClientGUID.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClientGUID.ForeColor = System.Drawing.Color.SlateGray;
            this.lblClientGUID.Location = new System.Drawing.Point(19, 330);
            this.lblClientGUID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblClientGUID.Name = "lblClientGUID";
            this.lblClientGUID.Size = new System.Drawing.Size(96, 23);
            this.lblClientGUID.TabIndex = 26;
            this.lblClientGUID.Text = "Client GUID";
            // 
            // txtGUID
            // 
            this.txtGUID.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtGUID.Location = new System.Drawing.Point(20, 379);
            this.txtGUID.Margin = new System.Windows.Forms.Padding(4);
            this.txtGUID.Name = "txtGUID";
            this.txtGUID.Size = new System.Drawing.Size(493, 34);
            this.txtGUID.TabIndex = 27;
            // 
            // lblHTTPSServerURL
            // 
            this.lblHTTPSServerURL.AutoSize = true;
            this.lblHTTPSServerURL.BackColor = System.Drawing.Color.Transparent;
            this.lblHTTPSServerURL.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHTTPSServerURL.ForeColor = System.Drawing.Color.SlateGray;
            this.lblHTTPSServerURL.Location = new System.Drawing.Point(15, 190);
            this.lblHTTPSServerURL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHTTPSServerURL.Name = "lblHTTPSServerURL";
            this.lblHTTPSServerURL.Size = new System.Drawing.Size(145, 23);
            this.lblHTTPSServerURL.TabIndex = 24;
            this.lblHTTPSServerURL.Text = "HTTPS Server URL";
            // 
            // txtHttpsAddress
            // 
            this.txtHttpsAddress.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtHttpsAddress.Location = new System.Drawing.Point(17, 244);
            this.txtHttpsAddress.Margin = new System.Windows.Forms.Padding(4);
            this.txtHttpsAddress.Name = "txtHttpsAddress";
            this.txtHttpsAddress.Size = new System.Drawing.Size(493, 34);
            this.txtHttpsAddress.TabIndex = 25;
            // 
            // chkBypassValidation
            // 
            this.chkBypassValidation.AutoSize = true;
            this.chkBypassValidation.BackColor = System.Drawing.Color.Transparent;
            this.chkBypassValidation.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBypassValidation.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkBypassValidation.Location = new System.Drawing.Point(449, 443);
            this.chkBypassValidation.Margin = new System.Windows.Forms.Padding(4);
            this.chkBypassValidation.Name = "chkBypassValidation";
            this.chkBypassValidation.Size = new System.Drawing.Size(174, 24);
            this.chkBypassValidation.TabIndex = 23;
            this.chkBypassValidation.Text = "Bypass SSL Validation";
            this.chkBypassValidation.UseVisualStyleBackColor = false;
            this.chkBypassValidation.Visible = false;
            this.chkBypassValidation.CheckedChanged += new System.EventHandler(this.chkBypassValidation_CheckedChanged);
            // 
            // btnCloseConnection
            // 
            this.btnCloseConnection.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnCloseConnection.Location = new System.Drawing.Point(417, 431);
            this.btnCloseConnection.Margin = new System.Windows.Forms.Padding(4);
            this.btnCloseConnection.Name = "btnCloseConnection";
            this.btnCloseConnection.Size = new System.Drawing.Size(100, 41);
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
            this.lblSocketException.Location = new System.Drawing.Point(104, 540);
            this.lblSocketException.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSocketException.Name = "lblSocketException";
            this.lblSocketException.Size = new System.Drawing.Size(0, 25);
            this.lblSocketException.TabIndex = 21;
            this.lblSocketException.Visible = false;
            // 
            // lblServerSettings
            // 
            this.lblServerSettings.AutoSize = true;
            this.lblServerSettings.BackColor = System.Drawing.Color.Transparent;
            this.lblServerSettings.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServerSettings.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblServerSettings.Location = new System.Drawing.Point(9, 7);
            this.lblServerSettings.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblServerSettings.Name = "lblServerSettings";
            this.lblServerSettings.Size = new System.Drawing.Size(185, 37);
            this.lblServerSettings.TabIndex = 20;
            this.lblServerSettings.Text = "Server Settings";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblStatus.Location = new System.Drawing.Point(104, 498);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 25);
            this.lblStatus.TabIndex = 16;
            this.lblStatus.Visible = false;
            // 
            // tabLocalListener
            // 
            this.tabLocalListener.Controls.Add(this.txtWhiteList);
            this.tabLocalListener.Controls.Add(this.chkEnableWhitelist);
            this.tabLocalListener.Controls.Add(this.chkAutoStartListener);
            this.tabLocalListener.Controls.Add(this.lblListeningPort);
            this.tabLocalListener.Controls.Add(this.txtListeningPort);
            this.tabLocalListener.Controls.Add(this.lblListeningStatus);
            this.tabLocalListener.Controls.Add(this.btnStopListening);
            this.tabLocalListener.Controls.Add(this.btnStartListening);
            this.tabLocalListener.Controls.Add(this.chkRequireListenerKey);
            this.tabLocalListener.Controls.Add(this.lblAuthKey);
            this.tabLocalListener.Controls.Add(this.txtAuthListeningKey);
            this.tabLocalListener.Controls.Add(this.chkEnableListening);
            this.tabLocalListener.Controls.Add(this.lblLocalListenerSettingsDesc);
            this.tabLocalListener.Controls.Add(this.lblLocalListenerSettings);
            this.tabLocalListener.Location = new System.Drawing.Point(4, 37);
            this.tabLocalListener.Margin = new System.Windows.Forms.Padding(4);
            this.tabLocalListener.Name = "tabLocalListener";
            this.tabLocalListener.Padding = new System.Windows.Forms.Padding(4);
            this.tabLocalListener.Size = new System.Drawing.Size(835, 570);
            this.tabLocalListener.TabIndex = 3;
            this.tabLocalListener.Text = "Local Listener";
            this.tabLocalListener.UseVisualStyleBackColor = true;
            // 
            // txtWhiteList
            // 
            this.txtWhiteList.Location = new System.Drawing.Point(20, 358);
            this.txtWhiteList.Margin = new System.Windows.Forms.Padding(4);
            this.txtWhiteList.Multiline = true;
            this.txtWhiteList.Name = "txtWhiteList";
            this.txtWhiteList.Size = new System.Drawing.Size(493, 114);
            this.txtWhiteList.TabIndex = 48;
            // 
            // chkEnableWhitelist
            // 
            this.chkEnableWhitelist.AutoSize = true;
            this.chkEnableWhitelist.BackColor = System.Drawing.Color.Transparent;
            this.chkEnableWhitelist.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkEnableWhitelist.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkEnableWhitelist.Location = new System.Drawing.Point(19, 321);
            this.chkEnableWhitelist.Margin = new System.Windows.Forms.Padding(4);
            this.chkEnableWhitelist.Name = "chkEnableWhitelist";
            this.chkEnableWhitelist.Size = new System.Drawing.Size(400, 29);
            this.chkEnableWhitelist.TabIndex = 47;
            this.chkEnableWhitelist.Text = "Enable IP Verification (Seperate with comma)";
            this.chkEnableWhitelist.UseVisualStyleBackColor = false;
            // 
            // chkAutoStartListener
            // 
            this.chkAutoStartListener.AutoSize = true;
            this.chkAutoStartListener.BackColor = System.Drawing.Color.Transparent;
            this.chkAutoStartListener.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutoStartListener.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkAutoStartListener.Location = new System.Drawing.Point(19, 101);
            this.chkAutoStartListener.Margin = new System.Windows.Forms.Padding(4);
            this.chkAutoStartListener.Name = "chkAutoStartListener";
            this.chkAutoStartListener.Size = new System.Drawing.Size(239, 29);
            this.chkAutoStartListener.TabIndex = 43;
            this.chkAutoStartListener.Text = "Start Listening on Startup";
            this.chkAutoStartListener.UseVisualStyleBackColor = false;
            // 
            // lblListeningPort
            // 
            this.lblListeningPort.AutoSize = true;
            this.lblListeningPort.BackColor = System.Drawing.Color.Transparent;
            this.lblListeningPort.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblListeningPort.ForeColor = System.Drawing.Color.SlateGray;
            this.lblListeningPort.Location = new System.Drawing.Point(15, 158);
            this.lblListeningPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblListeningPort.Name = "lblListeningPort";
            this.lblListeningPort.Size = new System.Drawing.Size(109, 23);
            this.lblListeningPort.TabIndex = 41;
            this.lblListeningPort.Text = "Listening Port";
            // 
            // txtListeningPort
            // 
            this.txtListeningPort.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtListeningPort.Location = new System.Drawing.Point(19, 182);
            this.txtListeningPort.Margin = new System.Windows.Forms.Padding(4);
            this.txtListeningPort.Name = "txtListeningPort";
            this.txtListeningPort.Size = new System.Drawing.Size(175, 34);
            this.txtListeningPort.TabIndex = 42;
            // 
            // lblListeningStatus
            // 
            this.lblListeningStatus.AutoSize = true;
            this.lblListeningStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblListeningStatus.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblListeningStatus.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblListeningStatus.Location = new System.Drawing.Point(13, 526);
            this.lblListeningStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblListeningStatus.Name = "lblListeningStatus";
            this.lblListeningStatus.Size = new System.Drawing.Size(175, 37);
            this.lblListeningStatus.TabIndex = 40;
            this.lblListeningStatus.Text = "Listening on {}";
            this.lblListeningStatus.Visible = false;
            // 
            // btnStopListening
            // 
            this.btnStopListening.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnStopListening.Location = new System.Drawing.Point(223, 481);
            this.btnStopListening.Margin = new System.Windows.Forms.Padding(4);
            this.btnStopListening.Name = "btnStopListening";
            this.btnStopListening.Size = new System.Drawing.Size(196, 41);
            this.btnStopListening.TabIndex = 39;
            this.btnStopListening.Text = "Stop Listening";
            this.btnStopListening.UseVisualStyleBackColor = true;
            this.btnStopListening.Click += new System.EventHandler(this.btnStopListening_Click);
            // 
            // btnStartListening
            // 
            this.btnStartListening.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnStartListening.Location = new System.Drawing.Point(19, 481);
            this.btnStartListening.Margin = new System.Windows.Forms.Padding(4);
            this.btnStartListening.Name = "btnStartListening";
            this.btnStartListening.Size = new System.Drawing.Size(196, 41);
            this.btnStartListening.TabIndex = 38;
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
            this.chkRequireListenerKey.Location = new System.Drawing.Point(19, 225);
            this.chkRequireListenerKey.Margin = new System.Windows.Forms.Padding(4);
            this.chkRequireListenerKey.Name = "chkRequireListenerKey";
            this.chkRequireListenerKey.Size = new System.Drawing.Size(253, 29);
            this.chkRequireListenerKey.TabIndex = 37;
            this.chkRequireListenerKey.Text = "Require Authentication Key";
            this.chkRequireListenerKey.UseVisualStyleBackColor = false;
            // 
            // lblAuthKey
            // 
            this.lblAuthKey.AutoSize = true;
            this.lblAuthKey.BackColor = System.Drawing.Color.Transparent;
            this.lblAuthKey.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAuthKey.ForeColor = System.Drawing.Color.SlateGray;
            this.lblAuthKey.Location = new System.Drawing.Point(15, 257);
            this.lblAuthKey.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAuthKey.Name = "lblAuthKey";
            this.lblAuthKey.Size = new System.Drawing.Size(147, 23);
            this.lblAuthKey.TabIndex = 34;
            this.lblAuthKey.Text = "Authentication Key";
            // 
            // txtAuthListeningKey
            // 
            this.txtAuthListeningKey.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtAuthListeningKey.Location = new System.Drawing.Point(19, 282);
            this.txtAuthListeningKey.Margin = new System.Windows.Forms.Padding(4);
            this.txtAuthListeningKey.Name = "txtAuthListeningKey";
            this.txtAuthListeningKey.Size = new System.Drawing.Size(493, 34);
            this.txtAuthListeningKey.TabIndex = 35;
            // 
            // chkEnableListening
            // 
            this.chkEnableListening.AutoSize = true;
            this.chkEnableListening.BackColor = System.Drawing.Color.Transparent;
            this.chkEnableListening.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkEnableListening.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkEnableListening.Location = new System.Drawing.Point(19, 129);
            this.chkEnableListening.Margin = new System.Windows.Forms.Padding(4);
            this.chkEnableListening.Name = "chkEnableListening";
            this.chkEnableListening.Size = new System.Drawing.Size(225, 29);
            this.chkEnableListening.TabIndex = 33;
            this.chkEnableListening.Text = "Local Listening Enabled";
            this.chkEnableListening.UseVisualStyleBackColor = false;
            // 
            // lblLocalListenerSettingsDesc
            // 
            this.lblLocalListenerSettingsDesc.BackColor = System.Drawing.Color.Transparent;
            this.lblLocalListenerSettingsDesc.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLocalListenerSettingsDesc.ForeColor = System.Drawing.Color.SlateGray;
            this.lblLocalListenerSettingsDesc.Location = new System.Drawing.Point(13, 44);
            this.lblLocalListenerSettingsDesc.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLocalListenerSettingsDesc.Name = "lblLocalListenerSettingsDesc";
            this.lblLocalListenerSettingsDesc.Size = new System.Drawing.Size(783, 82);
            this.lblLocalListenerSettingsDesc.TabIndex = 32;
            this.lblLocalListenerSettingsDesc.Text = "Enable this functionality to allow this computer to accept script execution reque" +
    "sts from other taskt or REST-capable clients.";
            // 
            // lblLocalListenerSettings
            // 
            this.lblLocalListenerSettings.AutoSize = true;
            this.lblLocalListenerSettings.BackColor = System.Drawing.Color.Transparent;
            this.lblLocalListenerSettings.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLocalListenerSettings.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblLocalListenerSettings.Location = new System.Drawing.Point(9, 7);
            this.lblLocalListenerSettings.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLocalListenerSettings.Name = "lblLocalListenerSettings";
            this.lblLocalListenerSettings.Size = new System.Drawing.Size(344, 37);
            this.lblLocalListenerSettings.TabIndex = 31;
            this.lblLocalListenerSettings.Text = "Local Listener Settings (BETA)";
            // 
            // tlpSettings
            // 
            this.tlpSettings.BackColor = System.Drawing.Color.Transparent;
            this.tlpSettings.ColumnCount = 1;
            this.tlpSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSettings.Controls.Add(this.uiSettingTabs, 0, 1);
            this.tlpSettings.Controls.Add(this.uiBtnOpen, 0, 2);
            this.tlpSettings.Controls.Add(this.pnlSettings, 0, 0);
            this.tlpSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSettings.Location = new System.Drawing.Point(0, 0);
            this.tlpSettings.Margin = new System.Windows.Forms.Padding(4);
            this.tlpSettings.Name = "tlpSettings";
            this.tlpSettings.RowCount = 3;
            this.tlpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 86F));
            this.tlpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 74F));
            this.tlpSettings.Size = new System.Drawing.Size(851, 779);
            this.tlpSettings.TabIndex = 26;
            // 
            // pnlSettings
            // 
            this.pnlSettings.BackColor = System.Drawing.Color.Transparent;
            this.pnlSettings.Controls.Add(this.lblMainLogo);
            this.pnlSettings.Controls.Add(this.lblManageSettings);
            this.pnlSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSettings.Location = new System.Drawing.Point(0, 0);
            this.pnlSettings.Margin = new System.Windows.Forms.Padding(0);
            this.pnlSettings.Name = "pnlSettings";
            this.pnlSettings.Size = new System.Drawing.Size(851, 86);
            this.pnlSettings.TabIndex = 26;
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
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(851, 779);
            this.Controls.Add(this.tlpSettings);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
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
            this.tlpSettings.ResumeLayout(false);
            this.pnlSettings.ResumeLayout(false);
            this.pnlSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblServerURL;
        private System.Windows.Forms.TextBox txtServerURL;
        private System.Windows.Forms.CheckBox chkAutomaticallyConnect;
        private System.Windows.Forms.CheckBox chkServerEnabled;
        private System.Windows.Forms.Label lblManageSettings;
        private System.Windows.Forms.CheckBox chkRetryOnDisconnect;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtPublicKey;
        private System.Windows.Forms.Label lblConnectKey;
        private System.Windows.Forms.CheckBox chkAutoCloseWindow;
        private System.Windows.Forms.CheckBox chkShowDebug;
        private CustomControls.UIPictureButton uiBtnOpen;
        private System.Windows.Forms.CheckBox chkEnableLogging;
        private System.Windows.Forms.Label lblMainLogo;
        private System.Windows.Forms.Label lblAutomationEngine;
        private System.Windows.Forms.Label lblApplicationSettings;
        private System.Windows.Forms.CheckBox chkAntiIdle;
        private System.Windows.Forms.Button btnUpdates;
        private System.Windows.Forms.CheckBox chkAdvancedDebug;
        private System.Windows.Forms.CheckBox chkCreateMissingVariables;
        private CustomControls.UITabControl uiSettingTabs;
        private System.Windows.Forms.TabPage tabAppSettings;
        private System.Windows.Forms.TabPage tabDebugSettings;
        private System.Windows.Forms.TabPage tabServerSettings;
        private System.Windows.Forms.Label lblServerSettings;
        private System.Windows.Forms.TableLayoutPanel tlpSettings;
        private System.Windows.Forms.Panel pnlSettings;
        private System.Windows.Forms.Label lblSocketException;
        private System.Windows.Forms.Button btnCloseConnection;
        private System.Windows.Forms.Timer tmrGetSocketStatus;
        private System.Windows.Forms.CheckBox chkBypassValidation;
        private System.Windows.Forms.Label lblRootFolder;
        private System.Windows.Forms.TextBox txtAppFolderPath;
        private System.Windows.Forms.Button btnSelectFolder;
        private System.Windows.Forms.TreeView tvExecutionTimes;
        private System.ComponentModel.BackgroundWorker bgwMetrics;
        private System.Windows.Forms.Label lblScriptExecutionMetrics;
        private System.Windows.Forms.Label lblGettingMetrics;
        private System.Windows.Forms.Button btnClearMetrics;
        private System.Windows.Forms.CheckBox chkTrackMetrics;
        private System.Windows.Forms.CheckBox chkInsertCommandsInline;
        private System.Windows.Forms.Button btnGenerateWikiDocs;
        private System.Windows.Forms.TextBox txtVariableStartMarker;
        private System.Windows.Forms.Label lblVariableDisplay;
        private System.Windows.Forms.TextBox txtVariableEndMarker;
        private System.Windows.Forms.Label lblVariablePatternDesc;
        private System.Windows.Forms.Label lblVariablePattern;
        private System.Windows.Forms.Label lblEndMarker;
        private System.Windows.Forms.Label lblStartMarker;
        private System.Windows.Forms.Label lblDelay;
        private System.Windows.Forms.TextBox txtCommandDelay;
        internal System.Windows.Forms.CheckBox chkOverrideInstances;
        private System.Windows.Forms.CheckBox chkSequenceDragDrop;
        private System.Windows.Forms.CheckBox chkMinimizeToTray;
        private System.Windows.Forms.Button btnLaunchAttendedMode;
        private System.Windows.Forms.Button btnSelectAttendedTaskFolder;
        private System.Windows.Forms.Label lblAttendedTasksFolder;
        private System.Windows.Forms.TextBox txtAttendedTaskFolder;
        private System.Windows.Forms.Label lblStartupMode;
        private System.Windows.Forms.ComboBox cboStartUpMode;
        private System.Windows.Forms.Button btnLaunchDisplayManager;
        private System.Windows.Forms.Label lblClientGUID;
        private System.Windows.Forms.TextBox txtGUID;
        private System.Windows.Forms.Label lblHTTPSServerURL;
        private System.Windows.Forms.TextBox txtHttpsAddress;
        private System.Windows.Forms.Button btnGetGUID;
        private System.Windows.Forms.Label lblServerSettingsDesc;
        private System.Windows.Forms.Label lblHTTPSDesc;
        private System.Windows.Forms.Label lblClientGUIDDesc;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnTaskPublish;
        private System.Windows.Forms.CheckBox chkPreloadCommands;
        private System.Windows.Forms.CheckBox chkSlimActionBar;
        private System.Windows.Forms.ComboBox cboCancellationKey;
        private System.Windows.Forms.Label lblEndScriptHotKey;
        private System.Windows.Forms.TabPage tabLocalListener;
        private System.Windows.Forms.Label lblLocalListenerSettingsDesc;
        private System.Windows.Forms.Label lblLocalListenerSettings;
        private System.Windows.Forms.Label lblAuthKey;
        private System.Windows.Forms.TextBox txtAuthListeningKey;
        private System.Windows.Forms.CheckBox chkEnableListening;
        private System.Windows.Forms.CheckBox chkRequireListenerKey;
        private System.Windows.Forms.Button btnStopListening;
        private System.Windows.Forms.Button btnStartListening;
        private System.Windows.Forms.Label lblListeningStatus;
        private System.Windows.Forms.CheckBox chkAutoStartListener;
        private System.Windows.Forms.Label lblListeningPort;
        private System.Windows.Forms.TextBox txtListeningPort;
        private System.Windows.Forms.CheckBox chkEnableWhitelist;
        private System.Windows.Forms.TextBox txtWhiteList;
        internal System.Windows.Forms.CheckBox chkAutoCalcVariables;
    }
}