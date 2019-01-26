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
using taskt.Core.AutomationCommands.Attributes;
using System.IO;
using taskt.Core;

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

            //gracefully handle post initialization setups (drop downs, etc)
            AfterFormInitialization();

        }

        private void AfterFormInitialization()
        {
            if (creationMode == CreationMode.Edit && selectedCommand is Core.AutomationCommands.BeginIfCommand)
            {
              //load combo boxes
              ifAction_SelectionChangeCommitted(null, null);
            }
            else if(creationMode == CreationMode.Edit && selectedCommand is Core.AutomationCommands.UIAutomationCommand)
            {
              //load UIA boxes
              UIAType_SelectionChangeCommitted(null, null);
            }
            else if(creationMode == CreationMode.Edit && selectedCommand is Core.AutomationCommands.SeleniumBrowserElementActionCommand)
            {
                seleniumAction_SelectionChangeCommitted(null, null);
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
                inputLabel.ForeColor = Color.WhiteSmoke;
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
                        taskt.UI.CustomControls.CommandItemControl helperControl = new taskt.UI.CustomControls.CommandItemControl();
                        helperControl.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
                        helperControl.ForeColor = Color.AliceBlue;
                        helperControl.Name = inputLabel.Name + "_helper";
                        helperControl.Tag = inputControl;

                        switch (attrib.additionalHelper)
                        {
                            case Core.AutomationCommands.Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper:
                                //show variable selector
                                helperControl.CommandImage = UI.Images.GetUIImage("VariableCommand");
                                helperControl.CommandDisplay = "Insert Variable";
                                helperControl.Click += ShowVariableSelector;
                                flw_InputVariables.Controls.Add(helperControl);
                                break;

                            case Core.AutomationCommands.Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper:
                                //show file selector
                                helperControl.CommandImage = UI.Images.GetUIImage("ClipboardGetTextCommand");
                                helperControl.CommandDisplay = "Select a File";
                                helperControl.ForeColor = Color.AliceBlue;
                                helperControl.Tag = inputControl;
                                helperControl.Click += ShowFileSelector;
                                flw_InputVariables.Controls.Add(helperControl);
                                break;

                            case Core.AutomationCommands.Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowImageRecogitionHelper:
                                //show file selector
                                helperControl.CommandImage = UI.Images.GetUIImage("OCRCommand");
                                helperControl.CommandDisplay = "Capture Reference Image";
                                helperControl.ForeColor = Color.AliceBlue;
                                helperControl.Tag = inputControl;
                                helperControl.Click += ShowImageCapture;
                                flw_InputVariables.Controls.Add(helperControl);


                                taskt.UI.CustomControls.CommandItemControl testRun = new taskt.UI.CustomControls.CommandItemControl();
                                testRun.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
                                testRun.ForeColor = Color.AliceBlue;

                                testRun.CommandImage = UI.Images.GetUIImage("OCRCommand");
                                testRun.CommandDisplay = "Run Image Recognition Test";
                                testRun.ForeColor = Color.AliceBlue;
                                testRun.Tag = inputControl;
                                testRun.Click += RunImageCapture;
                                flw_InputVariables.Controls.Add(testRun);
                                break;

                            case Core.AutomationCommands.Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowCodeBuilder:
                                //show variable selector
                                helperControl.CommandImage = UI.Images.GetUIImage("RunScriptCommand");
                                helperControl.CommandDisplay = "Code Builder";
                                helperControl.Click += ShowCodeBuilder;
                                flw_InputVariables.Controls.Add(helperControl);
                                break;

                            case Core.AutomationCommands.Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowMouseCaptureHelper:
                                helperControl.CommandImage = UI.Images.GetUIImage("SendMouseMoveCommand");
                                helperControl.CommandDisplay = "Capture Mouse Position";
                                helperControl.ForeColor = Color.AliceBlue;
                                helperControl.Click += ShowMouseCaptureForm;
                                flw_InputVariables.Controls.Add(helperControl);
                                break;
                            case Core.AutomationCommands.Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowElementRecorder:
                                //show variable selector
                                helperControl.CommandImage = UI.Images.GetUIImage("ClipboardGetTextCommand");
                                helperControl.CommandDisplay = "Element Recorder";

                                helperControl.Click += ShowElementRecorder;
                                flw_InputVariables.Controls.Add(helperControl);
                                break;
                            case Core.AutomationCommands.Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.GenerateDLLParameters:
                                //show variable selector
                                helperControl.CommandImage = UI.Images.GetUIImage("ExecuteDLLCommand");
                                helperControl.CommandDisplay = "Generate Parameters";
                                helperControl.Click += GenerateDLLParameters;
                                flw_InputVariables.Controls.Add(helperControl);
                                break;
                            case Core.AutomationCommands.Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowDLLExplorer:
                                //show variable selector
                                helperControl.CommandImage = UI.Images.GetUIImage("ExecuteDLLCommand");
                                helperControl.CommandDisplay = "Launch DLL Explorer";
                                helperControl.Click += ShowDLLExplorer;
                                flw_InputVariables.Controls.Add(helperControl);
                                break;
                            case Core.AutomationCommands.Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.AddInputParameter:
                                //show variable selector
                                helperControl.CommandImage = UI.Images.GetUIImage("ExecuteDLLCommand");
                                helperControl.CommandDisplay = "Add Input Parameter";
                                helperControl.Click += AddInputParameter;
                                flw_InputVariables.Controls.Add(helperControl);
                                break;
                            case Core.AutomationCommands.Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowHTMLBuilder:
                                helperControl.CommandImage = UI.Images.GetUIImage("ExecuteDLLCommand");
                                helperControl.CommandDisplay = "Launch HTML Builder";
                                helperControl.Click += ShowHTMLBuilder;
                                flw_InputVariables.Controls.Add(helperControl);
                                break;
                            default:
                                MessageBox.Show("Command Helper does not exist for: " + attrib.additionalHelper.ToString());
                                break;
                        }



                    }

                }

                //add to flow layout
                flw_InputVariables.Controls.Add(inputControl);


                //add additional offset
                this.Height = formHeight + 200;
            }
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
                InputControl.Name = inputField.Name;
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
                else if (inputField.Name == "v_AutomationType")
                {
                    control.SelectedIndexChanged += UIAType_SelectionChangeCommitted;
                }
                else if(inputField.Name == "v_SeleniumSearchType")
                {
                    control.SelectedIndexChanged += seleniumSearchType_SelectionChangeCommitted;
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
                    InputControl.Items.Add("All Windows");
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
                else if(inputField.Name == "v_InputHTML")
                {
                    InputControl = new RichTextBox();
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
                        if (!string.IsNullOrEmpty(scriptVariable.VariableName))
                            InputControl.Items.Add(scriptVariable.VariableName);
                    }

                    InputControl.Width = 350;

                    //if (scriptVariables.Count == 0)
                    //{
                    //    MessageBox.Show("The selected command requires a user variable but no user variables were found! To create a user variable, please select the 'Variables' button on the main form to define a new variable!", "No User-Defined Variables Found");
                    //}


                }
                else if ((inputField.Name == "v_WebActionParameterTable") || (inputField.Name == "v_IfActionParameterTable") || (inputField.Name == "v_MethodParameters"))
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
                    else if (currentCommand is Core.AutomationCommands.ExecuteDLLCommand)
                    {
                        var cmd = (Core.AutomationCommands.ExecuteDLLCommand)currentCommand;
                        InputControl.DataSource = cmd.v_MethodParameters;
                        InputControl.Font = new Font("Segoe UI", 8, FontStyle.Regular);
                    }
                }
                else if(inputField.Name == "v_UserInputConfig")
                {
                                          
                    InputControl = new DataGridView();
                    var dgv = (DataGridView)InputControl;
                    dgv.KeyDown += UserInputDataGridView_KeyDown;

                    InputControl.AllowUserToDeleteRows = true;
                    InputControl.AutoGenerateColumns = false;
                    InputControl.Name = inputField.Name;
                    InputControl.Width = 500;
                    InputControl.Height = 140;
                    

                   var typefield = new DataGridViewComboBoxColumn();
                    typefield.Items.Add("TextBox");
                    typefield.Items.Add("CheckBox");
                    typefield.Items.Add("ComboBox");
                    typefield.HeaderText = "Input Type";
                    typefield.DataPropertyName = "Type";
                    InputControl.Columns.Add(typefield);

                  var field = new DataGridViewTextBoxColumn();
                    field.HeaderText = "Input Label";
                    field.DataPropertyName = "Label";
                    InputControl.Columns.Add(field);


                    field = new DataGridViewTextBoxColumn();
                    field.HeaderText = "Input Size (X,Y)";
                    field.DataPropertyName = "Size";
                    InputControl.Columns.Add(field);

                    field = new DataGridViewTextBoxColumn();
                    field.HeaderText = "Default Value";
                    field.DataPropertyName = "DefaultValue";
                    InputControl.Columns.Add(field);

                    field = new DataGridViewTextBoxColumn();
                    field.HeaderText = "Assigned Variable";
                    field.DataPropertyName = "ApplyToVariable";                
                    InputControl.Columns.Add(field);


                    InputControl.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
                    InputControl.AllowUserToAddRows = false;
                    InputControl.AllowUserToDeleteRows = false;


                    var cmd = (Core.AutomationCommands.UserInputCommand)currentCommand;
                    InputControl.DataSource = cmd.v_UserInputConfig;

                }
                else if(inputField.Name == "v_UIAActionParameters")
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


                    var cmd = (Core.AutomationCommands.UIAutomationCommand)currentCommand;
                    InputControl.DataSource = cmd.v_UIAActionParameters;


                }
                else if (inputField.Name == "v_UIASearchParameters")
                {
                    InputControl = new DataGridView();

                    InputControl.Name = inputField.Name;
                    InputControl.Width = 500;
                    InputControl.Height = 140;

                    DataGridViewCheckBoxColumn enabled = new DataGridViewCheckBoxColumn();
                    enabled.HeaderText = "Enabled";
                    enabled.DataPropertyName = "Enabled";
                    InputControl.Columns.Add(enabled);

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

                    var cmd = (Core.AutomationCommands.UIAutomationCommand)currentCommand;
                    InputControl.DataSource = cmd.v_UIASearchParameters;

                }
                else if (inputField.Name == "v_KeyActions")
                {
                    InputControl = new DataGridView();

                    InputControl.Name = inputField.Name;
                    InputControl.Width = 500;
                    InputControl.Height = 140;

                   

                    DataGridViewComboBoxColumn propertyName = new DataGridViewComboBoxColumn();
                    propertyName.DataSource = Core.Common.GetAvailableKeys();
                    propertyName.HeaderText = "Selected Key";
                    propertyName.DataPropertyName = "Key";
                    InputControl.Columns.Add(propertyName);

                    DataGridViewComboBoxColumn propertyValue = new DataGridViewComboBoxColumn();
                    propertyValue.DataSource = new List<string> { "Key Press (Down + Up)", "Key Down", "Key Up" };
                    propertyValue.HeaderText = "Selected Action";
                    propertyValue.DataPropertyName = "Action";
                    InputControl.Columns.Add(propertyValue);

                    InputControl.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
                    InputControl.AllowUserToAddRows = true;
                    InputControl.AllowUserToDeleteRows = true;


                    var cmd = (Core.AutomationCommands.SendAdvancedKeyStrokesCommand)currentCommand;
                    InputControl.DataSource = cmd.v_KeyActions;


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
                else if (inputField.Name == "v_UIASearchParameters")
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
                    var cmd = (Core.AutomationCommands.UIAutomationCommand)currentCommand;
                    InputControl.DataSource = cmd.v_UIASearchParameters;
                    InputControl.Font = new Font("Segoe UI", 8, FontStyle.Regular);
                }
                else if (inputField.Name == "v_TextExtractionType")
                {
                    InputControl = new ComboBox();
                    InputControl.Height = 30;
                    InputControl.Width = 300;

  
                    ComboBox extractionTypeBox = (ComboBox)InputControl;
                    extractionTypeBox.Items.Add("Extract All After Text");
                    extractionTypeBox.Items.Add("Extract All Before Text");
                    extractionTypeBox.Items.Add("Extract All Between Text");

                    extractionTypeBox.SelectedIndexChanged += textExtraction_SelectionChangeCommitted;
                }
                else if (inputField.Name == "v_TextExtractionTable")
                {
                    InputControl = new DataGridView();

                    InputControl.Name = inputField.Name;
                    InputControl.Width = 400;
                    InputControl.Height = 180;

                    InputControl.AllowUserToAddRows = false;
                  
                    InputControl.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

                    //set datasource
                    var cmd = (Core.AutomationCommands.TextExtractorCommand)currentCommand;
                    InputControl.DataSource = cmd.v_TextExtractionTable;
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
                else if(inputField.Name == "v_ScriptCode")
                {
                    InputControl = new TextBox();
                    InputControl.Height = 100;
                    InputControl.Width = 300;
                    InputControl.Multiline = true;
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

        private void UserInputDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridView inputControl = (DataGridView)flw_InputVariables.Controls["v_UserInputConfig"];

            if (inputControl.SelectedRows.Count > 0)
            {
                inputControl.Rows.RemoveAt(inputControl.SelectedCells[0].RowIndex);
            }

        }

        #endregion Command UI Field Generation

        #region ComboBox Events

        //handles all combobox events
        private void textExtraction_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox extractionAction = (ComboBox)sender;
            DataGridView webActionParameterBox = (DataGridView)flw_InputVariables.Controls["v_TextExtractionTable"];
            Label additionalParameterLabel = (Label)flw_InputVariables.Controls["lbl_v_TextExtractionTable"];

            if ((webActionParameterBox == null) || (extractionAction == null) || (webActionParameterBox.DataSource == null))
                return;

            Core.AutomationCommands.TextExtractorCommand cmd = (Core.AutomationCommands.TextExtractorCommand)selectedCommand;
            DataTable actionParameters = cmd.v_TextExtractionTable;
            actionParameters.Rows.Clear();


            switch (extractionAction.Text)
            {
                case "Extract All After Text":
                    actionParameters.Rows.Add("Leading Text", "");
                    actionParameters.Rows.Add("Skip Past Occurences", "0");
                    break;
                case "Extract All Before Text":
                    actionParameters.Rows.Add("Trailing Text", "");
                    actionParameters.Rows.Add("Skip Past Occurences", "0");
                    break;
                case "Extract All Between Text":
                    actionParameters.Rows.Add("Leading Text", "");
                    actionParameters.Rows.Add("Trailing Text", "");
                    actionParameters.Rows.Add("Skip Past Occurences", "0");
                    break;
                default:
                    break;
            }


        }
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

                    break;
            
                default:
                    break;
            }


        }
        private void ifAction_SelectionChangeCommitted(object sender, EventArgs e)
        {
  

            ComboBox ifAction = (ComboBox)flw_InputVariables.Controls["v_IfActionType"];
            DataGridView ifActionParameterBox = (DataGridView)flw_InputVariables.Controls["v_IfActionParameterTable"];
            Label additionalParameterLabel = (Label)flw_InputVariables.Controls["lbl_v_IfActionParameterTable"];

            if ((ifActionParameterBox == null) || (ifAction == null) || (ifActionParameterBox.DataSource == null))
                return;

            Core.AutomationCommands.BeginIfCommand cmd = (Core.AutomationCommands.BeginIfCommand)selectedCommand;
            DataTable actionParameters = cmd.v_IfActionParameterTable;

            //sender is null when command is updating
            if (sender != null)
                        actionParameters.Rows.Clear();

            DataGridViewComboBoxCell comparisonComboBox = new DataGridViewComboBoxCell();

            //recorder control
            Control recorderControl = (Control)flw_InputVariables.Controls["guirecorder_helper"];

            //remove if exists            
            if (!(recorderControl is null))
            {
                flw_InputVariables.Controls.Remove(recorderControl);

            }


            switch (ifAction.Text)
            {
                case "Value":
                    additionalParameterLabel.Visible = true;
                    ifActionParameterBox.Visible = true;

                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Value1", "");
                        actionParameters.Rows.Add("Operand", "");
                        actionParameters.Rows.Add("Value2", "");
                    }              

                    //combobox cell for Variable Name
                    comparisonComboBox = new DataGridViewComboBoxCell();
                    comparisonComboBox.Items.Add("is equal to");
                    comparisonComboBox.Items.Add("is greater than");
                    comparisonComboBox.Items.Add("is greater than or equal to");
                    comparisonComboBox.Items.Add("is less than");
                    comparisonComboBox.Items.Add("is less than or equal to");
                    comparisonComboBox.Items.Add("is not equal to");

                    //assign cell as a combobox
                    ifActionParameterBox.Rows[1].Cells[1] = comparisonComboBox;

                    break;
                case "Variable Has Value":
                    additionalParameterLabel.Visible = true;
                    ifActionParameterBox.Visible = true;
                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Variable Name", "");
                    }
                       
                    break;
                case "Variable Is Numeric":
                    additionalParameterLabel.Visible = true;
                    ifActionParameterBox.Visible = true;
                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Variable Name", "");
                    }
                      
                    break;
                case "Error Occured":
                    additionalParameterLabel.Visible = true;
                    ifActionParameterBox.Visible = true;
                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Line Number", "");
                    }
                      
                    break;
                case "Error Did Not Occur":
                    additionalParameterLabel.Visible = true;
                    ifActionParameterBox.Visible = true;
  
                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Line Number", "");
                    }

                    break;
                case "Window Name Exists":
                case "Active Window Name Is":
                    additionalParameterLabel.Visible = true;
                    ifActionParameterBox.Visible = true;
                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Window Name", "");
                    }
              
                    break;
                case "File Exists":
                    additionalParameterLabel.Visible = true;
                    ifActionParameterBox.Visible = true;
                    if (sender != null)
                    {
                        actionParameters.Rows.Add("File Path", "");
                        actionParameters.Rows.Add("True When", "");
                    }


                    //combobox cell for Variable Name
                    comparisonComboBox = new DataGridViewComboBoxCell();
                    comparisonComboBox.Items.Add("It Does Exist");
                    comparisonComboBox.Items.Add("It Does Not Exist");

                    //assign cell as a combobox
                    ifActionParameterBox.Rows[1].Cells[1] = comparisonComboBox;

                    break;
                case "Folder Exists":
                    additionalParameterLabel.Visible = true;
                    ifActionParameterBox.Visible = true;


                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Folder Path", "");
                        actionParameters.Rows.Add("True When", "");
                    }

                    //combobox cell for Variable Name
                    comparisonComboBox = new DataGridViewComboBoxCell();
                    comparisonComboBox.Items.Add("It Does Exist");
                    comparisonComboBox.Items.Add("It Does Not Exist");

                    //assign cell as a combobox
                    ifActionParameterBox.Rows[1].Cells[1] = comparisonComboBox;
                    break;
                case "Web Element Exists":
                    additionalParameterLabel.Visible = true;
                    ifActionParameterBox.Visible = true;

                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Selenium Instance Name", "default");
                        actionParameters.Rows.Add("Element Search Method", "");
                        actionParameters.Rows.Add("Element Search Parameter", "");
                    }



                    comparisonComboBox = new DataGridViewComboBoxCell();
                    comparisonComboBox.Items.Add("Find Element By XPath");
                    comparisonComboBox.Items.Add("Find Element By ID");
                    comparisonComboBox.Items.Add("Find Element By Name");
                    comparisonComboBox.Items.Add("Find Element By Tag Name");
                    comparisonComboBox.Items.Add("Find Element By Class Name");
                    comparisonComboBox.Items.Add("Find Element By CSS Selector");

                    //assign cell as a combobox
                    ifActionParameterBox.Rows[1].Cells[1] = comparisonComboBox;

                    break;
                case "GUI Element Exists":

                    additionalParameterLabel.Visible = true;
                    ifActionParameterBox.Visible = true;
                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Window Name", "Current Window");
                        actionParameters.Rows.Add("Element Search Method", "");
                        actionParameters.Rows.Add("Element Search Parameter", "");
                    }



                    var parameterName = new DataGridViewComboBoxCell();
                    parameterName.Items.Add("AcceleratorKey");
                    parameterName.Items.Add("AccessKey");
                    parameterName.Items.Add("AutomationId");
                    parameterName.Items.Add("ClassName");
                    parameterName.Items.Add("FrameworkId");
                    parameterName.Items.Add("HasKeyboardFocus");
                    parameterName.Items.Add("HelpText");
                    parameterName.Items.Add("IsContentElement");
                    parameterName.Items.Add("IsControlElement");
                    parameterName.Items.Add("IsEnabled");
                    parameterName.Items.Add("IsKeyboardFocusable");
                    parameterName.Items.Add("IsOffscreen");
                    parameterName.Items.Add("IsPassword");
                    parameterName.Items.Add("IsRequiredForForm");
                    parameterName.Items.Add("ItemStatus");
                    parameterName.Items.Add("ItemType");
                    parameterName.Items.Add("LocalizedControlType");
                    parameterName.Items.Add("Name");
                    parameterName.Items.Add("NativeWindowHandle");
                    parameterName.Items.Add("ProcessID");

                    //assign cell as a combobox
                    ifActionParameterBox.Rows[1].Cells[1] = parameterName;

                    taskt.UI.CustomControls.CommandItemControl helperControl = new taskt.UI.CustomControls.CommandItemControl();
                    helperControl.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
                    helperControl.ForeColor = Color.AliceBlue;
                    helperControl.Name = "guirecorder_helper";
                    helperControl.CommandImage = UI.Images.GetUIImage("ClipboardGetTextCommand");
                    helperControl.CommandDisplay = "Element Recorder";
                    helperControl.Click += ShowIfElementRecorder;

                    var ifBoxIndex = flw_InputVariables.Controls.IndexOf(ifActionParameterBox);
                    flw_InputVariables.Controls.Add(helperControl);
                    flw_InputVariables.Controls.SetChildIndex(helperControl, ifBoxIndex);


                    break;

                default:
                    break;
            }
        }
        private void seleniumSearchType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox seleniumSearchType = (ComboBox)sender;

            ComboBox seleniumActionBox = (ComboBox)flw_InputVariables.Controls["v_SeleniumElementAction"];

            if (seleniumActionBox == null)
                return;

            seleniumActionBox.Items.Clear();

            if (seleniumSearchType.Text.Contains("Elements"))
            {
                seleniumActionBox.Items.Add("Get Matching Elements");
            }
            else
            {
                seleniumActionBox.Items.Add("Invoke Click");
                seleniumActionBox.Items.Add("Left Click");
                seleniumActionBox.Items.Add("Right Click");
                seleniumActionBox.Items.Add("Middle Click");
                seleniumActionBox.Items.Add("Double Left Click");
                seleniumActionBox.Items.Add("Clear Element");
                seleniumActionBox.Items.Add("Set Text");
                seleniumActionBox.Items.Add("Get Text");
                seleniumActionBox.Items.Add("Get Attribute");
                seleniumActionBox.Items.Add("Wait For Element To Exist");

            }
        }

        private void seleniumAction_SelectionChangeCommitted(object sender, EventArgs e)
        {

            ComboBox webAction = (ComboBox)flw_InputVariables.Controls["v_SeleniumElementAction"];
            DataGridView webActionParameterBox = (DataGridView)flw_InputVariables.Controls["v_WebActionParameterTable"];
            Label additionalParameterLabel = (Label)flw_InputVariables.Controls["lbl_v_WebActionParameterTable"];
            CustomControls.CommandItemControl variableHelper = (CustomControls.CommandItemControl)flw_InputVariables.Controls["lbl_v_WebActionParameterTable_helper"];

            if ((webActionParameterBox == null) || (webAction == null) || (webActionParameterBox.DataSource == null))
                return;

            Core.AutomationCommands.SeleniumBrowserElementActionCommand cmd = (Core.AutomationCommands.SeleniumBrowserElementActionCommand)selectedCommand;
            DataTable actionParameters = cmd.v_WebActionParameterTable;

            if (sender != null)
            {
                actionParameters.Rows.Clear();
            }
         

            switch (webAction.Text)
            {
                case "Invoke Click":
                case "Clear Element":
                    webActionParameterBox.Hide();
                    additionalParameterLabel.Hide();
                    variableHelper.Hide();
                    break;

                case "Left Click":
                case "Middle Click":
                case "Right Click":
                case "Double Left Click":
                    webActionParameterBox.Show();
                    additionalParameterLabel.Show();
                    variableHelper.Show();
                    if (sender != null)
                    {
                        actionParameters.Rows.Add("X Adjustment", 0);
                        actionParameters.Rows.Add("Y Adjustment", 0);
                    }
                    break;

                case "Set Text":
                    webActionParameterBox.Show();
                    additionalParameterLabel.Show();
                    variableHelper.Show();
                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Text To Set");
                        actionParameters.Rows.Add("Clear Element Before Setting Text");
                    }

                    DataGridViewComboBoxCell comparisonComboBox = new DataGridViewComboBoxCell();
                    comparisonComboBox.Items.Add("Yes");
                    comparisonComboBox.Items.Add("No");

                    //assign cell as a combobox
                    if (sender != null)
                    {
                        webActionParameterBox.Rows[1].Cells[1].Value = "No";
                    }
                    webActionParameterBox.Rows[1].Cells[1] = comparisonComboBox;


                    break;

                case "Get Text":
                case "Get Matching Elements":
                    webActionParameterBox.Show();
                    additionalParameterLabel.Show();
                    variableHelper.Show();
                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Variable Name");
                    }
                        break;

                case "Get Attribute":
                    webActionParameterBox.Show();
                    additionalParameterLabel.Show();
                    variableHelper.Show();
                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Attribute Name");
                        actionParameters.Rows.Add("Variable Name");
                    }
                    break;

                case "Wait For Element To Exist":
                    webActionParameterBox.Show();
                    additionalParameterLabel.Show();
                    variableHelper.Show();
                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Timeout (Seconds)");
                    }
                        break;
                default:
                    break;
            }
        }
        private void UIAType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //ComboBox selectedAction = (ComboBox)sender;
            ComboBox selectedAction = (ComboBox)flw_InputVariables.Controls["v_AutomationType"];

            if (selectedAction == null)
                return;

        //DataGridView searchParameters = (DataGridView)flw_InputVariables.Controls["v_UIASearchParameters"];
        DataGridView actionParameterView = (DataGridView)flw_InputVariables.Controls["v_UIAActionParameters"];

            if (actionParameterView == null)
                return;


            Core.AutomationCommands.UIAutomationCommand cmd = (Core.AutomationCommands.UIAutomationCommand)selectedCommand;
            DataTable actionParameters = cmd.v_UIAActionParameters;

            if (sender != null)
            {
                actionParameters.Rows.Clear();
            }


            if (selectedAction.Text == "Click Element")
            {

                var mouseClickBox = new DataGridViewComboBoxCell();
                mouseClickBox.Items.Add("Left Click");
                mouseClickBox.Items.Add("Middle Click");
                mouseClickBox.Items.Add("Right Click");
                mouseClickBox.Items.Add("Left Down");
                mouseClickBox.Items.Add("Middle Down");
                mouseClickBox.Items.Add("Right Down");
                mouseClickBox.Items.Add("Left Up");
                mouseClickBox.Items.Add("Middle Up");
                mouseClickBox.Items.Add("Right Up");


                if (sender != null)
                {
                    actionParameters.Rows.Add("Click Type", "");
                    actionParameters.Rows.Add("X Adjustment", 0);
                    actionParameters.Rows.Add("Y Adjustment", 0);
                }
                actionParameterView.Rows[0].Cells[1] = mouseClickBox;

            }
            else if(selectedAction.Text == "Check If Element Exists")
            {

                if (sender != null)
                {
                    actionParameters.Rows.Add("Apply To Variable", "");
                }
               
            }
            else
            {
                var parameterName = new DataGridViewComboBoxCell();
                parameterName.Items.Add("AcceleratorKey");
                parameterName.Items.Add("AccessKey");
                parameterName.Items.Add("AutomationId");
                parameterName.Items.Add("ClassName");
                parameterName.Items.Add("FrameworkId");
                parameterName.Items.Add("HasKeyboardFocus");
                parameterName.Items.Add("HelpText");
                parameterName.Items.Add("IsContentElement");
                parameterName.Items.Add("IsControlElement");
                parameterName.Items.Add("IsEnabled");
                parameterName.Items.Add("IsKeyboardFocusable");
                parameterName.Items.Add("IsOffscreen");
                parameterName.Items.Add("IsPassword");
                parameterName.Items.Add("IsRequiredForForm");
                parameterName.Items.Add("ItemStatus");
                parameterName.Items.Add("ItemType");
                parameterName.Items.Add("LocalizedControlType");
                parameterName.Items.Add("Name");
                parameterName.Items.Add("NativeWindowHandle");
                parameterName.Items.Add("ProcessID");


                if (sender != null)
                {
                    actionParameters.Rows.Add("Get Value From", "");
                    actionParameters.Rows.Add("Apply To Variable", "");
                }

             
                actionParameterView.Rows[0].Cells[1] = parameterName;

 
            }


        }
        private void cboSelectedCommand_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //find underlying command class and generate the required items on the UI flow layout for configuration
            var selectedCommandItem = cboSelectedCommand.Text;
            selectedCommand = commandList.Where(itm => itm.DisplayValue == selectedCommandItem).FirstOrDefault().CommandInstance;
            GenerateUIInputElements(selectedCommand);
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

                    try
                    {
                        var handleList = newAppCommand.FindHandleObjects(senderBox.Text);
                        handleControl.DataSource = handleList;
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show("Error Occured: " + ex.ToString());
                    }
                   

                   
                    
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
                cmd.v_XMousePosition = frmShowCursorPos.xPos.ToString();
                cmd.v_YMousePosition = frmShowCursorPos.yPos.ToString();
            }
        }
        private void ShowCodeBuilder(object sender, EventArgs e)
        {
            //get textbox text
            CustomControls.CommandItemControl commandItem = (CustomControls.CommandItemControl)sender;
            TextBox targetTextbox = (TextBox)commandItem.Tag;


            UI.Forms.Supplemental.frmCodeBuilder codeBuilder = new Supplemental.frmCodeBuilder(targetTextbox.Text);

            if (codeBuilder.ShowDialog() == DialogResult.OK)
            {

                targetTextbox.Text = codeBuilder.rtbCode.Text;
            }




        }
        private void ShowVariableSelector(object sender, EventArgs e)
        {
            //create variable selector form
            UI.Forms.Supplemental.frmVariableSelector newVariableSelector = new Supplemental.frmVariableSelector();

            //get copy of user variables and append system variables, then load to combobox
            var variableList = scriptVariables.Select(f => f.VariableName).ToList();
            variableList.AddRange(Core.Common.GenerateSystemVariables().Select(f => f.VariableName));
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

                //load settings
                var settings = new ApplicationSettings().GetOrCreateApplicationSettings();

                if (inputBox.Tag is TextBox)
                {
                    TextBox targetTextbox = (TextBox)inputBox.Tag;
                    //concat variable name with brackets [vVariable] as engine searches for the same
                    targetTextbox.Text = targetTextbox.Text + string.Concat(settings.EngineSettings.VariableStartMarker, newVariableSelector.lstVariables.SelectedItem.ToString(), settings.EngineSettings.VariableEndMarker);
                }
                else if(inputBox.Tag is ComboBox)
                {
                    ComboBox targetCombobox = (ComboBox)inputBox.Tag;
                    //concat variable name with brackets [vVariable] as engine searches for the same
                    targetCombobox.Text = targetCombobox.Text + string.Concat(settings.EngineSettings.VariableStartMarker, newVariableSelector.lstVariables.SelectedItem.ToString(), settings.EngineSettings.VariableEndMarker);
                }
                else if(inputBox.Tag is DataGridView)
                {
                    DataGridView targetDGV = (DataGridView)inputBox.Tag;

                    if (targetDGV.SelectedCells.Count == 0)
                    {
                        MessageBox.Show("Please make sure you have selected an action and selected a cell before attempting to insert a variable!", "No Cell Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (targetDGV.SelectedCells[0].ColumnIndex == 0)
                    {
                        MessageBox.Show("Invalid Cell Selected!", "Invalid Cell Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    targetDGV.SelectedCells[0].Value = targetDGV.SelectedCells[0].Value + string.Concat(settings.EngineSettings.VariableStartMarker, newVariableSelector.lstVariables.SelectedItem.ToString(), settings.EngineSettings.VariableEndMarker);
                }
           

            }
        }
        private void GenerateDLLParameters(object sender, EventArgs e)
        {
            Core.AutomationCommands.ExecuteDLLCommand cmd = (Core.AutomationCommands.ExecuteDLLCommand)selectedCommand;

            var filePath = flw_InputVariables.Controls["v_FilePath"].Text;
            var className = flw_InputVariables.Controls["v_ClassName"].Text;
            var methodName = flw_InputVariables.Controls["v_MethodName"].Text;
            DataGridView parameterBox = (DataGridView)flw_InputVariables.Controls["v_MethodParameters"];

            //clear all rows
            cmd.v_MethodParameters.Rows.Clear();

            //Load Assembly
            try
            {
                Assembly requiredAssembly = Assembly.LoadFrom(filePath);

                //get type
                Type t = requiredAssembly.GetType(className);

                //verify type was found
                if (t == null)
                {
                    MessageBox.Show("The class '" + className + "' was not found in assembly loaded at '" + filePath + "'", "Class Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                //get method
                MethodInfo m = t.GetMethod(methodName);

                //verify method found
                if (m == null)
                {
                    MessageBox.Show("The method '" + methodName + "' was not found in assembly loaded at '" + filePath + "'", "Method Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                //get parameters
                var reqdParams = m.GetParameters();

                if (reqdParams.Length > 0)
                {
                    cmd.v_MethodParameters.Rows.Clear();
                    foreach (var param in reqdParams)
                    {
                        cmd.v_MethodParameters.Rows.Add(param.Name, "");
                    }
                }
                else
                {
                    MessageBox.Show("There are no parameters required for this method!", "No Parameters Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                      
                }
             
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error generating the parameters: " + ex.ToString());
            }
        


        }
        private void ShowHTMLBuilder(object sender, EventArgs e)
        {
            var htmlForm = new Supplemental.frmHTMLBuilder();

            RichTextBox inputControl = (RichTextBox)flw_InputVariables.Controls["v_InputHTML"];
            htmlForm.rtbHTML.Text = inputControl.Text;

            if (htmlForm.ShowDialog() == DialogResult.OK)
            {
                inputControl.Text = htmlForm.rtbHTML.Text;
            }

        }
        private void AddInputParameter(object sender, EventArgs e)
        {

            DataGridView inputControl = (DataGridView)flw_InputVariables.Controls["v_UserInputConfig"];
            var inputTable = (DataTable)inputControl.DataSource;
            var newRow = inputTable.NewRow();
            newRow["Size"] = "500,100";
            inputTable.Rows.Add(newRow);

        }
           private void ShowDLLExplorer(object sender, EventArgs e)
        {
            //create form
            Supplemental.frmDLLExplorer dllExplorer = new Supplemental.frmDLLExplorer();

            //show dialog
            if (dllExplorer.ShowDialog() == DialogResult.OK)
            {
                //user accepted the selections
                //declare command
                Core.AutomationCommands.ExecuteDLLCommand cmd = (Core.AutomationCommands.ExecuteDLLCommand)selectedCommand;

                //add file name
                if (!string.IsNullOrEmpty(dllExplorer.FileName))
                {
                    flw_InputVariables.Controls["v_FilePath"].Text = dllExplorer.FileName;
                }

                //add class name
                if (dllExplorer.lstClasses.SelectedItem != null)
                {
                    flw_InputVariables.Controls["v_ClassName"].Text = dllExplorer.lstClasses.SelectedItem.ToString();
                }

                //add method name
                if (dllExplorer.lstMethods.SelectedItem != null)
                {
                    flw_InputVariables.Controls["v_MethodName"].Text = dllExplorer.lstMethods.SelectedItem.ToString();
                }

                //add parameters
                if ((dllExplorer.lstParameters.Items.Count > 0) && (dllExplorer.lstParameters.Items[0].ToString() != "This method requires no parameters!"))
                {
                    foreach (var param in dllExplorer.SelectedParameters)
                    {
                        cmd.v_MethodParameters.Rows.Add(param, "");
                    }
                }

           

            }

        }
        private void ShowElementRecorder(object sender, EventArgs e)
        {

            //get command reference
            Core.AutomationCommands.UIAutomationCommand cmd = (Core.AutomationCommands.UIAutomationCommand)selectedCommand;
           
            //create recorder
            UI.Forms.Supplemental.frmThickAppElementRecorder newElementRecorder = new Supplemental.frmThickAppElementRecorder();
            newElementRecorder.searchParameters = cmd.v_UIASearchParameters;

            //show form
            newElementRecorder.ShowDialog();

            ComboBox txtWindowName = (ComboBox)flw_InputVariables.Controls["v_WindowName"];
            txtWindowName.Text = newElementRecorder.cboWindowTitle.Text;

            this.WindowState = FormWindowState.Normal;
            this.BringToFront();



        }
        private void ShowIfElementRecorder(object sender, EventArgs e)
        {

            //get command reference
            Core.AutomationCommands.UIAutomationCommand cmd = new Core.AutomationCommands.UIAutomationCommand();

            //create recorder
            UI.Forms.Supplemental.frmThickAppElementRecorder newElementRecorder = new Supplemental.frmThickAppElementRecorder();
            newElementRecorder.searchParameters = cmd.v_UIASearchParameters;

            //show form
            newElementRecorder.ShowDialog();

            this.WindowState = FormWindowState.Normal;
            this.BringToFront();

            var sb = new StringBuilder();
            sb.AppendLine("Element Properties Found!");
            sb.AppendLine(Environment.NewLine);
            sb.AppendLine("Element Search Method - Element Search Parameter");
            foreach (DataRow rw in cmd.v_UIASearchParameters.Rows)
            {
                if (rw.ItemArray[2].ToString().Trim() == string.Empty)
                    continue;

                sb.AppendLine(rw.ItemArray[1].ToString() + " - " + rw.ItemArray[2].ToString());
            }

            DataGridView ifActionBox = (DataGridView)flw_InputVariables.Controls["v_IfActionParameterTable"];
            ifActionBox.Rows[0].Cells[1].Value = newElementRecorder.cboWindowTitle.Text;


            MessageBox.Show(sb.ToString());




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
        private void frmCommandEditor_Resize(object sender, EventArgs e)
        {
            foreach (Control item in flw_InputVariables.Controls)
            {
                item.Width = this.Width - 50;
            }

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