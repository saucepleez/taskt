using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;
using taskt.Core.Script;

namespace taskt.UI.Forms.ScriptBuilder
{
    public partial class frmScriptBuilder
    {
        /*
         * this file has frmScriptBuilder form event handlers
         */

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
            // load all commands
            automationCommands = CustomControls.CommandControls.GenerateCommandsAndControls();

            // title
            var info = System.Diagnostics.FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            this.Text = info.ProductName;
            lblMainLogo.Text = info.ProductName;

            // init Pen
            indentDashLine = new Pen(Color.LightGray, 1);
            indentDashLine.DashStyle = DashStyle.Dash;

            // set controls double buffered
            foreach (Control control in Controls)
            {
                typeof(Control).InvokeMember("DoubleBuffered",
                    BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                    null, control, new object[] { true });
            }

            // create undo list
            //undoList = new List<List<ListViewItem>>();

            // get app settings
            //appSettings = new Core.ApplicationSettings();
            //appSettings = appSettings.GetOrCreateApplicationSettings();
            //appSettings = Core.ApplicationSettings.GetOrCreateApplicationSettings();
            //appSettings = App.Taskt_UNSAFE_Settings;
            appSettings = App.GetFrmScriptBuilderApplicationSettings();

            // notification mananger
            this.notificationManager = new NotificationManager4frmScriptBuilder(tlpControls.RowStyles[5], pnlStatus, 2);
            // show/hide status bar
            if (appSettings.ClientSettings.HideNotifyAutomatically)
            {
                notificationManager.HideNotification();
            }
            else
            {
                notificationManager.ShowNotification();
            }

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

            // handle action bar preference
            // hide action panel
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

            // get scripts folder
            //var rpaScriptsFolder = Core.IO.Folders.GetFolder(Core.IO.Folders.FolderType.ScriptsFolder);
            var rpaScriptsFolder = Core.IO.Folders.GetScriptsFolderPath();

            if (!System.IO.Directory.Exists(rpaScriptsFolder))
            {
                using (var userDialog = new General.frmDialog("Would you like to create a folder to save your scripts in now? A script folder is required to save scripts generated with this application. The new script folder path would be '" + rpaScriptsFolder + "'.", "Unable to locate Script Folder!", General.frmDialog.DialogType.YesNo, 0))
                {
                    if (userDialog.ShowDialog() == DialogResult.OK)
                    {
                        System.IO.Directory.CreateDirectory(rpaScriptsFolder);
                    }
                }
            }

            // get latest files for recent files list on load
            GenerateRecentFiles();

            // instantiate for script variables
            if (!editMode)
            {
                scriptVariables = new List<ScriptVariable>();
                scriptInfo = new ScriptInformation();
            }

            // instantiate and populate display icons for commands
            scriptImages = Images.UIImageList();

            // set image list
            lstScriptActions.SmallImageList = scriptImages;
            lstScriptActions.Columns[0].Width = 14; // 1digit width
            lstScriptActions.Columns[1].Width = 16; // icon size
            lstScriptActions.Columns[2].Width = lstScriptActions.ClientSize.Width - 30;

            // set listview column size
            frmScriptBuilder_SizeChanged(null, null);

            GenerateTreeViewCommands();

            // start attended mode if selected
            if (appSettings.ClientSettings.StartupMode == "Attended Task Mode")
            {
                this.WindowState = FormWindowState.Minimized;
                ShowAttendedModeFormProcess();
            }

            this.dontSaveFlag = false;

            // set searchform
            frmSearch = new Supplemental.frmSearchCommands(this);

            // instance count
            instanceList = new Core.InstanceCounter();

            // miniMap
            miniMapImg = new Bitmap(8, lstScriptActions.Height);

            // command search box
            SetCommandSearchBoxState();

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
            RemoveOldBeforeConvertedScriptFiles();

            // check update
            if ((appSettings.ClientSettings.CheckForUpdateAtStartup) && (this.parentBuilder == null))
            {
                Core.Update.ApplicationUpdate.ShowUpdateResultAsync(appSettings.ClientSettings.SkipBetaVersionUpdate);
            }

            lineCharWidth = DecideScriptActionsLineCharacterWidth();
        }

        private void frmScriptBuilder_Shown(object sender, EventArgs e)
        {
            Program.HideSplashScreen();

            if (editMode)
            {
                return;
            }

            Notify("Welcome! Press 'Start Edit Script' to get started!");
        }

        private void frmScriptBuilder_Resize(object sender, EventArgs e)
        {
            // check when minimized
            if ((this.WindowState == FormWindowState.Minimized) && (appSettings.ClientSettings.MinimizeToTray))
            {
                //appSettings = new Core.ApplicationSettings().GetOrCreateApplicationSettings();
                //appSettings = Core.ApplicationSettings.GetOrCreateApplicationSettings();
                if (appSettings == null)
                {
                    //appSettings = App.Taskt_UNSAFE_Settings;
                    appSettings = App.GetFrmScriptBuilderApplicationSettings();
                }

                if (appSettings.ClientSettings.MinimizeToTray)
                {
                    notifyTray.Visible = true;
                    notifyTray.ShowBalloonTip(3000);
                    this.ShowInTaskbar = false;
                }
            }

            pnlMain.Invalidate();
        }

        private void frmScriptBuilder_SizeChanged(object sender, EventArgs e)
        {
            lstScriptActions.Columns[2].Width = this.Width - 340;
        }

        private void frmScriptBuilder_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !BeginFormCloseProcess();
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
    }
}
