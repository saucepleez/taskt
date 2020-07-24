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
using System.Windows.Forms;
using taskt.Core.Script;
using taskt.Core;
using taskt.UI.Forms.Supplement_Forms;

namespace taskt.UI.Forms
{
    public partial class frmScriptElements : ThemedForm
    {
        public List<ScriptElement> ScriptElements { get; set; }
        public string ScriptName { get; set; }
        private TreeNode _userElementParentNode;
        private string _leadingValue = "Default Value: ";
        private string _emptyValue = "(no default value)";
        private string _leadingType = "Default Type: ";
        private string _emptyType = "(no default type)";

        #region Initialization and Form Load
        public frmScriptElements()
        {
            InitializeComponent();
        }

        private void frmScriptElements_Load(object sender, EventArgs e)
        {
           //initialize
            _userElementParentNode = InitializeNodes("My Task Elements", ScriptElements);
            ExpandUserElementNode();
            lblMainLogo.Text = ScriptName + " elements";
        }

        private TreeNode InitializeNodes(string parentName, List<ScriptElement> elements)
        {
            //create a root node (parent)
            TreeNode parentNode = new TreeNode(parentName);

            //add each item to parent
            foreach (var item in elements)
            {
                AddUserElementNode(parentNode, item.ElementName, item.ElementType, item.ElementValue);
            }

            //add parent to treeview
            tvScriptElements.Nodes.Add(parentNode);

            //return parent and utilize if needed
            return parentNode;
        }

        #endregion

        #region Add/Cancel Buttons
        private void uiBtnOK_Click(object sender, EventArgs e)
        {
            //remove all elements
            ScriptElements.Clear();

            //loop each element and add
            for (int i = 0; i < _userElementParentNode.Nodes.Count; i++)
            {
                //get name and value
                var elementName = _userElementParentNode.Nodes[i].Text;
                var elementType = (ScriptElementType)Enum.Parse(typeof(ScriptElementType), _userElementParentNode.Nodes[i].Nodes[0].Text
                                                         .Replace(_leadingType, "").Replace(_emptyType, "").Replace(" ", ""));
                var elementValue = _userElementParentNode.Nodes[i].Nodes[1].Text.Replace(_leadingValue, "").Replace(_emptyValue, "");

                //add to list
                ScriptElements.Add(new ScriptElement() { ElementName = elementName, ElementType = elementType, ElementValue = elementValue });
            }

            //return success result
            DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            //cancel and close
            DialogResult = DialogResult.Cancel;
        }
        #endregion

        #region Add/Edit Elements
        private void uiBtnNew_Click(object sender, EventArgs e)
        {
            //create element editing form
            frmAddElement addElementForm = new frmAddElement();
            addElementForm.ScriptElements = ScriptElements;

            ExpandUserElementNode();

            //validate if user added element
            if (addElementForm.ShowDialog() == DialogResult.OK)
            {
                ScriptElementType addElementFormType = (ScriptElementType)Enum.Parse(typeof(ScriptElementType),
                                                        addElementForm.cbxElementType.SelectedItem.ToString().Replace(" ", ""));

                //add newly edited node
                AddUserElementNode(_userElementParentNode, addElementForm.txtElementName.Text, addElementFormType, 
                                   addElementForm.txtDefaultValue.Text);
            }
        }

        private void tvScriptElements_DoubleClick(object sender, EventArgs e)
        {
            //handle double clicks outside
            if (tvScriptElements.SelectedNode == null)
            {
                return;
            }

            //if parent was selected return
            if (tvScriptElements.SelectedNode.Parent == null)
            {
                //user selected top parent
                return;
            }

            //top node check
            var topNode = GetSelectedTopNode();

            if (topNode.Text != "My Task Elements")
            {
                return;
            }

            string elementName, elementValue;
            ScriptElementType elementType;
            TreeNode parentNode;

            if(tvScriptElements.SelectedNode.Nodes.Count == 0)
            {
                parentNode = tvScriptElements.SelectedNode.Parent;
                elementName = tvScriptElements.SelectedNode.Parent.Text;
                elementType = (ScriptElementType)Enum.Parse(typeof(ScriptElementType), tvScriptElements.SelectedNode.Parent.Nodes[0].Text
                                                     .Replace(_leadingType, "").Replace(_emptyType, "").Replace(" ", ""));
                elementValue = tvScriptElements.SelectedNode.Parent.Nodes[1].Text.Replace(_leadingValue, "").Replace(_emptyValue, "");
            }
            else
            {
                parentNode = tvScriptElements.SelectedNode;
                elementName = tvScriptElements.SelectedNode.Text;
                elementType = (ScriptElementType)Enum.Parse(typeof(ScriptElementType), tvScriptElements.SelectedNode.Nodes[0].Text
                                                     .Replace(_leadingType, "").Replace(_emptyType, "").Replace(" ", ""));
                elementValue = tvScriptElements.SelectedNode.Nodes[1].Text.Replace(_leadingValue, "").Replace(_emptyValue, "");
            }

            //create element editing form
            frmAddElement addElementForm = new frmAddElement(elementName, elementType, elementValue);
            addElementForm.ScriptElements = ScriptElements;

            ExpandUserElementNode();

            //validate if user added element
            if (addElementForm.ShowDialog() == DialogResult.OK)
            {
                //remove parent
                parentNode.Remove();

                ScriptElementType addElementFormType = (ScriptElementType)Enum.Parse(typeof(ScriptElementType), 
                                                        addElementForm.cbxElementType.SelectedItem.ToString().Replace(" ", ""));

                //add newly edited node
                AddUserElementNode(_userElementParentNode, addElementForm.txtElementName.Text, addElementFormType, 
                                   addElementForm.txtDefaultValue.Text);
            }
        }

        private void AddUserElementNode(TreeNode parentNode, string elementName, ScriptElementType elementType, string elementText)
        {
            //add new node and sort
            var childNode = new TreeNode(elementName);

            if (elementText == string.Empty)
            {
                elementText = _emptyValue;
            }

            TreeNode elementTypeNode = new TreeNode("TypeNode");
            elementTypeNode.Text = _leadingType + elementType.Description();
            TreeNode elementValueNode = new TreeNode("ValueNode");
            elementValueNode.Text = _leadingValue + elementText;

            childNode.Nodes.Add(elementTypeNode);
            childNode.Nodes.Add(elementValueNode);

            parentNode.Nodes.Add(childNode);
            tvScriptElements.Sort();
            ExpandUserElementNode();
        }

        private void ExpandUserElementNode()
        {
            if (_userElementParentNode != null)
            {
                _userElementParentNode.ExpandAll();
            }
        }

        private void tvScriptElements_KeyDown(object sender, KeyEventArgs e)
        {
            //handling outside
            if (tvScriptElements.SelectedNode == null)
            {
                return;
            }

            //if parent was selected return
            if (tvScriptElements.SelectedNode.Parent == null)
            {
                //user selected top parent
                return;
            }

            //top node check
            var topNode = GetSelectedTopNode();

            if (topNode.Text != "My Task Elements")
            {
                return;
            }

            //if user selected delete
            if (e.KeyCode == Keys.Delete)
            {
                //determine which node is the parent
                TreeNode parentNode;
                if (tvScriptElements.SelectedNode.Nodes.Count == 0)
                {
                    parentNode = tvScriptElements.SelectedNode.Parent;
                }
                else
                {
                    parentNode = tvScriptElements.SelectedNode;
                }

                //remove parent node
                parentNode.Remove();
            }
        }

        private TreeNode GetSelectedTopNode()
        {
            TreeNode node = tvScriptElements.SelectedNode;
            while (node.Parent != null)
            {
                node = node.Parent;
            }
            return node;
        }
        #endregion

        private void pnlBottom_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Theme.CreateGradient(pnlBottom.ClientRectangle), pnlBottom.ClientRectangle);
        }
    }
}