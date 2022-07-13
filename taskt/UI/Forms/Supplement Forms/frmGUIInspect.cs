using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using taskt.Core.Automation.Commands;
using System.Xml.Linq;
using System.Windows.Automation;

namespace taskt.UI.Forms.Supplement_Forms
{
    public partial class frmGUIInspect : ThemedForm
    {
        private XElement xml = null;

        public frmGUIInspect()
        {
            InitializeComponent();
        }

        private void frmGUIInspect_Load(object sender, EventArgs e)
        {
            reloadWindowNames();
        }
        private void btnReload_Click(object sender, EventArgs e)
        {
            reloadWindowNames();
        }
        private void cmbWindowList_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbWindowList.Text != "")
            {
                createElementTree();
                tvElements.Focus();
            }
        }

        private void tvElements_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Parent != null)
            {
                showElementInformation((AutomationElement)e.Node.Tag);
            }
            else
            {
                txtXPath.Text = "";
            }
        }

        private void reloadWindowNames()
        {
            List<string> windows = WindowNameControls.GetAllWindowTitles();

            string currentWindow = cmbWindowList.SelectedText;

            cmbWindowList.SuspendLayout();
            cmbWindowList.BeginUpdate();
            cmbWindowList.Items.Clear();

            foreach(string win in windows)
            {
                cmbWindowList.Items.Add(win);
            }

            if (cmbWindowList.Items.Contains(currentWindow))
            {
                cmbWindowList.Text = currentWindow;
            }
            else
            {
                cmbWindowList.Text = "";
            }

            cmbWindowList.EndUpdate();
            cmbWindowList.ResumeLayout();

            createElementTree();
        }

        private void createElementTree()
        {
            if (cmbWindowList.Text == "")
            {
                tvElements.Nodes.Clear();
                xml = null;
                return;
            }
            string windowName = cmbWindowList.Text;

            var nodes = AutomationElementControls.GetElementTreeNode(windowName, out xml);

            tvElements.SuspendLayout();
            tvElements.BeginUpdate();

            tvElements.Nodes.Clear();
            tvElements.Nodes.Add(nodes);

            tvElements.ExpandAll();

            tvElements.Nodes[0].EnsureVisible();    // move to top

            tvElements.EndUpdate();
            tvElements.ResumeLayout();

            txtElementInformation.Text = "";
        }

        #region Footer buttons
        private void showElementInformation(AutomationElement elem)
        {
            txtElementInformation.Text = AutomationElementControls.GetInspectResultFromAutomationElement(elem);

            txtXPath.Text = AutomationElementControls.GetXPath(xml, elem);
        }

        private void uiBtnAdd_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        #endregion

        #region Double-Click TextBox
        private void txtElementInformation_DoubleClick(object sender, EventArgs e)
        {
            txtElementInformation.SelectAll();
            Clipboard.SetText(txtElementInformation.Text);

            lblMessage.Text = "Element Result Copied!!";
            lblMessage.Visible = true;
            timerLabelShowTime.Start();
        }
        private void txtXPath_DoubleClick(object sender, EventArgs e)
        {
            txtXPath.SelectAll();
            Clipboard.SetText(txtXPath.Text);

            lblMessage.Text = "XPath Copied!!";
            lblMessage.Visible = true;
            timerLabelShowTime.Start();
        }

        private void timerLabelShowTime_Tick(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }
        #endregion

        #region Properties

        public string XPath
        {
            get
            {
                return txtXPath.Text;
            }
        }

        public string WindowName
        {
            get
            {
                return cmbWindowList.Text;
            }
        }

        #endregion

    }
}
