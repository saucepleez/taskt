using System;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using taskt.Core.Automation.User32;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
using System.Drawing;
using System.Text;

namespace taskt.Core.Automation.Commands
{




    [Serializable]
    [Attributes.ClassAttributes.Group("If Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to evaluate a logical statement to determine if the statement is true.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to check if a statement is 'true' or 'false' and subsequently take an action based on either condition. Any 'BeginIf' command must have a following 'EndIf' command.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command evaluates supplied arguments and if evaluated to true runs sub elements")]
    public class BeginIfCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select type of If Command")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Value")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Date Compare")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Variable Compare")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Variable Has Value")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Variable Is Numeric")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Window Name Exists")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Active Window Name Is")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("File Exists")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Folder Exists")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Web Element Exists")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("GUI Element Exists")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Error Occured")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Error Did Not Occur")]
        [Attributes.PropertyAttributes.InputSpecification("Select the necessary comparison type.")]
        [Attributes.PropertyAttributes.SampleUsage("Select **Value**, **Window Name Exists**, **Active Window Name Is**, **File Exists**, **Folder Exists**, **Web Element Exists**, **Error Occured**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_IfActionType { get; set; }

        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Additional Parameters")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select the required comparison parameters.")]
        [Attributes.PropertyAttributes.SampleUsage("n/a")]
        [Attributes.PropertyAttributes.Remarks("")]
        public DataTable v_IfActionParameterTable { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private DataGridView IfGridViewHelper;

        [XmlIgnore]
        [NonSerialized]
        private ComboBox ActionDropdown;

        [XmlIgnore]
        [NonSerialized]
        private List<Control> ParameterControls;

        [XmlIgnore]
        [NonSerialized]
        CommandItemControl RecorderControl;
        public BeginIfCommand()
        {
            this.CommandName = "BeginIfCommand";
            this.SelectionName = "Begin If";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            //define parameter table
            this.v_IfActionParameterTable = new System.Data.DataTable
            {
                TableName = DateTime.Now.ToString("IfActionParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"))
            };
            this.v_IfActionParameterTable.Columns.Add("Parameter Name");
            this.v_IfActionParameterTable.Columns.Add("Parameter Value");

            IfGridViewHelper = new DataGridView();
            IfGridViewHelper.AllowUserToAddRows = true;
            IfGridViewHelper.AllowUserToDeleteRows = true;
            IfGridViewHelper.Size = new Size(400, 250);
            IfGridViewHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            IfGridViewHelper.DataBindings.Add("DataSource", this, "v_IfActionParameterTable", false, DataSourceUpdateMode.OnPropertyChanged);
            IfGridViewHelper.AllowUserToAddRows = false;
            IfGridViewHelper.AllowUserToDeleteRows = false;
            IfGridViewHelper.MouseEnter += IfGridViewHelper_MouseEnter;

            RecorderControl = new taskt.UI.CustomControls.CommandItemControl();
            RecorderControl.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            RecorderControl.ForeColor = Color.AliceBlue;
            RecorderControl.Name = "guirecorder_helper";
            RecorderControl.CommandImage = UI.Images.GetUIImage("ClipboardGetTextCommand");
            RecorderControl.CommandDisplay = "Element Recorder";
            RecorderControl.Click += ShowIfElementRecorder;
            RecorderControl.Hide();
           



        }

        private void IfGridViewHelper_MouseEnter(object sender, EventArgs e)
        {
            ifAction_SelectionChangeCommitted(null, null);
        }

        public override void RunCommand(object sender, Core.Script.ScriptAction parentCommand)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            var ifResult = DetermineStatementTruth(sender);

            int startIndex, endIndex, elseIndex;
            if (parentCommand.AdditionalScriptCommands.Any(item => item.ScriptCommand is Core.Automation.Commands.ElseCommand))
            {
                elseIndex = parentCommand.AdditionalScriptCommands.FindIndex(a => a.ScriptCommand is Core.Automation.Commands.ElseCommand);

                if (ifResult)
                {
                    startIndex = 0;
                    endIndex = elseIndex;
                }
                else
                {
                    startIndex = elseIndex + 1;
                    endIndex = parentCommand.AdditionalScriptCommands.Count;
                }
            }
            else if (ifResult)
            {
                startIndex = 0;
                endIndex = parentCommand.AdditionalScriptCommands.Count;
            }
            else
            {
                return;
            }

            for (int i = startIndex; i < endIndex; i++)
            {
                if ((engine.IsCancellationPending) || (engine.CurrentLoopCancelled) || (engine.CurrentLoopContinuing))
                    return;

                engine.ExecuteCommand(parentCommand.AdditionalScriptCommands[i]);
            }

        }
        public bool DetermineStatementTruth(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            bool ifResult = false;

            if (v_IfActionType == "Value")
            {
                string value1 = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                  where rw.Field<string>("Parameter Name") == "Value1"
                                  select rw.Field<string>("Parameter Value")).FirstOrDefault());
                string operand = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                   where rw.Field<string>("Parameter Name") == "Operand"
                                   select rw.Field<string>("Parameter Value")).FirstOrDefault());
                string value2 = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                  where rw.Field<string>("Parameter Name") == "Value2"
                                  select rw.Field<string>("Parameter Value")).FirstOrDefault());

                value1 = value1.ConvertToUserVariable(sender);
                value2 = value2.ConvertToUserVariable(sender);



                decimal cdecValue1, cdecValue2;

                switch (operand)
                {
                    case "is equal to":
                        ifResult = (value1 == value2);
                        break;

                    case "is not equal to":
                        ifResult = (value1 != value2);
                        break;

                    case "is greater than":
                        cdecValue1 = Convert.ToDecimal(value1);
                        cdecValue2 = Convert.ToDecimal(value2);
                        ifResult = (cdecValue1 > cdecValue2);
                        break;

                    case "is greater than or equal to":
                        cdecValue1 = Convert.ToDecimal(value1);
                        cdecValue2 = Convert.ToDecimal(value2);
                        ifResult = (cdecValue1 >= cdecValue2);
                        break;

                    case "is less than":
                        cdecValue1 = Convert.ToDecimal(value1);
                        cdecValue2 = Convert.ToDecimal(value2);
                        ifResult = (cdecValue1 < cdecValue2);
                        break;

                    case "is less than or equal to":
                        cdecValue1 = Convert.ToDecimal(value1);
                        cdecValue2 = Convert.ToDecimal(value2);
                        ifResult = (cdecValue1 <= cdecValue2);
                        break;
                }
            }
            else if (v_IfActionType == "Date Compare")
            {
                string value1 = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                  where rw.Field<string>("Parameter Name") == "Value1"
                                  select rw.Field<string>("Parameter Value")).FirstOrDefault());
                string operand = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                   where rw.Field<string>("Parameter Name") == "Operand"
                                   select rw.Field<string>("Parameter Value")).FirstOrDefault());
                string value2 = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                  where rw.Field<string>("Parameter Name") == "Value2"
                                  select rw.Field<string>("Parameter Value")).FirstOrDefault());

                value1 = value1.ConvertToUserVariable(sender);
                value2 = value2.ConvertToUserVariable(sender);



                DateTime dt1, dt2;
                dt1 = DateTime.Parse(value1);
                dt2 = DateTime.Parse(value2);
                switch (operand)
                {
                    case "is equal to":
                        ifResult = (dt1 == dt2);
                        break;

                    case "is not equal to":
                        ifResult = (dt1 != dt2);
                        break;

                    case "is greater than":
                        ifResult = (dt1 > dt2);
                        break;

                    case "is greater than or equal to":
                        ifResult = (dt1 >= dt2);
                        break;

                    case "is less than":
                        ifResult = (dt1 < dt2);
                        break;

                    case "is less than or equal to":
                        ifResult = (dt1 <= dt2);
                        break;
                }
            }
            else if (v_IfActionType == "Variable Compare")
            {
                string value1 = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                  where rw.Field<string>("Parameter Name") == "Value1"
                                  select rw.Field<string>("Parameter Value")).FirstOrDefault());
                string operand = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                   where rw.Field<string>("Parameter Name") == "Operand"
                                   select rw.Field<string>("Parameter Value")).FirstOrDefault());
                string value2 = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                  where rw.Field<string>("Parameter Name") == "Value2"
                                  select rw.Field<string>("Parameter Value")).FirstOrDefault());

                string caseSensitive = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                         where rw.Field<string>("Parameter Name") == "Case Sensitive"
                                         select rw.Field<string>("Parameter Value")).FirstOrDefault());

                value1 = value1.ConvertToUserVariable(sender);
                value2 = value2.ConvertToUserVariable(sender);

                if (caseSensitive == "No")
                {
                    value1 = value1.ToUpper();
                    value2 = value2.ToUpper();
                }



                switch (operand)
                {
                    case "contains":
                        ifResult = (value1.Contains(value2));
                        break;

                    case "does not contain":
                        ifResult = (!value1.Contains(value2));
                        break;

                    case "is equal to":
                        ifResult = (value1 == value2);
                        break;

                    case "is not equal to":
                        ifResult = (value1 != value2);
                        break;
                }
            }
            else if (v_IfActionType == "Variable Has Value")
            {
                string variableName = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                        where rw.Field<string>("Parameter Name") == "Variable Name"
                                        select rw.Field<string>("Parameter Value")).FirstOrDefault());

                var actualVariable = variableName.ConvertToUserVariable(sender).Trim();

                if (!string.IsNullOrEmpty(actualVariable))
                {
                    ifResult = true;
                }
                else
                {
                    ifResult = false;
                }

            }
            else if (v_IfActionType == "Variable Is Numeric")
            {
                string variableName = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                        where rw.Field<string>("Parameter Name") == "Variable Name"
                                        select rw.Field<string>("Parameter Value")).FirstOrDefault());

                var actualVariable = variableName.ConvertToUserVariable(sender).Trim();

                var numericTest = decimal.TryParse(actualVariable, out decimal parsedResult);

                if (numericTest)
                {
                    ifResult = true;
                }
                else
                {
                    ifResult = false;
                }

            }
            else if (v_IfActionType == "Error Occured")
            {
                //get line number
                string userLineNumber = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                          where rw.Field<string>("Parameter Name") == "Line Number"
                                          select rw.Field<string>("Parameter Value")).FirstOrDefault());

                //convert to variable
                string variableLineNumber = userLineNumber.ConvertToUserVariable(sender);

                //convert to int
                int lineNumber = int.Parse(variableLineNumber);

                //determine if error happened
                if (engine.ErrorsOccured.Where(f => f.LineNumber == lineNumber).Count() > 0)
                {

                    var error = engine.ErrorsOccured.Where(f => f.LineNumber == lineNumber).FirstOrDefault();
                    error.ErrorMessage.StoreInUserVariable(sender, "Error.Message");
                    error.LineNumber.ToString().StoreInUserVariable(sender, "Error.Line");
                    error.StackTrace.StoreInUserVariable(sender, "Error.StackTrace");

                    ifResult = true;
                }
                else
                {
                    ifResult = false;
                }

            }
            else if (v_IfActionType == "Error Did Not Occur")
            {
                //get line number
                string userLineNumber = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                          where rw.Field<string>("Parameter Name") == "Line Number"
                                          select rw.Field<string>("Parameter Value")).FirstOrDefault());

                //convert to variable
                string variableLineNumber = userLineNumber.ConvertToUserVariable(sender);

                //convert to int
                int lineNumber = int.Parse(variableLineNumber);

                //determine if error happened
                if (engine.ErrorsOccured.Where(f => f.LineNumber == lineNumber).Count() == 0)
                {
                    ifResult = true;
                }
                else
                {
                    var error = engine.ErrorsOccured.Where(f => f.LineNumber == lineNumber).FirstOrDefault();
                    error.ErrorMessage.StoreInUserVariable(sender, "Error.Message");
                    error.LineNumber.ToString().StoreInUserVariable(sender, "Error.Line");
                    error.StackTrace.StoreInUserVariable(sender, "Error.StackTrace");

                    ifResult = false;
                }

            }
            else if (v_IfActionType == "Window Name Exists")
            {
                //get user supplied name
                string windowName = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                      where rw.Field<string>("Parameter Name") == "Window Name"
                                      select rw.Field<string>("Parameter Value")).FirstOrDefault());
                //variable translation
                string variablizedWindowName = windowName.ConvertToUserVariable(sender);

                //search for window
                IntPtr windowPtr = User32Functions.FindWindow(variablizedWindowName);

                //conditional
                if (windowPtr != IntPtr.Zero)
                {
                    ifResult = true;
                }



            }
            else if (v_IfActionType == "Active Window Name Is")
            {
                string windowName = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                      where rw.Field<string>("Parameter Name") == "Window Name"
                                      select rw.Field<string>("Parameter Value")).FirstOrDefault());

                string variablizedWindowName = windowName.ConvertToUserVariable(sender);

                var currentWindowTitle = User32Functions.GetActiveWindowTitle();

                if (currentWindowTitle == variablizedWindowName)
                {
                    ifResult = true;
                }

            }
            else if (v_IfActionType == "File Exists")
            {

                string fileName = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                    where rw.Field<string>("Parameter Name") == "File Path"
                                    select rw.Field<string>("Parameter Value")).FirstOrDefault());

                string trueWhenFileExists = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                              where rw.Field<string>("Parameter Name") == "True When"
                                              select rw.Field<string>("Parameter Value")).FirstOrDefault());

                var userFileSelected = fileName.ConvertToUserVariable(sender);

                bool existCheck = false;
                if (trueWhenFileExists == "It Does Exist")
                {
                    existCheck = true;
                }


                if (System.IO.File.Exists(userFileSelected) == existCheck)
                {
                    ifResult = true;
                }


            }
            else if (v_IfActionType == "Folder Exists")
            {
                string folderName = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                      where rw.Field<string>("Parameter Name") == "Folder Path"
                                      select rw.Field<string>("Parameter Value")).FirstOrDefault());

                string trueWhenFileExists = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                              where rw.Field<string>("Parameter Name") == "True When"
                                              select rw.Field<string>("Parameter Value")).FirstOrDefault());

                var userFolderSelected = folderName.ConvertToUserVariable(sender);

                bool existCheck = false;
                if (trueWhenFileExists == "It Does Exist")
                {
                    existCheck = true;
                }


                if (System.IO.Directory.Exists(folderName) == existCheck)
                {
                    ifResult = true;
                }

            }
            else if (v_IfActionType == "Web Element Exists")
            {
                string instanceName = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                        where rw.Field<string>("Parameter Name") == "Selenium Instance Name"
                                        select rw.Field<string>("Parameter Value")).FirstOrDefault());

                string parameterName = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                         where rw.Field<string>("Parameter Name") == "Element Search Parameter"
                                         select rw.Field<string>("Parameter Value")).FirstOrDefault());

                string searchMethod = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                        where rw.Field<string>("Parameter Name") == "Element Search Method"
                                        select rw.Field<string>("Parameter Value")).FirstOrDefault());


                SeleniumBrowserElementActionCommand newElementActionCommand = new SeleniumBrowserElementActionCommand();
                newElementActionCommand.v_SeleniumSearchType = searchMethod;
                newElementActionCommand.v_InstanceName = instanceName.ConvertToUserVariable(sender);
                bool elementExists = newElementActionCommand.ElementExists(sender, searchMethod, parameterName);
                ifResult = elementExists;

            }
            else if (v_IfActionType == "GUI Element Exists")
            {
                string windowName = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                      where rw.Field<string>("Parameter Name") == "Window Name"
                                      select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertToUserVariable(sender));

                string elementSearchParam = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                              where rw.Field<string>("Parameter Name") == "Element Search Parameter"
                                              select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertToUserVariable(sender));

                string elementSearchMethod = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                               where rw.Field<string>("Parameter Name") == "Element Search Method"
                                               select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertToUserVariable(sender));


                UIAutomationCommand newUIACommand = new UIAutomationCommand();
                newUIACommand.v_WindowName = windowName;
                newUIACommand.v_UIASearchParameters.Rows.Add(true, elementSearchMethod, elementSearchParam);
                var handle = newUIACommand.SearchForGUIElement(sender, windowName);

                if (handle is null)
                {
                    ifResult = false;
                }
                else
                {
                    ifResult = true;
                }


            }
            else
            {
                throw new Exception("If type not recognized!");
            }

            return ifResult;
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);


            ActionDropdown = (ComboBox)CommandControls.CreateDropdownFor("v_IfActionType", this);
            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_IfActionType", this));
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_IfActionType", this, new Control[] { ActionDropdown }, editor));
            ActionDropdown.SelectionChangeCommitted += ifAction_SelectionChangeCommitted;

            RenderedControls.Add(ActionDropdown);



            ParameterControls = new List<Control>();
            ParameterControls.Add(CommandControls.CreateDefaultLabelFor("v_IfActionParameterTable", this));
            ParameterControls.Add(RecorderControl);
            ParameterControls.AddRange(CommandControls.CreateUIHelpersFor("v_IfActionParameterTable", this, new Control[] { IfGridViewHelper }, editor));
            ParameterControls.Add(IfGridViewHelper);


            RenderedControls.AddRange(ParameterControls);





            return RenderedControls;
        }
        private void ifAction_SelectionChangeCommitted(object sender, EventArgs e)
        {


            ComboBox ifAction = (ComboBox)ActionDropdown;
            DataGridView ifActionParameterBox = (DataGridView)IfGridViewHelper;
           



            Core.Automation.Commands.BeginIfCommand cmd = (Core.Automation.Commands.BeginIfCommand)this;
            DataTable actionParameters = cmd.v_IfActionParameterTable;

            //sender is null when command is updating
            if (sender != null)
                actionParameters.Rows.Clear();

            DataGridViewComboBoxCell comparisonComboBox = new DataGridViewComboBoxCell();

            //recorder control
            Control recorderControl = (Control)RecorderControl;

            //remove if exists            
            if (RecorderControl.Visible)
            {
                RecorderControl.Hide();
            }


            switch (ifAction.SelectedItem)
            {
                case "Value":
                case "Date Compare":

                    ifActionParameterBox.Visible = true;

                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Value1", "");
                        actionParameters.Rows.Add("Operand", "");
                        actionParameters.Rows.Add("Value2", "");
                        ifActionParameterBox.DataSource = actionParameters;
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
                case "Variable Compare":
   
                    ifActionParameterBox.Visible = true;

                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Value1", "");
                        actionParameters.Rows.Add("Operand", "");
                        actionParameters.Rows.Add("Value2", "");
                        actionParameters.Rows.Add("Case Sensitive", "No");
                        ifActionParameterBox.DataSource = actionParameters;
                    }

                    //combobox cell for Variable Name
                    comparisonComboBox = new DataGridViewComboBoxCell();
                    comparisonComboBox.Items.Add("contains");
                    comparisonComboBox.Items.Add("does not contain");
                    comparisonComboBox.Items.Add("is equal to");
                    comparisonComboBox.Items.Add("is not equal to");

                    //assign cell as a combobox
                    ifActionParameterBox.Rows[1].Cells[1] = comparisonComboBox;

                    comparisonComboBox = new DataGridViewComboBoxCell();
                    comparisonComboBox.Items.Add("Yes");
                    comparisonComboBox.Items.Add("No");

                    //assign cell as a combobox
                    ifActionParameterBox.Rows[3].Cells[1] = comparisonComboBox;

                    break;

                case "Variable Has Value":
      
                    ifActionParameterBox.Visible = true;
                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Variable Name", "");
                        ifActionParameterBox.DataSource = actionParameters;
                    }

                    break;
                case "Variable Is Numeric":
              
                    ifActionParameterBox.Visible = true;
                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Variable Name", "");
                        ifActionParameterBox.DataSource = actionParameters;
                    }

                    break;
                case "Error Occured":
             
                    ifActionParameterBox.Visible = true;
                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Line Number", "");
                        ifActionParameterBox.DataSource = actionParameters;
                    }

                    break;
                case "Error Did Not Occur":

                    ifActionParameterBox.Visible = true;

                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Line Number", "");
                        ifActionParameterBox.DataSource = actionParameters;
                    }

                    break;
                case "Window Name Exists":
                case "Active Window Name Is":
   
                    ifActionParameterBox.Visible = true;
                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Window Name", "");
                        ifActionParameterBox.DataSource = actionParameters;
                    }

                    break;
                case "File Exists":
         
                    ifActionParameterBox.Visible = true;
                    if (sender != null)
                    {
                        actionParameters.Rows.Add("File Path", "");
                        actionParameters.Rows.Add("True When", "");
                        ifActionParameterBox.DataSource = actionParameters;
                    }


                    //combobox cell for Variable Name
                    comparisonComboBox = new DataGridViewComboBoxCell();
                    comparisonComboBox.Items.Add("It Does Exist");
                    comparisonComboBox.Items.Add("It Does Not Exist");

                    //assign cell as a combobox
                    ifActionParameterBox.Rows[1].Cells[1] = comparisonComboBox;

                    break;
                case "Folder Exists":
           
                    ifActionParameterBox.Visible = true;


                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Folder Path", "");
                        actionParameters.Rows.Add("True When", "");
                        ifActionParameterBox.DataSource = actionParameters;
                    }

                    //combobox cell for Variable Name
                    comparisonComboBox = new DataGridViewComboBoxCell();
                    comparisonComboBox.Items.Add("It Does Exist");
                    comparisonComboBox.Items.Add("It Does Not Exist");

                    //assign cell as a combobox
                    ifActionParameterBox.Rows[1].Cells[1] = comparisonComboBox;
                    break;
                case "Web Element Exists":
      
                    ifActionParameterBox.Visible = true;

                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Selenium Instance Name", "default");
                        actionParameters.Rows.Add("Element Search Method", "");
                        actionParameters.Rows.Add("Element Search Parameter", "");
                        ifActionParameterBox.DataSource = actionParameters;
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

         
                    ifActionParameterBox.Visible = true;
                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Window Name", "Current Window");
                        actionParameters.Rows.Add("Element Search Method", "");
                        actionParameters.Rows.Add("Element Search Parameter", "");
                        ifActionParameterBox.DataSource = actionParameters;
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

                    RecorderControl.Show();

                    break;

                default:
                    break;
            }
        }
        private void ShowIfElementRecorder(object sender, EventArgs e)
        {

            //get command reference
            Core.Automation.Commands.UIAutomationCommand cmd = new Core.Automation.Commands.UIAutomationCommand();

            //create recorder
            UI.Forms.Supplemental.frmThickAppElementRecorder newElementRecorder = new UI.Forms.Supplemental.frmThickAppElementRecorder();
            newElementRecorder.searchParameters = cmd.v_UIASearchParameters;

            //show form
            newElementRecorder.ShowDialog();


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

            DataGridView ifActionBox = IfGridViewHelper;
            ifActionBox.Rows[0].Cells[1].Value = newElementRecorder.cboWindowTitle.Text;


            MessageBox.Show(sb.ToString());




        }
        public override string GetDisplayValue()
        {
            switch (v_IfActionType)
            {
                case "Value":
                case "Date Compare":
                case "Variable Compare":
                    string value1 = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                      where rw.Field<string>("Parameter Name") == "Value1"
                                      select rw.Field<string>("Parameter Value")).FirstOrDefault());
                    string operand = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                       where rw.Field<string>("Parameter Name") == "Operand"
                                       select rw.Field<string>("Parameter Value")).FirstOrDefault());
                    string value2 = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                      where rw.Field<string>("Parameter Name") == "Value2"
                                      select rw.Field<string>("Parameter Value")).FirstOrDefault());

                    return "If (" + value1 + " " + operand + " " + value2 + ")";

                case "Variable Has Value":
                    string variableName = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                      where rw.Field<string>("Parameter Name") == "Variable Name"
                                      select rw.Field<string>("Parameter Value")).FirstOrDefault());

                    return "If (Variable " + variableName + " Has Value)";
                case "Variable Is Numeric":
                    string varName = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                            where rw.Field<string>("Parameter Name") == "Variable Name"
                                            select rw.Field<string>("Parameter Value")).FirstOrDefault());

                    return "If (Variable " + varName + " Is Numeric)";

                case "Error Occured":

                    string lineNumber = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                          where rw.Field<string>("Parameter Name") == "Line Number"
                                          select rw.Field<string>("Parameter Value")).FirstOrDefault());

                    return "If (Error Occured on Line Number " + lineNumber + ")";
                case "Error Did Not Occur":

                    string lineNum = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                          where rw.Field<string>("Parameter Name") == "Line Number"
                                          select rw.Field<string>("Parameter Value")).FirstOrDefault());

                    return "If (Error Did Not Occur on Line Number " + lineNum + ")";
                case "Window Name Exists":
                case "Active Window Name Is":

                    string windowName = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                          where rw.Field<string>("Parameter Name") == "Window Name"
                                          select rw.Field<string>("Parameter Value")).FirstOrDefault());

                    return "If " + v_IfActionType + " [Name: " + windowName + "]";
                case "File Exists":

                    string filePath = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                        where rw.Field<string>("Parameter Name") == "File Path"
                                        select rw.Field<string>("Parameter Value")).FirstOrDefault());

                    string fileCompareType = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                               where rw.Field<string>("Parameter Name") == "True When"
                                               select rw.Field<string>("Parameter Value")).FirstOrDefault());


                    return "If " + v_IfActionType + " [File: " + filePath + "]";

                case "Folder Exists":

                    string folderPath = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                          where rw.Field<string>("Parameter Name") == "Folder Path"
                                          select rw.Field<string>("Parameter Value")).FirstOrDefault());

                    string folderCompareType = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                                 where rw.Field<string>("Parameter Name") == "True When"
                                                 select rw.Field<string>("Parameter Value")).FirstOrDefault());


                    return "If " + v_IfActionType + " [Folder: " + folderPath + "]";

                case "Web Element Exists":


                    string parameterName = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                             where rw.Field<string>("Parameter Name") == "Element Search Parameter"
                                             select rw.Field<string>("Parameter Value")).FirstOrDefault());

                    string searchMethod = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                            where rw.Field<string>("Parameter Name") == "Element Search Method"
                                            select rw.Field<string>("Parameter Value")).FirstOrDefault());


                    return "If Web Element Exists [" + searchMethod + ": " + parameterName + "]";

                case "GUI Element Exists":


                    string guiWindowName = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                         where rw.Field<string>("Parameter Name") == "Window Name"
                                         select rw.Field<string>("Parameter Value")).FirstOrDefault());

                    string guiSearch = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                             where rw.Field<string>("Parameter Name") == "Element Search Parameter"
                                             select rw.Field<string>("Parameter Value")).FirstOrDefault());




                    return "If GUI Element Exists [Find " + guiSearch + " Element In " + guiWindowName + "]";


                default:

                    return "If .... ";
            }

        }
    }
}