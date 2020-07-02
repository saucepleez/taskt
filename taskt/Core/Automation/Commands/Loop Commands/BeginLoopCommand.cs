using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.ClassAttributes;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Engine;
using taskt.Core.Automation.User32;
using taskt.Core.Script;
using taskt.Core.Utilities.CommonUtilities;
using taskt.UI;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using taskt.UI.Forms.Supplement_Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Group("Loop Commands")]
    [Description("This command evaluates a specified logical statement and executes the contained commands repeatedly (in loop) " +
        "until that logical statement becomes false.")]
    public class BeginLoopCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Loop Condition")]
        [PropertyUISelectionOption("Value Compare")]
        [PropertyUISelectionOption("Date Compare")]
        [PropertyUISelectionOption("Variable Compare")]
        [PropertyUISelectionOption("Variable Has Value")]
        [PropertyUISelectionOption("Variable Is Numeric")]
        [PropertyUISelectionOption("Window Name Exists")]
        [PropertyUISelectionOption("Active Window Name Is")]
        [PropertyUISelectionOption("File Exists")]
        [PropertyUISelectionOption("Folder Exists")]
        [PropertyUISelectionOption("Web Element Exists")]
        [PropertyUISelectionOption("GUI Element Exists")]
        [PropertyUISelectionOption("Error Occured")]
        [PropertyUISelectionOption("Error Did Not Occur")]
        [InputSpecification("Select the necessary condition type.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_LoopActionType { get; set; }

        [XmlElement]
        [PropertyDescription("Additional Parameters")]
        [InputSpecification("Supply or Select the required comparison parameters.")]
        [SampleUsage("Param Value || {vParamValue}")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public DataTable v_LoopActionParameterTable { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private DataGridView _loopGridViewHelper;

        [XmlIgnore]
        [NonSerialized]
        private ComboBox _actionDropdown;

        [XmlIgnore]
        [NonSerialized]
        private List<Control> _parameterControls;

        [XmlIgnore]
        [NonSerialized]
        private CommandItemControl _recorderControl;

        public BeginLoopCommand()
        {
            CommandName = "BeginLoopCommand";
            SelectionName = "Begin Loop";
            CommandEnabled = true;
            CustomRendering = true;

            //define parameter table
            v_LoopActionParameterTable = new DataTable
            {
                TableName = DateTime.Now.ToString("LoopActionParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"))
            };
            v_LoopActionParameterTable.Columns.Add("Parameter Name");
            v_LoopActionParameterTable.Columns.Add("Parameter Value");

            _loopGridViewHelper = new DataGridView();
            _loopGridViewHelper.AllowUserToAddRows = true;
            _loopGridViewHelper.AllowUserToDeleteRows = true;
            _loopGridViewHelper.Size = new Size(400, 250);
            _loopGridViewHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _loopGridViewHelper.DataBindings.Add("DataSource", this, "v_LoopActionParameterTable", false, DataSourceUpdateMode.OnPropertyChanged);
            _loopGridViewHelper.AllowUserToAddRows = false;
            _loopGridViewHelper.AllowUserToDeleteRows = false;
            _loopGridViewHelper.MouseEnter += LoopGridViewHelper_MouseEnter;

            _recorderControl = new CommandItemControl();
            _recorderControl.Padding = new Padding(10, 0, 0, 0);
            _recorderControl.ForeColor = Color.AliceBlue;
            _recorderControl.Name = "guirecorder_helper";
            _recorderControl.CommandImage = Images.GetUIImage("ClipboardGetTextCommand");
            _recorderControl.CommandDisplay = "Element Recorder";
            _recorderControl.Click += ShowLoopElementRecorder;
            _recorderControl.Hide();
        }

        public override void RunCommand(object sender, ScriptAction parentCommand)
        {
            var engine = (AutomationEngineInstance)sender;
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

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            _actionDropdown = (ComboBox)CommandControls.CreateDropdownFor("v_LoopActionType", this);
            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_LoopActionType", this));
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_LoopActionType", this, new Control[] { _actionDropdown }, editor));
            _actionDropdown.SelectionChangeCommitted += loopAction_SelectionChangeCommitted;
            RenderedControls.Add(_actionDropdown);

            _parameterControls = new List<Control>();
            _parameterControls.Add(CommandControls.CreateDefaultLabelFor("v_LoopActionParameterTable", this));
            _parameterControls.Add(_recorderControl);
            _parameterControls.AddRange(CommandControls.CreateUIHelpersFor("v_LoopActionParameterTable", this, new Control[] { _loopGridViewHelper }, editor));
            _parameterControls.Add(_loopGridViewHelper);
            RenderedControls.AddRange(_parameterControls);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            switch (v_LoopActionType)
            {
                case "Value Compare":
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

        private void LoopGridViewHelper_MouseEnter(object sender, EventArgs e)
        {
            loopAction_SelectionChangeCommitted(null, null);
        }

        private void loopAction_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox loopAction = (ComboBox)_actionDropdown;
            DataGridView loopActionParameterBox = (DataGridView)_loopGridViewHelper;

            BeginLoopCommand cmd = (BeginLoopCommand)this;
            DataTable actionParameters = cmd.v_LoopActionParameterTable;

            //sender is null when command is updating
            if (sender != null)
                actionParameters.Rows.Clear();

            DataGridViewComboBoxCell comparisonComboBox = new DataGridViewComboBoxCell();

            //recorder control
            Control recorderControl = (Control)_recorderControl;

            //remove if exists            
            if (_recorderControl.Visible)
            {
                _recorderControl.Hide();
            }

            switch (loopAction.SelectedItem)
            {
                case "Value Compare":
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

                    _recorderControl.Show();
                    break;

                default:
                    break;
            }
        }

        private void ShowLoopElementRecorder(object sender, EventArgs e)
        {
            //get command reference
            UIAutomationCommand cmd = new UIAutomationCommand();

            //create recorder
            frmThickAppElementRecorder newElementRecorder = new frmThickAppElementRecorder();
            newElementRecorder.SearchParameters = cmd.v_UIASearchParameters;

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

            DataGridView loopActionBox = _loopGridViewHelper;
            loopActionBox.Rows[0].Cells[1].Value = newElementRecorder.cboWindowTitle.Text;

            MessageBox.Show(sb.ToString());
        }

        public bool DetermineStatementTruth(object sender)
        {
            var engine = (AutomationEngineInstance)sender;

            bool loopResult = false;

            if (v_LoopActionType == "Value Compare")
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

                if (File.Exists(userFileSelected) == existCheck)
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

                if (Directory.Exists(folderName) == existCheck)
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


                SeleniumElementActionCommand newElementActionCommand = new SeleniumElementActionCommand();
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
    }
}
