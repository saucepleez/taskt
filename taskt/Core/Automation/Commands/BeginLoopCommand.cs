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
    [Attributes.ClassAttributes.Group("Loop Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to evaluate a logical statement to determine if the statement is true. The following actions will repeat continuously until that statement becomes false")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to check if a statement is 'true' or 'false' and subsequently loop actions based on either condition. Any 'BeginLoop' command must have a following 'EndLoop' command.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command evaluates supplied arguments and if evaluated to true runs sub elements")]
    public class BeginLoopCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select type of Loop Command")]
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
        public string v_LoopActionType { get; set; }

        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Additional Parameters")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select the required comparison parameters.")]
        [Attributes.PropertyAttributes.SampleUsage("n/a")]
        [Attributes.PropertyAttributes.Remarks("")]
        public DataTable v_LoopActionParameterTable { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private DataGridView LoopGridViewHelper;

        [XmlIgnore]
        [NonSerialized]
        private ComboBox ActionDropdown;

        [XmlIgnore]
        [NonSerialized]
        private List<Control> ParameterControls;

        [XmlIgnore]
        [NonSerialized]
        CommandItemControl RecorderControl;

        public BeginLoopCommand()
        {
            this.CommandName = "BeginLoopCommand";
            this.SelectionName = "Begin Loop";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            //define parameter table
            this.v_LoopActionParameterTable = new System.Data.DataTable
            {
                TableName = DateTime.Now.ToString("LoopActionParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"))
            };
            this.v_LoopActionParameterTable.Columns.Add("Parameter Name");
            this.v_LoopActionParameterTable.Columns.Add("Parameter Value");

            LoopGridViewHelper = new DataGridView();
            LoopGridViewHelper.AllowUserToAddRows = true;
            LoopGridViewHelper.AllowUserToDeleteRows = true;
            LoopGridViewHelper.Size = new Size(400, 250);
            LoopGridViewHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            LoopGridViewHelper.DataBindings.Add("DataSource", this, "v_LoopActionParameterTable", false, DataSourceUpdateMode.OnPropertyChanged);
            LoopGridViewHelper.AllowUserToAddRows = false;
            LoopGridViewHelper.AllowUserToDeleteRows = false;
            LoopGridViewHelper.MouseEnter += LoopGridViewHelper_MouseEnter;

            RecorderControl = new taskt.UI.CustomControls.CommandItemControl();
            RecorderControl.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            RecorderControl.ForeColor = Color.AliceBlue;
            RecorderControl.Name = "guirecorder_helper";
            RecorderControl.CommandImage = UI.Images.GetUIImage("ClipboardGetTextCommand");
            RecorderControl.CommandDisplay = "Element Recorder";
            RecorderControl.Click += ShowLoopElementRecorder;
            RecorderControl.Hide();
        }
        private void LoopGridViewHelper_MouseEnter(object sender, EventArgs e)
        {
            loopAction_SelectionChangeCommitted(null, null);
        }
        public override void RunCommand(object sender, Core.Script.ScriptAction parentCommand)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var loopResult = DetermineStatementTruth(sender);
            engine.ReportProgress("Starting Loop"); 

            while (loopResult)
            {
                foreach (var cmd in parentCommand.AdditionalScriptCommands)
                {
                    if (engine.IsCancellationPending)
                        return;

                    engine.ExecuteCommand(cmd);

                    if (engine.CurrentLoopCancelled)
                    {
                        engine.ReportProgress("Exiting Loop"); 
                        engine.CurrentLoopCancelled = false;
                        return;
                    }

                    if (engine.CurrentLoopContinuing)
                    {
                        engine.ReportProgress("Continuing Next Loop"); 
                        engine.CurrentLoopContinuing = false;
                        break;
                    }
                }
                loopResult = DetermineStatementTruth(sender);
            }
        }

        public bool DetermineStatementTruth(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            bool loopResult = false;

            if (v_LoopActionType == "Value")
            {
                string value1 = ((from rw in v_LoopActionParameterTable.AsEnumerable()
                                  where rw.Field<string>("Parameter Name") == "Value1"
                                  select rw.Field<string>("Parameter Value")).FirstOrDefault());
                string operand = ((from rw in v_LoopActionParameterTable.AsEnumerable()
                                   where rw.Field<string>("Parameter Name") == "Operand"
                                   select rw.Field<string>("Parameter Value")).FirstOrDefault());
                string value2 = ((from rw in v_LoopActionParameterTable.AsEnumerable()
                                  where rw.Field<string>("Parameter Name") == "Value2"
                                  select rw.Field<string>("Parameter Value")).FirstOrDefault());

                value1 = value1.ConvertToUserVariable(sender);
                value2 = value2.ConvertToUserVariable(sender);



                decimal cdecValue1, cdecValue2;

                switch (operand)
                {
                    case "is equal to":
                        loopResult = (value1 == value2);
                        break;

                    case "is not equal to":
                        loopResult = (value1 != value2);
                        break;

                    case "is greater than":
                        cdecValue1 = Convert.ToDecimal(value1);
                        cdecValue2 = Convert.ToDecimal(value2);
                        loopResult = (cdecValue1 > cdecValue2);
                        break;

                    case "is greater than or equal to":
                        cdecValue1 = Convert.ToDecimal(value1);
                        cdecValue2 = Convert.ToDecimal(value2);
                        loopResult = (cdecValue1 >= cdecValue2);
                        break;

                    case "is less than":
                        cdecValue1 = Convert.ToDecimal(value1);
                        cdecValue2 = Convert.ToDecimal(value2);
                        loopResult = (cdecValue1 < cdecValue2);
                        break;

                    case "is less than or equal to":
                        cdecValue1 = Convert.ToDecimal(value1);
                        cdecValue2 = Convert.ToDecimal(value2);
                        loopResult = (cdecValue1 <= cdecValue2);
                        break;
                }
            }
            else if (v_LoopActionType == "Date Compare")
            {
                string value1 = ((from rw in v_LoopActionParameterTable.AsEnumerable()
                                  where rw.Field<string>("Parameter Name") == "Value1"
                                  select rw.Field<string>("Parameter Value")).FirstOrDefault());
                string operand = ((from rw in v_LoopActionParameterTable.AsEnumerable()
                                   where rw.Field<string>("Parameter Name") == "Operand"
                                   select rw.Field<string>("Parameter Value")).FirstOrDefault());
                string value2 = ((from rw in v_LoopActionParameterTable.AsEnumerable()
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
                        loopResult = (dt1 == dt2);
                        break;

                    case "is not equal to":
                        loopResult = (dt1 != dt2);
                        break;

                    case "is greater than":
                        loopResult = (dt1 > dt2);
                        break;

                    case "is greater than or equal to":
                        loopResult = (dt1 >= dt2);
                        break;

                    case "is less than":
                        loopResult = (dt1 < dt2);
                        break;

                    case "is less than or equal to":
                        loopResult = (dt1 <= dt2);
                        break;
                }
            }
            else if (v_LoopActionType == "Variable Compare")
            {
                string value1 = ((from rw in v_LoopActionParameterTable.AsEnumerable()
                                  where rw.Field<string>("Parameter Name") == "Value1"
                                  select rw.Field<string>("Parameter Value")).FirstOrDefault());
                string operand = ((from rw in v_LoopActionParameterTable.AsEnumerable()
                                   where rw.Field<string>("Parameter Name") == "Operand"
                                   select rw.Field<string>("Parameter Value")).FirstOrDefault());
                string value2 = ((from rw in v_LoopActionParameterTable.AsEnumerable()
                                  where rw.Field<string>("Parameter Name") == "Value2"
                                  select rw.Field<string>("Parameter Value")).FirstOrDefault());

                string caseSensitive = ((from rw in v_LoopActionParameterTable.AsEnumerable()
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
                        loopResult = (value1.Contains(value2));
                        break;

                    case "does not contain":
                        loopResult = (!value1.Contains(value2));
                        break;

                    case "is equal to":
                        loopResult = (value1 == value2);
                        break;

                    case "is not equal to":
                        loopResult = (value1 != value2);
                        break;
                }
            }
            else if (v_LoopActionType == "Variable Has Value")
            {
                string variableName = ((from rw in v_LoopActionParameterTable.AsEnumerable()
                                        where rw.Field<string>("Parameter Name") == "Variable Name"
                                        select rw.Field<string>("Parameter Value")).FirstOrDefault());

                var actualVariable = variableName.ConvertToUserVariable(sender).Trim();

                if (!string.IsNullOrEmpty(actualVariable))
                {
                    loopResult = true;
                }
                else
                {
                    loopResult = false;
                }

            }
            else if (v_LoopActionType == "Variable Is Numeric")
            {
                string variableName = ((from rw in v_LoopActionParameterTable.AsEnumerable()
                                        where rw.Field<string>("Parameter Name") == "Variable Name"
                                        select rw.Field<string>("Parameter Value")).FirstOrDefault());

                var actualVariable = variableName.ConvertToUserVariable(sender).Trim();

                var numericTest = decimal.TryParse(actualVariable, out decimal parsedResult);

                if (numericTest)
                {
                    loopResult = true;
                }
                else
                {
                    loopResult = false;
                }

            }
            else if (v_LoopActionType == "Error Occured")
            {
                //get line number
                string userLineNumber = ((from rw in v_LoopActionParameterTable.AsEnumerable()
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

                    loopResult = true;
                }
                else
                {
                    loopResult = false;
                }

            }
            else if (v_LoopActionType == "Error Did Not Occur")
            {
                //get line number
                string userLineNumber = ((from rw in v_LoopActionParameterTable.AsEnumerable()
                                          where rw.Field<string>("Parameter Name") == "Line Number"
                                          select rw.Field<string>("Parameter Value")).FirstOrDefault());

                //convert to variable
                string variableLineNumber = userLineNumber.ConvertToUserVariable(sender);

                //convert to int
                int lineNumber = int.Parse(variableLineNumber);

                //determine if error happened
                if (engine.ErrorsOccured.Where(f => f.LineNumber == lineNumber).Count() == 0)
                {
                    loopResult = true;
                }
                else
                {
                    var error = engine.ErrorsOccured.Where(f => f.LineNumber == lineNumber).FirstOrDefault();
                    error.ErrorMessage.StoreInUserVariable(sender, "Error.Message");
                    error.LineNumber.ToString().StoreInUserVariable(sender, "Error.Line");
                    error.StackTrace.StoreInUserVariable(sender, "Error.StackTrace");

                    loopResult = false;
                }

            }
            else if (v_LoopActionType == "Window Name Exists")
            {
                //get user supplied name
                string windowName = ((from rw in v_LoopActionParameterTable.AsEnumerable()
                                      where rw.Field<string>("Parameter Name") == "Window Name"
                                      select rw.Field<string>("Parameter Value")).FirstOrDefault());
                //variable translation
                string variablizedWindowName = windowName.ConvertToUserVariable(sender);

                //search for window
                IntPtr windowPtr = User32Functions.FindWindow(variablizedWindowName);

                //conditional
                if (windowPtr != IntPtr.Zero)
                {
                    loopResult = true;
                }



            }
            else if (v_LoopActionType == "Active Window Name Is")
            {
                string windowName = ((from rw in v_LoopActionParameterTable.AsEnumerable()
                                      where rw.Field<string>("Parameter Name") == "Window Name"
                                      select rw.Field<string>("Parameter Value")).FirstOrDefault());

                string variablizedWindowName = windowName.ConvertToUserVariable(sender);

                var currentWindowTitle = User32Functions.GetActiveWindowTitle();

                if (currentWindowTitle == variablizedWindowName)
                {
                    loopResult = true;
                }

            }
            else if (v_LoopActionType == "File Exists")
            {

                string fileName = ((from rw in v_LoopActionParameterTable.AsEnumerable()
                                    where rw.Field<string>("Parameter Name") == "File Path"
                                    select rw.Field<string>("Parameter Value")).FirstOrDefault());

                string trueWhenFileExists = ((from rw in v_LoopActionParameterTable.AsEnumerable()
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
                    loopResult = true;
                }


            }
            else if (v_LoopActionType == "Folder Exists")
            {
                string folderName = ((from rw in v_LoopActionParameterTable.AsEnumerable()
                                      where rw.Field<string>("Parameter Name") == "Folder Path"
                                      select rw.Field<string>("Parameter Value")).FirstOrDefault());

                string trueWhenFileExists = ((from rw in v_LoopActionParameterTable.AsEnumerable()
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
                    loopResult = true;
                }

            }
            else if (v_LoopActionType == "Web Element Exists")
            {
                string instanceName = ((from rw in v_LoopActionParameterTable.AsEnumerable()
                                        where rw.Field<string>("Parameter Name") == "Selenium Instance Name"
                                        select rw.Field<string>("Parameter Value")).FirstOrDefault());

                string parameterName = ((from rw in v_LoopActionParameterTable.AsEnumerable()
                                         where rw.Field<string>("Parameter Name") == "Element Search Parameter"
                                         select rw.Field<string>("Parameter Value")).FirstOrDefault());

                string searchMethod = ((from rw in v_LoopActionParameterTable.AsEnumerable()
                                        where rw.Field<string>("Parameter Name") == "Element Search Method"
                                        select rw.Field<string>("Parameter Value")).FirstOrDefault());


                SeleniumBrowserElementActionCommand newElementActionCommand = new SeleniumBrowserElementActionCommand();
                newElementActionCommand.v_SeleniumSearchType = searchMethod;
                newElementActionCommand.v_InstanceName = instanceName.ConvertToUserVariable(sender);
                bool elementExists = newElementActionCommand.ElementExists(sender, searchMethod, parameterName);
                loopResult = elementExists;

            }
            else if (v_LoopActionType == "GUI Element Exists")
            {
                string windowName = ((from rw in v_LoopActionParameterTable.AsEnumerable()
                                      where rw.Field<string>("Parameter Name") == "Window Name"
                                      select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertToUserVariable(sender));

                string elementSearchParam = ((from rw in v_LoopActionParameterTable.AsEnumerable()
                                              where rw.Field<string>("Parameter Name") == "Element Search Parameter"
                                              select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertToUserVariable(sender));

                string elementSearchMethod = ((from rw in v_LoopActionParameterTable.AsEnumerable()
                                               where rw.Field<string>("Parameter Name") == "Element Search Method"
                                               select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertToUserVariable(sender));


                UIAutomationCommand newUIACommand = new UIAutomationCommand();
                newUIACommand.v_WindowName = windowName;
                newUIACommand.v_UIASearchParameters.Rows.Add(true, elementSearchMethod, elementSearchParam);
                var handle = newUIACommand.SearchForGUIElement(sender, windowName);

                if (handle is null)
                {
                    loopResult = false;
                }
                else
                {
                    loopResult = true;
                }


            }
            else
            {
                throw new Exception("Loop type not recognized!");
            }

            return loopResult;
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            ActionDropdown = (ComboBox)CommandControls.CreateDropdownFor("v_LoopActionType", this);
            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_LoopActionType", this));
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_LoopActionType", this, new Control[] { ActionDropdown }, editor));
            ActionDropdown.SelectionChangeCommitted += loopAction_SelectionChangeCommitted;

            RenderedControls.Add(ActionDropdown);

            ParameterControls = new List<Control>();
            ParameterControls.Add(CommandControls.CreateDefaultLabelFor("v_LoopActionParameterTable", this));
            ParameterControls.Add(RecorderControl);
            ParameterControls.AddRange(CommandControls.CreateUIHelpersFor("v_LoopActionParameterTable", this, new Control[] { LoopGridViewHelper }, editor));
            ParameterControls.Add(LoopGridViewHelper);

            RenderedControls.AddRange(ParameterControls);

            return RenderedControls;
        }

        private void loopAction_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox loopAction = (ComboBox)ActionDropdown;
            DataGridView loopActionParameterBox = (DataGridView)LoopGridViewHelper;

            Core.Automation.Commands.BeginLoopCommand cmd = (Core.Automation.Commands.BeginLoopCommand)this;
            DataTable actionParameters = cmd.v_LoopActionParameterTable;

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


            switch (loopAction.SelectedItem)
            {
                case "Value":
                case "Date Compare":

                    loopActionParameterBox.Visible = true;

                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Value1", "");
                        actionParameters.Rows.Add("Operand", "");
                        actionParameters.Rows.Add("Value2", "");
                        loopActionParameterBox.DataSource = actionParameters;
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
                    loopActionParameterBox.Rows[1].Cells[1] = comparisonComboBox;

                    break;
                case "Variable Compare":

                    loopActionParameterBox.Visible = true;

                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Value1", "");
                        actionParameters.Rows.Add("Operand", "");
                        actionParameters.Rows.Add("Value2", "");
                        actionParameters.Rows.Add("Case Sensitive", "No");
                        loopActionParameterBox.DataSource = actionParameters;
                    }

                    //combobox cell for Variable Name
                    comparisonComboBox = new DataGridViewComboBoxCell();
                    comparisonComboBox.Items.Add("contains");
                    comparisonComboBox.Items.Add("does not contain");
                    comparisonComboBox.Items.Add("is equal to");
                    comparisonComboBox.Items.Add("is not equal to");

                    //assign cell as a combobox
                    loopActionParameterBox.Rows[1].Cells[1] = comparisonComboBox;

                    comparisonComboBox = new DataGridViewComboBoxCell();
                    comparisonComboBox.Items.Add("Yes");
                    comparisonComboBox.Items.Add("No");

                    //assign cell as a combobox
                    loopActionParameterBox.Rows[3].Cells[1] = comparisonComboBox;

                    break;

                case "Variable Has Value":

                    loopActionParameterBox.Visible = true;
                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Variable Name", "");
                        loopActionParameterBox.DataSource = actionParameters;
                    }

                    break;
                case "Variable Is Numeric":

                    loopActionParameterBox.Visible = true;
                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Variable Name", "");
                        loopActionParameterBox.DataSource = actionParameters;
                    }

                    break;
                case "Error Occured":

                    loopActionParameterBox.Visible = true;
                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Line Number", "");
                        loopActionParameterBox.DataSource = actionParameters;
                    }

                    break;
                case "Error Did Not Occur":

                    loopActionParameterBox.Visible = true;

                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Line Number", "");
                        loopActionParameterBox.DataSource = actionParameters;
                    }

                    break;
                case "Window Name Exists":
                case "Active Window Name Is":

                    loopActionParameterBox.Visible = true;
                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Window Name", "");
                        loopActionParameterBox.DataSource = actionParameters;
                    }

                    break;
                case "File Exists":

                    loopActionParameterBox.Visible = true;
                    if (sender != null)
                    {
                        actionParameters.Rows.Add("File Path", "");
                        actionParameters.Rows.Add("True When", "");
                        loopActionParameterBox.DataSource = actionParameters;
                    }


                    //combobox cell for Variable Name
                    comparisonComboBox = new DataGridViewComboBoxCell();
                    comparisonComboBox.Items.Add("It Does Exist");
                    comparisonComboBox.Items.Add("It Does Not Exist");

                    //assign cell as a combobox
                    loopActionParameterBox.Rows[1].Cells[1] = comparisonComboBox;

                    break;
                case "Folder Exists":

                    loopActionParameterBox.Visible = true;


                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Folder Path", "");
                        actionParameters.Rows.Add("True When", "");
                        loopActionParameterBox.DataSource = actionParameters;
                    }

                    //combobox cell for Variable Name
                    comparisonComboBox = new DataGridViewComboBoxCell();
                    comparisonComboBox.Items.Add("It Does Exist");
                    comparisonComboBox.Items.Add("It Does Not Exist");

                    //assign cell as a combobox
                    loopActionParameterBox.Rows[1].Cells[1] = comparisonComboBox;
                    break;
                case "Web Element Exists":

                    loopActionParameterBox.Visible = true;

                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Selenium Instance Name", "default");
                        actionParameters.Rows.Add("Element Search Method", "");
                        actionParameters.Rows.Add("Element Search Parameter", "");
                        loopActionParameterBox.DataSource = actionParameters;
                    }



                    comparisonComboBox = new DataGridViewComboBoxCell();
                    comparisonComboBox.Items.Add("Find Element By XPath");
                    comparisonComboBox.Items.Add("Find Element By ID");
                    comparisonComboBox.Items.Add("Find Element By Name");
                    comparisonComboBox.Items.Add("Find Element By Tag Name");
                    comparisonComboBox.Items.Add("Find Element By Class Name");
                    comparisonComboBox.Items.Add("Find Element By CSS Selector");

                    //assign cell as a combobox
                    loopActionParameterBox.Rows[1].Cells[1] = comparisonComboBox;

                    break;
                case "GUI Element Exists":


                    loopActionParameterBox.Visible = true;
                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Window Name", "Current Window");
                        actionParameters.Rows.Add("Element Search Method", "");
                        actionParameters.Rows.Add("Element Search Parameter", "");
                        loopActionParameterBox.DataSource = actionParameters;
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
                    loopActionParameterBox.Rows[1].Cells[1] = parameterName;

                    RecorderControl.Show();

                    break;

                default:
                    break;
            }
        }

        private void ShowLoopElementRecorder(object sender, EventArgs e)
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

            DataGridView loopActionBox = LoopGridViewHelper;
            loopActionBox.Rows[0].Cells[1].Value = newElementRecorder.cboWindowTitle.Text;

            MessageBox.Show(sb.ToString());
        }

        public override string GetDisplayValue()
        {
            switch (v_LoopActionType)
            {
                case "Value":
                case "Date Compare":
                case "Variable Compare":
                    string value1 = ((from rw in v_LoopActionParameterTable.AsEnumerable()
                                      where rw.Field<string>("Parameter Name") == "Value1"
                                      select rw.Field<string>("Parameter Value")).FirstOrDefault());
                    string operand = ((from rw in v_LoopActionParameterTable.AsEnumerable()
                                       where rw.Field<string>("Parameter Name") == "Operand"
                                       select rw.Field<string>("Parameter Value")).FirstOrDefault());
                    string value2 = ((from rw in v_LoopActionParameterTable.AsEnumerable()
                                      where rw.Field<string>("Parameter Name") == "Value2"
                                      select rw.Field<string>("Parameter Value")).FirstOrDefault());

                    return "Loop While (" + value1 + " " + operand + " " + value2 + ")";

                case "Variable Has Value":
                    string variableName = ((from rw in v_LoopActionParameterTable.AsEnumerable()
                                            where rw.Field<string>("Parameter Name") == "Variable Name"
                                            select rw.Field<string>("Parameter Value")).FirstOrDefault());

                    return "Loop While (Variable " + variableName + " Has Value)";
                case "Variable Is Numeric":
                    string varName = ((from rw in v_LoopActionParameterTable.AsEnumerable()
                                       where rw.Field<string>("Parameter Name") == "Variable Name"
                                       select rw.Field<string>("Parameter Value")).FirstOrDefault());

                    return "Loop While (Variable " + varName + " Is Numeric)";

                case "Error Occured":

                    string lineNumber = ((from rw in v_LoopActionParameterTable.AsEnumerable()
                                          where rw.Field<string>("Parameter Name") == "Line Number"
                                          select rw.Field<string>("Parameter Value")).FirstOrDefault());

                    return "Loop While (Error Occured on Line Number " + lineNumber + ")";
                case "Error Did Not Occur":

                    string lineNum = ((from rw in v_LoopActionParameterTable.AsEnumerable()
                                       where rw.Field<string>("Parameter Name") == "Line Number"
                                       select rw.Field<string>("Parameter Value")).FirstOrDefault());

                    return "Loop While (Error Did Not Occur on Line Number " + lineNum + ")";
                case "Window Name Exists":
                case "Active Window Name Is":

                    string windowName = ((from rw in v_LoopActionParameterTable.AsEnumerable()
                                          where rw.Field<string>("Parameter Name") == "Window Name"
                                          select rw.Field<string>("Parameter Value")).FirstOrDefault());

                    return "Loop While " + v_LoopActionType + " [Name: " + windowName + "]";
                case "File Exists":

                    string filePath = ((from rw in v_LoopActionParameterTable.AsEnumerable()
                                        where rw.Field<string>("Parameter Name") == "File Path"
                                        select rw.Field<string>("Parameter Value")).FirstOrDefault());

                    string fileCompareType = ((from rw in v_LoopActionParameterTable.AsEnumerable()
                                               where rw.Field<string>("Parameter Name") == "True When"
                                               select rw.Field<string>("Parameter Value")).FirstOrDefault());


                    return "Loop While " + v_LoopActionType + " [File: " + filePath + "]";

                case "Folder Exists":

                    string folderPath = ((from rw in v_LoopActionParameterTable.AsEnumerable()
                                          where rw.Field<string>("Parameter Name") == "Folder Path"
                                          select rw.Field<string>("Parameter Value")).FirstOrDefault());

                    string folderCompareType = ((from rw in v_LoopActionParameterTable.AsEnumerable()
                                                 where rw.Field<string>("Parameter Name") == "True When"
                                                 select rw.Field<string>("Parameter Value")).FirstOrDefault());


                    return "Loop While " + v_LoopActionType + " [Folder: " + folderPath + "]";

                case "Web Element Exists":


                    string parameterName = ((from rw in v_LoopActionParameterTable.AsEnumerable()
                                             where rw.Field<string>("Parameter Name") == "Element Search Parameter"
                                             select rw.Field<string>("Parameter Value")).FirstOrDefault());

                    string searchMethod = ((from rw in v_LoopActionParameterTable.AsEnumerable()
                                            where rw.Field<string>("Parameter Name") == "Element Search Method"
                                            select rw.Field<string>("Parameter Value")).FirstOrDefault());


                    return "Loop While Web Element Exists [" + searchMethod + ": " + parameterName + "]";

                case "GUI Element Exists":


                    string guiWindowName = ((from rw in v_LoopActionParameterTable.AsEnumerable()
                                             where rw.Field<string>("Parameter Name") == "Window Name"
                                             select rw.Field<string>("Parameter Value")).FirstOrDefault());

                    string guiSearch = ((from rw in v_LoopActionParameterTable.AsEnumerable()
                                         where rw.Field<string>("Parameter Name") == "Element Search Parameter"
                                         select rw.Field<string>("Parameter Value")).FirstOrDefault());




                    return "Loop While GUI Element Exists [Find " + guiSearch + " Element In " + guiWindowName + "]";


                default:

                    return "Loop While .... ";
            }

        }
    }
}
