using System;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Windows.Automation;
using System.Windows.Forms;
using taskt.Core.Common;
using taskt.Core.User32;
using taskt.Utilities;
using Enums = taskt.Core.Enums;

namespace taskt.UI.Forms.Supplement_Forms
{
    public partial class frmThickAppElementRecorder : UIForm
    {
        public DataTable SearchParameters;
        public string LastItemClicked;

        public frmThickAppElementRecorder()
        {
            InitializeComponent();
        }
        
        private void frmThickAppElementRecorder_Load(object sender, EventArgs e)
        {
            //create data source from windows
            cboWindowTitle.DataSource = Common.GetAvailableWindowNames();
        }

        private void pbRecord_Click(object sender, EventArgs e)
        {
            // this.WindowState = FormWindowState.Minimized;

            if (!chkStopOnClick.Checked)
            {
                lblDescription.Text = $"Recording.  Press F2 to stop recording!";
                MoveFormToBottomRight(this);
                TopMost = true;
            }
            else
            {
                WindowState = FormWindowState.Minimized;
            }
         
            Size = new Size(540, 156);
            SearchParameters = new DataTable();
            SearchParameters.Columns.Add("Enabled");
            SearchParameters.Columns.Add("Parameter Name");
            SearchParameters.Columns.Add("Parameter Value");
            SearchParameters.TableName = DateTime.Now.ToString("UIASearchParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"));

            //clear all
            SearchParameters.Rows.Clear();

            //get window name and find window
            string windowName = cboWindowTitle.Text;
            IntPtr hWnd = User32Functions.FindWindow(windowName);

            //check if window is found
            if (hWnd != IntPtr.Zero)
            {
                //set window state and move to 0,0
                User32Functions.SetWindowState(hWnd, Enums.WindowState.SwShowNormal);
                User32Functions.SetForegroundWindow(hWnd);
                User32Functions.SetWindowPosition(hWnd, 0, 0);

                //start global hook and wait for left mouse down event
                GlobalHook.StartEngineCancellationHook(Keys.F2);
                GlobalHook.HookStopped += GlobalHook_HookStopped;
                GlobalHook.StartElementCaptureHook(chkStopOnClick.Checked);
                GlobalHook.MouseEvent += GlobalHook_MouseEvent;
            }
        }

        private void GlobalHook_HookStopped(object sender, EventArgs e)
        {
            GlobalHook_MouseEvent(null, null);
            Close();
        }

        private void GlobalHook_MouseEvent(object sender, MouseCoordinateEventArgs e)
        {
            //mouse down has occured

            //invoke UIA
            try
            {
                AutomationElement element = AutomationElement.FromPoint(e.MouseCoordinates);
                AutomationElement.AutomationElementInformation elementProperties = element.Current;

                LastItemClicked = $"[Name:{element.Current.Name}].[ID:{element.Current.AutomationId.ToString()}].[Class:{element.Current.ClassName}]";
                lblSubHeader.Text = LastItemClicked;

                SearchParameters.Rows.Clear();

                //get properties from class via reflection
                PropertyInfo[] properties = typeof(AutomationElement.AutomationElementInformation).GetProperties();
                Array.Sort(properties, (x, y) => String.Compare(x.Name, y.Name));

                //loop through each property and get value from the element
                foreach (PropertyInfo property in properties)
                {
                    try
                    {
                        var propName = property.Name;
                        var propValue = property.GetValue(elementProperties, null);

                        //if property is a basic type then display
                        if ((propValue is string) || (propValue is bool) || (propValue is int) || (propValue is double))
                        {
                            SearchParameters.Rows.Add(false, propName, propValue);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error Iterating over properties in window: " + ex.ToString());
                    }
                }
            }
            catch (Exception)
            {
                lblDescription.Text = "Error cloning element. Please Try Again.";
                //MessageBox.Show("Error in recording, please try again! " + ex.ToString());
            }

            if (chkStopOnClick.Checked)
            {
                Close();     
            }
        }

        private void pbRefresh_Click(object sender, EventArgs e)
        {
            //handle window refresh requests
            cboWindowTitle.DataSource = Common.GetAvailableWindowNames();
        }

        private void uiBtnOk_Click(object sender, EventArgs e)
        {       
            DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
