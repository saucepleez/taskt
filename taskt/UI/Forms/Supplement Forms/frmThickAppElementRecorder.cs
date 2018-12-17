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
using taskt.Core.AutomationCommands;

namespace taskt.UI.Forms.Supplemental
{
    public partial class frmThickAppElementRecorder : UIForm
    {
        
        public frmThickAppElementRecorder()
        {
            InitializeComponent();
        }

        public DataTable searchParameters;
        private void frmThickAppElementRecorder_Load(object sender, EventArgs e)
        {
            //create data source from windows
            cboWindowTitle.DataSource = Core.Common.GetAvailableWindowNames();
         
        }


      
        private void pbRecord_Click(object sender, EventArgs e)
        {

            this.WindowState = FormWindowState.Minimized;

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
                User32Functions.GlobalHook.StartElementCaptureHook();
                User32Functions.GlobalHook.MouseEvent += GlobalHook_MouseEvent;

            }
        }

        private void GlobalHook_MouseEvent(object sender, MouseCoordinateEventArgs e)
        {
            //mouse down has occured

            //invoke UIA
            try
            {
                System.Windows.Automation.AutomationElement element = System.Windows.Automation.AutomationElement.FromPoint(e.MouseCoordinates);
                System.Windows.Automation.AutomationElement.AutomationElementInformation elementProperties = element.Current;

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
            catch (Exception ex)
            {
                MessageBox.Show("Error in recording, please try again! " + ex.ToString());
            }

            this.WindowState = FormWindowState.Normal;
            this.Close();

           


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
