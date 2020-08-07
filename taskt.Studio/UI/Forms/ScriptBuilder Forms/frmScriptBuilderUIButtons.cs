using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using taskt.Commands;
using taskt.Core.Enums;
using taskt.Core.IO;
using taskt.Core.Script;
using taskt.Core.Settings;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Server;
using taskt.UI.CustomControls.CustomUIControls;
using taskt.UI.Forms.Supplement_Forms;

namespace taskt.UI.Forms.ScriptBuilder_Forms
{
    public partial class frmScriptBuilder : Form
    {
        #region UI Buttons
        #region File Actions Tool Strip and Buttons
        private void uiBtnNew_Click(object sender, EventArgs e)
        {
            NewFile();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewFile();
        }

        private void NewFile()
        {
            ScriptFilePath = null;

            string title = $"New Tab {(uiScriptTabControl.TabCount + 1)} *";
            TabPage newTabPage = new TabPage(title);
            newTabPage.Name = title;
            newTabPage.Tag = new ScriptObject(new List<ScriptVariable>(), new List<ScriptElement>());
            newTabPage.ToolTipText = "";
            uiScriptTabControl.Controls.Add(newTabPage);
            newTabPage.Controls.Add(NewLstScriptActions(title));
            newTabPage.Controls.Add(pnlCommandHelper);

            uiScriptTabControl.SelectedTab = newTabPage;

            _selectedTabScriptActions = (UIListView)uiScriptTabControl.SelectedTab.Controls[0];
            _selectedTabScriptActions.Items.Clear();
            HideSearchInfo();

            _scriptVariables = new List<ScriptVariable>();
            //assign ProjectPath variable
            var projectPathVariable = new ScriptVariable
            {
                VariableName = "ProjectPath",
                VariableValue = _scriptProjectPath
            };
            _scriptVariables.Add(projectPathVariable);
            GenerateRecentFiles();
            newTabPage.Controls[0].Hide();
            pnlCommandHelper.Show();
        }

        private void uiBtnOpen_Click(object sender, EventArgs e)
        {
            //show ofd
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = _scriptProjectPath;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Filter = "Json (*.json)|*.json";

            //if user selected file
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //open file
                OpenFile(openFileDialog.FileName);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //show ofd
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = _scriptProjectPath;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Filter = "Json (*.json)|*.json";

            //if user selected file
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //open file
                OpenFile(openFileDialog.FileName);
            }
        }

        public delegate void OpenFileDelegate(string filepath);
        public void OpenFile(string filePath)
        {
            if (InvokeRequired)
            {
                var d = new OpenFileDelegate(OpenFile);
                Invoke(d, new object[] { filePath });
            }
            else
            {
                try
                {
                    //create or switch to TabPage
                    string fileName = Path.GetFileNameWithoutExtension(filePath);
                    var foundTab = uiScriptTabControl.TabPages.Cast<TabPage>().Where(t => t.ToolTipText == filePath)
                                                                              .FirstOrDefault();
                    if (foundTab == null)
                    {
                        TabPage newtabPage = new TabPage(fileName);
                        newtabPage.Name = fileName;
                        newtabPage.ToolTipText = filePath;
                        
                        uiScriptTabControl.TabPages.Add(newtabPage);
                        newtabPage.Controls.Add(NewLstScriptActions(fileName));
                        uiScriptTabControl.SelectedTab = newtabPage;
                    }
                    else
                    {
                        uiScriptTabControl.SelectedTab = foundTab;
                        return;
                    }

                    _selectedTabScriptActions = (UIListView)uiScriptTabControl.SelectedTab.Controls[0];

                    //get deserialized script
                    Script deserializedScript = Script.DeserializeFile(filePath);

                    //check if script is a part of the currently opened project
                    string openScriptProjectName = deserializedScript.ProjectName;

                    if (openScriptProjectName != _scriptProject.ProjectName)
                    {
                        uiScriptTabControl.TabPages.RemoveAt(uiScriptTabControl.TabCount - 1);
                        throw new Exception("Attempted to load a script not part of the currently open project");
                    }

                    //reinitialize
                    _selectedTabScriptActions.Items.Clear();
                    _scriptVariables = new List<ScriptVariable>();
                    _scriptElements = new List<ScriptElement>();

                    if (deserializedScript.Commands.Count == 0)
                    {
                        Notify("Error Parsing File: Commands not found!");
                    }

                    //update file path and reflect in title bar
                    ScriptFilePath = filePath;

                    string scriptFileName = Path.GetFileNameWithoutExtension(ScriptFilePath);
                    _selectedTabScriptActions.Name = $"{scriptFileName}ScriptActions";

                    //assign variables
                    _scriptVariables.AddRange(deserializedScript.Variables);
                    _scriptElements.AddRange(deserializedScript.Elements);
                    uiScriptTabControl.SelectedTab.Tag = new ScriptObject(_scriptVariables, _scriptElements );

                    //update ProjectPath variable
                    var projectPathVariable = _scriptVariables.Where(v => v.VariableName == "ProjectPath").SingleOrDefault();
                    if (projectPathVariable != null)
                        projectPathVariable.VariableValue = _scriptProjectPath;
                    else
                    {
                        projectPathVariable = new ScriptVariable
                        {
                            VariableName = "ProjectPath",
                            VariableValue = _scriptProjectPath
                        };
                        _scriptVariables.Add(projectPathVariable);
                    }

                    //populate commands
                    PopulateExecutionCommands(deserializedScript.Commands);

                    FileInfo scriptFileInfo = new FileInfo(_scriptFilePath);
                    uiScriptTabControl.SelectedTab.Text = scriptFileInfo.Name.Replace(".json", "");

                    //notify
                    Notify("Script Loaded Successfully!");
                }
                catch (Exception ex)
                {
                    //signal an error has happened
                    Notify("An Error Occured: " + ex.Message);
                }
            }           
        }

        private void uiBtnSave_Click(object sender, EventArgs e)
        {
            //clear selected items
            ClearSelectedListViewItems();
            SaveToFile(false);
        }

        private void uiBtnSaveAs_Click(object sender, EventArgs e)
        {
            //clear selected items
            ClearSelectedListViewItems();
            SaveToFile(true);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //clear selected items
            ClearSelectedListViewItems();
            SaveToFile(false);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //clear selected items
            ClearSelectedListViewItems();
            SaveToFile(true);
        }

        private void SaveToFile(bool saveAs)
        {
            if (_selectedTabScriptActions.Items.Count == 0)
            {
                Notify("You must have at least 1 automation command to save.");
                return;
            }

            int beginLoopValidationCount = 0;
            int beginIfValidationCount = 0;
            int tryCatchValidationCount = 0;
            int retryValidationCount = 0;
            int beginSwitchValidationCount = 0;

            foreach (ListViewItem item in _selectedTabScriptActions.Items)
            {
                if ((item.Tag is LoopCollectionCommand) || (item.Tag is LoopContinuouslyCommand) ||
                    (item.Tag is LoopNumberOfTimesCommand) || (item.Tag is BeginLoopCommand) ||
                    (item.Tag is BeginMultiLoopCommand))
                {
                    beginLoopValidationCount++;
                }
                else if (item.Tag is EndLoopCommand)
                {
                    beginLoopValidationCount--;
                }
                else if ((item.Tag is BeginIfCommand) || (item.Tag is BeginMultiIfCommand))
                {
                    beginIfValidationCount++;
                }
                else if (item.Tag is EndIfCommand)
                {
                    beginIfValidationCount--;
                }
                else if (item.Tag is BeginTryCommand)
                {
                    tryCatchValidationCount++;
                }
                else if (item.Tag is EndTryCommand)
                {
                    tryCatchValidationCount--;
                }
                else if (item.Tag is BeginRetryCommand)
                {
                    retryValidationCount++;
                }
                else if (item.Tag is EndRetryCommand)
                {
                    retryValidationCount--;
                }
                else if(item.Tag is BeginSwitchCommand)
                {
                    beginSwitchValidationCount++;
                }
                else if (item.Tag is EndSwitchCommand)
                {
                    beginSwitchValidationCount--;
                }

                //end loop was found first
                if (beginLoopValidationCount < 0)
                {
                    Notify("Please verify the ordering of your loops.");
                    return;
                }

                //end if was found first
                if (beginIfValidationCount < 0)
                {
                    Notify("Please verify the ordering of your ifs.");
                    return;
                }

                if (tryCatchValidationCount < 0)
                {
                    Notify("Please verify the ordering of your try/catch blocks.");
                    return;
                }

                if (retryValidationCount < 0)
                {
                    Notify("Please verify the ordering of your retry blocks.");
                    return;
                }

                if (beginSwitchValidationCount < 0)
                {
                    Notify("Please verify the ordering of your switch/case blocks.");
                    return;
                }
            }

            //extras were found
            if (beginLoopValidationCount != 0)
            {
                Notify("Please verify the ordering of your loops.");
                return;
            }

            //extras were found
            if (beginIfValidationCount != 0)
            {
                Notify("Please verify the ordering of your ifs.");
                return;
            }

            if (tryCatchValidationCount != 0)
            {
                Notify("Please verify the ordering of your try/catch blocks.");
                return;
            }

            if (retryValidationCount != 0)
            {
                Notify("Please verify the ordering of your retry blocks.");
                return;
            }

            if (beginSwitchValidationCount != 0)
            {
                Notify("Please verify the ordering of your switch/case blocks.");
                return;
            }

            //define default output path
            if (string.IsNullOrEmpty(ScriptFilePath) || (saveAs))
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = _scriptProjectPath;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.Filter = "Json (*.json)|*.json";

                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;

                if (!saveFileDialog.FileName.ToString().Contains(_scriptProjectPath))
                {
                    Notify("An Error Occured: Attempted to save script outside of project directory");
                    return;
                }

                ScriptFilePath = saveFileDialog.FileName;
                string scriptFileName = Path.GetFileNameWithoutExtension(ScriptFilePath);
                if (uiScriptTabControl.SelectedTab.Text != scriptFileName)
                    UpdateTabPage(uiScriptTabControl.SelectedTab, ScriptFilePath);
            }

            //serialize script
            try
            {
                var exportedScript = Script.SerializeScript(_selectedTabScriptActions.Items, _scriptVariables, _scriptElements, ScriptFilePath, _scriptProject.ProjectName);
                _scriptProject.SaveProject(ScriptFilePath, exportedScript, _mainFileName);
                uiScriptTabControl.SelectedTab.Text = uiScriptTabControl.SelectedTab.Text.Replace(" *", "");
                //show success dialog
                Notify("File has been saved successfully!");
            }
            catch (Exception ex)
            {
                Notify("An Error Occured: " + ex.Message);
            }
        }

        private void ClearSelectedListViewItems()
        {
            _selectedTabScriptActions.SelectedItems.Clear();
            _selectedIndex = -1;
            _selectedTabScriptActions.Invalidate();
        }

        private void uiBtnImport_Click(object sender, EventArgs e)
        {
            BeginImportProcess();
        }

        private void importFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BeginImportProcess();
        }

        private void BeginImportProcess()
        {
            //show ofd
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Folders.GetFolder(FolderType.ScriptsFolder);
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Filter = "Json (*.json)|*.json";

            //if user selected file
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //import
                Cursor.Current = Cursors.WaitCursor;
                Import(openFileDialog.FileName);
                Cursor.Current = Cursors.Default;
            }
        }

        private void Import(string filePath)
        {
            try
            {
                //deserialize file
                Script deserializedScript = Script.DeserializeFile(filePath);

                if (deserializedScript.Commands.Count == 0)
                {
                    Notify("Error Parsing File: Commands not found!");
                }

                //variables for comments
                var fileName = new FileInfo(filePath).Name;
                var dateTimeNow = DateTime.Now.ToString();

                //comment
                _selectedTabScriptActions.Items.Add(CreateScriptCommandListViewItem(new AddCodeCommentCommand()
                {
                    v_Comment = "Imported From " + fileName + " @ " + dateTimeNow
                }));

                //import
                PopulateExecutionCommands(deserializedScript.Commands);
                foreach (ScriptVariable var in deserializedScript.Variables)
                {
                    if (_scriptVariables.Find(alreadyExists => alreadyExists.VariableName == var.VariableName) == null)
                    {
                        _scriptVariables.Add(var);
                    }
                }

                foreach (ScriptElement elem in deserializedScript.Elements)
                {
                    if (_scriptElements.Find(alreadyExists => alreadyExists.ElementName == elem.ElementName) == null)
                    {
                        _scriptElements.Add(elem);
                    }
                }

                //comment
                _selectedTabScriptActions.Items.Add(CreateScriptCommandListViewItem(new AddCodeCommentCommand() { v_Comment = "End Import From " + fileName + " @ " + dateTimeNow }));

                //format listview
                //notify
                Notify("Script Imported Successfully!");
            }
            catch (Exception ex)
            {
                //signal an error has happened
                Notify("An Error Occured: " + ex.Message);
            }
        }

        public void PopulateExecutionCommands(List<ScriptAction> commandDetails)
        {

            foreach (ScriptAction item in commandDetails)
            {
                _selectedTabScriptActions.Items.Add(CreateScriptCommandListViewItem(item.ScriptCommand));
                if (item.AdditionalScriptCommands?.Count > 0)
                    PopulateExecutionCommands(item.AdditionalScriptCommands);
            }

            if (pnlCommandHelper.Visible)
            {
                uiScriptTabControl.SelectedTab.Controls.Remove(pnlCommandHelper);
                uiScriptTabControl.SelectedTab.Controls[0].Show();
            }
            else if (!uiScriptTabControl.SelectedTab.Controls[0].Visible)
                uiScriptTabControl.SelectedTab.Controls[0].Show();
        }

        #region Restart And Close Buttons
        private void restartApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
       
        private void uiBtnRestart_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
        private void closeApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void uiBtnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion
        #endregion

        #region Options Tool Strip and Buttons
        private void variablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenVariableManager();
        }

        private void uiBtnAddVariable_Click(object sender, EventArgs e)
        {
            OpenVariableManager();
        }

        private void OpenVariableManager()
        {
            frmScriptVariables scriptVariableEditor = new frmScriptVariables();
            scriptVariableEditor.ScriptName = uiScriptTabControl.SelectedTab.Name;
            scriptVariableEditor.ScriptVariables = _scriptVariables;

            if (scriptVariableEditor.ShowDialog() == DialogResult.OK)
            {
                _scriptVariables = scriptVariableEditor.ScriptVariables;
                if (!uiScriptTabControl.SelectedTab.Text.Contains(" *"))
                    uiScriptTabControl.SelectedTab.Text += " *";
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenSettingsManager();
        }

        private void uiBtnSettings_Click(object sender, EventArgs e)
        {
            OpenSettingsManager();
        }

        private void OpenSettingsManager()
        {
            //show settings dialog
            frmSettings newSettings = new frmSettings(this);
            newSettings.ShowDialog();

            //reload app settings
            _appSettings = new ApplicationSettings();
            _appSettings = _appSettings.GetOrCreateApplicationSettings();

            //reinit
            HttpServerClient.InitializeScriptEngine(new frmScriptEngine());
            HttpServerClient.Initialize();
        }

        private void showSearchBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //set to empty
            tsSearchResult.Text = "";
            tsSearchBox.Text = "";

            //show or hide
            tsSearchBox.Visible = !tsSearchBox.Visible;
            tsSearchButton.Visible = !tsSearchButton.Visible;
            tsSearchResult.Visible = !tsSearchResult.Visible;

            //update verbiage
            if (tsSearchBox.Visible)
            {
                showSearchBarToolStripMenuItem.Text = "Hide Search Bar";
            }
            else
            {
                showSearchBarToolStripMenuItem.Text = "Show Search Bar";
            }
        }

        private void uiBtnClearAll_Click(object sender, EventArgs e)
        {
            HideSearchInfo();
            _selectedTabScriptActions.Items.Clear();
        }

        private void aboutTasktToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout frmAboutForm = new frmAbout();
            frmAboutForm.Show();
        }
        #endregion

        #region Script Events Tool Strip and Buttons
        private void recordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RecordSequence();
        }

        private void uiBtnRecordSequence_Click(object sender, EventArgs e)
        {
            RecordSequence();
        }

        private void RecordSequence()
        {
            Hide();
            frmScreenRecorder sequenceRecorder = new frmScreenRecorder();
            sequenceRecorder.CallBackForm = this;
            sequenceRecorder.ShowDialog();
            uiScriptTabControl.SelectedTab.Controls.Remove(pnlCommandHelper);
            uiScriptTabControl.SelectedTab.Controls[0].Show();

            Show();
            BringToFront();
        }

        private void scheduleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmScheduleManagement scheduleManager = new frmScheduleManagement();
            scheduleManager.Show();
        }

        private void uiBtnScheduleManagement_Click(object sender, EventArgs e)
        {
            frmScheduleManagement scheduleManager = new frmScheduleManagement();
            scheduleManager.Show();
        }

        private void debugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveToolStripMenuItem_Click(null, null);
            _isDebugMode = true;
            RunScript();
        }

        private void uiBtnDebugScript_Click(object sender, EventArgs e)
        {
            debugToolStripMenuItem_Click(sender, e);
        }

        private void RunScript()
        {
            if (_selectedTabScriptActions.Items.Count == 0)
            {
                Notify("You must first build the script by adding commands!");
                return;
            }

            if (ScriptFilePath == null)
            {
                Notify("You must first save your script before you can run it!");
                return;
            }

            TabPage currentTab = uiScriptTabControl.SelectedTab;
            foreach (TabPage openTab in uiScriptTabControl.TabPages)
            {
                if (openTab.Text.Contains(" *"))
                {
                    uiScriptTabControl.SelectedTab = openTab;
                    //clear selected items
                    ClearSelectedListViewItems();
                    SaveToFile(false); // Save & Run!
                }
            }
            uiScriptTabControl.SelectedTab = currentTab;

            Notify("Running Script..");

            //initialize Logger
            switch (_appSettings.EngineSettings.LoggingSinkType)
            {
                case SinkType.File:
                    if (string.IsNullOrEmpty(_appSettings.EngineSettings.LoggingValue1.Trim()))
                        _appSettings.EngineSettings.LoggingValue1 = Path.Combine(Folders.GetFolder(FolderType.LogFolder), "taskt Engine Logs.txt");

                    EngineLogger = new Logging().CreateFileLogger(_appSettings.EngineSettings.LoggingValue1, Serilog.RollingInterval.Day,
                        _appSettings.EngineSettings.MinLogLevel);
                    break;
                case SinkType.HTTP:
                    EngineLogger = new Logging().CreateHTTPLogger(_appSettings.EngineSettings.LoggingValue1, _appSettings.EngineSettings.MinLogLevel);
                    break;
                case SinkType.SignalR:
                    string[] groupNames = _appSettings.EngineSettings.LoggingValue3.Split(',').Select(x => x.Trim()).ToArray();
                    string[] userIDs = _appSettings.EngineSettings.LoggingValue4.Split(',').Select(x => x.Trim()).ToArray();

                    EngineLogger = new Logging().CreateSignalRLogger(_appSettings.EngineSettings.LoggingValue1, _appSettings.EngineSettings.LoggingValue2,
                        groupNames, userIDs, _appSettings.EngineSettings.MinLogLevel);
                    break;
            }

            //initialize Engine
            CurrentEngine = new frmScriptEngine(ScriptFilePath, this, EngineLogger, null, null, false, _isDebugMode);

            //executionManager = new ScriptExectionManager();
            //executionManager.CurrentlyExecuting = true;
            //executionManager.ScriptName = new System.IO.FileInfo(ScriptFilePath).Name;

            CurrentEngine.CallBackForm = this;
            ((frmScriptEngine)CurrentEngine).Show();
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveToolStripMenuItem_Click(null, null);
            _isDebugMode = false;
            RunScript();
        }

        private void uiBtnRunScript_Click(object sender, EventArgs e)
        {
            runToolStripMenuItem_Click(sender, e);
        }
        #endregion

        #region Element Buttons
        private void elementManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenElementManager();
        }

        private void uiBtnAddElement_Click(object sender, EventArgs e)
        {
            OpenElementManager();
        }

        private void OpenElementManager()
        {
            frmScriptElements scriptElementEditor = new frmScriptElements();
            scriptElementEditor.ScriptName = uiScriptTabControl.SelectedTab.Name;
            scriptElementEditor.ScriptElements = _scriptElements;

            if (scriptElementEditor.ShowDialog() == DialogResult.OK)
            {
                CreateUndoSnapshot();
                _scriptElements = scriptElementEditor.ScriptElements;               
            }
        }

        private void elementRecorderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmHTMLElementRecorder elementRecorder = new frmHTMLElementRecorder();
            elementRecorder.ScriptElements = _scriptElements; 
            elementRecorder.ShowDialog();
            CreateUndoSnapshot();
            _scriptElements = elementRecorder.ScriptElements;           
        }

        private void uiBtnElementRecorder_Click(object sender, EventArgs e)
        {
            elementRecorderToolStripMenuItem_Click(sender, e);
        }
        #endregion
        #endregion
    }
}
