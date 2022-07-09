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
    public partial class frmInspect : ThemedForm
    {
        private XElement xml = null;

        public frmInspect()
        {
            InitializeComponent();
        }

        private void frmInspect_Load(object sender, EventArgs e)
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

            tvElements.Nodes.Clear();
            tvElements.Nodes.Add(nodes);

            tvElements.ExpandAll();

            tvElements.ResumeLayout();

            txtElementInformation.Text = "";
        }

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
    }
}
