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
        public List<Core.Script.ScriptVariable> scriptVariables { get; }
        TreeNode bufferedUserVariableParentNode = new TreeNode();
        TreeNode bufferedSystemVariableParentNode = new TreeNode();

        private string leadingValue = "Default Value: ";
        private string emptyValue = "(no default value)";
        public Core.ApplicationSettings appSettings;

        static string User_Variables_Text = "My Task Variables";
        static string System_Variables_Text = "Default Task Variables";

        #region Initialization and Form Load

        public frmScriptVariables(List<Core.Script.ScriptVariable> variables, Core.ApplicationSettings appSettings)
        {
            InitializeComponent();
            this.scriptVariables = variables;
            this.appSettings = appSettings;
        }
        private void frmScriptVariables_Load(object sender, EventArgs e)
        {
            ImageList tvVariablesImageList = new ImageList();
            tvVariablesImageList.ImageSize = new Size(16, 16);
            tvVariablesImageList.Images.Add(Properties.Resources.command_group);
            tvVariablesImageList.Images.Add(Properties.Resources.action_bar_variable);
            tvVariablesImageList.Images.Add(Properties.Resources.taskt_variable_helper);
            tvScriptVariables.ImageList = tvVariablesImageList;

            ////initialize
            // bufferedUserVariableParentNode = InitializeNodes(User_Variable_Text, scriptVariables);
            // InitializeNodes(Taskt_Variable_Text, Core.Common.GenerateSystemVariables());
            InitializeNodes();
        }

        // create variables TreeNode
        private void InitializeNodes()
        {
            ////create a root node (parent)
            //TreeNode parentNode = new TreeNode(parentName);

            ////add each item to parent
            //foreach (var item in variables)
            //{
            //    AddUserVariableNode(parentNode, item.VariableName, (string)item.VariableValue);
            //}

            //tvScriptVariables.BeginUpdate();
            ////add parent to treeview
            //tvScriptVariables.Nodes.Add(parentNode);
            //tvScriptVariables.Sort();
            //ExpandUserVariableNode();
            //tvScriptVariables.EndUpdate();

            ////return parent and utilize if needed
            //return parentNode;

            // System Variables
            bufferedSystemVariableParentNode.Nodes.Clear();
            bufferedSystemVariableParentNode.Text = System_Variables_Text;
            List<Core.Script.ScriptVariable> systemVars = Core.Common.GenerateSystemVariables();
            foreach(var item in systemVars)
            {
                AddUserVariableNode(bufferedSystemVariableParentNode, item.VariableName, (string)item.VariableValue);
            }

            // User Variables
            bufferedUserVariableParentNode.Nodes.Clear();
            bufferedUserVariableParentNode.Text = User_Variables_Text;
            foreach(var item in scriptVariables)
            {
                AddUserVariableNode(bufferedUserVariableParentNode, item.VariableName, (string)item.VariableValue);
            }

            //tvScriptVariables.BeginUpdate();
            //tvScriptVariables.Nodes.Clear();
            //tvScriptVariables.Nodes.Add((TreeNode)bufferedSystemVariableParentNode.Clone());
            //tvScriptVariables.Nodes.Add((TreeNode)bufferedUserVariableParentNode.Clone());
            //tvScriptVariables.Nodes[1].ExpandAll();
            //tvScriptVariables.Sort();
            //tvScriptVariables.EndUpdate();
            showAllVariables();
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
            foreach(TreeNode targetVariable in bufferedUserVariableParentNode.Nodes)
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
                    string newVariableName = addVariableForm.VariableName.Trim();
                    string newVariableValue = addVariableForm.VariableValue.Trim();
                    // check variable name
                    if (checkVariableNameIsValid(newVariableName))
                    {
                        if (!isVariableExists(newVariableName))
                        {
                            // variable doesnt exists
                            if (addVariableForm.editMode == Supplement_Forms.frmAddVariable.frmAddVariablesEditMode.Edit)
                            {
                                //targetVariable.Remove();
                                RemoveUserVariableNode(bufferedUserVariableParentNode, variableName);
                                RemoveUserVariableNode(tvScriptVariables.Nodes[1], variableName);
                            }
                            //add newly edited node
                            AddUserVariableNode(bufferedUserVariableParentNode, newVariableName, newVariableValue);
                            AddUserVariableNode(tvScriptVariables.Nodes[1], newVariableName, newVariableValue);
                        }
                        else
                        {
                            // variable exists
                            if ((addVariableForm.editMode == Supplement_Forms.frmAddVariable.frmAddVariablesEditMode.Edit) && (variableName == newVariableName))
                            {
                                //targetVariable.Remove();
                                RemoveUserVariableNode(bufferedUserVariableParentNode, variableName);
                                RemoveUserVariableNode(tvScriptVariables.Nodes[1], variableName);

                                //add newly edited node
                                AddUserVariableNode(bufferedUserVariableParentNode, newVariableName, newVariableValue);
                                AddUserVariableNode(tvScriptVariables.Nodes[1], newVariableName, newVariableValue);
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
            tvScriptVariables.Nodes[1].ExpandAll();
            tvScriptVariables.Sort();
            //ExpandUserVariableNode();
            tvScriptVariables.EndUpdate();
        }
        private void AddVariableProcess(string variableName)
        {
            variableName = variableName.Trim();
            if (variableName.Length == 0)
            {
                using (var fm = new Forms.Supplemental.frmDialog("Variable Name is empty.", "Variable Error", Supplemental.frmDialog.DialogType.OkOnly, 0))
                {
                    fm.ShowDialog();
                }
                return;
            }
            if (checkVariableNameIsValid(variableName))
            {
                if (!isVariableExists(variableName))
                {
                    //add newly edited node
                    tvScriptVariables.BeginUpdate();
                    AddUserVariableNode(bufferedUserVariableParentNode, variableName, "");
                    AddUserVariableNode(tvScriptVariables.Nodes[1], variableName, "");
                    tvScriptVariables.Nodes[1].ExpandAll();
                    tvScriptVariables.Sort();
                    tvScriptVariables.EndUpdate();
                }
                else
                {
                    using (var fm = new Forms.Supplemental.frmDialog("'" + variableName + "' is already exists.", "Variable Error", Supplemental.frmDialog.DialogType.OkOnly, 0))
                    {
                        fm.ShowDialog();
                    }
                }
            }
            else
            {
                using (var fm = new Forms.Supplemental.frmDialog("'" + variableName + "' contains bad character(ex. { } [ ] + - * /)." + Environment.NewLine + "Or equals reserved keyword.", "Variable Error", Supplemental.frmDialog.DialogType.OkOnly, 0))
                {
                    fm.ShowDialog();
                }
            }
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

            if (topNode.Text != User_Variables_Text)
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
            //parentNode.Remove();
            RemoveUserVariableNode(tvScriptVariables.Nodes[1], parentNode.Text);
            RemoveUserVariableNode(bufferedUserVariableParentNode, parentNode.Text);
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

            if (topNode.Text != User_Variables_Text)
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
            if (e.Button == MouseButtons.Right)
            {
                //if (tvScriptVariables.Parent != null)
                //{
                //    addToolStripMenuItem.Visible = false;
                //}
                if (topNode.Text == User_Variables_Text)
                {
                    if (tvScriptVariables.SelectedNode.Text == User_Variables_Text)
                    {
                        if (tvScriptVariables.SelectedNode.IsExpanded)
                        {
                            collapseToolStripMenuItem.Visible = true;
                            expandToolStripMenuItem.Visible = false;
                        }
                        else
                        {
                            collapseToolStripMenuItem.Visible = false;
                            expandToolStripMenuItem.Visible = true;
                        }
                        addToolStripMenuItem.Visible = true;
                        rootVariableContestMenuStrip.Show(Cursor.Position);
                    }
                    else
                    {
                        editVariableContextMenuStrip.Show(Cursor.Position);
                    }
                }
                else
                {
                    if (tvScriptVariables.SelectedNode.IsExpanded)
                    {
                        collapseToolStripMenuItem.Visible = true;
                        expandToolStripMenuItem.Visible = false;
                    }
                    else
                    {
                        collapseToolStripMenuItem.Visible = false;
                        expandToolStripMenuItem.Visible = true;
                    }
                    addToolStripMenuItem.Visible = false;
                    rootVariableContestMenuStrip.Show(Cursor.Position);
                }
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
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditSelectedVariableProcess();
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveSelectedVariableProcess();
        }
        private void clearFilterEditVariableContextMenuStrip_Click(object sender, EventArgs e)
        {
            showAllVariables();
        }
        #endregion

        #region rootVariableToolStripMenu events
        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddOrEditVaiableProcess(null);
        }
        private void expandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tvScriptVariables.BeginUpdate();
            tvScriptVariables.SelectedNode.ExpandAll();
            tvScriptVariables.EndUpdate();
        }

        private void collapseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tvScriptVariables.BeginUpdate();
            tvScriptVariables.SelectedNode.Collapse();
            tvScriptVariables.EndUpdate();
        }
        private void clearFilterRootVariableContextMenuStrip_Click(object sender, EventArgs e)
        {
            showAllVariables();
        }
        #endregion

        #region variables method
        private void AddUserVariableNode(TreeNode parentNode, string VariableName, string variableText)
        {
            //add new node and sort
            var childNode = new TreeNode(VariableName, 1, 1);

            if (variableText == string.Empty)
            {
                variableText = emptyValue;
            }

            childNode.Nodes.Add(new TreeNode(leadingValue + variableText, 2, 2));
            parentNode.Nodes.Add(childNode);
            //tvScriptVariables.Sort();
            //ExpandUserVariableNode();
        }
        private void RemoveUserVariableNode(TreeNode parentNode, string VariableName)
        {
            foreach (TreeNode item in parentNode.Nodes)
            {
                if (item.Text == VariableName)
                {
                    item.Remove();
                    break;
                }
            }
        }

        private void ExpandUserVariableNode()
        {
            //if (bufferedUserVariableParentNode != null)
            //{
            //    bufferedUserVariableParentNode.ExpandAll();
            //}
            tvScriptVariables.Nodes[1].ExpandAll();
        }

        private bool isVariableExists(string variableName)
        {
            //foreach(TreeNode parentNode in tvScriptVariables.Nodes)
            //{
            //    foreach(TreeNode child in parentNode.Nodes)
            //    {
            //        if (variableName == child.Text)
            //        {
            //            return true;
            //        }
            //    }
            //}
            foreach(TreeNode item in bufferedSystemVariableParentNode.Nodes)
            {
                if (item.Text == variableName)
                {
                    return true;
                }
            }
            foreach(TreeNode item in bufferedUserVariableParentNode.Nodes)
            {
                if (item.Text == variableName)
                {
                    return true;
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
            bufferedUserVariableParentNode.Remove();
            bufferedUserVariableParentNode = null;
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

        #region filter variables
        private void picSearch_Click(object sender, EventArgs e)
        {
            BeginFilterVariablesProcess();
        }
        private void picClear_Click(object sender, EventArgs e)
        {
            //txtSearchBox.Text = "";
            showAllVariables();
        }
        private void picAdd_Click(object sender, EventArgs e)
        {
            AddVariableProcess(txtSearchBox.Text);
        }
        private void txtSearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                BeginFilterVariablesProcess();
            }
        }
        private void BeginFilterVariablesProcess()
        {
            string keyword = txtSearchBox.Text.ToLower().Trim();
            if (keyword.Length > 0)
            {
                filterVariables(keyword);
            }
            else
            {
                showAllVariables();
            }
        }

        private void filterVariables(string keyword)
        {
            string variableName = "", variableValue = "";

            // System variables
            TreeNode systemVars = new TreeNode();
            systemVars.Text = System_Variables_Text;
            foreach(TreeNode item in bufferedSystemVariableParentNode.Nodes)
            {
                if (item.Text.ToLower().Contains(keyword))
                {
                    GetVariableNameAndValue(item, out variableName, out variableValue);
                    AddUserVariableNode(systemVars, variableName, variableValue);
                }
            }

            // User variables
            TreeNode userVars = new TreeNode();
            userVars.Text = User_Variables_Text;
            foreach(TreeNode item in bufferedUserVariableParentNode.Nodes)
            {
                if (item.Text.ToLower().Contains(keyword))
                {
                    GetVariableNameAndValue(item, out variableName, out variableValue);
                    AddUserVariableNode(userVars, variableName, variableValue);
                }
            }

            tvScriptVariables.BeginUpdate();
            tvScriptVariables.Nodes.Clear();
            tvScriptVariables.Nodes.Add(systemVars);
            tvScriptVariables.Nodes.Add(userVars);
            //tvScriptVariables.ExpandAll();
            tvScriptVariables.Nodes[1].ExpandAll();
            tvScriptVariables.EndUpdate();

            clearFilterEditVariableContextMenuStrip.Enabled = true;
            clearFilterRootVariableContextMenuStrip.Enabled = true;
        }
        private void showAllVariables()
        {
            txtSearchBox.Text = "";

            tvScriptVariables.BeginUpdate();
            tvScriptVariables.Nodes.Clear();
            tvScriptVariables.Nodes.Add((TreeNode)bufferedSystemVariableParentNode.Clone());
            tvScriptVariables.Nodes.Add((TreeNode)bufferedUserVariableParentNode.Clone());
            tvScriptVariables.Nodes[1].ExpandAll();
            tvScriptVariables.Sort();
            tvScriptVariables.EndUpdate();

            clearFilterEditVariableContextMenuStrip.Enabled = false;
            clearFilterRootVariableContextMenuStrip.Enabled = false;
        }
        #endregion

    }
}