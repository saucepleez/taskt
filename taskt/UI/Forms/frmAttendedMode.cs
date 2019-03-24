using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace taskt.UI.Forms
{
    public partial class frmAttendedMode : Form
    {
        Core.ApplicationSettings appSettings { get; set; }
        public frmAttendedMode()
        {
            InitializeComponent();
        }

        private void frmAttendedMode_Load(object sender, EventArgs e)
        {
            //get app settings
            appSettings = new Core.ApplicationSettings().GetOrCreateApplicationSettings();

            //setup file system watcher
            attendedScriptWatcher.Path = appSettings.ClientSettings.AttendedTasksFolder;

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

        private void LoadAttendedScripts()
        {
            //clear script list
            cboSelectedScript.Items.Clear();
        
            //get script files
            var files = System.IO.Directory.GetFiles(appSettings.ClientSettings.AttendedTasksFolder);

            //loop each file and add to potential
            foreach (var fil in files)
            {
                var filInfo = new System.IO.FileInfo(fil);
                cboSelectedScript.Items.Add(filInfo.Name);
            }


        }

        #region Flashing Animation
        private void frmAttendedMode_Shown(object sender, EventArgs e)
        {
            tmrBackColorFlash.Enabled = true;
        }

        private int flashCount = 0;
        private void tmrBackColorFlash_Tick(object sender, EventArgs e)
        {

            ;
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

            flashCount++;

            if (flashCount == 6)
            {
                tmrBackColorFlash.Enabled = false;
            }

        }

        #endregion

        #region Form Dragging
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        private void frmAttendedMode_MouseMove(object sender, MouseEventArgs e)
        {

            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void frmAttendedMode_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void frmAttendedMode_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        #endregion

        private void uiBtnRun_Click(object sender, EventArgs e)
        {
            //build script path and execute
            var scriptFilePath = System.IO.Path.Combine(appSettings.ClientSettings.AttendedTasksFolder, cboSelectedScript.Text);
            UI.Forms.frmScriptEngine newEngine = new UI.Forms.frmScriptEngine(scriptFilePath, null);
            newEngine.Show();
        }

        private void attendedScriptWatcher_Created(object sender, System.IO.FileSystemEventArgs e)
        {
            LoadAttendedScripts();
        }
    }
}
