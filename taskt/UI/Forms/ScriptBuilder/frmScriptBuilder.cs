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
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Commands;
using taskt.Core.Script;

namespace taskt.UI.Forms.ScriptBuilder
{
    public partial class frmScriptBuilder : Form
    //Form tracks the overall configuration and enables script editing, saving, and running
    //Features ability to add, drag/drop reorder commands
    {
        private TreeNode[] bufferedCommandList;
        private ImageList bufferedCommandTreeImages;

        private List<ScriptVariable> scriptVariables;
        private ScriptInformation scriptInfo;
        
        private bool editMode { get; set; }

        private ImageList scriptImages;

        public Core.ApplicationSettings appSettings;
        private List<List<ListViewItem>> undoList;
        private DateTime lastAntiIdleEvent;
        private int undoIndex = -1;
        private int selectedIndex = -1;
        private int DnDIndex = -1;

        private bool dontSaveFlag = false;

        private Core.InstanceCounter instanceList = null;
        private int[,] miniMap = null;
        private Bitmap miniMapImg = null;

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

        private Pen indentDashLine;

        // forms
        private Supplemental.frmSearchCommands frmSearch = null;
        private frmAttendedMode frmAttended = null;

        private string _scriptFilePath = null;
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

        //public TreeNode[] treeAllCommands
        //{
        //    get
        //    {
        //        return this.bufferedCommandList;
        //    }
        //}
        //public ImageList treeAllCommandsImage
        //{
        //    get
        //    {
        //        return this.commandTreeImages;
        //    }
        //}
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

        #region Instance and Form Events
        public frmScriptBuilder()
        {
            InitializeComponent();
        }
        public frmScriptBuilder(string filePath)
        {
            InitializeComponent();
            this._scriptFilePath = filePath;
        }

        private void frmScriptBuilder_Load(object sender, EventArgs e)
        {
            //load all commands
            automationCommands = CustomControls.CommandControls.GenerateCommandsandControls();

            // title
            var info = System.Diagnostics.FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            this.Text = info.ProductName;
            lblMainLogo.Text = info.ProductName;

            // init Pen
            indentDashLine = new Pen(Color.LightGray, 1);
            indentDashLine.DashStyle = DashStyle.Dash;

            //set controls double buffered
            foreach (Control control in Controls)
            {
                typeof(Control).InvokeMember("DoubleBuffered",
                    BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                    null, control, new object[] { true });
            }

            //create undo list
            undoList = new List<List<ListViewItem>>();

            //get app settings
            appSettings = new Core.ApplicationSettings();
            appSettings = appSettings.GetOrCreateApplicationSettings();

            if (appSettings.ServerSettings.ServerConnectionEnabled && appSettings.ServerSettings.HTTPGuid == Guid.Empty)
            {              
                Core.Server.HttpServerClient.GetGuid();
            }
            else if (appSettings.ServerSettings.ServerConnectionEnabled && appSettings.ServerSettings.HTTPGuid != Guid.Empty)
            {
                 Core.Server.HttpServerClient.CheckIn();
            }

            Core.Server.HttpServerClient.associatedBuilder = this;
    
            Core.Server.LocalTCPListener.Initialize(this);

            // script serializer
            this.scriptSerializer = Core.Script.Script.CreateSerializer();

            //handle action bar preference
            //hide action panel

            if (this.editMode)
            {
                tlpControls.RowStyles[1].SizeType = SizeType.Absolute;
                tlpControls.RowStyles[1].Height = 0;

                tlpControls.RowStyles[2].SizeType = SizeType.Absolute;
                tlpControls.RowStyles[2].Height = 81;

            }
            else if (appSettings.ClientSettings.UseSlimActionBar)
            {
                tlpControls.RowStyles[2].SizeType = SizeType.Absolute;
                tlpControls.RowStyles[2].Height = 0;
            }
            else
            {
                tlpControls.RowStyles[1].SizeType = SizeType.Absolute;
                tlpControls.RowStyles[1].Height = 0;
            }

            //get scripts folder
            var rpaScriptsFolder = Core.IO.Folders.GetFolder(Core.IO.Folders.FolderType.ScriptsFolder);

            if (!System.IO.Directory.Exists(rpaScriptsFolder))
            {
                using (var userDialog = new UI.Forms.Supplemental.frmDialog("Would you like to create a folder to save your scripts in now? A script folder is required to save scripts generated with this application. The new script folder path would be '" + rpaScriptsFolder + "'.", "Unable to locate Script Folder!", UI.Forms.Supplemental.frmDialog.DialogType.YesNo, 0))
                {
                    if (userDialog.ShowDialog() == DialogResult.OK)
                    {
                        System.IO.Directory.CreateDirectory(rpaScriptsFolder);
                    }
                }
            }

            //get latest files for recent files list on load
            GenerateRecentFiles();

            //show/hide status bar
            if (appSettings.ClientSettings.HideNotifyAutomatically)
            {
                HideNotificationRow();
            }
            else
            {
                ShowNotificationRow();
            }

            //instantiate for script variables
            if (!editMode)
            {
                scriptVariables = new List<ScriptVariable>();
                scriptInfo = new ScriptInformation();
            }

            //instantiate and populate display icons for commands
            scriptImages = Images.UIImageList();

            //set image list
            lstScriptActions.SmallImageList = scriptImages;
            lstScriptActions.Columns[0].Width = 14; // 1digit width
            lstScriptActions.Columns[1].Width = 16; // icon size
            lstScriptActions.Columns[2].Width = lstScriptActions.ClientSize.Width - 30;

            //set listview column size
            frmScriptBuilder_SizeChanged(null, null);

            GenerateTreeViewCommands();


            //start attended mode if selected
            if (appSettings.ClientSettings.StartupMode == "Attended Task Mode")
            {
                this.WindowState = FormWindowState.Minimized;
                showAttendedModeFormProcess();
            }

            this.dontSaveFlag = false;

            // set searchform
            frmSearch = new Supplemental.frmSearchCommands(this);

            // instance count
            instanceList = new Core.InstanceCounter(appSettings);

            // miniMap
            miniMapImg = new Bitmap(8, lstScriptActions.Height);

            // command search box
            setCommandSearchBoxState();

            // release
            GC.Collect();

            if (this._scriptFilePath != null)
            {
                if (this._scriptFilePath.StartsWith("*"))
                {
                    OpenScriptFromFilePath(this.ScriptFilePath.Substring(1), false);
                }
                else
                {
                    OpenScriptFromFilePath(this.ScriptFilePath, true);
                }
            }

            // remove old auto saved files
            RemoveOldAutoSavedFiles();
            RemoveOldRunWithoutSavingScriptFiles();

            // check update
            if ((appSettings.ClientSettings.CheckForUpdateAtStartup) && (this.parentBuilder == null))
            {
                Core.Update.ApplicationUpdate.ShowUpdateResultAsync(appSettings.ClientSettings.SkipBetaVersionUpdate);
            }
        }
        
        private void frmScriptBuilder_Shown(object sender, EventArgs e)
        {

            Program.SplashForm.Hide();

            if (editMode)
                return;

            Notify("Welcome! Press 'Start Edit Script' to get started!");
        }

        private void pnlControlContainer_Paint(object sender, PaintEventArgs e)
        {
            //Rectangle rect = new Rectangle(0, 0, pnlControlContainer.Width, pnlControlContainer.Height);
            //using (LinearGradientBrush brush = new LinearGradientBrush(rect, Color.White, Color.WhiteSmoke, LinearGradientMode.Vertical))
            //{
            //    e.Graphics.FillRectangle(brush, rect);
            //}

            //Pen steelBluePen = new Pen(Color.SteelBlue, 2);
            //Pen lightSteelBluePen = new Pen(Color.LightSteelBlue, 1);
            ////e.Graphics.DrawLine(steelBluePen, 0, 0, pnlControlContainer.Width, 0);
            //e.Graphics.DrawLine(lightSteelBluePen, 0, 0, pnlControlContainer.Width, 0);
            //e.Graphics.DrawLine(lightSteelBluePen, 0, pnlControlContainer.Height - 1, pnlControlContainer.Width, pnlControlContainer.Height - 1);
        }
        
        private void frmScriptBuilder_Resize(object sender, EventArgs e)
        {
            //check when minimized

            if ((this.WindowState == FormWindowState.Minimized) && (appSettings.ClientSettings.MinimizeToTray))
            {
                appSettings = new Core.ApplicationSettings().GetOrCreateApplicationSettings();
                if (appSettings.ClientSettings.MinimizeToTray)
                {
                    notifyTray.Visible = true;
                    notifyTray.ShowBalloonTip(3000);
                    this.ShowInTaskbar = false;
                }
            }

            pnlMain.Invalidate();
        }
        #endregion

        #region Form title methods
        private void UpdateWindowTitle()
        {
            if (ScriptFilePath != null)
            {
                System.IO.FileInfo scriptFileInfo = new System.IO.FileInfo(ScriptFilePath);
                this.Text = "taskt - (" + scriptFileInfo.Name + ")";
            }
            else
            {
                this.Text = "taskt";
            }

            if (this.dontSaveFlag)
            {
                this.Text += " *";
            }
        }

        private void ChangeSaveState(bool dontSaveNow)
        {
            this.dontSaveFlag = dontSaveNow;
            UpdateWindowTitle();
            SetAutoSaveState();
        }
        #endregion

        #region Link Labels
        private void GenerateRecentFiles()
        {
            flwRecentFiles.Controls.Clear();


            var scriptPath = Core.IO.Folders.GetFolder(Core.IO.Folders.FolderType.ScriptsFolder);

            if (!System.IO.Directory.Exists(scriptPath))
            {
                lblRecentFiles.Text = "Script Folder does not exist";
                lblFilesMissing.Text = "Directory Not Found: " + scriptPath;
                lblRecentFiles.ForeColor = Color.White;
                lblFilesMissing.ForeColor = Color.White;
                lblFilesMissing.Show();
                flwRecentFiles.Hide();
                return;
            }

            var directory = new System.IO.DirectoryInfo(scriptPath);

            var recentFiles = directory.GetFiles("*.xml")
                .OrderByDescending(file => file.LastWriteTime).Select(f => f.Name);


            if (recentFiles.Count() == 0)
            {
                lblRecentFiles.Text = "No Recent Files Found";
                lblRecentFiles.ForeColor = Color.White;
                lblFilesMissing.ForeColor = Color.White;
                lblFilesMissing.Show();
                flwRecentFiles.Hide();
            }
            else
            {
                flwRecentFiles.SuspendLayout();
                foreach (var fil in recentFiles)
                {
                    if (flwRecentFiles.Controls.Count == 7)
                    {
                        break;
                    }

                    LinkLabel newFileLink = new LinkLabel();
                    newFileLink.Text = fil;
                    newFileLink.AutoSize = true;
                    newFileLink.LinkColor = Color.AliceBlue;
                    newFileLink.Font = lnkGitIssue.Font;
                    newFileLink.Margin = new Padding(0, 0, 0, 0);
                    newFileLink.LinkClicked += NewFileLink_LinkClicked;
                    flwRecentFiles.Controls.Add(newFileLink);
                }
                flwRecentFiles.ResumeLayout();
            }
        }
        private void lnkStartEdit_Click(object sender, EventArgs e)
        {
            pnlCommandHelper.Visible = false;
        }
        private void lnkGitProject_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            showGitProjectPage();
        }
        private void lnkGitLatestReleases_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            showGitReleasePage();
        }
        private void lnkGitIssue_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            showGitIssuePage();
        }
        private void lnkGitWiki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            showWikiPage();
        }
        private void linkGitter_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            showGitterPage();
        }
        private void NewFileLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel senderLink = (LinkLabel)sender;
            string targetScriptPath = Core.IO.Folders.GetFolder(Core.IO.Folders.FolderType.ScriptsFolder) + senderLink.Text;
            OpenScriptFromFilePath(targetScriptPath, true);
        }
        #endregion

        #region show sites
        private void showGitProjectPage()
        {
            System.Diagnostics.Process.Start(Core.MyURLs.GitProjectURL);
        }
        private void showGitReleasePage()
        {
            System.Diagnostics.Process.Start(Core.MyURLs.GitReleaseURL);
        }
        private void showGitIssuePage()
        {
            System.Diagnostics.Process.Start(Core.MyURLs.GitIssueURL);
        }
        private void showWikiPage()
        {
            System.Diagnostics.Process.Start(Core.MyURLs.WikiURL);
        }
        private void showGitterPage()
        {
            System.Diagnostics.Process.Start(Core.MyURLs.GitterURL);
        }
        private void showThisCommandHelp(ScriptCommand command)
        {
            string parent = ((Core.Automation.Attributes.ClassAttributes.Group)command.GetType().GetCustomAttribute(typeof(Core.Automation.Attributes.ClassAttributes.Group))).groupName;
            System.Diagnostics.Process.Start(Core.MyURLs.GetWikiURL(command.SelectionName, parent));
        }
        private void BeginShowThisCommandHelpProcess()
        {
            if (lstScriptActions.SelectedItems.Count > 0)
            {
                showThisCommandHelp((ScriptCommand)lstScriptActions.SelectedItems[0].Tag);
            }
        }
        #endregion

        #region ListView Events
        #region ListView DragDrop

        private void lstScriptActions_ItemDrag(object sender, ItemDragEventArgs e)
        {
            this.currentEditAction = CommandEditAction.Move;
            lstScriptActions.DoDragDrop(lstScriptActions.SelectedItems, DragDropEffects.Move);
        }

        private void lstScriptActions_DragOver(object sender, DragEventArgs e)
        {
            if (this.currentEditAction == CommandEditAction.Move)
            {
                Point cp = lstScriptActions.PointToClient(new Point(e.X, e.Y));
                var dragToItem = lstScriptActions.HitTest(cp.X, cp.Y);
                this.DnDIndex = (dragToItem.Item == null) ? -1 : dragToItem.Item.Index;
            }
            lstScriptActions.Invalidate();
        }

        private void lstScriptActions_DragEnter(object sender, DragEventArgs e)
        {
            int len = e.Data.GetFormats().Length - 1;
            if (e.Data.GetFormats()[0] == "System.Windows.Forms.ListView+SelectedListViewItemCollection")
            {
                e.Effect = DragDropEffects.Move;
                this.currentEditAction = CommandEditAction.Move;
            }
            else
            {
                //Console.WriteLine(formatType);
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    if ((e.KeyState & 12) != 0) // Shift or Ctrl
                    {
                        e.Effect = DragDropEffects.Copy;
                    }
                    else
                    {
                        e.Effect = DragDropEffects.Move;
                    }
                    lstScriptActions.BackColor = Color.LightGray;
                    this.currentEditAction = CommandEditAction.Normal;
                }
            }
        }

        private void lstScriptActions_DragLeave(object sender, EventArgs e)
        {
            lstScriptActions.BackColor = Color.WhiteSmoke;
        }

        private void lstScriptActions_DragDrop(object sender, DragEventArgs e)
        {
            lstScriptActions.BackColor = Color.WhiteSmoke;

            string[] fileNames = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if ((fileNames != null) && (fileNames.Length > 0))
            {
                var targetFile = fileNames[0];
                if (System.IO.Path.GetExtension(targetFile).ToLower() == ".xml")
                {
                    if ((e.KeyState & 12) != 0) // Shift or Ctrl
                    {
                        ImportScriptFromFilePath(targetFile);
                    }
                    else
                    {
                        OpenScriptFromFilePath(targetFile, true);
                    }
                }
                else
                {
                    using (var fm = new UI.Forms.Supplemental.frmDialog("This file type can not open.", "File Open Error", UI.Forms.Supplemental.frmDialog.DialogType.OkOnly, 0))
                    {
                        fm.ShowDialog();
                    }
                }
            }
            else if (lstScriptActions.SelectedItems.Count > 0)
            {
                MoveCommands(e);
            }
        }

        private void lstScriptActions_MouseMove(object sender, MouseEventArgs e)
        {
            lstScriptActions.Invalidate();
        }

        private void MoveCommands(DragEventArgs e)
        {
            CreateUndoSnapshot();

            //Returns the location of the mouse pointer in the ListView control.
            Point cp = lstScriptActions.PointToClient(new Point(e.X, e.Y));
            //Obtain the item that is located at the specified location of the mouse pointer.
            ListViewItem dragToItem = lstScriptActions.GetItemAt(cp.X, cp.Y);
            if (dragToItem == null)
            {
                return;
            }

            lstScriptActions.BeginUpdate();

            //drag and drop for sequence
            if ((dragToItem.Tag is SequenceCommand) && (appSettings.ClientSettings.EnableSequenceDragDrop))
            {
                //sequence command for drag drop
                var sequence = (SequenceCommand)dragToItem.Tag;

                //add command to script actions
                for (int i = 0; i <= lstScriptActions.SelectedItems.Count - 1; i++)
                {
                    var command = (ScriptCommand)lstScriptActions.SelectedItems[i].Tag;
                    sequence.v_scriptActions.Add(command);
                }

                //remove originals
                int[] indices = new int[lstScriptActions.SelectedIndices.Count];
                lstScriptActions.SelectedIndices.CopyTo(indices, 0);
                for (int i = indices.Length - 1; i>=0; i--)
                {
                    lstScriptActions.Items.RemoveAt(indices[i]);
                }
            }
            else
            {
                //Obtain the index of the item at the mouse pointer.
                int dragIndex = dragToItem.Index;

                ListViewItem[] sel = new ListViewItem[lstScriptActions.SelectedItems.Count];
                for (int i = 0; i <= lstScriptActions.SelectedItems.Count - 1; i++)
                {
                    sel[i] = lstScriptActions.SelectedItems[i];
                }
                for (int i = 0; i < sel.GetLength(0); i++)
                {
                    //Obtain the ListViewItem to be dragged to the target location.
                    ListViewItem dragItem = sel[i];
                    int itemIndex = dragIndex;
                    if (itemIndex == dragItem.Index)
                    {
                        //return;
                        break;
                    }
                    if (dragItem.Index < itemIndex)
                        itemIndex++;
                    else
                        itemIndex = dragIndex + i;
                    //Insert the item at the mouse pointer.
                    ListViewItem insertItem = (ListViewItem)dragItem.Clone();

                    var command = (ScriptCommand)insertItem.Tag;
                    command.IsDontSavedCommand = true;
                    command.IsNewInsertedCommand = true;

                    lstScriptActions.Items.Insert(itemIndex, insertItem);
                    //Removes the item from the initial location while
                    //the item is moved to the new location.
                    lstScriptActions.Items.Remove(dragItem);
                }
            }
            this.currentEditAction = CommandEditAction.Normal;

            IndentListViewItems();  // update indent

            ChangeSaveState(true);

            lstScriptActions.EndUpdate();
            lstScriptActions.Invalidate();
        }

        #endregion

        #region ListView Search

        private void txtCommandSearch_TextChanged(object sender, EventArgs e)
        {
            //if (lstScriptActions.Items.Count == 0)
            //    return;

            //reqdIndex = 0;

            //if (txtCommandSearch.Text == "")
            //{
            //    //hide info
            //    HideSearchInfo();

            //    //clear indexes
            //    matchingSearchIndex.Clear();
            //    currentIndexInMatchItems = -1;

            //    //repaint
            //    lstScriptActions.Invalidate();
            //}
            //else
            //{
            //    lblCurrentlyViewing.Show();
            //    lblTotalResults.Show();
            //    SearchForItemInListView();

            //    //repaint
            //    lstScriptActions.Invalidate();
            //}
        }

        private void tsSearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                SearchForItemInListView();
            }
        }

        private void txtCommandSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                SearchForItemInListView();
            }
        }

        private void HideSearchInfo()
        {
            lblCurrentlyViewing.Hide();
            lblTotalResults.Hide();
        }
        private void SearchForItemInListView()
        {
            var searchCriteria = txtCommandSearch.Text;

            if (searchCriteria == "")
            {
                searchCriteria = tsSearchBox.Text;
            }

            if (searchCriteria.Length > 0)
            {
                if ((string)pbSearch.Tag != searchCriteria)
                {
                    pbSearch.Tag = searchCriteria;
                    AdvancedSearchItemInCommands(searchCriteria, false, false, false, false, true, false, "");
                }
                else
                {
                    MoveMostNearMatchedLine(true);
                }
            }
            else
            {
                ClearHighlightListViewItem();
            }
        }

        private void pbSearch_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void pbSearch_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        public int AdvancedSearchItemInCommands(string keyword, bool caseSensitive, bool checkParameters, bool checkCommandName, bool checkComment, bool checkDisplayText, bool checkInstanceType, string instanceType)
        {
            int matchedCount = 0;

            this.currentIndexInMatchItems = -1;
            lstScriptActions.SuspendLayout();
            lstScriptActions.BeginUpdate();
            foreach(ListViewItem itm in lstScriptActions.Items)
            {
                ScriptCommand cmd = (ScriptCommand)itm.Tag;
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

        #region ListView Copy, Paste, Edit, Delete

        private void EditSelectedCommand()
        {
            if (lstScriptActions.SelectedItems.Count != 1)
            {
                return;
            }

            //bring up edit mode to edit the action
            ListViewItem selectedCommandItem = lstScriptActions.SelectedItems[0];

            //set selected command from the listview item tag object which was assigned to the command
            var currentCommand = (ScriptCommand)selectedCommandItem.Tag;

            //check if editing a sequence
            if (currentCommand is SequenceCommand)
            {

                if (editMode)
                {
                    MessageBox.Show("Embedding Sequence Commands within Sequence Commands not yet supported.");
                    return;
                }

                //get sequence events
                SequenceCommand sequence = (SequenceCommand)currentCommand;

                using (var newBuilder = new frmScriptBuilder())
                {
                    //add variables

                    newBuilder.scriptVariables = new List<ScriptVariable>();
                    newBuilder.instanceList = instanceList;

                    foreach (var variable in this.scriptVariables)
                    {
                        newBuilder.scriptVariables.Add(variable);
                    }

                    //append to new builder
                    foreach (var cmd in sequence.v_scriptActions)
                    {
                        newBuilder.lstScriptActions.Items.Add(CreateScriptCommandListViewItem(cmd));
                    }

                    //apply editor style format
                    newBuilder.ApplyEditorFormat();

                    newBuilder.parentBuilder = this;

                    //if data has been changed
                    if (newBuilder.ShowDialog() == DialogResult.OK)
                    {
                        ChangeSaveState(true);

                        //create updated list
                        List<ScriptCommand> updatedList = new List<ScriptCommand>();

                        //update to list
                        for (int i = 0; i < newBuilder.lstScriptActions.Items.Count; i++)
                        {
                            var command = (ScriptCommand)newBuilder.lstScriptActions.Items[i].Tag;
                            updatedList.Add(command);
                        }

                        //apply new list to existing sequence
                        sequence.v_scriptActions = updatedList;
                        sequence.IsDontSavedCommand = true;

                        //update label
                        selectedCommandItem.Text = sequence.GetDisplayValue();
                    }
                }
            }
            else
            {
                ScriptCommand cloneCommand = currentCommand.Clone();
                currentCommand.RemoveInstance(instanceList);

                //create new command editor form
                using (var editCommand = new frmCommandEditor(automationCommands, GetConfiguredCommands(), this.bufferedCommandList, this.bufferedCommandTreeImages))
                {
                    //creation mode edit locks form to current command
                    editCommand.creationMode = frmCommandEditor.CreationMode.Edit;

                    editCommand.editingCommand = currentCommand;

                    //create clone of current command so databinding does not affect if changes are not saved
                    editCommand.originalCommand = Core.Common.Clone(currentCommand);

                    //set variables
                    editCommand.scriptVariables = this.scriptVariables;

                    // set taskt settings
                    editCommand.appSettings = this.appSettings;

                    // set instance counter
                    editCommand.instanceList = this.instanceList;

                    // set size, position
                    if ((lastEditorSize.Width != 0) && (lastEditorSize.Height != 0))
                    {
                        editCommand.Size = lastEditorSize;
                        editCommand.StartPosition = FormStartPosition.Manual;
                        editCommand.Location = lastEditorPosition;
                    }

                    //show edit command form and save changes on OK result
                    if (editCommand.ShowDialog(this) == DialogResult.OK)
                    {
                        ChangeSaveState(true);

                        selectedCommandItem.Tag = editCommand.selectedCommand;
                        selectedCommandItem.Text = editCommand.selectedCommand.GetDisplayValue(); //+ "(" + cmdDetails.SelectedVariables() + ")";
                        selectedCommandItem.SubItems.Add(editCommand.selectedCommand.GetDisplayValue());

                        editCommand.selectedCommand.IsDontSavedCommand = true;

                        editCommand.selectedCommand.AddInstance(instanceList);
                    }
                    else
                    {
                        cloneCommand.AddInstance(instanceList);
                    }
                }
            }
        }

        private void SelectAllRows()
        {
            lstScriptActions.BeginUpdate();
            foreach (ListViewItem itm in lstScriptActions.Items)
            {
                itm.Selected = true;
            }
            lstScriptActions.EndUpdate();
        }

        private void DeleteRows()
        {
            lstScriptActions.BeginUpdate();

            int[] indices = new int[lstScriptActions.SelectedItems.Count];
            lstScriptActions.SelectedIndices.CopyTo(indices, 0);

            // remove instance name
            List<ScriptCommand> removeCommands = new List<ScriptCommand>();
            foreach(int i in indices)
            {
                removeCommands.Add((ScriptCommand)lstScriptActions.Items[i].Tag);
            }
            RemoveInstanceName(removeCommands);

            for (var i = indices.Length - 1; i >= 0; i--)
            {
                lstScriptActions.Items.RemoveAt(indices[i]);
            }
            lstScriptActions.EndUpdate();
            ChangeSaveState(true);

            CreateUndoSnapshot();

            // check indent
            IndentListViewItems();

            lstScriptActions.Invalidate();
        }

        private void CutRows()
        {
            //copy into list for all selected            
            if (lstScriptActions.SelectedItems.Count >= 1)
            {
                ChangeSaveState(true);

                var commands = new List<ScriptCommand>();

                lstScriptActions.BeginUpdate();
                int[] indices = new int[lstScriptActions.SelectedItems.Count];
                lstScriptActions.SelectedIndices.CopyTo(indices, 0);

                for (int i = 0; i <indices.Length; i++)
                {
                    commands.Add((ScriptCommand)lstScriptActions.Items[indices[i]].Tag);
                }
                for (int i = indices.Length - 1; i >= 0; i--)
                {
                    lstScriptActions.Items.RemoveAt(indices[i]);
                }
                lstScriptActions.EndUpdate();

                // set clipborad xml string
                Clipboard.SetText(Core.Script.Script.SerializeScript(commands, scriptSerializer));

                // remove instance name
                RemoveInstanceName(commands);

                //Notify(rowsSelectedForCopy.Count + " item(s) cut to clipboard!");
                Notify(commands.Count + " item(s) cut to clipboard!");

                // release
                commands.Clear();

                // check indent
                IndentListViewItems();
            }
        }

        private void CopyRows()
        {
            //copy into list for all selected            
            if (lstScriptActions.SelectedItems.Count >= 1)
            {
                ChangeSaveState(true);

                var commands = new List<ScriptCommand>();

                foreach (ListViewItem item in lstScriptActions.SelectedItems)
                {
                    commands.Add((ScriptCommand)item.Tag);
                }

                // set clipborad xml string
                Clipboard.SetText(Core.Script.Script.SerializeScript(commands, scriptSerializer));

                Notify(commands.Count + " item(s) copied to clipboard!");

                // release
                commands.Clear();
            }
        }

        private void PasteRows()
        {
            var sc = Core.Script.Script.DeserializeXML(Clipboard.GetText(), scriptSerializer);
            if (sc != null)
            {
                ChangeSaveState(true);

                lstScriptActions.BeginUpdate();
                InsertExecutionCommands(sc.Commands);
                lstScriptActions.EndUpdate();

                // add instance name
                AddInstanceName(sc.Commands.Select(t => t.ScriptCommand).ToList());

                Notify(sc.Commands.Count + " item(s) pasted!");
                // release
                sc = null;

                // check indent
                IndentListViewItems();
            }
            else
            {
                Notify("Error! can not paste item(s).");
            }
        }

        private void UndoChange()
        {
            if (undoList.Count > 0)
            {
                if ((undoIndex < 0) || (undoIndex >= undoList.Count))
                {
                    undoIndex = undoList.Count - 1;
                }

                lstScriptActions.Items.Clear();

                foreach (ListViewItem rowItem in undoList[undoIndex])
                {
                    lstScriptActions.Items.Add(rowItem);
                }

                undoIndex--;

                // check indent
                IndentListViewItems();

                lstScriptActions.Invalidate();
            }
        }

        private void RedoChange()
        {
            if (undoList.Count > 0)
            {
                undoIndex++;

                if (undoIndex > undoList.Count - 1)
                {
                    undoIndex = undoList.Count - 1;
                }

                lstScriptActions.Items.Clear();

                foreach (ListViewItem rowItem in undoList[undoIndex])
                {
                    lstScriptActions.Items.Add(rowItem);
                }

                // check indent
                IndentListViewItems();

                lstScriptActions.Invalidate();
            }
        }
        private void SetSelectedCodeToCommented(bool setCommented)
        {
            //warn if nothing was selected
            if (lstScriptActions.SelectedItems.Count == 0)
            {
                Notify("No code was selected!");
            }

            //get each item and set appropriately
            foreach (ListViewItem item in lstScriptActions.SelectedItems)
            {
                var selectedCommand = (ScriptCommand)item.Tag;
                selectedCommand.IsCommented = setCommented;
            }

            ChangeSaveState(true);

            //recolor
            lstScriptActions.Invalidate();
        }

        private void SetPauseBeforeExecution()
        {
            //warn if nothing was selected
            if (lstScriptActions.SelectedItems.Count == 0)
            {
                Notify("No code was selected!");
            }

            //get each item and set appropriately
            foreach (ListViewItem item in lstScriptActions.SelectedItems)
            {
                var selectedCommand = (ScriptCommand)item.Tag;
                selectedCommand.PauseBeforeExeucution = !selectedCommand.PauseBeforeExeucution;
            }

            ChangeSaveState(true);

            lstScriptActions.Invalidate();
        }

        private void ClearSelectedListViewItems()
        {
            if (lstScriptActions.FocusedItem != null)
            {
                lstScriptActions.FocusedItem.Focused = false;
            }
            
            lstScriptActions.SelectedItems.Clear();
            selectedIndex = -1;
            lstScriptActions.Invalidate();
        }

        private void ClearHighlightListViewItem()
        {
            this.currentScriptEditorMode = CommandEditorState.Normal;
            lstScriptActions.Invalidate();
        }

        private void CreateUndoSnapshot()
        {
            List<ListViewItem> itemList = new List<ListViewItem>();
            foreach (ListViewItem rowItem in lstScriptActions.Items)
            {
                itemList.Add(rowItem);
            }

            undoList.Add(itemList);

            if (undoList.Count > 10)
            {
                undoList.RemoveAt(0);
            }


            undoIndex = itemList.Count - 1;
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

        #region ListView Create Item

        private ListViewItem CreateScriptCommandListViewItem(ScriptCommand cmdDetails, bool isOpenFile = false)
        {
            ListViewItem newCommand = new ListViewItem();

            string dispValue = cmdDetails.GetDisplayValue();

            if (!isOpenFile)
            {
                cmdDetails.IsDontSavedCommand = true;
                cmdDetails.IsNewInsertedCommand = true;
            }

            newCommand.Text = dispValue;
            newCommand.ToolTipText = dispValue;

            newCommand.SubItems.AddRange(new string[] { "", "" });

            cmdDetails.RenderedControls = null;
            newCommand.Tag = cmdDetails;

            newCommand.ImageIndex = Images.GetUIImageList(cmdDetails.GetType().Name);
            return newCommand;
        }

        public void AddCommandToListView(ScriptCommand selectedCommand)
        {
            if (pnlCommandHelper.Visible)
            {
                pnlCommandHelper.Hide();
            }

            var command = CreateScriptCommandListViewItem(selectedCommand);

            // count instance name
            AddInstanceName(new List<ScriptCommand>() { selectedCommand });

            //insert to end by default
            var insertionIndex = lstScriptActions.Items.Count;

            //verify setting to insert inline is selected and if an item is currently selected
            if ((appSettings.ClientSettings.InsertCommandsInline) && (lstScriptActions.SelectedItems.Count > 0))
            {
                //insert inline
                insertionIndex = lstScriptActions.SelectedItems[0].Index + 1;            
            }

            // insert comment above if, loop, try
            if (appSettings.ClientSettings.InsertCommentIfLoopAbove)
            {
                if ((selectedCommand is BeginListLoopCommand) || (selectedCommand is BeginContinousLoopCommand) || (selectedCommand is BeginNumberOfTimesLoopCommand) || (selectedCommand is BeginLoopCommand) || (selectedCommand is BeginMultiLoopCommand))
                {
                    lstScriptActions.Items.Insert(insertionIndex, CreateScriptCommandListViewItem(new CommentCommand() { v_Comment = "Please enter a description of the loop here" }));
                    insertionIndex++;
                }
                else if((selectedCommand is BeginIfCommand) || (selectedCommand is BeginMultiIfCommand))
                {
                    lstScriptActions.Items.Insert(insertionIndex, CreateScriptCommandListViewItem(new CommentCommand() { v_Comment = "Please enter a description of the if here" }));
                    insertionIndex++;
                }
                else if(selectedCommand is TryCommand)
                {
                    lstScriptActions.Items.Insert(insertionIndex, CreateScriptCommandListViewItem(new CommentCommand() { v_Comment = "Please enter a description of the error handling here" }));
                    insertionIndex++;
                }
            }

            //insert command
            lstScriptActions.Items.Insert(insertionIndex, command);

            var focusIndex = insertionIndex;

            //special types also get a following command and comment
            if ((selectedCommand is BeginListLoopCommand) || (selectedCommand is BeginContinousLoopCommand) || (selectedCommand is BeginNumberOfTimesLoopCommand) || (selectedCommand is BeginLoopCommand) || (selectedCommand is BeginMultiLoopCommand))
            {
                lstScriptActions.Items.Insert(insertionIndex + 1, CreateScriptCommandListViewItem(new CommentCommand() { v_Comment = "Items in this section will run within the loop" }));
                lstScriptActions.Items.Insert(insertionIndex + 2, CreateScriptCommandListViewItem(new EndLoopCommand()));
                focusIndex++;
            }
            else if ((selectedCommand is BeginIfCommand) || (selectedCommand is BeginMultiIfCommand))
            {
                if (appSettings.ClientSettings.InsertElseAutomatically)
                {
                    lstScriptActions.Items.Insert(insertionIndex + 1, CreateScriptCommandListViewItem(new CommentCommand() { v_Comment = "Items in this section will run if the statement is true" }));
                    lstScriptActions.Items.Insert(insertionIndex + 2, CreateScriptCommandListViewItem(new ElseCommand()));
                    lstScriptActions.Items.Insert(insertionIndex + 3, CreateScriptCommandListViewItem(new CommentCommand() { v_Comment = "Items in this section will run if the statement is false" }));
                    lstScriptActions.Items.Insert(insertionIndex + 4, CreateScriptCommandListViewItem(new EndIfCommand()));
                }
                else
                {
                    lstScriptActions.Items.Insert(insertionIndex + 1, CreateScriptCommandListViewItem(new CommentCommand() { v_Comment = "Items in this section will run if the statement is true" }));
                    lstScriptActions.Items.Insert(insertionIndex + 2, CreateScriptCommandListViewItem(new EndIfCommand()));
                }
                focusIndex++;
            }
            else if (selectedCommand is TryCommand)
            {
                lstScriptActions.Items.Insert(insertionIndex + 1, CreateScriptCommandListViewItem(new CommentCommand() { v_Comment = "Items in this section will be handled if error occurs" }));
                lstScriptActions.Items.Insert(insertionIndex + 2, CreateScriptCommandListViewItem(new CatchExceptionCommand() { v_Comment = "Items in this section will run if error occurs" }));
                lstScriptActions.Items.Insert(insertionIndex + 3, CreateScriptCommandListViewItem(new CommentCommand() { v_Comment = "This section executes if error occurs above" }));
                lstScriptActions.Items.Insert(insertionIndex + 4, CreateScriptCommandListViewItem(new EndTryCommand()));
                focusIndex++;
            }

            // focus insert command
            if (lstScriptActions.SelectedItems.Count > 0)
            {
                lstScriptActions.MultiSelect = false;
                for (var i = lstScriptActions.SelectedItems.Count - 1; i >= 0; i--)
                {
                    var idx = lstScriptActions.SelectedItems[i].Index;
                    lstScriptActions.Items[idx].Focused = false;
                }
            }
            lstScriptActions.Items[focusIndex].Selected = true;
            lstScriptActions.MultiSelect = true;

            CreateUndoSnapshot();

            // check indent
            IndentListViewItems();

            lstScriptActions.Invalidate();

            //AutoSizeLineNumberColumn();
        }

        #endregion

        #region ListView Comment, Coloring, Drawing
        private void IndentListViewItems()
        {
            int indent = 0;
            foreach (ListViewItem rowItem in lstScriptActions.Items)
            {
                if (rowItem is null)
                {
                    continue;
                }

                if ((rowItem.Tag is BeginIfCommand) || (rowItem.Tag is BeginMultiIfCommand) || (rowItem.Tag is BeginListLoopCommand) || (rowItem.Tag is BeginContinousLoopCommand) || (rowItem.Tag is BeginNumberOfTimesLoopCommand) || (rowItem.Tag is TryCommand) || (rowItem.Tag is BeginLoopCommand) || (rowItem.Tag is BeginMultiLoopCommand))
                {
                    indent += 2;
                    rowItem.IndentCount = indent;
                    indent += 2;
                }
                else if ((rowItem.Tag is EndLoopCommand) || (rowItem.Tag is EndIfCommand) || (rowItem.Tag is EndTryCommand))
                {
                    indent -= 2;
                    if (indent < 0) indent = 0;
                    rowItem.IndentCount = indent;
                    indent -= 2;
                    if (indent < 0) indent = 0;
                }
                else if ((rowItem.Tag is ElseCommand) || (rowItem.Tag is CatchExceptionCommand) || (rowItem.Tag is FinallyCommand))
                {
                    indent -= 2;
                    if (indent < 0) indent = 0;
                    rowItem.IndentCount = indent;
                    indent += 2;
                    if (indent < 0) indent = 0;
                }
                else
                {
                    rowItem.IndentCount = indent;
                }
            }
            AutoSizeLineNumberColumn();

            if (appSettings.ClientSettings.ShowScriptMiniMap)
            {
                CreateMiniMap();
            }
        }

        private void AutoSizeLineNumberColumn()
        {
            //auto adjust column width based on # of commands
            int columnWidth = (14 * lstScriptActions.Items.Count.ToString().Length);
            int difWidth = columnWidth - lstScriptActions.Columns[0].Width;

            lstScriptActions.Columns[0].Width = columnWidth;
            lstScriptActions.Columns[2].Width = lstScriptActions.ClientSize.Width - columnWidth - lstScriptActions.Columns[1].Width;
        }

        private void CreateMiniMap()
        {
            if (lstScriptActions.Items.Count == 0)
            {
                return;
            }

            int lvItemHeight = lstScriptActions.Items[0].Bounds.Height;
            if (lvItemHeight < 1)
            {
                return;
            }

            int commandsNum = lstScriptActions.Items.Count;
            int lvShowLines = lstScriptActions.ClientRectangle.Height / lvItemHeight;

            int miniMapHeight = lvItemHeight * lvShowLines;
            int mapItemHeight = miniMapHeight / commandsNum;
            if (mapItemHeight < 2)
            {
                mapItemHeight = 2;
            }
            else if (mapItemHeight > lvItemHeight)
            {
                mapItemHeight = lvItemHeight;
            }

            int miniMapItems = miniMapHeight / mapItemHeight;
            if (miniMapItems > commandsNum)
            {
                miniMapItems = commandsNum;
            }

            double oneItemRatio = (double)miniMapItems / commandsNum;
            int steps = 1;
            if (oneItemRatio< 1.0)
            {
                double invRatio = 1.0 / oneItemRatio;
                steps = (int)invRatio;
                if (invRatio - steps != 0.0)
                {
                    steps++;
                }

                miniMapItems = (commandsNum / steps) + (commandsNum % steps == 0 ? 0 : 1);
            }

            if ((miniMap == null) || (miniMap.GetLength(0) != miniMapItems))
            {
                miniMap = null;
                miniMap = new int[miniMapItems, 2];
            }

            if (oneItemRatio < 1.0)
            {
                for (int i = 0; i < miniMapItems; i++)
                {
                    int startIndex = i * steps;
                    int lastIndex = startIndex + steps;
                    if (lastIndex > commandsNum)
                    {
                        lastIndex = commandsNum;
                    }

                    miniMap[i, 0] = (int)MiniMapState.Normal;
                    miniMap[i, 1] = (int)MiniMapState.Normal;
                    for (int j = startIndex; j < lastIndex; j++)
                    {
                        var command = (ScriptCommand)lstScriptActions.Items[j].Tag;
                        if (command.IsDontSavedCommand)
                        {
                            miniMap[i, 0] = (int)MiniMapState.DontSave;
                            break;
                        }
                        else if (command.IsNewInsertedCommand)
                        {
                            miniMap[i, 0] = (int)MiniMapState.NewInserted;
                            break;
                        }
                    }
                    for (int j = startIndex; j < lastIndex; j++)
                    {
                        var command = (ScriptCommand)lstScriptActions.Items[j].Tag;
                        if (j == selectedIndex)
                        {
                            miniMap[i, 1] = (int)MiniMapState.Cursor;
                            break;
                        }
                        else if (command.IsMatched)
                        {
                            miniMap[i, 1] = (int)MiniMapState.Matched;
                            break;
                        }
                        else if (!command.IsValid)
                        {
                            miniMap[i, 1] = (int)MiniMapState.Error;
                            break;
                        }
                        else if (command.IsCommented || (command is CommentCommand))
                        {
                            miniMap[i, 1] = (int)MiniMapState.Comment;
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < commandsNum; i++)
                {
                    var command = (ScriptCommand)lstScriptActions.Items[i].Tag;
                    if (command.IsDontSavedCommand)
                    {
                        miniMap[i, 0] = (int)MiniMapState.DontSave;
                    }
                    else if (command.IsNewInsertedCommand)
                    {
                        miniMap[i, 0] = (int)MiniMapState.NewInserted;
                    }
                    else
                    {
                        miniMap[i, 0] = (int)MiniMapState.Normal;
                    }

                    if (i == selectedIndex)
                    {
                        miniMap[i, 1] = (int)MiniMapState.Cursor;
                    }
                    else if (command.IsMatched)
                    {
                        miniMap[i, 1] = (int)MiniMapState.Matched;
                    }
                    else if (!command.IsValid)
                    {
                        miniMap[i, 1] = (int)MiniMapState.Error;
                    }
                    else if (command.IsCommented || (command is CommentCommand))
                    {
                        miniMap[i, 1] = (int)MiniMapState.Comment;
                    }
                    else
                    {
                        miniMap[i, 1] = (int)MiniMapState.Normal;
                    }
                }
            }

            if ((miniMapImg == null) || (miniMapImg.Height != lstScriptActions.Height))
            {
                miniMapImg = new Bitmap(8, lstScriptActions.Height);
            }
            using (Graphics g = Graphics.FromImage(miniMapImg))
            {
                g.DrawLine(new Pen(Color.White), 0, 0, 0, miniMapImg.Height);
                g.DrawLine(new Pen(Color.White), 4, 0, 4, miniMapImg.Height);
                for (int i = 0; i < miniMapItems; i++)
                {
                    SolidBrush co;
                    switch ((MiniMapState)miniMap[i, 0])
                    {
                        case MiniMapState.DontSave:
                            co = new SolidBrush(Core.Theme.scriptTexts["number-dontsaved"].BackColor);
                            break;
                        case MiniMapState.NewInserted:
                            co = new SolidBrush(Core.Theme.scriptTexts["number-newline"].BackColor);
                            break;
                        default:
                            co = new SolidBrush(Core.Theme.scriptTexts["normal"].BackColor);
                            break;
                    }
                    g.FillRectangle(co, 1, mapItemHeight * i, 3, mapItemHeight);

                    switch ((MiniMapState)miniMap[i, 1])
                    {
                        //case MiniMapState.Cursor:
                        //    co = new SolidBrush(taskt.Core.Theme.scriptTexts["selected-normal"].BackColor);
                        //    break;
                        case MiniMapState.Matched:
                            co = new SolidBrush(Core.Theme.scriptTexts["current-match"].BackColor);
                            break;
                        case MiniMapState.Error:
                            co = new SolidBrush(Core.Theme.scriptTexts["invalid"].FontColor);
                            break;
                        case MiniMapState.Comment:
                            co = new SolidBrush(Core.Theme.scriptTexts["comment"].FontColor);
                            break;
                        default:
                            co = new SolidBrush(Core.Theme.scriptTexts["normal"].BackColor);
                            break;
                    }
                    g.FillRectangle(co, 5, mapItemHeight * i, 3, mapItemHeight);

                    if ((MiniMapState)miniMap[i, 1] == MiniMapState.Cursor)
                    {
                        g.FillRectangle(new SolidBrush(Color.Navy), 0, mapItemHeight * i, 8, mapItemHeight);
                    }
                }
                g.FillRectangle(new SolidBrush(Color.DarkGray), 0, mapItemHeight * miniMapItems, 8, lstScriptActions.Height - (mapItemHeight * miniMapItems));
                // DBG
                //miniMapImg.Save(System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\minimap.png");
            }

            //Console.WriteLine("cmdLines: " + lvCommandsNum +  ", 1itemHe: " + mapItemHeight + ", maxLines: " + maxMapItems + ", 1lineCmds: " + oneItemRatio);
        }

        private void lstScriptActions_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            //switch between column index
            switch (e.ColumnIndex)
            {
                case 0:
                    //draw row number
                    drawLineNumber(e);
                    break;
                case 1:
                    //draw command icon
                    drawCommandIcon(e);
                    break;

                case 2:
                    //write command text
                    drawCommandText(e);

                    break;
            }
        }

        private void drawLineNumber(DrawListViewSubItemEventArgs e)
        {
            //AutoSizeLineNumberColumn();
            var command = (ScriptCommand)e.Item.Tag;

            Core.Theme.UIFont trg = Core.Theme.scriptTexts[decideLineNumberText(command)];

            e.Graphics.FillRectangle(new SolidBrush(trg.BackColor), e.Bounds);
            e.Graphics.DrawString((e.ItemIndex + 1).ToString(), new Font(trg.Font, trg.FontSize, trg.Style), new SolidBrush(trg.FontColor), e.Bounds);
        }

        private void drawCommandIcon(DrawListViewSubItemEventArgs e)
        {
            var command = (ScriptCommand)e.Item.Tag;
            var modifiedBounds = e.Bounds;
            var img = Images.GetUIImage(command.GetType().Name);
            if (img != null)
            {
                e.Graphics.DrawImage(img, modifiedBounds.Left, modifiedBounds.Top + 3);
            }
        }

        private void drawCommandText(DrawListViewSubItemEventArgs e)
        {
            //get listviewitem
            ListViewItem item = e.Item;
            var command = (ScriptCommand)item.Tag;
            var modifiedBounds = e.Bounds;

            Core.Theme.UIFont trg;

            int indentWidth = appSettings.ClientSettings.IndentWidth;

            if ((debugLine > 0) && (e.ItemIndex == debugLine - 1))
            {
                trg = Core.Theme.scriptTexts["debug"];
            }
            else if ((currentScriptEditorMode == CommandEditorState.Search) || (currentScriptEditorMode == CommandEditorState.AdvencedSearch) || (currentScriptEditorMode == CommandEditorState.ReplaceSearch) || (currentScriptEditorMode == CommandEditorState.HighlightCommand))
            {
                if (command.IsMatched)
                {
                    if ((e.Item.Focused) || (e.Item.Selected))
                    {
                        trg = Core.Theme.scriptTexts["current-match"];
                    }
                    else
                    {
                        trg = Core.Theme.scriptTexts["match"];
                    }
                }
                else
                {
                    trg = Core.Theme.scriptTexts[decideNormalCommandText(e, command)];
                }
            }
            else
            {
                trg = Core.Theme.scriptTexts[decideNormalCommandText(e, command)];
            }

            //fille with background color
            e.Graphics.FillRectangle(new SolidBrush(trg.BackColor), modifiedBounds);

            //get indent count
            var indentPixels = (item.IndentCount * indentWidth);

            //set indented X position
            modifiedBounds.X += indentPixels;

            //draw string
            e.Graphics.DrawString(command.GetDisplayValue(),
                           new Font(trg.Font, trg.FontSize, trg.Style), new SolidBrush(trg.FontColor), modifiedBounds);

            // Emphasis Drag&Drop Line
            // DBG
            //Console.WriteLine("DRW " + this.currentEditAction + ", " + this.DnDIndex);
            if ((this.currentEditAction == CommandEditAction.Move) && (item.Index == this.DnDIndex))
            {
                int y = (lstScriptActions.SelectedItems[0].Index <= this.DnDIndex) ? (modifiedBounds.Y + modifiedBounds.Height - 1) : (modifiedBounds.Y);
                e.Graphics.DrawLine(new Pen(Color.DarkRed), modifiedBounds.X, y, modifiedBounds.X + modifiedBounds.Width, y);
            }

            // indent tab line
            if ((item.IndentCount > 0) && appSettings.ClientSettings.ShowIndentLine)
            {
                int offset;
                int i;
                if (item.IndentCount % 4 == 0)
                {
                    offset = indentWidth * 2;
                    i = item.IndentCount - 2;
                }
                else
                {
                    offset = 0;
                    i = item.IndentCount;
                }
                int bottomY = modifiedBounds.Y + modifiedBounds.Height;
                for (i = (item.IndentCount % 4 != 0 ? item.IndentCount - 2 : item.IndentCount); i > 0; i -= 4)
                {
                    int x = modifiedBounds.X - (i * indentWidth) + offset;
                    e.Graphics.DrawLine(indentDashLine, x, modifiedBounds.Y, x, bottomY);
                }

                int baseX = modifiedBounds.X - (item.IndentCount * indentWidth) + 2;
                e.Graphics.DrawLine(indentDashLine, baseX, modifiedBounds.Y, baseX, bottomY);
            }

            if (appSettings.ClientSettings.ShowScriptMiniMap)
            {
                modifiedBounds.X -= indentPixels;
                int baseX2 = modifiedBounds.Width - 8;
                e.Graphics.DrawImage(miniMapImg, new Rectangle(modifiedBounds.X + baseX2, modifiedBounds.Y, 8, modifiedBounds.Height),
                                                    new Rectangle(0, modifiedBounds.Y, 8, modifiedBounds.Height), GraphicsUnit.Pixel);
            }
        }

        private string decideNormalCommandText(DrawListViewSubItemEventArgs e, ScriptCommand command)
        {
            string ret;
            
            if (command.PauseBeforeExeucution)
            {
                ret = "pause";
            }
            else if ((command is CommentCommand) || (command.IsCommented))
            {
                ret = "comment";
            }
            else if (!command.IsValid)
            {
                ret = "invalid";
            }
            else
            {
                ret = "normal";
            }

            if ((e.Item.Focused) || (e.Item.Selected))
            {
                ret = "selected-" + ret;
            }

            return ret;
        }
        private string decideLineNumberText(ScriptCommand command)
        {
            string ret;

            if (command.IsDontSavedCommand)
            {
                ret = "dontsaved";
            }
            else if (command.IsNewInsertedCommand)
            {
                ret = "newline";
            }
            else
            {
                ret = "normal";
            }
            return "number-" + ret;
        }

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

        #region lstScriptActions event
        private void lstScriptActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            miniMapLoadingDelayTimer.Stop();
            miniMapLoadingDelayTimer.Start();
        }

        private void miniMapLoadingDelayTimer_Tick(object sender, EventArgs e)
        {
            miniMapLoadingDelayTimer.Stop();
            lstScriptActions_SelectedIndexChangedProcess();
        }

        private void lstScriptActions_SelectedIndexChangedProcess()
        {
            if (!appSettings.ClientSettings.InsertCommandsInline)
                return;

            //check to see if an item has been selected last
            if (lstScriptActions.SelectedItems.Count > 0)
            {
                selectedIndex = lstScriptActions.SelectedItems[0].Index;
            }
            else
            {
                //nothing is selected
                selectedIndex = -1;
            }

            if (appSettings.ClientSettings.ShowScriptMiniMap)
            {
                CreateMiniMap();
            }
        }

        private void lstScriptActions_KeyDown(object sender, KeyEventArgs e)
        {
            //delete from listview if required
            if (e.KeyCode == Keys.Delete)
            {
                DeleteRows();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                //if user presses enter simulate double click event
                lstScriptActions_DoubleClick(null, null);
            }
            else if ((e.Control) && (e.KeyCode == Keys.X))
            {
                CutRows();
            }
            else if ((e.Control) && (e.KeyCode == Keys.C))
            {
                CopyRows();
            }
            else if ((e.Control) && (e.KeyCode == Keys.V))
            {
                PasteRows();
            }
            else if ((e.Control) && (e.KeyCode == Keys.Z))
            {
                UndoChange();
            }
            else if ((e.Control) && (e.KeyCode == Keys.R))
            {
                RedoChange();
            }
            else if ((e.Control) && (e.Shift) && (e.KeyCode == Keys.E))
            {
                lstScriptActions_DoubleClick(null, null);
            }
            else if ((e.Control) && (e.KeyCode == Keys.E))
            {
                SetSelectedCodeToCommented(false);
            }
            else if ((e.Control) && (e.KeyCode == Keys.D))
            {
                SetSelectedCodeToCommented(true);
            }
            else if ((e.Control) && (e.KeyCode == Keys.A))
            {
                SelectAllRows();
            }
            else if ((e.Control) && (e.Shift) && (e.KeyCode == Keys.F))
            {
                // highlight this command
                HighlightAllCurrentSelectedCommand();
            }
            else if (e.KeyCode == Keys.F1)
            {
                if (lstScriptActions.SelectedItems.Count > 0)
                {
                    showThisCommandHelp((ScriptCommand)lstScriptActions.SelectedItems[0].Tag);
                }
            }
        }

        private void lstScriptActions_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (lstScriptActions.FocusedItem.Bounds.Contains(e.Location) == true)
                {
                    if (lstScriptActions.SelectedIndices.Count == 0)
                    {
                        // no selected command
                        insertCommentToolStripMenuItem.Visible = false;
                    }
                    else if (lstScriptActions.SelectedIndices[0] == 0)
                    {
                        // when select line 1, can not insert comment line 0
                        aboveHereToolStripMenuItem.Enabled = false;
                        insertCommentToolStripMenuItem.Visible = true;
                    }
                    else
                    {
                        aboveHereToolStripMenuItem.Enabled = true;
                        insertCommentToolStripMenuItem.Visible = true;
                    }

                    if ((ScriptCommand)lstScriptActions.SelectedItems[0].Tag is EnterKeysCommand)
                    {
                        multiSendKeystrokesEditToolStripMenuItem.Visible = true;
                    }
                    else
                    {
                        multiSendKeystrokesEditToolStripMenuItem.Visible = false;
                    }

                    lstContextStrip.Show(Cursor.Position);
                }
            }
        }
        private void lstScriptActions_DoubleClick(object sender, EventArgs e)
        {
            EditSelectedCommand();
        }
        #endregion


        #region lstContextStrip click event handlers
        private void disableSelectedCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSelectedCodeToCommented(true);
        }

        private void enableSelectedCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSelectedCodeToCommented(false);
        }

        private void pauseBeforeExecutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetPauseBeforeExecution();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectAllRows();
        }

        private void cutSelectedActionssToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CutRows();
        }

        private void copySelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyRows();
        }

        private void pasteSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PasteRows();
        }

        private void deleteSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteRows();
        }

        private void aboveHereToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstScriptActions.SelectedIndices.Count > 0)
            {
                int index = lstScriptActions.SelectedIndices[0];
                ClearSelectedListViewItems();

                if (index >= 1)
                {
                    index--;
                }
                lstScriptActions.Items[index].Selected = true;

                bool currentInsertMode = appSettings.ClientSettings.InsertCommandsInline;
                appSettings.ClientSettings.InsertCommandsInline = true;
                AddNewCommand("Misc Commands - Add Code Comment");
                appSettings.ClientSettings.InsertCommandsInline = currentInsertMode;
            }
        }

        private void belowHereToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstScriptActions.SelectedIndices.Count > 0)
            {
                int index = lstScriptActions.SelectedIndices[0];
                ClearSelectedListViewItems();
                lstScriptActions.Items[index].Selected = true;

                bool currentInsertMode = appSettings.ClientSettings.InsertCommandsInline;
                appSettings.ClientSettings.InsertCommandsInline = true;
                AddNewCommand("Misc Commands - Add Code Comment");
                appSettings.ClientSettings.InsertCommandsInline = currentInsertMode;
            }
        }
        private void moveToParentToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //create command list
            var commandList = new List<ScriptCommand>();

            //loop each
            for (int i = lstScriptActions.SelectedItems.Count - 1; i >= 0; i--)
            {
                //add to list and remove existing
                commandList.Add((ScriptCommand)lstScriptActions.SelectedItems[i].Tag);
                lstScriptActions.Items.Remove(lstScriptActions.SelectedItems[i]);
            }

            //reverse commands only if not inserting inline
            if (!appSettings.ClientSettings.InsertCommandsInline)
            {
                commandList.Reverse();
            }

            //add to parent
            commandList.ForEach(x => parentBuilder.AddCommandToListView(x));
        }

        private void editThisCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lstScriptActions_DoubleClick(null, null);
        }

        private void multiSendKeystrokesEditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var commands = Supplemental.frmMultiSendKeystrokes.GetConsecutiveSendKeystrokesCommands(lstScriptActions, appSettings);
            if (commands.Count == 0)
            {
                return;
            }

            using (var fm = new Supplemental.frmMultiSendKeystrokes(appSettings, scriptVariables, commands))
            {
                if (fm.ShowDialog() == DialogResult.OK)
                {
                    ChangeSaveState(true);

                    var newCommands = fm.SendKeystrokesCommands();

                    // remove current
                    int selectedIndex = lstScriptActions.SelectedIndices[0];
                    for (int i = selectedIndex + commands.Count - 1; i >= selectedIndex; i--)
                    {
                        lstScriptActions.Items.RemoveAt(i);
                    }

                    lstScriptActions.BeginUpdate();

                    int idx = selectedIndex;
                    foreach (var cmd in newCommands)
                    {
                        var lstCommand = CreateScriptCommandListViewItem(cmd);
                        lstScriptActions.Items.Insert(idx, lstCommand);
                        idx++;
                    }

                    lstScriptActions.EndUpdate();
                }
            }
        }

        private void whereThisCommandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchSelectedCommand();
        }

        private void ViewJSONCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var currentCommand = lstScriptActions.SelectedItems[0].Tag;

            var jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(currentCommand, new Newtonsoft.Json.JsonSerializerSettings() { TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All });

            using (var dialog = new UI.Forms.Supplemental.frmDialog(jsonText, "JSON Command Code", UI.Forms.Supplemental.frmDialog.DialogType.OkOnly, 0))
            {
                dialog.ShowDialog();
            }
        }

        private void ViewXMLCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var currentCommand = (ScriptCommand)lstScriptActions.SelectedItems[0].Tag;

            string scriptXML = Core.Script.Script.SerializeScript(new List<ScriptCommand>() { currentCommand }, scriptSerializer);

            int startIdx = scriptXML.IndexOf("<ScriptCommand ");

            int endIdx = scriptXML.IndexOf("</ScriptCommand>");
            if (endIdx < 0)
            {
                endIdx = scriptXML.IndexOf("</ScriptAction>");
            }

            string commandXML = scriptXML.Substring(startIdx, endIdx - startIdx - 1).Trim();

            using (var dialog = new UI.Forms.Supplemental.frmDialog(commandXML, "XML Command Code", UI.Forms.Supplemental.frmDialog.DialogType.OkOnly, 0))
            {
                dialog.ShowDialog();
            }
        }
        private void searchThisCommnadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HighlightAllCurrentSelectedCommand();
        }

        private void clearHighlightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearHighlightListViewItem();
        }

        private void helpThisCommandToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            BeginShowThisCommandHelpProcess();
        }

        private void showScriptInfoMenuItem_Click(object sender, EventArgs e)
        {
            showScriptInformationForm();
        }

        private void variableManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showVariableManager();
        }
        #endregion

        #endregion

        #endregion

        #region Bottom Notification Panel
        List<string> notificationList = new List<string>();
        private DateTime notificationExpires;
        private bool isDisplaying;
        public string notificationText { get; set; }

        private void tmrNotify_Tick(object sender, EventArgs e)
        {
            if (appSettings ==  null)
            {
                return;
            }

            if (appSettings.ClientSettings.HideNotifyAutomatically)
            {
                if ((notificationExpires < DateTime.Now) && (isDisplaying))
                {
                    HideNotification();
                }

                if ((appSettings.ClientSettings.AntiIdleWhileOpen) && (DateTime.Now > lastAntiIdleEvent.AddMinutes(1)))
                {
                    PerformAntiIdle();
                }

                //check if notification is required
                if ((notificationList.Count > 0) && (notificationExpires < DateTime.Now))
                {
                    var itemToDisplay = notificationList[0];
                    notificationList.RemoveAt(0);
                    notificationExpires = DateTime.Now.AddSeconds(2);
                    ShowNotification(itemToDisplay);
                }
            }
        }
        public void Notify(string notificationText)
        {
            // DBG
            //MessageBox.Show(notificationText);

            if (appSettings.ClientSettings.HideNotifyAutomatically)
            {
                notificationList.Add(notificationText);
            }
            else
            {
                this.notificationText = notificationText;
                pnlStatus_Paint(pnlStatus, new PaintEventArgs(pnlStatus.CreateGraphics(), pnlStatus.Bounds));    // force draw
            }
        }
        private void ShowNotification(string textToDisplay)
        {
            notificationText = textToDisplay;

            pnlStatus.SuspendLayout();

            ShowNotificationRow();
            pnlStatus.ResumeLayout();

            isDisplaying = true;
        }

        private void HideNotification()
        {
            pnlStatus.SuspendLayout();
            HideNotificationRow();
            pnlStatus.ResumeLayout();

            isDisplaying = false;
        }

        private void HideNotificationRow()
        {
            tlpControls.RowStyles[5].Height = 0;
        }
        private void ShowNotificationRow()
        {
            tlpControls.RowStyles[5].Height = 30;
        }
        
        private void pnlStatus_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.FromArgb(59, 59, 59));
            e.Graphics.DrawString(notificationText, pnlStatus.Font, Brushes.White, 30, 4);
            e.Graphics.DrawImage(Properties.Resources.message, 5, 3, 24, 24);
        }

        private void notifyTray_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (appSettings.ClientSettings.MinimizeToTray)
            {
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;

                if (appSettings.ClientSettings.HideNotifyAutomatically)
                {
                    notifyTray.Visible = false;
                }
            }
        }

        private void pnlStatus_DoubleClick(object sender, EventArgs e)
        {
            using (var fm = new UI.Forms.Supplemental.frmDialog(notificationText, "Status Message", UI.Forms.Supplemental.frmDialog.DialogType.OkOnly, 0))
            {
                fm.ShowDialog();
            }
            if (appSettings.ClientSettings.HideNotifyAutomatically)
            {
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
                notifyTray.Visible = false;
            }
        }

        private void frmScriptBuilder_SizeChanged(object sender, EventArgs e)
        {
            lstScriptActions.Columns[2].Width = this.Width - 340;
        }
        #endregion

        #region Auto Save Timer & Script File

        private void autoSaveTimer_Tick(object sender, EventArgs e)
        {
            if (this.dontSaveFlag)
            {
                // DBG
                //Console.WriteLine("now autosave");

                (var autoSavePath, var saveTime) = Script.GetAutoSaveScriptFilePath();

                //serialize script
                try
                {
                    scriptInfo.TasktVersion = Application.ProductVersion;
                    Core.Script.Script.SerializeScript(lstScriptActions.Items, scriptVariables, scriptInfo, appSettings.EngineSettings, scriptSerializer, autoSavePath);
                    Notify("Script automatically saved. (" + saveTime + ")");
                }
                catch (Exception ex)
                {
                    Notify("Auto Save Error: " + ex.ToString());
                }
            }
        }

        private void SetAutoSaveState()
        {
            if (appSettings.ClientSettings.EnabledAutoSave)
            {
                autoSaveTimer.Enabled = this.dontSaveFlag;
                if (this.dontSaveFlag)
                {
                    autoSaveTimer.Interval = appSettings.ClientSettings.AutoSaveInterval * 60000;
                    // DBG
                    //autoSaveTimer.Interval = appSettings.ClientSettings.AutoSaveInterval * 1000;
                    autoSaveTimer.Start();
                }
                else
                {
                    autoSaveTimer.Stop();
                }
            }
        }

        private void RemoveOldAutoSavedFiles()
        {
            RemoveOldScriptFiles(Script.GetAutoSaveFolderPath(), appSettings.ClientSettings.RemoveAutoSaveFileDays);
        }

        private void RemoveOldRunWithoutSavingScriptFiles()
        {
            RemoveOldScriptFiles(Script.GetRunWithoutSavingFolderPath(), appSettings.ClientSettings.RemoveRunWithtoutSavingFileDays);
        }

        private void RemoveOldScriptFiles(string folderPath, int days)
        {
            var files = System.IO.Directory.GetFiles(folderPath, "*.xml");
            foreach (var fp in files)
            {
                var info = new System.IO.FileInfo(fp);
                var diff = DateTime.Now - info.CreationTime;
                if (diff.TotalDays >= days)
                {
                    System.IO.File.Delete(fp);
                }
            }
        }

        #endregion

        #region Open, Save, Parse, Import, Validate File
        private void BeginOpenScriptProcess()
        {
            CheckAndSaveScriptIfForget();

            //show ofd
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Core.IO.Folders.GetFolder(Core.IO.Folders.FolderType.ScriptsFolder);
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Filter = "Xml (*.xml)|*.xml";

                //if user selected file
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //open file
                    OpenFile(openFileDialog.FileName);
                }
            }
        }

        private void CheckAndSaveScriptIfForget()
        {
            if (this.dontSaveFlag)
            {
                if (MessageBox.Show("This script has not been saved yet.\nDo you save it?", "taskt", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {

                    BeginSaveScriptProcess((ScriptFilePath == ""));
                }
            }
        }

        private void OpenFile(string filePath)
        {
            // check file exists
            if (!System.IO.File.Exists(filePath))
            {
                using(var fm = new UI.Forms.Supplemental.frmDialog(filePath + " does not exits.", "Open Error", UI.Forms.Supplemental.frmDialog.DialogType.OkOnly, 0))
                {
                    fm.ShowDialog();
                    return;
                }
            }

            try
            {
                //reinitialize
                lstScriptActions.Items.Clear();
                scriptVariables = new List<ScriptVariable>();
                scriptInfo = null;
                instanceList = new Core.InstanceCounter(appSettings);

                //get deserialized script
                Script deserializedScript = Core.Script.Script.DeserializeFile(filePath, appSettings.EngineSettings, scriptSerializer);

                if (deserializedScript.Commands.Count == 0)
                {
                    Notify("Error Parsing File: Commands not found!");
                }

                //update file path and reflect in title bar
                this.ScriptFilePath = filePath;

                //assign variables
                scriptVariables = deserializedScript.Variables;

                //script information
                scriptInfo = deserializedScript.Info;
                if (scriptInfo == null)
                {
                    scriptInfo = new ScriptInformation();
                }

                lstScriptActions.BeginUpdate();

                //populate commands
                PopulateExecutionCommands(deserializedScript.Commands);

                // validate
                CheckValidateCommands(deserializedScript.Commands.Select(t => t.ScriptCommand).ToList());

                lstScriptActions.EndUpdate();

                // check indent
                IndentListViewItems();

                // validate
                //CheckValidateScriptFile();

                // Instance Count
                AddInstanceName(deserializedScript.Commands.Select(t => t.ScriptCommand).ToList());

                //format listview
                ChangeSaveState(false);

                //notify
                Notify("Script Loaded Successfully!");

                // release
                deserializedScript = null;
            }
            catch (Exception ex)
            {
                // DBG
                //MessageBox.Show(ex.Message);
                //signal an error has happened
                Notify("An Error Occured: " + ex.Message);
            }
        }

        private void BeginImportProcess()
        {
            //show ofd
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Core.IO.Folders.GetFolder(Core.IO.Folders.FolderType.ScriptsFolder);
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Filter = "Xml (*.xml)|*.xml";

                //if user selected file
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //import
                    Cursor.Current = Cursors.WaitCursor;
                    Import(openFileDialog.FileName);
                    Cursor.Current = Cursors.Default;
                }
            }
        }
        private void Import(string filePath)
        {
            // check file exists
            if (!System.IO.File.Exists(filePath))
            {
                using (var fm = new UI.Forms.Supplemental.frmDialog(filePath + " does not exits.", "Open Error", UI.Forms.Supplemental.frmDialog.DialogType.OkOnly, 0))
                {
                    fm.ShowDialog();
                    return;
                }
            }

            try
            {
                //deserialize file      
                Script deserializedScript = Core.Script.Script.DeserializeFile(filePath, appSettings.EngineSettings, scriptSerializer);

                if (deserializedScript.Commands.Count == 0)
                {
                    Notify("Error Parsing File: Commands not found!");
                }

                //variables for comments
                var fileName = new System.IO.FileInfo(filePath).Name;
                var dateTimeNow = DateTime.Now.ToString();

                lstScriptActions.BeginUpdate();

                //comment
                lstScriptActions.Items.Add(CreateScriptCommandListViewItem(new CommentCommand() { v_Comment = "Imported From " + fileName + " @ " + dateTimeNow }));
                //import
                PopulateExecutionCommands(deserializedScript.Commands, false);
                foreach (ScriptVariable var in deserializedScript.Variables)
                {
                    if (scriptVariables.Find(alreadyExists => alreadyExists.VariableName == var.VariableName) == null)
                    {
                        scriptVariables.Add(var);
                    }
                }

                // validate imported commands
                CheckValidateCommands(deserializedScript.Commands.Select(t => t.ScriptCommand).ToList());

                //comment
                lstScriptActions.Items.Add(CreateScriptCommandListViewItem(new CommentCommand() { v_Comment = "End Import From " + fileName + " @ " + dateTimeNow }));

                lstScriptActions.EndUpdate();

                // count instance name
                AddInstanceName(deserializedScript.Commands.Select(t => t.ScriptCommand).ToList());

                ChangeSaveState(true);

                // check indent
                IndentListViewItems();

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

        public void PopulateExecutionCommands(List<ScriptAction> commandDetails, bool isOpen = true)
        {
            foreach (ScriptAction item in commandDetails)
            {
                lstScriptActions.Items.Add(CreateScriptCommandListViewItem(item.ScriptCommand, isOpen));
                if (item.AdditionalScriptCommands.Count > 0)
                {
                    PopulateExecutionCommands(item.AdditionalScriptCommands);
                }
            }

            if (pnlCommandHelper.Visible)
            {
                pnlCommandHelper.Hide();
            }
        }
        public int InsertExecutionCommands(List<ScriptAction> commandDetails, int position = -1)
        {
            if (position < 0)
            {
                if (lstScriptActions.SelectedIndices.Count == 0)
                {
                    position = lstScriptActions.Items.Count - 1;
                }
                else
                {
                    position = lstScriptActions.SelectedIndices[0];
                }
            }
            foreach (ScriptAction item in commandDetails)
            {
                lstScriptActions.Items.Insert(position + 1, CreateScriptCommandListViewItem(item.ScriptCommand));
                position++;
                if (item.AdditionalScriptCommands.Count > 0)
                {
                    position = InsertExecutionCommands(item.AdditionalScriptCommands, position);
                }
            }
            return position;
        }

        private void BeginSaveScriptProcess(bool SaveAs)
        {
            //clear selected items
            ClearSelectedListViewItems();
            SaveToFile(SaveAs);
        }

        private List<ScriptCommand> GetConfiguredCommands()
        {
            List<ScriptCommand> ConfiguredCommands = new List<ScriptCommand>();
            foreach (ListViewItem item in lstScriptActions.Items)
            {
                ConfiguredCommands.Add(item.Tag as ScriptCommand);
            }

            return ConfiguredCommands;
        }

        private void SaveToFile(bool saveAs)
        {
            if (lstScriptActions.Items.Count == 0)
            {
                Notify("You must have at least 1 automation command to save.");
                return;
            }

            int beginLoopValidationCount = 0;
            int beginIfValidationCount = 0;
            int tryCatchValidationCount = 0;
            foreach (ListViewItem item in lstScriptActions.Items)
            {
                if ((item.Tag is BeginListLoopCommand) || (item.Tag is BeginContinousLoopCommand) ||(item.Tag is BeginNumberOfTimesLoopCommand) || (item.Tag is BeginLoopCommand) || (item.Tag is BeginMultiLoopCommand))
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
                else if(item.Tag is TryCommand)
                {
                    tryCatchValidationCount++;
                }
                else if (item.Tag is EndTryCommand)
                {
                    tryCatchValidationCount--;
                }

                if (tryCatchValidationCount < 0)
                {
                    Notify("Please verify the ordering of your try/catch blocks.");
                    return;
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

            //define default output path
            if ((this.ScriptFilePath == null) || (saveAs))
            {
                using (var saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.InitialDirectory = Core.IO.Folders.GetFolder(Core.IO.Folders.FolderType.ScriptsFolder);
                    saveFileDialog.RestoreDirectory = true;
                    saveFileDialog.Filter = "Xml (*.xml)|*.xml";

                    if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    this.ScriptFilePath = saveFileDialog.FileName;
                }
            }

            // check file path is only filename
            if (System.IO.Path.GetFileName(this.ScriptFilePath) == this.ScriptFilePath)
            {
                this.ScriptFilePath = appSettings.ClientSettings.RootFolder + "\\" + this.ScriptFilePath;
            }

            //serialize script
            try
            {
                scriptInfo.TasktVersion = Application.ProductVersion;
                var exportedScript = Core.Script.Script.SerializeScript(lstScriptActions.Items, scriptVariables, scriptInfo, appSettings.EngineSettings, scriptSerializer, this.ScriptFilePath);
                //show success dialog
                Notify("File has been saved successfully!");
                ChangeSaveState(false);
            }
            catch (Exception ex)
            {
                Notify("Save Error: " + ex.ToString());
            }
        }

        private void CheckValidateCommands(List<ScriptCommand> commands)
        {
            using (var fm = new frmCommandEditor(automationCommands, GetConfiguredCommands()))
            {
                fm.appSettings = this.appSettings;
                foreach (var cmd in commands)
                {
                    cmd.IsValidate(fm);
                }
            }
        }

        public bool ImportScriptFromFilePath(string filePath)
        {
            Import(filePath);
            return true;
        }

        public bool OpenScriptFromFilePath(string filePath, bool normalFileOpen = false)
        {
            CheckAndSaveScriptIfForget();
            OpenFile(filePath);
            if (normalFileOpen)
            {
                this.ScriptFilePath = filePath;
            }
            else
            {
                this.ScriptFilePath = null;
            }
            ChangeSaveState(!normalFileOpen);
            return true;
        }

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
            CheckAndSaveScriptIfForget();

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
            instanceList = new Core.InstanceCounter(appSettings);

            ChangeSaveState(false);
        }
        #endregion

        #region Run Script file
        private void BeginRunScriptProcess()
        {
            if (lstScriptActions.Items.Count == 0)
            {
                // MessageBox.Show("You must first build the script by adding commands!", "Please Build Script");
                Notify("You must first build the script by adding commands!");
                return;
            }


            if (ScriptFilePath == null)
            {
                //MessageBox.Show("You must first save your script before you can run it!", "Please Save Script");
                Notify("You must first save your script before you can run it!");
                return;
            }

            //clear selected items
            ClearSelectedListViewItems();

            Notify("Running Script..");

            frmScriptEngine newEngine = new frmScriptEngine(ScriptFilePath, this);
            newEngine.callBackForm = this;
            newEngine.Show();
        }
        #endregion

        #region misc?
        private void btnManageVariables_Click(object sender, EventArgs e)
        {
            showVariableManager();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var recorder = new UI.Forms.Supplemental.frmThickAppElementRecorder())
            {
                recorder.ShowDialog();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            for (int i = 30; i > 0; i--)
            {
                tlpControls.RowStyles[4].Height = i;
            }
        }

        private void uiBtnAbout_Click(object sender, EventArgs e)
        {
            using (var frmAboutForm = new Supplemental.frmAbout())
            {
                frmAboutForm.ShowDialog();
            }
        }

        private void PerformAntiIdle()
        {

            lastAntiIdleEvent = DateTime.Now;
            var mouseMove = new MoveMouseCommand();
            mouseMove.v_XMousePosition = (Cursor.Position.X + 1).ToString();
            mouseMove.v_YMousePosition = (Cursor.Position.Y + 1).ToString();
            Notify("Anti-Idle Triggered");
        }
        #endregion

        #region Create Command Logic
        private void AddNewCommand(string specificCommand = "")
        {
            // DBG
            //MessageBox.Show(specificCommand);

            //bring up new command configuration form
            using (var newCommandForm = new frmCommandEditor(automationCommands, GetConfiguredCommands(), this.bufferedCommandList, this.bufferedCommandTreeImages))
            {
                newCommandForm.creationMode = frmCommandEditor.CreationMode.Add;
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

                //if a command was selected
                if (newCommandForm.ShowDialog(this) == DialogResult.OK)
                {
                    ChangeSaveState(true);

                    //add to listview
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
            return Core.CommandsTreeControls.GetSelectedFullCommandName(tvCommands);
        }
        #endregion

        #region TreeView Events
        private void GenerateTreeViewCommands()
        {
            bufferedCommandList = Core.CommandsTreeControls.CreateAllCommandsArray(appSettings.ClientSettings);
            bufferedCommandTreeImages = Core.CommandsTreeControls.CreateCommandImageList();
            tvCommands.ImageList = bufferedCommandTreeImages;

            ShowAllCommands();
        }

        private void tvCommands_DoubleClick(object sender, EventArgs e)
        {
            BeforeAddNewCommandProcess();
        }

        private void tvCommands_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                tvCommands_DoubleClick(this, null);
            }
        }

        private void tvCommands_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                tvCommands.SelectedNode = e.Node;
            }
        }

        private void tvCommands_MouseClick(object sender, MouseEventArgs e)
        {
            if (tvCommands.SelectedNode == null)
            {
                return;
            }

            if (e.Button == MouseButtons.Right)
            {
                if (tvCommands.SelectedNode.Level == 0)
                {
                    if (tvCommands.SelectedNode.IsExpanded)
                    {
                        expandRootTVCommandMenuStrip.Visible = false;
                        collapseRootTVCommandMenuStrip.Visible = true;
                    }
                    else
                    {
                        expandRootTVCommandMenuStrip.Visible = true;
                        collapseRootTVCommandMenuStrip.Visible = false;
                    }
                    rootTVCommandMenuStrip.Show(Cursor.Position);
                }
                else
                {
                    if (tvCommands.SelectedNode.Nodes.Count > 0)
                    {
                        if (tvCommands.SelectedNode.IsExpanded)
                        {
                            expandRootTVCommandMenuStrip.Visible = false;
                            collapseRootTVCommandMenuStrip.Visible = true;
                        }
                        else
                        {
                            expandRootTVCommandMenuStrip.Visible = true;
                            collapseRootTVCommandMenuStrip.Visible = false;
                        }
                        rootTVCommandMenuStrip.Show(Cursor.Position);
                    }
                    else
                    {
                        cmdTVCommandMenuStrip.Show(Cursor.Position);
                    }
                }
            }
            else if (e.Button == MouseButtons.Left)
            {
                var trg = tvCommands.HitTest(e.X, e.Y);
                if (trg.Location.ToString() == "Image")
                {
                    var node = trg.Node;
                    if (node.Nodes.Count > 0)
                    {
                        if (node.IsExpanded)
                        {
                            node.Collapse();
                        }
                        else
                        {
                            node.Expand();
                        }
                    }
                }
                //Console.WriteLine(trg.Location.ToString() + ", " + trg.Node.Text);
            }
        }
        private void picCommandSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtCommandFilter.Text.ToLower().Trim();
            if (keyword == "")
            {
                ShowAllCommands();
            }
            else
            {
                ShowFilterCommands(keyword);
            }
        }
        private void picCommandClear_Click(object sender, EventArgs e)
        {
            ShowAllCommands();
        }
        private void txtCommandFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;

                string keyword = txtCommandFilter.Text.ToLower().Trim();
                if (keyword == "")
                {
                    ShowAllCommands();
                }
                else
                {
                    ShowFilterCommands(keyword);
                }
            }
        }
        private void ShowFilterCommands(string keyword)
        {
            TreeNode[] filterdCommands = Core.CommandsTreeControls.FilterCommands(keyword, bufferedCommandList, appSettings.ClientSettings);

            Core.CommandsTreeControls.ShowCommandsTree(tvCommands, filterdCommands, true);

            clearCmdTVCommandMenuStrip.Enabled = true;
            clearRootTVCommandMenuStrip.Enabled = true;
        }
        private void ShowAllCommands()
        {
            txtCommandFilter.Text = "";

            Core.CommandsTreeControls.ShowCommandsTree(tvCommands, bufferedCommandList);

            clearCmdTVCommandMenuStrip.Enabled = false;
            clearRootTVCommandMenuStrip.Enabled = false;
        }

        private void SearchSelectedCommand()
        {
            if (lstScriptActions.SelectedItems.Count == 0)
            {
                return;
            }

            var command = (ScriptCommand)lstScriptActions.SelectedItems[0].Tag;
            var tp = command.GetType();
            var group = (Core.Automation.Attributes.ClassAttributes.Group)tp.GetCustomAttribute(typeof(Core.Automation.Attributes.ClassAttributes.Group));

            Core.CommandsTreeControls.FocusCommand(group.groupName, command.SelectionName, tvCommands);
        }

        #endregion

        #region Variable Edit, Settings form
        private void showVariableManager()
        {
            using (var scriptVariableEditor = new frmScriptVariables(this.scriptVariables, this.appSettings))
            {
                if (scriptVariableEditor.ShowDialog() == DialogResult.OK)
                {
                    this.scriptVariables = scriptVariableEditor.scriptVariables.OrderBy(v => v.VariableName).ToList();
                    ChangeSaveState(true);
                }
            }
        }

        private List<string> getAllVariablesNames()
        {
            List<string> variables = new List<string>();
            variables.AddRange(scriptVariables.Select(f => f.VariableName));
            variables.AddRange(Core.Common.GenerateSystemVariables().Select(f => f.VariableName));
            return variables;
        }

        private void showSettingForm()
        {
            //show settings dialog
            using (var newSettings = new Supplemental.frmSettings(this))
            {
                newSettings.ShowDialog();

                //reload app settings
                appSettings = new Core.ApplicationSettings();
                appSettings = appSettings.GetOrCreateApplicationSettings();

                //reinit
                Core.Server.HttpServerClient.Initialize();
            }
        }
        private void showNewSettingForm()
        {
            using (var newSettings = new Supplemental.frmNewSettings(this))
            {
                newSettings.ShowDialog();

                //reload app settings
                appSettings = new Core.ApplicationSettings();
                appSettings = appSettings.GetOrCreateApplicationSettings();

                //reinit
                Core.Server.HttpServerClient.Initialize();
            }
        }
        private void showScriptInformationForm()
        {
            using (var frm = new Supplemental.frmScriptInformations())
            {
                frm.infos = scriptInfo;
                frm.ShowDialog();
                ChangeSaveState(true);
            }
        }

        private void setCommandSearchBoxState()
        {
            var state = appSettings.ClientSettings.ShowCommandSearchBar;

            //set to empty
            tsSearchResult.Text = "";
            tsSearchBox.Text = "";

            //show or hide
            tsSearchBox.Visible = state;
            tsSearchButton.Visible = state;
            tsSearchResult.Visible = state;

            //update verbiage
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

        #region taskt About Form
        private void showAboutForm()
        {
            using (var aboutForm = new Supplemental.frmAbout())
            {
                aboutForm.ShowDialog();
            }
        }
        #endregion

        #region taskt header icon
        private void lblMainLogo_Click(object sender, EventArgs e)
        {
            showAboutForm();
        }
        #endregion

        #region pnlControlContainer event handler
        private void uiBtnNew_Click(object sender, EventArgs e)
        {
            BeginNewScriptProcess();
        }

        private void uiBtnOpen_Click(object sender, EventArgs e)
        {
            BeginOpenScriptProcess();
        }

        private void uiBtnImport_Click(object sender, EventArgs e)
        {
            BeginImportProcess();
        }

        private void uiBtnSave_Click(object sender, EventArgs e)
        {
            BeginSaveScriptProcess(false);
        }

        private void uiBtnSaveAs_Click(object sender, EventArgs e)
        {
            BeginSaveScriptProcess(true);
        }

        private void uiBtnAddVariable_Click(object sender, EventArgs e)
        {
            showVariableManager();
        }

        private void uiBtnSettings_Click(object sender, EventArgs e)
        {
            showSettingForm();
        }

        private void uiBtnClearAll_Click(object sender, EventArgs e)
        {
            HideSearchInfo();
            lstScriptActions.Items.Clear();
        }

        private void uiBtnRecordSequence_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (var sequenceRecorder = new Supplemental.frmSequenceRecorder())
            {
                sequenceRecorder.callBackForm = this;
                sequenceRecorder.ShowDialog();
            }

            pnlCommandHelper.Hide();

            this.Show();
            this.BringToFront();
        }

        private void uiBtnScheduleManagement_Click(object sender, EventArgs e)
        {
            using (var scheduleManager = new Supplemental.frmScheduleManagement())
            {
                scheduleManager.Show();
            }
        }

        private void uiBtnRunScript_Click(object sender, EventArgs e)
        {
            BeginRunScriptProcess();
        }

        private void pbSearch_Click(object sender, EventArgs e)
        {
            SearchForItemInListView();
        }

        private void uiBtnKeep_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void uiBtnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnSequenceImport_Click(object sender, EventArgs e)
        {
            BeginImportProcess();
        }
        #endregion


        #region MenuStrip1 click event handler

        #region File Actions menu event handler
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BeginNewScriptProcess();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BeginOpenScriptProcess();
        }

        private void importFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BeginImportProcess();
        }
        private void sampleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var fm = new Supplemental.frmSample(this))
            {
                fm.ShowDialog();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BeginSaveScriptProcess(false);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BeginSaveScriptProcess(true);
        }
        private void restartApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckAndSaveScriptIfForget();

            Application.Restart();
        }

        private void closeApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BeginFormCloseProcess();
            Application.Exit();
        }
        #endregion

        #region Edit menu items click handler
        private void editThisActionStripMenuItem_Click(object sender, EventArgs e)
        {
            EditSelectedCommand();
        }

        private void helpThisCommandStripMenuItem_Click(object sender, EventArgs e)
        {
            BeginShowThisCommandHelpProcess();
        }

        private void whereThisCommandToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SearchSelectedCommand();
        }

        private void enableSelectedActionsStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSelectedCodeToCommented(false);
        }

        private void disableSelectedActionsStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSelectedCodeToCommented(true);
        }

        private void pauseBeforeExeutionStripMenuItem_Click(object sender, EventArgs e)
        {
            SetPauseBeforeExecution();
        }

        private void SelectAllStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectAllRows();
        }
        private void CutScriptStripMenuItem_Click(object sender, EventArgs e)
        {
            CutRows();
        }

        private void CopyStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyRows();
        }

        private void PasteStripMenuItem_Click(object sender, EventArgs e)
        {
            PasteRows();
        }

        private void deleteStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteRows();
        }

        private void SearchStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSearch.CurrentMode = Supplemental.frmSearchCommands.SearchReplaceMode.Search;
            frmSearch.variables = getAllVariablesNames();
            frmSearch.Show();
        }

        private void ReplaceStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSearch.CurrentMode = Supplemental.frmSearchCommands.SearchReplaceMode.Replace;
            frmSearch.variables = getAllVariablesNames();
            frmSearch.Show();
            //MessageBox.Show("sorry, work in process ;-)");
        }
        private void highlightThisCommandStripMenuItem_Click(object sender, EventArgs e)
        {
            HighlightAllCurrentSelectedCommand();
        }
        private void clearSearchHighlightsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ClearHighlightListViewItem();
        }

        #endregion

        #region Options menu event handler
        private void variablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showVariableManager();
        }
        private void scriptInformationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showScriptInformationForm();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showSettingForm();
        }

        private void newSettigsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showNewSettingForm();
        }

        private void showScriptFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowScriptFolderProcess();
        }


        private void showLogFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowLogFolderProcess();
        }

        private void showSearchBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            appSettings.ClientSettings.ShowCommandSearchBar = !appSettings.ClientSettings.ShowCommandSearchBar;
            setCommandSearchBoxState();
        }

        private void guiInspectToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fm = new Supplement_Forms.frmGUIInspect();
            fm.Show();
        }

        private void jsonPathHelperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fm = new Supplement_Forms.frmJSONPathHelper();
            fm.Show();
        }

        private void showFormatCheckerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fm = new Supplement_Forms.frmFormatChecker();
            fm.Show();
        }
        #endregion

        #region Script Actions menu event handler
        private void recordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();

            using (var sequenceRecorder = new Supplemental.frmSequenceRecorder())
            {
                sequenceRecorder.callBackForm = this;
                sequenceRecorder.ShowDialog();
            }

            pnlCommandHelper.Hide();

            this.Show();
            this.BringToFront();
        }

        private void scheduleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var scheduleManager = new Supplemental.frmScheduleManagement();
            scheduleManager.Show();
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tempFilePath = Script.GetRunWithoutSavingScriptFilePath();
            var currentFilePath = this.ScriptFilePath;
            var currentDontSaveFlag = this.dontSaveFlag;

            this.ScriptFilePath = tempFilePath;
            saveAndRunToolStripMenuItem_Clicked(null, null);

            this.ScriptFilePath = currentFilePath;
            this.dontSaveFlag = currentDontSaveFlag;
            UpdateWindowTitle();
        }
        #endregion

        #region Save And Run menu event handler
        private void saveAndRunToolStripMenuItem_Clicked(object sender, EventArgs e)
        {
            if (scriptInfo.RunTimes != int.MaxValue)
            {
                scriptInfo.RunTimes++;
            }
            scriptInfo.LastRunTime = DateTime.Now;
            BeginSaveScriptProcess((this.ScriptFilePath == ""));
            BeginRunScriptProcess();
        }
        #endregion

        #region Help tool strip
        private void tasktProjectPageStripMenuItem_Click(object sender, EventArgs e)
        {
            showGitProjectPage();
        }

        private void tasktWikiStripMenuItem_Click(object sender, EventArgs e)
        {
            showWikiPage();
        }

        private void tasktGitterStripMenuItem_Click(object sender, EventArgs e)
        {
            showGitterPage();
        }

        private void releaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showGitReleasePage();
        }

        private void issueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showGitIssuePage();
        }

        private void aboutStripMenuItem_Click(object sender, EventArgs e)
        {
            showAboutForm();
        }
        #endregion

        #endregion

        #region Drag&Drop script file
        private void pnlCommandHelper_DragDrop(object sender, DragEventArgs e)
        {
            pnlCommandHelper.BackColor = Color.FromArgb(59, 59, 59);
            string[] fileNames = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (fileNames.Length > 0)
            {
                var targetFile = fileNames[0];
                if (System.IO.Path.GetExtension(targetFile).ToLower() == ".xml")
                {
                    if ((e.KeyState & 12) != 0) // Shift or Ctrl
                    {
                        ImportScriptFromFilePath(targetFile);
                    }
                    else
                    {
                        OpenScriptFromFilePath(targetFile, true);
                    }
                }
                else
                {
                    using(var fm = new UI.Forms.Supplemental.frmDialog("This file type can not open.", "File Open Error", UI.Forms.Supplemental.frmDialog.DialogType.OkOnly, 0))
                    {
                        fm.ShowDialog();
                    }
                }
            }
        }

        private void pnlCommandHelper_DragEnter(object sender, DragEventArgs e)
        {
            pnlCommandHelper.BackColor = Color.FromArgb(59, 59, 128);
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                if ((e.KeyState & 12) != 0) // Shift or Ctrl
                {
                    e.Effect = DragDropEffects.Copy;
                }
                else
                {
                    e.Effect = DragDropEffects.Move;
                }
            }
        }

        private void pnlCommandHelper_DragLeave(object sender, EventArgs e)
        {
            pnlCommandHelper.BackColor = Color.FromArgb(59, 59, 59);
        }
        #endregion

        private void frmScriptBuilder_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !BeginFormCloseProcess();
        }

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

            CheckAndSaveScriptIfForget();
            return true;
        }

        #region tvCommandMenuStrip events

        private void expandRootTVCommandMenuStrip_Click(object sender, EventArgs e)
        {
            tvCommands.SelectedNode.Expand();
        }

        private void collapseRootTVCommandMenuStrip_Click(object sender, EventArgs e)
        {
            tvCommands.SelectedNode.Collapse();
        }

        private void clearRootTVCommandMenuStrip_Click(object sender, EventArgs e)
        {
            ShowAllCommands();
        }

        private void addCmdTVCommandMenuStrip_Click(object sender, EventArgs e)
        {
            BeforeAddNewCommandProcess();
        }

        private void helpCmdTVCommandMenuStrip_Click(object sender, EventArgs e)
        {
            if (tvCommands.SelectedNode == null)
            {
                return;
            }
            string commandName = GetSelectedCommandFullName();
            if (commandName.Length == 0)
            {
                return;
            }
            var cmd = automationCommands.Where(t => t.FullName == commandName).FirstOrDefault();
            if (cmd != null)
            {
                showThisCommandHelp(cmd.Command);
            }
        }

        private void sampleThisCommandTVCommandMenuStrip_Click(object sender, EventArgs e)
        {
            if (tvCommands.SelectedNode == null)
            {
                return;
            }
            string commandName = GetSelectedCommandFullName().Split('-')[1].Trim();
            using (var frm = new Supplemental.frmSample(this, commandName))
            {
                frm.ShowDialog();
            }
        }

        private void highlightCmdTVCommandMenuStrip_Click(object sender, EventArgs e)
        {
            if (tvCommands.SelectedNode == null)
            {
                return;
            }
            string commandName = GetSelectedCommandFullName();
            if (commandName.Length == 0)
            {
                return;
            }
            var cmd = automationCommands.Where(t => t.FullName == commandName).FirstOrDefault();
            if (cmd != null)
            {
                AdvancedSearchItemInCommands(((ScriptCommand)cmd.Command).SelectionName, false, false, true, false, false, false, "");
            }
        }

        private void clearCmdTVCommandMenuStrip_Click(object sender, EventArgs e)
        {
            ShowAllCommands();
        }
        #endregion

        #region AttendedMode
        public void showAttendedModeFormProcess()
        {
            try
            {
                if (this.frmAttended == null)
                {
                    this.frmAttended = new frmAttendedMode();
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
                this.frmAttended = new frmAttendedMode();
                this.frmAttended.Show();
            }
        }
        #endregion

        #region show folders
        private void ShowScriptFolderProcess()
        {
            System.Diagnostics.Process.Start(appSettings.ClientSettings.RootFolder + "\\My Scripts");
        }

        private void ShowLogFolderProcess()
        {
            System.Diagnostics.Process.Start(appSettings.ClientSettings.RootFolder + "\\Logs");
        }

        #endregion

        #region Welcom Screen
        private void picRecentFiles_Click(object sender, EventArgs e)
        {
            BeginOpenScriptProcess();
        }
        #endregion

        #region CommandEditor
        public void SetCommandEditorSizeAndPosition(frmCommandEditor editor)
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
    }
}

