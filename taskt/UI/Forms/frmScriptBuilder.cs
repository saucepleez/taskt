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

using SuperSocket.ClientEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace taskt.UI.Forms
{

    public partial class frmScriptBuilder : Form
    //Form tracks the overall configuration and enables script editing, saving, and running
    //Features ability to add, drag/drop reorder commands
    {
        #region Instance and Form Events
        //private List<ListViewItem> rowsSelectedForCopy { get; set; }
        private List<Core.Script.ScriptVariable> scriptVariables;
        private Core.Script.ScriptInformation scriptInfo;
        private List<taskt.UI.CustomControls.AutomationCommand> automationCommands { get; set; }
        bool editMode { get; set; }
        private ImageList uiImages; 
        public Core.ApplicationSettings appSettings;
        private List<List<ListViewItem>> undoList;
        private DateTime lastAntiIdleEvent;
        private int undoIndex = -1;
        private int reqdIndex;
        private int selectedIndex = -1;

        private bool dontSaveFlag = false;

        public CommandEditorState currentScriptEditMode = CommandEditorState.Normal;
        private List<int> matchingSearchIndex = new List<int>();
        private int currentIndexInMatchItems = -1;
        private frmScriptBuilder parentBuilder { get; set; }

        private Pen indentDashLine;

        private Supplement_Forms.frmSearchCommands frmSearch = null;

        public enum CommandEditorState
        {
            Normal,
            Search,
            AdvencedSearch
        }

        public frmScriptBuilder()
        {
            InitializeComponent();
        }
        private string scriptFilePath;
        public string ScriptFilePath
        {
            get
            {
                return scriptFilePath;
            }
            set
            {
                scriptFilePath = value;
                UpdateWindowTitle();
            }
        }


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
        }

        private void frmScriptBuilder_Load(object sender, EventArgs e)
        {
            //load all commands
            automationCommands = taskt.UI.CustomControls.CommandControls.GenerateCommandsandControls();

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
            //Core.Sockets.SocketClient.Initialize();
            //Core.Sockets.SocketClient.associatedBuilder = this;


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
                using (UI.Forms.Supplemental.frmDialog userDialog = new UI.Forms.Supplemental.frmDialog("Would you like to create a folder to save your scripts in now? A script folder is required to save scripts generated with this application. The new script folder path would be '" + rpaScriptsFolder + "'.", "Unable to locate Script Folder!", UI.Forms.Supplemental.frmDialog.DialogType.YesNo, 0))
                {
                    if (userDialog.ShowDialog() == DialogResult.OK)
                    {
                        System.IO.Directory.CreateDirectory(rpaScriptsFolder);
                    }
                }
            }

            //get latest files for recent files list on load
            GenerateRecentFiles();

            //no height for status bar
            HideNotificationRow();

            //instantiate for script variables
            if (!editMode)
            {
                scriptVariables = new List<Core.Script.ScriptVariable>();
                scriptInfo = new Core.Script.ScriptInformation();
            }


            //pnlHeader.BackColor = Color.FromArgb(255, 214, 88);

            //instantiate and populate display icons for commands
            uiImages = UI.Images.UIImageList();

            //set image list
            lstScriptActions.SmallImageList = uiImages;


            //set listview column size
            frmScriptBuilder_SizeChanged(null, null);

            tvCommands.SuspendLayout();

            if (appSettings.ClientSettings.GroupingBySubgroup)
            {
                var groupedCommands = automationCommands.GroupBy(f => new { f.DisplayGroup, f.DisplaySubGroup })
                                    .OrderBy(f => f.Key.DisplayGroup);
                string prevPrimayGroup = "----";
                TreeNode pGroup = null;
                foreach (var primaryGroup in groupedCommands)
                {
                    if (primaryGroup.Key.DisplayGroup != prevPrimayGroup)
                    {
                        if (prevPrimayGroup != "----")
                        {
                            tvCommands.Nodes.Add(pGroup);
                        }
                        pGroup = new TreeNode(primaryGroup.Key.DisplayGroup);
                        prevPrimayGroup = primaryGroup.Key.DisplayGroup;
                    }

                    string prevSubGroup = "----";
                    TreeNode sGroup = null;
                    foreach (var cmd in primaryGroup)
                    {
                        if (cmd.DisplaySubGroup != prevSubGroup)
                        {
                            if (prevSubGroup != "----")
                            {
                                if (prevSubGroup != "")
                                {
                                    pGroup.Nodes.Add(sGroup);
                                }
                            }
                            prevSubGroup = cmd.DisplaySubGroup;

                            if (cmd.DisplaySubGroup != "")
                            {
                                sGroup = new TreeNode(cmd.DisplaySubGroup);
                            }
                        }

                        TreeNode subNode = new TreeNode(cmd.ShortName);
                        if (!cmd.Command.CustomRendering)
                        {
                            subNode.ForeColor = Color.Red;
                        }
                        if (cmd.DisplaySubGroup == "")
                        {
                            pGroup.Nodes.Add(subNode);
                        }
                        else
                        {
                            sGroup.Nodes.Add(subNode);
                        }
                    }
                    if (prevSubGroup != "")
                    {
                        pGroup.Nodes.Add(sGroup);
                    }
                }
                tvCommands.Nodes.Add(pGroup);
            }
            else
            {
                var groupedCommands = automationCommands.GroupBy(f => f.DisplayGroup);

                foreach (var cmd in groupedCommands)
                {
                    TreeNode newGroup = new TreeNode(cmd.Key);

                    foreach (var subcmd in cmd)
                    {
                        TreeNode subNode = new TreeNode(subcmd.ShortName);

                        if (!subcmd.Command.CustomRendering)
                        {
                            subNode.ForeColor = Color.Red;
                        }
                        newGroup.Nodes.Add(subNode);
                    }
                    tvCommands.Nodes.Add(newGroup);
                }
            }
            tvCommands.Sort();
            tvCommands.ResumeLayout();

            //tvCommands.ImageList = uiImages;

            //start attended mode if selected
            if (appSettings.ClientSettings.StartupMode == "Attended Task Mode")
            {

                this.WindowState = FormWindowState.Minimized;
                var frmAttended = new frmAttendedMode();
                frmAttended.Show();

            }

            this.dontSaveFlag = false;

            // set searchform
            frmSearch = new Supplement_Forms.frmSearchCommands();
            frmSearch.parentForm = this;

            // release
            GC.Collect();

        }
        
        private void frmScriptBuilder_Shown(object sender, EventArgs e)
        {

            taskt.Program.SplashForm.Hide();

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
                //Label noFilesLabel = new Label();
                //noFilesLabel.Text = "No Recent Files Found";
                //noFilesLabel.AutoSize = true;
                //noFilesLabel.ForeColor = Color.SteelBlue;
                //noFilesLabel.Font = lnkGitIssue.Font;
                //noFilesLabel.Margin = new Padding(0, 0, 0, 0);
                //flwRecentFiles.Controls.Add(noFilesLabel);
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
                        break;

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
            //System.Diagnostics.Process.Start("https://github.com/saucepleez/taskt");
            showGitProjectPage();
        }
        private void lnkGitLatestReleases_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //System.Diagnostics.Process.Start("https://github.com/saucepleez/taskt/releases");
            showGitReleasePage();
        }
        private void lnkGitIssue_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //System.Diagnostics.Process.Start("https://github.com/saucepleez/taskt/issues/new");
            showGitIssuePage();
        }
        private void lnkGitWiki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //System.Diagnostics.Process.Start("https://wiki.taskt.net/");
            showWikiPage();
        }
        private void NewFileLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel senderLink = (LinkLabel)sender;
            OpenFile(Core.IO.Folders.GetFolder(Core.IO.Folders.FolderType.ScriptsFolder) + senderLink.Text);
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
        #endregion


        #region ListView Events
        #region ListView DragDrop

        private void lstScriptActions_ItemDrag(object sender, ItemDragEventArgs e)
        {
            lstScriptActions.DoDragDrop(lstScriptActions.SelectedItems, DragDropEffects.Move);
        }

        private void lstScriptActions_DragEnter(object sender, DragEventArgs e)
        {
            int len = e.Data.GetFormats().Length - 1;
            int i;
            for (i = 0; i <= len; i++)
            {
                if (e.Data.GetFormats()[i].Equals("System.Windows.Forms.ListView+SelectedListViewItemCollection"))
                {
                    //The data from the drag source is moved to the target.
                    e.Effect = DragDropEffects.Move;
                }
            }
        }

        private void lstScriptActions_DragDrop(object sender, DragEventArgs e)
        {

            //Return if the items are not selected in the ListView control.
            if (lstScriptActions.SelectedItems.Count == 0)
            {
                return;
            }

            CreateUndoSnapshot();

            //Returns the location of the mouse pointer in the ListView control.
            Point cp = lstScriptActions.PointToClient(new Point(e.X, e.Y));
            //Obtain the item that is located at the specified location of the mouse pointer.
            ListViewItem dragToItem = lstScriptActions.GetItemAt(cp.X, cp.Y);
            if (dragToItem == null)
            {
                return;
            }

            //drag and drop for sequence
            if ((dragToItem.Tag is Core.Automation.Commands.SequenceCommand) && (appSettings.ClientSettings.EnableSequenceDragDrop))
            {
                //sequence command for drag drop
                var sequence = (Core.Automation.Commands.SequenceCommand)dragToItem.Tag;

                //add command to script actions
                for (int i = 0; i <= lstScriptActions.SelectedItems.Count - 1; i++)
                {
                    var command = (Core.Automation.Commands.ScriptCommand)lstScriptActions.SelectedItems[i].Tag;
                    sequence.v_scriptActions.Add(command);
                }

                //remove originals
                for (int i = lstScriptActions.SelectedItems.Count - 1; i >= 0 ; i--)
                {
                    lstScriptActions.Items.Remove(lstScriptActions.SelectedItems[i]);
                }

                //return back
                return;
            }




            //Obtain the index of the item at the mouse pointer.
            int dragIndex = dragToItem.Index;


            //foreach (ListViewItem command in lstScriptActions.SelectedItems)
            //{

            //    if (command.Tag is Core.Automation.Commands.EndLoopCommand)
            //    {
            //        for (int i = 0; i < dragIndex; i++)
            //        {
            //            if (lstScriptActions.Items[i].Tag is Core.Automation.Commands.BeginLoopCommand)
            //            {

            //            }
            //        }


            //    }

            //}



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
                    return;
                }
                if (dragItem.Index < itemIndex)
                    itemIndex++;
                else
                    itemIndex = dragIndex + i;
                //Insert the item at the mouse pointer.
                ListViewItem insertItem = (ListViewItem)dragItem.Clone();
                lstScriptActions.Items.Insert(itemIndex, insertItem);
                //Removes the item from the initial location while
                //the item is moved to the new location.
                lstScriptActions.Items.Remove(dragItem);
                //FormatCommandListView();
                lstScriptActions.Invalidate();
            }

        }

        #endregion

        #region ListView Search

        private void txtCommandSearch_TextChanged(object sender, EventArgs e)
        {
            if (lstScriptActions.Items.Count == 0)
                return;

            reqdIndex = 0;

            if (txtCommandSearch.Text == "")
            {
                //hide info
                HideSearchInfo();

                //clear indexes
                matchingSearchIndex.Clear();
                currentIndexInMatchItems = -1;

                //repaint
                lstScriptActions.Invalidate();
            }
            else
            {
                lblCurrentlyViewing.Show();
                lblTotalResults.Show();
                SearchForItemInListView();

                //repaint
                lstScriptActions.Invalidate();
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

            var matchingItems = (from ListViewItem itm in lstScriptActions.Items
                                 where itm.Text.Contains(searchCriteria)
                                 select itm).ToList();


            int? matchCount = matchingItems.Count();
            int totalMatches = matchCount ?? 0;


            if ((reqdIndex == matchingItems.Count) || (reqdIndex < 0))
            {
                reqdIndex = 0;
            }

            lblTotalResults.Show();

            if (totalMatches == 0)
            {
                currentScriptEditMode = CommandEditorState.Normal;
                reqdIndex = -1;
                lblTotalResults.Text = "No Matches Found";
                lblCurrentlyViewing.Hide();
                //clear indexes
                matchingSearchIndex.Clear();
                reqdIndex = -1;
                lstScriptActions.Invalidate();
                return;
            }
            else
            {
                currentScriptEditMode = CommandEditorState.Search;
                lblCurrentlyViewing.Text = "Viewing " + (reqdIndex + 1) + " of " + totalMatches + "";
                tsSearchResult.Text = "Viewing " + (reqdIndex + 1) + " of " + totalMatches + "";
                lblTotalResults.Text = totalMatches + " total results found";
            }

            ClearSelectedListViewItems();

            matchingSearchIndex = new List<int>();
            foreach (ListViewItem itm in matchingItems)
            {
                matchingSearchIndex.Add(itm.Index);
                //itm.BackColor = Color.LightGoldenrodYellow;
            }

            currentIndexInMatchItems = matchingItems[0].Index;
            lstScriptActions.Items[0].Selected = true;
            //currentIndexInMatchItems = matchingItems[reqdIndex].Index;

            lstScriptActions.Invalidate();

            //lstScriptActions.EnsureVisible(currentIndexInMatchItems);
        }

        private void pbSearch_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void pbSearch_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        public int AdvancedSearchItemInCommands(string keyword, bool caseSensitive, bool checkParameters, bool checkCommandName, bool checkComment, bool checkDisplayText)
        {
            matchingSearchIndex.Clear();
            matchingSearchIndex = new List<int>();
            int matchedCount = 0;

            this.currentIndexInMatchItems = -1;
            lstScriptActions.SuspendLayout();
            foreach(ListViewItem itm in lstScriptActions.Items)
            {
                if (((Core.Automation.Commands.ScriptCommand)itm.Tag).checkMatched(keyword, caseSensitive, checkParameters, checkCommandName, checkComment, checkDisplayText))
                {
                    matchedCount++;
                    matchingSearchIndex.Add(itm.Index);
                }
            }
            lstScriptActions.ResumeLayout();

            this.currentScriptEditMode = CommandEditorState.AdvencedSearch;
            lstScriptActions.Invalidate();
            return matchedCount;
        }

        public void MoveMostNearMatchedLine(bool backToTop)
        {
            if (this.currentScriptEditMode != CommandEditorState.AdvencedSearch)
            {
                return;
            }
            if (matchingSearchIndex.Count == 0)
            {
                return;
            }

            if (this.currentIndexInMatchItems >= 0)
            {
                // select matched item yet
                int nextIndex = this.currentIndexInMatchItems + 1;
                if (backToTop)
                {
                    nextIndex %= matchingSearchIndex.Count;
                }
                else
                {
                    if (nextIndex >= matchingSearchIndex.Count)
                    {
                        nextIndex--;
                    }
                }
                this.currentIndexInMatchItems = nextIndex;
                ClearSelectedListViewItems();

                lstScriptActions.Items[matchingSearchIndex[nextIndex]].Selected = true;
            }
            else
            {
                if (lstScriptActions.SelectedIndices.Count > 0)
                {
                    // some item selected yet. -> search near item
                    int currentIndex = lstScriptActions.SelectedIndices[0];
                    if (matchingSearchIndex[matchingSearchIndex.Count - 1] > currentIndex)
                    {
                        if (backToTop)
                        {
                            this.currentIndexInMatchItems = 0;
                            ClearSelectedListViewItems();
                            lstScriptActions.Items[matchingSearchIndex[0]].Selected = true;
                        }
                        else
                        {
                            ClearSelectedListViewItems();
                            return;
                        }
                    }
                    else
                    {
                        int targetIndex = matchingSearchIndex.Count - 1;
                        while (currentIndex > matchingSearchIndex[targetIndex])
                        {
                            targetIndex--;
                        }
                        targetIndex++;
                        this.currentIndexInMatchItems = targetIndex;
                        ClearSelectedListViewItems();
                        lstScriptActions.Items[matchingSearchIndex[targetIndex]].Selected = true;
                    }
                }
                else
                {
                    // no selected item -> select fist matched item
                    this.currentIndexInMatchItems = 0;
                    ClearSelectedListViewItems();
                    lstScriptActions.Items[matchingSearchIndex[0]].Selected = true;
                }
            }

            // scroll
            lstScriptActions.EnsureVisible(matchingSearchIndex[this.currentIndexInMatchItems]);
            lstScriptActions.Invalidate();
        }

        private void HighlightAllCurrentSelectedCommand()
        {
            if (lstScriptActions.SelectedIndices.Count > 0)
            {
                string keyword = ((Core.Automation.Commands.ScriptCommand)lstScriptActions.SelectedItems[0].Tag).SelectionName;
                AdvancedSearchItemInCommands(keyword, false, false, true, false, false);
            }
        }

        #endregion



        #region ListView Copy, Paste, Edit, Delete


        private void SelectAllRows()
        {
            foreach (ListViewItem itm in lstScriptActions.Items)
            {
                itm.Selected = true;
            }
        }

        private void DeleteRows()
        {
            foreach (ListViewItem itm in lstScriptActions.SelectedItems)
            {
                lstScriptActions.Items.Remove(itm);

                ChangeSaveState(true);
            }

            CreateUndoSnapshot();

            // check indent
            IndentListViewItems();

            lstScriptActions.Invalidate();
            //FormatCommandListView();
        }

        private void CutRows()
        {

            //initialize list of items to copy   
            //if (rowsSelectedForCopy == null)
            //{
            //    rowsSelectedForCopy = new List<ListViewItem>();
            //}
            //else
            //{
            //    rowsSelectedForCopy.Clear();
            //}

            //copy into list for all selected            
            if (lstScriptActions.SelectedItems.Count >= 1)
            {
                ChangeSaveState(true);

                var commands = new List<Core.Automation.Commands.ScriptCommand>();

                foreach (ListViewItem item in lstScriptActions.SelectedItems)
                {
                    commands.Add((Core.Automation.Commands.ScriptCommand)item.Tag);
                    //rowsSelectedForCopy.Add(item);
                    lstScriptActions.Items.Remove(item);
                }
                // set clipborad xml string
                Clipboard.SetText(taskt.Core.Script.Script.SerializeScript(commands));

                //Notify(rowsSelectedForCopy.Count + " item(s) cut to clipboard!");
                Notify(commands.Count + " item(s) cut to clipboard!");

                // release
                commands.Clear();
                commands = null;

                // check indent
                IndentListViewItems();
            }
        }

        private void CopyRows()
        {

            //initialize list of items to copy   
            //if (rowsSelectedForCopy == null)
            //{
            //    rowsSelectedForCopy = new List<ListViewItem>();
            //}
            //else
            //{
            //    rowsSelectedForCopy.Clear();
            //}

            //copy into list for all selected            
            if (lstScriptActions.SelectedItems.Count >= 1)
            {
                ChangeSaveState(true);

                var commands = new List<Core.Automation.Commands.ScriptCommand>();

                foreach (ListViewItem item in lstScriptActions.SelectedItems)
                {
                    commands.Add((Core.Automation.Commands.ScriptCommand)item.Tag);
                    //rowsSelectedForCopy.Add(item);
                }

                // set clipborad xml string
                Clipboard.SetText(taskt.Core.Script.Script.SerializeScript(commands));

                //Notify(rowsSelectedForCopy.Count + " item(s) copied to clipboard!");
                Notify(commands.Count + " item(s) copied to clipboard!");

                // release
                commands.Clear();
                commands = null;
            }
        }

        private void PasteRows()
        {

            //if (rowsSelectedForCopy != null)
            //{

            //    if (lstScriptActions.SelectedItems.Count == 0)
            //    {
            //        MessageBox.Show("In order to paste, you must first select a command to paste under.", "Select Command To Paste Under");
            //        return;
            //    }

            //    //int destinationIndex = lstScriptActions.SelectedItems[0].Index + 1;

            //    //foreach (ListViewItem item in rowsSelectedForCopy)
            //    //{
            //    //    Core.Automation.Commands.ScriptCommand duplicatedCommand = (Core.Automation.Commands.ScriptCommand)Core.Common.Clone(item.Tag);
            //    //    duplicatedCommand.GenerateID();
            //    //    lstScriptActions.Items.Insert(destinationIndex, CreateScriptCommandListViewItem(duplicatedCommand));
            //    //    destinationIndex += 1;                  
            //    //}

            //    //lstScriptActions.Invalidate();

            //    //Notify(rowsSelectedForCopy.Count + " item(s) pasted!");

            //    var sc = Core.Script.Script.DeserializeXML(Clipboard.GetText());
            //    //PopulateExecutionCommands(sc.Commands);
            //    InsertExecutionCommands(sc.Commands);
            //}

            var sc = Core.Script.Script.DeserializeXML(Clipboard.GetText());
            if (sc != null)
            {
                ChangeSaveState(true);

                InsertExecutionCommands(sc.Commands);

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
                var selectedCommand = (Core.Automation.Commands.ScriptCommand)item.Tag;
                selectedCommand.IsCommented = setCommented;
            }

            ChangeSaveState(true);

            //recolor
            lstScriptActions.Invalidate();

            //clear selection
            lstScriptActions.SelectedIndices.Clear();

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
                var selectedCommand = (Core.Automation.Commands.ScriptCommand)item.Tag;
                selectedCommand.PauseBeforeExeucution = !selectedCommand.PauseBeforeExeucution;
            }

            //recolor
            //FormatCommandListView();

            ChangeSaveState(true);

            lstScriptActions.Invalidate();

            //clear selection
            lstScriptActions.SelectedIndices.Clear();

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
            this.currentScriptEditMode = CommandEditorState.Normal;
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
            lstContextStripSep2.Visible = true;
            lstContextStripSep4.Visible = false;
            showScriptInfoMenuItem.Visible = false;
        }
        #endregion

        #region ListView Create Item

        private ListViewItem CreateScriptCommandListViewItem(Core.Automation.Commands.ScriptCommand cmdDetails)
        {
            ListViewItem newCommand = new ListViewItem();
            newCommand.Text = cmdDetails.GetDisplayValue();
            newCommand.SubItems.Add(cmdDetails.GetDisplayValue());
            newCommand.SubItems.Add(cmdDetails.GetDisplayValue());
            //cmdDetails.RenderedControls = null;
            newCommand.Tag = cmdDetails;
            newCommand.ForeColor = cmdDetails.DisplayForeColor;
            newCommand.BackColor = Color.DimGray;
            //newCommand.ImageIndex = uiImages.Images.IndexOfKey(cmdDetails.GetType().Name);
            newCommand.ImageIndex = taskt.UI.Images.GetUIImageList(cmdDetails.GetType().Name);
            return newCommand;
        }

        public void AddCommandToListView(Core.Automation.Commands.ScriptCommand selectedCommand)
        {
            if (pnlCommandHelper.Visible)
            {
                pnlCommandHelper.Hide();
            }

            var command = CreateScriptCommandListViewItem(selectedCommand);

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
                if ((selectedCommand is Core.Automation.Commands.BeginExcelDatasetLoopCommand) || (selectedCommand is Core.Automation.Commands.BeginListLoopCommand) || (selectedCommand is Core.Automation.Commands.BeginContinousLoopCommand) || (selectedCommand is Core.Automation.Commands.BeginNumberOfTimesLoopCommand) || (selectedCommand is Core.Automation.Commands.BeginLoopCommand) || (selectedCommand is Core.Automation.Commands.BeginMultiLoopCommand))
                {
                    lstScriptActions.Items.Insert(insertionIndex, CreateScriptCommandListViewItem(new Core.Automation.Commands.CommentCommand() { v_Comment = "Please enter a description of the loop here" }));
                    insertionIndex++;
                }
                else if((selectedCommand is Core.Automation.Commands.BeginIfCommand) || (selectedCommand is Core.Automation.Commands.BeginMultiIfCommand))
                {
                    lstScriptActions.Items.Insert(insertionIndex, CreateScriptCommandListViewItem(new Core.Automation.Commands.CommentCommand() { v_Comment = "Please enter a description of the if here" }));
                    insertionIndex++;
                }
                else if(selectedCommand is Core.Automation.Commands.TryCommand)
                {
                    lstScriptActions.Items.Insert(insertionIndex, CreateScriptCommandListViewItem(new Core.Automation.Commands.CommentCommand() { v_Comment = "Please enter a description of the error handling here" }));
                    insertionIndex++;
                }
            }

            //insert command
            lstScriptActions.Items.Insert(insertionIndex, command);

            var focusIndex = insertionIndex;

            //special types also get a following command and comment
            if ((selectedCommand is Core.Automation.Commands.BeginExcelDatasetLoopCommand) || (selectedCommand is Core.Automation.Commands.BeginListLoopCommand) || (selectedCommand is Core.Automation.Commands.BeginContinousLoopCommand) || (selectedCommand is Core.Automation.Commands.BeginNumberOfTimesLoopCommand) || (selectedCommand is Core.Automation.Commands.BeginLoopCommand) || (selectedCommand is Core.Automation.Commands.BeginMultiLoopCommand))
            {
                lstScriptActions.Items.Insert(insertionIndex + 1, CreateScriptCommandListViewItem(new Core.Automation.Commands.CommentCommand() { v_Comment = "Items in this section will run within the loop" }));
                lstScriptActions.Items.Insert(insertionIndex + 2, CreateScriptCommandListViewItem(new Core.Automation.Commands.EndLoopCommand()));
                focusIndex++;
            }
            else if ((selectedCommand is Core.Automation.Commands.BeginIfCommand) || (selectedCommand is Core.Automation.Commands.BeginMultiIfCommand))
            {
                if (appSettings.ClientSettings.InsertElseAutomatically)
                {
                    lstScriptActions.Items.Insert(insertionIndex + 1, CreateScriptCommandListViewItem(new Core.Automation.Commands.CommentCommand() { v_Comment = "Items in this section will run if the statement is true" }));
                    lstScriptActions.Items.Insert(insertionIndex + 2, CreateScriptCommandListViewItem(new Core.Automation.Commands.ElseCommand()));
                    lstScriptActions.Items.Insert(insertionIndex + 3, CreateScriptCommandListViewItem(new Core.Automation.Commands.CommentCommand() { v_Comment = "Items in this section will run if the statement is false" }));
                    lstScriptActions.Items.Insert(insertionIndex + 4, CreateScriptCommandListViewItem(new Core.Automation.Commands.EndIfCommand()));
                }
                else
                {
                    lstScriptActions.Items.Insert(insertionIndex + 1, CreateScriptCommandListViewItem(new Core.Automation.Commands.CommentCommand() { v_Comment = "Items in this section will run if the statement is true" }));
                    lstScriptActions.Items.Insert(insertionIndex + 2, CreateScriptCommandListViewItem(new Core.Automation.Commands.EndIfCommand()));
                }
                focusIndex++;
            }
            else if (selectedCommand is Core.Automation.Commands.TryCommand)
            {
                lstScriptActions.Items.Insert(insertionIndex + 1, CreateScriptCommandListViewItem(new Core.Automation.Commands.CommentCommand() { v_Comment = "Items in this section will be handled if error occurs" }));
                lstScriptActions.Items.Insert(insertionIndex + 2, CreateScriptCommandListViewItem(new Core.Automation.Commands.CatchExceptionCommand() { v_Comment = "Items in this section will run if error occurs" }));
                lstScriptActions.Items.Insert(insertionIndex + 3, CreateScriptCommandListViewItem(new Core.Automation.Commands.CommentCommand() { v_Comment = "This section executes if error occurs above" }));
                lstScriptActions.Items.Insert(insertionIndex + 4, CreateScriptCommandListViewItem(new Core.Automation.Commands.EndTryCommand()));
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

                if ((rowItem.Tag is Core.Automation.Commands.BeginIfCommand) || (rowItem.Tag is Core.Automation.Commands.BeginMultiIfCommand) || (rowItem.Tag is Core.Automation.Commands.BeginExcelDatasetLoopCommand) || (rowItem.Tag is Core.Automation.Commands.BeginListLoopCommand) || (rowItem.Tag is Core.Automation.Commands.BeginContinousLoopCommand) || (rowItem.Tag is Core.Automation.Commands.BeginNumberOfTimesLoopCommand) || (rowItem.Tag is Core.Automation.Commands.TryCommand) || (rowItem.Tag is Core.Automation.Commands.BeginLoopCommand) || (rowItem.Tag is Core.Automation.Commands.BeginMultiLoopCommand))
                {
                    indent += 2;
                    rowItem.IndentCount = indent;
                    indent += 2;
                }
                else if ((rowItem.Tag is Core.Automation.Commands.EndLoopCommand) || (rowItem.Tag is Core.Automation.Commands.EndIfCommand) || (rowItem.Tag is Core.Automation.Commands.EndTryCommand))
                {
                    indent -= 2;
                    if (indent < 0) indent = 0;
                    rowItem.IndentCount = indent;
                    indent -= 2;
                    if (indent < 0) indent = 0;
                }
                else if ((rowItem.Tag is Core.Automation.Commands.ElseCommand) || (rowItem.Tag is Core.Automation.Commands.CatchExceptionCommand) || (rowItem.Tag is Core.Automation.Commands.FinallyCommand))
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
        }
        private void AutoSizeLineNumberColumn()
        {
            //auto adjust column width based on # of commands
            int columnWidth = (14 * lstScriptActions.Items.Count.ToString().Length);
            lstScriptActions.Columns[0].Width = columnWidth;
        }

        private void lstScriptActions_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {

            //handle indents
            //IndentListViewItems();

            //auto size line numbers based on command count
            AutoSizeLineNumberColumn();


            //get listviewitem
            ListViewItem item = e.Item;

            //get script command reference
            var command = (Core.Automation.Commands.ScriptCommand)item.Tag;


            //create modified bounds
            var modifiedBounds = e.Bounds;
            //modifiedBounds.Y += 2;

            //switch between column index
            switch (e.ColumnIndex)
            {
                case 0:
                    //draw row number
                    e.Graphics.DrawString((e.ItemIndex + 1).ToString(),
                        lstScriptActions.Font, Brushes.LightSlateGray, modifiedBounds);
                    break;
                case 1:
                    //draw command icon
                    //var img = uiImages.Images[command.GetType().Name];
                    var img = taskt.UI.Images.GetUIImage(command.GetType().Name);
                    if (img != null)
                    {
                        e.Graphics.DrawImage(img, modifiedBounds.Left, modifiedBounds.Top + 3);
                    }

                    break;

                case 2:
                    ////write command text
                    ////Brush commandNameBrush, commandBackgroundBrush;
                    //taskt.Core.Theme.UIFont trg;

                    //if ((debugLine > 0) && (e.ItemIndex == debugLine - 1))
                    //{
                    //    //debugging coloring
                    //    //commandNameBrush = Brushes.White;
                    //    //commandBackgroundBrush = Brushes.OrangeRed;
                    //    trg = taskt.Core.Theme.scriptTexts["debug"];
                    //}
                    ////else if ((currentIndexInMatchItems >= 0) && (e.ItemIndex == currentIndexInMatchItems))
                    ////{
                    ////    //search primary item coloring
                    ////    //commandNameBrush = Brushes.Black;
                    ////    //commandBackgroundBrush = Brushes.Goldenrod;
                    ////    trg = taskt.Core.Theme.scriptTexts["current"];
                    ////}
                    //else if (matchingSearchIndex.Contains(e.ItemIndex) && currentScriptEditMode == EditMode.Search)
                    //{
                    //    //search match item coloring
                    //    //commandNameBrush = Brushes.Black;
                    //    //commandBackgroundBrush = Brushes.LightYellow;
                    //    if ((e.Item.Focused) || (e.Item.Selected))
                    //    {
                    //        trg = taskt.Core.Theme.scriptTexts["current-match"];
                    //    }
                    //    else
                    //    {
                    //        trg = taskt.Core.Theme.scriptTexts["match"];
                    //    }
                    //}
                    //else if (this.currentScriptEditMode == EditMode.AdvencedSearch)
                    //{
                    //    if (command.IsMatched)
                    //    {
                    //        if ((e.Item.Focused) || (e.Item.Selected))
                    //        {
                    //            trg = taskt.Core.Theme.scriptTexts["current-match"];
                    //        }
                    //        else
                    //        {
                    //            trg = taskt.Core.Theme.scriptTexts["match"];
                    //        }
                    //    }
                    //    else if ((command is Core.Automation.Commands.CommentCommand) || (command.IsCommented))
                    //    {
                    //        trg = taskt.Core.Theme.scriptTexts["comment"];
                    //    }
                    //    else if (!command.IsValid)
                    //    {
                    //        trg = taskt.Core.Theme.scriptTexts["invalid"];
                    //    }
                    //    else
                    //    {
                    //        trg = taskt.Core.Theme.scriptTexts["normal"];
                    //    }
                    //}
                    //else if ((e.Item.Focused) || (e.Item.Selected))
                    //{
                    //    //selected item coloring
                    //    if ((command is Core.Automation.Commands.CommentCommand) || (command.IsCommented))
                    //    {
                    //        // disable command
                    //        //commandNameBrush = Brushes.LightGreen;
                    //        trg = taskt.Core.Theme.scriptTexts["selected-comment"];
                    //    }
                    //    else if (command.PauseBeforeExeucution)
                    //    {
                    //        // pause
                    //        //commandNameBrush = Brushes.Plum;
                    //        trg = taskt.Core.Theme.scriptTexts["selected-pause"];
                    //    }
                    //    else if (!command.IsValid)
                    //    {
                    //        // invalid
                    //        //commandNameBrush = Brushes.Crimson;
                    //        trg = taskt.Core.Theme.scriptTexts["selected-invalid"];
                    //    }
                    //    else
                    //    {
                    //        //commandNameBrush = Brushes.White;
                    //        trg = taskt.Core.Theme.scriptTexts["selected-normal"];
                    //    }
                    //    //commandBackgroundBrush = Brushes.DodgerBlue;
                    //}
                    //else if (command.PauseBeforeExeucution)
                    //{
                    //    //pause before execution coloring
                    //    //commandNameBrush = Brushes.MediumPurple;
                    //    //commandBackgroundBrush = Brushes.Lavender;
                    //    trg = taskt.Core.Theme.scriptTexts["pause"];
                    //}
                    //else if ((command is Core.Automation.Commands.CommentCommand) || (command.IsCommented))
                    //{
                    //    //comments and commented command coloring
                    //    //commandNameBrush = Brushes.ForestGreen;
                    //    //commandBackgroundBrush = Brushes.WhiteSmoke;
                    //    trg = taskt.Core.Theme.scriptTexts["comment"];
                    //}
                    //else if (!command.IsValid)
                    //{
                    //    //standard coloring
                    //    //if (command.IsValid)
                    //    //{
                    //    //    //commandNameBrush = Brushes.SteelBlue;
                    //    //    trg = taskt.Core.Theme.scriptTexts["normal"];

                    //    //}
                    //    //else
                    //    //{
                    //    //    //commandNameBrush = Brushes.Crimson;
                    //    //    trg = taskt.Core.Theme.scriptTexts["invalid"];
                    //    //}
                    //    //commandBackgroundBrush = Brushes.WhiteSmoke;
                    //    trg = taskt.Core.Theme.scriptTexts["invalid"];
                    //}
                    //else
                    //{
                    //    trg = taskt.Core.Theme.scriptTexts["normal"];
                    //}

                    ////fille with background color
                    ////e.Graphics.FillRectangle(commandBackgroundBrush, modifiedBounds);
                    //e.Graphics.FillRectangle(new SolidBrush(trg.BackColor), modifiedBounds);

                    ////get indent count
                    //var indentPixels = (item.IndentCount * 15);

                    ////set indented X position
                    //modifiedBounds.X += indentPixels;

                    ////draw string
                    ////e.Graphics.DrawString(command.GetDisplayValue(),
                    ////               lstScriptActions.Font, commandNameBrush, modifiedBounds);
                    //e.Graphics.DrawString(command.GetDisplayValue(),
                    //               new Font(trg.Font, trg.FontSize, trg.Style), new SolidBrush(trg.FontColor), modifiedBounds);

                    //if ((item.IndentCount > 0) && appSettings.ClientSettings.ShowIndentLine)
                    //{
                    //    int offset;
                    //    int i;
                    //    if (item.IndentCount % 4 == 0)
                    //    {
                    //        offset = 30;
                    //        i = item.IndentCount - 2;
                    //    }
                    //    else
                    //    {
                    //        offset = 0;
                    //        i = item.IndentCount;
                    //    }
                    //    int bottomY = modifiedBounds.Y + modifiedBounds.Height;
                    //    for (i = (item.IndentCount % 4 != 0 ? item.IndentCount - 2 : item.IndentCount); i > 0; i -= 4)
                    //    {
                    //        int x = modifiedBounds.X - (i * 15) + offset;
                    //        e.Graphics.DrawLine(indentDashLine, x, modifiedBounds.Y, x, bottomY);
                    //    }

                    //    int baseX = modifiedBounds.X - (item.IndentCount * 15) + 2;
                    //    e.Graphics.DrawLine(indentDashLine, baseX, modifiedBounds.Y, baseX, bottomY);
                    //}
                    drawCommandText(e);

                    break;
            }
        }

        private void drawCommandText(DrawListViewSubItemEventArgs e)
        {
            //get listviewitem
            ListViewItem item = e.Item;
            var command = (Core.Automation.Commands.ScriptCommand)item.Tag;
            var modifiedBounds = e.Bounds;

            taskt.Core.Theme.UIFont trg;

            int indentWidth = appSettings.ClientSettings.IndentWidth;

            if ((debugLine > 0) && (e.ItemIndex == debugLine - 1))
            {
                trg = taskt.Core.Theme.scriptTexts["debug"];
            }
            else if ((currentScriptEditMode == CommandEditorState.Search) || (currentScriptEditMode == CommandEditorState.AdvencedSearch))
            {
                if (matchingSearchIndex.Contains(e.ItemIndex))
                { 
                    if ((e.Item.Focused) || (e.Item.Selected))
                    {
                        trg = taskt.Core.Theme.scriptTexts["current-match"];
                    }
                    else
                    {
                        trg = taskt.Core.Theme.scriptTexts["match"];
                    }
                }
                else
                {
                    trg = taskt.Core.Theme.scriptTexts[decideNormalCommandText(e, command)];
                }
            }
            else
            {
                trg = taskt.Core.Theme.scriptTexts[decideNormalCommandText(e, command)];
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
        }
        private string decideNormalCommandText(DrawListViewSubItemEventArgs e, Core.Automation.Commands.ScriptCommand command)
        {
            string ret;
            
            if (command.PauseBeforeExeucution)
            {
                ret = "pause";
            }
            else if ((command is Core.Automation.Commands.CommentCommand) || (command.IsCommented))
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

                //FormatCommandListView();
            }
        }


        #region lstScriptActions event
        private void lstScriptActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!appSettings.ClientSettings.InsertCommandsInline)
                return;

            //check to see if an item has been selected last
            if (lstScriptActions.SelectedItems.Count > 0)
            {
                selectedIndex = lstScriptActions.SelectedItems[0].Index;
                //FormatCommandListView();
            }
            else
            {
                //nothing is selected
                selectedIndex = -1;
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
                //foreach (ListViewItem item in lstScriptActions.Items)
                //{
                //    item.Selected = true;
                //}
                SelectAllRows();
            }
            else if ((e.Control) && (e.Shift) && (e.KeyCode == Keys.F))
            {
                // highlight this command
                HighlightAllCurrentSelectedCommand();
            }
        }
        private void lstScriptActions_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (lstScriptActions.FocusedItem.Bounds.Contains(e.Location) == true)
                {
                    lstContextStrip.Show(Cursor.Position);
                }
            }
        }
        private void lstScriptActions_MouseMove(object sender, MouseEventArgs e)
        {
            lstScriptActions.Invalidate();
        }
        private void lstScriptActions_DoubleClick(object sender, EventArgs e)
        {

            if (lstScriptActions.SelectedItems.Count != 1)
            {
                return;
            }


            //bring up edit mode to edit the action
            ListViewItem selectedCommandItem = lstScriptActions.SelectedItems[0];


            //set selected command from the listview item tag object which was assigned to the command
            var currentCommand = (Core.Automation.Commands.ScriptCommand)selectedCommandItem.Tag;


            //check if editing a sequence
            if (currentCommand is Core.Automation.Commands.SequenceCommand)
            {

                if (editMode)
                {
                    MessageBox.Show("Embedding Sequence Commands within Sequence Commands not yet supported.");
                    return;
                }


                //get sequence events
                Core.Automation.Commands.SequenceCommand sequence = (Core.Automation.Commands.SequenceCommand)currentCommand;

                using (frmScriptBuilder newBuilder = new frmScriptBuilder())
                {
                    //add variables

                    newBuilder.scriptVariables = new List<Core.Script.ScriptVariable>();

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
                        List<Core.Automation.Commands.ScriptCommand> updatedList = new List<Core.Automation.Commands.ScriptCommand>();

                        //update to list
                        for (int i = 0; i < newBuilder.lstScriptActions.Items.Count; i++)
                        {
                            var command = (Core.Automation.Commands.ScriptCommand)newBuilder.lstScriptActions.Items[i].Tag;
                            updatedList.Add(command);
                        }

                        //apply new list to existing sequence
                        sequence.v_scriptActions = updatedList;

                        //update label
                        selectedCommandItem.Text = sequence.GetDisplayValue();
                    }
                }
            }
            else
            {

                //create new command editor form
                using (UI.Forms.frmCommandEditor editCommand = new UI.Forms.frmCommandEditor(automationCommands, GetConfiguredCommands()))
                {
                    //creation mode edit locks form to current command
                    editCommand.creationMode = UI.Forms.frmCommandEditor.CreationMode.Edit;

                    //editCommand.defaultStartupCommand = currentCommand.SelectionName;
                    editCommand.editingCommand = currentCommand;

                    //create clone of current command so databinding does not affect if changes are not saved
                    editCommand.originalCommand = Core.Common.Clone(currentCommand);

                    //set variables
                    editCommand.scriptVariables = this.scriptVariables;

                    // set taskt settings
                    editCommand.appSettings = this.appSettings;

                    //show edit command form and save changes on OK result
                    if (editCommand.ShowDialog() == DialogResult.OK)
                    {
                        ChangeSaveState(true);

                        selectedCommandItem.Tag = editCommand.selectedCommand;
                        selectedCommandItem.Text = editCommand.selectedCommand.GetDisplayValue(); //+ "(" + cmdDetails.SelectedVariables() + ")";
                        selectedCommandItem.SubItems.Add(editCommand.selectedCommand.GetDisplayValue());
                    }
                }
            }
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
        private void moveToParentToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //create command list
            var commandList = new List<Core.Automation.Commands.ScriptCommand>();

            //loop each
            for (int i = lstScriptActions.SelectedItems.Count - 1; i >= 0; i--)
            {
                //add to list and remove existing
                commandList.Add((Core.Automation.Commands.ScriptCommand)lstScriptActions.SelectedItems[i].Tag);
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
        private void viewCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var currentCommand = lstScriptActions.SelectedItems[0].Tag;

            var jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(currentCommand, new Newtonsoft.Json.JsonSerializerSettings() { TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All });

            using (var dialog = new Supplemental.frmDialog(jsonText, "Command Code", Supplemental.frmDialog.DialogType.OkOnly, 0))
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
        private void showScriptInfoMenuItem_Click(object sender, EventArgs e)
        {
            using (frmScriptInformations frm = new frmScriptInformations())
            {
                frm.infos = scriptInfo;
                frm.ShowDialog();
                ChangeSaveState(true);
            }
        }
        #endregion

        #endregion

        #endregion

        #region Bottom Notification Panel
        List<string> notificationList = new List<string>();
        private DateTime notificationExpires;
        private bool isDisplaying;


        private void tmrNotify_Tick(object sender, EventArgs e)
        {
            if (appSettings ==  null)
            {
                return;
            }

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
        public void Notify(string notificationText)
        {
            notificationList.Add(notificationText);
        }
        private void ShowNotification(string textToDisplay)
        {
            notificationText = textToDisplay;
            //lblStatus.Left = 20;
            //lblStatus.Text = textToDisplay;

            pnlStatus.SuspendLayout();
            //for (int i = 0; i < 30; i++)
            //{
            //    tlpControls.RowStyles[3].Height = i;
            //}
            ShowNotificationRow();
            pnlStatus.ResumeLayout();
            isDisplaying = true;
        }
        private void HideNotification()
        {

            pnlStatus.SuspendLayout();
            //for (int i = 30; i > 0; i--)
            //{
            //    tlpControls.RowStyles[3].Height = i;
            //}
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
        public string notificationText { get; set; }
        private void pnlStatus_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawString(notificationText, pnlStatus.Font, Brushes.White, 30, 4);
            e.Graphics.DrawImage(Properties.Resources.message, 5, 3, 24, 24);

        }

        private void notifyTray_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (appSettings.ClientSettings.MinimizeToTray)
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

        #region Open, Save, Parse, Import File
        private void BeginOpenScriptProcess()
        {
            CheckAndSaveScriptIfForget();

            //show ofd
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
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

            try
            {
                //reinitialize
                lstScriptActions.Items.Clear();
                scriptVariables = new List<Core.Script.ScriptVariable>();
                scriptInfo = null;

                //get deserialized script
                Core.Script.Script deserializedScript = Core.Script.Script.DeserializeFile(filePath, appSettings.EngineSettings);

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
                    scriptInfo = new Core.Script.ScriptInformation();
                }

                //populate commands
                PopulateExecutionCommands(deserializedScript.Commands);

                // check indent
                IndentListViewItems();

                //format listview
                ChangeSaveState(false);

                //notify
                Notify("Script Loaded Successfully!");

                // release
                deserializedScript = null;
            }
            catch (Exception ex)
            {
                //signal an error has happened
                Notify("An Error Occured: " + ex.Message);
            }
        }

        private void BeginImportProcess()
        {
            //show ofd
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
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

            try
            {
                //deserialize file      
                Core.Script.Script deserializedScript = Core.Script.Script.DeserializeFile(filePath, appSettings.EngineSettings);

                if (deserializedScript.Commands.Count == 0)
                {
                    Notify("Error Parsing File: Commands not found!");
                }

                //variables for comments
                var fileName = new System.IO.FileInfo(filePath).Name;
                var dateTimeNow = DateTime.Now.ToString();

                //comment
                lstScriptActions.Items.Add(CreateScriptCommandListViewItem(new Core.Automation.Commands.CommentCommand() { v_Comment = "Imported From " + fileName + " @ " + dateTimeNow }));

                //import
                PopulateExecutionCommands(deserializedScript.Commands);
                foreach (Core.Script.ScriptVariable var in deserializedScript.Variables)
                {
                    if (scriptVariables.Find(alreadyExists => alreadyExists.VariableName == var.VariableName) == null)
                    {
                        scriptVariables.Add(var);
                    }
                }

                //comment
                lstScriptActions.Items.Add(CreateScriptCommandListViewItem(new Core.Automation.Commands.CommentCommand() { v_Comment = "End Import From " + fileName + " @ " + dateTimeNow }));


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

        public void PopulateExecutionCommands(List<Core.Script.ScriptAction> commandDetails)
        {
            foreach (Core.Script.ScriptAction item in commandDetails)
            {
                lstScriptActions.Items.Add(CreateScriptCommandListViewItem(item.ScriptCommand));
                if (item.AdditionalScriptCommands.Count > 0) PopulateExecutionCommands(item.AdditionalScriptCommands);
            }

            if (pnlCommandHelper.Visible)
            {
                pnlCommandHelper.Hide();
            }
        }
        public int InsertExecutionCommands(List<Core.Script.ScriptAction> commandDetails, int position = -1)
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
            foreach (Core.Script.ScriptAction item in commandDetails)
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

        private List<Core.Automation.Commands.ScriptCommand> GetConfiguredCommands()
        {
            List<Core.Automation.Commands.ScriptCommand> ConfiguredCommands = new List<Core.Automation.Commands.ScriptCommand>();
            foreach (ListViewItem item in lstScriptActions.Items)
            {
                ConfiguredCommands.Add(item.Tag as Core.Automation.Commands.ScriptCommand);
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
                if ((item.Tag is Core.Automation.Commands.BeginExcelDatasetLoopCommand) || (item.Tag is Core.Automation.Commands.BeginListLoopCommand) || (item.Tag is Core.Automation.Commands.BeginContinousLoopCommand) ||(item.Tag is Core.Automation.Commands.BeginNumberOfTimesLoopCommand) || (item.Tag is Core.Automation.Commands.BeginLoopCommand) || (item.Tag is Core.Automation.Commands.BeginMultiLoopCommand))
                {
                    beginLoopValidationCount++;
                }
                else if (item.Tag is Core.Automation.Commands.EndLoopCommand)
                {
                    beginLoopValidationCount--;
                }
                else if ((item.Tag is Core.Automation.Commands.BeginIfCommand) || (item.Tag is Core.Automation.Commands.BeginMultiIfCommand))
                {
                    beginIfValidationCount++;
                }
                else if (item.Tag is Core.Automation.Commands.EndIfCommand)
                {
                    beginIfValidationCount--;
                }
                else if(item.Tag is Core.Automation.Commands.TryCommand)
                {
                    tryCatchValidationCount++;
                }
                else if (item.Tag is Core.Automation.Commands.EndTryCommand)
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
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
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

            //serialize script
            try
            {
                scriptInfo.TasktVersion = Application.ProductVersion;
                var exportedScript = Core.Script.Script.SerializeScript(lstScriptActions.Items, scriptVariables, scriptInfo, appSettings.EngineSettings, this.ScriptFilePath);
                //show success dialog
                Notify("File has been saved successfully!");
                ChangeSaveState(false);
            }
            catch (Exception ex)
            {
                Notify("Save Error: " + ex.ToString());
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
            scriptVariables = new List<Core.Script.ScriptVariable>();
            scriptInfo = new Core.Script.ScriptInformation();

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

            UI.Forms.frmScriptEngine newEngine = new UI.Forms.frmScriptEngine(ScriptFilePath, this);
            newEngine.callBackForm = this;
            newEngine.Show();
        }
        #endregion


        #region Automation Engine Delegate

        #endregion

        #region misc?
        private void btnManageVariables_Click(object sender, EventArgs e)
        {

            //using (UI.Forms.frmScriptVariables scriptVariableEditor = new UI.Forms.frmScriptVariables())
            //{
            //    scriptVariableEditor.appSettings = this.appSettings;
            //    scriptVariableEditor.scriptVariables = this.scriptVariables;

            //    if (scriptVariableEditor.ShowDialog() == DialogResult.OK)
            //    {
            //        this.scriptVariables = scriptVariableEditor.scriptVariables;
            //    }
            //}
            showVariableManager();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            using (UI.Forms.Supplemental.frmThickAppElementRecorder recorder = new Supplemental.frmThickAppElementRecorder())
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
            using (UI.Forms.Supplemental.frmAbout frmAboutForm = new UI.Forms.Supplemental.frmAbout())
            {
                frmAboutForm.ShowDialog();
            }
        }

        private void PerformAntiIdle()
        {

            lastAntiIdleEvent = DateTime.Now;
            var mouseMove = new Core.Automation.Commands.SendMouseMoveCommand();
            mouseMove.v_XMousePosition = (Cursor.Position.X + 1).ToString();
            mouseMove.v_YMousePosition = (Cursor.Position.Y + 1).ToString();
            Notify("Anti-Idle Triggered");
        }
        #endregion

        #region Create Command Logic
        private void AddNewCommand(string specificCommand = "")
        {
            //bring up new command configuration form
            using (var newCommandForm = new UI.Forms.frmCommandEditor(automationCommands, GetConfiguredCommands()))
            {
                newCommandForm.creationMode = UI.Forms.frmCommandEditor.CreationMode.Add;
                newCommandForm.scriptVariables = this.scriptVariables;
                // set taskt settings
                newCommandForm.appSettings = this.appSettings;
                if (specificCommand != "")
                    newCommandForm.defaultStartupCommand = specificCommand;

                //if a command was selected
                if (newCommandForm.ShowDialog() == DialogResult.OK)
                {
                    ChangeSaveState(true);

                    //add to listview
                    AddCommandToListView(newCommandForm.selectedCommand);
                }
            }
        }
        #endregion


        #region TreeView Events
        private void tvCommands_DoubleClick(object sender, EventArgs e)
        {
            //exit if parent node is clicked
            if (tvCommands.SelectedNode.Parent == null)
            {
                return;
            }

            if (tvCommands.SelectedNode.Level == 1)
            {
                if (tvCommands.SelectedNode.Nodes.Count > 0)
                {
                    return;
                }
                else
                {
                    AddNewCommand(tvCommands.SelectedNode.Parent.Text + " - " + tvCommands.SelectedNode.Text);
                }
            }
            else
            {
                // maybe level == 2
                AddNewCommand(tvCommands.SelectedNode.Parent.Parent.Text + " - " + tvCommands.SelectedNode.Text);
            }
        }
       private void tvCommands_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                tvCommands_DoubleClick(this, null);
            }

        }
        #endregion

        

        



        #region Variable Edit
        private void showVariableManager()
        {
            using (UI.Forms.frmScriptVariables scriptVariableEditor = new UI.Forms.frmScriptVariables())
            {
                scriptVariableEditor.appSettings = this.appSettings;
                scriptVariableEditor.scriptVariables = this.scriptVariables;

                if (scriptVariableEditor.ShowDialog() == DialogResult.OK)
                {
                    this.scriptVariables = scriptVariableEditor.scriptVariables;
                }
            }
        }
        #endregion

        #region taskt About Form
        private void showAboutForm()
        {
            using (Supplemental.frmAbout aboutForm = new Supplemental.frmAbout())
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
            //UI.Forms.frmScriptVariables scriptVariableEditor = new UI.Forms.frmScriptVariables();
            //scriptVariableEditor.scriptVariables = this.scriptVariables;

            //if (scriptVariableEditor.ShowDialog() == DialogResult.OK)
            //{
            //    this.scriptVariables = scriptVariableEditor.scriptVariables;
            //}
            showVariableManager();
        }
        private void uiBtnSettings_Click(object sender, EventArgs e)
        {
            //show settings dialog
            using (frmSettings newSettings = new frmSettings(this))
            {
                newSettings.ShowDialog();

                //reload app settings
                appSettings = new Core.ApplicationSettings();
                appSettings = appSettings.GetOrCreateApplicationSettings();

                //reinit
                Core.Server.HttpServerClient.Initialize();
            }
        }
        private void uiBtnClearAll_Click(object sender, EventArgs e)
        {
            HideSearchInfo();
            lstScriptActions.Items.Clear();
        }
        private void uiBtnRecordSequence_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (frmSequenceRecorder sequenceRecorder = new frmSequenceRecorder())
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
            using (UI.Forms.frmScheduleManagement scheduleManager = new UI.Forms.frmScheduleManagement())
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
            if (txtCommandSearch.Text != "" || tsSearchBox.Text != "")
            {
                reqdIndex++;
                SearchForItemInListView();
            }
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

        private void SearchStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSearch.Show();
        }

        private void ReplaceStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("sorry, work in process ;-)");
        }
        private void clearSearchHighlightsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //this.currentScriptEditMode = CommandEditorState.Normal;
            //lstScriptActions.Invalidate();
            ClearHighlightListViewItem();
        }
        #endregion

        #region Options menu event handler
        private void variablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //UI.Forms.frmScriptVariables scriptVariableEditor = new UI.Forms.frmScriptVariables();
            //scriptVariableEditor.scriptVariables = this.scriptVariables;

            //if (scriptVariableEditor.ShowDialog() == DialogResult.OK)
            //{
            //    this.scriptVariables = scriptVariableEditor.scriptVariables;
            //}
            showVariableManager();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //show settings dialog
            using (frmSettings newSettings = new frmSettings(this))
            {
                newSettings.ShowDialog();
            }

            //reload app settings
            appSettings = new Core.ApplicationSettings();
            appSettings = appSettings.GetOrCreateApplicationSettings();

            //reinit
            Core.Server.HttpServerClient.Initialize();
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
                showSearchBarToolStripMenuItem.Checked = true;
            }
            else
            {
                showSearchBarToolStripMenuItem.Text = "Show Search Bar";
                showSearchBarToolStripMenuItem.Checked = false;
            }

        }
        #endregion

        #region Script Actions menu event handler
        private void recordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();

            using (frmSequenceRecorder sequenceRecorder = new frmSequenceRecorder())
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
            UI.Forms.frmScheduleManagement scheduleManager = new UI.Forms.frmScheduleManagement();
            scheduleManager.Show();
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BeginRunScriptProcess();
        }
        #endregion

        #region Save And Run menu event handler
        private void saveAndRunToolStripMenuItem_Clicked(object sender, EventArgs e)
        {
            //saveToolStripMenuItem_Click(null, null);
            //runToolStripMenuItem_Click(null, null);
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

        
    }

}

