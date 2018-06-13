//Copyright (c) 2018 Jason Bayldon
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

namespace taskt.UI.Forms
{
    public partial class frmScriptVariables : UIForm
    {
        public List<Core.Script.ScriptVariable> scriptVariables { get; set; }
        TreeNode userVariableParentNode;
        public string leadingValue = "Default Value: ";
        public string emptyValue = "(no default value)";
        #region Initialization and Form Load

        public frmScriptVariables()
        {
            InitializeComponent();
        }
        private void frmScriptVariables_Load(object sender, EventArgs e)
        {
           //initialize
            userVariableParentNode = InitializeNodes("My Task Variables", scriptVariables);
            InitializeNodes("Default Task Variables", Core.Common.GenerateSystemVariables());
        }

        private TreeNode InitializeNodes(string parentName, List<Core.Script.ScriptVariable> variables)
        {

            //create a root node (parent)
            TreeNode parentNode = new TreeNode(parentName);

            //add each item to parent
            foreach (var item in variables)
            {
                AddUserVariableNode(parentNode, item.variableName, (string)item.variableValue);
            }

            //add parent to treeview
            tvScriptVariables.Nodes.Add(parentNode);

            //return parent and utilize if needed
            return parentNode;
        }

        #endregion

        #region Add/Cancel Buttons
        private void uiBtnOK_Click(object sender, EventArgs e)
        {

            //remove all variables
            scriptVariables.Clear();

            //loop each variable and add
            for (int i = 0; i < userVariableParentNode.Nodes.Count; i++)
            {
                //get name and value
                var variableName = userVariableParentNode.Nodes[i].Text;
                var variableValue = userVariableParentNode.Nodes[i].Nodes[0].Text.Replace(leadingValue, "").Replace(emptyValue, "");

                //add to list
                scriptVariables.Add(new Core.Script.ScriptVariable() { variableName = variableName, variableValue = variableValue });
        

            }

            //return success result
            this.DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            //cancel and close
            this.DialogResult = DialogResult.Cancel;
        }

        #endregion

        #region Add/Edit Variables

        private void uiBtnNew_Click(object sender, EventArgs e)
        {
            //create variable editing form
            Supplement_Forms.frmAddVariable addVariableForm = new Supplement_Forms.frmAddVariable();
            ExpandUserVariableNode();

            //validate if user added variable
            if (addVariableForm.ShowDialog() == DialogResult.OK)
            {
                //add newly edited node
                AddUserVariableNode(userVariableParentNode, addVariableForm.txtVariableName.Text, addVariableForm.txtDefaultValue.Text);
            }


        }

        

        private void tvScriptVariables_DoubleClick(object sender, EventArgs e)
        {

            //handle double clicks outside
            if (tvScriptVariables.SelectedNode == null)
            {
                return;
            }

            //if parent was selected return
            if (tvScriptVariables.SelectedNode.Parent == null)
            {
                //user selected top parent
                return;
            }

            //top node check
            var topNode = GetSelectedTopNode();

            if (topNode.Text != "My Task Variables")
            {
                return;
            }

            string variableName, variableValue;
            TreeNode parentNode;
            if(tvScriptVariables.SelectedNode.Nodes.Count == 0)
            {
                parentNode = tvScriptVariables.SelectedNode.Parent;
                variableName = tvScriptVariables.SelectedNode.Parent.Text;
                variableValue = tvScriptVariables.SelectedNode.Text.Replace(leadingValue, "").Replace(emptyValue, "");
            }
            else
            {
                parentNode = tvScriptVariables.SelectedNode;
                variableName = tvScriptVariables.SelectedNode.Text;
                variableValue = tvScriptVariables.SelectedNode.Nodes[0].Text.Replace(leadingValue, "").Replace(emptyValue, "");
            }

            //create variable editing form
            Supplement_Forms.frmAddVariable addVariableForm = new Supplement_Forms.frmAddVariable(variableName, variableValue);
            ExpandUserVariableNode();

            //validate if user added variable
            if (addVariableForm.ShowDialog() == DialogResult.OK)
            {
                //remove parent
                parentNode.Remove();

                //add newly edited node
                AddUserVariableNode(userVariableParentNode, addVariableForm.txtVariableName.Text, addVariableForm.txtDefaultValue.Text);
            }

        }
        private void AddUserVariableNode(TreeNode parentNode, string variableName, string variableText)
        {
            //add new node and sort
            var childNode = new TreeNode(variableName);

            if (variableText == string.Empty)
            {
                variableText = emptyValue;
            }

            childNode.Nodes.Add(leadingValue + variableText);
            parentNode.Nodes.Add(childNode);
            tvScriptVariables.Sort();
            ExpandUserVariableNode();

        }

        private void ExpandUserVariableNode()
        {
            if (userVariableParentNode != null)
            {
                userVariableParentNode.ExpandAll();
            }
        }


 
        private void tvScriptVariables_KeyDown(object sender, KeyEventArgs e)
        {
            //if parent was selected return
            if (tvScriptVariables.SelectedNode.Parent == null)
            {
                //user selected top parent
                return;
            }

            //top node check
            var topNode = GetSelectedTopNode();

            if (topNode.Text != "My Task Variables")
            {
                return;
            }

            //if user selected delete
            if (e.KeyCode == Keys.Delete)
            {
                //determine which node is the parent
                TreeNode parentNode;
                if (tvScriptVariables.SelectedNode.Nodes.Count == 0)
                {
                    parentNode = tvScriptVariables.SelectedNode.Parent;
                }
                else
                {
                    parentNode = tvScriptVariables.SelectedNode;
                }

                //remove parent node
                parentNode.Remove();
            }
        }

        private TreeNode GetSelectedTopNode()
        {
            TreeNode node = tvScriptVariables.SelectedNode;
            while (node.Parent != null)
            {
                node = node.Parent;
            }
            return node;
        }
        #endregion
    }
}