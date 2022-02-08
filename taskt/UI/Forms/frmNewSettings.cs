using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace taskt.UI.Forms
{
    public partial class frmNewSettings : ThemedForm
    {
        Core.ApplicationSettings newAppSettings;
        frmScriptBuilder scriptBuilderForm;

        private string[] keysList;

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
            newAppSettings = new Core.ApplicationSettings();
            newAppSettings = newAppSettings.GetOrCreateApplicationSettings();

            tvSettingsMenu.ExpandAll();
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

            switch (rootNode.Text + " - " + tvSettingsMenu.SelectedNode.Text)
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

                case "Editor - Command List":
                    showEditorCommandListSettings();
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
                case "Editor - Variable":
                    showEditorVariableSettings();
                    break;

                case "Network - Local Listener":
                    showNetworkLocalListerSettings();
                    break;
                case "Network - Server":
                    break;

                default:
                    break;
            }

            flowLayoutSettings.ResumeLayout();
        }
        #endregion

        #region Application
        private void showApplicationStartUpSetting()
        {
            removeSettingControls();

            createLabel("lblTitie", "Start Up", FontSize.Large, true);
            createCheckBox("chkAntiIdle", "Anti-Idle (while app is open)", newAppSettings.ClientSettings, "AntiIdleWhileOpen", true);
            CheckBox chkPre = createCheckBox("chkPreLoadCommands", "Load Commands at Startup (Reduces Flicker)", newAppSettings.ClientSettings, "PreloadBuilderCommands", true);
            chkPre.Visible = false;
            createLabel("lblStartMode", "Start Mode:", FontSize.Normal, false);
            createComboBox("cmbStartMode", new string[] { "Builder Mode","Attended Task Mode"}, 200, newAppSettings.ClientSettings, "StartupMode", true);

            Button btnAttended = createButton("btnLunchAttended", "Launch Attended Mode", 240, true);
            btnAttended.Click += (sender, e) => btnLaunchAttendedMode_Click(sender, e);
        }

        private void showApplicationFolderSettings()
        {
            removeSettingControls();

            createLabel("lblTitle", "Folder", FontSize.Large, true);
            
            createLabel("lblRootFolder", "taskt Root Folder", FontSize.Small, true);
            TextBox txtAppFolder = createTextBox("txtAppFolderPath", 440, newAppSettings.ClientSettings, "RootFolder", false);
            Button rootButton = createButton("btnSelectRootFolder", "...", 42, true);
            rootButton.Click += (sender, e) => btnSelectRootFolder_Click(sender, e, txtAppFolder);

            createLabel("lblTaskFolder", "Attended Tasks Folder (Default Folder for saving Script Files)", FontSize.Small, true);
            TextBox txtTasksFolder = createTextBox("txtAttendedTasksFolder", 440, newAppSettings.ClientSettings, "AttendedTasksFolder", false);
            Button tasksFolder = createButton("btnSelectTasksFolder", "...", 42, true);
            tasksFolder.Click += (sender, e) => btnSelectAttendedTaskFolder_Click(sender, e, txtTasksFolder);
        }
        private void showApplicationDebugSettings()
        {
            removeSettingControls();

            createLabel("lblTitle", "Debug", FontSize.Large, true);

            createCheckBox("chkShowDebug", "Show Debug Window when Script Execute", newAppSettings.EngineSettings, "ShowDebugWindow", true);
            createCheckBox("chkAutoCloseDebugWindow", "Automatically Close Debug Window", newAppSettings.EngineSettings, "AutoCloseDebugWindow", true);
            createCheckBox("chkShowAdvancedDebug", "Show Advanced Debug Logs During Execution", newAppSettings.EngineSettings, "ShowAdvancedDebugOutput", true);
        }
        private void showApplicationMetricSettings()
        {
            removeSettingControls();

            createLabel("lblTitle", "Script Metric", FontSize.Large, true);

            createCheckBox("chkTrackMetrics", "Track Execution Metrics", newAppSettings.EngineSettings, "TrackExecutionMetrics", true);

            createLabel("lblTitleMetrics", "Script Execution Metrics (Last 10 per Script)", FontSize.Small, true);
            createLabel("lblMetrics", "Getting Metrics...", FontSize.Normal, true);

            createButton("btnClearMetrics", "Clear Metrics", 200, true);
        }
        private void showApplicationOtherSettings()
        {
            removeSettingControls();

            createLabel("lblTitle", "Other", FontSize.Large, true);

            createCheckBox("chkMinimizeToTary", "Minimize to System Tray", newAppSettings.ClientSettings, "MinimizeToTray", true);
        }
        private void showApplicationScriptFileSettings()
        {
            removeSettingControls();

            createLabel("lblTitle", "Script File", FontSize.Large, true);

            createCheckBox("lblScriptIntermediate", "Export Intermediate Script File", newAppSettings.EngineSettings, "ExportIntermediateXML", true);
        }
        #endregion

        #region Automation Engine
        private void showAutomationEngineParserSettings()
        {
            removeSettingControls();

            createLabel("lblTitle", "Parser", FontSize.Large, true);

            createLabel("lblStartMarker", "Start Marker:", FontSize.Normal, false);
            TextBox txtStart = createTextBox("txtStartMarker", 40, newAppSettings.EngineSettings, "VariableStartMarker", false);
            createLabel("lblEndMarker", "End Marker:", FontSize.Normal, false);
            TextBox txtEnd = createTextBox("txtEndMarker", 40, newAppSettings.EngineSettings, "VariableEndMarker", true);
            Label lblNotice = createLabel("lblMarkerNotice", "If Start Maker and End Marker are the same,\nthe variable may not expand properly.", FontSize.Small, false);
            lblNotice.Padding = new Padding(0, 4, 0, 0);
            Label lblExample = createLabel("lblVariableExample", newAppSettings.EngineSettings.VariableStartMarker + "VariableName" + newAppSettings.EngineSettings.VariableEndMarker, FontSize.NormalBold, true);

            txtStart.TextChanged += (sender, e) => VariableMarker_TextChanged(sender, e, txtStart, txtEnd, lblExample);
            txtEnd.TextChanged += (sender, e) => VariableMarker_TextChanged(sender, e, txtStart, txtEnd, lblExample);

            createCheckBox("chkCalculateAutomatically", "Calculate Automatically", newAppSettings.EngineSettings, "AutoCalcVariables", true);

            createCheckBox("chkUserNewParser", "Use New Parser (beta)", newAppSettings.EngineSettings, "UseNewParser", true);
            createCheckBox("chkIgnoreFirstMarker", "Ignore First Variable Marker In Output Parameter (Check is strongly recommended)", newAppSettings.EngineSettings, "IgnoreFirstVariableMarkerInOutputParameter", true);
        }
        private void showAutomationEngineKeywordSettings()
        {
            removeSettingControls();

            createLabel("lblTitle", "Keyword", FontSize.Large, true);

            createLabel("lblWindowKeyword", "Window Keyword", FontSize.NormalBold, true);

            createLabel("lblCurrentWindow", "Current Window Keyword", FontSize.Small, true);
            createTextBox("txtCurrentWindow", 400, newAppSettings.EngineSettings, "CurrentWindowKeyword", true);

            createLabel("lblExcelKeyword", "Excel Keyword", FontSize.NormalBold, true);
            createLabel("lblCurrentSheet", "Current Worksheet Keyword", FontSize.Small, true);
            createTextBox("txtCurrentSheet", 400, newAppSettings.EngineSettings, "CurrentWorksheetKeyword", true);
            createLabel("lblNextSheet", "Next Worksheet Keyword", FontSize.Small, true);
            createTextBox("txtNextSheet", 400, newAppSettings.EngineSettings, "NextWorksheetKeyword", true);
            createLabel("lblPreviousSheet", "Previous Worksheet Keyword", FontSize.Small, true);
            createTextBox("txtPreviousSheet", 400, newAppSettings.EngineSettings, "PreviousWorksheetKeyword", true);
        }

        private void showAutomationEngineEngineSettings()
        {
            removeSettingControls();

            createLabel("lblTitle", "Engine", FontSize.Large, true);

            createCheckBox("chkOverrideAppInstance", "Override App Instances", newAppSettings.EngineSettings, "OverrideExistingAppInstances", true);

            createLabel("lblCommandDelay", "Default delay between executing commands (ms):", FontSize.Normal, false);
            createTextBox("txtCommandDelay", 80, newAppSettings.EngineSettings, "DelayBetweenCommands", true);

            

            createLabel("lblCancelKey", "End Script Hotkey:", FontSize.Normal, false);
            createComboBox("cmbCancellationKey", keysList, 240, newAppSettings.EngineSettings, "CancellationKey", true);
        }
        private void showAutomationEngineVariableSettings()
        {
            removeSettingControls();

            createLabel("lblTitle", "Variable", FontSize.Large, true);

            createCheckBox("chkCreateMissingVariable", "Create Missing Variables During Execution", newAppSettings.EngineSettings, "CreateMissingVariablesDuringExecution", true);
        }
        private void showAutomationEngineLogSettings()
        {
            removeSettingControls();

            createLabel("lblTitle", "Log", FontSize.Large, true);

            createCheckBox("chkEnableLogging", "Enable Diagnostic Logging", newAppSettings.EngineSettings, "EnableDiagnosticLogging", true);
        }
        #endregion

        #region Editor
        private void showEditorMenuBarSettings()
        {
            removeSettingControls();

            createLabel("lblTitle", "Menu Bar", FontSize.Large, true);

            createCheckBox("chkUseSlimBar", "Use Slim Menu Bar (Restart required)", newAppSettings.ClientSettings, "UseSlimActionBar", true);
        }
        private void showEditorCommandListSettings()
        {
            removeSettingControls();

            createLabel("lblTitle", "Command List", FontSize.Large, true);

            createLabel("lblGrouping", "Command List", FontSize.NormalBold, true);
            createCheckBox("chkGroupBySubgroup", "Gruoping by subgroup (Restart required)", newAppSettings.ClientSettings, "GroupingBySubgroup", true);

            createLabel("lblSearch", "Command Search", FontSize.NormalBold, true);

            createCheckBox("chkMakeGroupNameSearchTarget", "Make Group Name a Search Target", newAppSettings.ClientSettings, "SearchTargetGroupName", true);
            createCheckBox("chkGreedlyGroupName", "Show All Commands if Group Name Matches", newAppSettings.ClientSettings, "SearchGreedlyGroupName", true);
            createCheckBox("chkMakeSubGroupNameSearchTarget", "Make SubGroup Name a Search Target", newAppSettings.ClientSettings, "SearchTargetSubGroupName", true);
            createCheckBox("chkGreedlySubGroupName", "Show All Commands if SubGroup Name Matches", newAppSettings.ClientSettings, "SearchGreedlySubGroupName", true);
        }
        private void showEditorInstanceSettings()
        {
            removeSettingControls();

            createLabel("lblTitle", "Instance", FontSize.Large, true);

            createLabel("lblSortHeader", "Instance Sort", FontSize.NormalBold, true);
            createLabel("lblSortOrder", "Instance Name Sort Order:", FontSize.Normal, false);
            createComboBox("cmbSortOrder", new string[] { "Creation Frequently", "By Name", "Frequency Of Use", "No Sorting" }, 240, newAppSettings.ClientSettings, "InstanceNameOrder", true);

            createLabel("lblDefaultInstance", "Default Instance Name", FontSize.NormalBold, true);
            createLabel("lblDefaultDatabase", "Default Database Instance Name", FontSize.Small, true);
            createTextBox("txtDefaultDatabase", 400, newAppSettings.ClientSettings, "DefaultDBInstanceName", true);
            createLabel("lblDefaultExcel", "Default Excel Instance Name", FontSize.Small, true);
            createTextBox("txtDefaultExcel", 400, newAppSettings.ClientSettings, "DefaultExcelInstanceName", true);
            createLabel("lblDefaultNLG", "Default NLG Instance Name", FontSize.Small, true);
            createTextBox("txtDefaultNLG", 400, newAppSettings.ClientSettings, "DefaultNLGInstanceName", true);
            createLabel("lblDefaultStopWatch", "Default StopWatch Instance Name", FontSize.Small, true);
            createTextBox("txtDefaultStopWatch", 400, newAppSettings.ClientSettings, "DefaultStopWatchInstanceName", true);
            createLabel("lblDefaultWebBrowser", "Default WebBrowser Instance Name", FontSize.Small, true);
            createTextBox("txtDefaultWebBrowser", 400, newAppSettings.ClientSettings, "DefaultBrowserInstanceName", true);
            createLabel("lblDefaultWord", "Default Word Instance Name", FontSize.Small, true);
            createTextBox("txtDefaultWord", 400, newAppSettings.ClientSettings, "DefaultWordInstanceName", true);
        }
        private void showEditorInsertCommandSettings()
        {
            removeSettingControls();

            createLabel("lblTitle", "Insert Command", FontSize.Large, true);

            createCheckBox("chkInsertCommandsInline", "New Commands Insert Below Selected Command", newAppSettings.ClientSettings, "InsertCommandsInline", true);
            createCheckBox("chkSequenceDragDrop", "Allow Drag and Drop into Sequence Commands", newAppSettings.ClientSettings, "EnableSequenceDragDrop", true);
            createCheckBox("chkInsertElse", "Insert Else when BeginIf command inserted", newAppSettings.ClientSettings, "InsertElseAutomatically", true);
            createCheckBox("chkInsertCommentIfLoop", "Insert Comment above If, Loop, Try", newAppSettings.ClientSettings, "InsertCommentIfLoopAbove", true);
        }
        private void showEditorMiniMapSettings()
        {
            removeSettingControls();

            createLabel("lblTitle", "Mini Map", FontSize.Large, true);

            createCheckBox("chkShowMiniMap", "Show Script Mini Map (beta)", newAppSettings.ClientSettings, "ShowScriptMiniMap", true);
        }
        private void showEditorIndentSettings()
        {
            removeSettingControls();

            createLabel("lblTitle", "Indent", FontSize.Large, true);

            createCheckBox("chkShowIndentLine", "Show Indent Line", newAppSettings.ClientSettings, "ShowIndentLine", true);
            createLabel("lblIndentWidth", "Indent Width (1 to 32):", FontSize.Normal, false);
            createTextBox("txtIndentWidth", 60, newAppSettings.ClientSettings, "IndentWidth", true);
        }
        private void showEditorVariableSettings()
        {
            removeSettingControls();

            createLabel("lblTitle", "Variable", FontSize.Large, true);

            createCheckBox("chkInsertVariablePosition", "Insert variable at cursor position(Textbox / Combobox)", newAppSettings.ClientSettings, "InsertVariableAtCursor", true);
        }
        #endregion

        #region Network
        private void showNetworkLocalListerSettings()
        {
            removeSettingControls();

            createLabel("lblTitle", "Local Listener (Beta)", FontSize.Large, true);

            createLabel("lbmMessage", "Enable this functionality to allow this computer to accept script execution requests from \nother taskt or REST-capable clients.", FontSize.Small, true);

            createCheckBox("chkAutoStartListener", "Start Listening on Startup", newAppSettings.ListenerSettings, "StartListenerOnStartup", true);
            createCheckBox("chkEnableListening", "Local Listening Enabled", newAppSettings.ListenerSettings, "LocalListeningEnabled", true);

            createLabel("lblListeningPort", "Listening Port:", FontSize.Normal, false);
            createTextBox("txtListeningPort", 120, newAppSettings.ListenerSettings, "ListeningPort", true);

            createCheckBox("chkRequireListenerKey", "Require Authentication Key", newAppSettings.ListenerSettings, "RequireListenerAuthenticationKey", true);
            createLabel("lblAuthenicationKey", "Authentication Key", FontSize.Small, true);
            createTextBox("txtAuthenicationKey", 480, newAppSettings, "ListenerSettings.AuthKey", true);

            createCheckBox("chkEnableWhitelit", "Enable IP Verification (Seperate with comma)", newAppSettings.ListenerSettings, "EnableWhitelist", true);
            TextBox txtWhite = createTextBox("txtWhitelist", 480, newAppSettings.ListenerSettings, "IPWhiteList", true);
            txtWhite.Multiline = true;
            txtWhite.ScrollBars = ScrollBars.Vertical;
            txtWhite.Height = 80;

            Button btnStart = createButton("btnStartListening", "Start Listening", 140, false);
            Button btnEnd = createButton("btnEndListening", "Stop Listening", 140, true);

            Label lblListeningState = createLabel("lblListeningState", "Listening on {}", FontSize.Large, true);
        }
        #endregion

        #region Create Controls
        private void removeSettingControls()
        {
            flowLayoutSettings.Controls.Clear();
        }
        private Label createLabel(string name, string text, FontSize fontSize = FontSize.Normal, bool isBreak = false)
        {
            Label lbl = new Label();
            lbl.Name = name;
            lbl.Text = text;

            lbl.AutoSize = true;

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

        private TextBox createTextBox(string name, int width, object source, string memberName, bool isBreak = false)
        {
            TextBox txt = new TextBox();
            txt.Name = name;
            txt.Width = width;
            txt.Height = 29;
            txt.Font = new Font("Segoe UI", 12);

            txt.DataBindings.Add("Text", source, memberName, false, DataSourceUpdateMode.OnPropertyChanged);

            flowLayoutSettings.Controls.Add(txt);
            flowLayoutSettings.SetFlowBreak(txt, isBreak);

            return txt;
        }

        private CheckBox createCheckBox(string name, string text, object source, string memberName, bool isBreak = false)
        {
            CheckBox chk = new CheckBox();
            chk.Name = name;
            chk.AutoSize = true;
            chk.Text = text;
            chk.Font = new Font("Segoe UI Semilight", (Single)11.25);
            chk.ForeColor = Color.SteelBlue;

            chk.DataBindings.Add("Checked", source, memberName, false, DataSourceUpdateMode.OnPropertyChanged);

            flowLayoutSettings.Controls.Add(chk);
            flowLayoutSettings.SetFlowBreak(chk, isBreak);

            return chk;
        }

        private ComboBox createComboBox(string name, string[] items, int width, object source, string memberName, bool isBreak = false)
        {
            ComboBox cmb = new ComboBox();
            cmb.Name = name;
            cmb.Font = new Font("Segoe UI", 12);
            cmb.DropDownStyle = ComboBoxStyle.DropDownList;

            cmb.BeginUpdate();
            cmb.Items.AddRange(items);
            cmb.EndUpdate();
            cmb.Width = width;

            cmb.DataBindings.Add("Text", source, memberName, false, DataSourceUpdateMode.OnPropertyChanged);

            flowLayoutSettings.Controls.Add(cmb);
            flowLayoutSettings.SetFlowBreak(cmb, isBreak);

            return cmb;
        }

        private Button createButton(string name, string text, int width, bool isBreak = false)
        {
            Button btn = new Button();
            btn.Name = name;
            btn.Text = text;
            btn.Width = width;
            btn.Height = 29;
            btn.Font = new Font("Segoe UI", (Single)9.75);

            flowLayoutSettings.Controls.Add(btn);
            flowLayoutSettings.SetFlowBreak(btn, isBreak);

            return btn;
        }
        #endregion

        #region StartUp Events
        private void btnLaunchAttendedMode_Click(object sender, EventArgs e)
        {
            scriptBuilderForm.showAttendedModeFormProcess();
            this.Close();
        }
        #endregion

        #region Folder Events
        private void btnSelectRootFolder_Click(object sender, EventArgs e, TextBox txt)
        {
            string currentFolerPath = newAppSettings.ClientSettings.RootFolder;

            //prompt user to confirm they want to select a new folder
            var updateFolderRequest = 
                MessageBox.Show(
                    "Would you like to change the default root folder that taskt uses to store tasks and information? " + Environment.NewLine + Environment.NewLine +
                    "Current Root Folder: " + currentFolerPath, 
                    "Change Default Root Folder", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            //if user does not want to update folder then exit
            if (updateFolderRequest == DialogResult.No)
            {
                return;
            }

            //user folder browser to let user select top level folder
            using (var fbd = new FolderBrowserDialog())
            {
                //check if user selected a folder
                if (fbd.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    //create references to old and new root folders
                    var oldRootFolder = currentFolerPath;
                    var newRootFolder = System.IO.Path.Combine(fbd.SelectedPath, "taskt");

                    //ask user to confirm
                    var confirmNewFolderSelection = 
                        MessageBox.Show(
                            "Please confirm the changes below:" + Environment.NewLine + Environment.NewLine +
                            "Old Root Folder: " + oldRootFolder + Environment.NewLine + Environment.NewLine +
                            "New Root Folder: " + newRootFolder, 
                            "Change Default Root Folder", 
                            MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                    //handle if user decides to cancel
                    if (confirmNewFolderSelection == DialogResult.Cancel)
                    {
                        return;
                    }

                    //ask if we should migrate the data
                    var migrateCopyData = 
                        MessageBox.Show(
                            "Would you like to attempt to move the data from the old folder to the new folder?  Please note, depending on how many files you have, this could take a few minutes.", 
                            "Migrate Data?", 
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    //check if user wants to migrate data
                    if (migrateCopyData == DialogResult.Yes)
                    {
                        try
                        {
                            //find and copy files
                            foreach (string dirPath in System.IO.Directory.GetDirectories(oldRootFolder, "*", System.IO.SearchOption.AllDirectories))
                            {
                                System.IO.Directory.CreateDirectory(dirPath.Replace(oldRootFolder, newRootFolder));
                            }
                            foreach (string newPath in System.IO.Directory.GetFiles(oldRootFolder, "*.*", System.IO.SearchOption.AllDirectories))
                            {
                                System.IO.File.Copy(newPath, newPath.Replace(oldRootFolder, newRootFolder), true);
                            }

                            MessageBox.Show("Data Migration Complete", "Data Migration Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                        catch (Exception ex)
                        {
                            //handle any unexpected errors
                            MessageBox.Show("An Error Occured during Data Migration Copy: " + ex.ToString());
                        }
                    }

                    //update textbox which will be updated once user selects "Ok"
                    newAppSettings.ClientSettings.RootFolder = newRootFolder;
                }
            }
        }

        private void btnSelectAttendedTaskFolder_Click(object sender, EventArgs e, TextBox txt)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    var newAttendedTaskFolder = System.IO.Path.Combine(fbd.SelectedPath);
                    txt.Text = newAttendedTaskFolder;
                }
            }
        }
        #endregion

        #region Parser Events
        private void VariableMarker_TextChanged(object sender,EventArgs e, TextBox startMarker, TextBox endMaker, Label exampleLabel)
        {
            exampleLabel.Text = startMarker.Text + "VariableName" + endMaker.Text;
        }
        #endregion
    }
}
