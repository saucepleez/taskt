using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using taskt.Core.Automation.Commands;
using taskt.Core.Automation.User32;

namespace taskt.UI.Forms.Supplemental
{
    public partial class frmThickAppElementRecorder : UIForm
    {
        
        public frmThickAppElementRecorder()
        {
            InitializeComponent();
        }

        public DataTable searchParameters;
        public string LastItemClicked;
        private void frmThickAppElementRecorder_Load(object sender, EventArgs e)
        {
            //create data source from windows
            cboWindowTitle.DataSource = Core.Common.GetAvailableWindowNames();
            
        }


      
        private void pbRecord_Click(object sender, EventArgs e)
        {

            // this.WindowState = FormWindowState.Minimized;

            if (!chkStopOnClick.Checked)
            {
                lblDescription.Text = $"Recording.  Press F2 to stop recording!";
                MoveFormToBottomRight(this);
                this.TopMost = true;
            }
            else
            {
                this.WindowState = FormWindowState.Minimized;
            }
         
            this.Size = new Size(540, 156);


            this.searchParameters = new DataTable();
            this.searchParameters.Columns.Add("Enabled");
            this.searchParameters.Columns.Add("Parameter Name");
            this.searchParameters.Columns.Add("Parameter Value");
            this.searchParameters.TableName = DateTime.Now.ToString("UIASearchParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"));


            //clear all
            searchParameters.Rows.Clear();

            //get window name and find window
            string windowName = cboWindowTitle.Text;
            IntPtr hWnd = User32Functions.FindWindow(windowName);

            //check if window is found
            if (hWnd != IntPtr.Zero)
            {
                //set window state and move to 0,0
                User32Functions.SetWindowState(hWnd, User32Functions.WindowState.SW_SHOWNORMAL);
                User32Functions.SetForegroundWindow(hWnd);
                User32Functions.SetWindowPosition(hWnd, 0, 0);

                //start global hook and wait for left mouse down event
                User32Functions.GlobalHook.StartEngineCancellationHook(Keys.F2);
                User32Functions.GlobalHook.HookStopped += GlobalHook_HookStopped;
                User32Functions.GlobalHook.StartElementCaptureHook(chkStopOnClick.Checked);
                User32Functions.GlobalHook.MouseEvent += GlobalHook_MouseEvent;
            }
        }

        private void GlobalHook_HookStopped(object sender, EventArgs e)
        {
            GlobalHook_MouseEvent(null, null);
            this.Close();
        }

        private void GlobalHook_MouseEvent(object sender, MouseCoordinateEventArgs e)
        {
            //mouse down has occured

            //invoke UIA
            try
            {
            
                System.Windows.Automation.AutomationElement element = System.Windows.Automation.AutomationElement.FromPoint(e.MouseCoordinates);
                System.Windows.Automation.AutomationElement.AutomationElementInformation elementProperties = element.Current;

                LastItemClicked = $"[Name:{element.Current.Name}].[ID:{element.Current.AutomationId.ToString()}].[Class:{element.Current.ClassName}]";
                lblSubHeader.Text = LastItemClicked;

                searchParameters.Rows.Clear();

                //get properties from class via reflection
                System.Reflection.PropertyInfo[] properties = typeof(System.Windows.Automation.AutomationElement.AutomationElementInformation).GetProperties();
                Array.Sort(properties, (x, y) => String.Compare(x.Name, y.Name));

                //loop through each property and get value from the element
                foreach (System.Reflection.PropertyInfo property in properties)
                {
                    try
                    {         
                    var propName = property.Name;
                    var propValue = property.GetValue(elementProperties, null);

                    //if property is a basic type then display
                    if ((propValue is string) || (propValue is bool) || (propValue is int) || (propValue is double))
                    {
                        searchParameters.Rows.Add(false, propName, propValue);
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
                this.Close();     
            }
              
            
           


        }

        private void pbRefresh_Click(object sender, EventArgs e)
        {
            //handle window refresh requests
            cboWindowTitle.DataSource = Core.Common.GetAvailableWindowNames();
        }

        private void uiBtnOk_Click(object sender, EventArgs e)
        {       
            this.DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }

}
