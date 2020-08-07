using Serilog.Core;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using taskt.Core.Enums;
using taskt.Core.IO;
using taskt.Core.Settings;
using taskt.Core.Utilities.CommonUtilities;

namespace taskt.UI.Forms
{
    public partial class frmAttendedMode : Form
    {
        #region Variables
        private ApplicationSettings _appSettings;
        private int _flashCount = 0;
        private bool _dragging = false;
        private Point _dragCursorPoint;
        private Point _dragFormPoint;
        #endregion

        #region Form Events
        public frmAttendedMode()
        {
            InitializeComponent();
        }

        private void frmAttendedMode_Load(object sender, EventArgs e)
        {
            //get app settings
            _appSettings = new ApplicationSettings().GetOrCreateApplicationSettings();

            //setup file system watcher
            attendedScriptWatcher.Path = _appSettings.ClientSettings.AttendedTasksFolder;

            //move form to default location
            MoveToDefaultFormLocation();

            //load scripts to be used for attended automation
            LoadAttendedScripts();
        }

        private void frmAttendedMode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //move to default location
            MoveToDefaultFormLocation();
        }

        private void uiBtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MoveToDefaultFormLocation()
        {
            //move to top middle of screen
            Screen myScreen = Screen.FromControl(this);
            Rectangle area = myScreen.WorkingArea;

            this.Top = 0;
            this.Left = (area.Width - this.Width) / 2;
        }

        private void uiBtnRun_Click(object sender, EventArgs e)
        {
            //build script path and execute
            var scriptFilePath = Path.Combine(_appSettings.ClientSettings.AttendedTasksFolder, cboSelectedScript.Text);

            //initialize Logger
            Logger engineLogger = null;
            switch (_appSettings.EngineSettings.LoggingSinkType)
            {
                case SinkType.File:
                    if (string.IsNullOrEmpty(_appSettings.EngineSettings.LoggingValue1.Trim()))
                        _appSettings.EngineSettings.LoggingValue1 = Path.Combine(Folders.GetFolder(FolderType.LogFolder), "taskt Engine Logs.txt");

                    engineLogger = new Logging().CreateFileLogger(_appSettings.EngineSettings.LoggingValue1, Serilog.RollingInterval.Day,
                        _appSettings.EngineSettings.MinLogLevel);
                    break;
                case SinkType.HTTP:
                    engineLogger = new Logging().CreateHTTPLogger(_appSettings.EngineSettings.LoggingValue1, _appSettings.EngineSettings.MinLogLevel);
                    break;
                case SinkType.SignalR:
                    string[] groupNames = _appSettings.EngineSettings.LoggingValue3.Split(',').Select(x => x.Trim()).ToArray();
                    string[] userIDs = _appSettings.EngineSettings.LoggingValue4.Split(',').Select(x => x.Trim()).ToArray();

                    engineLogger = new Logging().CreateSignalRLogger(_appSettings.EngineSettings.LoggingValue1, _appSettings.EngineSettings.LoggingValue2, 
                        groupNames, userIDs, _appSettings.EngineSettings.MinLogLevel);
                    break;
            }
            
            frmScriptEngine newEngine = new frmScriptEngine(scriptFilePath, null, engineLogger);
            newEngine.Show();
        }

        private void attendedScriptWatcher_Created(object sender, FileSystemEventArgs e)
        {
            LoadAttendedScripts();
        }

        private void LoadAttendedScripts()
        {
            //clear script list
            cboSelectedScript.Items.Clear();
        
            //get script files
            var files = Directory.GetFiles(_appSettings.ClientSettings.AttendedTasksFolder);

            //loop each file and add to potential
            foreach (var fil in files)
            {
                var filInfo = new FileInfo(fil);
                cboSelectedScript.Items.Add(filInfo.Name);
            }
        }
        #endregion

        #region Flashing Animation
        private void frmAttendedMode_Shown(object sender, EventArgs e)
        {
            tmrBackColorFlash.Enabled = true;
        }

        private void tmrBackColorFlash_Tick(object sender, EventArgs e)
        {
            if (this.BackColor == Color.FromArgb(59, 59, 59))
            {
                this.BackColor = Color.LightYellow;
                uiBtnClose.DisplayTextBrush = Color.Black;
                uiBtnRun.DisplayTextBrush = Color.Black;
            }
            else
            {
                this.BackColor = Color.FromArgb(59, 59, 59);
                uiBtnClose.DisplayTextBrush = Color.White;
                uiBtnRun.DisplayTextBrush = Color.White;
            }

            _flashCount++;

            if (_flashCount == 6)
            {
                tmrBackColorFlash.Enabled = false;
            }
        }
        #endregion

        #region Form Dragging
        private void frmAttendedMode_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(_dragCursorPoint));
                this.Location = Point.Add(_dragFormPoint, new Size(dif));
            }
        }

        private void frmAttendedMode_MouseDown(object sender, MouseEventArgs e)
        {
            _dragging = true;
            _dragCursorPoint = Cursor.Position;
            _dragFormPoint = this.Location;
        }

        private void frmAttendedMode_MouseUp(object sender, MouseEventArgs e)
        {
            _dragging = false;
        }
        #endregion
    }
}
