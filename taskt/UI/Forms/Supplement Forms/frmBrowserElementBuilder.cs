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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SHDocVw;
using MSHTML;
using System.Runtime.InteropServices;

namespace taskt.UI.Forms.Supplemental
{
    public partial class frmBrowserElementBuilder : UIForm
    {
        //overall form events

        #region Form Events

        InternetExplorer ie;
        DataTable searchParameters;

        //public delegate void SearchParametersDelegate(DataTable searchParams);
        public frmBrowserElementBuilder()
        {
            InitializeComponent();
        }

        private void frmBrowserElementBuilder_Load(object sender, EventArgs e)
        {
            //find all IE windows
            FindIEWindows();

            //gridview setup
            dgvSearchParameters.AutoGenerateColumns = false;
            DataGridViewCheckBoxColumn enabledColumn = new DataGridViewCheckBoxColumn();
            enabledColumn.HeaderText = "Enabled";
            enabledColumn.DataPropertyName = "Enabled";
            dgvSearchParameters.Columns.Add(enabledColumn);

            DataGridViewTextBoxColumn propertyName = new DataGridViewTextBoxColumn();
            propertyName.HeaderText = "Property Name";
            propertyName.DataPropertyName = "Property Name";
            dgvSearchParameters.Columns.Add(propertyName);

            DataGridViewTextBoxColumn propertyValue = new DataGridViewTextBoxColumn();
            propertyValue.HeaderText = "Property Value";
            propertyValue.DataPropertyName = "Property Value";
            dgvSearchParameters.Columns.Add(propertyValue);
        }

        #endregion Form Events

        //combobox events for form items

        #region ComboBox Events

        private void cboIEWindow_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var shellWindows = new ShellWindows();
            foreach (IWebBrowser2 shellWindow in shellWindows)
            {
                if (shellWindow.Document is MSHTML.HTMLDocument)
                {
                    if (shellWindow.Document.Title == cboIEWindow.Text)
                    {
                        ie = shellWindow.Application;
                        var events = (HTMLDocumentEvents2_Event)ie.Document;

                        events.onclick += (evt) =>
                        {
                            searchParameters = new DataTable();
                            searchParameters.Columns.Add("Enabled");
                            searchParameters.Columns.Add("Property Name");
                            searchParameters.Columns.Add("Property Value");

                            if (evt.srcElement is IHTMLElement)

                            {
                                IHTMLElement srcInfo = evt.srcElement;
                                var elementProperties = srcInfo.GetType().GetProperties();

                                foreach (System.Reflection.PropertyInfo prp in elementProperties)
                                {
                                    if ((prp.PropertyType == typeof(string)) && (!prp.Name.Contains("IHTML")) || (prp.PropertyType == typeof(int)) && (!prp.Name.Contains("IHTML")))
                                    {
                                        string propName = prp.Name;
                                        string propValue = Convert.ToString(prp.GetValue(srcInfo));
                                        searchParameters.Rows.Add(false, propName, propValue);
                                    }
                                }

                                dgvSearchParameters.Invoke((MethodInvoker)(() => dgvSearchParameters.DataSource = searchParameters));
                            }

                            return false;
                        };

                        var activateWindow = new Core.Automation.Commands.ActivateWindowCommand();
                        activateWindow.v_WindowName = cboIEWindow.Text + " - Internet Explorer";
                        activateWindow.RunCommand(null);

                        var moveWindow = new Core.Automation.Commands.MoveWindowCommand();
                        moveWindow.v_WindowName = cboIEWindow.Text + " - Internet Explorer";
                        moveWindow.v_XWindowPosition = "0";
                        moveWindow.v_YWindowPosition = "0";
                        moveWindow.RunCommand(null);

                        MoveFormToBottomRight(this);
                        this.TopMost = true;

                        foreach (Form frm in Application.OpenForms)
                        {
                            if (frm.Name != this.Name)
                            {
                                frm.WindowState = FormWindowState.Minimized;
                            }
                        }
                    }
                }
            }
        }

        private void cboAction_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //if (cboAction.Text == "Invoke Click")
            // {
            //     dgvRequiredParameters.Visible = false;
            //     dgvRequiredParameters.DataSource = null;
            //     return;
            // }

            // actionParameters = new DataTable();
            // actionParameters.Columns.Add("Parameter Name");
            // actionParameters.Columns.Add("Parameter Value");

            // switch (cboAction.Text)
            // {
            //     case "Get Attribute":
            //         dgvRequiredParameters.Visible = true;
            //         actionParameters.Rows.Add("Attribute Name");
            //         actionParameters.Rows.Add("Variable Name");
            //         break;
            //     case "Set Attribute":
            //         dgvRequiredParameters.Visible = true;
            //         actionParameters.Rows.Add("Attribute Name");
            //         actionParameters.Rows.Add("Value To Set");
            //         break;
            //     default:
            //         break;
            // }

            // dgvRequiredParameters.DataSource = actionParameters;
        }

        #endregion ComboBox Events

        //helper for locating IE windows

        #region IEWindowHelper

        private void uiBtnRefresh_Click(object sender, EventArgs e)
        {
            dgvSearchParameters.DataSource = null;
            FindIEWindows();
        }
        private void FindIEWindows()
        {
            //cboIEWindow.Items.Clear();
            var shellWindows = new ShellWindows();
            foreach (IWebBrowser2 shellWindow in shellWindows)
            {
                try
                {
                    if (shellWindow.Document is MSHTML.HTMLDocument)
                    {
                        string windowTitle = shellWindow.Document.Title;
                        cboIEWindow.Items.Add(windowTitle);
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        #endregion IEWindowHelper

        //dialog result

        #region Ok/Cancel Buttons

        private void uiBtnOK_Click(object sender, EventArgs e)
        {
            dgvSearchParameters.EndEdit();
            this.DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        #endregion Ok/Cancel Buttons

        private void cboIEWindow_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void cboAction_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}