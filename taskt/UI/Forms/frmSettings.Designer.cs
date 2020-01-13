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
            this.chkSequenceDragDrop = new System.Windows.Forms.CheckBox();
            this.btnGenerateWikiDocs = new System.Windows.Forms.Button();
            this.chkInsertCommandsInline = new System.Windows.Forms.CheckBox();
            this.btnClearMetrics = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.lblMetrics = new System.Windows.Forms.Label();
            this.tvExecutionTimes = new System.Windows.Forms.TreeView();
            this.btnSelectFolder = new System.Windows.Forms.Button();
            this.lblRootFolder = new System.Windows.Forms.Label();
            this.txtAppFolderPath = new System.Windows.Forms.TextBox();
            this.tabDebugSettings = new System.Windows.Forms.TabPage();
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tmrGetSocketStatus = new System.Windows.Forms.Timer(this.components);
            this.bgwMetrics = new System.ComponentModel.BackgroundWorker();
            this.chkAutoCalcVariables = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOpen)).BeginInit();
            this.uiSettingTabs.SuspendLayout();
            this.tabAppSettings.SuspendLayout();
            this.tabDebugSettings.SuspendLayout();
            this.tabServerSettings.SuspendLayout();
            this.tabLocalListener.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnConnect.Location = new System.Drawing.Point(312, 350);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 33);
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
            this.chkRetryOnDisconnect.Location = new System.Drawing.Point(302, 364);
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
            this.label1.Location = new System.Drawing.Point(6, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(452, 28);
            this.label1.TabIndex = 14;
            this.label1.Text = "Manage settings used by the application";
            // 
            // chkAutomaticallyConnect
            // 
            this.chkAutomaticallyConnect.AutoSize = true;
            this.chkAutomaticallyConnect.BackColor = System.Drawing.Color.Transparent;
            this.chkAutomaticallyConnect.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutomaticallyConnect.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkAutomaticallyConnect.Location = new System.Drawing.Point(12, 127);
            this.chkAutomaticallyConnect.Name = "chkAutomaticallyConnect";
            this.chkAutomaticallyConnect.Size = new System.Drawing.Size(158, 24);
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
            this.chkServerEnabled.Location = new System.Drawing.Point(12, 102);
            this.chkServerEnabled.Name = "chkServerEnabled";
            this.chkServerEnabled.Size = new System.Drawing.Size(205, 24);
            this.chkServerEnabled.TabIndex = 12;
            this.chkServerEnabled.Text = "Server Connection Enabled";
            this.chkServerEnabled.UseVisualStyleBackColor = false;
            // 
            // txtServerURL
            // 
            this.txtServerURL.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtServerURL.Location = new System.Drawing.Point(275, 360);
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
            this.label3.Location = new System.Drawing.Point(290, 364);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(232, 16);
            this.label3.TabIndex = 9;
            this.label3.Text = "Server URL ex. ws://localhost:port/ws)\r";
            this.label3.Visible = false;
            // 
            // txtPublicKey
            // 
            this.txtPublicKey.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtPublicKey.Location = new System.Drawing.Point(275, 354);
            this.txtPublicKey.Name = "txtPublicKey";
            this.txtPublicKey.Size = new System.Drawing.Size(371, 29);
            this.txtPublicKey.TabIndex = 19;
            this.txtPublicKey.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.SlateGray;
            this.label2.Location = new System.Drawing.Point(310, 367);
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
            this.chkEnableLogging.Location = new System.Drawing.Point(10, 76);
            this.chkEnableLogging.Name = "chkEnableLogging";
            this.chkEnableLogging.Size = new System.Drawing.Size(212, 25);
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
            this.chkAutoCloseWindow.Location = new System.Drawing.Point(10, 56);
            this.chkAutoCloseWindow.Name = "chkAutoCloseWindow";
            this.chkAutoCloseWindow.Size = new System.Drawing.Size(275, 25);
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
            this.chkShowDebug.Location = new System.Drawing.Point(10, 36);
            this.chkShowDebug.Name = "chkShowDebug";
            this.chkShowDebug.Size = new System.Drawing.Size(177, 25);
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
            this.uiBtnOpen.Location = new System.Drawing.Point(3, 576);
            this.uiBtnOpen.Name = "uiBtnOpen";
            this.uiBtnOpen.Size = new System.Drawing.Size(48, 48);
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
            this.lblOptions.TabIndex = 15;
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
            this.lblApplicationSettings.TabIndex = 21;
            this.lblApplicationSettings.Text = "Application Settings";
            // 
            // chkAntiIdle
            // 
            this.chkAntiIdle.AutoSize = true;
            this.chkAntiIdle.BackColor = System.Drawing.Color.Transparent;
            this.chkAntiIdle.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAntiIdle.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkAntiIdle.Location = new System.Drawing.Point(10, 36);
            this.chkAntiIdle.Name = "chkAntiIdle";
            this.chkAntiIdle.Size = new System.Drawing.Size(209, 24);
            this.chkAntiIdle.TabIndex = 20;
            this.chkAntiIdle.Text = "Anti-Idle (while app is open)";
            this.chkAntiIdle.UseVisualStyleBackColor = false;
            // 
            // btnUpdates
            // 
            this.btnUpdates.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdates.Location = new System.Drawing.Point(12, 331);
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
            this.chkAdvancedDebug.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAdvancedDebug.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkAdvancedDebug.Location = new System.Drawing.Point(10, 96);
            this.chkAdvancedDebug.Name = "chkAdvancedDebug";
            this.chkAdvancedDebug.Size = new System.Drawing.Size(344, 25);
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
            this.chkCreateMissingVariables.Location = new System.Drawing.Point(10, 116);
            this.chkCreateMissingVariables.Name = "chkCreateMissingVariables";
            this.chkCreateMissingVariables.Size = new System.Drawing.Size(318, 25);
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
            this.uiSettingTabs.Location = new System.Drawing.Point(3, 73);
            this.uiSettingTabs.Name = "uiSettingTabs";
            this.uiSettingTabs.SelectedIndex = 0;
            this.uiSettingTabs.Size = new System.Drawing.Size(632, 497);
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
            this.tabAppSettings.Controls.Add(this.label6);
            this.tabAppSettings.Controls.Add(this.txtAttendedTaskFolder);
            this.tabAppSettings.Controls.Add(this.btnLaunchAttendedMode);
            this.tabAppSettings.Controls.Add(this.chkMinimizeToTray);
            this.tabAppSettings.Controls.Add(this.chkSequenceDragDrop);
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
            this.tabAppSettings.Size = new System.Drawing.Size(624, 463);
            this.tabAppSettings.TabIndex = 0;
            this.tabAppSettings.Text = "Application";
            // 
            // chkSlimActionBar
            // 
            this.chkSlimActionBar.AutoSize = true;
            this.chkSlimActionBar.BackColor = System.Drawing.Color.Transparent;
            this.chkSlimActionBar.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSlimActionBar.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkSlimActionBar.Location = new System.Drawing.Point(10, 115);
            this.chkSlimActionBar.Name = "chkSlimActionBar";
            this.chkSlimActionBar.Size = new System.Drawing.Size(154, 24);
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
            this.chkPreloadCommands.Location = new System.Drawing.Point(10, 135);
            this.chkPreloadCommands.Name = "chkPreloadCommands";
            this.chkPreloadCommands.Size = new System.Drawing.Size(320, 24);
            this.chkPreloadCommands.TabIndex = 41;
            this.chkPreloadCommands.Text = "Load Commands at Startup (Reduces Flicker)";
            this.chkPreloadCommands.UseVisualStyleBackColor = false;
            this.chkPreloadCommands.Visible = false;
            // 
            // btnLaunchDisplayManager
            // 
            this.btnLaunchDisplayManager.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLaunchDisplayManager.Location = new System.Drawing.Point(225, 364);
            this.btnLaunchDisplayManager.Name = "btnLaunchDisplayManager";
            this.btnLaunchDisplayManager.Size = new System.Drawing.Size(207, 27);
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
            this.lblStartupMode.Location = new System.Drawing.Point(10, 274);
            this.lblStartupMode.Name = "lblStartupMode";
            this.lblStartupMode.Size = new System.Drawing.Size(70, 17);
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
            this.cboStartUpMode.Location = new System.Drawing.Point(13, 293);
            this.cboStartUpMode.Name = "cboStartUpMode";
            this.cboStartUpMode.Size = new System.Drawing.Size(219, 29);
            this.cboStartUpMode.TabIndex = 38;
            // 
            // btnSelectAttendedTaskFolder
            // 
            this.btnSelectAttendedTaskFolder.Location = new System.Drawing.Point(504, 241);
            this.btnSelectAttendedTaskFolder.Name = "btnSelectAttendedTaskFolder";
            this.btnSelectAttendedTaskFolder.Size = new System.Drawing.Size(42, 30);
            this.btnSelectAttendedTaskFolder.TabIndex = 37;
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
            this.label6.Location = new System.Drawing.Point(9, 223);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(130, 17);
            this.label6.TabIndex = 36;
            this.label6.Text = "Attended Tasks Folder";
            // 
            // txtAttendedTaskFolder
            // 
            this.txtAttendedTaskFolder.Location = new System.Drawing.Point(12, 242);
            this.txtAttendedTaskFolder.Name = "txtAttendedTaskFolder";
            this.txtAttendedTaskFolder.Size = new System.Drawing.Size(490, 29);
            this.txtAttendedTaskFolder.TabIndex = 35;
            // 
            // btnLaunchAttendedMode
            // 
            this.btnLaunchAttendedMode.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLaunchAttendedMode.Location = new System.Drawing.Point(225, 331);
            this.btnLaunchAttendedMode.Name = "btnLaunchAttendedMode";
            this.btnLaunchAttendedMode.Size = new System.Drawing.Size(207, 27);
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
            this.chkMinimizeToTray.Location = new System.Drawing.Point(10, 96);
            this.chkMinimizeToTray.Name = "chkMinimizeToTray";
            this.chkMinimizeToTray.Size = new System.Drawing.Size(186, 24);
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
            this.chkSequenceDragDrop.Location = new System.Drawing.Point(10, 76);
            this.chkSequenceDragDrop.Name = "chkSequenceDragDrop";
            this.chkSequenceDragDrop.Size = new System.Drawing.Size(341, 24);
            this.chkSequenceDragDrop.TabIndex = 32;
            this.chkSequenceDragDrop.Text = "Allow Drag and Drop into Sequence Commands";
            this.chkSequenceDragDrop.UseVisualStyleBackColor = false;
            // 
            // btnGenerateWikiDocs
            // 
            this.btnGenerateWikiDocs.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerateWikiDocs.Location = new System.Drawing.Point(12, 364);
            this.btnGenerateWikiDocs.Name = "btnGenerateWikiDocs";
            this.btnGenerateWikiDocs.Size = new System.Drawing.Size(207, 27);
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
            this.chkInsertCommandsInline.Location = new System.Drawing.Point(10, 56);
            this.chkInsertCommandsInline.Name = "chkInsertCommandsInline";
            this.chkInsertCommandsInline.Size = new System.Drawing.Size(351, 24);
            this.chkInsertCommandsInline.TabIndex = 30;
            this.chkInsertCommandsInline.Text = "New Commands Insert Below Selected Command";
            this.chkInsertCommandsInline.UseVisualStyleBackColor = false;
            // 
            // btnClearMetrics
            // 
            this.btnClearMetrics.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearMetrics.Location = new System.Drawing.Point(11, 551);
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
            this.label5.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.SlateGray;
            this.label5.Location = new System.Drawing.Point(13, 401);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(248, 17);
            this.label5.TabIndex = 28;
            this.label5.Text = "Script Execution Metrics (Last 10 per Script)";
            // 
            // lblMetrics
            // 
            this.lblMetrics.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDownGrid;
            this.lblMetrics.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMetrics.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblMetrics.Location = new System.Drawing.Point(12, 420);
            this.lblMetrics.Name = "lblMetrics";
            this.lblMetrics.Size = new System.Drawing.Size(534, 127);
            this.lblMetrics.TabIndex = 27;
            this.lblMetrics.Text = "Getting Metrics...";
            // 
            // tvExecutionTimes
            // 
            this.tvExecutionTimes.Location = new System.Drawing.Point(12, 420);
            this.tvExecutionTimes.Name = "tvExecutionTimes";
            this.tvExecutionTimes.Size = new System.Drawing.Size(534, 127);
            this.tvExecutionTimes.TabIndex = 26;
            this.tvExecutionTimes.Visible = false;
            // 
            // btnSelectFolder
            // 
            this.btnSelectFolder.Location = new System.Drawing.Point(504, 190);
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
            this.lblRootFolder.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRootFolder.ForeColor = System.Drawing.Color.SlateGray;
            this.lblRootFolder.Location = new System.Drawing.Point(9, 171);
            this.lblRootFolder.Name = "lblRootFolder";
            this.lblRootFolder.Size = new System.Drawing.Size(101, 17);
            this.lblRootFolder.TabIndex = 24;
            this.lblRootFolder.Text = "taskt Root Folder";
            // 
            // txtAppFolderPath
            // 
            this.txtAppFolderPath.Location = new System.Drawing.Point(12, 190);
            this.txtAppFolderPath.Name = "txtAppFolderPath";
            this.txtAppFolderPath.Size = new System.Drawing.Size(490, 29);
            this.txtAppFolderPath.TabIndex = 23;
            // 
            // tabDebugSettings
            // 
            this.tabDebugSettings.BackColor = System.Drawing.Color.WhiteSmoke;
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
            this.tabDebugSettings.Size = new System.Drawing.Size(624, 463);
            this.tabDebugSettings.TabIndex = 1;
            this.tabDebugSettings.Text = "Automation Engine";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.SteelBlue;
            this.label16.Location = new System.Drawing.Point(6, 424);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(134, 21);
            this.label16.TabIndex = 37;
            this.label16.Text = "End Script Hotkey:";
            // 
            // cboCancellationKey
            // 
            this.cboCancellationKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCancellationKey.FormattingEnabled = true;
            this.cboCancellationKey.Location = new System.Drawing.Point(142, 422);
            this.cboCancellationKey.Name = "cboCancellationKey";
            this.cboCancellationKey.Size = new System.Drawing.Size(154, 29);
            this.cboCancellationKey.TabIndex = 36;
            // 
            // chkOverrideInstances
            // 
            this.chkOverrideInstances.AutoSize = true;
            this.chkOverrideInstances.BackColor = System.Drawing.Color.Transparent;
            this.chkOverrideInstances.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkOverrideInstances.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkOverrideInstances.Location = new System.Drawing.Point(10, 156);
            this.chkOverrideInstances.Name = "chkOverrideInstances";
            this.chkOverrideInstances.Size = new System.Drawing.Size(187, 25);
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
            this.lblDelay.Location = new System.Drawing.Point(6, 207);
            this.lblDelay.Name = "lblDelay";
            this.lblDelay.Size = new System.Drawing.Size(349, 21);
            this.lblDelay.TabIndex = 34;
            this.lblDelay.Text = "Default delay between executing commands (ms):";
            // 
            // txtCommandDelay
            // 
            this.txtCommandDelay.Location = new System.Drawing.Point(359, 205);
            this.txtCommandDelay.Name = "txtCommandDelay";
            this.txtCommandDelay.Size = new System.Drawing.Size(77, 29);
            this.txtCommandDelay.TabIndex = 33;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.SteelBlue;
            this.label10.Location = new System.Drawing.Point(15, 372);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(98, 21);
            this.label10.TabIndex = 32;
            this.label10.Text = "End Marker:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.SteelBlue;
            this.label9.Location = new System.Drawing.Point(12, 338);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(106, 21);
            this.label9.TabIndex = 31;
            this.label9.Text = "Start Marker:";
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.SteelBlue;
            this.label8.Location = new System.Drawing.Point(10, 259);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(410, 72);
            this.label8.TabIndex = 30;
            this.label8.Text = "Indicate the start and end markers for variables.  When the engine runs, it will " +
    "automatically replace the variable with the stored value.";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.SteelBlue;
            this.label7.Location = new System.Drawing.Point(6, 232);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(149, 30);
            this.label7.TabIndex = 29;
            this.label7.Text = "Variable Pattern";
            // 
            // lblVariableDisplay
            // 
            this.lblVariableDisplay.AutoSize = true;
            this.lblVariableDisplay.BackColor = System.Drawing.Color.Transparent;
            this.lblVariableDisplay.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVariableDisplay.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblVariableDisplay.Location = new System.Drawing.Point(163, 353);
            this.lblVariableDisplay.Name = "lblVariableDisplay";
            this.lblVariableDisplay.Size = new System.Drawing.Size(133, 25);
            this.lblVariableDisplay.TabIndex = 28;
            this.lblVariableDisplay.Text = "VariableName";
            // 
            // txtVariableEndMarker
            // 
            this.txtVariableEndMarker.Location = new System.Drawing.Point(119, 368);
            this.txtVariableEndMarker.Name = "txtVariableEndMarker";
            this.txtVariableEndMarker.Size = new System.Drawing.Size(26, 29);
            this.txtVariableEndMarker.TabIndex = 27;
            this.txtVariableEndMarker.TextChanged += new System.EventHandler(this.txtVariableStartMarker_TextChanged);
            // 
            // txtVariableStartMarker
            // 
            this.txtVariableStartMarker.Location = new System.Drawing.Point(119, 335);
            this.txtVariableStartMarker.Name = "txtVariableStartMarker";
            this.txtVariableStartMarker.Size = new System.Drawing.Size(26, 29);
            this.txtVariableStartMarker.TabIndex = 26;
            this.txtVariableStartMarker.TextChanged += new System.EventHandler(this.txtVariableStartMarker_TextChanged);
            // 
            // chkTrackMetrics
            // 
            this.chkTrackMetrics.AutoSize = true;
            this.chkTrackMetrics.BackColor = System.Drawing.Color.Transparent;
            this.chkTrackMetrics.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkTrackMetrics.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkTrackMetrics.Location = new System.Drawing.Point(10, 136);
            this.chkTrackMetrics.Name = "chkTrackMetrics";
            this.chkTrackMetrics.Size = new System.Drawing.Size(188, 25);
            this.chkTrackMetrics.TabIndex = 25;
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
            this.tabServerSettings.Size = new System.Drawing.Size(624, 463);
            this.tabServerSettings.TabIndex = 2;
            this.tabServerSettings.Text = "Server";
            // 
            // btnTaskPublish
            // 
            this.btnTaskPublish.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnTaskPublish.Location = new System.Drawing.Point(14, 343);
            this.btnTaskPublish.Name = "btnTaskPublish";
            this.btnTaskPublish.Size = new System.Drawing.Size(147, 33);
            this.btnTaskPublish.TabIndex = 33;
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
            this.label15.Location = new System.Drawing.Point(14, 283);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(453, 20);
            this.label15.TabIndex = 32;
            this.label15.Text = "Indicates the GUID the client will use when connecting to taskt server";
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.SlateGray;
            this.label14.Location = new System.Drawing.Point(10, 36);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(587, 67);
            this.label14.TabIndex = 30;
            this.label14.Text = resources.GetString("label14.Text");
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.SteelBlue;
            this.label13.Location = new System.Drawing.Point(10, 176);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(418, 20);
            this.label13.TabIndex = 29;
            this.label13.Text = "Enter the location of the taskt server ex. https://localhost:60281";
            // 
            // btnGetGUID
            // 
            this.btnGetGUID.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnGetGUID.Location = new System.Drawing.Point(12, 231);
            this.btnGetGUID.Name = "btnGetGUID";
            this.btnGetGUID.Size = new System.Drawing.Size(147, 33);
            this.btnGetGUID.TabIndex = 28;
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
            this.label12.Location = new System.Drawing.Point(14, 268);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(74, 17);
            this.label12.TabIndex = 26;
            this.label12.Text = "Client GUID";
            // 
            // txtGUID
            // 
            this.txtGUID.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtGUID.Location = new System.Drawing.Point(15, 308);
            this.txtGUID.Name = "txtGUID";
            this.txtGUID.Size = new System.Drawing.Size(371, 29);
            this.txtGUID.TabIndex = 27;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.SlateGray;
            this.label11.Location = new System.Drawing.Point(11, 154);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(111, 17);
            this.label11.TabIndex = 24;
            this.label11.Text = "HTTPS Server URL";
            // 
            // txtHttpsAddress
            // 
            this.txtHttpsAddress.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtHttpsAddress.Location = new System.Drawing.Point(13, 198);
            this.txtHttpsAddress.Name = "txtHttpsAddress";
            this.txtHttpsAddress.Size = new System.Drawing.Size(371, 29);
            this.txtHttpsAddress.TabIndex = 25;
            // 
            // chkBypassValidation
            // 
            this.chkBypassValidation.AutoSize = true;
            this.chkBypassValidation.BackColor = System.Drawing.Color.Transparent;
            this.chkBypassValidation.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBypassValidation.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkBypassValidation.Location = new System.Drawing.Point(337, 360);
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
            this.btnCloseConnection.Location = new System.Drawing.Point(313, 350);
            this.btnCloseConnection.Name = "btnCloseConnection";
            this.btnCloseConnection.Size = new System.Drawing.Size(75, 33);
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
            this.lblSocketException.Location = new System.Drawing.Point(78, 439);
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
            this.label4.TabIndex = 20;
            this.label4.Text = "Server Settings";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblStatus.Location = new System.Drawing.Point(78, 405);
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
            this.tabLocalListener.Size = new System.Drawing.Size(624, 463);
            this.tabLocalListener.TabIndex = 3;
            this.tabLocalListener.Text = "Local Listener";
            this.tabLocalListener.UseVisualStyleBackColor = true;
            // 
            // txtWhiteList
            // 
            this.txtWhiteList.Location = new System.Drawing.Point(15, 291);
            this.txtWhiteList.Multiline = true;
            this.txtWhiteList.Name = "txtWhiteList";
            this.txtWhiteList.Size = new System.Drawing.Size(371, 93);
            this.txtWhiteList.TabIndex = 48;
            // 
            // chkEnableWhitelist
            // 
            this.chkEnableWhitelist.AutoSize = true;
            this.chkEnableWhitelist.BackColor = System.Drawing.Color.Transparent;
            this.chkEnableWhitelist.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkEnableWhitelist.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkEnableWhitelist.Location = new System.Drawing.Point(14, 261);
            this.chkEnableWhitelist.Name = "chkEnableWhitelist";
            this.chkEnableWhitelist.Size = new System.Drawing.Size(318, 24);
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
            this.chkAutoStartListener.Location = new System.Drawing.Point(14, 82);
            this.chkAutoStartListener.Name = "chkAutoStartListener";
            this.chkAutoStartListener.Size = new System.Drawing.Size(193, 24);
            this.chkAutoStartListener.TabIndex = 43;
            this.chkAutoStartListener.Text = "Start Listening on Startup";
            this.chkAutoStartListener.UseVisualStyleBackColor = false;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.BackColor = System.Drawing.Color.Transparent;
            this.label19.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.SlateGray;
            this.label19.Location = new System.Drawing.Point(11, 128);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(83, 17);
            this.label19.TabIndex = 41;
            this.label19.Text = "Listening Port";
            // 
            // txtListeningPort
            // 
            this.txtListeningPort.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtListeningPort.Location = new System.Drawing.Point(14, 148);
            this.txtListeningPort.Name = "txtListeningPort";
            this.txtListeningPort.Size = new System.Drawing.Size(132, 29);
            this.txtListeningPort.TabIndex = 42;
            // 
            // lblListeningStatus
            // 
            this.lblListeningStatus.AutoSize = true;
            this.lblListeningStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblListeningStatus.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblListeningStatus.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblListeningStatus.Location = new System.Drawing.Point(10, 427);
            this.lblListeningStatus.Name = "lblListeningStatus";
            this.lblListeningStatus.Size = new System.Drawing.Size(137, 30);
            this.lblListeningStatus.TabIndex = 40;
            this.lblListeningStatus.Text = "Listening on {}";
            this.lblListeningStatus.Visible = false;
            // 
            // btnStopListening
            // 
            this.btnStopListening.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnStopListening.Location = new System.Drawing.Point(167, 391);
            this.btnStopListening.Name = "btnStopListening";
            this.btnStopListening.Size = new System.Drawing.Size(147, 33);
            this.btnStopListening.TabIndex = 39;
            this.btnStopListening.Text = "Stop Listening";
            this.btnStopListening.UseVisualStyleBackColor = true;
            this.btnStopListening.Click += new System.EventHandler(this.btnStopListening_Click);
            // 
            // btnStartListening
            // 
            this.btnStartListening.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnStartListening.Location = new System.Drawing.Point(14, 391);
            this.btnStartListening.Name = "btnStartListening";
            this.btnStartListening.Size = new System.Drawing.Size(147, 33);
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
            this.chkRequireListenerKey.Location = new System.Drawing.Point(14, 183);
            this.chkRequireListenerKey.Name = "chkRequireListenerKey";
            this.chkRequireListenerKey.Size = new System.Drawing.Size(204, 24);
            this.chkRequireListenerKey.TabIndex = 37;
            this.chkRequireListenerKey.Text = "Require Authentication Key";
            this.chkRequireListenerKey.UseVisualStyleBackColor = false;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.BackColor = System.Drawing.Color.Transparent;
            this.label20.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.ForeColor = System.Drawing.Color.SlateGray;
            this.label20.Location = new System.Drawing.Point(11, 209);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(113, 17);
            this.label20.TabIndex = 34;
            this.label20.Text = "Authentication Key";
            // 
            // txtAuthListeningKey
            // 
            this.txtAuthListeningKey.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtAuthListeningKey.Location = new System.Drawing.Point(14, 229);
            this.txtAuthListeningKey.Name = "txtAuthListeningKey";
            this.txtAuthListeningKey.Size = new System.Drawing.Size(371, 29);
            this.txtAuthListeningKey.TabIndex = 35;
            // 
            // chkEnableListening
            // 
            this.chkEnableListening.AutoSize = true;
            this.chkEnableListening.BackColor = System.Drawing.Color.Transparent;
            this.chkEnableListening.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkEnableListening.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkEnableListening.Location = new System.Drawing.Point(14, 105);
            this.chkEnableListening.Name = "chkEnableListening";
            this.chkEnableListening.Size = new System.Drawing.Size(180, 24);
            this.chkEnableListening.TabIndex = 33;
            this.chkEnableListening.Text = "Local Listening Enabled";
            this.chkEnableListening.UseVisualStyleBackColor = false;
            // 
            // label17
            // 
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.SlateGray;
            this.label17.Location = new System.Drawing.Point(10, 36);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(587, 67);
            this.label17.TabIndex = 32;
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
            this.label18.TabIndex = 31;
            this.label18.Text = "Local Listener Settings (BETA)";
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
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(638, 633);
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
            this.panel1.Size = new System.Drawing.Size(638, 70);
            this.panel1.TabIndex = 26;
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
            // chkAutoCalcVariables
            // 
            this.chkAutoCalcVariables.AutoSize = true;
            this.chkAutoCalcVariables.BackColor = System.Drawing.Color.Transparent;
            this.chkAutoCalcVariables.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutoCalcVariables.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkAutoCalcVariables.Location = new System.Drawing.Point(10, 178);
            this.chkAutoCalcVariables.Name = "chkAutoCalcVariables";
            this.chkAutoCalcVariables.Size = new System.Drawing.Size(255, 25);
            this.chkAutoCalcVariables.TabIndex = 38;
            this.chkAutoCalcVariables.Text = "Calculate Variables Automatically";
            this.chkAutoCalcVariables.UseVisualStyleBackColor = false;
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 633);
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
        private System.Windows.Forms.CheckBox chkInsertCommandsInline;
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
        private System.Windows.Forms.CheckBox chkSequenceDragDrop;
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
    }
}