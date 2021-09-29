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
        public Core.ApplicationSettings appSettings { get; set; }

        #region form events
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
        #endregion

        #region DoubleClick event
        private void frmAttendedMode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //move to default location
            MoveToDefaultFormLocation();
        }

        private void MoveToDefaultFormLocation()
        {
            //move to top middle of screen
            Screen myScreen = Screen.FromControl(this);
            Rectangle area = myScreen.WorkingArea;

            this.Top = 0;
            this.Left = (area.Width - this.Width) / 2;
        }
        #endregion

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
                uiBtnMenu.DisplayTextBrush = Color.Black;
                uiBtnRun.DisplayTextBrush = Color.Black;
            }
            else
            {
                this.BackColor = Color.FromArgb(59, 59, 59);
                uiBtnMenu.DisplayTextBrush = Color.White;
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

        #region buttons
        private void uiBtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void uiBtnRun_Click(object sender, EventArgs e)
        {
            if (cboSelectedScript.Text == "")
            {
                MessageBox.Show("Please select a script file.", "taskt", MessageBoxButtons.OK);
                return;
            }

            //build script path and execute
            var scriptFilePath = System.IO.Path.Combine(attendedScriptWatcher.Path, cboSelectedScript.Text);
            UI.Forms.frmScriptEngine newEngine = new UI.Forms.frmScriptEngine(scriptFilePath, null);
            newEngine.Show();
        }

        private void attendedScriptWatcher_Created(object sender, System.IO.FileSystemEventArgs e)
        {
            LoadAttendedScripts();
        }
        
        #endregion

        #region menu events
        private void uiBtnMenu_Click(object sender, EventArgs e)
        {
            attededMenuStrip.Show(Cursor.Position);
        }
        private void closeAttendedMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void selectFolderAttendedMenuItem_Click(object sender, EventArgs e)
        {
            ChangeFolderProcess();
        }

        private void selectFileAttendedMenuItem_Click(object sender, EventArgs e)
        {
            SelectFileProcess();
        }
        #endregion

        #region form shortcut key
        private void frmAttendedMode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.F)
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    ChangeFolderProcess();
                }
                else if (e.KeyCode == Keys.O)
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    SelectFileProcess();
                }
                else if (e.KeyCode == Keys.W)
                {
                    this.Close();
                }
            }
        }
        #endregion

        #region file process
        private void ChangeFolderProcess()
        {
            using (var fm = new System.Windows.Forms.FolderBrowserDialog())
            {
                if (fm.ShowDialog() == DialogResult.OK)
                {
                    attendedScriptWatcher.Path = fm.SelectedPath;
                    LoadAttendedScripts();
                }
            }
        }
        private void SelectFileProcess()
        {
            using (var fm = new OpenFileDialog())
            {
                fm.Filter = "Script file (*.xml)|*.xml|All files (*.*)|*.*";
                fm.InitialDirectory = attendedScriptWatcher.Path;
                if (fm.ShowDialog() == DialogResult.OK)
                {
                    string newPath = fm.FileName;
                    attendedScriptWatcher.Path = System.IO.Path.GetDirectoryName(newPath);
                    LoadAttendedScripts();

                    cboSelectedScript.SelectedItem = System.IO.Path.GetFileName(newPath);
                }
            }
        }
        private void LoadAttendedScripts()
        {
            //get script files
            var files = System.IO.Directory.GetFiles(attendedScriptWatcher.Path);

            cboSelectedScript.BeginUpdate();

            //clear script list
            cboSelectedScript.Items.Clear();
            //loop each file and add to potential
            foreach (var fil in files)
            {
                var filInfo = new System.IO.FileInfo(fil);
                cboSelectedScript.Items.Add(filInfo.Name);
            }

            cboSelectedScript.EndUpdate();
        }
        #endregion
    }
}
