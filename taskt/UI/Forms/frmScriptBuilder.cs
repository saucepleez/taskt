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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using taskt.Core;
using taskt.Core.Automation.Commands;
using taskt.Core.Settings;
using taskt.UI.CustomControls;
using taskt.Core.Script;
using taskt.Core.Server;
using taskt.UI.Forms.Supplement_Forms;
using taskt.Core.IO;
using Newtonsoft.Json;

namespace taskt.UI.Forms
{
    public partial class frmScriptBuilder : Form
    //Form tracks the overall configuration and enables script editing, saving, and running
    //Features ability to add, drag/drop reorder commands
    {
        #region Instance Variables
        private List<ListViewItem> _rowsSelectedForCopy;
        private List<ScriptVariable> _scriptVariables;
        private List<AutomationCommand> _automationCommands;
        private bool _editMode;
        private ImageList _uiImages;
        private ApplicationSettings _appSettings;
        private List<List<ListViewItem>> _undoList;
        private DateTime _lastAntiIdleEvent;
        private int _undoIndex = -1;
        private int _reqdIndex;
        private int _selectedIndex = -1;
        private List<int> _matchingSearchIndex = new List<int>();
        private int _currentIndex = -1;
        private frmScriptBuilder _parentBuilder;

        private string _scriptFilePath;
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
        private Project _scriptProject;
        private string _scriptProjectPath;
        private int _debugLine;
        public int DebugLine
        {
            get
            {
                return _debugLine;
            }
            set
            {
                _debugLine = value;
                if (_debugLine > 0)
                {
                    try
                    {
                        lstScriptActions.EnsureVisible(_debugLine - 1);
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
        private List<string> _notificationList = new List<string>();
        private DateTime _notificationExpires;
        private bool _isDisplaying;
        private string _notificationText;
        #endregion

        #region Form Events
        public frmScriptBuilder()
        {
            InitializeComponent();
        }

        private void UpdateWindowTitle()
        {
            if (ScriptFilePath != null)
            {
                FileInfo scriptFileInfo = new FileInfo(ScriptFilePath);
                Text = "taskt - (Project: " + _scriptProject.GetProjectName() + " - Script: " + scriptFileInfo.Name + ")";
            }
            else if (_scriptProject.GetProjectName() != null)
            {
                Text = "taskt - (Project: " + _scriptProject.GetProjectName() + ")";
            }
            else
            {
                Text = "taskt";
            }
        }

        private void frmScriptBuilder_Load(object sender, EventArgs e)
        {
            //load all commands
            _automationCommands = CommandControls.GenerateCommandsandControls();

            //set controls double buffered
            foreach (Control control in Controls)
            {
                typeof(Control).InvokeMember("DoubleBuffered",
                    BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                    null, control, new object[] { true });
            }

            //create undo list
            _undoList = new List<List<ListViewItem>>();

            //get app settings
            _appSettings = new ApplicationSettings();
            _appSettings = _appSettings.GetOrCreateApplicationSettings();

            if (_appSettings.ServerSettings.ServerConnectionEnabled && _appSettings.ServerSettings.HTTPGuid == Guid.Empty)
            {
                HttpServerClient.GetGuid();
            }
            else if (_appSettings.ServerSettings.ServerConnectionEnabled && _appSettings.ServerSettings.HTTPGuid != Guid.Empty)
            {
                HttpServerClient.CheckIn();
            }

            HttpServerClient.AssociatedBuilder = this;

            LocalTCPClient.Initialize(this);
            //Core.Sockets.SocketClient.Initialize();
            //Core.Sockets.SocketClient.associatedBuilder = this;

            //handle action bar preference
            //hide action panel

            if (_editMode)
            {
                tlpControls.RowStyles[1].SizeType = SizeType.Absolute;
                tlpControls.RowStyles[1].Height = 0;

                tlpControls.RowStyles[2].SizeType = SizeType.Absolute;
                tlpControls.RowStyles[2].Height = 81;
            }
            else if (_appSettings.ClientSettings.UseSlimActionBar)
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
            var rpaScriptsFolder = Folders.GetFolder(Folders.FolderType.ScriptsFolder);

            if (!Directory.Exists(rpaScriptsFolder))
            {
                frmDialog userDialog = new frmDialog("Would you like to create a folder to save your scripts in now? " +
                    "A script folder is required to save scripts generated with this application. " +
                    "The new script folder path would be '" + rpaScriptsFolder + "'.", "Unable to locate Script Folder!",
                    frmDialog.DialogType.YesNo, 0);

                if (userDialog.ShowDialog() == DialogResult.OK)
                {
                    Directory.CreateDirectory(rpaScriptsFolder);
                }
            }

            //get latest files for recent files list on load
            GenerateRecentFiles();

            //no height for status bar
            HideNotificationRow();

            //instantiate for script variables
            if (!_editMode)
            {
                _scriptVariables = new List<ScriptVariable>();
            }
            //pnlHeader.BackColor = Color.FromArgb(255, 214, 88);

            //instantiate and populate display icons for commands
            _uiImages = Images.UIImageList();

            //set image list
            lstScriptActions.SmallImageList = _uiImages;

            //set listview column size
            frmScriptBuilder_SizeChanged(null, null);

            var groupedCommands = _automationCommands.GroupBy(f => f.DisplayGroup);

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

            tvCommands.Sort();
            //tvCommands.ImageList = uiImages;

            //start attended mode if selected
            if (_appSettings.ClientSettings.StartupMode == "Attended Task Mode")
            {
                WindowState = FormWindowState.Minimized;
                var frmAttended = new frmAttendedMode();
                frmAttended.Show();
            }
        }

        private void GenerateRecentFiles()
        {
            flwRecentFiles.Controls.Clear();

            var scriptPath = Folders.GetFolder(Folders.FolderType.ScriptsFolder);

            if (!Directory.Exists(scriptPath))
            {
                lblRecentFiles.Text = "Script Folder does not exist";
                lblFilesMissing.Text = "Directory Not Found: " + scriptPath;
                lblRecentFiles.ForeColor = Color.White;
                lblFilesMissing.ForeColor = Color.White;
                lblFilesMissing.Show();
                flwRecentFiles.Hide();
                return;
            }

            var directory = new DirectoryInfo(scriptPath);
            var recentFiles = directory.GetFiles()
                                       .OrderByDescending(file => file.LastWriteTime)
                                       .Select(f => f.Name);

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
                foreach (var fil in recentFiles)
                {
                    if (flwRecentFiles.Controls.Count == 7)
                        return;

                    LinkLabel newFileLink = new LinkLabel();
                    newFileLink.Text = fil;
                    newFileLink.AutoSize = true;
                    newFileLink.LinkColor = Color.AliceBlue;
                    newFileLink.Font = lnkGitIssue.Font;
                    newFileLink.Margin = new Padding(0, 0, 0, 0);
                    newFileLink.LinkClicked += NewFileLink_LinkClicked;
                    flwRecentFiles.Controls.Add(newFileLink);
                }
            }
        }

        private void frmScriptBuilder_Shown(object sender, EventArgs e)
        {
            Program.SplashForm.Hide();

            if (_editMode)
                return;

            AddProject();
            Notify("Welcome! Press 'Add Command' to get started!");
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

        private void lblMainLogo_Click(object sender, EventArgs e)
        {
            frmAbout aboutForm = new frmAbout();
            aboutForm.Show();
        }

        private void lstScriptActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_appSettings.ClientSettings.InsertCommandsInline)
                return;

            //check to see if an item has been selected last
            if (lstScriptActions.SelectedItems.Count > 0)
            {
                _selectedIndex = lstScriptActions.SelectedItems[0].Index;
                //FormatCommandListView();
            }
            else
            {
                //nothing is selected
                _selectedIndex = -1;
            }

        }

        private void frmScriptBuilder_SizeChanged(object sender, EventArgs e)
        {
            lstScriptActions.Columns[2].Width = Width - 340;
        }

        private void frmScriptBuilder_Resize(object sender, EventArgs e)
        {
            //check when minimized
            if ((WindowState == FormWindowState.Minimized) && (_appSettings.ClientSettings.MinimizeToTray))
            {
                _appSettings = new ApplicationSettings().GetOrCreateApplicationSettings();
                if (_appSettings.ClientSettings.MinimizeToTray)
                {
                    notifyTray.Visible = true;
                    notifyTray.ShowBalloonTip(3000);
                    ShowInTaskbar = false;
                }
            }
            pnlMain.Invalidate();
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
            if ((dragToItem.Tag is SequenceCommand) && (_appSettings.ClientSettings.EnableSequenceDragDrop))
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
            //    if (command.Tag is EndLoopCommand)
            //    {
            //        for (int i = 0; i < dragIndex; i++)
            //        {
            //            if (lstScriptActions.Items[i].Tag is BeginLoopCommand)
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

        #region ListView Copy, Paste, Edit, Delete
        private void lstScriptActions_KeyDown(object sender, KeyEventArgs e)
        {
            //delete from listview if required
            if (e.KeyCode == Keys.Delete)
            {
                foreach (ListViewItem itm in lstScriptActions.SelectedItems)
                {
                    lstScriptActions.Items.Remove(itm);
                }

                CreateUndoSnapshot();
                lstScriptActions.Invalidate();
                //FormatCommandListView();
            }
            else if(e.KeyCode == Keys.Enter)
            {
                //if user presses enter simulate double click event
                lstScriptActions_DoubleClick(null, null);
            }
            else if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.X:
                        CutRows();
                        break;
                    case Keys.C:
                        CopyRows();
                        break;
                    case Keys.V:
                        PasteRows();
                        break;
                    case Keys.Z:
                        UndoChange();
                        break;
                    case Keys.R:
                        RedoChange();
                        break;
                    case Keys.A:
                        foreach (ListViewItem item in lstScriptActions.Items)
                        {
                            item.Selected = true;
                        }
                        break;
                }
            }
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
            var currentCommand = (ScriptCommand)selectedCommandItem.Tag;

            //check if editing a sequence
            if (currentCommand is SequenceCommand)
            {
                if (_editMode)
                {
                    MessageBox.Show("Embedding Sequence Commands within Sequence Commands not yet supported.");
                    return;
                }

                //get sequence events
                SequenceCommand sequence = (SequenceCommand)currentCommand;
                frmScriptBuilder newBuilder = new frmScriptBuilder();

                //add variables
                newBuilder._scriptVariables = new List<ScriptVariable>();

                foreach (var variable in _scriptVariables)
                {
                    newBuilder._scriptVariables.Add(variable);
                }

                //append to new builder
                foreach (var cmd in sequence.v_scriptActions)
                {
                    newBuilder.lstScriptActions.Items.Add(CreateScriptCommandListViewItem(cmd));
                }

                //apply editor style format
                newBuilder.ApplyEditorFormat();

                newBuilder._parentBuilder = this;

                //if data has been changed
                if (newBuilder.ShowDialog() == DialogResult.OK)
                {
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

                    //update label
                    selectedCommandItem.Text = sequence.GetDisplayValue();
                }
            }
            else
            {
                //create new command editor form
                frmCommandEditor editCommand = new frmCommandEditor(_automationCommands, GetConfiguredCommands());

                //creation mode edit locks form to current command
                editCommand.CreationModeInstance = frmCommandEditor.CreationMode.Edit;

                //editCommand.defaultStartupCommand = currentCommand.SelectionName;
                editCommand.EditingCommand = currentCommand;

                //create clone of current command so databinding does not affect if changes are not saved
                editCommand.OriginalCommand = Common.Clone(currentCommand);

                //set variables
                editCommand.ScriptVariables = _scriptVariables;

                //show edit command form and save changes on OK result
                if (editCommand.ShowDialog() == DialogResult.OK)
                {
                    selectedCommandItem.Tag = editCommand.SelectedCommand;
                    selectedCommandItem.Text = editCommand.SelectedCommand.GetDisplayValue(); //+ "(" + cmdDetails.SelectedVariables() + ")";
                    selectedCommandItem.SubItems.Add(editCommand.SelectedCommand.GetDisplayValue());
                }
            }
        }

        private void ApplyEditorFormat()
        {
            _editMode = true;
            Text = "edit sequence";
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
        }

        private void CutRows()
        {
            //initialize list of items to copy
            if (_rowsSelectedForCopy == null)
            {
                _rowsSelectedForCopy = new List<ListViewItem>();
            }
            else
            {
                _rowsSelectedForCopy.Clear();
            }

            //copy into list for all selected
            if (lstScriptActions.SelectedItems.Count >= 1)
            {
                foreach (ListViewItem item in lstScriptActions.SelectedItems)
                {
                    _rowsSelectedForCopy.Add(item);
                    lstScriptActions.Items.Remove(item);
                }

                Notify(_rowsSelectedForCopy.Count + " item(s) cut to clipboard!");
            }
        }

        private void CopyRows()
        {

            //initialize list of items to copy
            if (_rowsSelectedForCopy == null)
            {
                _rowsSelectedForCopy = new List<ListViewItem>();
            }
            else
            {
                _rowsSelectedForCopy.Clear();
            }

            //copy into list for all selected
            if (lstScriptActions.SelectedItems.Count >= 1)
            {
                foreach (ListViewItem item in lstScriptActions.SelectedItems)
                {
                    _rowsSelectedForCopy.Add(item);
                }

                Notify(_rowsSelectedForCopy.Count + " item(s) copied to clipboard!");
            }
        }

        private void PasteRows()
        {
            if (_rowsSelectedForCopy != null)
            {

                if (lstScriptActions.SelectedItems.Count == 0)
                {
                    MessageBox.Show("In order to paste, you must first select a command to paste under.",
                        "Select Command To Paste Under");
                    return;
                }

                int destinationIndex = lstScriptActions.SelectedItems[0].Index + 1;

                foreach (ListViewItem item in _rowsSelectedForCopy)
                {
                    ScriptCommand duplicatedCommand = (ScriptCommand)Common.Clone(item.Tag);
                    duplicatedCommand.GenerateID();
                    lstScriptActions.Items.Insert(destinationIndex, CreateScriptCommandListViewItem(duplicatedCommand));
                    destinationIndex += 1;
                }

                lstScriptActions.Invalidate();
                Notify(_rowsSelectedForCopy.Count + " item(s) pasted!");
            }
        }

        private void UndoChange()
        {
            if (_undoList.Count > 0)
            {
                if ((_undoIndex < 0) || (_undoIndex >= _undoList.Count))
                {
                    _undoIndex = _undoList.Count - 1;
                }

                lstScriptActions.Items.Clear();

                foreach (ListViewItem rowItem in _undoList[_undoIndex])
                {
                    lstScriptActions.Items.Add(rowItem);
                }

                _undoIndex--;

                lstScriptActions.Invalidate();
            }
        }

        private void RedoChange()
        {
            if (_undoList.Count > 0)
            {
                _undoIndex++;

                if (_undoIndex > _undoList.Count - 1)
                {
                    _undoIndex = _undoList.Count - 1;
                }

                lstScriptActions.Items.Clear();

                foreach (ListViewItem rowItem in _undoList[_undoIndex])
                {
                    lstScriptActions.Items.Add(rowItem);
                }

                lstScriptActions.Invalidate();
            }
        }

        private void CreateUndoSnapshot()
        {
            List<ListViewItem> itemList = new List<ListViewItem>();
            foreach (ListViewItem rowItem in lstScriptActions.Items)
            {
                itemList.Add(rowItem);
            }

            _undoList.Add(itemList);

            if (_undoList.Count > 10)
            {
                _undoList.RemoveAt(0);
            }

            _undoIndex = itemList.Count - 1;
        }
        #endregion

        #region ListView Create Item
        private ListViewItem CreateScriptCommandListViewItem(ScriptCommand cmdDetails)
        {
            ListViewItem newCommand = new ListViewItem();
            newCommand.Text = cmdDetails.GetDisplayValue();
            newCommand.SubItems.Add(cmdDetails.GetDisplayValue());
            newCommand.SubItems.Add(cmdDetails.GetDisplayValue());
            //cmdDetails.RenderedControls = null;
            newCommand.Tag = cmdDetails;
            newCommand.ForeColor = cmdDetails.DisplayForeColor;
            newCommand.BackColor = Color.DimGray;
            newCommand.ImageIndex = _uiImages.Images.IndexOfKey(cmdDetails.GetType().Name);
            return newCommand;
        }
        #endregion

        #region ListView Comment, Coloring, ToolStrip
        private void IndentListViewItems()
        {
            int indent = 0;
            foreach (ListViewItem rowItem in lstScriptActions.Items)
            {
                if (rowItem is null)
                {
                    continue;
                }

                if ((rowItem.Tag is BeginIfCommand) || (rowItem.Tag is BeginMultiIfCommand) ||
                    (rowItem.Tag is LoopListCommand) || (rowItem.Tag is LoopContinuouslyCommand) ||
                    (rowItem.Tag is LoopNumberOfTimesCommand) || (rowItem.Tag is TryCommand) ||
                    (rowItem.Tag is BeginLoopCommand) || (rowItem.Tag is BeginMultiLoopCommand))
                {
                    indent += 2;
                    rowItem.IndentCount = indent;
                    indent += 2;
                }
                else if ((rowItem.Tag is EndLoopCommand) || (rowItem.Tag is EndIfCommand) ||
                    (rowItem.Tag is EndTryCommand))
                {
                    indent -= 2;
                    if (indent < 0) indent = 0;
                    rowItem.IndentCount = indent;
                    indent -= 2;
                    if (indent < 0) indent = 0;
                }
                else if ((rowItem.Tag is ElseCommand) || (rowItem.Tag is CatchExceptionCommand) ||
                    (rowItem.Tag is FinallyCommand))
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
            IndentListViewItems();

            //auto size line numbers based on command count
            AutoSizeLineNumberColumn();

            //get listviewitem
            ListViewItem item = e.Item;

            //get script command reference
            var command = (ScriptCommand)item.Tag;

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
                    var img = _uiImages.Images[command.GetType().Name];
                    if (img != null)
                    {
                        e.Graphics.DrawImage(img, modifiedBounds.Left, modifiedBounds.Top + 3);
                    }
                    break;
                case 2:
                    //write command text
                    Brush commandNameBrush, commandBackgroundBrush;
                    if ((_debugLine > 0) && (e.ItemIndex == _debugLine - 1))
                    {
                        //debugging coloring
                        commandNameBrush = Brushes.White;
                        commandBackgroundBrush = Brushes.OrangeRed;
                    }
                    else if ((_currentIndex >= 0) && (e.ItemIndex == _currentIndex))
                    {
                        //search primary item coloring
                        commandNameBrush = Brushes.Black;
                        commandBackgroundBrush = Brushes.Goldenrod;
                    }
                    else if (_matchingSearchIndex.Contains(e.ItemIndex))
                    {
                        //search match item coloring
                        commandNameBrush = Brushes.Black;
                        commandBackgroundBrush = Brushes.LightYellow;
                    }
                    else if ((e.Item.Focused) || (e.Item.Selected))
                    {
                        //selected item coloring
                        commandNameBrush = Brushes.White;
                        commandBackgroundBrush = Brushes.DodgerBlue;
                    }
                    else if (command.PauseBeforeExeucution)
                    {
                        //pause before execution coloring
                        commandNameBrush = Brushes.MediumPurple;
                        commandBackgroundBrush = Brushes.Lavender;
                    }
                    else if ((command is AddCodeCommentCommand) || (command.IsCommented))
                    {
                        //comments and commented command coloring
                        commandNameBrush = Brushes.ForestGreen;
                        commandBackgroundBrush = Brushes.WhiteSmoke;
                    }
                    else
                    {
                        //standard coloring
                        commandNameBrush = Brushes.SteelBlue;
                        commandBackgroundBrush = Brushes.WhiteSmoke;
                    }

                    //fille with background color
                    e.Graphics.FillRectangle(commandBackgroundBrush, modifiedBounds);

                    //get indent count
                    var indentPixels = (item.IndentCount * 15);

                    //set indented X position
                    modifiedBounds.X += indentPixels;

                    //draw string
                    e.Graphics.DrawString(command.GetDisplayValue(),
                                   lstScriptActions.Font, commandNameBrush, modifiedBounds);
                    break;
            }
        }

        private void lstScriptActions_MouseMove(object sender, MouseEventArgs e)
        {
            lstScriptActions.Invalidate();
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
                var selectedCommand = (ScriptCommand)item.Tag;
                selectedCommand.PauseBeforeExeucution = !selectedCommand.PauseBeforeExeucution;
            }

            //recolor
            //FormatCommandListView();
            lstScriptActions.Invalidate();

            //clear selection
            lstScriptActions.SelectedIndices.Clear();
        }

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
            if (!_appSettings.ClientSettings.InsertCommandsInline)
            {
                commandList.Reverse();
            }

            //add to parent
            commandList.ForEach(x => _parentBuilder.AddCommandToListView(x));
        }

        public void AddCommandToListView(ScriptCommand selectedCommand)
        {
            if (pnlCommandHelper.Visible)
            {
                pnlCommandHelper.Hide();
            }
            var command = CreateScriptCommandListViewItem(selectedCommand);

            //insert to end by default
            var insertionIndex = lstScriptActions.Items.Count;

            //verify setting to insert inline is selected and if an item is currently selected
            if ((_appSettings.ClientSettings.InsertCommandsInline) && (lstScriptActions.SelectedItems.Count > 0))
            {
                //insert inline
                insertionIndex = lstScriptActions.SelectedItems[0].Index + 1;
            }

            //insert command
            lstScriptActions.Items.Insert(insertionIndex, command);

            //special types also get a following command and comment
            if ((selectedCommand is LoopListCommand) || (selectedCommand is LoopContinuouslyCommand) ||
                (selectedCommand is LoopNumberOfTimesCommand) || (selectedCommand is BeginLoopCommand) ||
                (selectedCommand is BeginMultiLoopCommand))
            {
                lstScriptActions.Items.Insert(insertionIndex + 1, CreateScriptCommandListViewItem(new AddCodeCommentCommand()
                {
                    v_Comment = "Items in this section will run within the loop"
                }));
                lstScriptActions.Items.Insert(insertionIndex + 2, CreateScriptCommandListViewItem(new EndLoopCommand()));
            }
            else if ((selectedCommand is BeginIfCommand) || (selectedCommand is BeginMultiIfCommand))
            {
                lstScriptActions.Items.Insert(insertionIndex + 1, CreateScriptCommandListViewItem(new AddCodeCommentCommand()
                {
                    v_Comment = "Items in this section will run if the statement is true"
                }));
                lstScriptActions.Items.Insert(insertionIndex + 2, CreateScriptCommandListViewItem(new EndIfCommand()));
            }
            else if (selectedCommand is TryCommand)
            {
                lstScriptActions.Items.Insert(insertionIndex + 1, CreateScriptCommandListViewItem(new AddCodeCommentCommand()
                {
                    v_Comment = "Items in this section will be handled if error occurs"
                }));
                lstScriptActions.Items.Insert(insertionIndex + 2, CreateScriptCommandListViewItem(new CatchExceptionCommand()
                {
                    v_Comment = "Items in this section will run if error occurs"
                }));
                lstScriptActions.Items.Insert(insertionIndex + 3, CreateScriptCommandListViewItem(new AddCodeCommentCommand()
                {
                    v_Comment = "This section executes if error occurs above"
                }));
                lstScriptActions.Items.Insert(insertionIndex + 4, CreateScriptCommandListViewItem(new EndTryCommand()));
            }

            CreateUndoSnapshot();
            lstScriptActions.Invalidate();
            AutoSizeLineNumberColumn();
        }

        private void pnlStatus_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawString(_notificationText, pnlStatus.Font, Brushes.White, 30, 4);
            e.Graphics.DrawImage(Properties.Resources.message, 5, 3, 24, 24);
        }
        #endregion

        #region ListView Search
        private void txtCommandSearch_TextChanged(object sender, EventArgs e)
        {
            if (lstScriptActions.Items.Count == 0)
                return;

            _reqdIndex = 0;

            if (txtCommandSearch.Text == "")
            {
                //hide info
                HideSearchInfo();

                //clear indexes
                _matchingSearchIndex.Clear();
                _currentIndex = -1;

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

        private void pbSearch_Click(object sender, EventArgs e)
        {
            if (txtCommandSearch.Text != "" || tsSearchBox.Text != "")
            {
                _reqdIndex++;
                SearchForItemInListView();
            }
        }

        private void SearchForItemInListView()
        {
            var searchCriteria = txtCommandSearch.Text;

            if (searchCriteria == "")
            {
                searchCriteria = tsSearchBox.Text;
            }

            var matchingItems = lstScriptActions.Items.OfType<ListViewItem>()
                                                      .Where(x => x.Text.Contains(searchCriteria))
                                                      .ToList();

            int? matchCount = matchingItems.Count();
            int totalMatches = matchCount ?? 0;

            if ((_reqdIndex == matchingItems.Count) || (_reqdIndex < 0))
            {
                _reqdIndex = 0;
            }

            lblTotalResults.Show();

            if (totalMatches == 0)
            {
                _reqdIndex = -1;
                lblTotalResults.Text = "No Matches Found";
                lblCurrentlyViewing.Hide();
                //clear indexes
                _matchingSearchIndex.Clear();
                _reqdIndex = -1;
                lstScriptActions.Invalidate();
                return;
            }
            else
            {
                lblCurrentlyViewing.Text = "Viewing " + (_reqdIndex + 1) + " of " + totalMatches + "";
                tsSearchResult.Text = "Viewing " + (_reqdIndex + 1) + " of " + totalMatches + "";
                lblTotalResults.Text = totalMatches + " total results found";
            }

            _matchingSearchIndex = new List<int>();
            foreach (ListViewItem itm in matchingItems)
            {
                _matchingSearchIndex.Add(itm.Index);
                itm.BackColor = Color.LightGoldenrodYellow;
            }

            _currentIndex = matchingItems[_reqdIndex].Index;

            lstScriptActions.Invalidate();
            lstScriptActions.EnsureVisible(_currentIndex);
        }

        private void pbSearch_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void pbSearch_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }
        #endregion
        #endregion

        #region Bottom Notification Panel
        private void tmrNotify_Tick(object sender, EventArgs e)
        {
            if (_appSettings ==  null)
            {
                return;
            }

            if ((_notificationExpires < DateTime.Now) && (_isDisplaying))
            {
                HideNotification();
            }

            if ((_appSettings.ClientSettings.AntiIdleWhileOpen) && (DateTime.Now > _lastAntiIdleEvent.AddMinutes(1)))
            {
                PerformAntiIdle();
            }

            //check if notification is required
            if ((_notificationList.Count > 0) && (_notificationExpires < DateTime.Now))
            {
                var itemToDisplay = _notificationList[0];
                _notificationList.RemoveAt(0);
                _notificationExpires = DateTime.Now.AddSeconds(2);
                ShowNotification(itemToDisplay);
            }
        }

        public void Notify(string notificationText)
        {
            _notificationList.Add(notificationText);
        }

        private void ShowNotification(string textToDisplay)
        {
            _notificationText = textToDisplay;
            //lblStatus.Left = 20;
            //lblStatus.Text = textToDisplay;

            pnlStatus.SuspendLayout();
            //for (int i = 0; i < 30; i++)
            //{
            //    tlpControls.RowStyles[3].Height = i;
            //}
            ShowNotificationRow();
            pnlStatus.ResumeLayout();
            _isDisplaying = true;
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
            _isDisplaying = false;
        }

        private void HideNotificationRow()
        {
            tlpControls.RowStyles[5].Height = 0;
        }

        private void ShowNotificationRow()
        {
            tlpControls.RowStyles[5].Height = 30;
        }

        private void PerformAntiIdle()
        {
            _lastAntiIdleEvent = DateTime.Now;
            var mouseMove = new SendMouseMoveCommand();
            mouseMove.v_XMousePosition = (Cursor.Position.X + 1).ToString();
            mouseMove.v_YMousePosition = (Cursor.Position.Y + 1).ToString();
            Notify("Anti-Idle Triggered");
        }

        private void notifyTray_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (_appSettings.ClientSettings.MinimizeToTray)
            {
                WindowState = FormWindowState.Normal;
                ShowInTaskbar = true;
                notifyTray.Visible = false;
            }
        }
        #endregion

        #region Create Command Logic
        private void AddNewCommand(string specificCommand = "")
        {
            //bring up new command configuration form
            var newCommandForm = new frmCommandEditor(_automationCommands, GetConfiguredCommands());
            newCommandForm.CreationModeInstance = frmCommandEditor.CreationMode.Add;
            newCommandForm.ScriptVariables = _scriptVariables;
            if (specificCommand != "")
                newCommandForm.DefaultStartupCommand = specificCommand;

            //if a command was selected
            if (newCommandForm.ShowDialog() == DialogResult.OK)
            {
                //add to listview
                AddCommandToListView(newCommandForm.SelectedCommand);
            }
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
        #endregion

        #region TreeView Events
        private void tvCommands_DoubleClick(object sender, EventArgs e)
        {
            //exit if parent node is clicked
            if (tvCommands.SelectedNode.Parent == null)
            {
                return;
            }
            AddNewCommand(tvCommands.SelectedNode.Parent.Text + " - " + tvCommands.SelectedNode.Text);
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

        #region Link Labels
        private void lnkGitProject_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/saucepleez/taskt");
        }
        private void lnkGitLatestReleases_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/saucepleez/taskt/releases");
        }
        private void lnkGitIssue_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/saucepleez/taskt/issues/new");
        }
        private void lnkGitWiki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://wiki.taskt.net/");
        }
        private void NewFileLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel senderLink = (LinkLabel)sender;
            OpenFile(Folders.GetFolder(Folders.FolderType.ScriptsFolder) + senderLink.Text);
        }
        #endregion

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
            lstScriptActions.Items.Clear();
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
            pnlCommandHelper.Show();
        }

        private void uiBtnOpen_Click(object sender, EventArgs e)
        {
            //show ofd
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = _scriptProjectPath;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Filter = "Xml (*.xml)|*.xml";

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
            openFileDialog.Filter = "Xml (*.xml)|*.xml";

            //if user selected file
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //open file
                OpenFile(openFileDialog.FileName);
            }
        }

        private void OpenFile(string filePath)
        {
            try
            {
                //get deserialized script
                Script deserializedScript = Script.DeserializeFile(filePath);

                //check if script is a part of the currently opened project
                string openScriptProjectName = deserializedScript.ProjectName;

                if (openScriptProjectName != _scriptProject.GetProjectName())
                    throw new Exception("Attempted to load a script not part of the currently open project");

                //reinitialize
                lstScriptActions.Items.Clear();
                _scriptVariables = new List<ScriptVariable>();

                if (deserializedScript.Commands.Count == 0)
                {
                    Notify("Error Parsing File: Commands not found!");
                }

                //update file path and reflect in title bar
                ScriptFilePath = filePath;

                //assign ProjectPath variable
                var projectPathVariable = new ScriptVariable
                {
                    VariableName = "ProjectPath",
                    VariableValue = _scriptProjectPath
                };

                _scriptVariables.Add(projectPathVariable);

                //assign variables
                _scriptVariables.AddRange(deserializedScript.Variables);

                //populate commands
                PopulateExecutionCommands(deserializedScript.Commands);

                //notify
                Notify("Script Loaded Successfully!");
            }
            catch (Exception ex)
            {
                //signal an error has happened
                Notify("An Error Occured: " + ex.Message);
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
                if ((item.Tag is LoopListCommand) || (item.Tag is LoopContinuouslyCommand) || (item.Tag is LoopNumberOfTimesCommand) || (item.Tag is BeginLoopCommand) || (item.Tag is BeginMultiLoopCommand))
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
                else if (item.Tag is TryCommand)
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
            if ((ScriptFilePath == null) || (saveAs))
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = _scriptProjectPath;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.Filter = "Xml (*.xml)|*.xml";

                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                if (!saveFileDialog.FileName.ToString().Contains(_scriptProjectPath))
                {
                    Notify("An Error Occured: Attempted to save script outside of project directory");
                    return;
                }

                ScriptFilePath = saveFileDialog.FileName;
            }

            //serialize script
            try
            {
                var exportedScript = Script.SerializeScript(lstScriptActions.Items, _scriptVariables, ScriptFilePath, _scriptProject.GetProjectName());
                _scriptProject.SaveProject(ScriptFilePath, exportedScript);

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
            lstScriptActions.SelectedItems.Clear();
            _selectedIndex = -1;
            lstScriptActions.Invalidate();
        }

        private void uiBtnImport_Click(object sender, EventArgs e)
        {
            BeginImportProcess();
        }

        private void importFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BeginImportProcess();
        }

        private void btnSequenceImport_Click(object sender, EventArgs e)
        {
            BeginImportProcess();
        }

        private void BeginImportProcess()
        {
            //show ofd
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Folders.GetFolder(Folders.FolderType.ScriptsFolder);
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
                lstScriptActions.Items.Add(CreateScriptCommandListViewItem(new AddCodeCommentCommand()
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

                //comment
                lstScriptActions.Items.Add(CreateScriptCommandListViewItem(new AddCodeCommentCommand() { v_Comment = "End Import From " + fileName + " @ " + dateTimeNow }));

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
                lstScriptActions.Items.Add(CreateScriptCommandListViewItem(item.ScriptCommand));
                if (item.AdditionalScriptCommands.Count > 0)
                    PopulateExecutionCommands(item.AdditionalScriptCommands);
            }

            if (pnlCommandHelper.Visible)
            {
                pnlCommandHelper.Hide();
            }

        }

        private void restartApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            for (int i = 30; i > 0; i--)
            {
                tlpControls.RowStyles[4].Height = i;
            }
        }

        private void closeApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
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
            scriptVariableEditor.ScriptVariables = _scriptVariables;

            if (scriptVariableEditor.ShowDialog() == DialogResult.OK)
            {
                _scriptVariables = scriptVariableEditor.ScriptVariables;
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
            lstScriptActions.Items.Clear();
        }

        private void aboutTasktToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout frmAboutForm = new frmAbout();
            frmAboutForm.Show();
        }
        #endregion

        #region Script Actions Tool Strip and Buttons
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
            pnlCommandHelper.Hide();

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

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunScript();
        }

        private void uiBtnRunScript_Click(object sender, EventArgs e)
        {
            RunScript();
        }

        private void RunScript()
        {
            if (lstScriptActions.Items.Count == 0)
            {
                Notify("You must first build the script by adding commands!");
                return;
            }

            if (ScriptFilePath == null)
            {
                Notify("You must first save your script before you can run it!");
                return;
            }

            //clear selected items
            ClearSelectedListViewItems();
            SaveToFile(false); // Save & Run!
            Notify("Running Script..");

            frmScriptEngine newEngine = new frmScriptEngine(ScriptFilePath, this);

            //executionManager = new ScriptExectionManager();
            //executionManager.CurrentlyExecuting = true;
            //executionManager.ScriptName = new System.IO.FileInfo(ScriptFilePath).Name;

            newEngine.CallBackForm = this;
            newEngine.Show();
        }
        #endregion

        #region Save And Run Tool Strip and Buttons
        private void saveAndRunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveToolStripMenuItem_Click(null, null);
            runToolStripMenuItem_Click(null, null);
        }
        #endregion

        #region Save And Close Buttons
        private void uiBtnKeep_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void uiBtnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        #endregion

        #region View Code Tool Strip
        private void viewCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var currentCommand = lstScriptActions.SelectedItems[0].Tag;
            var jsonText = JsonConvert.SerializeObject(currentCommand, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            });
            var dialog = new frmDialog(jsonText, "Command Code", frmDialog.DialogType.OkOnly, 0);
            dialog.ShowDialog();
        }
        #endregion

        #region Project Builder in Tool Strip and Buttons
        private void addProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddProject();
        }

        private void uiBtnProject_Click(object sender, EventArgs e)
        {
            AddProject();
        }

        public void AddProject()
        {
            var projectBuilder = new Supplement_Forms.frmProjectBuilder();
            projectBuilder.ShowDialog();

            //Close taskt if add project form is closed at startup
            if (projectBuilder.DialogResult == DialogResult.Cancel && _scriptProject == null)
            {
                Application.Exit();
            }

            //Create new taskt project
            else if (projectBuilder.CreateProject == true)
            {
                _scriptProjectPath = projectBuilder.NewProjectPath;
                //Create new main script
                string mainScriptPath = Path.Combine(_scriptProjectPath, "Main.xml");
                lstScriptActions.Items.Clear();
                _scriptVariables = new List<ScriptVariable>();
                var helloWorldCommand = new ShowMessageCommand();
                helloWorldCommand.v_Message = "Hello World";
                lstScriptActions.Items.Insert(0, CreateScriptCommandListViewItem(helloWorldCommand));

                //Begin saving as main.xml
                ClearSelectedListViewItems();

                try
                {
                    //Serialize main script
                    var mainScript = Script.SerializeScript(lstScriptActions.Items, _scriptVariables, mainScriptPath, projectBuilder.NewProjectName);
                    //Create new project
                    Project proj = new Project(projectBuilder.NewProjectName);
                    //Save new project
                    proj.SaveProject(mainScriptPath, mainScript);
                    //Open new project
                    _scriptProject = Project.OpenProject(mainScriptPath);
                    //Open main script
                    OpenFile(mainScriptPath);
                    ScriptFilePath = mainScriptPath;
                    //Show success dialog
                    Notify("Project has been created successfully!");
                }
                catch (Exception ex)
                {
                    Notify("An Error Occured: " + ex.Message);
                }
            }

            //Open existing taskt project
            else if (projectBuilder.OpenProject == true)
            {
                try
                {
                    //Open project
                    _scriptProject = Project.OpenProject(projectBuilder.ExistingMainPath);
                    _scriptProjectPath = Path.GetDirectoryName(projectBuilder.ExistingMainPath);
                    //Open Main.xml
                    OpenFile(projectBuilder.ExistingMainPath);
                    //show success dialog
                    Notify("Project has been opened successfully!");
                }
                catch (Exception ex)
                {
                    //show fail dialog
                    Notify("An Error Occured: " + ex.Message);
                    //Try adding project again
                    AddProject();
                }
            }
        }
        #endregion
        #endregion
    }
}

