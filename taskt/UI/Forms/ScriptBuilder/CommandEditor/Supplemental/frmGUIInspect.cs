using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Windows.Automation;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;
using taskt.Core.Automation.Commands;
using taskt.Core.Automation.Commands.UIAutomationGroup;
using taskt.Core.Automation.Engine;
using taskt.Core.Script;

/*
 * NOTE: This form is called primarily by frmCommandEditor, so the namespace looks like this
 */
namespace taskt.UI.Forms.ScriptBuilder.CommandEditor.Supplemental
{
    public partial class frmGUIInspect : DialogLikeThemedForm
    {
        private XElement xml = null;
        private Dictionary<string, AutomationElement> hashTable = null;

        private AutomationEngineInstance engine = new AutomationEngineInstance(false);

        public frmGUIInspect()
        {
            InitializeComponent();
            this.FormClosed += SupplementFormsEvents.SupplementFormClosed;
        }

        #region form events
        private void frmGUIInspect_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            SupplementFormsEvents.SupplementFormLoad(this);

            // set time interval
            var searchTime = App.Taskt_Settings.ClientSettings.GUIInspectSearchTime + 1;
            timerElementReload.Interval = searchTime * 1000;
            chkElementReload.Text = $"A&uto Reload ({searchTime}s)";

            ReloadWindowNames();
        }
        #endregion

        #region window name events

        /// <summary>
        /// click window name reload button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReload_Click(object sender, EventArgs e)
        {
            ReloadWindowNames();
        }

        /// <summary>
        /// window name changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbWindowList_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbWindowList.Text != string.Empty)
            {
                CreateUIElementXMLTree();
                tvElements.Focus();
            }
        }

        private void ReloadWindowNames()
        {
            string currentWindow = cmbWindowList.Text;

            cmbWindowList.Enabled = false;

            cmbWindowList.SuspendLayout();
            cmbWindowList.BeginUpdate();
            cmbWindowList.Items.Clear();

            var windows = EM_CanHandleWindowNameExtensionMethods.GetAllWindowNamesAndHandles().Select(item => item.Item2).Distinct().ToList();
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
                cmbWindowList.Text = string.Empty;
            }

            cmbWindowList.EndUpdate();
            cmbWindowList.ResumeLayout();

            cmbWindowList.Enabled = true;

            CreateUIElementXMLTree();

            ShowMessageTimer("Window Names Updated");
        }

        /// <summary>
        /// create UIElement XML Tree
        /// </summary>
        private void CreateUIElementXMLTree()
        {
            if (cmbWindowList.Text == string.Empty)
            {
                tvElements.Nodes.Clear();
                xml = null;
                return;
            }

            string windowName = cmbWindowList.Text;

            cmbWindowList.Enabled = false;

            try
            {
                var nodes = GetTreeNodeFromUIElementXML(windowName, engine);

                tvElements.SuspendLayout();
                tvElements.BeginUpdate();

                tvElements.Nodes.Clear();
                tvElements.Nodes.Add(nodes);

                tvElements.ExpandAll();

                tvElements.Nodes[0].EnsureVisible();    // move to top

                tvElements.EndUpdate();
                tvElements.ResumeLayout();

                txtElementInformation.Text = string.Empty;

                ShowMessageTimer("UIElement Tree created.");
            }
            catch(Exception ex)
            {
                tvElements.Nodes.Clear();
                txtElementInformation.Text = $"Error: {ex.Message}";
            }

            cmbWindowList.Enabled = true;
        }

        /// <summary>
        /// get tree node from UIElement xml
        /// </summary>
        /// <param name="windowName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        private TreeNode GetTreeNodeFromUIElementXML(string windowName, AutomationEngineInstance engine)
        {
            //// cache request
            //var cacheReq = new CacheRequest();
            //cacheReq.Add(AutomationElement.NameProperty);
            //cacheReq.Add(AutomationElement.ControlTypeProperty);
            //cacheReq.Add(AutomationElement.LocalizedControlTypeProperty);
            //cacheReq.TreeScope = TreeScope.Element | TreeScope.Children;

            //var root = GetFromWindowName(windowName, engine);

            //cacheReq.Push();

            //var walker = TreeWalker.RawViewWalker;

            //var tree = CreateTreeNodeFromAutomationElement(root);
            //xml = CreateXmlElement(root);

            //GetChildElementTreeNode(tree, xml, root, walker, cacheReq, 1, engine);

            //cacheReq.Pop();

            //return tree;

            AutomationElement winRoot;
            using (var winElem = new InnerScriptVariable(engine))
            {
                // get target window UIElement
                var getWinElem = new UIAutomationGetWindowUIElementCommand()
                {
                    v_WindowName = windowName,
                    v_Result = winElem.VariableName,
                };
                getWinElem.RunCommand(engine);

                winRoot = (AutomationElement)winElem.VariableValue;
            }

            // wait time func
            var stopDateTime = DateTime.Now.AddSeconds(App.Taskt_Settings.ClientSettings.GUIInspectSearchTime);
            var waitFunc = new Func<bool>(() =>
            {
                return DateTime.Now > stopDateTime;
            });

            // create XML tree
            var searchXML = new UIAutomationUIElementActionAfterSearchUIElementByXPathFromWindowNameCommand()
            {
                v_WindowName = windowName,
            };
            (xml, hashTable) = searchXML.DeepCreateUIElementXMLCore(winRoot, waitFunc, engine);

            // DGB
            //Console.WriteLine("## element list");
            //foreach(var item in hashTable)
            //{
            //    var key = item.Key;
            //    var v = item.Value;
            //    Console.WriteLine($"{item.Key}, {v.Current.Name}, {v.Current.GetHashCode()}");
            //}

            var tree = CreateTreeNodeFromUIElement(winRoot);
            CreateTreeNodeFromChildElementsOfUIElemetXML(tree, xml);
            return tree;
        }

        /// <summary>
        /// create tree node From Child
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="root"></param>
        private void CreateTreeNodeFromChildElementsOfUIElemetXML(TreeNode tree, XElement root)
        {
            foreach (var element in root.Elements())
            {
                var node = CreateTreeNodeFromUIElement(GetUIElementFromXML(element));
                tree.Nodes.Add(node);
                if (element.Elements().Count() > 0)
                {
                    CreateTreeNodeFromChildElementsOfUIElemetXML(node, element);
                }
            }
        }

        //private static void GetChildElementTreeNode(TreeNode tree, XElement xml, AutomationElement rootElement, TreeWalker walker, CacheRequest cacheRequest, int depth, Engine.AutomationEngineInstance engine)
        //{
        //    var node = walker.GetFirstChild(rootElement, cacheRequest);
        //    //var node = walker.GetLastChild(rootElement);

        //    int siblingCount = 0;
        //    while (node != null)
        //    {
        //        var item = CreateTreeNodeFromAutomationElement(node);
        //        tree.Nodes.Add(item);

        //        var childXml = CreateXmlElement(node);
        //        xml.Add(childXml);

        //        if ((walker.GetFirstChild(node, cacheRequest) != null) && (depth < engine.engineSettings.MaxUIElementInpectDepth))
        //        //if ((walker.GetLastChild(node) != null) && (depth < engine.engineSettings.MaxUIElementInpectDepth))
        //        {
        //            GetChildElementTreeNode(item, childXml, node, walker, cacheRequest, (depth + 1), engine);
        //        }

        //        siblingCount++;
        //        if (siblingCount >= engine.engineSettings.MaxUIElementInspectSiblingNodes)
        //        {
        //            break;
        //        }

        //        node = walker.GetNextSibling(node, cacheRequest);
        //        //node = walker.GetPreviousSibling(node);
        //    }
        //}

        /// <summary>
        /// get UIElement from XML Tree
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        private AutomationElement GetUIElementFromXML(XElement elem)
        {
            var h = elem.Attribute("Hash").Value;
            if (hashTable.ContainsKey(h))
            {
                return hashTable[h];
            }
            else
            {
                throw new Exception($"Strange UIElement Hash '{h}'");
            }
        }

        /// <summary>
        /// Create treeNode from UIElement
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private static TreeNode CreateTreeNodeFromUIElement(AutomationElement element)
        {
            var node = new TreeNode
            {
                Text = $"\"{element.Current.Name}\" {element.Current.LocalizedControlType}",
                Tag = element
            };
            return node;
        }

        /// <summary>
        /// get UIElement Hash
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        private string GetUIElementHash(AutomationElement elem)
        {
            try
            {
                return hashTable.FirstOrDefault(item => (item.Value == elem)).Key;
            }
            catch
            {
                throw new Exception($"hashTable firstOrDefalult {elem.Current.Name} {elem.Current.LocalizedControlType}");
            }
        }

        /// <summary>
        /// get XElement from UIElement
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        private XElement GetXElementFromUIElement(AutomationElement elem)
        {
            var searchPath = $"//{EM_CanHandleUIElementExtentionMethods.GetControlTypeText(elem)}[@Hash=\"{GetUIElementHash(elem)}\"]";
            return xml.XPathSelectElement(searchPath);
        }

        /// <summary>
        /// Get XPath from Root
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="elem"></param>
        /// <param name="useNameAttribute"></param>
        /// <param name="useAutomationIdAttribute"></param>
        /// <returns></returns>
        private string GetXPathFromRoot(AutomationElement elem, bool useNameAttribute = true, bool useAutomationIdAttribute = false)
        {
            var trgElement = GetXElementFromUIElement(elem);

            // not found
            if (trgElement == null)
            {
                return string.Empty;
            }

            var xpath = string.Empty;
            while (trgElement.Parent != null)
            {
                xpath = CreateXPath(trgElement, useNameAttribute, useAutomationIdAttribute) + xpath;
                trgElement = trgElement.Parent;
            }

            return xpath;
        }

        /// <summary>
        ///  get xpath from UIElement
        /// </summary>
        /// <param name="trgElem"></param>
        /// <param name="curElem"></param>
        /// <param name="useNameAttribute"></param>
        /// <param name="useAutomationIdAttribute"></param>
        /// <returns></returns>
        public string GetXPathFromUIElement(AutomationElement trgElem, AutomationElement curElem, bool useNameAttribute = true, bool useAutomationIdAttribute = false)
        {
            var trgElement = GetXElementFromUIElement(trgElem);

            var curElement = GetXElementFromUIElement(curElem);
            if (curElement == null)
            {
                // curElem is root-window-node ?
                if (xml.Attribute("Hash").Value == curElem.GetHashCode().ToString())
                {
                    curElement = xml;
                }
            }

            // no found
            if ((trgElement == null) || (curElement == null))
            {
                return string.Empty;
            }

            string xpath = string.Empty;
            while (trgElement.Parent != null)
            {
                xpath = CreateXPath(trgElement, useNameAttribute, useAutomationIdAttribute) + xpath;
                trgElement = trgElement.Parent;

                if (trgElement == curElement)
                {
                    break;
                }
            }

            if (trgElement == curElement)
            {
                return $"/{xpath}";
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// create XPath
        /// </summary>
        /// <param name="elemNode"></param>
        /// <param name="useNameAttribute"></param>
        /// <param name="useAutomationIdAttribute"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private string CreateXPath(XElement elemNode, bool useNameAttribute = true, bool useAutomationIdAttribute = false)
        {
            var parentNode = elemNode.Parent;

            string elemType = elemNode.Name.ToString();
            string elemHash = elemNode.Attribute("Hash").Value;
            string xpath;

            // use AutomationId attribute
            if (useAutomationIdAttribute && (elemNode.Attribute("AutomationId").Value != string.Empty))
            {
                xpath = $"/{elemType}[@AutomationId=\"{SecurityElement.Escape(elemNode.Attribute("AutomationId").Value)}\"]";
                var idNode = parentNode.XPathSelectElement($".{xpath}");
                if (idNode != null)
                {
                    if (idNode.Attribute("Hash").Value == elemHash)
                    {
                        return xpath;
                    }
                }
            }

            // use Name attribute
            if (useNameAttribute && (elemNode.Attribute("Name").Value != string.Empty))
            {
                xpath = $"/{elemType}[@Name=\"{SecurityElement.Escape(elemNode.Attribute("Name").Value)}\"]";
                var nameNode = parentNode.XPathSelectElement($".{xpath}");
                if (nameNode != null)
                {
                    if (nameNode.Attribute("Hash").Value == elemHash)
                    {
                        return xpath;
                    }
                }
            }

            // tag-name & index XPath
            xpath = $"/{elemType}";
            var typeNodes = parentNode.XPathSelectElements($".{xpath}");
            int idx = 1;
            foreach (XElement nd in typeNodes)
            {
                if (nd.Attribute("Hash").Value == elemHash)
                {
                    return $"{xpath}[{idx}]";
                }
                idx++;
            }

            throw new Exception("Fail Create UIElement XPath");
        }
        #endregion

        #region treeview events
        private void tvElements_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Parent != null)
            {
                var elem = (AutomationElement)e.Node.Tag;
                ShowUIElementInformation((AutomationElement)e.Node.Tag);
                HighlightUIElement(elem);
            }
            else
            {
                txtXPath.Text = string.Empty;
            }
        }

        private void tvElements_AfterCheck(object sender, TreeViewEventArgs e)
        {
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
            if (cmbWindowList.Text != string.Empty)
            {
                timerElementReload.Stop();

                CreateUIElementXMLTree();
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
                ShowUIElementInformation(elem);
            }
        }
        #endregion

        #region node methods

        /// <summary>
        /// get (Microsoft) Inspect Tool like result from UIElement
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        public static string GetInspectResultFromUIElement(AutomationElement elem)
        {
            //string res = "";
            var res = new StringBuilder();

            try
            {
                res.Append($"Name:\t\"{elem.Current.Name}\"\r\n");
                res.Append($"ControlType:\t{EM_CanHandleUIElementExtentionMethods.GetControlTypeText(elem)}\r\n");
                res.Append($"LocalizedControlType:\t\"{elem.Current.LocalizedControlType}\"\r\n");
                res.Append($"IsEnabled:\t{elem.Current.IsEnabled}\r\n");
                res.Append($"IsOffscreen:\t{elem.Current.IsOffscreen}\r\n");
                res.Append($"IsKeyboardFocusable:\t{elem.Current.IsKeyboardFocusable}\r\n");
                res.Append($"HasKeyboardFocusable:\t{elem.Current.HasKeyboardFocus}\r\n");
                res.Append($"AccessKey:\t\"{elem.Current.AccessKey}\"\r\n");
                res.Append($"ProcessId:\t{elem.Current.ProcessId}\r\n");
                res.Append($"AutomationId:\t\"{elem.Current.AutomationId}\"\r\n");
                res.Append($"FrameworkId:\t\"{elem.Current.FrameworkId}\"\r\n");
                res.Append($"ClassName:\t\"{elem.Current.ClassName}\"\r\n");
                res.Append($"IsContentElement:\t{elem.Current.IsContentElement}\r\n");
                res.Append($"IsPassword:\t{elem.Current.IsPassword}\r\n");

                res.Append($"AcceleratorKey:\t\"{elem.Current.AcceleratorKey}\"\r\n");
                res.Append($"HelpText:\t\"{elem.Current.HelpText}\"\r\n");
                res.Append($"IsControlElement:\t{elem.Current.IsControlElement}\r\n");
                res.Append($"IsRequiredForForm:\t{elem.Current.IsRequiredForForm}\r\n");
                res.Append($"ItemStatus:\t\"{elem.Current.ItemStatus}\r\n");
                res.Append($"ItemType:\t\"{elem.Current.ItemType}\"\r\n");
                res.Append($"NativeWindowHandle:\t{elem.Current.NativeWindowHandle}\r\n");

                res.Append($"IsDockPatternAvailableProperty:\t{(bool)elem.GetCurrentPropertyValue(AutomationElement.IsDockPatternAvailableProperty)}\r\n");
                res.Append($"IsExpandCollapsePatternAvailableProperty:\t{(bool)elem.GetCurrentPropertyValue(AutomationElement.IsExpandCollapsePatternAvailableProperty)}\r\n");
                res.Append($"IsGridPatternAvailableProperty:\t{(bool)elem.GetCurrentPropertyValue(AutomationElement.IsGridPatternAvailableProperty)}\r\n");
                res.Append($"IsGridItemPatternAvailableProperty:\t{(bool)elem.GetCurrentPropertyValue(AutomationElement.IsGridItemPatternAvailableProperty)}\r\n");
                res.Append($"IsInvokePatternAvailableProperty:\t{(bool)elem.GetCurrentPropertyValue(AutomationElement.IsInvokePatternAvailableProperty)}\r\n");
                res.Append($"IsMultipleViewPatternAvailableProperty:\t{(bool)elem.GetCurrentPropertyValue(AutomationElement.IsMultipleViewPatternAvailableProperty)}\r\n");
                res.Append($"IsRangeValuePatternAvailableProperty:\t{(bool)elem.GetCurrentPropertyValue(AutomationElement.IsRangeValuePatternAvailableProperty)}\r\n");
                res.Append($"IsScrollPatternAvailableProperty:\t{(bool)elem.GetCurrentPropertyValue(AutomationElement.IsScrollPatternAvailableProperty)}\r\n");
                res.Append($"IsScrollItemPatternAvailableProperty:\t{(bool)elem.GetCurrentPropertyValue(AutomationElement.IsScrollItemPatternAvailableProperty)}\r\n");
                res.Append($"IsSelectionPatternAvailableProperty:\t{(bool)elem.GetCurrentPropertyValue(AutomationElement.IsSelectionPatternAvailableProperty)}\r\n");
                res.Append($"IsSelectionItemPatternAvailableProperty:\t{(bool)elem.GetCurrentPropertyValue(AutomationElement.IsSelectionItemPatternAvailableProperty)}\r\n");
                res.Append($"IsTablePatternAvailableProperty:\t{(bool)elem.GetCurrentPropertyValue(AutomationElement.IsTablePatternAvailableProperty)}\r\n");
                res.Append($"IsTableItemPatternAvailableProperty:\t{(bool)elem.GetCurrentPropertyValue(AutomationElement.IsTableItemPatternAvailableProperty)}\r\n");
                res.Append($"IsTextPatternAvailableProperty:\t{(bool)elem.GetCurrentPropertyValue(AutomationElement.IsTextPatternAvailableProperty)}\r\n");
                res.Append($"IsTogglePatternAvailableProperty:\t{(bool)elem.GetCurrentPropertyValue(AutomationElement.IsTogglePatternAvailableProperty)}\r\n");
                res.Append($"IsTransformPatternAvailableProperty:\t{(bool)elem.GetCurrentPropertyValue(AutomationElement.IsTransformPatternAvailableProperty)}\r\n");
                res.Append($"IsValuePatternAvailableProperty:\t{(bool)elem.GetCurrentPropertyValue(AutomationElement.IsValuePatternAvailableProperty)}\r\n");
                res.Append($"IsWindowPatternAvailableProperty:\t{(bool)elem.GetCurrentPropertyValue(AutomationElement.IsWindowPatternAvailableProperty)}\r\n");
            }
            catch (Exception ex)
            {
                res.Append($"Error: {ex.Message}");
            }

            return res.ToString();
        }

        /// <summary>
        /// show UIElement Information
        /// </summary>
        /// <param name="elem"></param>
        private void ShowUIElementInformation(AutomationElement elem)
        {
            txtElementInformation.Text = GetInspectResultFromUIElement(elem);

            if ((chkShowInTree.Checked) && (chkXPathRelative.Checked))
            {
                TreeNode chk = GetCheckedNode();
                AutomationElement curElem = (AutomationElement)chk.Tag;
                txtXPath.Text = GetXPathFromUIElement(elem, curElem, chkUseNameAttr.Checked, chkUseAutomationIdAttr.Checked);
            }
            else
            {
                txtXPath.Text = GetXPathFromRoot(elem, chkUseNameAttr.Checked, chkUseAutomationIdAttr.Checked);
            }
        }

        /// <summary>
        /// highlight (wrap yellow line) UIElement
        /// </summary>
        /// <param name="elem"></param>
        private void HighlightUIElement(AutomationElement elem)
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
                
                txtElementInformation.Text = string.Empty;
                txtXPath.Text = string.Empty;
            }
            tvElements.Focus();
        }

        private void chkXPathRelative_CheckedChanged(object sender, EventArgs e)
        {
            if (tvElements.SelectedNode != null)
            {
                ShowUIElementInformation((AutomationElement)tvElements.SelectedNode.Tag);
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
