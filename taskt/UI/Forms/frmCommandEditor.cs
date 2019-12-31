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
using SHDocVw;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using taskt.Core.Automation.Attributes;
using System.IO;
using taskt.Core;
using taskt.UI.CustomControls;

namespace taskt.UI.Forms
{
    public partial class frmCommandEditor : ThemedForm
    {
        //list of available commands
        List<AutomationCommand> commandList = new List<AutomationCommand>();
        //list of variables, assigned from frmScriptBuilder
        public List<Core.Script.ScriptVariable> scriptVariables;
        //reference to currently selected command
        public Core.Automation.Commands.ScriptCommand selectedCommand;
        //reference to original command
        public Core.Automation.Commands.ScriptCommand originalCommand;
        //assigned by frmScriptBuilder to restrict inputs for editing existing commands
        public CreationMode creationMode;
        //startup command, assigned from frmScriptBuilder
        public string defaultStartupCommand;
        //editing command, assigned from frmScriptBuilder when editing a command
        public Core.Automation.Commands.ScriptCommand editingCommand;
        //track existing commands for visibility
        public List<Core.Automation.Commands.ScriptCommand> configuredCommands;
        public frmCommandEditor(List<AutomationCommand> commands, List<Core.Automation.Commands.ScriptCommand> existingCommands)
        {
            InitializeComponent();
            commandList = commands;
            configuredCommands = existingCommands;
        }

        public enum CreationMode
        {
            Add,
            Edit
        }

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

                        var cmd = (Core.Automation.Commands.ImageRecognitionCommand)selectedCommand;

                        if (!string.IsNullOrEmpty(cmd.v_ImageCapture))
                        {
                            typedControl.Image = Core.Common.Base64ToImage(cmd.v_ImageCapture);
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

        private void frmCommandEditor_Shown(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.Sizable;
        }

        #endregion Form Events

        private void cboSelectedCommand_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //clear controls
            flw_InputVariables.Controls.Clear();

            //find underlying command item
            var selectedCommandItem = cboSelectedCommand.Text;

            //get command
            var userSelectedCommand = commandList.Where(itm => itm.FullName == selectedCommandItem).FirstOrDefault();

            //create new command for binding
            selectedCommand = (Core.Automation.Commands.ScriptCommand)Activator.CreateInstance(userSelectedCommand.CommandClass);


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

            //add each control
            foreach (var ctrl in userSelectedCommand.UIControls)
            {
                flw_InputVariables.Controls.Add(ctrl);
            }
        
        }


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
                    var cmd = (Core.Automation.Commands.ImageRecognitionCommand)selectedCommand;
                    cmd.v_ImageCapture = typedControl.EncodedImage;
                }


            }

            this.DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        #endregion Save/Close Buttons


        private void frmCommandEditor_Resize(object sender, EventArgs e)
        {
            foreach (Control item in flw_InputVariables.Controls)
            {
                item.Width = this.Width - 70;
            }

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
