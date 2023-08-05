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
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Commands;

namespace taskt.UI.Forms
{
    public partial class frmCommandEditor : ThemedForm
    {
        //list of available commands
        List<AutomationCommand> commandList = new List<AutomationCommand>();
        //list of variables, assigned from frmScriptBuilder
        public List<Core.Script.ScriptVariable> scriptVariables;
        //reference to currently selected command
        public ScriptCommand selectedCommand;
        //reference to original command
        public ScriptCommand originalCommand;
        //assigned by frmScriptBuilder to restrict inputs for editing existing commands
        public CreationMode creationMode;
        //startup command, assigned from frmScriptBuilder
        public string defaultStartupCommand;
        //editing command, assigned from frmScriptBuilder when editing a command
        public ScriptCommand editingCommand;
        //track existing commands for visibility
        public List<ScriptCommand> configuredCommands;
        // taskt setting
        public Core.ApplicationSettings appSettings;
        // instance counter
        public Core.InstanceCounter instanceList;

        // command tree
        private TreeNode[] treeAllCommands;
        private ImageList treeAllCommandsImage;

        public enum CreationMode
        {
            Add,
            Edit
        }

        #region constructors
        public frmCommandEditor(List<AutomationCommand> commands, List<ScriptCommand> existingCommands)
        {
            InitializeComponent();
            commandList = commands;
            configuredCommands = existingCommands;

            treeAllCommands = null;
            treeAllCommandsImage = null;

            cboSelectedCommand.Enabled = false;
        }

        public frmCommandEditor(List<AutomationCommand> commands, List<ScriptCommand> existingCommands, TreeNode[] treeCommands, ImageList treeCommandImage)
        {
            InitializeComponent();
            commandList = commands;
            configuredCommands = existingCommands;
            this.treeAllCommands = treeCommands;
            this.treeAllCommandsImage = treeCommandImage;
        }
        #endregion

        #region Form Events

        //handle events for the form
        private void frmNewCommand_Load(object sender, EventArgs e)
        {
            //declare loaded editor
            CommandControls.CurrentEditor = this;

            //order list
            commandList = commandList.OrderBy(itm => itm.FullName).ToList();

            //set command list
            cboSelectedCommand.DataSource = commandList;

            //Set DisplayMember to track DisplayValue from the class
            cboSelectedCommand.DisplayMember = "FullName";

            if ((creationMode == CreationMode.Add) && (defaultStartupCommand != null) && (commandList.Where(x => x.FullName == defaultStartupCommand).Count() > 0))
            {
                cboSelectedCommand.SelectedIndex = cboSelectedCommand.FindStringExact(defaultStartupCommand);
            }
            else if (creationMode == CreationMode.Edit)
            {
                // var requiredCommand = commandList.Where(x => x.FullName.Contains(defaultStartupCommand)).FirstOrDefault(); //&& x.CommandClass.Name == originalCommand.CommandName).FirstOrDefault();

                var requiredCommand = commandList.Where(x => x.Command.ToString() == editingCommand.ToString()).FirstOrDefault();

                if (requiredCommand == null)
                {
                    MessageBox.Show("Command was not found! " + defaultStartupCommand);
                }
                else
                {
                    cboSelectedCommand.SelectedIndex = cboSelectedCommand.FindStringExact(requiredCommand.FullName);
                }
            }
            else
            {
                cboSelectedCommand.SelectedIndex = 0;
            }

            //force commit event to populate the flow layout
            cboSelectedCommand_SelectionChangeCommitted(null, null);

            //apply original variables if command is being updated
            if (originalCommand != null)
            {
                //copy original properties
                CopyPropertiesTo(originalCommand, selectedCommand);

                Action<Control> readValueFunc = new Action<Control>(c =>
                {
                    foreach (Binding b in c.DataBindings)
                    {
                        b.ReadValue();
                    }

                    //helper for box
                    if (c is UIPictureBox pic)
                    {
                        //var typedControl = (UIPictureBox)c;

                        var cmd = (ImageRecognitionCommand)selectedCommand;

                        if (!string.IsNullOrEmpty(cmd.v_ImageCapture))
                        {
                            pic.Image = Core.Common.Base64ToImage(cmd.v_ImageCapture);
                        }
                    }
                });

                //update bindings
                foreach (Control ctrl in flw_InputVariables.Controls)
                {
                    readValueFunc(ctrl);

                    if (ctrl is FlowLayoutPanel flp)
                    {
                        foreach(Control c in flp.Controls)
                        {
                            readValueFunc(c);
                        }
                    }
                }


                //handle selection change events
            }

            //gracefully handle post initialization setups (drop downs, etc)
            AfterFormInitialization();
        }
        private void CopyPropertiesTo(object fromObject, object toObject)
        {
            PropertyInfo[] toObjectProperties = toObject.GetType().GetProperties();
            foreach (PropertyInfo propTo in toObjectProperties)
            {
                try
                {
                    PropertyInfo propFrom = fromObject.GetType().GetProperty(propTo.Name);
                    if (propFrom != null && propFrom.CanWrite)
                        propTo.SetValue(toObject, propFrom.GetValue(fromObject, null), null);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private void AfterFormInitialization()
        {
            //force control resizing
            frmCommandEditor_Resize(null, null);
        }

        private void FocusFirstEditableControl()
        {
            Control targetControl = null;
            foreach(Control ctrl in flw_InputVariables.Controls)
            {
                if (ctrl is FlowLayoutPanel flp)
                {
                    foreach(Control c in flp.Controls)
                    {
                        if ((c is TextBox) || (c is ComboBox) || (c is DataGridView))
                        {
                            targetControl = c;
                            break;
                        }
                    }
                    if (targetControl != null)
                    {
                        break;
                    }
                }
                else if((ctrl is TextBox) || (ctrl is ComboBox) || (ctrl is DataGridView))
                {
                    targetControl = ctrl;
                    break;
                }
            }
            if (targetControl != null)
            {
                targetControl.Focus();
                if (targetControl is TextBox txt)
                {
                    txt.SelectionStart = txt.Text.Length;
                    txt.SelectionLength = 0;
                }
            }
        }

        private void frmCommandEditor_Shown(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.Sizable;

            // focus first control
            FocusFirstEditableControl();

            if (this.creationMode == CreationMode.Edit)
            {
                this.Text = "Edit Command";
            }
        }

        private void frmCommandEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            // set Size, Position
            if (this.Owner is frmScriptBuilder f)
            {
                f.setCommandEditorSizeAndPosition(this);
            }

            //((frmScriptBuilder)this.Owner).setCommandEditorSizeAndPosition(this);
        }

        #endregion Form Events

        private void cboSelectedCommand_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //find underlying command item
            var selectedCommandItem = cboSelectedCommand.Text;

            //get command
            var userSelectedCommand = commandList.Where(itm => itm.FullName == selectedCommandItem).FirstOrDefault();

            //create new command for binding
            selectedCommand = (ScriptCommand)Activator.CreateInstance(userSelectedCommand.CommandClass);


            //Todo: MAKE OPTION TO RENDER ON THE FLY

            //if (true)
            //{
            //    var renderedControls = selectedCommand.Render(null);
            //    userSelectedCommand.UIControls = new List<Control>();
            //    userSelectedCommand.UIControls.AddRange(renderedControls);
            //}


            //update data source
            userSelectedCommand.Command = selectedCommand;

            //bind controls to new data source
            userSelectedCommand.Bind(this);

            flw_InputVariables.SuspendLayout();

            //clear controls
            flw_InputVariables.Controls.Clear();

            //add each control
            foreach (var ctrl in userSelectedCommand.UIControls)
            {
                flw_InputVariables.Controls.Add(ctrl);
            }

            flw_InputVariables.ResumeLayout();

            //focus first TextBox
            FocusFirstEditableControl();

            // resize
            frmCommandEditor_Resize(null, null);
        }


        #region Save/Close Buttons

        //handles returning DialogResult

        private void uiBtnAdd_Click(object sender, EventArgs e)
        {
            selectedCommand.BeforeValidate();

            //commit any datagridviews
            foreach (Control ctrl in flw_InputVariables.Controls)
            {
                if (ctrl is DataGridView)
                {
                    DataGridView currentControl = (DataGridView)ctrl;
                    currentControl.EndEdit();
                }

                //if (ctrl is UIPictureBox)
                //{
                //    var typedControl = (UIPictureBox)ctrl;
                //    var cmd = (Core.Automation.Commands.ImageRecognitionCommand)selectedCommand;
                //    cmd.v_ImageCapture = typedControl.EncodedImage;
                //}
            }

            if (selectedCommand.IsValidate(this))
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                if (!appSettings.ClientSettings.DontShowValidationMessage)
                {
                    using(Supplemental.frmDialog fm = new Supplemental.frmDialog(selectedCommand.validationResult, selectedCommand.SelectionName, Supplemental.frmDialog.DialogType.OkOnly, 0))
                    {
                        fm.ShowDialog();
                    }
                }
                this.DialogResult = DialogResult.OK;
            }
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        #endregion Save/Close Buttons


        private void frmCommandEditor_Resize(object sender, EventArgs e)
        {
            flw_InputVariables.SuspendLayout();
            foreach (Control item in flw_InputVariables.Controls)
            {
                item.Width = this.Width - 70;

                // flow
                if (item is FlowLayoutPanel flp)
                {
                    //flp.SuspendLayout();
                    foreach(Control c in flp.Controls)
                    {
                        c.Width = this.Width - 70;
                    }
                }
            }
            flw_InputVariables.ResumeLayout();

            // when resize, selectedCommand has null
            selectedCommand?.AfterShown();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        #region footer buttons event

        private void uiButtonVariable_Click(object sender, EventArgs e)
        {
            using (frmScriptVariables scriptVariableEditor = new frmScriptVariables(this.scriptVariables, this.appSettings))
            {
                //scriptVariableEditor.appSettings = this.appSettings;
                //scriptVariableEditor.scriptVariables = this.scriptVariables;

                if (scriptVariableEditor.ShowDialog() == DialogResult.OK)
                {
                    this.scriptVariables = scriptVariableEditor.scriptVariables.OrderBy(v => v.VariableName).ToList();
                    reloadVariableList();
                }
            }
        }

        private void btnHelpThisCommand_Click(object sender, EventArgs e)
        {
            var userSelectedCommand = commandList.Where(itm => (itm.FullName == cboSelectedCommand.Text)).FirstOrDefault();
            //string page = userSelectedCommand.ShortName.ToLower().Replace(" ", "-").Replace("/", "-") + "-command.md";
            ////string parent = ((Core.Automation.Attributes.ClassAttributes.Group)userSelectedCommand.GetType().GetCustomAttribute(typeof(Core.Automation.Attributes.ClassAttributes.Group))).groupName.ToLower().Replace(" ", "-");
            //string parent = userSelectedCommand.DisplayGroup.ToLower().Replace(" ", "-").Replace("/", "-");
            System.Diagnostics.Process.Start(Core.MyURLs.GetWikiURL(userSelectedCommand.ShortName, userSelectedCommand.DisplayGroup));
        }
        #endregion

        #region reload variable list
        private void reloadVariableList()
        {
            PropertyInfo[] props = selectedCommand.GetType().GetProperties();
            foreach(PropertyInfo prop in props)
            {
                if (prop.Name.StartsWith("v_") && (prop.Name != "v_Comment"))
                {
                    //var varList = (Core.Automation.Attributes.PropertyAttributes.PropertyIsVariablesList)prop.GetCustomAttribute(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyIsVariablesList));
                    var vProp = PropertyControls.GetVirtualProperty(prop);
                    var varList = PropertyControls.GetCustomAttributeWithVirtual<Core.Automation.Attributes.PropertyAttributes.PropertyIsVariablesList>(prop, vProp);
                    //if ((varList != null) && (varList.isVariablesList))
                    if (varList?.isVariablesList ?? false)
                    {
                        var trgCtrl = flw_InputVariables.Controls.Find(prop.Name, true).FirstOrDefault();
                        if ((trgCtrl != null) && (trgCtrl is ComboBox cmb))
                        {
                            CommandControls.AddVariableNames(cmb, this);
                        }
                    }
                }
            }
        }

        #endregion

        #region command list
        private void cboSelectedCommand_Click(object sender, EventArgs e)
        {
            using(var fm = new Supplement_Forms.frmCommandList(appSettings, treeAllCommands, treeAllCommandsImage, cboSelectedCommand.Text))
            {
                if (fm.ShowDialog() == DialogResult.OK)
                {
                    string newCommand = fm.FullCommandName;
                    if (cboSelectedCommand.Text != fm.FullCommandName)
                    {
                        cboSelectedCommand.Text = fm.FullCommandName;
                        cboSelectedCommand_SelectionChangeCommitted(null, null);
                    }
                }
            }
        }
        #endregion
    }
}
