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
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using taskt.Core;
using taskt.Core.Automation.Commands;
using taskt.Core.IO;
using taskt.Core.Script;
using taskt.Core.Server;
using taskt.Core.Settings;
using taskt.UI.CustomControls;
using taskt.UI.CustomControls.CustomUIControls;
using taskt.UI.Forms.Supplement_Forms;
using Point = System.Drawing.Point;

namespace taskt.UI.Forms.ScriptBuilder_Forms
{
    public partial class frmScriptBuilder : Form
    //Form tracks the overall configuration and enables script editing, saving, and running
    //Features ability to add, drag/drop reorder commands
    {
        #region Instance Variables
        private List<ListViewItem> _rowsSelectedForCopy;
        private List<ScriptVariable> _scriptVariables;
        private List<ScriptElement> _scriptElements;
        private List<AutomationCommand> _automationCommands;
        private bool _editMode;
        private ImageList _uiImages;
        private ApplicationSettings _appSettings;
        private DateTime _lastAntiIdleEvent;
        private int _reqdIndex;
        private int _selectedIndex = -1;
        private List<int> _matchingSearchIndex = new List<int>();
        private int _currentIndex = -1;
        private frmScriptBuilder _parentBuilder;
        private UIListView _selectedTabScriptActions;
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
        private string _mainFileName;
        private Point _lastClickPosition;
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
                        IsScriptRunning = true;
                        _selectedTabScriptActions.EnsureVisible(_debugLine - 1);
                    }
                    catch (Exception)
                    {
                        //log exception?
                    }
                }
                else if (_debugLine == 0)
                {
                    IsScriptRunning = false;
                    IsScriptSteppedOver = false;
                    IsScriptSteppedInto = false;
                    RemoveDebugTab();
                }

                _selectedTabScriptActions.Invalidate();
                //FormatCommandListView();

                if (IsScriptSteppedInto || IsScriptSteppedOver)
                    LoadDebugTab(uiPaneTabs.TabPages["DebugVariables"]);

            }
        }
        private List<string> _notificationList = new List<string>();
        private DateTime _notificationExpires;
        private bool _isDisplaying;
        private string _notificationText;
        public frmScriptEngine CurrentEngine { get; set; }
        public bool IsScriptRunning { get; set; }
        public bool IsScriptPaused { get; set; }
        public bool IsScriptSteppedOver { get; set; }
        public bool IsScriptSteppedInto { get; set; }
        public bool IsUnhandledException { get; set; }
        private bool _isDebugMode;
        #endregion

        #region Form Events
        public frmScriptBuilder()
        {
            _selectedTabScriptActions = NewLstScriptActions();
            InitializeComponent();           
        }

        private void UpdateWindowTitle()
        {
            if (!string.IsNullOrEmpty(ScriptFilePath))
            {
                FileInfo scriptFileInfo = new FileInfo(ScriptFilePath);
                Text = "taskt - (Project: " + _scriptProject.ProjectName + " - Script: " + scriptFileInfo.Name + ")";
            }
            else if (_scriptProject.ProjectName != null)
            {
                Text = "taskt - (Project: " + _scriptProject.ProjectName + ")";
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
                _scriptElements = new List<ScriptElement>();
            }
            //pnlHeader.BackColor = Color.FromArgb(255, 214, 88);

            //instantiate and populate display icons for commands
            _uiImages = UIImage.UIImageList();

            //set image list
            _selectedTabScriptActions.SmallImageList = _uiImages;

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

        private void frmScriptBuilder_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = CheckForUnsavedScripts();
            if (result == DialogResult.Cancel)
                e.Cancel = true;
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

        private void frmScriptBuilder_SizeChanged(object sender, EventArgs e)
        {
            _selectedTabScriptActions.Columns[2].Width = Width - 340;
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
            newCommandForm.ScriptElements = _scriptElements;
            if (specificCommand != "")
                newCommandForm.DefaultStartupCommand = specificCommand;

            //if a command was selected
            if (newCommandForm.ShowDialog() == DialogResult.OK)
            {
                //add to listview
                CreateUndoSnapshot();
                AddCommandToListView(newCommandForm.SelectedCommand);              
            }

            if (newCommandForm.SelectedCommand.CommandName == "SeleniumElementActionCommand")
            {
                CreateUndoSnapshot();
                _scriptElements = newCommandForm.ScriptElements;
            }
        }

        private List<ScriptCommand> GetConfiguredCommands()
        {
            List<ScriptCommand> ConfiguredCommands = new List<ScriptCommand>();
            foreach (ListViewItem item in _selectedTabScriptActions.Items)
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
            Process.Start("https://github.com/saucepleez/taskt");
        }
        private void lnkGitLatestReleases_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/saucepleez/taskt/releases");
        }
        private void lnkGitIssue_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/saucepleez/taskt/issues/new");
        }
        private void lnkGitWiki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://wiki.taskt.net/");
        }
        private void NewFileLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel senderLink = (LinkLabel)sender;
            OpenFile(Folders.GetFolder(Folders.FolderType.ScriptsFolder) + senderLink.Text);
        }
        #endregion
    }
}

