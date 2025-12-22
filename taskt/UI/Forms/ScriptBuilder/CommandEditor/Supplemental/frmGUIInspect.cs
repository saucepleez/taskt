using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Automation;
using System.Windows.Forms;
using System.Xml.Linq;
using taskt.Core.Automation.Commands;
using taskt.Core.Script;

/*
 * NOTE: This form is called primarily by frmCommandEditor, so the namespace looks like this
 */
namespace taskt.UI.Forms.ScriptBuilder.CommandEditor.Supplemental
{
    public partial class frmGUIInspect : DialogLikeThemedForm
    {
        private XElement xml = null;
        private Core.Automation.Engine.AutomationEngineInstance engine;

        public frmGUIInspect()
        {
            InitializeComponent();
            engine = new Core.Automation.Engine.AutomationEngineInstance();
            this.FormClosed += SupplementFormsEvents.SupplementFormClosed;
        }

        #region form events
        private void frmGUIInspect_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            SupplementFormsEvents.SupplementFormLoad(this);
            ReloadWindowNames();
        }
        #endregion

        #region window name events
        private void btnReload_Click(object sender, EventArgs e)
        {
            ReloadWindowNames();
        }

        private void cmbWindowList_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbWindowList.Text != "")
            {
                CreateElementTree();
                tvElements.Focus();
            }
        }

        private void ReloadWindowNames()
        {
            var windows = EM_CanHandleWindowNameExtensionMethods.GetAllWindowNamesAndHandles().Select(item => item.Item2).Distinct().ToList();

            string currentWindow = cmbWindowList.Text;

            cmbWindowList.Enabled = false;

            cmbWindowList.SuspendLayout();
            cmbWindowList.BeginUpdate();
            cmbWindowList.Items.Clear();

            foreach (string win in windows)
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

            cmbWindowList.Enabled = true;

            CreateElementTree();

            ShowMessageTimer("Window Names Updated");
        }

        private void CreateElementTree()
        {
            if (cmbWindowList.Text == "")
            {
                tvElements.Nodes.Clear();
                xml = null;
                return;
            }
            string windowName = cmbWindowList.Text;

            cmbWindowList.Enabled = false;

            try
            {
                var nodes = UIElementControls.GetElementTreeNode(windowName, engine, out xml);

                tvElements.SuspendLayout();
                tvElements.BeginUpdate();

                tvElements.Nodes.Clear();
                tvElements.Nodes.Add(nodes);

                tvElements.ExpandAll();

                tvElements.Nodes[0].EnsureVisible();    // move to top

                tvElements.EndUpdate();
                tvElements.ResumeLayout();

                txtElementInformation.Text = "";

                ShowMessageTimer("Element Tree created.");
            }
            catch(Exception ex)
            {
                tvElements.Nodes.Clear();
                txtElementInformation.Text = "Error: " + ex.Message;
            }

            cmbWindowList.Enabled = true;
        }
        #endregion

        #region treeview events
        private void tvElements_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Parent != null)
            {
                AutomationElement elem = (AutomationElement)e.Node.Tag;
                ShowElementInformation((AutomationElement)e.Node.Tag);
                HighlightElement(elem);
            }
            else
            {
                txtXPath.Text = "";
            }
        }
        private void tvElements_AfterCheck(object sender, TreeViewEventArgs e)
        {
            //Console.WriteLine(countCheckedInTvElements());

            if (e.Action != TreeViewAction.Unknown)
            {
                ClearAllCheckInTvElements();
                e.Node.Checked = true;
            }
        }

        private void ClearAllCheckInTvElements()
        {
            if (!tvElements.CheckBoxes)
            {
                return;
            }

            tvElements.BeginUpdate();

            tvElements.Nodes[0].Checked = false;
            ClearCheckTreeNode(tvElements.Nodes[0]);

            tvElements.EndUpdate();
        }

        private static void ClearCheckTreeNode(TreeNode root)
        {
            if (root.Nodes.Count == 0)
            {
                root.Checked = false;
                return;
            }
            foreach(TreeNode node in root.Nodes)
            {
                node.Checked = false;
                if (node.Nodes.Count > 0)
                {
                    ClearCheckTreeNode(node);
                }
            }
        }

        private TreeNode GetCheckedNode()
        {
            if (!tvElements.CheckBoxes)
            {
                return tvElements.Nodes[0];
            }

            TreeNode checkedNode = SearchCheckedNode(tvElements.Nodes[0]);
            if (checkedNode != null)
            {
                return checkedNode;
            }
            else
            {
                return tvElements.Nodes[0];
            }
        }

        private static TreeNode SearchCheckedNode(TreeNode root)
        {
            if (root.Nodes.Count == 0)
            {
                if (root.Checked)
                {
                    return root;
                }
                else
                {
                    return null;
                }
            }

            foreach(TreeNode node in root.Nodes)
            {
                if (node.Checked)
                {
                    return node;
                }

                TreeNode t = SearchCheckedNode(node);
                if (t != null)
                {
                    return t;
                }
            }

            return null;
        }

        private void chkElementReload_CheckedChanged(object sender, EventArgs e)
        {
            if (chkElementReload.Checked)
            {
                timerElementReload.Stop();
                timerElementReload.Start();
            }
            else
            {
                timerElementReload.Stop();
            }
        }

        private void timerElementReload_Tick(object sender, EventArgs e)
        {
            if (cmbWindowList.Text != "")
            {
                timerElementReload.Stop();

                CreateElementTree();
                tvElements.Focus();
                
                timerElementReload.Start();
            }
        }
        #endregion

        #region XPath checkbox events
        private void chkUseNameAttr_CheckedChanged(object sender, EventArgs e)
        {
            ReloadXPath();
        }

        private void chkUseAutomationIdAttr_CheckedChanged(object sender, EventArgs e)
        {
            ReloadXPath();
        }

        private void ReloadXPath()
        {
            if (tvElements.SelectedNode != null)
            {
                AutomationElement elem = (AutomationElement)tvElements.SelectedNode.Tag;
                ShowElementInformation(elem);
            }
        }
        #endregion

        #region node methods
        private void ShowElementInformation(AutomationElement elem)
        {
            txtElementInformation.Text = UIElementControls.GetInspectResultFromAutomationElement(elem);

            if ((chkShowInTree.Checked) && (chkXPathRelative.Checked))
            {
                TreeNode chk = GetCheckedNode();
                AutomationElement curElem = (AutomationElement)chk.Tag;
                txtXPath.Text = UIElementControls.GetXPath(xml, elem, curElem, chkUseNameAttr.Checked, chkUseAutomationIdAttr.Checked);
            }
            else
            {
                txtXPath.Text = UIElementControls.GetXPath(xml, elem, chkUseNameAttr.Checked, chkUseAutomationIdAttr.Checked);
            }
        }

        private void HighlightElement(AutomationElement elem)
        {
            try
            {
                System.Windows.Rect rect = elem.Current.BoundingRectangle;
                Rectangle outerRect = new Rectangle
                {
                    X = (int)rect.X - 3,
                    Y = (int)rect.Y - 3,
                    Width = (int)rect.Width + 6,
                    Height = (int)rect.Height + 6
                };
                Rectangle middleRect = new Rectangle
                {
                    X = (int)rect.X - 2,
                    Y = (int)rect.Y - 2,
                    Width = (int)rect.Width + 4,
                    Height = (int)rect.Height + 4
                };
                Rectangle innerRect = new Rectangle
                {
                    X = (int)rect.X,
                    Y = (int)rect.Y,
                    Width = (int)rect.Width,
                    Height = (int)rect.Height
                };

                Graphics g = Graphics.FromHwnd(IntPtr.Zero);

                g.DrawRectangle(new Pen(Color.Black, 1), outerRect);
                g.DrawRectangle(new Pen(Color.Yellow, 2), middleRect);
                g.DrawRectangle(new Pen(Color.Black, 1), innerRect);
            }
            catch
            {
                return;
            }
        }
        #endregion

        #region Footer buttons

        private void uiBtnAdd_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        #endregion

        #region Double-Click TextBox
        private void txtElementInformation_DoubleClick(object sender, EventArgs e)
        {
            txtElementInformation.SelectAll();
            Clipboard.SetText(txtElementInformation.Text);

            ShowMessageTimer("Element Result Copied!!");
        }

        private void txtXPath_DoubleClick(object sender, EventArgs e)
        {
            txtXPath.SelectAll();
            if (!string.IsNullOrEmpty(txtXPath.Text))
            {
                Clipboard.SetText(txtXPath.Text);
                //lblMessage.Text = "XPath Copied!!";
                //lblMessage.Visible = true;
                //timerLabelShowTime.Start();
                ShowMessageTimer("XPath Copied!!");
            }
        }

        private void timerLabelShowTime_Tick(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        private void ShowMessageTimer(string message)
        {
            lblMessage.Text = message;
            lblMessage.Visible = true;
            timerLabelShowTime.Stop();
            timerLabelShowTime.Start();
        }
        #endregion

        #region controlPanel
        private void chkShowInTree_CheckedChanged(object sender, EventArgs e)
        {
            TreeNode selectedNode = null;
            if (tvElements.SelectedNode != null)
            {
                selectedNode = tvElements.SelectedNode;
            }

            bool chkState = chkShowInTree.Checked;
            tvElements.CheckBoxes = chkState;
            chkXPathRelative.Visible = chkState;
            chkXPathRelative.Checked = chkState;

            if (chkState)
            {
                chkShowInTree.Text = "Show Check";
            }
            else
            {
                chkShowInTree.Text = "Hide Check";
            }

            tvElements.ExpandAll();

            if (selectedNode != null)
            {
                tvElements.SelectedNode = selectedNode;
                tvElements.SelectedNode.EnsureVisible();
            }
            else
            {
                if (tvElements.Nodes.Count > 0)
                {
                    tvElements.Nodes[0].EnsureVisible();
                }
                
                txtElementInformation.Text = "";
                txtXPath.Text = "";
            }
            tvElements.Focus();
        }

        private void chkXPathRelative_CheckedChanged(object sender, EventArgs e)
        {
            if (tvElements.SelectedNode != null)
            {
                ShowElementInformation((AutomationElement)tvElements.SelectedNode.Tag);
            }
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

        public string InspectResult
        {
            get
            {
                return txtElementInformation.Text;
            }
        }

        #endregion
    }
}
