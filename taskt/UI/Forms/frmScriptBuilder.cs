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
        private TreeNode[] bufferedCommandList;
        private ImageList bufferedCommandTreeImages;

        //private List<ListViewItem> rowsSelectedForCopy { get; set; }
        private List<Core.Script.ScriptVariable> scriptVariables;
        private Core.Script.ScriptInformation scriptInfo;
        
        private bool editMode { get; set; }

        private ImageList scriptImages;

        public Core.ApplicationSettings appSettings;
        private List<List<ListViewItem>> undoList;
        private DateTime lastAntiIdleEvent;
        private int undoIndex = -1;
        //private int reqdIndex;
        private int selectedIndex = -1;
        private int DnDIndex = -1;

        private bool dontSaveFlag = false;

        private Core.InstanceCounter instanceList = null;
        private int[,] miniMap = null;
        private Bitmap miniMapImg = null;

        //private Timer miniMapLoadingDelayTimer = new Timer() { Interval = 100 };

        public CommandEditorState currentScriptEditorMode = CommandEditorState.Normal;
        public CommandEditAction currentEditAction = CommandEditAction.Normal;

        private Size lastEditorSize = new Size { Height = 0, Width = 0 };
        private Point lastEditorPosition;

        // search & replace
        //private List<int> matchingSearchIndex = new List<int>();
        private int currentIndexInMatchItems = -1;
        public int MatchedLines { private set; get; }

        private frmScriptBuilder parentBuilder { get; set; }

        private Pen indentDashLine;

        // forms
        private Supplement_Forms.frmSearchCommands frmSearch = null;
        private frmAttendedMode frmAttended = null;

        private string _scriptFilePath = null;

        #region properties
        private List<taskt.UI.CustomControls.AutomationCommand> automationCommands { get; set; }

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
            automationCommands = taskt.UI.CustomControls.CommandControls.GenerateCommandsandControls();

            // title
            var info = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
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
                scriptVariables = new List<Core.Script.ScriptVariable>();
                scriptInfo = new Core.Script.ScriptInformation();
            }


            //pnlHeader.BackColor = Color.FromArgb(255, 214, 88);

            //instantiate and populate display icons for commands
            scriptImages = UI.Images.UIImageList();

            //set image list
            lstScriptActions.SmallImageList = scriptImages;
            lstScriptActions.Columns[0].Width = 14; // 1digit width
            lstScriptActions.Columns[1].Width = 16; // icon size
            lstScriptActions.Columns[2].Width = lstScriptActions.ClientSize.Width - 30;

            // myToolTip
            //myToolTip.SetToolTip(lstScriptActions, "");

            //set listview column size
            frmScriptBuilder_SizeChanged(null, null);

            GenerateTreeViewCommands();


            //start attended mode if selected
            if (appSettings.ClientSettings.StartupMode == "Attended Task Mode")
            {
                this.WindowState = FormWindowState.Minimized;
                //var frmAttended = new frmAttendedMode();
                //frmAttended.Show();
                showAttendedModeFormProcess();
            }

            this.dontSaveFlag = false;

            // set searchform
            frmSearch = new Supplement_Forms.frmSearchCommands(this);

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

            // check update
            if ((appSettings.ClientSettings.CheckForUpdateAtStartup) && (this.parentBuilder == null))
            {
                taskt.Core.ApplicationUpdate.ShowUpdateResultAsync();
            }
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
        private void linkGitter_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            showGitterPage();
        }
        private void NewFileLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel senderLink = (LinkLabel)sender;
            string targetScriptPath = Core.IO.Folders.GetFolder(Core.IO.Folders.FolderType.ScriptsFolder) + senderLink.Text;
            //OpenFile(Core.IO.Folders.GetFolder(Core.IO.Folders.FolderType.ScriptsFolder) + senderLink.Text);
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
        private void showThisCommandHelp(Core.Automation.Commands.ScriptCommand command)
        {
            //string page = command.SelectionName.ToLower().Replace(" ", "-").Replace("/", "-") + "-command.md";
            //string parent = ((Core.Automation.Attributes.ClassAttributes.Group)command.GetType().GetCustomAttribute(typeof(Core.Automation.Attributes.ClassAttributes.Group))).groupName.ToLower().Replace(" ", "-").Replace("/", "-");
            //System.Diagnostics.Process.Start(Core.MyURLs.WikiBaseURL + parent + "/" + page);
            string parent = ((Core.Automation.Attributes.ClassAttributes.Group)command.GetType().GetCustomAttribute(typeof(Core.Automation.Attributes.ClassAttributes.Group))).groupName;
            System.Diagnostics.Process.Start(Core.MyURLs.GetWikiURL(command.SelectionName, parent));
        }
        private void BeginShowThisCommandHelpProcess()
        {
            if (lstScriptActions.SelectedItems.Count > 0)
            {
                showThisCommandHelp((Core.Automation.Commands.ScriptCommand)lstScriptActions.SelectedItems[0].Tag);
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
            //for (int i = 0; i <= len; i++)
            //{
            //    if (e.Data.GetFormats()[i].Equals("System.Windows.Forms.ListView+SelectedListViewItemCollection"))
            //    {
            //        //The data from the drag source is moved to the target.
            //        e.Effect = DragDropEffects.Move;
            //    }
            //}
        }
        private void lstScriptActions_DragLeave(object sender, EventArgs e)
        {
            lstScriptActions.BackColor = Color.WhiteSmoke;
        }

        private void lstScriptActions_DragDrop(object sender, DragEventArgs e)
        {
            lstScriptActions.BackColor = Color.WhiteSmoke;
            //this.currentEditAction = CommandEditAction.Normal;

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
                    using (var fm = new UI.Forms.Supplemental.frmDialog("This file type can not open.", "File Open Error", Supplemental.frmDialog.DialogType.OkOnly, 0))
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

            //var hitPoint = e.Location;
            //ListViewHitTestInfo info = lstScriptActions.HitTest(hitPoint);
            //if (info.SubItem != null)
            //{
            //    //if (info.Item == currentHovered)
            //    //{
            //    //    if ((DateTime.Now - hoverStarted).TotalMilliseconds >= 500)
            //    //    {
            //    //        myToolTip.Show(info.Item.Text, lstScriptActions, hitPoint);
            //    //    }

            //    //}
            //    //else
            //    //{
            //    //    myToolTip.Hide(lstScriptActions);
            //    //    currentHovered = info.Item;
            //    //    hoverStarted = DateTime.Now;
            //    //}
            //    myToolTip.SetToolTip(lstScriptActions, info.Item.Text);
            //}
            //else
            //{
            //    //myToolTip.Hide(lstScriptActions);
            //    //currentHovered = null;
            //    myToolTip.SetToolTip(lstScriptActions, "");
            //}
        }

        private void MoveCommands(DragEventArgs e)
        {
            ////Return if the items are not selected in the ListView control.
            //if (lstScriptActions.SelectedItems.Count == 0)
            //{
            //    return;
            //}

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
                //for (int i = lstScriptActions.SelectedItems.Count - 1; i >= 0; i--)
                //{
                //    lstScriptActions.Items.Remove(lstScriptActions.SelectedItems[i]);
                //}
                int[] indices = new int[lstScriptActions.SelectedIndices.Count];
                lstScriptActions.SelectedIndices.CopyTo(indices, 0);
                for (int i = indices.Length - 1; i>=0; i--)
                {
                    lstScriptActions.Items.RemoveAt(indices[i]);
                }
                //lstScriptActions.EndUpdate();
                //return back
                //return;
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

                    var command = (Core.Automation.Commands.ScriptCommand)insertItem.Tag;
                    command.IsDontSavedCommand = true;
                    command.IsNewInsertedCommand = true;

                    lstScriptActions.Items.Insert(itemIndex, insertItem);
                    //Removes the item from the initial location while
                    //the item is moved to the new location.
                    lstScriptActions.Items.Remove(dragItem);
                    //FormatCommandListView();
                    //lstScriptActions.Invalidate();
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

            //var matchingItems = (from ListViewItem itm in lstScriptActions.Items
            //                     where itm.Text.Contains(searchCriteria)
            //                     select itm).ToList();


            //int? matchCount = matchingItems.Count();
            //int totalMatches = matchCount ?? 0;


            //if ((reqdIndex == matchingItems.Count) || (reqdIndex < 0))
            //{
            //    reqdIndex = 0;
            //}

            //lblTotalResults.Show();

            //if (totalMatches == 0)
            //{
            //    currentScriptEditorMode = CommandEditorState.Normal;
            //    reqdIndex = -1;
            //    lblTotalResults.Text = "No Matches Found";
            //    lblCurrentlyViewing.Hide();
            //    //clear indexes
            //    matchingSearchIndex.Clear();
            //    reqdIndex = -1;
            //    lstScriptActions.Invalidate();
            //    return;
            //}
            //else
            //{
            //    currentScriptEditorMode = CommandEditorState.Search;
            //    lblCurrentlyViewing.Text = "Viewing " + (reqdIndex + 1) + " of " + totalMatches + "";
            //    tsSearchResult.Text = "Viewing " + (reqdIndex + 1) + " of " + totalMatches + "";
            //    lblTotalResults.Text = totalMatches + " total results found";
            //}

            //ClearSelectedListViewItems();

            //matchingSearchIndex = new List<int>();
            //foreach (ListViewItem itm in matchingItems)
            //{
            //    matchingSearchIndex.Add(itm.Index);
            //    //itm.BackColor = Color.LightGoldenrodYellow;
            //}

            //currentIndexInMatchItems = matchingItems[0].Index;
            //lstScriptActions.Items[0].Selected = true;
            ////currentIndexInMatchItems = matchingItems[reqdIndex].Index;

            //lstScriptActions.Invalidate();

            ////lstScriptActions.EnsureVisible(currentIndexInMatchItems);

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
            //matchingSearchIndex.Clear();
            //matchingSearchIndex = new List<int>();
            int matchedCount = 0;

            this.currentIndexInMatchItems = -1;
            lstScriptActions.SuspendLayout();
            lstScriptActions.BeginUpdate();
            foreach(ListViewItem itm in lstScriptActions.Items)
            {
                Core.Automation.Commands.ScriptCommand cmd = (Core.Automation.Commands.ScriptCommand)itm.Tag;
                if (cmd.checkMatched(keyword, caseSensitive, checkParameters, checkCommandName, checkComment, checkDisplayText, checkInstanceType, instanceType))
                {
                    matchedCount++;
                    //matchingSearchIndex.Add(itm.Index);
                }
                //else if (instanceName)
                //{
                //    if (cmd.checkInstanceMatched(keyword, instanceType, caseSensitive))
                //    {
                //        matchedCount++;
                //        //matchingSearchIndex.Add(itm.Index);
                //    }
                //}
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
            //if (this.currentScriptEditorMode != CommandEditorState.AdvencedSearch)
            //{
            //    return;
            //}
            switch (this.currentScriptEditorMode)
            {
                case CommandEditorState.Normal:
                    return;
                    break;

                case CommandEditorState.Search:
                case CommandEditorState.AdvencedSearch:
                case CommandEditorState.ReplaceSearch:
                case CommandEditorState.HighlightCommand:
                    break;

                default:
                    return;
                    break;
            }

            if (this.MatchedLines == 0)
            {
                return;
            }

            //if (matchingSearchIndex.Count == 0)
            //{
            //    return;
            //}

            //if (this.currentIndexInMatchItems >= 0)
            //{
            //    // select matched item yet
            //    int nextIndex = this.currentIndexInMatchItems + 1;
            //    if (backToTop)
            //    {
            //        nextIndex %= matchingSearchIndex.Count;
            //    }
            //    else
            //    {
            //        if (nextIndex >= matchingSearchIndex.Count)
            //        {
            //            nextIndex--;
            //        }
            //    }
            //    this.currentIndexInMatchItems = nextIndex;
            //    ClearSelectedListViewItems();

            //    lstScriptActions.Items[matchingSearchIndex[nextIndex]].Selected = true;
            //}
            //else
            //{
            //    if (lstScriptActions.SelectedIndices.Count > 0)
            //    {
            //        // some item selected yet. -> search near item
            //        int currentIndex = lstScriptActions.SelectedIndices[0];
            //        if (matchingSearchIndex[matchingSearchIndex.Count - 1] > currentIndex)
            //        {
            //            if (backToTop)
            //            {
            //                this.currentIndexInMatchItems = 0;
            //                ClearSelectedListViewItems();
            //                lstScriptActions.Items[matchingSearchIndex[0]].Selected = true;
            //            }
            //            else
            //            {
            //                ClearSelectedListViewItems();
            //                return;
            //            }
            //        }
            //        else
            //        {
            //            int targetIndex = matchingSearchIndex.Count - 1;
            //            while (currentIndex > matchingSearchIndex[targetIndex])
            //            {
            //                targetIndex--;
            //            }
            //            targetIndex++;
            //            this.currentIndexInMatchItems = targetIndex;
            //            ClearSelectedListViewItems();
            //            lstScriptActions.Items[matchingSearchIndex[targetIndex]].Selected = true;
            //        }
            //    }
            //    else
            //    {
            //        // no selected item -> select fist matched item
            //        this.currentIndexInMatchItems = 0;
            //        ClearSelectedListViewItems();
            //        lstScriptActions.Items[matchingSearchIndex[0]].Selected = true;
            //    }
            //}

            int lines = lstScriptActions.Items.Count;

            if (this.currentIndexInMatchItems >= 0)
            {
                if (backToTop)
                {
                    for (int i = 1; i < lines; i++)
                    {
                        int idx = (i + this.currentIndexInMatchItems) % lines;
                        if (((Core.Automation.Commands.ScriptCommand)lstScriptActions.Items[idx].Tag).IsMatched)
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
                        if (((Core.Automation.Commands.ScriptCommand)lstScriptActions.Items[i].Tag).IsMatched)
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
                    if (((Core.Automation.Commands.ScriptCommand)lstScriptActions.Items[i].Tag).IsMatched)
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
                string keyword = ((Core.Automation.Commands.ScriptCommand)lstScriptActions.SelectedItems[0].Tag).SelectionName;
                AdvancedSearchItemInCommands(keyword, false, false, true, false, false, false, "");
                this.currentScriptEditorMode = CommandEditorState.HighlightCommand;
            }
        }
        public int ReplaceSearchInItemCommands(string keyword, bool caseSensitive, string instanceType, bool allProperties, bool instanceName, bool comment)
        {
            //matchingSearchIndex.Clear();
            //matchingSearchIndex = new List<int>();
            int matchedCount = 0;

            this.currentIndexInMatchItems = -1;

            lstScriptActions.SuspendLayout();
            lstScriptActions.BeginUpdate();

            if (allProperties)
            {
                foreach(ListViewItem itm in lstScriptActions.Items)
                {
                    if (((Core.Automation.Commands.ScriptCommand)itm.Tag).checkMatched(keyword, caseSensitive, true, false, false, false, false, ""))
                    {
                        matchedCount++;
                        //matchingSearchIndex.Add(itm.Index);
                    }
                }
            }
            else if (instanceName)
            {
                foreach (ListViewItem itm in lstScriptActions.Items)
                {
                    if (((Core.Automation.Commands.ScriptCommand)itm.Tag).checkMatched(keyword, caseSensitive, false, false, false, false, true, instanceType))
                    {
                        matchedCount++;
                        //matchingSearchIndex.Add(itm.Index);
                    }
                }
            }
            else if (comment)
            {
                foreach (ListViewItem itm in lstScriptActions.Items)
                {
                    if (((Core.Automation.Commands.ScriptCommand)itm.Tag).checkMatched(keyword, false, false, false, true, false, false, ""))
                    {
                        matchedCount++;
                        //matchingSearchIndex.Add(itm.Index);
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
                    if (((Core.Automation.Commands.ScriptCommand)lstScriptActions.Items[trgIdx].Tag).ReplaceAllParameters(keyword, replacedText, caseSensitive))
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
                    if (((Core.Automation.Commands.ScriptCommand)lstScriptActions.Items[trgIdx].Tag).ReplaceInstance(keyword, replacedText, instanceType, caseSensitive))
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
                    if (((Core.Automation.Commands.ScriptCommand)lstScriptActions.Items[trgIdx].Tag).ReplaceComment(keyword, replacedText, caseSensitive))
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
                    if (((Core.Automation.Commands.ScriptCommand)lstScriptActions.Items[(i + currentIndex) % rows].Tag).ReplaceAllParameters(keyword, replacedText, caseSensitive))
                    {
                        replaceCount++;
                    }
                }
            }
            else if (instanceName)
            {
                for (int i = 0; i < rows; i++)
                {
                    if (((Core.Automation.Commands.ScriptCommand)lstScriptActions.Items[(i + currentIndex) % rows].Tag).ReplaceInstance(keyword, replacedText, instanceType, caseSensitive))
                    {
                        replaceCount++;
                    }
                }
            }
            else if (comment)
            {
                for (int i = 0; i < rows; i++)
                {
                    if (((Core.Automation.Commands.ScriptCommand)lstScriptActions.Items[(i + currentIndex) % rows].Tag).ReplaceComment(keyword, replacedText, caseSensitive))
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
                        List<Core.Automation.Commands.ScriptCommand> updatedList = new List<Core.Automation.Commands.ScriptCommand>();

                        //update to list
                        for (int i = 0; i < newBuilder.lstScriptActions.Items.Count; i++)
                        {
                            var command = (Core.Automation.Commands.ScriptCommand)newBuilder.lstScriptActions.Items[i].Tag;
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
                //Core.InstanceNameType nameType = instanceList.getInstanceNameType(currentCommand);
                Core.Automation.Commands.ScriptCommand cloneCommand = currentCommand.Clone();
                //instanceList.removeInstance(currentCommand);
                currentCommand.removeInstance(instanceList);

                //create new command editor form
                using (UI.Forms.frmCommandEditor editCommand = new UI.Forms.frmCommandEditor(automationCommands, GetConfiguredCommands(), this.bufferedCommandList, this.bufferedCommandTreeImages))
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

                        //instanceList.addInstance(editCommand.selectedCommand);
                        editCommand.selectedCommand.addInstance(instanceList);
                    }
                    else
                    {
                        //if (nameType != null)
                        //{
                        //    instanceList.addInstance(nameType);
                        //}
                        //instanceList.addInstance(cloneCommand);
                        cloneCommand.addInstance(instanceList);
                    }
                }
            }
        }

        private void SelectAllRows()
        {
            // temp remove handle
            //lstScriptActions.SelectedIndexChanged -= lstScriptActions_SelectedIndexChanged;

            lstScriptActions.BeginUpdate();
            foreach (ListViewItem itm in lstScriptActions.Items)
            {
                itm.Selected = true;
            }
            lstScriptActions.EndUpdate();

            // attach handle
            //lstScriptActions.SelectedIndexChanged += lstScriptActions_SelectedIndexChanged;
            //lstScriptActions_SelectedIndexChanged(null, null);
        }

        private void DeleteRows()
        {
            lstScriptActions.BeginUpdate();

            int[] indices = new int[lstScriptActions.SelectedItems.Count];
            lstScriptActions.SelectedIndices.CopyTo(indices, 0);
            //foreach (ListViewItem itm in lstScriptActions.SelectedItems)
            //{
            //    lstScriptActions.Items.Remove(itm);
            //}

            // remove instance name
            List<Core.Automation.Commands.ScriptCommand> removeCommands = new List<Core.Automation.Commands.ScriptCommand>();
            foreach(int i in indices)
            {
                removeCommands.Add((Core.Automation.Commands.ScriptCommand)lstScriptActions.Items[i].Tag);
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

                lstScriptActions.BeginUpdate();
                int[] indices = new int[lstScriptActions.SelectedItems.Count];
                lstScriptActions.SelectedIndices.CopyTo(indices, 0);
                //foreach (ListViewItem item in lstScriptActions.SelectedItems)
                //{
                //    commands.Add((Core.Automation.Commands.ScriptCommand)item.Tag);
                //    //rowsSelectedForCopy.Add(item);
                //    //lstScriptActions.Items.Remove(item);
                //}
                for (int i = 0; i <indices.Length; i++)
                {
                    commands.Add((Core.Automation.Commands.ScriptCommand)lstScriptActions.Items[indices[i]].Tag);
                }
                for (int i = indices.Length - 1; i >= 0; i--)
                {
                    lstScriptActions.Items.RemoveAt(indices[i]);
                }
                lstScriptActions.EndUpdate();
                // set clipborad xml string
                Clipboard.SetText(taskt.Core.Script.Script.SerializeScript(commands));

                // remove instance name
                RemoveInstanceName(commands);

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
                var selectedCommand = (Core.Automation.Commands.ScriptCommand)item.Tag;
                selectedCommand.IsCommented = setCommented;
            }

            ChangeSaveState(true);

            //clear selection
            //lstScriptActions.SelectedIndices.Clear();
            //ClearSelectedListViewItems();

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
                var selectedCommand = (Core.Automation.Commands.ScriptCommand)item.Tag;
                selectedCommand.PauseBeforeExeucution = !selectedCommand.PauseBeforeExeucution;
            }

            //recolor
            //FormatCommandListView();

            ChangeSaveState(true);

            //clear selection
            //lstScriptActions.SelectedIndices.Clear();
            //ClearSelectedListViewItems();

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
            //lstContextStripSep3.Visible = true;
            //lstContextStripSep4.Visible = false;
            showScriptInfoMenuItem.Visible = false;
        }
        #endregion

        #region ListView Create Item

        private ListViewItem CreateScriptCommandListViewItem(Core.Automation.Commands.ScriptCommand cmdDetails, bool isOpenFile = false)
        {
            //cmdDetails.IsDontSavedCommand = true;
            //cmdDetails.IsNewInsertedCommand = true;

            ListViewItem newCommand = new ListViewItem();

            string dispValue = cmdDetails.GetDisplayValue();

            if (!isOpenFile)
            {
                cmdDetails.IsDontSavedCommand = true;
                cmdDetails.IsNewInsertedCommand = true;
            }

            //newCommand.Text = cmdDetails.GetDisplayValue();
            //newCommand.SubItems.Add(cmdDetails.GetDisplayValue());
            //newCommand.SubItems.Add(cmdDetails.GetDisplayValue());

            newCommand.Text = dispValue;
            newCommand.ToolTipText = dispValue;

            //newCommand.SubItems.AddRange(new string[] { "", "" });
            newCommand.SubItems.AddRange(new string[] { "", "" });

            //ListViewItem.ListViewSubItem subItem = new ListViewItem.ListViewSubItem();
            //subItem.Text = dispValue;
            //newCommand.SubItems.AddRange(new ListViewItem.ListViewSubItem[] { subItem, subItem, subItem });

            cmdDetails.RenderedControls = null;
            newCommand.Tag = cmdDetails;
            //newCommand.ForeColor = cmdDetails.DisplayForeColor;
            //newCommand.BackColor = Color.DimGray;
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

            // count instance name
            AddInstanceName(new List<Core.Automation.Commands.ScriptCommand>() { selectedCommand });

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
                if ((selectedCommand is Core.Automation.Commands.BeginListLoopCommand) || (selectedCommand is Core.Automation.Commands.BeginContinousLoopCommand) || (selectedCommand is Core.Automation.Commands.BeginNumberOfTimesLoopCommand) || (selectedCommand is Core.Automation.Commands.BeginLoopCommand) || (selectedCommand is Core.Automation.Commands.BeginMultiLoopCommand))
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
            if ((selectedCommand is Core.Automation.Commands.BeginListLoopCommand) || (selectedCommand is Core.Automation.Commands.BeginContinousLoopCommand) || (selectedCommand is Core.Automation.Commands.BeginNumberOfTimesLoopCommand) || (selectedCommand is Core.Automation.Commands.BeginLoopCommand) || (selectedCommand is Core.Automation.Commands.BeginMultiLoopCommand))
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

                if ((rowItem.Tag is Core.Automation.Commands.BeginIfCommand) || (rowItem.Tag is Core.Automation.Commands.BeginMultiIfCommand) || (rowItem.Tag is Core.Automation.Commands.BeginListLoopCommand) || (rowItem.Tag is Core.Automation.Commands.BeginContinousLoopCommand) || (rowItem.Tag is Core.Automation.Commands.BeginNumberOfTimesLoopCommand) || (rowItem.Tag is Core.Automation.Commands.TryCommand) || (rowItem.Tag is Core.Automation.Commands.BeginLoopCommand) || (rowItem.Tag is Core.Automation.Commands.BeginMultiLoopCommand))
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
                        var command = (Core.Automation.Commands.ScriptCommand)lstScriptActions.Items[j].Tag;
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
                        var command = (Core.Automation.Commands.ScriptCommand)lstScriptActions.Items[j].Tag;
                        //if ((lstScriptActions.Items[j].Selected) && (lstScriptActions.Items[j].Focused))
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
                        else if (command.IsCommented || (command is Core.Automation.Commands.CommentCommand))
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
                    var command = (Core.Automation.Commands.ScriptCommand)lstScriptActions.Items[i].Tag;
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

                    //if ((lstScriptActions.Items[i].Selected) && (lstScriptActions.Items[i].Focused))
                    //if (lstScriptActions.Items[i].Selected)
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
                    else if (command.IsCommented || (command is Core.Automation.Commands.CommentCommand))
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
                //g.FillRectangle(new SolidBrush(Color.Black), 0, 0, 8, lstScriptActions.Height);
                g.DrawLine(new Pen(Color.White), 0, 0, 0, miniMapImg.Height);
                g.DrawLine(new Pen(Color.White), 4, 0, 4, miniMapImg.Height);
                for (int i = 0; i < miniMapItems; i++)
                {
                    SolidBrush co;
                    switch ((MiniMapState)miniMap[i, 0])
                    {
                        case MiniMapState.DontSave:
                            co = new SolidBrush(taskt.Core.Theme.scriptTexts["number-dontsaved"].BackColor);
                            break;
                        case MiniMapState.NewInserted:
                            co = new SolidBrush(taskt.Core.Theme.scriptTexts["number-newline"].BackColor);
                            break;
                        default:
                            co = new SolidBrush(taskt.Core.Theme.scriptTexts["normal"].BackColor);
                            break;
                    }
                    g.FillRectangle(co, 1, mapItemHeight * i, 3, mapItemHeight);

                    switch ((MiniMapState)miniMap[i, 1])
                    {
                        //case MiniMapState.Cursor:
                        //    co = new SolidBrush(taskt.Core.Theme.scriptTexts["selected-normal"].BackColor);
                        //    break;
                        case MiniMapState.Matched:
                            co = new SolidBrush(taskt.Core.Theme.scriptTexts["current-match"].BackColor);
                            break;
                        case MiniMapState.Error:
                            co = new SolidBrush(taskt.Core.Theme.scriptTexts["invalid"].FontColor);
                            break;
                        case MiniMapState.Comment:
                            co = new SolidBrush(taskt.Core.Theme.scriptTexts["comment"].FontColor);
                            break;
                        default:
                            co = new SolidBrush(taskt.Core.Theme.scriptTexts["normal"].BackColor);
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

            //handle indents
            //IndentListViewItems();

            //auto size line numbers based on command count
            //AutoSizeLineNumberColumn();


            //get listviewitem
            //ListViewItem item = e.Item;

            //get script command reference
            //var command = (Core.Automation.Commands.ScriptCommand)item.Tag;


            //create modified bounds
            //var modifiedBounds = e.Bounds;
            //modifiedBounds.Y += 2;

            //switch between column index
            switch (e.ColumnIndex)
            {
                case 0:
                    //draw row number
                    //e.Graphics.DrawString((e.ItemIndex + 1).ToString(),
                    //    lstScriptActions.Font, Brushes.LightSlateGray, modifiedBounds);
                    drawLineNumber(e);
                    break;
                case 1:
                    //draw command icon
                    //var img = uiImages.Images[command.GetType().Name];
                    //var img = taskt.UI.Images.GetUIImage(command.GetType().Name);
                    //if (img != null)
                    //{
                    //    e.Graphics.DrawImage(img, modifiedBounds.Left, modifiedBounds.Top + 3);
                    //}
                    drawCommandIcon(e);
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

        private void drawLineNumber(DrawListViewSubItemEventArgs e)
        {
            //AutoSizeLineNumberColumn();
            var command = (Core.Automation.Commands.ScriptCommand)e.Item.Tag;

            taskt.Core.Theme.UIFont trg = taskt.Core.Theme.scriptTexts[decideLineNumberText(command)];

            //e.Graphics.DrawString((e.ItemIndex + 1).ToString(), lstScriptActions.Font, Brushes.LightSlateGray, e.Bounds);
            e.Graphics.FillRectangle(new SolidBrush(trg.BackColor), e.Bounds);
            e.Graphics.DrawString((e.ItemIndex + 1).ToString(), new Font(trg.Font, trg.FontSize, trg.Style), new SolidBrush(trg.FontColor), e.Bounds);
        }

        private void drawCommandIcon(DrawListViewSubItemEventArgs e)
        {
            var command = (Core.Automation.Commands.ScriptCommand)e.Item.Tag;
            var modifiedBounds = e.Bounds;
            var img = taskt.UI.Images.GetUIImage(command.GetType().Name);
            if (img != null)
            {
                e.Graphics.DrawImage(img, modifiedBounds.Left, modifiedBounds.Top + 3);
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
            else if ((currentScriptEditorMode == CommandEditorState.Search) || (currentScriptEditorMode == CommandEditorState.AdvencedSearch) || (currentScriptEditorMode == CommandEditorState.ReplaceSearch) || (currentScriptEditorMode == CommandEditorState.HighlightCommand))
            {
                //if (matchingSearchIndex.Contains(e.ItemIndex))
                //{ 
                //    if ((e.Item.Focused) || (e.Item.Selected))
                //    {
                //        trg = taskt.Core.Theme.scriptTexts["current-match"];
                //    }
                //    else
                //    {
                //        trg = taskt.Core.Theme.scriptTexts["match"];
                //    }
                //}
                if (command.IsMatched)
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

            // Emphasis Drag&Drop Line
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

            //if (miniMap == null)
            //{
            //    return;
            //}

            //int maxMapItems = miniMap.GetLength(0);
            //int commmandItems = lstScriptActions.Items.Count;
            //double itemRatio = maxMapItems / commmandItems;

            //modifiedBounds.X -= indentPixels;
            //int mapX = modifiedBounds.Width - 8;

            //if (itemRatio < 1.0)
            //{
            //    // mada
            //}
            //else
            //{
            //    SolidBrush co;
            //    switch ((MiniMapState)miniMap[e.ItemIndex, 0])
            //    {
            //        case MiniMapState.DontSave:
            //            co = new SolidBrush(taskt.Core.Theme.scriptTexts["number-dontsaved"].BackColor);
            //            break;
            //        case MiniMapState.NewInserted:
            //            co = new SolidBrush(taskt.Core.Theme.scriptTexts["number-newline"].BackColor);
            //            break;
            //        default:
            //            co = new SolidBrush(taskt.Core.Theme.scriptTexts["number-normal"].BackColor);
            //            break;
            //    }
            //    int baseX = modifiedBounds.X + mapX;
            //    e.Graphics.DrawLine(new Pen(Color.White), baseX, modifiedBounds.Y, baseX, modifiedBounds.Y + modifiedBounds.Height);
            //    baseX += 1;
            //    e.Graphics.FillRectangle(co, baseX, modifiedBounds.Y, 3, modifiedBounds.Height);

            //    switch ((MiniMapState)miniMap[e.ItemIndex, 1])
            //    {
            //        case MiniMapState.Cursor:
            //            co = new SolidBrush(taskt.Core.Theme.scriptTexts["selected-normal"].BackColor);
            //            break;
            //        case MiniMapState.Matched:
            //            co = new SolidBrush(taskt.Core.Theme.scriptTexts["match"].FontColor);
            //            break;
            //        case MiniMapState.Error:
            //            co = new SolidBrush(taskt.Core.Theme.scriptTexts["invalid"].FontColor);
            //            break;
            //        case MiniMapState.Comment:
            //            co = new SolidBrush(taskt.Core.Theme.scriptTexts["comment"].FontColor);
            //            break;
            //        default:
            //            co = new SolidBrush(taskt.Core.Theme.scriptTexts["normal"].BackColor);
            //            break;
            //    }
            //    baseX += 3;
            //    e.Graphics.DrawLine(new Pen(Color.White), baseX, modifiedBounds.Y, baseX, modifiedBounds.Y + modifiedBounds.Height);
            //    baseX += 1;
            //    e.Graphics.FillRectangle(co, baseX, modifiedBounds.Y, 3, modifiedBounds.Height);
            //}

            if (appSettings.ClientSettings.ShowScriptMiniMap)
            {
                modifiedBounds.X -= indentPixels;
                int baseX2 = modifiedBounds.Width - 8;
                e.Graphics.DrawImage(miniMapImg, new Rectangle(modifiedBounds.X + baseX2, modifiedBounds.Y, 8, modifiedBounds.Height),
                                                    new Rectangle(0, modifiedBounds.Y, 8, modifiedBounds.Height), GraphicsUnit.Pixel);
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
        private string decideLineNumberText(Core.Automation.Commands.ScriptCommand command)
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

                //FormatCommandListView();
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
                //FormatCommandListView();
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
            else if (e.KeyCode == Keys.F1)
            {
                if (lstScriptActions.SelectedItems.Count > 0)
                {
                    showThisCommandHelp((Core.Automation.Commands.ScriptCommand)lstScriptActions.SelectedItems[0].Tag);
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

        private void whereThisCommandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchSelectedCommand();
        }

        private void ViewJSONCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var currentCommand = lstScriptActions.SelectedItems[0].Tag;

            var jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(currentCommand, new Newtonsoft.Json.JsonSerializerSettings() { TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All });

            using (var dialog = new Supplemental.frmDialog(jsonText, "JSON Command Code", Supplemental.frmDialog.DialogType.OkOnly, 0))
            {
                dialog.ShowDialog();
            }
        }

        private void ViewXMLCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var currentCommand = (taskt.Core.Automation.Commands.ScriptCommand)lstScriptActions.SelectedItems[0].Tag;

            string scriptXML = taskt.Core.Script.Script.SerializeScript(new List<Core.Automation.Commands.ScriptCommand>() { currentCommand });

            int startIdx = scriptXML.IndexOf("<ScriptCommand ");

            int endIdx = scriptXML.IndexOf("</ScriptCommand>");
            if (endIdx < 0)
            {
                endIdx = scriptXML.IndexOf("</ScriptAction>");
            }

            string commandXML = scriptXML.Substring(startIdx, endIdx - startIdx - 1).Trim();

            using (var dialog = new Supplemental.frmDialog(commandXML, "XML Command Code", Supplemental.frmDialog.DialogType.OkOnly, 0))
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
            //if (lstScriptActions.SelectedItems.Count > 0)
            //{
            //    showThisCommandHelp((Core.Automation.Commands.ScriptCommand)lstScriptActions.SelectedItems[0].Tag);
            //}
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
        
        private void pnlStatus_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.FromArgb(59, 59, 59));
            //e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(59, 59, 59)), e.ClipRectangle);
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

        #region Open, Save, Parse, Import, Validate File
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
            // check file exists
            if (!System.IO.File.Exists(filePath))
            {
                using(var fm = new UI.Forms.Supplemental.frmDialog(filePath + " does not exits.", "Open Error", Supplemental.frmDialog.DialogType.OkOnly, 0))
                {
                    fm.ShowDialog();
                    return;
                }
            }

            try
            {
                //reinitialize
                lstScriptActions.Items.Clear();
                scriptVariables = new List<Core.Script.ScriptVariable>();
                scriptInfo = null;
                instanceList = new Core.InstanceCounter(appSettings);

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
            // check file exists
            if (!System.IO.File.Exists(filePath))
            {
                using (var fm = new UI.Forms.Supplemental.frmDialog(filePath + " does not exits.", "Open Error", Supplemental.frmDialog.DialogType.OkOnly, 0))
                {
                    fm.ShowDialog();
                    return;
                }
            }

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

                lstScriptActions.BeginUpdate();

                //comment
                lstScriptActions.Items.Add(CreateScriptCommandListViewItem(new Core.Automation.Commands.CommentCommand() { v_Comment = "Imported From " + fileName + " @ " + dateTimeNow }));
                //import
                PopulateExecutionCommands(deserializedScript.Commands, false);
                foreach (Core.Script.ScriptVariable var in deserializedScript.Variables)
                {
                    if (scriptVariables.Find(alreadyExists => alreadyExists.VariableName == var.VariableName) == null)
                    {
                        scriptVariables.Add(var);
                    }
                }

                // validate imported commands
                CheckValidateCommands(deserializedScript.Commands.Select(t => t.ScriptCommand).ToList());

                //comment
                lstScriptActions.Items.Add(CreateScriptCommandListViewItem(new Core.Automation.Commands.CommentCommand() { v_Comment = "End Import From " + fileName + " @ " + dateTimeNow }));

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

        public void PopulateExecutionCommands(List<Core.Script.ScriptAction> commandDetails, bool isOpen = true)
        {
            foreach (Core.Script.ScriptAction item in commandDetails)
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
                if ((item.Tag is Core.Automation.Commands.BeginListLoopCommand) || (item.Tag is Core.Automation.Commands.BeginContinousLoopCommand) ||(item.Tag is Core.Automation.Commands.BeginNumberOfTimesLoopCommand) || (item.Tag is Core.Automation.Commands.BeginLoopCommand) || (item.Tag is Core.Automation.Commands.BeginMultiLoopCommand))
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

            // check file path is only filename
            if (System.IO.Path.GetFileName(this.ScriptFilePath) == this.ScriptFilePath)
            {
                this.ScriptFilePath = appSettings.ClientSettings.RootFolder + "\\" + this.ScriptFilePath;
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

        private void CheckValidateCommands(List<Core.Automation.Commands.ScriptCommand> commands)
        {
            using (var fm = new UI.Forms.frmCommandEditor(automationCommands, GetConfiguredCommands()))
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
                //this.ScriptFilePath = System.IO.Path.GetFileName(filePath);
                this.ScriptFilePath = filePath;
            }
            else
            {
                this.ScriptFilePath = null;
            }
            ChangeSaveState(!normalFileOpen);
            return true;
        }

        private void AddInstanceName(List<Core.Automation.Commands.ScriptCommand> items)
        {
            foreach(Core.Automation.Commands.ScriptCommand command in items)
            {
                //instanceList.addInstance(command);
                command.addInstance(instanceList);
            }
        }

        private void RemoveInstanceName(List<Core.Automation.Commands.ScriptCommand> items)
        {
            foreach (Core.Automation.Commands.ScriptCommand command in items)
            {
                //instanceList.removeInstance(command);
                command.removeInstance(instanceList);
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
            // DBG
            //MessageBox.Show(specificCommand);

            //bring up new command configuration form
            using (var newCommandForm = new UI.Forms.frmCommandEditor(automationCommands, GetConfiguredCommands(), this.bufferedCommandList, this.bufferedCommandTreeImages))
            {
                newCommandForm.creationMode = UI.Forms.frmCommandEditor.CreationMode.Add;
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
                    var selectedComamnd = (Core.Automation.Commands.ScriptCommand)newCommandForm.selectedCommand;
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
            //if (tvCommands.SelectedNode.Level == 1)
            //{
            //    if (tvCommands.SelectedNode.Nodes.Count > 0)
            //    {
            //        return;
            //    }
            //    else
            //    {
            //        AddNewCommand(tvCommands.SelectedNode.Parent.Text + " - " + tvCommands.SelectedNode.Text);
            //    }
            //}
            //else
            //{
            //    // maybe level == 2
            //    AddNewCommand(tvCommands.SelectedNode.Parent.Parent.Text + " - " + tvCommands.SelectedNode.Text);
            //}
            string commandName = GetSelectedCommandFullName();
            if (commandName.Length > 0)
            {
                AddNewCommand(commandName);
            }
        }
        private string GetSelectedCommandFullName()
        {
            //switch (tvCommands.SelectedNode.Level)
            //{
            //    case 0:
            //        return "";
            //        break;

            //    case 1:
            //        if (tvCommands.SelectedNode.Nodes.Count > 0)
            //        {
            //            return "";
            //        }
            //        else if (tvCommands.SelectedNode.ImageIndex == 1)
            //        {
            //            return "";
            //        }
            //        else
            //        {
            //            return tvCommands.SelectedNode.Parent.Text + " - " + tvCommands.SelectedNode.Text;
            //        }
            //        break;

            //    case 2:
            //        return tvCommands.SelectedNode.Parent.Parent.Text + " - " + tvCommands.SelectedNode.Text;
            //        break;

            //    default:
            //        return "";
            //        break;
            //}
            return taskt.Core.CommandsTreeControls.GetSelectedFullCommandName(tvCommands);
        }
        #endregion


        #region TreeView Events
        private void GenerateTreeViewCommands()
        {
            bufferedCommandList = taskt.Core.CommandsTreeControls.CreateAllCommandsArray(appSettings.ClientSettings);
            bufferedCommandTreeImages = taskt.Core.CommandsTreeControls.CreateCommandImageList();
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
            TreeNode[] filterdCommands = taskt.Core.CommandsTreeControls.FilterCommands(keyword, bufferedCommandList, appSettings.ClientSettings);

            taskt.Core.CommandsTreeControls.ShowCommandsTree(tvCommands, filterdCommands, true);

            clearCmdTVCommandMenuStrip.Enabled = true;
            clearRootTVCommandMenuStrip.Enabled = true;
        }
        private void ShowAllCommands()
        {
            txtCommandFilter.Text = "";

            taskt.Core.CommandsTreeControls.ShowCommandsTree(tvCommands, bufferedCommandList);

            clearCmdTVCommandMenuStrip.Enabled = false;
            clearRootTVCommandMenuStrip.Enabled = false;
        }

        private void SearchSelectedCommand()
        {
            if (lstScriptActions.SelectedItems.Count == 0)
            {
                return;
            }

            var command = (Core.Automation.Commands.ScriptCommand)lstScriptActions.SelectedItems[0].Tag;
            var tp = command.GetType();
            var group = (Core.Automation.Attributes.ClassAttributes.Group)tp.GetCustomAttribute(typeof(Core.Automation.Attributes.ClassAttributes.Group));

            //TreeNode parentNode = null;
            //foreach (TreeNode node in tvCommands.Nodes)
            //{
            //    if (node.Text == group.groupName)
            //    {
            //        parentNode = node;
            //        break;
            //    }
            //}
            //if (parentNode != null)
            //{
            //    parentNode.Expand();
            //    foreach (TreeNode node in parentNode.Nodes)
            //    {
            //        if (node.Nodes.Count > 0)
            //        {
            //            foreach (TreeNode no in node.Nodes)
            //            {
            //                if (no.Text == command.SelectionName)
            //                {
            //                    node.Expand();
            //                    tvCommands.SelectedNode = no;
            //                    tvCommands.Focus();
            //                    break;
            //                }
            //            }
            //        }
            //        else
            //        {
            //            if (node.Text == command.SelectionName)
            //            {
            //                tvCommands.SelectedNode = node;
            //                tvCommands.Focus();
            //                break;
            //            }
            //        }
            //    }
            //}
            taskt.Core.CommandsTreeControls.FocusCommand(group.groupName, command.SelectionName, tvCommands);
        }

        #endregion


        #region Variable Edit, Settings form
        private void showVariableManager()
        {
            using (UI.Forms.frmScriptVariables scriptVariableEditor = new UI.Forms.frmScriptVariables(this.scriptVariables, this.appSettings))
            {
                //scriptVariableEditor.appSettings = this.appSettings;
                //scriptVariableEditor.scriptVariables = this.scriptVariables;

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
        private void showNewSettingForm()
        {
            using (frmNewSettings newSettings = new frmNewSettings(this))
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
            using (frmScriptInformations frm = new frmScriptInformations())
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
            ////show settings dialog
            //using (frmSettings newSettings = new frmSettings(this))
            //{
            //    newSettings.ShowDialog();

            //    //reload app settings
            //    appSettings = new Core.ApplicationSettings();
            //    appSettings = appSettings.GetOrCreateApplicationSettings();

            //    //reinit
            //    Core.Server.HttpServerClient.Initialize();
            //}
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
            //if (txtCommandSearch.Text != "" || tsSearchBox.Text != "")
            //{
            //    //reqdIndex++;
            //    SearchForItemInListView();
            //}
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
            using (var fm = new Forms.frmSample(this))
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
            frmSearch.CurrentMode = Supplement_Forms.frmSearchCommands.SearchReplaceMode.Search;
            frmSearch.variables = getAllVariablesNames();
            frmSearch.Show();
        }

        private void ReplaceStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSearch.CurrentMode = Supplement_Forms.frmSearchCommands.SearchReplaceMode.Replace;
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
        private void scriptInformationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showScriptInformationForm();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ////show settings dialog
            //using (frmSettings newSettings = new frmSettings(this))
            //{
            //    newSettings.ShowDialog();
            //}

            ////reload app settings
            //appSettings = new Core.ApplicationSettings();
            //appSettings = appSettings.GetOrCreateApplicationSettings();

            ////reinit
            //Core.Server.HttpServerClient.Initialize();
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
            //string bk_ScriptFilePath = this.ScriptFilePath;

            //// create templete script file
            //string cur = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff");
            //Random rnd = new Random();
            //do
            //{
            //    int v = rnd.Next();
            //    if (!System.IO.File.Exists(this.ScriptFilePath + cur + "-" + v.ToString() + ".xml"))
            //    {
            //        this.ScriptFilePath += cur + "-" + v.ToString() + ".xml";
            //        break;
            //    }
            //} while (true);
            //BeginSaveScriptProcess(false);

            BeginRunScriptProcess();

            //// remove templete script file
            //System.IO.File.Delete(this.ScriptFilePath);

            //this.ScriptFilePath = bk_ScriptFilePath;
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

            var fm = new taskt.UI.Forms.Supplement_Forms.frmMultiSendKeystrokes(appSettings, scriptVariables);
            fm.ShowDialog();
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
                    using(var fm = new UI.Forms.Supplemental.frmDialog("This file type can not open.", "File Open Error", Supplemental.frmDialog.DialogType.OkOnly, 0))
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
            using (var frm = new frmSample(this, commandName))
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
                AdvancedSearchItemInCommands(((Core.Automation.Commands.ScriptCommand)cmd.Command).SelectionName, false, false, true, false, false, false, "");
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
        public void setCommandEditorSizeAndPosition(Forms.frmCommandEditor editor)
        {
            if (editor == null)
            {
                return;
            }

            this.lastEditorSize = editor.Size;
            this.lastEditorPosition = editor.Location;
        }

        #endregion

        private void multiSendKeystrokesEditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var commands = Supplement_Forms.frmMultiSendKeystrokes.GetConsecutiveSendKeystrokesCommands(lstScriptActions, appSettings);
            if (commands.Count == 0)
            {
                return;
            }

            using(var fm = new Supplement_Forms.frmMultiSendKeystrokes(appSettings, scriptVariables, commands))
            {
                if (fm.ShowDialog() == DialogResult.OK)
                {
                    ChangeSaveState(true);

                    var newXMLCommands = taskt.Core.Script.Script.SerializeScript(fm.SendKeystrokesCommands());

                    // remove current
                    int selectedIndex = lstScriptActions.SelectedIndices[0];
                    for (int i = selectedIndex + commands.Count - 1; i >= selectedIndex; i--)
                    {
                        lstScriptActions.Items.RemoveAt(i);
                    }

                    int startIndex = (selectedIndex > 0) ? selectedIndex - 1 : 0;

                    var newCommands = taskt.Core.Script.Script.DeserializeScript(newXMLCommands);
                    InsertExecutionCommands(newCommands.Commands, startIndex);
                }
            }
        }
    }

}

