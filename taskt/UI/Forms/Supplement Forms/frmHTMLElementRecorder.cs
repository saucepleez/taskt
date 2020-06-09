using System;
using System.Data;
using System.Windows.Forms;
using taskt.Core.Automation.User32;
using HtmlAgilityPack;

namespace taskt.UI.Forms.Supplement_Forms
{
    public partial class frmHTMLElementRecorder : UIForm
    {
        public DataTable SearchParameters;
        public string LastItemClicked;

        public frmHTMLElementRecorder()
        {
            InitializeComponent();
        }

        private void frmHTMLElementRecorder_Load(object sender, EventArgs e)
        {

        }

        private void pbRecord_Click(object sender, EventArgs e)
        {
            TopMost = true;
            if (!chkStopOnClick.Checked)
            {
                lblDescription.Text = $"Recording.  Press F2 to stop recording!";
            }

            SearchParameters = new DataTable();
            SearchParameters.Columns.Add("Enabled");
            SearchParameters.Columns.Add("Parameter Name");
            SearchParameters.Columns.Add("Parameter Value");
            SearchParameters.TableName = DateTime.Now.ToString("UIASearchParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"));

            //clear all
            SearchParameters.Rows.Clear();

            //start global hook and wait for left mouse down event
            GlobalHook.StartEngineCancellationHook(Keys.F2);
            GlobalHook.HookStopped += GlobalHook_HookStopped;
            GlobalHook.StartElementCaptureHook(chkStopOnClick.Checked);
            wbElementRecorder.Document.Click += new HtmlElementEventHandler(Document_Click);

        }
        private void GlobalHook_HookStopped(object sender, EventArgs e)
        {
            Document_Click(null, null);
            Close();
        }

        private void Document_Click(object sender, HtmlElementEventArgs e)
        {
            //mouse down has occured
            try
            {
                HtmlElement element = wbElementRecorder.Document.GetElementFromPoint(e.ClientMousePosition);

                string savedId = element.Id;
                string uniqueId = Guid.NewGuid().ToString();
                element.Id = uniqueId;

                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(element.Document.GetElementsByTagName("html")[0].OuterHtml);
                element.Id = savedId;
                HtmlNode node = doc.GetElementbyId(uniqueId);

                string xpath = node.XPath.Replace("[1]", "");
                string name = element.GetAttribute("name") == null ? "" : element.GetAttribute("name");
                string id = element.GetAttribute("id") == null ? "" : element.GetAttribute("id"); ;
                string className = element.GetAttribute("className") == null ? "" : element.GetAttribute("className");
                string linkText = element.TagName.ToLower() == "a" ? element.InnerText : "";

                LastItemClicked = $"[XPath:{xpath}].[ID:{id}].[Name:{name}].[Tag Name:{element.TagName}].[Class:{className}].[Link Text:{linkText}]";
                lblSubHeader.Text = LastItemClicked;

                SearchParameters.Rows.Clear();
                SearchParameters.Rows.Add("XPath", xpath);
                SearchParameters.Rows.Add("ID", id);
                SearchParameters.Rows.Add("Name", name);
                SearchParameters.Rows.Add("Tag Name", element.TagName);
                SearchParameters.Rows.Add("Class Name", className);
                SearchParameters.Rows.Add("CSS Selector", element.Style); //TODO produce the appropriate CSS selector for selenium automation
                SearchParameters.Rows.Add("Link Text", linkText);
            }
            catch (Exception)
            {
                lblDescription.Text = "Error cloning element. Please Try Again.";
            }

            if (chkStopOnClick.Checked)
            {
                Close();
            }
        }

        private void pbRefresh_Click(object sender, EventArgs e)
        {
            wbElementRecorder.Refresh();
        }

        private void uiBtnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void pbGo_Click(object sender, EventArgs e)
        {
            wbElementRecorder.Navigate(tbURL.Text);
        }

        private void pbBack_Click(object sender, EventArgs e)
        {
            wbElementRecorder.GoBack();
        }

        private void pbForward_Click(object sender, EventArgs e)
        {
            wbElementRecorder.GoForward();
        }

        private void tbURL_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                wbElementRecorder.Navigate(tbURL.Text);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
    }
}
