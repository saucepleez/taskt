//Copyright (c) 2019 Jason Bayldon
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Commands;
using taskt.Core.Script;

namespace taskt.UI.Forms.ScriptBuilder
{
    /// <summary>
    /// Script Builder
    /// Form tracks the overall configuration and enables script editing, saving, and running
    /// Features ability to add, drag/drop reorder commands
    /// </summary>
    public partial class frmScriptBuilder : Form
    {
        private TreeNode[] bufferedCommandList;
        private ImageList bufferedCommandTreeImages;

        private List<ScriptVariable> scriptVariables;
        private ScriptInformation scriptInfo;
        
        private bool editMode { get; set; }

        private ImageList scriptImages;

        public Core.SafeFrmScriptBuilderApplicationSettings appSettings;

        /// <summary>
        /// undo redo manager
        /// </summary>
        private UndoRedoManager4frmScriptBuilder undoRedo = new UndoRedoManager4frmScriptBuilder();

        private DateTime lastAntiIdleEvent;
        
        private int selectedIndex = -1;
        private int DnDIndex = -1;

        private bool dontSaveFlag = false;

        private Core.InstanceCounter instanceList = null;
        private int[,] miniMap = null;
        private Bitmap miniMapImg = null;

        //List<string> notificationList = new List<string>();
        //private DateTime notificationExpires;
        //private bool isDisplaying;
        //public string notificationText { get; set; }

        /// <summary>
        /// notification manager
        /// </summary>
        private NotificationManager4frmScriptBuilder notificationManager;

        #region CommandEditor form variables
        public CommandEditorState currentScriptEditorMode = CommandEditorState.Normal;
        public CommandEditAction currentEditAction = CommandEditAction.Normal;

        private Size lastEditorSize = new Size { Height = 0, Width = 0 };
        private Point lastEditorPosition;
        #endregion

        #region variables for Child form of CommandEditor form
        private bool isRememberChildCommandEditorPosition = false;
        private Point lastChildCommandEditorPosition;
        #endregion

        // search & replace
        private int currentIndexInMatchItems = -1;
        public int MatchedLines { private set; get; }

        private frmScriptBuilder parentBuilder { get; set; }

        /// <summary>
        /// indent dash line
        /// </summary>
        private Pen indentDashLine;

        /// <summary>
        /// width of 1 charactor in lstScriptAction line column
        /// </summary>
        private int lineCharWidth = 14;

        // forms
        private Supplemental.frmSearchCommands frmSearch = null;
        private AttendedMode.frmAttendedMode frmAttended = null;

        /// <summary>
        /// current open script folder path
        /// </summary>
        private string _scriptFilePath = null;
        /// <summary>
        /// script xml serializer
        /// </summary>
        private XmlSerializer scriptSerializer = null;

        #region properties
        private List<CustomControls.AutomationCommand> automationCommands { get; set; }

        public string ScriptFilePath
        {
            get
            {
                return _scriptFilePath;
            }
            set
            {
                _scriptFilePath = value;
                UpdateWindowTitle();
            }
        }
        #endregion

        #region enums
        public enum CommandEditorState
        {
            Normal,
            Search,
            AdvencedSearch,
            ReplaceSearch,
            HighlightCommand,
        }
        public enum CommandEditAction
        {
            Normal,
            Move
        }
        private enum MiniMapState
        {
            Normal,
            Cursor,
            Matched,
            Comment,
            Error,
            Warning,
            DontSave,
            NewInserted
        }
        #endregion


        private int debugLine;
        public int DebugLine
        {
            get
            {
                return debugLine;
            }
            set
            {
                debugLine = value;
                if (debugLine > 0)
                {
                    try
                    {
                        lstScriptActions.EnsureVisible(debugLine - 1);
                    }
                    catch (Exception)
                    {
                        //log exception?
                    }

                }

                lstScriptActions.Invalidate();
            }
        }



        #region ListView Events

        #region ListView Search

        public int AdvancedSearchItemInCommands(string keyword, bool caseSensitive, bool checkParameters, bool checkCommandName, bool checkComment, bool checkDisplayText, bool checkInstanceType, string instanceType)
        {
            int matchedCount = 0;

            this.currentIndexInMatchItems = -1;
            lstScriptActions.SuspendLayout();
            lstScriptActions.BeginUpdate();
            foreach(ListViewItem itm in lstScriptActions.Items)
            {
                var cmd = (ScriptCommand)itm.Tag;
                if (cmd.CheckMatched(keyword, caseSensitive, checkParameters, checkCommandName, checkComment, checkDisplayText, checkInstanceType, instanceType))
                {
                    matchedCount++;
                }
            }
            lstScriptActions.EndUpdate();
            lstScriptActions.ResumeLayout();

            this.MatchedLines = matchedCount;
            this.currentScriptEditorMode = CommandEditorState.AdvencedSearch;
            lstScriptActions.Invalidate();
            return matchedCount;
        }

        public void MoveMostNearMatchedLine(bool backToTop)
        {
            switch (this.currentScriptEditorMode)
            {
                case CommandEditorState.Normal:
                    return;

                case CommandEditorState.Search:
                case CommandEditorState.AdvencedSearch:
                case CommandEditorState.ReplaceSearch:
                case CommandEditorState.HighlightCommand:
                    break;

                default:
                    return;
            }

            if (this.MatchedLines == 0)
            {
                return;
            }

            int lines = lstScriptActions.Items.Count;

            if (this.currentIndexInMatchItems >= 0)
            {
                if (backToTop)
                {
                    for (int i = 1; i < lines; i++)
                    {
                        int idx = (i + this.currentIndexInMatchItems) % lines;
                        if (((ScriptCommand)lstScriptActions.Items[idx].Tag).IsMatched)
                        {
                            ClearSelectedListViewItems();
                            this.currentIndexInMatchItems = idx;
                            lstScriptActions.Items[idx].Selected = true;
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = this.currentIndexInMatchItems + 1; i < lines; i++)
                    {
                        if (((ScriptCommand)lstScriptActions.Items[i].Tag).IsMatched)
                        {
                            ClearSelectedListViewItems();
                            this.currentIndexInMatchItems = i;
                            lstScriptActions.Items[i].Selected = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < lines; i++)
                {
                    if (((ScriptCommand)lstScriptActions.Items[i].Tag).IsMatched)
                    {
                        ClearSelectedListViewItems();
                        this.currentIndexInMatchItems = i;
                        lstScriptActions.Items[i].Selected = true;
                        break;
                    }
                }
            }

            // scroll
            if (this.currentIndexInMatchItems >= 0)
            {
                lstScriptActions.EnsureVisible(this.currentIndexInMatchItems);
                lstScriptActions.Invalidate();
            }
        }

        private void HighlightAllCurrentSelectedCommand()
        {
            if (lstScriptActions.SelectedIndices.Count > 0)
            {
                string keyword = ((ScriptCommand)lstScriptActions.SelectedItems[0].Tag).SelectionName;
                AdvancedSearchItemInCommands(keyword, false, false, true, false, false, false, "");
                this.currentScriptEditorMode = CommandEditorState.HighlightCommand;
            }
        }

        public int ReplaceSearchInItemCommands(string keyword, bool caseSensitive, string instanceType, bool allProperties, bool instanceName, bool comment)
        {
            int matchedCount = 0;

            this.currentIndexInMatchItems = -1;

            lstScriptActions.SuspendLayout();
            lstScriptActions.BeginUpdate();

            if (allProperties)
            {
                foreach(ListViewItem itm in lstScriptActions.Items)
                {
                    var targetCommand = (ScriptCommand)itm.Tag;
                    if (targetCommand.CheckMatched(keyword, caseSensitive, true, false, false, false, false, ""))
                    {
                        matchedCount++;
                    }
                }
            }
            else if (instanceName)
            {
                foreach (ListViewItem itm in lstScriptActions.Items)
                {
                    var targetCommand = (ScriptCommand)itm.Tag;
                    if (targetCommand.CheckMatched(keyword, caseSensitive, false, false, false, false, true, instanceType))
                    {
                        matchedCount++;
                    }
                }
            }
            else if (comment)
            {
                foreach (ListViewItem itm in lstScriptActions.Items)
                {
                    var targetCommand = (ScriptCommand)itm.Tag;
                    if (targetCommand.CheckMatched(keyword, false, false, false, true, false, false, ""))
                    {
                        matchedCount++;
                    }
                }
            }
            lstScriptActions.EndUpdate();
            lstScriptActions.ResumeLayout();

            this.currentScriptEditorMode = CommandEditorState.ReplaceSearch;
            lstScriptActions.Invalidate();
            return matchedCount;
        }

        public bool ReplaceInItemCommands(string keyword, string replacedText, bool caseSensitive, bool backToTop, string instanceType, bool allProparties, bool instanceName, bool comment)
        {
            int currentIndex = (lstScriptActions.SelectedIndices.Count > 0) ? lstScriptActions.SelectedIndices[0] : 0;
            int rows = lstScriptActions.Items.Count;

            int loopTimes = (backToTop) ? rows : (rows - currentIndex);
            int newIndex = -1;

            lstScriptActions.SuspendLayout();
            lstScriptActions.BeginUpdate();
            if (allProparties)
            {
                for (int i = 0; i < loopTimes; i++)
                {
                    int trgIdx = (i + currentIndex) % rows;
                    var targetCommand = (ScriptCommand)lstScriptActions.Items[trgIdx].Tag;
                    if (targetCommand.Replace(SearchReplaceControls.ReplaceTarget.Parameters, keyword, replacedText, caseSensitive))
                    {
                        newIndex = trgIdx;
                        break;
                    }
                }
            }
            else if (instanceName)
            {
                for (int i = 0; i < loopTimes; i++)
                {
                    int trgIdx = (i + currentIndex) % rows;
                    var targetCommand = (ScriptCommand)lstScriptActions.Items[trgIdx].Tag;
                    if (targetCommand.Replace(SearchReplaceControls.ReplaceTarget.Instance, keyword, replacedText, caseSensitive, instanceType))
                    {
                        newIndex = trgIdx;
                        break;
                    }
                }
            }
            else if (comment)
            {
                for (int i = 0; i < loopTimes; i++)
                {
                    int trgIdx = (i + currentIndex) % rows;
                    var targetCommand = (ScriptCommand)lstScriptActions.Items[trgIdx].Tag;
                    if (targetCommand.Replace(SearchReplaceControls.ReplaceTarget.Comment, keyword, replacedText, caseSensitive))
                    {
                        newIndex = trgIdx;
                        break;
                    }
                }
            }
            lstScriptActions.EndUpdate();
            lstScriptActions.ResumeLayout();

            if (newIndex >= 0)
            {
                ClearSelectedListViewItems();
                lstScriptActions.Items[newIndex].Selected = true;
                lstScriptActions.Invalidate();
                lstScriptActions.EnsureVisible(newIndex);
                return true;
            }
            else
            {
                return false;
            }
        }

        public int ReplaceAllInItemCommands(string keyword, string replacedText, bool caseSensitive, string instanceType, bool allProparties, bool instanceName, bool comment)
        {
            int currentIndex = (lstScriptActions.SelectedIndices.Count > 0) ? lstScriptActions.SelectedIndices[0] : 0;
            int rows = lstScriptActions.Items.Count;

            int replaceCount = 0;

            lstScriptActions.SuspendLayout();
            lstScriptActions.BeginUpdate();
            if (allProparties)
            {
                for (int i = 0; i < rows; i++)
                {
                    int trgIdx = (i + currentIndex) % rows;
                    var targetCommand = (ScriptCommand)lstScriptActions.Items[trgIdx].Tag;
                    if (targetCommand.Replace(SearchReplaceControls.ReplaceTarget.Parameters, keyword, replacedText, caseSensitive))
                    {
                        replaceCount++;
                    }
                }
            }
            else if (instanceName)
            {
                for (int i = 0; i < rows; i++)
                {
                    int trgIdx = (i + currentIndex) % rows;
                    var targetCommand = (ScriptCommand)lstScriptActions.Items[trgIdx].Tag;
                    if (targetCommand.Replace(SearchReplaceControls.ReplaceTarget.Instance, keyword, replacedText, caseSensitive, instanceType))
                    {
                        replaceCount++;
                    }
                }
            }
            else if (comment)
            {
                for (int i = 0; i < rows; i++)
                {
                    int trgIdx = (i + currentIndex) % rows;
                    var targetCommand = (ScriptCommand)lstScriptActions.Items[trgIdx].Tag;
                    if (targetCommand.Replace(SearchReplaceControls.ReplaceTarget.Comment, keyword, replacedText, caseSensitive))
                    {
                        replaceCount++;
                    }
                }
            }
            lstScriptActions.EndUpdate();
            lstScriptActions.ResumeLayout();

            // draw
            if (replaceCount > 0)
            {
                lstScriptActions.Invalidate();
            }

            return replaceCount;
        }
        #endregion


        #region Editor Mode
        private void ApplyEditorFormat()
        {
            editMode = true;
            this.Text = "edit sequence";
            lblMainLogo.Text = "edit sequence";

            lstScriptActions.Invalidate();
            pnlCommandHelper.Hide();

            grpSaveClose.Location = grpFileActions.Location;

            grpRecordRun.Hide();
            grpFileActions.Hide();
            grpVariable.Hide();
            grpSaveClose.Show();

            grpSearch.Left = grpSaveClose.Right + 20;

            moveToParentToolStripMenuItem.Visible = true;
            showScriptInfoMenuItem.Visible = false;
        }
        #endregion

        #endregion


        #region Open, Save, Parse, Import, Validate File
        
        private void AddInstanceName(List<ScriptCommand> items)
        {
            foreach(ScriptCommand command in items)
            {
                command.AddInstance(instanceList);
            }
        }

        private void RemoveInstanceName(List<ScriptCommand> items)
        {
            foreach (ScriptCommand command in items)
            {
                command.RemoveInstance(instanceList);
            }
        }

        #endregion

        #region New script file
        private void BeginNewScriptProcess()
        {
            if (!CheckAndSaveScriptIfForget())
            {
                return;
            }

            NewScript();

            HideSearchInfo();
            GenerateRecentFiles();
            pnlCommandHelper.Show();
        }

        private void NewScript()
        {
            this.ScriptFilePath = null;
            lstScriptActions.Items.Clear();
            scriptVariables = new List<ScriptVariable>();
            scriptInfo = new ScriptInformation();
            instanceList = new Core.InstanceCounter();
            undoRedo = new UndoRedoManager4frmScriptBuilder();
            undoSplitMenuItem.Enabled = false;
            redoSplitMenuItem.Enabled = false;

            ChangeSaveState(false);
        }
        #endregion

        #region misc?

        private void PerformAntiIdle()
        {

            lastAntiIdleEvent = DateTime.Now;
            var mouseMove = new MoveMouseCommand
            {
                v_XMousePosition = (Cursor.Position.X + 1).ToString(),
                v_YMousePosition = (Cursor.Position.Y + 1).ToString()
            };
            Notify("Anti-Idle Triggered");
        }
        #endregion

        #region Create Command Logic
        private void AddNewCommand(string specificCommand = "")
        {
            // DBG
            //MessageBox.Show(specificCommand);

            // bring up new command configuration form
            using (var newCommandForm = new CommandEditor.frmCommandEditor(automationCommands, GetConfiguredCommands(), this.bufferedCommandList, this.bufferedCommandTreeImages))
            {
                newCommandForm.creationMode = CommandEditor.frmCommandEditor.CreationMode.Add;
                newCommandForm.scriptVariables = this.scriptVariables;
                // set taskt settings
                newCommandForm.appSettings = this.appSettings;
                // set instance counter
                newCommandForm.instanceList = this.instanceList;

                if (specificCommand != "")
                {
                    newCommandForm.defaultStartupCommand = specificCommand;
                }

                // set size, position
                if ((lastEditorSize.Width != 0) && (lastEditorSize.Height != 0))
                {
                    newCommandForm.Size = lastEditorSize;
                    newCommandForm.StartPosition = FormStartPosition.Manual;
                    newCommandForm.Location = lastEditorPosition;
                }

                // if a command was selected
                if (newCommandForm.ShowDialog(this) == DialogResult.OK)
                {
                    ChangeSaveState(true);

                    // add to listview
                    var selectedComamnd = (ScriptCommand)newCommandForm.selectedCommand;
                    selectedComamnd.IsDontSavedCommand = true;
                    selectedComamnd.IsNewInsertedCommand = true;
                    AddCommandToListView(selectedComamnd);
                }
            }
        }
        private void BeforeAddNewCommandProcess()
        {
            if (tvCommands.SelectedNode == null)
            {
                return;
            }

            string commandName = GetSelectedCommandFullName();
            if (commandName.Length > 0)
            {
                AddNewCommand(commandName);
            }
        }
        private string GetSelectedCommandFullName()
        {
            return CustomControls.CommandsTreeControls.GetSelectedFullCommandName(tvCommands);
        }
        #endregion

        #region commands process
        
        private void GenerateTreeViewCommands()
        {
            bufferedCommandList = CustomControls.CommandsTreeControls.CreateAllCommandsArray(appSettings.ClientSettings);
            bufferedCommandTreeImages = CustomControls.CommandsTreeControls.CreateCommandImageList();
            tvCommands.ImageList = bufferedCommandTreeImages;

            ShowAllCommands();
        }

        #endregion

        #region Variable Edit, Settings form
       

        private List<string> GetAllVariablesNames()
        {
            var variables = new List<string>();
            variables.AddRange(scriptVariables.Select(f => f.VariableName));
            //variables.AddRange(Core.Common.GenerateSystemVariables().Select(f => f.VariableName));
            variables.AddRange(Core.Automation.Engine.SystemVariables.GetSystemVariablesName());
            return variables;
        }

        private void SetCommandSearchBoxState()
        {
            var state = appSettings.ClientSettings.ShowCommandSearchBar;

            // set to empty
            tsSearchResult.Text = "";
            tsSearchBox.Text = "";

            // show or hide
            tsSearchBox.Visible = state;
            tsSearchButton.Visible = state;
            tsSearchResult.Visible = state;

            // update verbiage
            if (state)
            {
                showSearchBarToolStripMenuItem.Text = "Hide Search Bar";
            }
            else
            {
                showSearchBarToolStripMenuItem.Text = "Show Search Bar";
            }
            showSearchBarToolStripMenuItem.Checked = state;
        }
        #endregion


        #region taskt header icon
        private void lblMainLogo_Click(object sender, EventArgs e)
        {
            ShowAboutForm();
        }
        #endregion

        private bool BeginFormCloseProcess()
        {
            if (frmSearch != null)
            {
                frmSearch = null;
            }

            if (notifyTray != null)
            {
                notifyTray.Visible = false;
                notifyTray.Dispose();
            }

            if (this.parentBuilder != null)
            {
                //TODO: i want to do better.
                return true;
            }

            if (!CheckAndSaveScriptIfForget())
            {
                return false;
            }
            return true;
        }

        #region AttendedMode
        public void ShowAttendedModeFormProcess()
        {
            try
            {
                if (this.frmAttended == null)
                {
                    this.frmAttended = new AttendedMode.frmAttendedMode();
                    this.frmAttended.Show();
                }
                else
                {
                    this.frmAttended.Show();
                }
            }
            catch 
            {
                this.frmAttended = null;
                this.frmAttended = new AttendedMode.frmAttendedMode();
                this.frmAttended.Show();
            }
        }
        #endregion

        #region CommandEditor
        public void SetCommandEditorSizeAndPosition(CommandEditor.frmCommandEditor editor)
        {
            if (editor == null)
            {
                return;
            }

            this.lastEditorSize = editor.Size;
            this.lastEditorPosition = editor.Location;
        }
        #endregion

        #region Child Form of CommandEditor
        public void SetPositionChildFormOfCommandEditor(Form fm)
        {
            if (isRememberChildCommandEditorPosition)
            {
                fm.Location = lastChildCommandEditorPosition;
            }
        }

        public void StorePositionChildFormOfCommandEditor(Form fm)
        {
            this.lastChildCommandEditorPosition = fm.Location;
            isRememberChildCommandEditorPosition = true;
        }
        #endregion

        private void lstScriptActionsContextStrip_Opened(object sender, EventArgs e)
        {
            //if (lstScriptActions.SelectedIndices.Count == 1)
            //{
            //    var idx = lstScriptActions.SelectedIndices[0];
            //    if (idx == 0)
            //    {
            //        moveUpToolStripMenu.Visible = false;
            //        moveDownToolStripMenu.Visible = true;
            //    }
            //    else if (idx == lstScriptActions.Items.Count - 1)
            //    {
            //        moveUpToolStripMenu.Visible = true;
            //        moveDownToolStripMenu.Visible = false;
            //    }
            //    else
            //    {
            //        moveUpToolStripMenu.Visible = true;
            //        moveDownToolStripMenu.Visible = true;
            //    }
            //}
            if (lstScriptActions.SelectedIndices.Count >= 1)
            {
                var rg = lstScriptActions.SelectedIndices[lstScriptActions.SelectedIndices.Count - 1] - lstScriptActions.SelectedIndices[0];
                if (rg + 1 == lstScriptActions.SelectedIndices.Count)
                {
                    moveUpToolStripMenu.Visible = true;
                    moveDownToolStripMenu.Visible = true;
                    if (lstScriptActions.SelectedIndices[0] == 0)
                    {
                        moveUpToolStripMenu.Visible = false;
                    }
                    if (lstScriptActions.SelectedIndices[lstScriptActions.SelectedIndices.Count - 1] == lstScriptActions.Items.Count - 1)
                    {
                        moveDownToolStripMenu.Visible = false;
                    }
                }
                else
                {
                    moveUpToolStripMenu.Visible = false;
                    moveDownToolStripMenu.Visible = false;
                }
            }
            else
            {
                moveUpToolStripMenu.Visible = false;
                moveDownToolStripMenu.Visible = false;
            }
        }

        private void moveUpToolStripMenu_Click(object sender, EventArgs e)
        {
            //if (lstScriptActions.SelectedIndices.Count > 1)
            //{
            //    return;
            //}
            int idx = lstScriptActions.SelectedIndices[0];
            if (idx == 0)
            {
                return;
            }
            //SwapScriptCommandListViewItems(idx, idx - 1);
            MoveScriptCommandListViewItems(idx - 1);
        }

        private void moveDownToolStripMenu_Click(object sender, EventArgs e)
        {
            //if (lstScriptActions.SelectedIndices.Count > 1)
            //{
            //    return;
            //}
            int idx = lstScriptActions.SelectedIndices[lstScriptActions.SelectedIndices.Count - 1];
            if (idx == lstScriptActions.Items.Count - 1)
            {
                return;
            }
            //SwapScriptCommandListViewItems(idx, idx + 1);
            MoveScriptCommandListViewItems(idx + 2);
        }
    }
}
