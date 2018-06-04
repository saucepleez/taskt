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
using taskt.Core.AutomationCommands.Attributes;
using System.IO;

namespace taskt.UI.Forms
{
    public partial class frmCommandEditor : Form
    {
        //list of available commands
        List<CommandItem> commandList = new List<CommandItem>();
        //list of variables, assigned from frmScriptBuilder
        public List<Core.Script.ScriptVariable> scriptVariables;
        //reference to currently selected command
        public Core.AutomationCommands.ScriptCommand selectedCommand;
        //assigned by frmScriptBuilder to restrict inputs for editing existing commands
        public CreationMode creationMode;
        //startup command, assigned from frmCommand Browser
        public string defaultStartupCommand;
        public frmCommandEditor()
        {
            InitializeComponent();
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
            //Track the creation mode required - Add or Edit depending on calling Form
            if (creationMode == CreationMode.Add)
            {
                //Set DisplayMember to track DisplayValue from the class
                cboSelectedCommand.DisplayMember = "DisplayValue";

                //Pull all available automation commands
                var commandClasses = Assembly.GetExecutingAssembly().GetTypes()
                          .Where(t => t.Namespace == "taskt.Core.AutomationCommands")
                          .Where(t => t.Name != "ScriptCommand")
                          .Where(t => t.IsAbstract == false)
                          .Where(t => t.BaseType.Name == "ScriptCommand")
                          .ToList();

                //Loop through each class
                foreach (var commandClass in commandClasses)
                {
                    var groupingAttribute = commandClass.GetCustomAttributes(typeof(Core.AutomationCommands.Attributes.ClassAttributes.Group), true);
                    string groupAttribute = "";
                    if (groupingAttribute.Length > 0)
                    {
                        var attributeFound = (Core.AutomationCommands.Attributes.ClassAttributes.Group)groupingAttribute[0];
                        groupAttribute = attributeFound.groupName;
                    }

                    //Instantiate Class
                    Core.AutomationCommands.ScriptCommand newCommand = (Core.AutomationCommands.ScriptCommand)Activator.CreateInstance(commandClass);

                    //If command is enabled, pull for display and configuration
                    if (newCommand.CommandEnabled)
                    {
                        CommandItem newCommandItem = new CommandItem();
                        newCommandItem.DisplayValue = string.Join(" - ", groupAttribute,newCommand.SelectionName);
                        newCommandItem.CommandInstance = newCommand;
                        commandList.Add(newCommandItem);
                    }
                }

                commandList = commandList.OrderBy(itm => itm.DisplayValue).ToList();

                //set combobox to coammand list
                cboSelectedCommand.DataSource = commandList;

                if ((defaultStartupCommand != null) && (commandList.Where(x => x.DisplayValue == defaultStartupCommand).Count() > 0))

                {
                    cboSelectedCommand.SelectedIndex = cboSelectedCommand.FindStringExact(defaultStartupCommand);
                }
                else
                {
                    //set index to first item
                    cboSelectedCommand.SelectedIndex = 0;
                }

                //force commit event to populate the flow layout
                cboSelectedCommand_SelectionChangeCommitted(null, null);
            }
            else

            {
                //enable only the command passed for edit mode
                cboSelectedCommand.Items.Add(selectedCommand.SelectionName);
                cboSelectedCommand.SelectedIndex = 0;
                cboSelectedCommand.Enabled = false;
                cboSelectedCommand.Text = selectedCommand.SelectionName;
                GenerateUIInputElements(selectedCommand);
            }
        }
        private void frmCommandEditor_Shown(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.Sizable;
        }

        #endregion Form Events

        #region Command UI Field Generation

        //handles generation of controls for the main flowlayout and tracking/assignment of input elements

        /// <summary>
        /// Generate UI elements for data-collection based on the selected command
        /// </summary>
        private void GenerateUIInputElements(Core.AutomationCommands.ScriptCommand currentCommand)
        {
            //remove all existing controls
            while (flw_InputVariables.Controls.Count > 0) flw_InputVariables.Controls.RemoveAt(0);

            //find all input variables -- all input variables start with "v_" in the associated class
            var inputVariableFields = currentCommand.GetType().GetProperties().Where(f => f.Name.StartsWith("v_")).ToList();

            //set form height
            int formHeight = 0;

            //loop through available variables
            foreach (var inputField in inputVariableFields)
            {
                //create a label for each variable name
                Label inputLabel = new Label();
                inputLabel.AutoSize = true;
                inputLabel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                inputLabel.ForeColor = Color.SteelBlue;
                inputLabel.Name = "lbl_" + inputField.Name;
                formHeight += 50;
                //apply friendly translation
                var propertyAttributesAssigned = inputField.GetCustomAttributes(typeof(Core.AutomationCommands.Attributes.PropertyAttributes.PropertyDescription), true);

                if (propertyAttributesAssigned.Length > 0)
                {
                    var attribute = (Core.AutomationCommands.Attributes.PropertyAttributes.PropertyDescription)propertyAttributesAssigned[0];
                    inputLabel.Text = attribute.propertyDescription;
                }
                else
                {
                    inputLabel.Text = inputField.Name;
                }

                var inputControl = GenerateInputControl(inputField, currentCommand);

                formHeight += inputControl.Height;

                //add label and input control to flow layout
                flw_InputVariables.Controls.Add(inputLabel);

                //find if UI helpers are applied
                var propertyAllowsVars = inputField.GetCustomAttributes(typeof(Core.AutomationCommands.Attributes.PropertyAttributes.PropertyUIHelper), true);

                if (propertyAllowsVars.Length > 0)
                {
                    foreach (Core.AutomationCommands.Attributes.PropertyAttributes.PropertyUIHelper attrib in propertyAllowsVars)
                    {
                        taskt.UI.CustomControls.CommandItemControl variableInsertion = new taskt.UI.CustomControls.CommandItemControl();
                        variableInsertion.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
                        variableInsertion.ForeColor = Color.Black;
                        variableInsertion.Tag = inputControl;

                        switch (attrib.additionalHelper)
                        {
                            case Core.AutomationCommands.Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper:
                                //show variable selector
                                variableInsertion.CommandImage = UI.Images.GetUIImage("VariableCommand");
                                variableInsertion.CommandDisplay = "Insert Variable";
                                variableInsertion.Click += ShowVariableSelector;
                                flw_InputVariables.Controls.Add(variableInsertion);
                                break;

                            case Core.AutomationCommands.Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper:
                                //show file selector
                                variableInsertion.CommandImage = UI.Images.GetUIImage("ClipboardGetTextCommand");
                                variableInsertion.CommandDisplay = "Select a File";
                                variableInsertion.ForeColor = Color.Black;
                                variableInsertion.Tag = inputControl;
                                variableInsertion.Click += ShowFileSelector;
                                flw_InputVariables.Controls.Add(variableInsertion);
                                break;

                            case Core.AutomationCommands.Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowImageRecogitionHelper:
                                //show file selector
                                variableInsertion.CommandImage = UI.Images.GetUIImage("OCRCommand");
                                variableInsertion.CommandDisplay = "Capture Reference Image";
                                variableInsertion.ForeColor = Color.Black;
                                variableInsertion.Tag = inputControl;
                                variableInsertion.Click += ShowImageCapture;
                                flw_InputVariables.Controls.Add(variableInsertion);


                                taskt.UI.CustomControls.CommandItemControl testRun = new taskt.UI.CustomControls.CommandItemControl();
                                testRun.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
                                testRun.ForeColor = Color.Black;

                                testRun.CommandImage = UI.Images.GetUIImage("OCRCommand");
                                testRun.CommandDisplay = "Run Image Recognition Test";
                                testRun.ForeColor = Color.Black;
                                testRun.Tag = inputControl;
                                testRun.Click += RunImageCapture;
                                flw_InputVariables.Controls.Add(testRun);


                                break;

                            default:
                                break;
                        }
                    }

                    //var attribute = (Core.AutomationCommands.Attributes.PropertyAttributes.PropertyUIHelper)propertyAllowsVars[0];
                    //if (attribute.propertyAllowsVariables)
                    //{
                    //    //show variable selector
                    //    taskt.UI.CustomControls.CommandItemControl variableInsertion = new taskt.UI.CustomControls.CommandItemControl();
                    //    variableInsertion.CommandImage = UI.Images.GetUIImage("VariableCommand");
                    //    variableInsertion.CommandDisplay = "Insert Variable";
                    //    variableInsertion.ForeColor = Color.Black;
                    //    variableInsertion.Tag = inputControl;
                    //    variableInsertion.Click += ShowVariableSelector;
                    //    flw_InputVariables.Controls.Add(variableInsertion);
                    //}
                }

                //these types get a helper button to launch another form
                if (inputField.Name == "v_WebSearchTable")
                {
                    Core.AutomationCommands.IEBrowserElementCommand webCommand = (Core.AutomationCommands.IEBrowserElementCommand)currentCommand;
                    taskt.UI.CustomControls.CommandItemControl newitm = new taskt.UI.CustomControls.CommandItemControl();

                    newitm.CommandImage = UI.Images.GetUIImage(webCommand.CommandName);
                    newitm.CommandDisplay = "Click here to Capture Web Element";
                    newitm.ForeColor = Color.Black;
                    newitm.Click += ShowElementCaptureForm;
                    flw_InputVariables.Controls.Add(newitm);
                }
                else if (inputField.Name == "v_XMousePosition")
                {
                    Core.AutomationCommands.SendMouseMoveCommand mouseCommand = (Core.AutomationCommands.SendMouseMoveCommand)currentCommand;
                    taskt.UI.CustomControls.CommandItemControl newitm = new taskt.UI.CustomControls.CommandItemControl();
                    newitm.CommandImage = UI.Images.GetUIImage(mouseCommand.CommandName);
                    newitm.CommandDisplay = "Click here to Capture Mouse Position";
                    newitm.ForeColor = Color.Black;
                    newitm.Click += ShowMouseCaptureForm;
                    flw_InputVariables.Controls.Add(newitm);
                }

                //add to flow layout
                flw_InputVariables.Controls.Add(inputControl);

                //handle edit mode to add combobox data
                if ((creationMode == CreationMode.Edit) && (currentCommand is Core.AutomationCommands.BeginIfCommand) && (inputControl is DataGridView))
                {
                    Core.AutomationCommands.BeginIfCommand ifCmd = (Core.AutomationCommands.BeginIfCommand)currentCommand;
                    if (ifCmd.v_IfActionType == "Value")
                    {
                        DataGridViewComboBoxCell comparisonComboBox = new DataGridViewComboBoxCell();
                        comparisonComboBox.Items.Add("is equal to");
                        comparisonComboBox.Items.Add("is greater than");
                        comparisonComboBox.Items.Add("is greater than or equal to");
                        comparisonComboBox.Items.Add("is less than");
                        comparisonComboBox.Items.Add("is less than or equal to");
                        comparisonComboBox.Items.Add("is not equal to");

                        //assign cell as a combobox
                        DataGridView inputCtrl = (DataGridView)inputControl;
                        inputCtrl.Rows[1].Cells[1] = comparisonComboBox;
                    }
                }
            }

            if ((currentCommand is Core.AutomationCommands.IEBrowserElementCommand) && (creationMode == CreationMode.Edit))
            {
                Core.AutomationCommands.IEBrowserElementCommand webCommand = (Core.AutomationCommands.IEBrowserElementCommand)currentCommand;

                if (webCommand.v_WebAction == "Invoke Click")
                {
                    DataGridView webActionParameterBox = (DataGridView)flw_InputVariables.Controls["v_WebActionParameterTable"];
                    Label additionalParameterLabel = (Label)flw_InputVariables.Controls["lbl_v_WebActionParameterTable"];
                    additionalParameterLabel.Visible = false;
                    webActionParameterBox.Visible = false;
                }
            }

            //add additional offset
            this.Height = formHeight + 200;
        }

        private Control GenerateInputControl(PropertyInfo inputField, Core.AutomationCommands.ScriptCommand currentCommand)
        {
            //create control to capture input which will be assigned to the class variable
            dynamic InputControl;

            //check if selection options were assigned
            var selectionOptions = inputField.GetCustomAttributes(typeof(Core.AutomationCommands.Attributes.PropertyAttributes.PropertyUISelectionOption));
            if (selectionOptions.Count() > 0)
            {
                //create combobox for selection item
                InputControl = new ComboBox();
                InputControl.Height = 30;
                InputControl.Width = 250;
                InputControl.Font = new Font("Segoe UI", 12, FontStyle.Regular);

                //loop through options
                foreach (Core.AutomationCommands.Attributes.PropertyAttributes.PropertyUISelectionOption option in selectionOptions)
                {
                    InputControl.Items.Add(option.uiOption);
                }

                ComboBox control = InputControl;
                //additional helper for specific fields
                if (inputField.Name == "v_SeleniumElementAction")
                {
                    control.SelectedIndexChanged += seleniumAction_SelectionChangeCommitted;
                }
                else if (inputField.Name == "v_IfActionType")
                {
                    control.SelectedIndexChanged += ifAction_SelectionChangeCommitted;
                }
            }
            else
            {
                //legacy population method
                if (inputField.Name == "v_WindowName")
                {
                    InputControl = new ComboBox();
                    InputControl.Height = 30;
                    InputControl.Width = 200;
                    //add an option for current window which is the window which is currently in the foreground
                    InputControl.Items.Add("Current Window");
                    //get all running processes
                    Process[] processlist = Process.GetProcesses();
                    //pull the main window title for each
                    foreach (Process process in processlist)
                    {
                        if (!String.IsNullOrEmpty(process.MainWindowTitle))
                        {
                            //add to the control list of available windows
                            InputControl.Items.Add(process.MainWindowTitle);
                        }
                    }
                }
                else if (inputField.Name == "v_ScreenshotWindowName")
                {
                    InputControl = new ComboBox();
                    InputControl.Height = 30;
                    InputControl.Width = 200;
                    //add an option for current window which is the window which is currently in the foreground
                    InputControl.Items.Add("Desktop");
                    //get all running processes
                    Process[] processlist = Process.GetProcesses();
                    //pull the main window title for each
                    foreach (Process process in processlist)
                    {
                        if (!String.IsNullOrEmpty(process.MainWindowTitle))
                        {
                            //add to the control list of available windows
                            InputControl.Items.Add(process.MainWindowTitle);
                        }
                    }
                }
                else if (inputField.Name == "v_AutomationWindowName")
                {
                    InputControl = new ComboBox();
                    InputControl.Height = 30;
                    InputControl.Width = 200;

                    //get all running processes
                    Process[] processlist = Process.GetProcesses();
                    //pull the main window title for each
                    foreach (Process process in processlist)
                    {
                        if (!String.IsNullOrEmpty(process.MainWindowTitle))
                        {
                            //add to the control list of available windows
                            InputControl.Items.Add(process.MainWindowTitle);
                        }
                    }

                    InputControl.SelectedIndexChanged +=
                             new System.EventHandler(AutomationWindowName_SelectedIndexChanged);
                }
                else if (inputField.Name == "v_AutomationHandleName")
                {
                    InputControl = new ComboBox();
                    InputControl.Height = 30;
                    InputControl.Width = 200;
                }
                else if (inputField.Name == "v_AutomationHandleDisplayName")
                {
                    InputControl = new ComboBox();
                    InputControl.Height = 30;
                    InputControl.Width = 200;

                    InputControl.SelectedIndexChanged +=
                           new System.EventHandler(DisplayHandleSelected_SelectedIndexChanged);
                }
                else if (inputField.Name == "v_MouseClick")
                {
                    InputControl = new ComboBox();
                    InputControl.Height = 30;
                    InputControl.Width = 300;
                    InputControl.Items.Add("None");
                    InputControl.Items.Add("Left Click");
                    InputControl.Items.Add("Middle Click");
                    InputControl.Items.Add("Right Click");
                    InputControl.Items.Add("Left Down");
                    InputControl.Items.Add("Middle Down");
                    InputControl.Items.Add("Right Down");
                    InputControl.Items.Add("Left Up");
                    InputControl.Items.Add("Middle Up");
                    InputControl.Items.Add("Right Up");
                }
                else if (inputField.Name == "v_WebAction")
                {
                    InputControl = new ComboBox();
                    InputControl.Height = 30;
                    InputControl.Width = 300;

                    InputControl.Items.Add("Invoke Click");
                    InputControl.Items.Add("Left Click");
                    InputControl.Items.Add("Middle Click");
                    InputControl.Items.Add("Right Click");

                    InputControl.Items.Add("Get Attribute");
                    InputControl.Items.Add("Set Attribute");

                    ComboBox webAction = (ComboBox)InputControl;
                    webAction.SelectedIndexChanged += webAction_SelectionChangeCommitted;
                }
                else if ((inputField.Name == "v_userVariableName") || (inputField.Name == "v_applyToVariableName"))
                {
                    InputControl = new ComboBox();
                    foreach (var scriptVariable in scriptVariables)
                    {
                        if (!string.IsNullOrEmpty(scriptVariable.variableName))
                            InputControl.Items.Add(scriptVariable.variableName);
                    }

                    if (scriptVariables.Count == 0)
                    {
                        MessageBox.Show("The selected command requires a user variable but no user variables were found! To create a user variable, please select the 'Variables' button on the main form to define a new variable!", "No User-Defined Variables Found");
                    }


                }
                else if ((inputField.Name == "v_WebActionParameterTable") || (inputField.Name == "v_IfActionParameterTable"))
                {
                    InputControl = new DataGridView();

                    InputControl.Name = inputField.Name;
                    InputControl.Width = 500;
                    InputControl.Height = 140;

                    DataGridViewTextBoxColumn propertyName = new DataGridViewTextBoxColumn();
                    propertyName.HeaderText = "Parameter Name";
                    propertyName.DataPropertyName = "Parameter Name";
                    InputControl.Columns.Add(propertyName);

                    DataGridViewTextBoxColumn propertyValue = new DataGridViewTextBoxColumn();
                    propertyValue.HeaderText = "Parameter Value";
                    propertyValue.DataPropertyName = "Parameter Value";
                    InputControl.Columns.Add(propertyValue);

                    InputControl.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
                    InputControl.AllowUserToAddRows = false;
                    InputControl.AllowUserToDeleteRows = false;

                    //set datasource

                    if (currentCommand is Core.AutomationCommands.IEBrowserElementCommand)
                    {
                        var cmd = (Core.AutomationCommands.IEBrowserElementCommand)currentCommand;
                        InputControl.DataSource = cmd.v_WebActionParameterTable;
                        InputControl.Font = new Font("Segoe UI", 8, FontStyle.Regular);
                    }
                    else if (currentCommand is Core.AutomationCommands.SeleniumBrowserElementActionCommand)
                    {
                        var cmd = (Core.AutomationCommands.SeleniumBrowserElementActionCommand)currentCommand;
                        InputControl.DataSource = cmd.v_WebActionParameterTable;
                        InputControl.Font = new Font("Segoe UI", 8, FontStyle.Regular);
                    }
                    else if (currentCommand is Core.AutomationCommands.BeginIfCommand)
                    {
                        var cmd = (Core.AutomationCommands.BeginIfCommand)currentCommand;
                        InputControl.DataSource = cmd.v_IfActionParameterTable;
                        InputControl.Font = new Font("Segoe UI", 8, FontStyle.Regular);
                    }
                }
                else if (inputField.Name == "v_IEBrowserName")
                {
                    InputControl = new ComboBox();
                    InputControl.Width = 300;
                    InputControl.Height = 50;

                    var shellWindows = new ShellWindows();
                    foreach (IWebBrowser2 shellWindow in shellWindows)
                    {
                        if (shellWindow.Document is MSHTML.HTMLDocument)
                            InputControl.Items.Add(shellWindow.Document.Title);
                    }
                }
                else if (inputField.Name == "v_Comment")
                {
                    // assumed that all "v_Comment" will need to have a larger box for typing user comments about the action
                    InputControl = new TextBox();

                    if (currentCommand is Core.AutomationCommands.CommentCommand)
                    {
                        InputControl.Height = 300;
                        InputControl.Width = 400;
                    }
                    else
                    {
                        InputControl.Height = 100;
                        InputControl.Width = 300;
                    }

                    InputControl.Multiline = true;
                }
                else if (inputField.Name == "v_WebSearchTable")
                {
                    InputControl = new DataGridView();

                    InputControl.Name = inputField.Name;
                    InputControl.Width = 400;
                    InputControl.Height = 180;

                    InputControl.AutoGenerateColumns = false;
                    DataGridViewCheckBoxColumn enabledColumn = new DataGridViewCheckBoxColumn();
                    enabledColumn.HeaderText = "Enabled";
                    enabledColumn.DataPropertyName = "Enabled";
                    InputControl.Columns.Add(enabledColumn);

                    DataGridViewTextBoxColumn propertyName = new DataGridViewTextBoxColumn();
                    propertyName.HeaderText = "Property Name";
                    propertyName.DataPropertyName = "Property Name";
                    InputControl.Columns.Add(propertyName);

                    DataGridViewTextBoxColumn propertyValue = new DataGridViewTextBoxColumn();
                    propertyValue.HeaderText = "Property Value";
                    propertyValue.DataPropertyName = "Property Value";
                    InputControl.Columns.Add(propertyValue);

                    InputControl.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

                    //set datasource
                    var cmd = (Core.AutomationCommands.IEBrowserElementCommand)currentCommand;
                    InputControl.DataSource = cmd.v_WebSearchTable;
                    InputControl.Font = new Font("Segoe UI", 8, FontStyle.Regular);
                }
                else if(inputField.Name == "v_ImageCapture")
                {

                    InputControl = new PictureBox();
                    InputControl.Name = inputField.Name;
                    InputControl.Width = 200;
                    InputControl.Height = 150;
                    InputControl.BackColor = Color.LightGray;
                    InputControl.SizeMode = PictureBoxSizeMode.AutoSize;

                }
                else
                {
                    //variable is simply a standard variable
                    InputControl = new TextBox();
                    InputControl.Height = 30;
                    InputControl.Width = 300;
                }

                //standard for all controls
                InputControl.Visible = true;
                InputControl.Name = inputField.Name;
            }

            if ((InputControl is PictureBox) && (currentCommand is taskt.Core.AutomationCommands.ImageRecognitionCommand))
            {
                var cmd = (Core.AutomationCommands.ImageRecognitionCommand)currentCommand;

                if ((cmd.v_ImageCapture != "") && (cmd.v_ImageCapture != null))
                {
                    InputControl.Image = Core.Common.Base64ToImage(cmd.v_ImageCapture);
                }

                InputControl.DataBindings.Add("Text", currentCommand, inputField.Name, false, DataSourceUpdateMode.OnPropertyChanged);

            }
            else if (!(InputControl is DataGridView)) //dgv already has binding set
            {
                InputControl.Font = new Font("Segoe UI", 12, FontStyle.Regular);
                InputControl.DataBindings.Add("Text", currentCommand, inputField.Name, false, DataSourceUpdateMode.OnPropertyChanged);
            }

            return InputControl;
        }

        #endregion Command UI Field Generation

        #region ComboBox Events

        //handles all combobox events
        private void webAction_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox webAction = (ComboBox)sender;
            DataGridView webActionParameterBox = (DataGridView)flw_InputVariables.Controls["v_WebActionParameterTable"];
            Label additionalParameterLabel = (Label)flw_InputVariables.Controls["lbl_v_WebActionParameterTable"];

            if ((webActionParameterBox == null) || (webAction == null) || (webActionParameterBox.DataSource == null))
                return;

            Core.AutomationCommands.IEBrowserElementCommand cmd = (Core.AutomationCommands.IEBrowserElementCommand)selectedCommand;
            DataTable actionParameters = cmd.v_WebActionParameterTable;
            actionParameters.Rows.Clear();

            switch (webAction.Text)
            {
                case "Invoke Click":
                    additionalParameterLabel.Visible = false;
                    webActionParameterBox.Visible = false;
                    break;

                case "Left Click":
                case "Middle Click":
                case "Right Click":
                    additionalParameterLabel.Visible = true;
                    webActionParameterBox.Visible = true;
                    actionParameters.Rows.Add("X Adjustment", 0);
                    actionParameters.Rows.Add("Y Adjustment", 0);
                    break;

                case "Set Attribute":
                    additionalParameterLabel.Visible = true;
                    webActionParameterBox.Visible = true;
                    actionParameters.Rows.Add("Attribute Name");
                    actionParameters.Rows.Add("Value To Set");

                    break;

                case "Get Attribute":
                    additionalParameterLabel.Visible = true;
                    webActionParameterBox.Visible = true;
                    actionParameters.Rows.Add("Attribute Name");
                    actionParameters.Rows.Add("Variable Name");

                    //DataGridViewComboBoxCell getAttributeComboBox = new DataGridViewComboBoxCell();
                    //getAttributeComboBox.Items.Add("innerText");
                    //webActionParameterBox.Rows[0].Cells[1] = getAttributeComboBox;

                    //DataGridViewComboBoxCell variableComboBox = new DataGridViewComboBoxCell();
                    //scriptVariables.Where(x => !string.IsNullOrEmpty(x.variableName)).ToList().ForEach(x => variableComboBox.Items.Add(x.variableName));
                    //webActionParameterBox.Rows[1].Cells[1] = variableComboBox;

                    break;

                default:
                    break;
            }

            //AutomationCommands.WebBrowserElementCommand cmd = (AutomationCommands.WebBrowserElementCommand)selectedCommand;
            //webActionParameterBox.DataSource = actionParameters;
            //cmd.v_WebSearchTable = actionParameters;
        }
        private void ifAction_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox ifAction = (ComboBox)sender;
            DataGridView ifActionParameterBox = (DataGridView)flw_InputVariables.Controls["v_IfActionParameterTable"];
            Label additionalParameterLabel = (Label)flw_InputVariables.Controls["lbl_v_IfActionParameterTable"];

            if ((ifActionParameterBox == null) || (ifAction == null) || (ifActionParameterBox.DataSource == null))
                return;

            Core.AutomationCommands.BeginIfCommand cmd = (Core.AutomationCommands.BeginIfCommand)selectedCommand;
            DataTable actionParameters = cmd.v_IfActionParameterTable;
            actionParameters.Rows.Clear();

            switch (ifAction.Text)
            {
                case "Value":
                    additionalParameterLabel.Visible = true;
                    ifActionParameterBox.Visible = true;
                    actionParameters.Rows.Add("Value1", "");
                    actionParameters.Rows.Add("Operand", "");
                    actionParameters.Rows.Add("Value2", "");

                    //combobox cell for Variable Name
                    DataGridViewComboBoxCell comparisonComboBox = new DataGridViewComboBoxCell();
                    comparisonComboBox.Items.Add("is equal to");
                    comparisonComboBox.Items.Add("is greater than");
                    comparisonComboBox.Items.Add("is greater than or equal to");
                    comparisonComboBox.Items.Add("is less than");
                    comparisonComboBox.Items.Add("is less than or equal to");
                    comparisonComboBox.Items.Add("is not equal to");

                    //assign cell as a combobox
                    ifActionParameterBox.Rows[1].Cells[1] = comparisonComboBox;

                    break;

                default:
                    break;
            }
        }
        private void seleniumAction_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox webAction = (ComboBox)sender;
            DataGridView webActionParameterBox = (DataGridView)flw_InputVariables.Controls["v_WebActionParameterTable"];
            Label additionalParameterLabel = (Label)flw_InputVariables.Controls["lbl_v_WebActionParameterTable"];

            if ((webActionParameterBox == null) || (webAction == null) || (webActionParameterBox.DataSource == null))
                return;

            Core.AutomationCommands.SeleniumBrowserElementActionCommand cmd = (Core.AutomationCommands.SeleniumBrowserElementActionCommand)selectedCommand;
            DataTable actionParameters = cmd.v_WebActionParameterTable;
            actionParameters.Rows.Clear();

            switch (webAction.Text)
            {
                case "Invoke Click":
                    webActionParameterBox.Hide();
                    additionalParameterLabel.Hide();
                    break;

                case "Left Click":
                case "Middle Click":
                case "Right Click":
                    webActionParameterBox.Show();
                    additionalParameterLabel.Show();
                    actionParameters.Rows.Add("X Adjustment", 0);
                    actionParameters.Rows.Add("Y Adjustment", 0);
                    break;

                case "Set Text":
                    webActionParameterBox.Show();
                    additionalParameterLabel.Show();
                    actionParameters.Rows.Add("Text To Set");
                    break;

                case "Get Text":
                    webActionParameterBox.Show();
                    additionalParameterLabel.Show();
                    actionParameters.Rows.Add("Variable Name");
                    break;

                case "Get Attribute":
                    webActionParameterBox.Show();
                    additionalParameterLabel.Show();
                    actionParameters.Rows.Add("Attribute Name");
                    actionParameters.Rows.Add("Variable Name");
                    break;

                case "Wait For Element To Exist":
                    webActionParameterBox.Show();
                    additionalParameterLabel.Show();
                    actionParameters.Rows.Add("Timeout (Seconds)");
                    break;

                default:
                    break;
            }
        }
        private void cboSelectedCommand_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //find underlying command class and generate the required items on the UI flow layout for configuration
            var selectedCommandItem = cboSelectedCommand.Text;
            selectedCommand = commandList.Where(itm => itm.DisplayValue == selectedCommandItem).FirstOrDefault().CommandInstance;
            GenerateUIInputElements(selectedCommand);
        }
        private void cboSelectedCommand_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void AutomationWindowName_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            var senderBox = (ComboBox)sender;

            if (senderBox.Text != string.Empty)
            {
                ComboBox handleControl;
                if (selectedCommand is Core.AutomationCommands.ThickAppClickItemCommand)
                {
                    handleControl = (ComboBox)flw_InputVariables.Controls["v_AutomationHandleName"];
                }
                else
                {
                    handleControl = (ComboBox)flw_InputVariables.Controls["v_AutomationHandleDisplayName"];
                }

                if (handleControl != null)
                {
                    Core.AutomationCommands.ThickAppClickItemCommand newAppCommand = new Core.AutomationCommands.ThickAppClickItemCommand();
                    var handleList = newAppCommand.FindHandleObjects(senderBox.Text);
                    handleControl.DataSource = handleList;
                }
            }
        }
        private void DisplayHandleSelected_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            var senderBox = (ComboBox)sender;

            if (senderBox.Text != string.Empty)
            {
                var WindowNameControl = (ComboBox)flw_InputVariables.Controls["v_AutomationWindowName"];
                var IDControl = (TextBox)flw_InputVariables.Controls["v_AutomationID"];

                if (IDControl != null)
                {
                    Core.AutomationCommands.ThickAppGetTextCommand newAppCommand = new Core.AutomationCommands.ThickAppGetTextCommand();
                    var AutomationID = newAppCommand.FindHandleID(WindowNameControl.Text, senderBox.Text);

                    IDControl.Text = AutomationID;
                    Core.AutomationCommands.ThickAppGetTextCommand cmd = (Core.AutomationCommands.ThickAppGetTextCommand)selectedCommand;
                    cmd.v_AutomationID = AutomationID;
                }
            }
        }

        private void SetComboBox()
        {
        }

        #endregion ComboBox Events

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
            }

            this.DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        #endregion Save/Close Buttons

        #region Assitance Forms Events

        //handles forms to help capture additional details

        private void ShowElementCaptureForm(object sender, EventArgs e)
        {
            taskt.UI.Forms.Supplemental.frmBrowserElementBuilder frmElementBuilder = new taskt.UI.Forms.Supplemental.frmBrowserElementBuilder();
            if (frmElementBuilder.ShowDialog() == DialogResult.OK)
            {
                Core.AutomationCommands.IEBrowserElementCommand cmd = (Core.AutomationCommands.IEBrowserElementCommand)selectedCommand;
                DataTable searchParameterTable = (DataTable)frmElementBuilder.dgvSearchParameters.DataSource;

                if (searchParameterTable == null)
                {
                    return;
                }

                searchParameterTable.TableName = "WebSearchParamTable" + DateTime.Now.ToString("MMddyy.hhmmss");
                DataGridView dgvSearchView = (DataGridView)flw_InputVariables.Controls["v_WebSearchTable"];
                dgvSearchView.DataSource = searchParameterTable;
                cmd.v_WebSearchTable = searchParameterTable;
            }

            ShowAllForms();
        }
        public static void ShowAllForms()
        {
            foreach (Form frm in Application.OpenForms)
            {
                frm.WindowState = FormWindowState.Normal;
            }
        }
        public static void HideAllForms()
        {
            foreach (Form frm in Application.OpenForms)
            {
                frm.WindowState = FormWindowState.Minimized;
            }
        }
        private void ShowMouseCaptureForm(object sender, EventArgs e)
        {
            taskt.UI.Forms.Supplemental.frmShowCursorPosition frmShowCursorPos = new taskt.UI.Forms.Supplemental.frmShowCursorPosition();

            //if user made a successful selection
            if (frmShowCursorPos.ShowDialog() == DialogResult.OK)
            {
                //Todo - ideally one function to add to textbox which adds to class

                //add selected variables to associated control text
                flw_InputVariables.Controls["v_XMousePosition"].Text = frmShowCursorPos.xPos.ToString();
                flw_InputVariables.Controls["v_YMousePosition"].Text = frmShowCursorPos.yPos.ToString();

                //find current command and add to underlying class
                Core.AutomationCommands.SendMouseMoveCommand cmd = (Core.AutomationCommands.SendMouseMoveCommand)selectedCommand;
                cmd.v_XMousePosition = frmShowCursorPos.xPos;
                cmd.v_YMousePosition = frmShowCursorPos.yPos;
            }
        }
        private void ShowVariableSelector(object sender, EventArgs e)
        {
            //create variable selector form
            UI.Forms.Supplemental.frmVariableSelector newVariableSelector = new Supplemental.frmVariableSelector();

            //get copy of user variables and append system variables, then load to combobox
            var variableList = scriptVariables.Select(f => f.variableName).ToList();
            variableList.AddRange(Core.Common.GenerateSystemVariables().Select(f => f.variableName));
            newVariableSelector.lstVariables.Items.AddRange(variableList.ToArray());

            //if user pressed "OK"
            if (newVariableSelector.ShowDialog() == DialogResult.OK)
            {
                //ensure that a variable was actually selected
                if (newVariableSelector.lstVariables.SelectedItem == null)
                {
                    //return out as nothing was selected
                    MessageBox.Show("There were no variables selected!");
                    return;
                }

                //grab the referenced input assigned to the 'insert variable' button instance
                CustomControls.CommandItemControl inputBox = (CustomControls.CommandItemControl)sender;
                //currently variable insertion is only available for simply textboxes

                if (inputBox.Tag is TextBox)
                {
                    TextBox targetTextbox = (TextBox)inputBox.Tag;
                    //concat variable name with brackets [vVariable] as engine searches for the same
                    targetTextbox.Text = targetTextbox.Text + string.Concat("[", newVariableSelector.lstVariables.SelectedItem.ToString(), "]");
                }
                else if(inputBox.Tag is ComboBox)
                {
                    ComboBox targetCombobox = (ComboBox)inputBox.Tag;
                    //concat variable name with brackets [vVariable] as engine searches for the same
                    targetCombobox.Text = targetCombobox.Text + string.Concat("[", newVariableSelector.lstVariables.SelectedItem.ToString(), "]");
                }

            }
        }
        private void ShowFileSelector(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                CustomControls.CommandItemControl inputBox = (CustomControls.CommandItemControl)sender;
                //currently variable insertion is only available for simply textboxes
                TextBox targetTextbox = (TextBox)inputBox.Tag;
                //concat variable name with brackets [vVariable] as engine searches for the same
                targetTextbox.Text = ofd.FileName;
            }
        }
        private void ShowImageCapture(object sender, EventArgs e)
        {

            HideAllForms();

            var userAcceptance = MessageBox.Show(this, "The image capture process will now begin and display a screenshot of the current desktop in a custom full-screen window.  You may stop the capture process at any time by pressing the 'ESC' key, or selecting 'Close' at the top left. Simply create the image by clicking once to start the rectangle and clicking again to finish. The image will be cropped to the boundary within the red rectangle. Shall we proceed?", "Image Capture", MessageBoxButtons.YesNo);

            if (userAcceptance == DialogResult.Yes)
            {

            Supplement_Forms.frmImageCapture imageCaptureForm = new Supplement_Forms.frmImageCapture();

            if (imageCaptureForm.ShowDialog() == DialogResult.OK)
            {
                CustomControls.CommandItemControl inputBox = (CustomControls.CommandItemControl)sender;
                PictureBox targetPictureBox = (PictureBox)inputBox.Tag;
                targetPictureBox.Image = imageCaptureForm.userSelectedBitmap;
                targetPictureBox.Text = Core.Common.ImageToBase64(imageCaptureForm.userSelectedBitmap);
            }

            }

            ShowAllForms();
        }
        private void RunImageCapture(object sender, EventArgs e)
        {

            //get input control
            CustomControls.CommandItemControl inputBox = (CustomControls.CommandItemControl)sender;
            PictureBox targetPictureBox = (PictureBox)inputBox.Tag;
            string imageSource = targetPictureBox.Text;

            if (string.IsNullOrEmpty(imageSource))
            {
                MessageBox.Show("Please capture an image before attempting to test!");
                return;
            }

            //hide all
            HideAllForms();



            try
            {
                //run image recognition
                Core.AutomationCommands.ImageRecognitionCommand imageRecognitionCommand = new Core.AutomationCommands.ImageRecognitionCommand();
                imageRecognitionCommand.v_ImageCapture = imageSource;
                imageRecognitionCommand.RunCommand(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString());
            }
            //show all forms
            ShowAllForms();


        }


        #endregion Assitance Forms Events
    }
    /// <summary>
    /// Helper class for tracking command names and instances of the associated class
    /// </summary>
    /// <remarks>
    /// Used by the command editor form
    /// </remarks>
    public class CommandItem
    {
        public String DisplayValue { get; set; }
        public taskt.Core.AutomationCommands.ScriptCommand CommandInstance { get; set; }
    }
}