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

namespace taskt.UI.Forms
{
    public partial class frmScriptVariables : ThemedForm
    {
        public List<Core.Script.ScriptVariable> scriptVariables { get; set; }
        TreeNode userVariableParentNode;
        private string leadingValue = "Default Value: ";
        private string emptyValue = "(no default value)";
        public Core.ApplicationSettings appSettings;

        static string UserVariableText = "My Task Variables";
        static string TasktVariableText = "Default Task Variables";

        #region Initialization and Form Load

        public frmScriptVariables()
        {
            InitializeComponent();
        }
        private void frmScriptVariables_Load(object sender, EventArgs e)
        {
           //initialize
            userVariableParentNode = InitializeNodes(UserVariableText, scriptVariables);
            InitializeNodes(TasktVariableText, Core.Common.GenerateSystemVariables());
        }

        // create variables TreeNode
        private TreeNode InitializeNodes(string parentName, List<Core.Script.ScriptVariable> variables)
        {
            //create a root node (parent)
            TreeNode parentNode = new TreeNode(parentName);

            //add each item to parent
            foreach (var item in variables)
            {
                AddUserVariableNode(parentNode, item.VariableName, (string)item.VariableValue);
            }

            tvScriptVariables.BeginUpdate();
            //add parent to treeview
            tvScriptVariables.Nodes.Add(parentNode);
            tvScriptVariables.Sort();
            ExpandUserVariableNode();
            tvScriptVariables.EndUpdate();

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
            //for (int i = 0; i < userVariableParentNode.Nodes.Count; i++)
            //{
            //    //get name and value
            //    var VariableName = userVariableParentNode.Nodes[i].Text;
            //    var VariableValue = userVariableParentNode.Nodes[i].Nodes[0].Text.Replace(leadingValue, "").Replace(emptyValue, "");

            //    //add to list
            //    scriptVariables.Add(new Core.Script.ScriptVariable() { VariableName = VariableName, VariableValue = VariableValue });
            //}
            foreach(TreeNode targetVariable in userVariableParentNode.Nodes)
            {
                var variableName = targetVariable.Text;
                var variableValue = targetVariable.Nodes[0].Text.Replace(leadingValue, "").Replace(emptyValue, "");
                scriptVariables.Add(new Core.Script.ScriptVariable() { VariableName = variableName, VariableValue = variableValue });
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

        #region Add, Edit, Remove Variables

        private void uiBtnNew_Click(object sender, EventArgs e)
        {
            ////create variable editing form
            //using (Supplement_Forms.frmAddVariable addVariableForm = new Supplement_Forms.frmAddVariable())
            //{
            //    addVariableForm.appSettings = this.appSettings;
            //    ExpandUserVariableNode();

            //    //validate if user added variable
            //    if (addVariableForm.ShowDialog() == DialogResult.OK)
            //    {
            //        string newVariable = addVariableForm.txtVariableName.Text.Trim();
            //        if (checkVariableNameIsValid(newVariable))
            //        {
            //            if (!isVariableExists(newVariable))
            //            {
            //                //add newly edited node
            //                AddUserVariableNode(userVariableParentNode, newVariable, addVariableForm.txtDefaultValue.Text);
            //            }
            //            else
            //            {
            //                //MessageBox.Show("'" + newVaiable + "' is already exists.", "Variable Error");
            //                using (var fm = new Forms.Supplemental.frmDialog("'" + newVariable + "' is already exists.", "Variable Error",Supplemental.frmDialog.DialogType.OkOnly, 0))
            //                {
            //                    fm.ShowDialog();
            //                }
            //            }
            //        }
            //        else
            //        {
            //            //MessageBox.Show("'" + newVaiable + "' contains bad character(ex. { } [ ] + - * /)." + Environment.NewLine + "Or equals reserved keyword.", "Variable Error");
            //            using (var fm = new Forms.Supplemental.frmDialog("'" + newVariable + "' contains bad character(ex. { } [ ] + - * /)." + Environment.NewLine + "Or equals reserved keyword.", "Variable Error", Supplemental.frmDialog.DialogType.OkOnly, 0))
            //            {
            //                fm.ShowDialog();
            //            }
            //        }
            //    }
            //}
            AddOrEditVaiableProcess(null);
        }

        private void AddOrEditVaiableProcess(TreeNode targetVariable = null)
        {
            Supplement_Forms.frmAddVariable addVariableForm;
            string variableName = "", variableValue = "";
            if (targetVariable == null)
            {
                addVariableForm = new Supplement_Forms.frmAddVariable(appSettings);
            }
            else
            {
                if (!GetVariableNameAndValue(targetVariable, out variableName, out variableValue))
                {
                    return;
                }
                addVariableForm = new Supplement_Forms.frmAddVariable(variableName, variableValue, appSettings);
            }

            tvScriptVariables.BeginUpdate();
            //create variable editing form
            using (addVariableForm)
            {
                //addVariableForm.appSettings = this.appSettings;
                //ExpandUserVariableNode();

                //validate if user added variable
                if (addVariableForm.ShowDialog() == DialogResult.OK)
                {
                    string newVariableName = addVariableForm.txtVariableName.Text.Trim();
                    // check variable name
                    if (checkVariableNameIsValid(newVariableName))
                    {
                        if (!isVariableExists(newVariableName))
                        {
                            // variable doesnt exists
                            if (addVariableForm.editMode == Supplement_Forms.frmAddVariable.frmAddVariablesEditMode.Edit)
                            {
                                targetVariable.Remove();
                            }
                            //add newly edited node
                            AddUserVariableNode(userVariableParentNode, newVariableName, addVariableForm.txtDefaultValue.Text);
                        }
                        else
                        {
                            // variable exists
                            if ((addVariableForm.editMode == Supplement_Forms.frmAddVariable.frmAddVariablesEditMode.Edit) && (variableName == newVariableName))
                            {
                                targetVariable.Remove();
                                //add newly edited node
                                AddUserVariableNode(userVariableParentNode, newVariableName, addVariableForm.txtDefaultValue.Text);
                            }
                            else
                            {
                                using (var fm = new Forms.Supplemental.frmDialog("'" + newVariableName + "' is already exists.", "Variable Error", Supplemental.frmDialog.DialogType.OkOnly, 0))
                                {
                                    fm.ShowDialog();
                                }
                            }
                        }
                        
                    }
                    else
                    {
                        using (var fm = new Forms.Supplemental.frmDialog("'" + newVariableName + "' contains bad character(ex. { } [ ] + - * /)." + Environment.NewLine + "Or equals reserved keyword.", "Variable Error", Supplemental.frmDialog.DialogType.OkOnly, 0))
                        {
                            fm.ShowDialog();
                        }
                    }
                }
            }
            tvScriptVariables.Sort();
            ExpandUserVariableNode();
            tvScriptVariables.EndUpdate();
        }
        private void EditSelectedVariableProcess()
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

            if (topNode.Text != UserVariableText)
            {
                return;
            }

            //string variableName, variableValue;
            TreeNode parentNode;
            if (tvScriptVariables.SelectedNode.Nodes.Count == 0)
            {
                parentNode = tvScriptVariables.SelectedNode.Parent;
            }
            else
            {
                parentNode = tvScriptVariables.SelectedNode;
            }
            AddOrEditVaiableProcess(parentNode);
        }
        private void RemoveSelectedVariableProcess()
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
        #endregion

        #region tvScriptVariables events
        private void tvScriptVariables_DoubleClick(object sender, EventArgs e)
        {
            ////handle double clicks outside
            //if (tvScriptVariables.SelectedNode == null)
            //{
            //    return;
            //}

            ////if parent was selected return
            //if (tvScriptVariables.SelectedNode.Parent == null)
            //{
            //    //user selected top parent
            //    return;
            //}

            ////top node check
            //var topNode = GetSelectedTopNode();

            //if (topNode.Text != "My Task Variables")
            //{
            //    return;
            //}

            ////string variableName, variableValue;
            //TreeNode parentNode;
            //if(tvScriptVariables.SelectedNode.Nodes.Count == 0)
            //{
            //    parentNode = tvScriptVariables.SelectedNode.Parent;
            //    //variableName = tvScriptVariables.SelectedNode.Parent.Text;
            //    //variableValue = tvScriptVariables.SelectedNode.Text.Replace(leadingValue, "").Replace(emptyValue, "");
            //}
            //else
            //{
            //    parentNode = tvScriptVariables.SelectedNode;
            //    //variableName = tvScriptVariables.SelectedNode.Text;
            //    //variableValue = tvScriptVariables.SelectedNode.Nodes[0].Text.Replace(leadingValue, "").Replace(emptyValue, "");
            //}
            ////variableName = parentNode.Text;
            ////variableValue = parentNode.Nodes[0].Text.Replace(leadingValue, "").Replace(emptyValue, "");

            //////create variable editing form
            ////using (Supplement_Forms.frmAddVariable addVariableForm = new Supplement_Forms.frmAddVariable(variableName, variableValue))
            ////{
            ////    addVariableForm.appSettings = this.appSettings;
            ////    ExpandUserVariableNode();

            ////    //validate if user added variable
            ////    if (addVariableForm.ShowDialog() == DialogResult.OK)
            ////    {
            ////        string newVariable = addVariableForm.txtVariableName.Text;
            ////        if (variableName == newVariable)
            ////        {
            ////            // variable name don't change

            ////            //remove parent
            ////            parentNode.Remove();

            ////            //add newly edited node
            ////            AddUserVariableNode(userVariableParentNode, newVariable, addVariableForm.txtDefaultValue.Text);
            ////        }
            ////        else
            ////        {
            ////            // variable name changed
            ////            if (checkVariableNameIsValid(newVariable))
            ////            {
            ////                if (!isVariableExists(newVariable))
            ////                {
            ////                    //remove parent
            ////                    parentNode.Remove();

            ////                    //add newly edited node
            ////                    AddUserVariableNode(userVariableParentNode, newVariable, addVariableForm.txtDefaultValue.Text);
            ////                }
            ////                else
            ////                {
            ////                    //MessageBox.Show("'" + newVariable + "' is already exists.", "Variable Error");
            ////                    using (var fm = new Forms.Supplemental.frmDialog("'" + newVariable + "' is already exists.", "Variable Error", Supplemental.frmDialog.DialogType.OkOnly, 0))
            ////                    {
            ////                        fm.ShowDialog();
            ////                    }
            ////                }
            ////            }
            ////            else
            ////            {
            ////                //MessageBox.Show("'" + newVariable + "' contains bad character(ex. { } [ ] + - * /)." + Environment.NewLine + "Or equals reserved keyword.", "Variable Error");
            ////                using (var fm = new Forms.Supplemental.frmDialog("'" + newVariable + "' contains bad character(ex. { } [ ] + - * /)." + Environment.NewLine + "Or equals reserved keyword.", "Variable Error", Supplemental.frmDialog.DialogType.OkOnly, 0))
            ////                {
            ////                    fm.ShowDialog();
            ////                }
            ////            }
            ////        }
            ////    }
            ////}
            //AddOrEditVaiableProcess(parentNode);
            EditSelectedVariableProcess();
        }
        private void tvScriptVariables_KeyDown(object sender, KeyEventArgs e)
        {
            //handling outside
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

            if (topNode.Text != UserVariableText)
            {
                return;
            }

            //if user selected delete
            if (e.KeyCode == Keys.Delete)
            {
                ////determine which node is the parent
                //TreeNode parentNode;
                //if (tvScriptVariables.SelectedNode.Nodes.Count == 0)
                //{
                //    parentNode = tvScriptVariables.SelectedNode.Parent;
                //}
                //else
                //{
                //    parentNode = tvScriptVariables.SelectedNode;
                //}

                ////remove parent node
                //parentNode.Remove();
                RemoveSelectedVariableProcess();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                //tvScriptVariables_DoubleClick(null, null);
                EditSelectedVariableProcess();
            }
        }
        private void tvScriptVariables_MouseClick(object sender, MouseEventArgs e)
        {
            if (tvScriptVariables.SelectedNode == null)
            {
                return;
            }
            TreeNode topNode = GetSelectedTopNode();
            if ((topNode.Text == UserVariableText) && (e.Button == MouseButtons.Right))
            {
                //if (tvScriptVariables.Parent != null)
                //{
                //    addToolStripMenuItem.Visible = false;
                //}
                if (tvScriptVariables.SelectedNode.Text == UserVariableText)
                {
                    addToolStripMenuItem.Visible = true;
                    editToolStripMenuItem.Visible = false;
                    removeToolStripMenuItem.Visible = false;
                }
                else
                {
                    addToolStripMenuItem.Visible = false;
                    editToolStripMenuItem.Visible = true;
                    removeToolStripMenuItem.Visible = true;
                }
                editVariableContextMenuStrip.Show(Cursor.Position);
            }
        }
        private void tvScriptVariables_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node == null)
            {
                return;
            }
            if (e.Button == MouseButtons.Right)
            {
                tvScriptVariables.SelectedNode = e.Node;
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
        private bool GetVariableNameAndValue(TreeNode tn, out string variableName, out string variableValue)
        {
            if (tn.Nodes.Count > 0)
            {
                variableName = tn.Text;
                variableValue = tn.Nodes[0].Text.Replace(leadingValue, "").Replace(emptyValue, "");
                return true;
            }
            else
            {
                variableName = "";
                variableValue = "";
                return false;
            }
        }
        #endregion

        #region editVariableToolStripMenu events
        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddOrEditVaiableProcess(null);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditSelectedVariableProcess();
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveSelectedVariableProcess();
        }
        #endregion

        #region variables method
        private void AddUserVariableNode(TreeNode parentNode, string VariableName, string variableText)
        {
            //add new node and sort
            var childNode = new TreeNode(VariableName);

            if (variableText == string.Empty)
            {
                variableText = emptyValue;
            }

            childNode.Nodes.Add(leadingValue + variableText);
            parentNode.Nodes.Add(childNode);
            //tvScriptVariables.Sort();
            //ExpandUserVariableNode();
        }

        private void ExpandUserVariableNode()
        {
            if (userVariableParentNode != null)
            {
                userVariableParentNode.ExpandAll();
            }
        }

        private bool isVariableExists(string variableName)
        {
            foreach(TreeNode parentNode in tvScriptVariables.Nodes)
            {
                foreach(TreeNode child in parentNode.Nodes)
                {
                    if (variableName == child.Text)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool checkVariableNameIsValid(string variableName)
        {
            //var disallowChars = appSettings.EngineSettings.DisallowVariableCharList();
            //foreach(string k in disallowChars)
            //{
            //    if (variableName.Contains(k))
            //    {
            //        return false;
            //    }
            //}

            //var keyNames = appSettings.EngineSettings.KeyNameList();
            //var upperVarName = variableName.ToUpper();
            //foreach(string k in keyNames)
            //{
            //    if (upperVarName == k)
            //    {
            //        return false;
            //    }
            //}
            //return true;
            return appSettings.EngineSettings.isValidVariableName(variableName);
        }
        #endregion

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(this.Theme.CreateGradient(panel2.ClientRectangle), panel2.ClientRectangle);
        }

        private void frmScriptVariables_FormClosed(object sender, FormClosedEventArgs e)
        {
            // release
            userVariableParentNode.Remove();
            userVariableParentNode = null;
        }

        #region form events
        private void frmScriptVariables_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && (e.KeyCode == Keys.N))
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                AddOrEditVaiableProcess(null);
            }
        }
        #endregion


    }
}