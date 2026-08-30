using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using taskt.Core;
using taskt.Core.IO;

namespace taskt.UI.Forms.ScriptBuilder.Supplemental
{
    public partial class frmNewSettings : DialogLikeThemedForm
    {
        /// <summary>
        /// new settings
        /// </summary>
        ApplicationSettings newAppSettings;

        /// <summary>
        /// parent scriptBuilderForm
        /// </summary>
        readonly frmScriptBuilder scriptBuilderForm;

        /// <summary>
        /// previous selected settings page name
        /// </summary>
        private string prevPage = "";

        // Metric
        //private Label lblMetrics = null;
        //private TreeView tvExecutionTimes = null;
        //private Button btnClearMetrics = null;

        // bgwMetricEvents
        private DoWorkEventHandler bgwMetricsDoWorkEvent = null;
        private RunWorkerCompletedEventHandler bgwMetricsRunWorkerCompletedEvent = null;

        // Local Listener
        //private Button btnStartListening = null;
        //private Button btnStopListening = null;
        //private Label lblListeningState = null;

        // Server
        //private Label lblSocketState = null;
        //private Label lblSocketException = null;

        // tmrGetSocketStatus Events
        private EventHandler tmrGetScoketStatusTickEvent = null;
        private EventHandler tcpListenerListenStartEvent = null;
        private EventHandler tcpListenerListenStopEvent = null;

        /// <summary>
        /// keyboard keys list
        /// </summary>
        private readonly string[] keysList;

        private enum FontSize
        {
            Small,
            Normal,
            NormalBold,
            Large
        }

        #region form events
        public frmNewSettings(frmScriptBuilder fm)
        {
            InitializeComponent();
            this.scriptBuilderForm = fm;

            var keys = Enum.GetValues(typeof(Keys));
            keysList = new string[keys.Length];
            int i = 0;
            foreach (var key in keys)
            {
                keysList[i++] = key.ToString();
            }
        }

        private void frmNewSettings_Load(object sender, EventArgs e)
        {
            //newAppSettings = new Core.ApplicationSettings();
            //newAppSettings = newAppSettings.GetOrCreateApplicationSettings();
            newAppSettings = ApplicationSettings.GetOrCreateApplicationSettings(App.Taskt_Settings_File_Path);

            // Network -> Server
            //Core.Server.LocalTCPListener.ListeningStarted += AutomationTCPListener_ListeningStarted;
            //Core.Server.LocalTCPListener.ListeningStopped += AutomationTCPListener_ListeningStopped;

            tvSettingsMenu.ExpandAll();

            tvSettingsMenu.Nodes[0].EnsureVisible();
        }
        #endregion

        #region footer buttons
        private void uiBtnOpen_Click(object sender, EventArgs e)
        {
            //Keys key = (Keys)Enum.Parse(typeof(Keys), cboCancellationKey.Text);
            //newAppSettings.EngineSettings.CancellationKey = key;
            //newAppSettings.Save(newAppSettings);
            //newAppSettings.Save();
            newAppSettings.Save(App.Taskt_Settings_File_Path);
            App.UpdateSettings(App.Taskt_Settings_File_Path);
            Core.Server.SocketClient.LoadSettings();
            this.Close();
        }

        private void uiCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region tvSettingMenu
        private void tvSettingsMenu_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode rootNode = tvSettingsMenu.SelectedNode;
            while (rootNode.Parent != null)
            {
                rootNode = rootNode.Parent;
            }

            flowLayoutSettings.SuspendLayout();

            string newPage = $"{rootNode.Text} - {tvSettingsMenu.SelectedNode.Text}";

            switch (newPage)
            {
                case "Application - Debug":
                    showApplicationDebugSettings();
                    break;
                case "Application - Folder":
                    showApplicationFolderSettings();
                    break;
                case "Application - Other":
                    showApplicationOtherSettings();
                    break;
                case "Application - Script File":
                    showApplicationScriptFileSettings();
                    break;
                case "Application - Script Metric":
                    showApplicationMetricSettings();
                    break;
                case "Application - Settings File":
                    showApplicationSettingsFile();
                    break;
                case "Application - Start Up":
                    showApplicationStartUpSetting();
                    break;

                case "Automation Engine - Engine":
                    showAutomationEngineEngineSettings();
                    break;
                case "Automation Engine - Keyword":
                    showAutomationEngineKeywordSettings();
                    break;
                case "Automation Engine - Log":
                    showAutomationEngineLogSettings();
                    break;
                case "Automation Engine - Parser":
                    showAutomationEngineParserSettings();
                    break;
                case "Automation Engine - Variable":
                    showAutomationEngineVariableSettings();
                    break;

                case "Documents - Command Reference":
                    showDocumentsCommandReferenceSettings();
                    break;

                case "Editor - Command Editor":
                    showEditorCommandEditorSettings();
                    break;
                case "Editor - Command List":
                    showEditorCommandListSettings();
                    break;
                case "Editor - GUI Inspect Tool":
                    showEditorGUIInspectToolSettings();
                    break;
                case "Editor - Indent":
                    showEditorIndentSettings();
                    break;
                case "Editor - Instance":
                    showEditorInstanceSettings();
                    break;
                case "Editor - Insert Command":
                    showEditorInsertCommandSettings();
                    break;
                case "Editor - Mini Map":
                    showEditorMiniMapSettings();
                    break;
                case "Editor - Menu Bar":
                    showEditorMenuBarSettings();
                    break;
                case "Editor - Status Bar":
                    showEditorStatusBarSettings();
                    break;
                case "Editor - Validation":
                    showEditorValidationSettings();
                    break;
                case "Editor - Variable":
                    showEditorVariableSettings();
                    break;

                case "Network - Local Listener":
                    showNetworkLocalListerSettings();
                    break;
                case "Network - Server":
                    showNetworkServerSettings();
                    break;

                case "Update - Check Update":
                    showUpdateCheckUpdateSettings();
                    break;

                case "VM - Display Manager":
                    showVMDisplayManagerSettings();
                    break;

                default:
                    newPage = prevPage;
                    break;
            }
            updatePrevPage(newPage);

            flowLayoutSettings.ResumeLayout();
        }

        private void updatePrevPage(string currentPage)
        {
            if (currentPage == prevPage)
            {
                return;
            }
            else
            {
                switch (prevPage)
                {
                    case "Application - Script Metric":
                        //lblMetrics = null;
                        //tvExecutionTimes = null;
                        //btnClearMetrics = null;
                        if (bgwMetricsDoWorkEvent != null)
                        {
                            bgwMetrics.DoWork -= bgwMetricsDoWorkEvent;
                            bgwMetricsDoWorkEvent = null;
                        }
                        if (bgwMetricsRunWorkerCompletedEvent != null)
                        {
                            bgwMetrics.RunWorkerCompleted -= bgwMetricsRunWorkerCompletedEvent;
                            bgwMetricsRunWorkerCompletedEvent = null;
                        }
                        break;

                    case "Network - Local Listener":
                        //btnStartListening = null;
                        //btnStopListening = null;
                        break;

                    case "Network - Server":
                        //lblSocketState = null;
                        //lblSocketException = null;
                        if (tmrGetScoketStatusTickEvent != null)
                        {
                            tmrGetSocketStatus.Tick -= tmrGetScoketStatusTickEvent;
                            tmrGetScoketStatusTickEvent = null;
                        }
                        if (tcpListenerListenStartEvent != null)
                        {
                            Core.Server.LocalTCPListener.ListeningStarted -= tcpListenerListenStartEvent;
                            tcpListenerListenStartEvent = null;
                        }
                        if (tcpListenerListenStopEvent != null)
                        {
                            Core.Server.LocalTCPListener.ListeningStopped -= tcpListenerListenStopEvent;
                            tcpListenerListenStopEvent = null;
                        }
                        break;
                }
            }
            prevPage = currentPage;
        }

        #endregion

        #region Application
        private void showApplicationStartUpSetting()
        {
            RemoveSettingControls();

            CreateLabel("lblTitie", "Start Up", FontSize.Large, true);
            CreateCheckBox("chkAntiIdle", "Anti-Idle (while app is open)", newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.AntiIdleWhileOpen), true);
            var chkPre = CreateCheckBox("chkPreLoadCommands", "Load Commands at Startup (Reduces Flicker)", newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.PreloadBuilderCommands), true);
            chkPre.Visible = false;
            CreateLabel("lblStartMode", "Start Mode:", FontSize.Normal, false);
            var cmbStart = CreateComboBox("cmbStartMode", new string[] { "Builder Mode","Attended Task Mode"}, 200, newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.StartupMode), true);
            cmbStart.SelectionChangeCommitted += (sender, e) =>
            {
                newAppSettings.GetClientSettings().StartupMode = ((ComboBox)sender).Text;
            };
            //cmbStart.SelectionChangeCommitted += (sender, e) => cmbStartUpMode_SelectionChangeCommitted(sender, e);

            //Button btnAttended = CreateButton("btnLunchAttended", "Launch Attended Mode", 240, true);
            //btnAttended.Click += (sender, e) => btnLaunchAttendedMode_Click(sender, e);
            //btnAttended.Click += (sender, e) =>
            //{
            //    if (MessageBox.Show("Close Settings form to launch Attended Mode.\nIf you have changed the settings, click the 'OK' button to save the changes.\nLaunch Attended Mode now ?", "Settings", MessageBoxButtons.YesNo) == DialogResult.Yes)
            //    {
            //        scriptBuilderForm.ShowAttendedModeFormProcess();
            //        this.Close();
            //    }
            //};
            CreateButton("btnLunchAttended", "Launch Attended Mode", 240, new Action<object, EventArgs>((sender, e) =>
            {
                if (MessageBox.Show("Close Settings form to launch Attended Mode.\nIf you have changed the settings, click the 'OK' button to save the changes.\nLaunch Attended Mode now ?", "Settings", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    scriptBuilderForm.ShowAttendedModeFormProcess();
                    this.Close();
                }
            }), true);
        }

        private void showApplicationFolderSettings()
        {
            RemoveSettingControls();

            CreateLabel("lblTitle", "Folder", FontSize.Large, true);
            
            CreateLabel("lblRootFolder", "taskt Root Folder", FontSize.Small, true);
            //TextBox txtAppFolder = CreateTextBox("txtAppFolderPath", 440, newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.RootFolder), false);
            CreateTextBox("txtAppFolderPath", 440, newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.RootFolder), false);
            CreateButton("btnSelectRootFolder", "...", 42, new Action<object, EventArgs>((sender, e) =>
            {
                string currentFolerPath = newAppSettings.ClientSettings.RootFolder;

                // prompt user to confirm they want to select a new folder
                var updateFolderRequest =
                    MessageBox.Show(
                        "Would you like to change the default root folder that taskt uses to store tasks and information? \r\n\r\n" +
                        $"Current Root Folder: {currentFolerPath}",
                        "Change Default Root Folder",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                // if user does not want to update folder then exit
                if (updateFolderRequest == DialogResult.No)
                {
                    return;
                }

                // user folder browser to let user select top level folder
                using (var fbd = new FolderBrowserDialog())
                {
                    // check if user selected a folder
                    if (fbd.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        // create references to old and new root folders
                        var oldRootFolder = currentFolerPath;
                        var newRootFolder = Path.Combine(fbd.SelectedPath, "taskt");

                        // ask user to confirm
                        var confirmNewFolderSelection =
                            MessageBox.Show(
                                "Please confirm the changes below:\r\n\r\n" +
                                $"Old Root Folder: {oldRootFolder}\r\n\r\n" +
                                "New Root Folder: " + newRootFolder,
                                "Change Default Root Folder",
                                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                        // handle if user decides to cancel
                        if (confirmNewFolderSelection == DialogResult.Cancel)
                        {
                            return;
                        }

                        // ask if we should migrate the data
                        var migrateCopyData =
                            MessageBox.Show(
                                "Would you like to attempt to move the data from the old folder to the new folder?  Please note, depending on how many files you have, this could take a few minutes.",
                                "Migrate Data?",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        // check if user wants to migrate data
                        if (migrateCopyData == DialogResult.Yes)
                        {
                            try
                            {
                                // find and copy files
                                foreach (string dirPath in Directory.GetDirectories(oldRootFolder, "*", SearchOption.AllDirectories))
                                {
                                    Directory.CreateDirectory(dirPath.Replace(oldRootFolder, newRootFolder));
                                }
                                foreach (string newPath in Directory.GetFiles(oldRootFolder, "*.*", SearchOption.AllDirectories))
                                {
                                    File.Copy(newPath, newPath.Replace(oldRootFolder, newRootFolder), true);
                                }

                                MessageBox.Show("Data Migration Complete", "Data Migration Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            }
                            catch (Exception ex)
                            {
                                // handle any unexpected errors
                                MessageBox.Show($"An Error Occured during Data Migration Copy: {ex}");
                            }
                        }

                        // update textbox which will be updated once user selects "Ok"
                        newAppSettings.GetClientSettings().RootFolder = newRootFolder;
                    }
                }
            }), true);
            //Button rootButton = CreateButton("btnSelectRootFolder", "...", 42, true);
            //rootButton.Click += (sender, e) => btnSelectRootFolder_Click(sender, e, txtAppFolder);

            CreateLabel("lblTaskFolder", "Attended Tasks Folder (Default Folder for saving Script Files)", FontSize.Small, true);
            var txtTasksFolder = CreateTextBox("txtAttendedTasksFolder", 440, newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.AttendedTasksFolder), false);
            CreateButton("btnSelectTasktFolder", "...", 42, new Action<object, EventArgs>((sender, e) =>
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    if (fbd.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        var checkPaths = new List<(string, string)>
                    {
                        (Folders.GetAutoSaveFolderPath(), Folders.AUTOSAVE_FOLDER_NAME),
                        (Folders.GetRunWithoutSavingFolderPath(), Folders.RUN_WITHOUT_SAVING_FOLDER_NAME),
                        (Folders.GetBeforeConvertedFolderPath(), Folders.BEFORE_CONVERTED_FOLDER_NAME),
                        (Folders.GetResourcesFolderPath(), Folders.RESOURCES_FOLDER_NAME),
                        (Folders.GetSamplesFolderPath(), Folders.SAMPLES_FOLDER_NAME),
                        (Folders.GetUpdateWorkingFolderPath(), Folders.UPDATE_FOLDER_NAME),
                    };

                        var newAttendedTaskFolder = Path.Combine(fbd.SelectedPath);

                        var newFullPath = Path.GetFullPath(newAttendedTaskFolder);

                        foreach ((var path, var folderName) in checkPaths)
                        {
                            if (newFullPath == path)
                            {
                                MessageBox.Show($"Selected folder is in the same location as the '{folderName}' folder");
                            }
                        }

                        txtTasksFolder.Text = newAttendedTaskFolder;
                    }
                }
            }), true);
            //Button tasksFolder = CreateButton("btnSelectTasksFolder", "...", 42, true);
            //tasksFolder.Click += (sender, e) => btnSelectAttendedTaskFolder_Click(sender, e, txtTasksFolder);
        }
        private void showApplicationDebugSettings()
        {
            RemoveSettingControls();

            CreateLabel("lblTitle", "Debug", FontSize.Large, true);

            CreateCheckBox("chkShowDebug", "Show Debug Window when Script Execute", newAppSettings.EngineSettings, nameof(newAppSettings.EngineSettings.ShowDebugWindow), true);
            CreateCheckBox("chkAutoCloseDebugWindow", "Automatically Close Debug Window", newAppSettings.EngineSettings, nameof(newAppSettings.EngineSettings.AutoCloseDebugWindow), true);
            CreateCheckBox("chkShowAdvancedDebug", "Show Advanced Debug Logs During Execution", newAppSettings.EngineSettings, nameof(newAppSettings.EngineSettings.ShowAdvancedDebugOutput), true);
        }
        private void showApplicationMetricSettings()
        {
            RemoveSettingControls();

            CreateLabel("lblTitle", "Script Metric", FontSize.Large, true);

            CreateCheckBox("chkTrackMetrics", "Track Execution Metrics", newAppSettings.EngineSettings, nameof(newAppSettings.EngineSettings.TrackExecutionMetrics), true);

            CreateLabel("lblTitleMetrics", "Script Execution Metrics (Last 10 per Script)", FontSize.Small, true);
            var lblMetrics = CreateLabel("lblMetrics", "Getting Metrics...", FontSize.Normal, true);

            var tvExecutionTimes = new TreeView
            {
                Name = "tvExecutionTimes",
                Size = new Size(500, 120),
                Font = new Font("Segoe UI", 12)
            };
            flowLayoutSettings.Controls.Add(tvExecutionTimes);

            //btnClearMetrics = CreateButton("btnClearMetrics", "Clear Metrics", 200, true);
            //btnClearMetrics.Click += (sender, e) => btnClearMetrics_Click(sender, e);
            var btnClearMetrics = CreateButton("btnClearMetrics", "Clear Metrics", 200, new Action<object, EventArgs>((sender, e) =>
            {
                new Metrics().ClearExecutionMetrics();
                bgwMetrics.RunWorkerAsync();
            }), true);

            // set bgwMetrics events
            bgwMetricsDoWorkEvent = (sender, e) =>
            {
                e.Result = new Metrics().ExecutionMetricsSummary();
            };
            bgwMetricsRunWorkerCompletedEvent = (sender, e) =>
            {
                //if ((lblMetrics == null) || (tvExecutionTimes == null) || (btnClearMetrics == null))
                //{
                //    return;
                //}

                if (e.Error != null)
                {
                    if (e.Error is FileNotFoundException)
                    {
                        lblMetrics.Text = "Metrics Unavailable - Metrics are only available after running tasks which will generate metrics logs";
                    }
                    else
                    {
                        lblMetrics.Text = $"Metrics Unavailable: {e.Error}";
                    }
                }
                else
                {
                    var metricsSummary = (List<ExecutionMetric>)(e.Result);

                    if (metricsSummary.Count == 0)
                    {
                        lblMetrics.Text = "No Metrics Found";
                        lblMetrics.Show();
                        tvExecutionTimes.Hide();
                        btnClearMetrics.Hide();
                    }
                    else
                    {
                        lblMetrics.Hide();
                        tvExecutionTimes.Show();
                        btnClearMetrics.Show();
                    }

                    foreach (var metric in metricsSummary)
                    {
                        var rootNode = new TreeNode
                        {
                            Text = $"{metric.FileName} [{metric.AverageExecutionTime} avg.]"
                        };

                        foreach (var metricItem in metric.ExecutionData)
                        {
                            var subNode = new TreeNode
                            {
                                Text = $" - {metricItem.LoggedOn.ToString("MM/dd/yy hh:mm")} {metricItem.ExecutionTime}"
                            };
                            rootNode.Nodes.Add(subNode);
                        }

                        tvExecutionTimes.Nodes.Add(rootNode);
                    }
                }
            };
            bgwMetrics.DoWork += bgwMetricsDoWorkEvent;
            bgwMetrics.RunWorkerCompleted += bgwMetricsRunWorkerCompletedEvent;

            //get metrics
            bgwMetrics.RunWorkerAsync();
        }
        private void showApplicationOtherSettings()
        {
            RemoveSettingControls();

            CreateLabel("lblTitle", "Other", FontSize.Large, true);

            CreateCheckBox("chkMinimizeToTary", "Minimize to System Tray", newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.MinimizeToTray), true);

            CreateLabel("lblResourceTitle", "Resources Folder", FontSize.NormalBold, true);
            CreateLabel("lblResource", "If you want to update the WebDriver (chromedriver.exe etc),\nopen the Resources folder and overwrite the file.", FontSize.Small, true);

            //Button btnRes = CreateButton("btnOpenResources", "Open 'Resources' Folder", 280, true);
            //btnRes.Click += (sender, e) => btnShowRecoures_Click(sender, e);

            CreateButton("btnOpenResources", "Open 'Resources' Folder", 280, new Action<object, EventArgs>((sender, e) =>
            {
                var myAssembly = System.Reflection.Assembly.GetEntryAssembly();
                var path = Path.Combine(Path.GetDirectoryName(myAssembly.Location), "Resources");
                System.Diagnostics.Process.Start(path);
            }), true);

            CreateLabel("lblWebDriverTitle", "Check WebDrivers", FontSize.Normal, true);            

            // get WebDrivers version
            var versions = GetWebDriverVersions();

            CreateLabel("lblChromeDriver", "Chrome Driver Version Result: " + versions["chrome"], FontSize.Small, true);

            //Button btnChrome = CreateButton("btnChrome", "Chrome Driver", 280, true);
            //btnChrome.Click += (sender, e) => btnChromeDriver_Click(sender, e);
            CreateButton("btnChrome", "Chrome Driver", 280, new Action<object, EventArgs>((sender, e) =>
            {
                System.Diagnostics.Process.Start(MyURLs.ChromeDriverURL);
            }), true);

            CreateLabel("lblEdgeDriver", "Edge Driver Version Result: " + versions["edge"], FontSize.Small, true);
            //Button btnEdge = CreateButton("btnEdge", "Edge Driver", 280, true);
            //btnEdge.Click += (sender, e) => btnEdgeDriver_Click(sender, e);
            CreateButton("btnEdge", "Edge Driver", 280, new Action<object, EventArgs>((sender, e) =>
            {
                System.Diagnostics.Process.Start(MyURLs.EdgeDriverURL);
            }), true);

            CreateLabel("lblGeckoDriver", "geckodriver Version Result: " + versions["gecko"], FontSize.Small, true);
            //Button btnGecko = CreateButton("btnGecko", "geckodriver (Firefox)", 280, true);
            //btnGecko.Click += (sender, e) => btnGeckoDriver_Click(sender, e);
            CreateButton("btnGecko", "geckodriver (Firefox)", 280, new Action<object, EventArgs>((sender, e) =>
            {
                System.Diagnostics.Process.Start(MyURLs.GeckoDriverURL);
            }), true);

            CreateLabel("lblIEDriver", "IE Driver Version Result: " + versions["ie"], FontSize.Small, true);
            //Button btnIE = CreateButton("btnIE", "IE Driver", 280, true);
            //btnIE.Click += (sender, e) => btnIEDriver_Click(sender, e);
            CreateButton("btnIE", "IE Driver", 280, new Action<object, EventArgs>((sender, e) =>
            {
                System.Diagnostics.Process.Start(MyURLs.IEDriverURL);
            }), true);
        }
        private void showApplicationScriptFileSettings()
        {
            RemoveSettingControls();

            CreateLabel("lblTitle", "Script File", FontSize.Large, true);

            CreateLabel("lblIntermediateTitle", "Intermediate Script File", FontSize.NormalBold, true);
            CreateCheckBox("lblScriptIntermediate", "Export Intermediate Script File", newAppSettings.EngineSettings, nameof(newAppSettings.EngineSettings.ExportIntermediateXML), true);

            CreateLabel("lblAutoSaveTitle", "Auto Save Script File", FontSize.NormalBold, true);
            CreateCheckBox("lblEnabledAutoSave", "Enable Auto Save Script File", newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.EnabledAutoSave), true);
            CreateLabel("lblAutoSaveInterval", "Auto Save Interval");
            CreateTextBox("txtAutoSaveInterval", 40, newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.AutoSaveInterval));
            CreateLabel("lblAutoSaveIntervalMin", "minute(s) [1-120]", FontSize.Normal, true);

            CreateLabel("lblRemoveOldAutoSaveScriptFile", "Delete Auto Saved Script Files that are more than ");
            CreateTextBox("txtRemoveOldAutoSaveScriptFileDays", 40, newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.RemoveAutoSaveFileDays), false);
            CreateLabel("lblRemoveOldAutoSaveScriptFile2", " days old", FontSize.Normal, true);

            //var showAutoSave = CreateButton("btnShowAutoSaveFolder", "Show 'AutoSave' Folder", 250, true);
            //showAutoSave.Click += btnShowAutoSaveFolder_Click;
            CreateButton("btnShowAutoSaveFolder", "Show 'AutoSave' Folder", 250, new Action<object, EventArgs>((sender, e) =>
            {
                System.Diagnostics.Process.Start(Folders.GetAutoSaveFolderPath());
            }), true);

            CreateLabel("lblRunWithoutSavingTitle", "Run Without Saving Script File", FontSize.NormalBold, true);
            CreateLabel("lblRemoveOldRunwoSavingScriptFile", "Delete 'Run without Saving' Script Files that are more than ");
            CreateTextBox("txtRemoveOldRunwoSavingScriptFileDays", 40, newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.RemoveRunWithtoutSavingFileDays), false);
            CreateLabel("lblRemoveOldRunwoSavingScriptFile2", " days old", FontSize.Normal, true);

            //var showRunWithout = CreateButton("btnShowRunWithoutFolder", "Show 'RunWithoutSaving' Folder", 250, true);
            //showRunWithout.Click += btnShowRunWithoutSavingFolder_Click;
            CreateButton("btnShowRunWithoutFolder", "Show 'RunWithoutSaving' Folder", 250, new Action<object, EventArgs>((sender, e) =>
            {
                System.Diagnostics.Process.Start(Folders.GetRunWithoutSavingFolderPath());
            }), true);

            CreateLabel("lblRunBeforeConvertedTitle", "Before Converted Script File", FontSize.NormalBold, true);
            CreateLabel("lblRemoveOldBeforeConvertedScriptFile", "Delete 'Before Converted' Script Files that are more than ");
            CreateTextBox("txtRemoveOldBeforeConvertedScriptFileDays", 40, newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.RemoveBeforeConvertedFileDays), false);
            CreateLabel("lblRemoveOldBeforeConvertedScriptFile2", " days old", FontSize.Normal, true);

            //var showBeforeConverted = CreateButton("btnShowBeforeConvertedFolder", "Show 'BeforeConverted' Folder", 250, true);
            //showBeforeConverted.Click += btnShowBeforeConvertedFolder_Click;
            CreateButton("btnShowBeforeConvertedFolder", "Show 'BeforeConverted' Folder", 250, new Action<object, EventArgs>((sender, e) =>
            {
                System.Diagnostics.Process.Start(Folders.GetBeforeConvertedFolderPath());
            }), true);

            // NOTE: scrollbar trap (why?)
            CreateLabel("lblFooterA", "", FontSize.NormalBold, true);
            CreateLabel("lblFooterB", "", FontSize.NormalBold, true);
        }
        private void showApplicationSettingsFile()
        {
            RemoveSettingControls();

            CreateLabel("lblTitle", "Settings File", FontSize.Large, true);

            CreateLabel("lblImport", "Import Settings", FontSize.NormalBold, true);
            //Button btnImport = CreateButton("btnImport", "Import", 200, true);
            //btnImport.Click += (sender, e) => btnImportSettings_Click(sender, e);
            CreateButton("btnImport", "Import", 200, new Action<object, EventArgs>((sender, e) =>
            {
                using (var frm = new OpenFileDialog())
                {
                    frm.Filter = "taskt Settings (*.xml)|*.xml|All Files(*.*)|*.*";
                    frm.Title = "Import Settings";
                    frm.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            newAppSettings = ApplicationSettings.Open(frm.FileName);
                            MessageBox.Show("Imported", "taskt", MessageBoxButtons.OK);
                        }
                        catch
                        {
                            MessageBox.Show("Fail import", "taskt", MessageBoxButtons.OK);
                        }
                    }
                }
            }), true);

            CreateLabel("lblExport", "Export Settings", FontSize.NormalBold, true);
            //Button btnExport = CreateButton("btnExport", "Export", 200, true);
            //btnExport.Click += (sender, e) => btnExportSettings_Click(sender, e);
            CreateButton("btnExport", "Export", 200, new Action<object, EventArgs>((sender, e) =>
            {
                using (var frm = new SaveFileDialog())
                {
                    frm.Filter = "taskt Settings (*.xml)|*.xml|All Files(*.*)|*.*";
                    frm.Title = "Import Settings";
                    frm.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            newAppSettings.Save(frm.FileName);
                            MessageBox.Show("Exported", "taskt", MessageBoxButtons.OK);
                        }
                        catch
                        {
                            MessageBox.Show("Fail export", "taskt", MessageBoxButtons.OK);
                        }
                    }
                }
            }), true);

            CreateLabel("lblLoadDefault", "Load Default Settings", FontSize.NormalBold, true);
            //Button btnLoadDefault = CreateButton("btnLoadDefault", "Load Default", 200, true);
            //btnLoadDefault.Click += (sender, e) => btnLoadDefaultSettings_Click(sender, e);
            CreateButton("btnLoadDefault", "Load Default", 200, new Action<object, EventArgs>((sender, e) =>
            {
                if (MessageBox.Show("Are you sure to Load Default Settings?", "taskt", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    newAppSettings = new ApplicationSettings();
                    MessageBox.Show("Load Default Settings", "taskt", MessageBoxButtons.OK);
                }
            }), true);
        }

        #endregion

        #region Automation Engine
        private void showAutomationEngineParserSettings()
        {
            RemoveSettingControls();

            CreateLabel("lblTitle", "Parser", FontSize.Large, true);

            CreateLabel("lblStartMarker", "Start Marker:", FontSize.Normal, false);
            var txtStart = CreateTextBox("txtStartMarker", 40, newAppSettings.EngineSettings, nameof(newAppSettings.EngineSettings.VariableStartMarker), false);
            CreateLabel("lblEndMarker", "End Marker:", FontSize.Normal, false);
            var txtEnd = CreateTextBox("txtEndMarker", 40, newAppSettings.EngineSettings, nameof(newAppSettings.EngineSettings.VariableEndMarker), true);
            var lblNotice = CreateLabel("lblMarkerNotice", "If Start Maker and End Marker are the same,\nthe variable may not expand properly.", FontSize.Small, false);
            lblNotice.Padding = new Padding(0, 4, 0, 0);
            var lblExample = CreateLabel("lblVariableExample", newAppSettings.EngineSettings.VariableStartMarker + "VariableName" + newAppSettings.EngineSettings.VariableEndMarker, FontSize.NormalBold, true);

            //txtStart.TextChanged += (sender, e) => VariableMarker_TextChanged(sender, e, txtStart, txtEnd, lblExample);
            //txtEnd.TextChanged += (sender, e) => VariableMarker_TextChanged(sender, e, txtStart, txtEnd, lblExample);

            txtStart.TextChanged += (sender, e) =>
            {
                lblExample.Text = $"{txtStart.Text}VariableName{txtEnd.Text}";
            };
            txtEnd.TextChanged += (sender, e) =>
            {
                lblExample.Text = $"{txtStart.Text}VariableName{txtEnd.Text}";
            };

            CreateCheckBox("chkCalculateAutomatically", "Calculate Automatically", newAppSettings.EngineSettings, nameof(newAppSettings.EngineSettings.AutoCalcVariables), true);

            CreateCheckBox("chkUserNewParser", "Use New Parser (beta)", newAppSettings.EngineSettings, "UseNewParser", true);
            CreateCheckBox("chkIgnoreFirstMarker", "Ignore First Variable Marker In Output Parameter (Check is strongly recommended)", newAppSettings.EngineSettings, nameof(newAppSettings.EngineSettings.IgnoreFirstVariableMarkerInOutputParameter), true);
        }

        //private void showAutomationUIElementInspect()
        //{
        //    removeSettingControls();

        //    createLabel("lblTitle", "UIElement Inspcet (beta)", FontSize.Large, true);

        //    createLabel("lblDepath", "Depath of Nodes: ");
        //    var txtDepath = createTextBox("txtDepath", 120, newAppSettings.EngineSettings, nameof(newAppSettings.EngineSettings.MaxUIElementInpectDepth), true);

        //    createLabel("lblSibling", "Number of Sibling Nodes: ");
        //    var txtSibling = createTextBox("txtSibling", 120, newAppSettings.EngineSettings, nameof(newAppSettings.EngineSettings.MaxUIElementInspectSiblingNodes), true);

        //    txtDepath.Enabled = false;
        //    txtSibling.Enabled = false;
        //}

        private void showAutomationEngineKeywordSettings()
        {
            RemoveSettingControls();

            CreateLabel("lblTitle", "Keyword", FontSize.Large, true);

            CreateLabel("lblAttention", "IMPORTANT", FontSize.NormalBold, true);
            CreateLabel("lblAttentionText", "These keywords are no longer supported.\rPlease use System Variables from now on.", FontSize.Normal, true);

            CreateLabel("lblWindowKeyword", "Window Keyword", FontSize.NormalBold, true);

            CreateLabel("lblCurrentWindow", "Current Window Keyword : Currently Window.CurrentWindowName", FontSize.Small, true);
            CreateTextBox("txtCurrentWindow", 400, newAppSettings.EngineSettings, nameof(newAppSettings.EngineSettings.CurrentWindowKeyword), true);

            CreateLabel("lblExcelKeyword", "Excel Keyword", FontSize.NormalBold, true);
            CreateLabel("lblCurrentSheet", "Current Worksheet Keyword : Currently Excel.CurrentWorksheet", FontSize.Small, true);
            CreateTextBox("txtCurrentSheet", 400, newAppSettings.EngineSettings, nameof(newAppSettings.EngineSettings.CurrentWorksheetKeyword), true);
            CreateLabel("lblNextSheet", "Next Worksheet Keyword : Currently Excel.NextWorksheet", FontSize.Small, true);
            CreateTextBox("txtNextSheet", 400, newAppSettings.EngineSettings, nameof(newAppSettings.EngineSettings.NextWorksheetKeyword), true);
            CreateLabel("lblPreviousSheet", "Previous Worksheet Keyword : Currently Excel.PreviousWorksheet", FontSize.Small, true);
            CreateTextBox("txtPreviousSheet", 400, newAppSettings.EngineSettings, nameof(newAppSettings.EngineSettings.PreviousWorksheetKeyword), true);
        }

        private void showAutomationEngineEngineSettings()
        {
            RemoveSettingControls();

            CreateLabel("lblTitle", "Engine", FontSize.Large, true);

            CreateCheckBox("chkOverrideAppInstance", "Override App Instances", newAppSettings.EngineSettings, nameof(newAppSettings.EngineSettings.OverrideExistingAppInstances), true);

            CreateLabel("lblCommandDelay", "Default delay between executing commands (ms):", FontSize.Normal, false);
            CreateTextBox("txtCommandDelay", 80, newAppSettings.EngineSettings, nameof(newAppSettings.EngineSettings.DelayBetweenCommands), true);

            CreateLabel("lblCancelKey", "End Script Hotkey:", FontSize.Normal, false);
            var cmb =CreateComboBox("cmbCancellationKey", keysList, 240, newAppSettings.EngineSettings, nameof(newAppSettings.EngineSettings.CancellationKey), true);
            //cmb.SelectionChangeCommitted += (sender, e) => cmdCancellationButton_SelectionChangeCommitted(sender, e);
            cmb.SelectionChangeCommitted += (sender, e) =>
            {
                var key = (Keys)Enum.Parse(typeof(Keys), cmb.Text);
                newAppSettings.GetEngineSettings().CancellationKey = key;
            };
        }
        private void showAutomationEngineVariableSettings()
        {
            RemoveSettingControls();

            CreateLabel("lblTitle", "Variable", FontSize.Large, true);

            CreateCheckBox("chkCreateMissingVariable", "Create Missing Variables During Execution", newAppSettings.EngineSettings, nameof(newAppSettings.EngineSettings.CreateMissingVariablesDuringExecution), true);
        }
        private void showAutomationEngineLogSettings()
        {
            RemoveSettingControls();

            CreateLabel("lblTitle", "Log", FontSize.Large, true);

            CreateCheckBox("chkEnableLogging", "Enable Diagnostic Logging", newAppSettings.EngineSettings, nameof(newAppSettings.EngineSettings.EnableDiagnosticLogging), true);
        }
        #endregion

        #region Documents
        private void showDocumentsCommandReferenceSettings()
        {
            RemoveSettingControls();

            CreateLabel("lblTitle", "Command Reference", FontSize.Large, true);

            //var btn = CreateButton("btnCreateCommandRef", "Create Command Reference", 300, true);
            //btn.Click += (sender, e) => btnCreateCommandReference_Click(sender, e);
            CreateButton("btnCreateCommandRef", "Create Command Reference", 300, new Action<object, EventArgs>((sender, e) =>
            {
                var docsRoot = DocumentationGeneration.GenerateMarkdownFiles();
                System.Diagnostics.Process.Start(docsRoot);
            }));
        }
        #endregion

        #region Editor
        private void showEditorMenuBarSettings()
        {
            RemoveSettingControls();

            CreateLabel("lblTitle", "Menu Bar", FontSize.Large, true);

            CreateCheckBox("chkUseSlimBar", "Use Slim Menu Bar (required restart)", newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.UseSlimActionBar), true);
            CreateCheckBox("chkShowCommandSearch", "Show Command Search Box when taskt is started (required restart)", newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.ShowCommandSearchBar), true);
        }
        private void showEditorCommandListSettings()
        {
            RemoveSettingControls();

            CreateLabel("lblTitle", "Command List", FontSize.Large, true);

            CreateLabel("lblGrouping", "Command List", FontSize.NormalBold, true);
            CreateCheckBox("chkGroupBySubgroup", "Gruoping by subgroup (Restart required)", newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.GroupingBySubgroup), true);
            CreateCheckBox("chkSupportIECommands", "Support IE Commands (Restart required)", newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.SupportIECommand), true);

            CreateLabel("lblSearch", "Command Search", FontSize.NormalBold, true);

            CreateCheckBox("chkMakeGroupNameSearchTarget", "Make Group Name a Search Target", newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.SearchTargetGroupName), true);
            CreateCheckBox("chkGreedlyGroupName", "Show All Commands if Group Name Matches", newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.SearchGreedlyGroupName), true);
            CreateCheckBox("chkMakeSubGroupNameSearchTarget", "Make SubGroup Name a Search Target", newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.SearchTargetSubGroupName), true);
            CreateCheckBox("chkGreedlySubGroupName", "Show All Commands if SubGroup Name Matches", newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.SearchGreedlySubGroupName), true);
        }

        private void showEditorGUIInspectToolSettings()
        {
            RemoveSettingControls();
            CreateLabel("lblTitle", "GUI Inspect Tool", FontSize.Large, true);

            CreateLabel("lblSearchSettings", "Search Settings", FontSize.NormalBold, true);
            CreateLabel("lblMaxSiblings", "Max Siblings", FontSize.Normal, false);
            CreateTextBox("txtMaxSiblings", 60, newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.GUIInspectMaxSiblings), true);
            CreateLabel("lblMaxDepth", "Max Depth", FontSize.Normal, false);
            CreateTextBox("txtMaxDepth", 60, newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.GUIInspectMaxDepth), true);
            CreateLabel("lblSearchTime", "Max Wait Time to Search", FontSize.Normal, false);
            CreateTextBox("txtSearchTime", 60, newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.GUIInspectSearchTime), true);
            CreateLabel("lblMouseMoveInterval", "Mouse Cursor Mode Interval (ms). Specify 500 or more", FontSize.Normal, false);
            CreateTextBox("txtMouseMoveInterval", 60, newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.GUIInspectMouseInterval), true);
        }

        private void showEditorInstanceSettings()
        {
            RemoveSettingControls();

            CreateLabel("lblTitle", "Instance", FontSize.Large, true);

            CreateLabel("lblSortHeader", "Instance Sort", FontSize.NormalBold, true);
            CreateLabel("lblSortOrder", "Instance Name Sort Order:", FontSize.Normal, false);
            var cmbSort = CreateComboBox("cmbSortOrder", new string[] { "Creation Frequently", "By Name", "Frequency Of Use", "No Sorting" }, 240, newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.InstanceNameOrder), true);
            cmbSort.Text = newAppSettings.ClientSettings.InstanceNameOrder;
            //cmbSort.SelectionChangeCommitted += (sender, e) => cmbInstanceSortOrder_SelectionChangeCommitted(sender, e);
            cmbSort.SelectionChangeCommitted += (sender, e) =>
            {
                newAppSettings.GetClientSettings().InstanceNameOrder = cmbSort.Text;
            };

            CreateLabel("lblDefaultInstance", "Default Instance Name", FontSize.NormalBold, true);
            CreateLabel("lblDefaultDatabase", "Default Database Instance Name", FontSize.Small, true);
            CreateTextBox("txtDefaultDatabase", 400, newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.DefaultDBInstanceName), true);
            CreateLabel("lblDefaultExcel", "Default Excel Instance Name", FontSize.Small, true);
            CreateTextBox("txtDefaultExcel", 400, newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.DefaultExcelInstanceName), true);
            CreateLabel("lblDefaultNLG", "Default NLG Instance Name", FontSize.Small, true);
            CreateTextBox("txtDefaultNLG", 400, newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.DefaultNLGInstanceName), true);
            CreateLabel("lblDefaultStopWatch", "Default StopWatch Instance Name", FontSize.Small, true);
            CreateTextBox("txtDefaultStopWatch", 400, newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.DefaultStopWatchInstanceName), true);
            CreateLabel("lblDefaultWebBrowser", "Default WebBrowser Instance Name", FontSize.Small, true);
            CreateTextBox("txtDefaultWebBrowser", 400, newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.DefaultBrowserInstanceName), true);
            CreateLabel("lblDefaultWord", "Default Word Instance Name", FontSize.Small, true);
            CreateTextBox("txtDefaultWord", 400, newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.DefaultWordInstanceName), true);

            CreateCheckBox("chkDontShowDefaultInstance", "Don't Show Default Instance When Multiple Instance Exists (partial support)", newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.DontShowDefaultInstanceWhenMultipleItemsExists), true);
        }
        private void showEditorInsertCommandSettings()
        {
            RemoveSettingControls();

            CreateLabel("lblTitle", "Insert Command", FontSize.Large, true);

            CreateCheckBox("chkInsertCommandsInline", "New Commands Insert Below Selected Command", newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.InsertCommandsInline), true);
            CreateCheckBox("chkSequenceDragDrop", "Allow Drag and Drop into Sequence Commands", newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.EnableSequenceDragDrop), true);
            CreateCheckBox("chkInsertElse", "Insert Else when BeginIf command inserted", newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.InsertElseAutomatically), true);
            CreateCheckBox("chkInsertCommentIfLoop", "Insert Comment above If, Loop, Try", newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.InsertCommentIfLoopAbove), true);
        }
        private void showEditorMiniMapSettings()
        {
            RemoveSettingControls();

            CreateLabel("lblTitle", "Mini Map", FontSize.Large, true);

            CreateCheckBox("chkShowMiniMap", "Show Script Mini Map (beta)", newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.ShowScriptMiniMap), true);
        }
        private void showEditorIndentSettings()
        {
            RemoveSettingControls();

            CreateLabel("lblTitle", "Indent", FontSize.Large, true);

            CreateCheckBox("chkShowIndentLine", "Show Indent Line", newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.ShowIndentLine), true);
            CreateLabel("lblIndentWidth", "Indent Width (1 to 32):", FontSize.Normal, false);
            CreateTextBox("txtIndentWidth", 60, newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.IndentWidth), true);
        }
        private void showEditorVariableSettings()
        {
            RemoveSettingControls();

            CreateLabel("lblTitle", "Variable", FontSize.Large, true);

            CreateCheckBox("chkInsertVariablePosition", "Insert variable at cursor position(Textbox / Combobox)", newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.InsertVariableAtCursor), true);
        }
        private void showEditorValidationSettings()
        {
            RemoveSettingControls();

            CreateLabel("lblTitle", "Validation", FontSize.Large, true);

            CreateCheckBox("chkSilentValidation", "Don't show Script Command Validation Message", newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.DontShowValidationMessage), true);
        }
        //private void showEditorCommandSearchSettings()
        //{
        //    removeSettingControls();

        //    createLabel("lblTitle", "Command Search", FontSize.Large, true);

        //    createCheckBox("chkShowCommandSearch", "Show Command Search Box when taskt is started (required restart)", newAppSettings.ClientSettings, "ShowCommandSearchBar", true);
        //}
        private void showEditorStatusBarSettings()
        {
            RemoveSettingControls();

            CreateLabel("lblTitle", "Status Bar", FontSize.Large, true);

            CreateCheckBox("chkHideNotifyAutomatically", "Hide Status Bar Automatically (reqired restart)", newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.HideNotifyAutomatically), true);
        }
        private void showEditorCommandEditorSettings()
        {
            RemoveSettingControls();

            CreateLabel("lblTitle", "Command Editor", FontSize.Large, true);

            CreateCheckBox("chkRememberCommandEditorSizeAndPosition", "Remember Command Editor Size and Position", newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.RememberCommandEditorSizeAndPosition), true);
            CreateCheckBox("chkRememberSupplementFormsForCommandEditorPosition", "Remember the Position of the Supplemental Forms for Command Editor", newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.RememberSupplementFormsForCommandEditorPosition), true);
            CreateCheckBox("chkPoliteTextInDescription", "Try Polite Text In Description (beta)", newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.ShowPoliteTextInDescription), true);
            CreateCheckBox("chkShowSampleUsegeInDescription", "Show Sample Usage In Description", newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.ShowSampleUsageInDescription), true);
            CreateCheckBox("chkShowDefaultValueInDescription", "Show Default Value In Description If Optional", newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.ShowDefaultValueInDescription), true);
            CreateCheckBox("chkChangeItemsWheelNotFocused", "Change Items with Mouse Wheel when NOT Forcused", newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.ChangeItemsWithWheelWhenNotForcused), true);
            CreateCheckBox("chkDisplayParameterNumBeforeDescription", "Display Parameter Number Before Description", newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.DisplayNumberBeforeParameterDescription), true);
        }
        #endregion

        #region Network
        private void showNetworkLocalListerSettings()
        {
            RemoveSettingControls();

            CreateLabel("lblTitle", "Local Listener (Beta)", FontSize.Large, true);

            CreateLabel("lbmMessage", "Enable this functionality to allow this computer to accept script execution requests from \nother taskt or REST-capable clients.", FontSize.Small, true);

            CreateCheckBox("chkAutoStartListener", "Start Listening on Startup", newAppSettings.ListenerSettings, nameof(newAppSettings.ListenerSettings.StartListenerOnStartup), true);
            CreateCheckBox("chkEnableListening", "Local Listening Enabled", newAppSettings.ListenerSettings, nameof(newAppSettings.ListenerSettings.LocalListeningEnabled), true);

            CreateLabel("lblListeningPort", "Listening Port:", FontSize.Normal, false);
            var txtListeningPort = CreateTextBox("txtListeningPort", 120, newAppSettings.ListenerSettings, nameof(newAppSettings.ListenerSettings.ListeningPort), true);

            CreateCheckBox("chkRequireListenerKey", "Require Authentication Key", newAppSettings.ListenerSettings, nameof(newAppSettings.ListenerSettings.RequireListenerAuthenticationKey), true);
            CreateLabel("lblAuthenicationKey", "Authentication Key", FontSize.Small, true);
            var txtAuthKey = CreateTextBox("txtAuthenicationKey", 480, newAppSettings.ListenerSettings, nameof(newAppSettings.ListenerSettings.AuthKey), true);
            //Button btnRegenerateAuthKey = CreateButton("btnRegenerateAuthKey", "Regenerate", 140, true);
            //btnRegenerateAuthKey.Click += (sender, e) => btnRegenerateAuthKey_Clicked(sender, e, txtAuthKey);
            CreateButton("btnRegenerateAuthKey", "Regenerate", 140, new Action<object, EventArgs>((sender, e) =>
            {
                newAppSettings.GetLocalListenerSettings().AuthKey = Guid.NewGuid().ToString();
                txtAuthKey.Text = newAppSettings.ListenerSettings.AuthKey;
            }));

            CreateCheckBox("chkEnableWhitelit", "Enable IP Verification (Seperate with comma)", newAppSettings.ListenerSettings, nameof(newAppSettings.ListenerSettings.EnableWhitelist), true);
            var txtWhite = CreateTextBox("txtWhitelist", 480, newAppSettings.ListenerSettings, nameof(newAppSettings.ListenerSettings.IPWhiteList), true);
            txtWhite.Multiline = true;
            txtWhite.ScrollBars = ScrollBars.Vertical;
            txtWhite.Height = 80;

            //btnStartListening = CreateButton("btnStartListening", "Start Listening", 140, false);
            //btnStartListening.Click += (sender, e) => btnStartListening_Click(sender, e, txtListeningPort);
            //var btnStartListening = CreateButton("btnStartListening", "Start Listening", 140, new Action<object, EventArgs>((sender, e) =>
            //{
            //    if (int.TryParse(txtListeningPort.Text, out var portNumber))
            //    {
            //        DisableListenerButtons();
            //        Core.Server.LocalTCPListener.StartListening(portNumber);
            //    }
            //}));
            var btnStartListening = CreateButton("btnStartListening", "Start Listening", 140);
            
            //btnStopListening = CreateButton("btnEndListening", "Stop Listening", 140, true);
            //btnStopListening.Click += (sender, e) => btnStopListening_Click(sender, e);
            //var btnStopListening = CreateButton("btnEndListening", "Stop Listening", 140, new Action<object, EventArgs>((sender, e) =>
            //{
            //    DisableListenerButtons();
            //    Core.Server.LocalTCPListener.StopAutomationListener();
            //}));
            var btnStopListening = CreateButton("btnEndListening", "Stop Listening", 140);

            var lblListeningState = CreateLabel("lblListeningState", "Listening on {}", FontSize.Large, true);

            // set button event
            btnStartListening.Click += (sender, e) =>
            {
                if (int.TryParse(txtListeningPort.Text, out var portNumber))
                {
                    DisableListenerButtons();
                    Core.Server.LocalTCPListener.StartListening(portNumber);
                }
            };
            btnStopListening.Click += (sender, e) =>
            {
                DisableListenerButtons();
                Core.Server.LocalTCPListener.StopAutomationListener();
            };

            // set tcp listener event
            tcpListenerListenStartEvent = (sender, e) =>
            {
                if (this.InvokeRequired)
                {
                    var stoppedDelegate = new AutomationTCPListener_StoppedDelegate(tcpListenerListenStartEvent);
                    Invoke(stoppedDelegate, new object[] { sender, e });
                }
                else
                {
                    SetupListeningUI();
                }
            };
            tcpListenerListenStopEvent = (sender, e) =>
            {
                if (this.InvokeRequired)
                {
                    var startedDelegate = new AutomationTCPListener_StoppedDelegate(tcpListenerListenStopEvent);
                    Invoke(startedDelegate, new object[] { sender, e });
                }
                else
                {
                    SetupListeningUI();
                }
            };

            SetupListeningUI();

            // 
            void DisableListenerButtons()
            {
                //if ((btnStartListening == null) || (btnStopListening == null))
                //{
                //    return;
                //}
                btnStartListening.Enabled = false;
                btnStopListening.Enabled = false;
            }

            void SetupListeningUI()
            {
                //if ((btnStartListening == null) || (btnStopListening == null) || (lblListeningState == null))
                //{
                //    return;
                //}

                if (Core.Server.LocalTCPListener.IsListening)
                {
                    lblListeningState.Text = $"Client is Listening at Endpoint '{Core.Server.LocalTCPListener.GetListeningAddress()}'.";
                    btnStartListening.Enabled = false;
                    btnStopListening.Enabled = true;
                }
                else
                {
                    lblListeningState.Text = $"Client is Not Listening!";
                    btnStartListening.Enabled = true;
                    btnStopListening.Enabled = false;
                }
                lblListeningState.Show();
            }
        }
        private void showNetworkServerSettings()
        {
            RemoveSettingControls();

            CreateLabel("lblTitle", "Server", FontSize.Large, true);

            CreateLabel("lblMessage", "Enable this functionality to connect to a local instance of taskt server for workforce \nmanagement. After testing the connection, the client will be assigned a new GUID \nwhich must be approved by an administrator in the server.", FontSize.Small, true);

            CreateCheckBox("chkServerEnabled", "Server Connection Enabled", newAppSettings.ServerSettings, nameof(newAppSettings.ServerSettings.ServerConnectionEnabled), true);
            CreateCheckBox("chkAutomaticallyConnect", "Check In On Startup", newAppSettings.ServerSettings, nameof(newAppSettings.ServerSettings.ConnectToServerOnStartup), true);

            CreateLabel("lblServerURL", "HTTPS Server URL", FontSize.NormalBold, true);
            CreateLabel("lblServerURLex", "Enter the location of the taskt server (ex. https://localhost:60281", FontSize.Normal, true);
            var txtAddress = CreateTextBox("txtHttpsAddress", 480, newAppSettings.ServerSettings, nameof(newAppSettings.ServerSettings.HTTPServerURL), true);
            //var btnTestConnection = CreateButton("btnTestConnection", "Test Connection", 240, true);
            //btnTestConnection.Click += (sender, e) => btnTestConnection_Click(sender, e, txtAddress);
            CreateButton("btnTestConnection", "Test Connection", 240, new Action<object, EventArgs>((sender, e) =>
            {
                var successfulConnection = Core.Server.HttpServerClient.TestConnection(txtAddress.Text);

                if (successfulConnection)
                {
                    var pulledNewGUID = Core.Server.HttpServerClient.GetGuid();

                    if (pulledNewGUID)
                    {
                        newAppSettings = ApplicationSettings.GetOrCreateApplicationSettings(App.Taskt_Settings_File_Path);

                        txtAddress.Text = newAppSettings.ServerSettings.HTTPGuid.ToString();
                        MessageBox.Show("Connected Successfully!\nGUID will be reloaded automatically the next time settings is loaded!", "Taskt", MessageBoxButtons.OK);
                    }
                    MessageBox.Show("Connected Successfully!", "Taskt", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("Unable To Connect!", "Taskt", MessageBoxButtons.OK);
                }
            }));
            
            CreateLabel("lblClientGUID", "Client GUID", FontSize.NormalBold, true);
            CreateLabel("lblClientGUIDex", "Indicates the GUID the client will use when connecting to taskt server", FontSize.Normal, true);
            CreateTextBox("txtGUID", 480, newAppSettings.ServerSettings, nameof(newAppSettings.ServerSettings.HTTPGuid), true);
            //var btnPublishTask = CreateButton("btnPublishTask", "Publish Task", 240, true);
            //btnPublishTask.Click += (sender, e) => btnPublishTask_Click(sender, e);
            CreateButton("btnPublishTask", "Publish Task", 240, new Action<object, EventArgs>((sender, e) =>
            {
                if (File.Exists(scriptBuilderForm.ScriptFilePath))
                {
                    Core.Server.HttpServerClient.PublishScript(scriptBuilderForm.ScriptFilePath, Core.Server.PublishedScript.PublishType.ServerReference);
                }
                else
                {
                    MessageBox.Show("Please open the task in order to publish it.", "Taskt", MessageBoxButtons.OK);
                }
            }));
            
            var lblSocketState = CreateLabel("lblSocketState", "Socket Status", FontSize.Large, true);
            var lblSocketException = CreateLabel("lblSocketException", "Socket Exception", FontSize.Normal, true);

            // set timer event
            tmrGetScoketStatusTickEvent = (sender, e) =>
            {
                if ((lblSocketState == null) || (lblSocketException == null))
                {
                    return;
                }
                lblSocketState.Text = $"Socket Status: {Core.Server.SocketClient.GetSocketState()}";
                if (Core.Server.SocketClient.connectionException != string.Empty)
                {
                    lblSocketException.Show();
                    lblSocketException.Text = Core.Server.SocketClient.connectionException;
                }
                else
                {
                    lblSocketException.Hide();
                }
            };
            tmrGetSocketStatus.Tick += tmrGetScoketStatusTickEvent;
        }
        #endregion

        #region Update
        private void showUpdateCheckUpdateSettings()
        {
            RemoveSettingControls();

            CreateLabel("lblTitle", "Check Update", FontSize.Large, true);

            //Button btn = CreateButton("btnCheckUpdate", "Check For Updates", 200, true);
            //btn.Click += (sender, e) => btnCheckUpdate_Click(sender, e);

            CreateButton("btnCheckUpdate", "Check For Updates", 200, new Action<object, EventArgs>((sender, e) =>
            {
                Core.Update.ApplicationUpdate.ShowUpdateResultSync(newAppSettings.ClientSettings.SkipBetaVersionUpdate, false);
            }));
            
            CreateCheckBox("chkUpdate", "Check for update at startup", newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.CheckForUpdateAtStartup), true);
            CreateCheckBox("chkSkipBeta", "Skip Beta version", newAppSettings.ClientSettings, nameof(newAppSettings.ClientSettings.SkipBetaVersionUpdate), true);
        }
        #endregion

        #region VM
        private void showVMDisplayManagerSettings()
        {
            RemoveSettingControls();

            CreateLabel("lblTitle", "Display Manager", FontSize.Large, true);

            //Button btn = CreateButton("btnDisplayManager", "Launch Display Manager", 200, true);
            //btn.Click += (sender, e) => btnLaunchDisplayManager_Click(sender, e);

            CreateButton("btnDisplayManager", "Launch Display Manager", 200, new Action<object, EventArgs>((sender, e) =>
            {
                if (MessageBox.Show("Close Settings form to launch Display Manager.\nIf you have changed the settings, click the 'OK' button to save the changes.\nLaunch Display Manager now ?", "Settings", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    var displayManager = new frmDisplayManager();
                    displayManager.Show();
                    this.Close();
                }
            }));
        }
        #endregion

        #region Create Controls

        /// <summary>
        /// remove cotrols for setting value parameters
        /// </summary>
        private void RemoveSettingControls()
        {
            flowLayoutSettings.Controls.Clear();
        }

        /// <summary>
        /// create Label
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <param name="fontSize"></param>
        /// <param name="isBreak"></param>
        /// <returns></returns>
        private Label CreateLabel(string name, string text, FontSize fontSize = FontSize.Normal, bool isBreak = false)
        {
            var lbl = new Label
            {
                Name = name,
                Text = text,

                AutoSize = true
            };

            switch (fontSize)
            {
                case FontSize.Small:
                    lbl.Font = new Font("Segoe UI Semilight", (Single)9.75);
                    lbl.ForeColor = Color.SlateGray;
                    lbl.Height = 16;
                    break;
                case FontSize.Normal:
                    lbl.Font = new Font("Segoe UI Light", 12);
                    lbl.ForeColor = Color.SteelBlue;
                    lbl.Height = 24;
                    break;
                case FontSize.NormalBold:
                    lbl.Font = new Font("Segoe UI Semibold", 12, FontStyle.Bold);
                    lbl.ForeColor = Color.SteelBlue;
                    lbl.Height = 24;
                    break;
                case FontSize.Large:
                    lbl.Font = new Font("Segoe UI Light", (Single)15.75);
                    lbl.ForeColor = Color.SteelBlue;
                    break;
            }

            flowLayoutSettings.Controls.Add(lbl);
            flowLayoutSettings.SetFlowBreak(lbl, isBreak);

            return lbl;
        }

        /// <summary>
        /// create textbox
        /// </summary>
        /// <param name="name"></param>
        /// <param name="width"></param>
        /// <param name="source"></param>
        /// <param name="memberName"></param>
        /// <param name="isBreak"></param>
        /// <returns></returns>
        private TextBox CreateTextBox(string name, int width, object source, string memberName, bool isBreak = false)
        {
            var txt = new TextBox
            {
                Name = name,
                Width = width,
                Height = 29,
                Font = new Font("Segoe UI", 12)
            };

            txt.DataBindings.Add("Text", source, memberName, false, DataSourceUpdateMode.OnPropertyChanged);

            flowLayoutSettings.Controls.Add(txt);
            flowLayoutSettings.SetFlowBreak(txt, isBreak);

            return txt;
        }

        /// <summary>
        /// create checkbox
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <param name="source"></param>
        /// <param name="memberName"></param>
        /// <param name="isBreak"></param>
        /// <returns></returns>
        private CheckBox CreateCheckBox(string name, string text, object source, string memberName, bool isBreak = false)
        {
            var chk = new CheckBox
            {
                Name = name,
                AutoSize = true,
                Text = text,
                Font = new Font("Segoe UI Semilight", (Single)11.25),
                ForeColor = Color.SteelBlue
            };

            chk.DataBindings.Add("Checked", source, memberName, false, DataSourceUpdateMode.OnPropertyChanged);

            flowLayoutSettings.Controls.Add(chk);
            flowLayoutSettings.SetFlowBreak(chk, isBreak);

            return chk;
        }

        /// <summary>
        /// create combobox
        /// </summary>
        /// <param name="name"></param>
        /// <param name="items"></param>
        /// <param name="width"></param>
        /// <param name="source"></param>
        /// <param name="memberName"></param>
        /// <param name="isBreak"></param>
        /// <returns></returns>
        private ComboBox CreateComboBox(string name, string[] items, int width, object source, string memberName, bool isBreak = false)
        {
            var cmb = new ComboBox
            {
                Name = name,
                Font = new Font("Segoe UI", 12),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            cmb.BeginUpdate();
            cmb.Items.AddRange(items);
            cmb.EndUpdate();
            cmb.Width = width;

            cmb.DataBindings.Add("Text", source, memberName, false, DataSourceUpdateMode.OnPropertyChanged);

            flowLayoutSettings.Controls.Add(cmb);
            flowLayoutSettings.SetFlowBreak(cmb, isBreak);

            return cmb;
        }

        /// <summary>
        /// create button
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <param name="width"></param>
        /// <param name="isBreak"></param>
        /// <returns></returns>
        private Button CreateButton(string name, string text, int width, bool isBreak = false)
        {
            var btn = new Button
            {
                Name = name,
                Text = text,
                Width = width,
                Height = 29,
                Font = new Font("Segoe UI", (Single)9.75)
            };

            flowLayoutSettings.Controls.Add(btn);
            flowLayoutSettings.SetFlowBreak(btn, isBreak);

            return btn;
        }

        /// <summary>
        /// create button, attach click event
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <param name="width"></param>
        /// <param name="clickEvent"></param>
        /// <param name="isBreak"></param>
        /// <returns></returns>
        private Button CreateButton(string name, string text, int width, Action<object, EventArgs> clickEvent, bool isBreak = false)
        {
            var btn = CreateButton(name, text, width, isBreak);
            btn.Click += new EventHandler(clickEvent);
            return btn;
        }
        #endregion

        #region StartUp Events
        //private void btnLaunchAttendedMode_Click(object sender, EventArgs e)
        //{
        //    if (MessageBox.Show("Close Settings form to launch Attended Mode.\nIf you have changed the settings, click the 'OK' button to save the changes.\nLaunch Attended Mode now ?", "Settings", MessageBoxButtons.YesNo) == DialogResult.Yes)
        //    {
        //        scriptBuilderForm.ShowAttendedModeFormProcess();
        //        this.Close();
        //    }
        //}
        //private void cmbStartUpMode_SelectionChangeCommitted(object sender, EventArgs e)
        //{
        //    newAppSettings.GetClientSettings().StartupMode = ((ComboBox)sender).Text;
        //}
        #endregion

        #region Folder Events
        //private void btnSelectRootFolder_Click(object sender, EventArgs e, TextBox txt)
        //{
        //    string currentFolerPath = newAppSettings.ClientSettings.RootFolder;

        //    // prompt user to confirm they want to select a new folder
        //    var updateFolderRequest = 
        //        MessageBox.Show(
        //            "Would you like to change the default root folder that taskt uses to store tasks and information? " + Environment.NewLine + Environment.NewLine +
        //            "Current Root Folder: " + currentFolerPath, 
        //            "Change Default Root Folder", 
        //            MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        //    // if user does not want to update folder then exit
        //    if (updateFolderRequest == DialogResult.No)
        //    {
        //        return;
        //    }

        //    // user folder browser to let user select top level folder
        //    using (var fbd = new FolderBrowserDialog())
        //    {
        //        // check if user selected a folder
        //        if (fbd.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
        //        {
        //            // create references to old and new root folders
        //            var oldRootFolder = currentFolerPath;
        //            var newRootFolder = System.IO.Path.Combine(fbd.SelectedPath, "taskt");

        //            // ask user to confirm
        //            var confirmNewFolderSelection = 
        //                MessageBox.Show(
        //                    "Please confirm the changes below:" + Environment.NewLine + Environment.NewLine +
        //                    "Old Root Folder: " + oldRootFolder + Environment.NewLine + Environment.NewLine +
        //                    "New Root Folder: " + newRootFolder, 
        //                    "Change Default Root Folder", 
        //                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

        //            // handle if user decides to cancel
        //            if (confirmNewFolderSelection == DialogResult.Cancel)
        //            {
        //                return;
        //            }

        //            // ask if we should migrate the data
        //            var migrateCopyData = 
        //                MessageBox.Show(
        //                    "Would you like to attempt to move the data from the old folder to the new folder?  Please note, depending on how many files you have, this could take a few minutes.", 
        //                    "Migrate Data?", 
        //                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        //            // check if user wants to migrate data
        //            if (migrateCopyData == DialogResult.Yes)
        //            {
        //                try
        //                {
        //                    // find and copy files
        //                    foreach (string dirPath in System.IO.Directory.GetDirectories(oldRootFolder, "*", System.IO.SearchOption.AllDirectories))
        //                    {
        //                        System.IO.Directory.CreateDirectory(dirPath.Replace(oldRootFolder, newRootFolder));
        //                    }
        //                    foreach (string newPath in System.IO.Directory.GetFiles(oldRootFolder, "*.*", System.IO.SearchOption.AllDirectories))
        //                    {
        //                        System.IO.File.Copy(newPath, newPath.Replace(oldRootFolder, newRootFolder), true);
        //                    }

        //                    MessageBox.Show("Data Migration Complete", "Data Migration Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

        //                }
        //                catch (Exception ex)
        //                {
        //                    // handle any unexpected errors
        //                    MessageBox.Show("An Error Occured during Data Migration Copy: " + ex.ToString());
        //                }
        //            }

        //            // update textbox which will be updated once user selects "Ok"
        //            newAppSettings.GetClientSettings().RootFolder = newRootFolder;
        //        }
        //    }
        //}

        //private void btnSelectAttendedTaskFolder_Click(object sender, EventArgs e, TextBox txt)
        //{
        //    using (var fbd = new FolderBrowserDialog())
        //    {
        //        if (fbd.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
        //        {
        //            var checkPaths = new List<(string, string)>
        //            {
        //                (Folders.GetAutoSaveFolderPath(), Folders.AUTOSAVE_FOLDER_NAME),
        //                (Folders.GetRunWithoutSavingFolderPath(), Folders.RUN_WITHOUT_SAVING_FOLDER_NAME),
        //                (Folders.GetBeforeConvertedFolderPath(), Folders.BEFORE_CONVERTED_FOLDER_NAME),
        //                (Folders.GetResourcesFolderPath(), Folders.RESOURCES_FOLDER_NAME),
        //                (Folders.GetSamplesFolderPath(), Folders.SAMPLES_FOLDER_NAME),
        //                (Folders.GetUpdateWorkingFolderPath(), Folders.UPDATE_FOLDER_NAME),
        //            };

        //            var newAttendedTaskFolder = System.IO.Path.Combine(fbd.SelectedPath);

        //            var newFullPath = System.IO.Path.GetFullPath(newAttendedTaskFolder);
                    
        //            foreach((var path, var folderName) in checkPaths)
        //            {
        //                if (newFullPath == path)
        //                {
        //                    MessageBox.Show($"Selected folder is in the same location as the '{folderName}' folder");
        //                }
        //            }

        //            txt.Text = newAttendedTaskFolder;
        //        }
        //    }
        //}
        #endregion

        #region Parser Events
        //private void VariableMarker_TextChanged(object sender,EventArgs e, TextBox startMarker, TextBox endMaker, Label exampleLabel)
        //{
        //    exampleLabel.Text = $"{startMarker.Text}VariableName{endMaker.Text}";
        //}
        #endregion

        #region Network Events
        //private void btnTestConnection_Click(object sender, EventArgs e, TextBox txtAddress)
        //{
        //    var successfulConnection = Core.Server.HttpServerClient.TestConnection(txtAddress.Text);

        //    if (successfulConnection)
        //    {
        //        var pulledNewGUID = Core.Server.HttpServerClient.GetGuid();

        //        if (pulledNewGUID)
        //        {
        //            newAppSettings = ApplicationSettings.GetOrCreateApplicationSettings(App.Taskt_Settings_File_Path);

        //            txtAddress.Text = newAppSettings.ServerSettings.HTTPGuid.ToString();
        //            MessageBox.Show("Connected Successfully!\nGUID will be reloaded automatically the next time settings is loaded!", "Taskt", MessageBoxButtons.OK);
        //        }
        //        MessageBox.Show("Connected Successfully!", "Taskt", MessageBoxButtons.OK);
        //    }
        //    else
        //    {
        //        MessageBox.Show("Unable To Connect!", "Taskt", MessageBoxButtons.OK);
        //    }
        //}
        //private void btnPublishTask_Click(object sender, EventArgs e)
        //{
        //    if (System.IO.File.Exists(scriptBuilderForm.ScriptFilePath))
        //    {
        //        Core.Server.HttpServerClient.PublishScript(scriptBuilderForm.ScriptFilePath, Core.Server.PublishedScript.PublishType.ServerReference);
        //    }
        //    else
        //    {
        //        MessageBox.Show("Please open the task in order to publish it.", "Taskt", MessageBoxButtons.OK);
        //    }
        //}
        //private void tmrGetSocketStatus_Tick(object sender, EventArgs e)
        //{
        //    if ((lblSocketState == null) || (lblSocketException == null))
        //    {
        //        return;
        //    }
        //    lblSocketState.Text = $"Socket Status: {Core.Server.SocketClient.GetSocketState()}";
        //    if (Core.Server.SocketClient.connectionException != string.Empty)
        //    {
        //        lblSocketException.Show();
        //        lblSocketException.Text = Core.Server.SocketClient.connectionException;
        //    }
        //    else
        //    {
        //        lblSocketException.Hide();
        //    }
        //}
        //private void btnStartListening_Click(object sender, EventArgs e, TextBox txtPort)
        //{
        //    if (int.TryParse(txtPort.Text, out var portNumber))
        //    {
        //        DisableListenerButtons();
        //        Core.Server.LocalTCPListener.StartListening(portNumber);
        //    }
        //}
        //private void btnStopListening_Click(object sender, EventArgs e)
        //{
        //    DisableListenerButtons();
        //    Core.Server.LocalTCPListener.StopAutomationListener();
        //}
        //private void DisableListenerButtons()
        //{
        //    if ((btnStartListening == null) || (btnStopListening == null))
        //    {
        //        return;
        //    }
        //    btnStartListening.Enabled = false;
        //    btnStopListening.Enabled = false;
        //}
        //private void SetupListeningUI()
        //{
        //    if ((btnStartListening == null) || (btnStopListening == null) || (lblListeningState == null))
        //    {
        //        return;
        //    }

        //    if (Core.Server.LocalTCPListener.IsListening)
        //    {
        //        lblListeningState.Text = $"Client is Listening at Endpoint '{Core.Server.LocalTCPListener.GetListeningAddress()}'.";
        //        btnStartListening.Enabled = false;
        //        btnStopListening.Enabled = true;
        //    }
        //    else
        //    {
        //        lblListeningState.Text = $"Client is Not Listening!";
        //        btnStartListening.Enabled = true;
        //        btnStopListening.Enabled = false;
        //    }
        //    lblListeningState.Show();
        //}
        #endregion

        #region LocalListener Events
        public delegate void AutomationTCPListener_StartedDelegate(object sender, EventArgs e);
        public delegate void AutomationTCPListener_StoppedDelegate(object sender, EventArgs e);
        //private void AutomationTCPListener_ListeningStopped(object sender, EventArgs e)
        //{
        //    if (this.InvokeRequired)
        //    {
        //        var stoppedDelegate = new AutomationTCPListener_StoppedDelegate(AutomationTCPListener_ListeningStopped);
        //        Invoke(stoppedDelegate, new object[] { sender, e });
        //    }
        //    else
        //    {
        //        SetupListeningUI();
        //    }
        //}
        //private void AutomationTCPListener_ListeningStarted(object sender, EventArgs e)
        //{
        //    if (this.InvokeRequired)
        //    {
        //        var startedDelegate = new AutomationTCPListener_StoppedDelegate(AutomationTCPListener_ListeningStarted);
        //        Invoke(startedDelegate, new object[] { sender, e });
        //    }
        //    else
        //    {
        //        SetupListeningUI();
        //    }
        //}
        
        //private void btnRegenerateAuthKey_Clicked(object sender, EventArgs e, TextBox txtAuth)
        //{
        //    newAppSettings.GetLocalListenerSettings().AuthKey = Guid.NewGuid().ToString();
        //    txtAuth.Text = newAppSettings.ListenerSettings.AuthKey;
        //}

        #endregion

        #region Metrics Events
        //private void btnClearMetrics_Click(object sender, EventArgs e)
        //{
        //    new Core.Metrics().ClearExecutionMetrics();
        //    bgwMetrics.RunWorkerAsync();
        //}
        //private void bgwMetrics_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    e.Result = new Metrics().ExecutionMetricsSummary();
        //}
        //private void bgwMetrics_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    if ((lblMetrics == null) || (tvExecutionTimes == null) || (btnClearMetrics == null))
        //    {
        //        return;
        //    }

        //    if (e.Error != null)
        //    {
        //        if (e.Error is FileNotFoundException)
        //        {
        //            lblMetrics.Text = "Metrics Unavailable - Metrics are only available after running tasks which will generate metrics logs";
        //        }
        //        else
        //        {
        //            lblMetrics.Text = $"Metrics Unavailable: {e.Error}";
        //        }
        //    }
        //    else
        //    {
        //        var metricsSummary = (List<ExecutionMetric>)(e.Result);

        //        if (metricsSummary.Count == 0)
        //        {
        //            lblMetrics.Text = "No Metrics Found";
        //            lblMetrics.Show();
        //            tvExecutionTimes.Hide();
        //            btnClearMetrics.Hide();
        //        }
        //        else
        //        {
        //            lblMetrics.Hide();
        //            tvExecutionTimes.Show();
        //            btnClearMetrics.Show();
        //        }

        //        foreach (var metric in metricsSummary)
        //        {
        //            var rootNode = new TreeNode
        //            {
        //                Text = $"{metric.FileName} [{metric.AverageExecutionTime} avg.]"
        //            };

        //            foreach (var metricItem in metric.ExecutionData)
        //            {
        //                var subNode = new TreeNode
        //                {
        //                    Text = $" - {metricItem.LoggedOn.ToString("MM/dd/yy hh:mm")} {metricItem.ExecutionTime}"
        //                };
        //                rootNode.Nodes.Add(subNode);
        //            }

        //            tvExecutionTimes.Nodes.Add(rootNode);
        //        }
        //    }
        //}
        #endregion

        #region Update Events
        //private void btnCheckUpdate_Click(object sender, EventArgs e)
        //{
        //    Core.Update.ApplicationUpdate.ShowUpdateResultSync(newAppSettings.ClientSettings.SkipBetaVersionUpdate, false);
        //}
        #endregion

        #region VM Events
        //private void btnLaunchDisplayManager_Click(object sender, EventArgs e)
        //{
        //    if (MessageBox.Show("Close Settings form to launch Display Manager.\nIf you have changed the settings, click the 'OK' button to save the changes.\nLaunch Display Manager now ?", "Settings", MessageBoxButtons.YesNo) == DialogResult.Yes)
        //    {
        //        var displayManager = new frmDisplayManager();
        //        displayManager.Show();
        //        this.Close();
        //    }
        //}
        #endregion

        #region Documents Events
        //private void btnCreateCommandReference_Click(object sender, EventArgs e)
        //{
        //    var docsRoot = DocumentationGeneration.GenerateMarkdownFiles();
        //    System.Diagnostics.Process.Start(docsRoot);
        //}
        #endregion

        #region Engine Events
        //private void cmdCancellationButton_SelectionChangeCommitted(object sender, EventArgs e)
        //{
        //    var key = (Keys)Enum.Parse(typeof(Keys), ((ComboBox)sender).Text);
        //    newAppSettings.GetEngineSettings().CancellationKey = key;
        //}
        #endregion

        #region Editor Events
        //private void cmbInstanceSortOrder_SelectionChangeCommitted(object sender, EventArgs e)
        //{
        //    newAppSettings.GetClientSettings().InstanceNameOrder = ((ComboBox)sender).Text;
        //}
        #endregion

        #region Application Events
        //private void btnShowRecoures_Click(object sender, EventArgs e)
        //{
        //    var myAssembly = System.Reflection.Assembly.GetEntryAssembly();
        //    string path = System.IO.Path.GetDirectoryName(myAssembly.Location) + "\\Resources";
        //    System.Diagnostics.Process.Start(path);
        //}

        //private void btnChromeDriver_Click(object sender, EventArgs e)
        //{
        //    System.Diagnostics.Process.Start(MyURLs.ChromeDriverURL);
        //}

        //private void btnEdgeDriver_Click(object sender, EventArgs e)
        //{
        //    System.Diagnostics.Process.Start(MyURLs.EdgeDriverURL);
        //}

        //private void btnGeckoDriver_Click(object sender, EventArgs e)
        //{
        //    System.Diagnostics.Process.Start(MyURLs.GeckoDriverURL);
        //}

        //private void btnIEDriver_Click(object sender, EventArgs e)
        //{
        //    System.Diagnostics.Process.Start(MyURLs.IEDriverURL);
        //}

        /// <summary>
        /// get webdrivers versions
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, string> GetWebDriverVersions()
        {
            var myAssembly = System.Reflection.Assembly.GetEntryAssembly();
            //string resourcePath = Path.GetDirectoryName(myAssembly.Location) + "\\Resources";
            var resourcePath = Path.Combine(Path.GetDirectoryName(myAssembly.Location), "Resources");

            var p = new System.Diagnostics.Process();
            p.StartInfo.FileName = Environment.GetEnvironmentVariable("ComSpec");
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardInput = false;
            p.StartInfo.CreateNoWindow = true;

            p.StartInfo.Arguments = @"/c """ + resourcePath + "\\ChromeDriver.exe\" -V";
            p.Start();
            string chromeVersion = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            p.StartInfo.Arguments = @"/c """ + resourcePath + "\\msedgedriver.exe\" -V";
            p.Start();
            string edgeVersion = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            p.StartInfo.Arguments = @"/c """ + resourcePath + "\\geckodriver.exe\" -V";
            p.Start();
            string geckoVersion = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            p.StartInfo.Arguments = @"/c """ + resourcePath + "\\IEDriverServer.exe\" -V";
            p.Start();
            string ieVersion = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            p.Close();

            var ret = new Dictionary<string, string>()
            {
                { "chrome", ExtractWebDriverVersion(chromeVersion)},
                { "edge", ExtractWebDriverVersion(edgeVersion)},
                { "gecko", ExtractWebDriverVersion(geckoVersion)},
                { "ie", ExtractWebDriverVersion(ieVersion)},
            };

            return ret;
        }

        /// <summary>
        /// extract webdriver version from version text
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private static string ExtractWebDriverVersion(string v)
        {
            int idx = v.IndexOf('(');
            return v.Substring(0, idx);
        }

        //private void btnImportSettings_Click(object sender, EventArgs e)
        //{
        //    using (var frm = new OpenFileDialog())
        //    {
        //        frm.Filter = "taskt Settings (*.xml)|*.xml|All Files(*.*)|*.*";
        //        frm.Title = "Import Settings";
        //        frm.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        //        if (frm.ShowDialog() == DialogResult.OK)
        //        {
        //            try
        //            {
        //                newAppSettings = ApplicationSettings.Open(frm.FileName);
        //                MessageBox.Show("Imported", "taskt", MessageBoxButtons.OK);
        //            }
        //            catch
        //            {
        //                MessageBox.Show("Fail import", "taskt", MessageBoxButtons.OK);
        //            }
        //        }
        //    }
        //}
        //private void btnExportSettings_Click(object sender, EventArgs e)
        //{
        //    using (var frm = new SaveFileDialog())
        //    {
        //        frm.Filter = "taskt Settings (*.xml)|*.xml|All Files(*.*)|*.*";
        //        frm.Title = "Import Settings";
        //        frm.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        //        if (frm.ShowDialog() == DialogResult.OK)
        //        {
        //            try
        //            {
        //                newAppSettings.Save(frm.FileName);
        //                MessageBox.Show("Exported", "taskt", MessageBoxButtons.OK);
        //            }
        //            catch
        //            {
        //                MessageBox.Show("Fail export", "taskt", MessageBoxButtons.OK);
        //            }
        //        }
        //    }
        //}
        //private void btnLoadDefaultSettings_Click(object sender, EventArgs e)
        //{
        //    if (MessageBox.Show("Are you sure to Load Default Settings?", "taskt", MessageBoxButtons.YesNo) == DialogResult.Yes)
        //    {
        //        newAppSettings = new ApplicationSettings();
        //        MessageBox.Show("Load Default Settings", "taskt", MessageBoxButtons.OK);
        //    }
        //}
        //private void btnShowAutoSaveFolder_Click(object sender, EventArgs e)
        //{
        //    System.Diagnostics.Process.Start(Folders.GetAutoSaveFolderPath());
        //}
        //private void btnShowRunWithoutSavingFolder_Click(object sender, EventArgs e)
        //{
        //    System.Diagnostics.Process.Start(Folders.GetRunWithoutSavingFolderPath());
        //}

        //private void btnShowBeforeConvertedFolder_Click(object sender, EventArgs e)
        //{
        //    System.Diagnostics.Process.Start(Folders.GetBeforeConvertedFolderPath());
        //}
        #endregion
    }
}
