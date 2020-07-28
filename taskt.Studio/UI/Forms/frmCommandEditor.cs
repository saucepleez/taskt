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
using taskt.Commands;
using taskt.Core.Command;
using taskt.Core.Common;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Script;
using taskt.UI.CustomControls;
using taskt.UI.CustomControls.CustomUIControls;

namespace taskt.UI.Forms
{
    public partial class frmCommandEditor : ThemedForm, IfrmCommandEditor
    {
        //list of available commands
        private List<AutomationCommand> _commandList = new List<AutomationCommand>();
        //list of variables, assigned from frmScriptBuilder
        public List<ScriptVariable> ScriptVariables { get; set; }
        //list of elements, assigned from frmScriptBuilder
        public List<ScriptElement> ScriptElements { get; set; }
        //reference to currently selected command
        public ScriptCommand SelectedCommand { get; set; }
        //reference to original command
        public ScriptCommand OriginalCommand { get; set; }
        //assigned by frmScriptBuilder to restrict inputs for editing existing commands
        public CreationMode CreationModeInstance { get; set; }
        //startup command, assigned from frmScriptBuilder
        public string DefaultStartupCommand { get; set; }
        //editing command, assigned from frmScriptBuilder when editing a command
        public ScriptCommand EditingCommand { get; set; }
        //track existing commands for visibility
        public List<ScriptCommand> ConfiguredCommands { get; set; }

        #region Form Events
        //handle events for the form

        public frmCommandEditor(List<AutomationCommand> commands, List<ScriptCommand> existingCommands)
        {
            InitializeComponent();
            _commandList = commands;
            ConfiguredCommands = existingCommands;
        }

        private void frmNewCommand_Load(object sender, EventArgs e)
        {
            //declare loaded editor
            CommandControls.CurrentEditor = this;

            //order list
            _commandList = _commandList.OrderBy(itm => itm.FullName).ToList();

            //set command list
            cboSelectedCommand.DataSource = _commandList;

            //Set DisplayMember to track DisplayValue from the class
            cboSelectedCommand.DisplayMember = "FullName";

            if ((CreationModeInstance == CreationMode.Add) && (DefaultStartupCommand != null) && (_commandList.Where(x => x.FullName == DefaultStartupCommand).Count() > 0))
            {
                cboSelectedCommand.SelectedIndex = cboSelectedCommand.FindStringExact(DefaultStartupCommand);
            }
            else if (CreationModeInstance == CreationMode.Edit)
            {
                // var requiredCommand = commandList.Where(x => x.FullName.Contains(defaultStartupCommand)).FirstOrDefault(); //&& x.CommandClass.Name == originalCommand.CommandName).FirstOrDefault();

                var requiredCommand = _commandList.Where(x => x.Command.ToString() == EditingCommand.ToString()).FirstOrDefault();

                if (requiredCommand == null)
                {
                    MessageBox.Show("Command was not found! " + DefaultStartupCommand);
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
            if (OriginalCommand != null)
            {
                //copy original properties
                CopyPropertiesTo(OriginalCommand, SelectedCommand);

                //update bindings
                foreach (Control c in flw_InputVariables.Controls)
                {
                    foreach (Binding b in c.DataBindings)
                    {
                        b.ReadValue();
                    }

                    //helper for box
                    if (c is UIPictureBox)
                    {
                        var typedControl = (UIPictureBox)c;

                        var cmd = (ImageRecognitionCommand)SelectedCommand;

                        if (!string.IsNullOrEmpty(cmd.v_ImageCapture))
                        {
                            typedControl.Image = Common.Base64ToImage(cmd.v_ImageCapture);
                        }
                    }
                }
                //handle selection change events
            }

            //gracefully handle post initialization setups (drop downs, etc)
            AfterFormInitialization();
        }

        private void frmCommandEditor_Shown(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.Sizable;
        }

        private void frmCommandEditor_Resize(object sender, EventArgs e)
        {
            foreach (Control item in flw_InputVariables.Controls)
            {
                item.Width = this.Width - 70;
            }
        }

        private void cboSelectedCommand_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //clear controls
            flw_InputVariables.Controls.Clear();

            //find underlying command item
            var selectedCommandItem = cboSelectedCommand.Text;

            //get command
            var userSelectedCommand = _commandList.Where(itm => itm.FullName == selectedCommandItem).FirstOrDefault();

            //create new command for binding
            SelectedCommand = (ScriptCommand)Activator.CreateInstance(userSelectedCommand.CommandClass);

            //Todo: MAKE OPTION TO RENDER ON THE FLY

            //if (true)
            //{
            //    var renderedControls = selectedCommand.Render(null);
            //    userSelectedCommand.UIControls = new List<Control>();
            //    userSelectedCommand.UIControls.AddRange(renderedControls);
            //}

            //update data source
            userSelectedCommand.Command = SelectedCommand;

            //bind controls to new data source
            userSelectedCommand.Bind(this);

            //add each control
            foreach (var ctrl in userSelectedCommand.UIControls)
            {
                flw_InputVariables.Controls.Add(ctrl);
            }
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
        #endregion Form Events

        #region Save/Close Buttons

        //handles returning DialogResult
        private void uiBtnAdd_Click(object sender, EventArgs e)
        {
            //commit any datagridviews
            foreach (Control ctrl in flw_InputVariables.Controls)
            {
                if (ctrl is DataGridView)
                {
                    DataGridView currentControl = (DataGridView)ctrl;
                    currentControl.EndEdit();
                }

                if (ctrl is UIPictureBox)
                {
                    var typedControl = (UIPictureBox)ctrl;
                    var cmd = (ImageRecognitionCommand)SelectedCommand;
                    cmd.v_ImageCapture = typedControl.EncodedImage;
                }
            }
            DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        #endregion Save/Close Buttons
    }
}
