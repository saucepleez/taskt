//Copyright (c) 2017 Jason Bayldon
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

        private ListViewItem rowSelectedForCopy { get; set; }
        private List<Core.Script.ScriptVariable> scriptVariables;
        bool editMode { get; set;}
        private ImageList uiImages;
        private WebSocket4Net.WebSocket webSocket;
        private string publicKey;
        private string webSocketConnectionID;
        public Core.ApplicationSettings appSettings;
        private string streamedXMLData { get; set; }
        private List<List<ListViewItem>> undoList;
        private int undoIndex = -1;
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

            if ((webSocketConnectionID != "") && (webSocketConnectionID != null))
            {
                this.Text = this.Text + " [" + webSocketConnectionID + "]";
            }



        }

        private void frmScriptBuilder_Load(object sender, EventArgs e)
        {
            //detect latest release
            //HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.github.com/repos/saucepleez/taskt/releases");
            //myHttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2;)";
            //HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

            //StreamReader reader = new StreamReader(myHttpWebResponse.GetResponseStream(), Encoding.UTF8);
            //String responseString = reader.ReadToEnd();

            //Newtonsoft.Json.Linq.JArray jsonArray = Newtonsoft.Json.Linq.JArray.Parse(responseString);
            //dynamic data = Newtonsoft.Json.Linq.JObject.Parse(jsonArray[0].ToString());





            //create undo list
            undoList = new List<List<ListViewItem>>();

            //get app settings
            var appSettingClass = new Core.ApplicationSettings();
            appSettings = appSettingClass.GetOrCreateApplicationSettings();

            //get server setting preferences
            var serverSettings = appSettings.ServerSettings;


            //try to connect to server
            if ((serverSettings.ServerConnectionEnabled) && (serverSettings.ConnectToServerOnStartup))
            {
                CreateSocketConnection(serverSettings.ServerURL, serverSettings.ServerPublicKey);
            }




            //create folder to store scripts
           // var rpaScriptsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\taskt\\My Scripts\\";
            var rpaScriptsFolder = Core.Common.GetScriptFolderPath();

            if (!System.IO.Directory.Exists(rpaScriptsFolder))
            {
                UI.Forms.Supplemental.frmDialog userDialog = new UI.Forms.Supplemental.frmDialog("Would you like to create a folder to save your scripts in now? A script folder is required to save scripts generated with this application. The new script folder path would be '" + rpaScriptsFolder + "'.", "Unable to locate Script Folder!", UI.Forms.Supplemental.frmDialog.DialogType.YesNo, 0);

                if (userDialog.ShowDialog() == DialogResult.OK)
                {
                    System.IO.Directory.CreateDirectory(rpaScriptsFolder);
                }
            }

            //get latest files for recent files list on load
            GenerateRecentFiles();

            //get current version
            lblHeaderVersion.Text = "v" + new Version(System.Windows.Forms.Application.ProductVersion);

            //no height for status bar
            HideNotificationRow();

            //instantiate for script variables
            scriptVariables = new List<Core.Script.ScriptVariable>();

            //pnlHeader.BackColor = Color.FromArgb(255, 214, 88);

            //instantiate and populate display icons for commands
            uiImages = UI.Images.UIImageList();



            lstScriptActions.SmallImageList = uiImages;



            // tvCommands.ImageList = uiImages;
            // tvCommands.ImageList.Images.Add(new Bitmap(1,1));

            //get commands
            var groupedCommands = Core.Common.GetGroupedCommands();
            foreach (var cmd in groupedCommands)
            {

                var group = cmd.Key as Core.AutomationCommands.Attributes.ClassAttributes.Group;
                TreeNode newGroup = new TreeNode(group.groupName);
                // newGroup.ImageIndex = tvCommands.ImageList.Images.Count - 1;
                // newGroup.SelectedImageIndex = tvCommands.ImageList.Images.Count - 1;

                foreach (var subcmd in cmd)
                {

                    Core.AutomationCommands.ScriptCommand newCommand = (Core.AutomationCommands.ScriptCommand)Activator.CreateInstance(subcmd);
                    TreeNode subNode = new TreeNode(newCommand.SelectionName);
                    //subNode.ImageIndex = uiImages.Images.IndexOfKey(newCommand.GetType().Name);
                    // subNode.SelectedImageIndex = uiImages.Images.IndexOfKey(newCommand.GetType().Name);
                    newGroup.Nodes.Add(subNode);

                }


                tvCommands.Nodes.Add(newGroup);

            }

            //tvCommands.ImageList = uiImages;


        }
        private void GenerateRecentFiles()
        {
            flwRecentFiles.Controls.Clear();

            var scriptPath = Core.Common.GetScriptFolderPath();

            if (!System.IO.Directory.Exists(scriptPath))
            {
                lblRecentFiles.Text = "Script Folder does not exist";
                lblFilesMissing.Text = "Directory Not Found: " + scriptPath;
                lblFilesMissing.Show();
                flwRecentFiles.Hide();
                return;
            }
        


            var directory = new System.IO.DirectoryInfo(scriptPath);

            var recentFiles = directory.GetFiles()
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
                lblFilesMissing.Show();
                flwRecentFiles.Hide();
            }
            else
            {
                foreach (var fil in recentFiles)
                {
                    if (flwRecentFiles.Controls.Count == 9)
                        return;

                    LinkLabel newFileLink = new LinkLabel();
                    newFileLink.Text = fil;
                    newFileLink.AutoSize = true;
                    newFileLink.LinkColor = Color.SteelBlue;
                    newFileLink.Font = lnkGitIssue.Font;
                    newFileLink.Margin = new Padding(0, 0, 0, 0);
                    newFileLink.LinkClicked += NewFileLink_LinkClicked;
                    flwRecentFiles.Controls.Add(newFileLink);

                }
            }


        }
        private void frmScriptBuilder_Shown(object sender, EventArgs e)
        {

            if (editMode)
                return;

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
            //Obtain the index of the item at the mouse pointer.
            int dragIndex = dragToItem.Index;


            //foreach (ListViewItem command in lstScriptActions.SelectedItems)
            //{

            //    if (command.Tag is Core.AutomationCommands.EndLoopCommand)
            //    {
            //        for (int i = 0; i < dragIndex; i++)
            //        {
            //            if (lstScriptActions.Items[i].Tag is Core.AutomationCommands.BeginLoopCommand)
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
                FormatCommandListView();
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
                FormatCommandListView();
            }
            else if ((e.Control) && (e.KeyCode == Keys.C))
            {
                //if (lstScriptActions.SelectedItems.Count == 1)
                //{
                //    rowSelectedForCopy = lstScriptActions.SelectedItems[0];
                //}

                CopyRow();

            }
            else if ((e.Control) && (e.KeyCode == Keys.V))
            {

                //if (rowSelectedForCopy != null)
                //{
                //    Core.AutomationCommands.ScriptCommand duplicatedCommand = (Core.AutomationCommands.ScriptCommand)Core.Common.Clone(rowSelectedForCopy.Tag);
                //    lstScriptActions.Items.Insert(lstScriptActions.SelectedIndices[0], CreateScriptCommandListViewItem(duplicatedCommand));
                //    FormatCommandListView();
                //}

                PasteRow();

            }
            else if ((e.Control) && (e.KeyCode == Keys.Z))
            {

                UndoChange();

            }
            else if ((e.Control) && (e.KeyCode == Keys.R))
            {

                RedoChange();

            }

            else if ((e.Control) && (e.KeyCode == Keys.A))
            {

                foreach (ListViewItem item in lstScriptActions.Items)
                {
                    item.Selected = true;
                }

            }


        }

        private void CopyRow()
        {
            if (lstScriptActions.SelectedItems.Count == 1)
            {
                rowSelectedForCopy = lstScriptActions.SelectedItems[0];
            }
        }

        private void PasteRow()
        {


            if (rowSelectedForCopy != null)
            {
                Core.AutomationCommands.ScriptCommand duplicatedCommand = (Core.AutomationCommands.ScriptCommand)Core.Common.Clone(rowSelectedForCopy.Tag);
                lstScriptActions.Items.Insert(lstScriptActions.SelectedIndices[0], CreateScriptCommandListViewItem(duplicatedCommand));
                FormatCommandListView();
            }

            CreateUndoSnapshot();

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

                FormatCommandListView();

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


                FormatCommandListView();


            }

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

        private void lstScriptActions_DoubleClick(object sender, EventArgs e)
        {

            if (lstScriptActions.SelectedItems.Count != 1)
            {
                return;
            }


            //bring up edit mode to edit the action
            ListViewItem selectedCommandItem = lstScriptActions.SelectedItems[0];


            //set selected command from the listview item tag object which was assigned to the command
            var currentCommand = (Core.AutomationCommands.ScriptCommand)selectedCommandItem.Tag;


            //check if editing a sequence
            if (currentCommand is Core.AutomationCommands.SequenceCommand)
            {

                if (editMode)
                {
                    MessageBox.Show("Embedding Sequence Commands within Sequence Commands not yet supported.");
                    return;
                }


                //get sequence events
                Core.AutomationCommands.SequenceCommand sequence = (Core.AutomationCommands.SequenceCommand)currentCommand;
                frmScriptBuilder newBuilder = new frmScriptBuilder();
           
                //append to new builder
                foreach (var cmd in sequence.v_scriptActions)
                {
                    newBuilder.lstScriptActions.Items.Add(CreateScriptCommandListViewItem(cmd));            
                }


                //apply editor style format
                newBuilder.ApplyEditorFormat();

                //if data has been changed
                if (newBuilder.ShowDialog() == DialogResult.OK)
                {

                    //create updated list
                    List<Core.AutomationCommands.ScriptCommand> updatedList = new List<Core.AutomationCommands.ScriptCommand>();

                    //update to list
                    for (int i = 0; i < newBuilder.lstScriptActions.Items.Count; i++)
                    {
                        var command = (Core.AutomationCommands.ScriptCommand)newBuilder.lstScriptActions.Items[i].Tag;
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
                UI.Forms.frmCommandEditor editCommand = new UI.Forms.frmCommandEditor();

                //creation mode edit locks form to current command
                editCommand.creationMode = UI.Forms.frmCommandEditor.CreationMode.Edit;

                //create clone of current command so databinding does not affect if changes are not saved
                editCommand.selectedCommand = Core.Common.Clone(currentCommand); ;

                //set variables
                editCommand.scriptVariables = this.scriptVariables;

                //show edit command form and save changes on OK result
                if (editCommand.ShowDialog() == DialogResult.OK)
                {
                    selectedCommandItem.Tag = editCommand.selectedCommand;
                    selectedCommandItem.Text = editCommand.selectedCommand.GetDisplayValue(); //+ "(" + cmdDetails.SelectedVariables() + ")";
                    selectedCommandItem.SubItems.Add(editCommand.selectedCommand.GetDisplayValue());
                }
            }



            

        }





        #endregion

        #region Editor Mode
        private void ApplyEditorFormat()
        {
            editMode = true;
            this.Text = "edit sequence";
            lblMainLogo.Text = "edit sequence";
            lblHeaderVersion.Hide();
            FormatCommandListView();
            pnlCommandHelper.Hide();
            
           
            grpSaveClose.Location = grpFileActions.Location;

            grpRecordRun.Hide();
            grpFileActions.Hide();
            grpVariable.Hide();
            grpSaveClose.Show();

            grpSearch.Left = grpSaveClose.Right + 20;

            //tlpControls.RowStyles[1].Height = 0;
        }


        private void uiBtnKeep_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void uiPictureButton3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }



        #endregion

        #region ListView Create Item

        private ListViewItem CreateScriptCommandListViewItem(Core.AutomationCommands.ScriptCommand cmdDetails)
        {
            ListViewItem newCommand = new ListViewItem();
            newCommand.Text = cmdDetails.GetDisplayValue(); //+ "(" + cmdDetails.SelectedVariables() + ")";
            newCommand.SubItems.Add(cmdDetails.GetDisplayValue());
            newCommand.Tag = cmdDetails;
            newCommand.ForeColor = cmdDetails.DisplayForeColor;

            newCommand.ImageIndex = uiImages.Images.IndexOfKey(cmdDetails.GetType().Name);
            return newCommand;
        }

        public void AddCommandToListView(Core.AutomationCommands.ScriptCommand selectedCommand)
        {

            //add command
            lstScriptActions.Items.Add(CreateScriptCommandListViewItem(selectedCommand));

            //special types also get a following command and comment
            if((selectedCommand is Core.AutomationCommands.BeginExcelDatasetLoopCommand) || (selectedCommand is Core.AutomationCommands.BeginListLoopCommand) || (selectedCommand is Core.AutomationCommands.BeginNumberOfTimesLoopCommand))
            {
                lstScriptActions.Items.Add(CreateScriptCommandListViewItem(new Core.AutomationCommands.CommentCommand() { v_Comment = "Items in this section will run within the loop" }));
                lstScriptActions.Items.Add(CreateScriptCommandListViewItem(new Core.AutomationCommands.EndLoopCommand()));
            }
            else if (selectedCommand is Core.AutomationCommands.BeginIfCommand)
            {
                lstScriptActions.Items.Add(CreateScriptCommandListViewItem(new Core.AutomationCommands.CommentCommand() { v_Comment = "Items in this section will run if the statement is true" }));
                lstScriptActions.Items.Add(CreateScriptCommandListViewItem(new Core.AutomationCommands.EndIfCommand()));
            }

            CreateUndoSnapshot();

            FormatCommandListView();
        }

        #endregion

        #region ListView Comment, Coloring, ToolStrip
        private void FormatCommandListView()
        {



            if (pnlCommandHelper.Visible)
                pnlCommandHelper.Hide();


            int indent = 0;
            foreach (ListViewItem rowItem in lstScriptActions.Items)
            {

                if ((rowItem.Tag is Core.AutomationCommands.BeginIfCommand) || (rowItem.Tag is Core.AutomationCommands.BeginExcelDatasetLoopCommand) || (rowItem.Tag is Core.AutomationCommands.BeginListLoopCommand) || (rowItem.Tag is Core.AutomationCommands.BeginNumberOfTimesLoopCommand))
                {
                    indent += 2;
                    rowItem.IndentCount = indent;
                    indent += 2;
                }
                else if ((rowItem.Tag is Core.AutomationCommands.EndLoopCommand) || (rowItem.Tag is Core.AutomationCommands.EndIfCommand))
                {
                    indent -= 2;
                    if (indent < 0) indent = 0;
                    rowItem.IndentCount = indent;
                    indent -= 2;
                    if (indent < 0) indent = 0;
                }
                else if (rowItem.Tag is Core.AutomationCommands.ElseCommand)
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




                //mod 2 to color alt rows
                if (rowItem.Index % 2 == 0)
                {
                    rowItem.BackColor = Color.White;
                }
                else
                {
                    rowItem.BackColor = Color.AliceBlue;
                }

                //if code is commented change back color
                var selectedCommand = (Core.AutomationCommands.ScriptCommand)rowItem.Tag;

                if ((selectedCommand.IsCommented) || (selectedCommand is Core.AutomationCommands.CommentCommand))
                {
                    rowItem.ForeColor = Color.ForestGreen;
                }
                else
                {
                    rowItem.ForeColor = selectedCommand.DisplayForeColor;
                }

                //handle pause before execution items
                if (selectedCommand.PauseBeforeExeucution)
                {
                    rowItem.BackColor = Color.LightYellow;
                    rowItem.ForeColor = Color.SteelBlue;
                }

                if (DebugLine > 0)
                {
                    lstScriptActions.Items[DebugLine - 1].BackColor = Color.OrangeRed;
                    lstScriptActions.Items[DebugLine - 1].ForeColor = Color.White;
                }





            }
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
                FormatCommandListView();
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
                var selectedCommand = (Core.AutomationCommands.ScriptCommand)item.Tag;
                selectedCommand.IsCommented = setCommented;
            }

            //recolor
            FormatCommandListView();

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
                var selectedCommand = (Core.AutomationCommands.ScriptCommand)item.Tag;
                selectedCommand.PauseBeforeExeucution = !selectedCommand.PauseBeforeExeucution;
            }

            //recolor
            FormatCommandListView();

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
        private void copySelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyRow();
        }
        private void pasteSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PasteRow();
        }
        #endregion

        #endregion

        #region Bottom Notification Panel
        List<string> notificationList = new List<string>();
        private DateTime notificationExpires;
        private bool isDisplaying;
        private void tmrNotify_Tick(object sender, EventArgs e)
        {

            if ((notificationExpires < DateTime.Now) && (isDisplaying))
            {
                HideNotification();
            }



            //check if notification is required
            if ((notificationList.Count > 0) && (notificationExpires < DateTime.Now))
            {
                var itemToDisplay = notificationList[0];
                notificationList.RemoveAt(0);
                notificationExpires = DateTime.Now.AddSeconds(3);
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
            tlpControls.RowStyles[4].Height = 0;
        }
        private void ShowNotificationRow()
        {
            tlpControls.RowStyles[4].Height = 30;
        }
        public string notificationText { get; set; }
        private void pnlStatus_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawString(notificationText, pnlStatus.Font, Brushes.White, 30, 4);
            e.Graphics.DrawImage(Properties.Resources.message, 5, 3, 24, 24);

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnManageVariables_Click(object sender, EventArgs e)
        {

            UI.Forms.frmScriptVariables scriptVariableEditor = new UI.Forms.frmScriptVariables();
            scriptVariableEditor.scriptVariables = this.scriptVariables;

            if (scriptVariableEditor.ShowDialog() == DialogResult.OK)
            {
                this.scriptVariables = scriptVariableEditor.scriptVariables;
            }

        }

        private void frmScriptBuilder_SizeChanged(object sender, EventArgs e)
        {
            lstScriptActions.Columns[0].Width = -2;
        }
        #endregion

        #region Open, Save, Parse File
        private void uiBtnOpen_Click(object sender, EventArgs e)
        {

            //var scriptFolder = Core.Common.GetScriptFolderPath();
            //var files = System.IO.Directory.GetFiles(scriptFolder);

            //if (files.Count() == 0)
            //{
            //    //MessageBox.Show("There were no scripts found in the scripts folder at " + scriptFolder, "No Scripts Found");
            //    Notify("There were no scripts found in the scripts folder at " + scriptFolder);
            //    return;
            //}

            ////create script selector
            //UI.Forms.Supplemental.frmScriptSelector frmSelector = new UI.Forms.Supplemental.frmScriptSelector();
            //frmSelector.fileList = files;

            ////if script was selected, then run the script
            //if (frmSelector.ShowDialog() == DialogResult.OK)
            //{
            //   var selectedScript = System.IO.Path.Combine(scriptFolder, frmSelector.cboSelectFile.Text);
            //    OpenFile(selectedScript);
            //}

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Core.Common.GetScriptFolderPath();
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Filter = "Xml (*.xml)|*.xml";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                OpenFile(openFileDialog.FileName);
            }
        }
        private void OpenFile(string filePath)
        {

            try
            {

                lstScriptActions.Items.Clear();
   
                scriptVariables = new List<Core.Script.ScriptVariable>();

                this.ScriptFilePath = filePath;

                Core.Script.Script deserializedScript = Core.Script.Script.DeserializeFile(ScriptFilePath);

                if (deserializedScript.Commands.Count == 0)
                {
                    Notify("Error Parsing File: Commands not found!");
                }

                scriptVariables = deserializedScript.Variables;

                PopulateExecutionCommands(deserializedScript.Commands);

                FormatCommandListView();

                Notify("Script Loaded Successfully!");
            }
            catch (Exception ex)
            {
                Notify("Oops, an error occured: " + ex.Message);
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            for (int i = 30; i > 0; i--)
            {
                tlpControls.RowStyles[4].Height = i;
            }
        }

        public void PopulateExecutionCommands(List<Core.Script.ScriptAction> commandDetails)
        {

            foreach (Core.Script.ScriptAction item in commandDetails)
            {
                lstScriptActions.Items.Add(CreateScriptCommandListViewItem(item.ScriptCommand));
                if (item.AdditionalScriptCommands.Count > 0) PopulateExecutionCommands(item.AdditionalScriptCommands);
            }


        }

        private void uiBtnSave_Click(object sender, EventArgs e)
        {
            SaveToFile(false);
        }
        private void uiBtnSaveAs_Click(object sender, EventArgs e)
        {
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
            foreach (ListViewItem item in lstScriptActions.Items)
            {
                if ((item.Tag is Core.AutomationCommands.BeginExcelDatasetLoopCommand) || (item.Tag is Core.AutomationCommands.BeginListLoopCommand) || (item.Tag is Core.AutomationCommands.BeginNumberOfTimesLoopCommand))
                {
                    beginLoopValidationCount++;
                }
                else if (item.Tag is Core.AutomationCommands.EndLoopCommand)
                {
                    beginLoopValidationCount--;
                }
                else if (item.Tag is Core.AutomationCommands.BeginIfCommand)
                {
                    beginIfValidationCount++;
                }
                else if (item.Tag is Core.AutomationCommands.EndIfCommand)
                {
                    beginIfValidationCount--;
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


            //define default output path
            if ((this.ScriptFilePath == null) || (saveAs))
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = Core.Common.GetScriptFolderPath();
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.Filter = "Xml (*.xml)|*.xml";

                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                this.ScriptFilePath = saveFileDialog.FileName;


                // var fileName = Microsoft.VisualBasic.Interaction.InputBox("Please enter a file name (without extension)", "Enter File Name", "Default", -1, -1);



                //var rpaScriptsFolder = Core.Common.GetScriptFolderPath();

                //if (!System.IO.Directory.Exists(rpaScriptsFolder))
                //{
                //    UI.Forms.Supplemental.frmDialog userDialog = new UI.Forms.Supplemental.frmDialog("Would you like to create a folder to save your scripts in now? A script folder is required to save scripts generated with this application. The new script folder path would be '" + rpaScriptsFolder + "'.", "Unable to locate Script Folder!", UI.Forms.Supplemental.frmDialog.DialogType.YesNo, 0);

                //    if (userDialog.ShowDialog() == DialogResult.OK)
                //    {
                //        System.IO.Directory.CreateDirectory(rpaScriptsFolder);
                //    }
                //    else
                //    {
                //        return;
                //    }


                //}


                //this.ScriptFilePath = rpaScriptsFolder + fileName + ".xml";
            }

            //serialize script
            try
            {
                var exportedScript = Core.Script.Script.SerializeScript(lstScriptActions.Items, scriptVariables, this.ScriptFilePath);
                //show success dialog
                Notify("File has been saved successfully!");
            }
            catch (Exception ex)
            {
                Notify("Error: " + ex.ToString());
            }


        }

        #endregion

        #region UI Buttons

        private void uiBtnAddVariable_Click(object sender, EventArgs e)
        {
            UI.Forms.frmScriptVariables scriptVariableEditor = new UI.Forms.frmScriptVariables();
            scriptVariableEditor.scriptVariables = this.scriptVariables;

            if (scriptVariableEditor.ShowDialog() == DialogResult.OK)
            {
                this.scriptVariables = scriptVariableEditor.scriptVariables;
            }
        }

        private void uiBtnRunScript_Click(object sender, EventArgs e)
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

            Notify("Running Script..");
            UI.Forms.frmScriptEngine newEngine = new UI.Forms.frmScriptEngine(ScriptFilePath, this);
            newEngine.callBackForm = this;
            newEngine.Show();

        }

        private void uiBtnNew_Click(object sender, EventArgs e)
        {
            this.ScriptFilePath = null;
            lstScriptActions.Items.Clear();
            HideSearchInfo();
            scriptVariables = new List<Core.Script.ScriptVariable>();
            GenerateRecentFiles();
            pnlCommandHelper.Show();
        }

        private void uiBtnScheduleManagement_Click(object sender, EventArgs e)
        {
            UI.Forms.frmScheduleManagement scheduleManager = new UI.Forms.frmScheduleManagement();
            scheduleManager.Show();
        }

       
        private void uiBtnAbout_Click(object sender, EventArgs e)
        {
            UI.Forms.Supplemental.frmAbout frmAboutForm = new UI.Forms.Supplemental.frmAbout();
            frmAboutForm.Show();
        }

        private void uiBtnSettings_Click(object sender, EventArgs e)
        {
            //show settings dialog
            frmSettings newSettings = new frmSettings(this);
            newSettings.ShowDialog();

        }
        #endregion

        #region Create Command Logic
        private void AddNewCommand(string specificCommand = "")
        {
            //bring up new command configuration form
            var newCommandForm = new UI.Forms.frmCommandEditor();
            newCommandForm.creationMode = UI.Forms.frmCommandEditor.CreationMode.Add;
            newCommandForm.scriptVariables = this.scriptVariables;
            if (specificCommand != "")
                newCommandForm.defaultStartupCommand = specificCommand;

            //if a command was selected
            if (newCommandForm.ShowDialog() == DialogResult.OK)
            {
                //add to listview
                AddCommandToListView(newCommandForm.selectedCommand);
            }

        }
        #endregion

        #region WebServer Socket Management

        public bool CreateSocketConnection(string serverURL, string publicKey)
        {
            try
            {
                this.publicKey = publicKey;
                webSocket = new WebSocket4Net.WebSocket(serverURL);
                webSocket.Error += new EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>(ConnectionError);
                webSocket.Opened += new EventHandler(ConnectionOpened);
                webSocket.Closed += new EventHandler(ConnectionClosed);
                webSocket.MessageReceived += new EventHandler<WebSocket4Net.MessageReceivedEventArgs>(websocket_MessageReceived);
                webSocket.Open();
                Notify("Connected To Server");
                return true;
            }
            catch (Exception ex)
            {
                Notify("Server Connection Problem: " + ex.Message);
                return false;
            }
        }
        public static string Encrypt(string publicKey, string data)
        {

            if (publicKey is null)
            {
                return string.Empty;
            }

            System.Security.Cryptography.CspParameters cspParams = new System.Security.Cryptography.CspParameters { ProviderType = 1 };
            System.Security.Cryptography.RSACryptoServiceProvider rsaProvider = new System.Security.Cryptography.RSACryptoServiceProvider(cspParams);

            rsaProvider.ImportCspBlob(Convert.FromBase64String(publicKey));

            byte[] plainBytes = System.Text.Encoding.UTF8.GetBytes(data);
            byte[] encryptedBytes = rsaProvider.Encrypt(plainBytes, false);

            return Encoding.Unicode.GetString(encryptedBytes);

        }


        public void SendMessage(string message)
        {
            //encrypt as needed
            if (webSocketConnectionID != null)
            {
                var encryptedString = Encrypt(publicKey, message);
                webSocket.Send(encryptedString);
            }

        }

        private void ConnectionOpened(object sender, EventArgs e)
        {
            SendMessage("ClientStatus=Available");
        }
        private void ConnectionClosed(object sender, EventArgs e)
        {
            //start retrying to connect
        }

        private void ConnectionError(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            //connection has failed, start retrying
        }
        private void websocket_MessageReceived(object sender, WebSocket4Net.MessageReceivedEventArgs e)
        {


            if (e.Message.Contains("connectionID"))
            {
                //set connection ID to display locally
                var connectionID = e.Message.Replace("connectionID=", "");
                webSocketConnectionID = connectionID;
                UpdateServerConnectionInfo(connectionID);
            }
            else if (e.Message.Contains("?xml"))
            {
                //code is being sent from the server so run it
                streamedXMLData = e.Message;
                RunXMLScript();
            }
            else if (e.Message.Contains("ping"))
            {
                SendMessage("Alive");
            }
        }

        private void UpdateServerConnectionInfo(string connectionID)
        {
            if (InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    UpdateServerConnectionInfo(connectionID);
                });

            }

            else
            {
                UpdateWindowTitle();
            }

        }
        private void RunXMLScript()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(RunXMLScript));
            }
            else
            {
                frmScriptEngine newEngine = new UI.Forms.frmScriptEngine("", this);
                newEngine.xmlInfo = streamedXMLData;
                newEngine.callBackForm = this;
                newEngine.Show();
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





            AddNewCommand(tvCommands.SelectedNode.Parent.Text + " - " + tvCommands.SelectedNode.Text);

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
            System.Diagnostics.Process.Start("https://github.com/saucepleez/taskt/wiki");
        }
        private void NewFileLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel senderLink = (LinkLabel)sender;
            OpenFile(Core.Common.GetScriptFolderPath() + senderLink.Text);
        }

        #endregion

        private void pnlCommandHelper_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void uiBtnRecordSequence_Click(object sender, EventArgs e)
        {

            this.Hide();
            frmSequenceRecorder sequenceRecorder = new frmSequenceRecorder();
            sequenceRecorder.callBackForm = this;
            sequenceRecorder.ShowDialog();

            pnlCommandHelper.Hide();

            this.Show();
            this.BringToFront();


        }
        private void uiBtnAbout_Click_1(object sender, EventArgs e)
        {
         
        }

        private void uiBtnClearAll_Click(object sender, EventArgs e)
        {
            HideSearchInfo();
            lstScriptActions.Items.Clear();
        }


        int reqdIndex;
        private void txtCommandSearch_TextChanged(object sender, EventArgs e)
        {

            if (lstScriptActions.Items.Count == 0)
                return;
        

            if (txtCommandSearch.Text == "")
            {
                HideSearchInfo();
                FormatCommandListView();
            }
            else
            {
                lblCurrentlyViewing.Show();
                lblTotalResults.Show();
                SearchForItemInListView();
            }
         
        }

        private void HideSearchInfo()
        {
            lblCurrentlyViewing.Hide();
            lblTotalResults.Hide();
        }
        private void btnSearchNext_Click(object sender, EventArgs e)
        {
           
        }

        private void pbSearch_Click(object sender, EventArgs e)
        {
            if (txtCommandSearch.Text != "")
            {
                reqdIndex++;
                SearchForItemInListView();
            }

        }


        private void SearchForItemInListView()
        {

            FormatCommandListView();


            var matchingItems = (from ListViewItem itm in lstScriptActions.Items
                                 where itm.Text.Contains(txtCommandSearch.Text)
                                 select itm).ToList();


            int? matchCount = matchingItems.Count();
            int totalMatches = matchCount ?? 0;

         
            if (reqdIndex == matchingItems.Count)
            {
                reqdIndex = 0;
            }

            lblTotalResults.Show();

            if (totalMatches == 0)
            {
                reqdIndex = 0;
                lblTotalResults.Text = "No Matches Found";
                lblCurrentlyViewing.Hide();
                return;
            }
            else
            {
                lblCurrentlyViewing.Text = "Viewing " + (reqdIndex + 1) + " of " + totalMatches + "";
                lblTotalResults.Text = totalMatches + " total results found";
            }

          

          


            foreach (ListViewItem itm in matchingItems)
            {
                itm.BackColor = Color.LightGoldenrodYellow;
            }


            var reqdItem = matchingItems[reqdIndex];
            reqdItem.BackColor = Color.Orange;


         

            lstScriptActions.EnsureVisible(reqdItem.Index);




         
        }

        private void pbSearch_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void pbSearch_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        private void lblMainLogo_Click(object sender, EventArgs e)
        {
            Supplemental.frmAbout aboutForm = new Supplemental.frmAbout();
            aboutForm.Show();
        }
    }


    public class SocketMessage
        {
        public string Message { get; set; }
        }

    public class SocketCommandMessage : SocketMessage
    {
        public Core.AutomationCommands.ScriptCommand cmd { get; set; }

    }


    }

