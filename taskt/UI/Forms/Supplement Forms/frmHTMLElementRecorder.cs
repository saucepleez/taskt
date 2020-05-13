using Microsoft.Office.Interop.Outlook;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;
using taskt.Core.Automation.Commands;
using taskt.Core.Automation.User32;

namespace taskt.UI.Forms.Supplemental
{
    public partial class frmHTMLElementRecorder : UIForm
    {
        public frmHTMLElementRecorder()
        {
            InitializeComponent();
        }

        public DataTable searchParameters;
        public string LastItemClicked;
        private void frmHTMLElementRecorder_Load(object sender, EventArgs e)
        {
  
        }
      
        private void pbRecord_Click(object sender, EventArgs e)
        {
            this.TopMost = true;
            if (!chkStopOnClick.Checked)
            {
                lblDescription.Text = $"Recording.  Press F2 to stop recording!";
            }

            this.searchParameters = new DataTable();
            this.searchParameters.Columns.Add("Enabled");
            this.searchParameters.Columns.Add("Parameter Name");
            this.searchParameters.Columns.Add("Parameter Value");
            this.searchParameters.TableName = DateTime.Now.ToString("UIASearchParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"));

            //clear all
            searchParameters.Rows.Clear();

            //start global hook and wait for left mouse down event
            User32Functions.GlobalHook.StartEngineCancellationHook(Keys.F2);
            User32Functions.GlobalHook.HookStopped += GlobalHook_HookStopped;
            User32Functions.GlobalHook.StartElementCaptureHook(chkStopOnClick.Checked);
            webBrowser1.Document.Click += new HtmlElementEventHandler(Document_Click);
            
        }
        private void GlobalHook_HookStopped(object sender, EventArgs e)
        {
            Document_Click(null, null);
            this.Close();
        }

        private void Document_Click(object sender, HtmlElementEventArgs e)
        {
            //mouse down has occured
            try
            {
                HtmlElement element = webBrowser1.Document.GetElementFromPoint(e.ClientMousePosition);

                string savedId = element.Id;
                string uniqueId = Guid.NewGuid().ToString();
                element.Id = uniqueId;

                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(element.Document.GetElementsByTagName("html")[0].OuterHtml);
                element.Id = savedId;
                HtmlAgilityPack.HtmlNode node = doc.GetElementbyId(uniqueId);
                
                string xpath = node.XPath.Replace("[1]", "");
                string name = element.GetAttribute("name") == null ? "" : element.GetAttribute("name");
                string id = element.GetAttribute("id") == null ? "" : element.GetAttribute("id"); ;
                string className = element.GetAttribute("className") == null ? "" : element.GetAttribute("className");
                string linkText = element.TagName.ToLower() == "a" ? element.InnerText : ""; 

                LastItemClicked = $"[XPath:{xpath}].[ID:{id}].[Name:{name}].[Tag Name:{element.TagName}].[Class:{className}].[Link Text:{linkText}]";
                lblSubHeader.Text = LastItemClicked;

                searchParameters.Rows.Clear();
                searchParameters.Rows.Add("XPath", xpath);
                searchParameters.Rows.Add("ID", id);
                searchParameters.Rows.Add("Name", name);
                searchParameters.Rows.Add("Tag Name", element.TagName);
                searchParameters.Rows.Add("Class Name", className);
                searchParameters.Rows.Add("CSS Selector", element.Style); //TODO produce the appropriate CSS selector for selenium automation
                searchParameters.Rows.Add("Link Text", linkText);
            }
            catch (System.Exception)
            {
                lblDescription.Text = "Error cloning element. Please Try Again.";
            }
            
            if (chkStopOnClick.Checked)
            {
                this.Close();     
            }

        }

        private void pbRefresh_Click(object sender, EventArgs e)
        {
            webBrowser1.Refresh();
        }

        private void uiBtnOk_Click(object sender, EventArgs e)
        {       
            this.DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void pbGo_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate(tbURL.Text);
        }

        private void pbBack_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
        }

        private void pbForward_Click(object sender, EventArgs e)
        {
            webBrowser1.GoForward();
        } 

        private void tbURL_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                webBrowser1.Navigate(tbURL.Text);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
    }

}
