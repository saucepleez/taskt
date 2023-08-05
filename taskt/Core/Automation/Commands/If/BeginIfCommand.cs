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
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Numeric Compare")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Date Compare")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Text Compare")]
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
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Boolean")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Boolean Compare")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("List Compare")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Dictionary Compare")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("DataTable Compare")]
        [Attributes.PropertyAttributes.InputSpecification("Select the necessary comparison type.")]
        [Attributes.PropertyAttributes.SampleUsage("Select **Value**, **Window Name Exists**, **Active Window Name Is**, **File Exists**, **Folder Exists**, **Web Element Exists**, **Error Occured**, **Boolean**")]
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

        //[XmlIgnore]
        //[NonSerialized]
        //CommandItemControl RecorderControl;

        [XmlIgnore]
        [NonSerialized]
        CommandItemControl lnkBrowserInstanceSelector;

        [XmlIgnore]
        [NonSerialized]
        CommandItemControl lnkWindowNameSelector;

        [XmlIgnore]
        [NonSerialized]
        CommandItemControl lnkBooleanSelector;

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
        }

        private void IfGridViewHelper_MouseEnter(object sender, EventArgs e, frmCommandEditor editor)
        {
            ifAction_SelectionChangeCommitted(null, null, editor);
        }

        public override void RunCommand(object sender, Core.Script.ScriptAction parentCommand)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            bool ifResult = ConditionControls.DetermineStatementTruth(v_IfActionType, v_IfActionParameterTable, engine);

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
        //public bool DetermineStatementTruth(object sender)
        //{
        //    var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

        //    bool ifResult = false;

        //    string actionType = v_IfActionType.ConvertToUserVariable(engine);

        //    switch (actionType.ToLower())
        //    {
        //        case "value":
        //            ifResult = DetermineStatementTruth_Value(engine);
        //            break;

        //        case "date compare":
        //            ifResult = DetermineStatementTruth_DateCompare(engine);
        //            break;

        //        case "variable compare":
        //            ifResult = DetermineStatementTruth_VariableCompare(engine);
        //            break;

        //        case "variable has value":
        //            ifResult = DetermineStatementTruth_VariableHasValue(engine);
        //            break;

        //        case "variable is numeric":
        //            ifResult = DetermineStatementTruth_VariableIsNumeric(engine);
        //            break;

        //        case "error occured":
        //            ifResult = DetermineStatementTruth_ErrorOccur(engine, false);
        //            break;

        //        case "error did not occur":
        //            ifResult = DetermineStatementTruth_ErrorOccur(engine, true);
        //            break;

        //        case "window name exists":
        //            ifResult = DetermineStatementTruth_WindowNameExists(engine);
        //            break;

        //        case "active window name is":
        //            ifResult = DetermineStatementTruth_ActiveWindow(engine);
        //            break;

        //        case "file exists":
        //            ifResult = DetermineStatementTruth_File(engine);
        //            break;

        //        case "folder exists":
        //            ifResult = DetermineStatementTruth_Folder(engine);
        //            break;

        //        case "web element exists":
        //            ifResult = DetermineStatementTruth_WebElement(engine);
        //            break;

        //        case "gui element exists":
        //            ifResult = DetermineStatementTruth_GUIElement(engine);
        //            break;

        //        case "boolean":
        //            ifResult = DetermineStatementTruth_Boolean(engine);
        //            break;

        //        default:
        //            throw new Exception("If type not recognized!");
        //            break;
        //    }

        //    //if (v_IfActionType == "Value")
        //    //{
        //    //    ifResult = DetermineStatementTruth_Value(engine);
        //    //}
        //    //else if (v_IfActionType == "Date Compare")
        //    //{
        //    //    ifResult = DetermineStatementTruth_DateCompare(engine);
        //    //}
        //    //else if (v_IfActionType == "Variable Compare")
        //    //{
        //    //    ifResult = DetermineStatementTruth_VariableCompare(engine);
        //    //}
        //    //else if (v_IfActionType == "Variable Has Value")
        //    //{
        //    //    ifResult = DetermineStatementTruth_VariableHasValue(engine);
        //    //}
        //    //else if (v_IfActionType == "Variable Is Numeric")
        //    //{
        //    //    ifResult = DetermineStatementTruth_VariableIsNumeric(engine);
        //    //}
        //    //else if (v_IfActionType == "Error Occured")
        //    //{
        //    //    ////get line number
        //    //    //string userLineNumber = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //    //    //                          where rw.Field<string>("Parameter Name") == "Line Number"
        //    //    //                          select rw.Field<string>("Parameter Value")).FirstOrDefault());

        //    //    ////convert to variable
        //    //    //string variableLineNumber = userLineNumber.ConvertToUserVariable(sender);

        //    //    ////convert to int
        //    //    //int lineNumber = int.Parse(variableLineNumber);

        //    //    ////determine if error happened
        //    //    //if (engine.ErrorsOccured.Where(f => f.LineNumber == lineNumber).Count() > 0)
        //    //    //{

        //    //    //    var error = engine.ErrorsOccured.Where(f => f.LineNumber == lineNumber).FirstOrDefault();
        //    //    //    error.ErrorMessage.StoreInUserVariable(sender, "Error.Message");
        //    //    //    error.LineNumber.ToString().StoreInUserVariable(sender, "Error.Line");
        //    //    //    error.StackTrace.StoreInUserVariable(sender, "Error.StackTrace");

        //    //    //    ifResult = true;
        //    //    //}
        //    //    //else
        //    //    //{
        //    //    //    ifResult = false;
        //    //    //}
        //    //    ifResult = DetermineStatementTruth_ErrorDidOccur(engine, false);
        //    //}
        //    //else if (v_IfActionType == "Error Did Not Occur")
        //    //{
        //    //    ifResult = DetermineStatementTruth_ErrorDidOccur(engine, true);
        //    //}
        //    //else if (v_IfActionType == "Window Name Exists")
        //    //{
        //    //    ifResult = DetermineStatementTruth_WindowNameExists(engine);
        //    //}
        //    //else if (v_IfActionType == "Active Window Name Is")
        //    //{
        //    //    ifResult = DetermineStatementTruth_ActiveWindow(engine);
        //    //}
        //    //else if (v_IfActionType == "File Exists")
        //    //{
        //    //    ifResult = DetermineStatementTruth_File(engine);
        //    //}
        //    //else if (v_IfActionType == "Folder Exists")
        //    //{
        //    //    ifResult = DetermineStatementTruth_Folder(engine);
        //    //}
        //    //else if (v_IfActionType == "Web Element Exists")
        //    //{
        //    //    ifResult = DetermineStatementTruth_WebElement(engine);
        //    //}
        //    //else if (v_IfActionType == "GUI Element Exists")
        //    //{
        //    //    ifResult = DetermineStatementTruth_GUIElement(engine);
        //    //}
        //    //else if (v_IfActionType == "Boolean")
        //    //{
        //    //    ifResult = DetermineStatementTruth_Boolean(engine);
        //    //}
        //    //else
        //    //{
        //    //    throw new Exception("If type not recognized!");
        //    //}

        //    return ifResult;
        //}

        //private bool DetermineStatementTruth_Value(Engine.AutomationEngineInstance engine)
        //{
        //    //string value1 = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //    //                  where rw.Field<string>("Parameter Name") == "Value1"
        //    //                  select rw.Field<string>("Parameter Value")).FirstOrDefault());
        //    //string operand = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //    //                   where rw.Field<string>("Parameter Name") == "Operand"
        //    //                   select rw.Field<string>("Parameter Value")).FirstOrDefault());
        //    //string value2 = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //    //                  where rw.Field<string>("Parameter Name") == "Value2"
        //    //                  select rw.Field<string>("Parameter Value")).FirstOrDefault());

        //    //value1 = value1.ConvertToUserVariable(sender);
        //    //value2 = value2.ConvertToUserVariable(sender);

        //    var param = DataTableControls.GetFieldValues(v_IfActionParameterTable, "Parameter Name", "Parameter Value");

        //    string operand = param["Operand"].ConvertToUserVariable(engine);

        //    bool isBoolCompare = false;
        //    decimal value1 = 0;
        //    decimal value2 = 0;
        //    switch (operand.ToLower())
        //    {
        //        case "is equal to":
        //        case "is not equal to":
        //            bool tempBool;
        //            isBoolCompare = bool.TryParse(param["Value1"], out tempBool) && bool.TryParse(param["Value2"], out tempBool);
        //            break;
        //        default:
        //            value1 = param["Value1"].ConvertToUserVariableAsDecimal("Value1", engine);
        //            value2 = param["Value2"].ConvertToUserVariableAsDecimal("Value2", engine);
        //            break;
        //    }

        //    //bool tempBool;
        //    //bool isBoolCompare = (bool.TryParse(value1, out tempBool) && bool.TryParse(value2, out tempBool));
        //    //decimal cdecValue1, cdecValue2;

        //    bool ifResult;
        //    switch (operand.ToLower())
        //    {
        //        case "is equal to":
        //            if (isBoolCompare)
        //            {
        //                ifResult = (bool.Parse(param["Value1"]) == bool.Parse(param["Value2"]));
        //            }
        //            else
        //            {
        //                ifResult = (param["Value1"] == param["Value2"]);
        //            }
        //            break;

        //        case "is not equal to":
        //            if (isBoolCompare)
        //            {
        //                ifResult = (bool.Parse(param["Value1"]) != bool.Parse(param["Value2"]));
        //            }
        //            else
        //            {
        //                ifResult = (param["Value1"] != param["Value2"]);
        //            }
        //            break;

        //        case "is greater than":
        //            //cdecValue1 = Convert.ToDecimal(value1);
        //            //cdecValue2 = Convert.ToDecimal(value2);
        //            //ifResult = (cdecValue1 > cdecValue2);
        //            ifResult = value1 > value2;
        //            break;

        //        case "is greater than or equal to":
        //            //cdecValue1 = Convert.ToDecimal(value1);
        //            //cdecValue2 = Convert.ToDecimal(value2);
        //            //ifResult = (cdecValue1 >= cdecValue2);
        //            ifResult = value1 >= value2;
        //            break;

        //        case "is less than":
        //            //cdecValue1 = Convert.ToDecimal(value1);
        //            //cdecValue2 = Convert.ToDecimal(value2);
        //            //ifResult = (cdecValue1 < cdecValue2);
        //            ifResult = value1 < value2;
        //            break;

        //        case "is less than or equal to":
        //            //cdecValue1 = Convert.ToDecimal(value1);
        //            //cdecValue2 = Convert.ToDecimal(value2);
        //            //ifResult = (cdecValue1 <= cdecValue2);
        //            ifResult = value1 <= value2;
        //            break;
        //        default:
        //            throw new Exception("Strange Operand " + param["Operand"]);
        //            break;
        //    }
        //    return ifResult;
        //}

        //private bool DetermineStatementTruth_DateCompare(Engine.AutomationEngineInstance engine)
        //{
        //    //string value1 = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //    //                  where rw.Field<string>("Parameter Name") == "Value1"
        //    //                  select rw.Field<string>("Parameter Value")).FirstOrDefault());
        //    //string operand = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //    //                   where rw.Field<string>("Parameter Name") == "Operand"
        //    //                   select rw.Field<string>("Parameter Value")).FirstOrDefault());
        //    //string value2 = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //    //                  where rw.Field<string>("Parameter Name") == "Value2"
        //    //                  select rw.Field<string>("Parameter Value")).FirstOrDefault());

        //    //value1 = value1.ConvertToUserVariable(sender);
        //    //value2 = value2.ConvertToUserVariable(sender);

        //    var param = DataTableControls.GetFieldValues(v_IfActionParameterTable, "Parameter Name", "Parameter Value");

        //    string operand = param["Operand"].ConvertToUserVariable(engine);

        //    DateTime dt1 = param["Value1"].ConvertToUserVariableAsDate("Value1", engine);
        //    DateTime dt2 = param["Value2"].ConvertToUserVariableAsDate("Value2", engine);

        //    //DateTime dt1, dt2;
        //    //dt1 = DateTime.Parse(value1);
        //    //dt2 = DateTime.Parse(value2);

        //    bool ifResult;
        //    switch (operand.ToLower())
        //    {
        //        case "is equal to":
        //            ifResult = (dt1 == dt2);
        //            break;

        //        case "is not equal to":
        //            ifResult = (dt1 != dt2);
        //            break;

        //        case "is greater than":
        //            ifResult = (dt1 > dt2);
        //            break;

        //        case "is greater than or equal to":
        //            ifResult = (dt1 >= dt2);
        //            break;

        //        case "is less than":
        //            ifResult = (dt1 < dt2);
        //            break;

        //        case "is less than or equal to":
        //            ifResult = (dt1 <= dt2);
        //            break;

        //        default:
        //            throw new Exception("Strange Operand " + param["Operand"]);
        //            break;
        //    }
        //    return ifResult;
        //}

        //private bool DetermineStatementTruth_VariableCompare(Engine.AutomationEngineInstance engine)
        //{
        //    //string value1 = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //    //                  where rw.Field<string>("Parameter Name") == "Value1"
        //    //                  select rw.Field<string>("Parameter Value")).FirstOrDefault());
        //    //string operand = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //    //                   where rw.Field<string>("Parameter Name") == "Operand"
        //    //                   select rw.Field<string>("Parameter Value")).FirstOrDefault());
        //    //string value2 = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //    //                  where rw.Field<string>("Parameter Name") == "Value2"
        //    //                  select rw.Field<string>("Parameter Value")).FirstOrDefault());

        //    //string caseSensitive = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //    //                         where rw.Field<string>("Parameter Name") == "Case Sensitive"
        //    //                         select rw.Field<string>("Parameter Value")).FirstOrDefault());

        //    //value1 = value1.ConvertToUserVariable(sender);
        //    //value2 = value2.ConvertToUserVariable(sender);

        //    var param = DataTableControls.GetFieldValues(v_IfActionParameterTable, "Parameter Name", "Parameter Value", engine);

        //    //if (caseSensitive == "No")
        //    //{
        //    //    value1 = value1.ToUpper();
        //    //    value2 = value2.ToUpper();
        //    //}

        //    string value1 = param["Value1"];
        //    string value2 = param["Value2"];
        //    if (param["Case Sensitive"].ToLower() == "no")
        //    {
        //        value1 = value1.ToLower();
        //        value2 = value2.ToLower();
        //    }

        //    bool ifResult;
        //    switch (param["Operand"].ToLower())
        //    {
        //        case "contains":
        //            ifResult = (value1.Contains(value2));
        //            break;

        //        case "does not contain":
        //            ifResult = (!value1.Contains(value2));
        //            break;

        //        case "is equal to":
        //            ifResult = (value1 == value2);
        //            break;

        //        case "is not equal to":
        //            ifResult = (value1 != value2);
        //            break;

        //        default:
        //            throw new Exception("Strange Operand " + param["Operand"]);
        //            break;
        //    }
        //    return ifResult;
        //}

        //private bool DetermineStatementTruth_VariableHasValue(Engine.AutomationEngineInstance engine)
        //{
        //    //string variableName = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //    //                        where rw.Field<string>("Parameter Name") == "Variable Name"
        //    //                        select rw.Field<string>("Parameter Value")).FirstOrDefault());

        //    //var actualVariable = variableName.ConvertToUserVariable(sender).Trim();

        //    var param = DataTableControls.GetFieldValues(v_IfActionParameterTable, "Parameter Name", "Parameter Value", engine);

        //    string actualVariable = param["Variable Name"].Trim();

        //    //if (!string.IsNullOrEmpty(actualVariable))
        //    //{
        //    //    ifResult = true;
        //    //}
        //    //else
        //    //{
        //    //    ifResult = false;
        //    //}

        //    return (!string.IsNullOrEmpty(actualVariable));
        //}

        //private bool DetermineStatementTruth_VariableIsNumeric(Engine.AutomationEngineInstance engine)
        //{
        //    //string variableName = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //    //                        where rw.Field<string>("Parameter Name") == "Variable Name"
        //    //                        select rw.Field<string>("Parameter Value")).FirstOrDefault());

        //    //var actualVariable = variableName.ConvertToUserVariable(sender).Trim();

        //    var dic = DataTableControls.GetFieldValues(v_IfActionParameterTable, "Parameter Name", "Parameter Value", engine);

        //    var numericTest = decimal.TryParse(dic["Variable Name"], out decimal parsedResult);

        //    //if (numericTest)
        //    //{
        //    //    ifResult = true;
        //    //}
        //    //else
        //    //{
        //    //    ifResult = false;
        //    //}
        //    return numericTest;
        //}

        //private bool DetermineStatementTruth_ErrorOccur(Engine.AutomationEngineInstance engine, bool inverseResult = false)
        //{
        //    ////get line number
        //    //string userLineNumber = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //    //                          where rw.Field<string>("Parameter Name") == "Line Number"
        //    //                          select rw.Field<string>("Parameter Value")).FirstOrDefault());

        //    ////convert to variable
        //    //string variableLineNumber = userLineNumber.ConvertToUserVariable(sender);

        //    ////convert to int
        //    //int lineNumber = int.Parse(variableLineNumber);

        //    var param = DataTableControls.GetFieldValues(v_IfActionParameterTable, "Parameter Name", "Parameter Value");
        //    int lineNumber = param["Line Number"].ConvertToUserVariableAsInteger("Line Number", engine);

        //    bool result;

        //    ////determine if error happened
        //    //if (engine.ErrorsOccured.Where(f => f.LineNumber == lineNumber).Count() == 0)
        //    //{
        //    //    result = true;
        //    //}
        //    //else
        //    //{
        //    //    var error = engine.ErrorsOccured.Where(f => f.LineNumber == lineNumber).FirstOrDefault();
        //    //    error.ErrorMessage.StoreInUserVariable(engine, "Error.Message");
        //    //    error.LineNumber.ToString().StoreInUserVariable(engine, "Error.Line");
        //    //    error.StackTrace.StoreInUserVariable(engine, "Error.StackTrace");

        //    //    result = false;
        //    //}

        //    //determine if error happened
        //    if (engine.ErrorsOccured.Where(f => f.LineNumber == lineNumber).Count() > 0)
        //    {

        //        var error = engine.ErrorsOccured.Where(f => f.LineNumber == lineNumber).FirstOrDefault();
        //        error.ErrorMessage.StoreInUserVariable(engine, "Error.Message");
        //        error.LineNumber.ToString().StoreInUserVariable(engine, "Error.Line");
        //        error.StackTrace.StoreInUserVariable(engine, "Error.StackTrace");

        //        result = true;
        //    }
        //    else
        //    {
        //        result = false;
        //    }

        //    return inverseResult ? !result : result;
        //}
        //private bool DetermineStatementTruth_WindowNameExists(Engine.AutomationEngineInstance engine)
        //{
        //    ////get user supplied name
        //    //string windowName = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //    //                      where rw.Field<string>("Parameter Name") == "Window Name"
        //    //                      select rw.Field<string>("Parameter Value")).FirstOrDefault());
        //    ////variable translation
        //    //string variablizedWindowName = windowName.ConvertToUserVariable(sender);

        //    var param = DataTableControls.GetFieldValues(v_IfActionParameterTable, "Parameter Name", "Parameter Value", engine);

        //    //search for window
        //    IntPtr windowPtr = User32Functions.FindWindow(param["Window Name"]);

        //    ////conditional
        //    //if (windowPtr != IntPtr.Zero)
        //    //{
        //    //    ifResult = true;
        //    //}
        //    return (windowPtr != IntPtr.Zero);
        //}
        //private bool DetermineStatementTruth_ActiveWindow(Engine.AutomationEngineInstance engine)
        //{
        //    //string windowName = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //    //                      where rw.Field<string>("Parameter Name") == "Window Name"
        //    //                      select rw.Field<string>("Parameter Value")).FirstOrDefault());

        //    //string variablizedWindowName = windowName.ConvertToUserVariable(sender);

        //    var param = DataTableControls.GetFieldValues(v_IfActionParameterTable, "Parameter Name", "Parameter Value", engine);

        //    var currentWindowTitle = User32Functions.GetActiveWindowTitle();

        //    //if (currentWindowTitle == variablizedWindowName)
        //    //{
        //    //    ifResult = true;
        //    //}
        //    return (currentWindowTitle == param["Window Name"]);
        //}
        //private bool DetermineStatementTruth_File(Engine.AutomationEngineInstance engine)
        //{
        //    //string fileName = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //    //                    where rw.Field<string>("Parameter Name") == "File Path"
        //    //                    select rw.Field<string>("Parameter Value")).FirstOrDefault());

        //    //string trueWhenFileExists = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //    //                              where rw.Field<string>("Parameter Name") == "True When"
        //    //                              select rw.Field<string>("Parameter Value")).FirstOrDefault());

        //    var param = DataTableControls.GetFieldValues(v_IfActionParameterTable, "Parameter Name", "Parameter Value", engine);

        //    //var userFileSelected = fileName.ConvertToUserVariable(sender);

        //    //bool existCheck = false;
        //    //if (trueWhenFileExists == "It Does Exist")
        //    //{
        //    //    existCheck = true;
        //    //}


        //    //if (System.IO.File.Exists(userFileSelected) == existCheck)
        //    //{
        //    //    ifResult = true;
        //    //}

        //    bool existCheck = System.IO.File.Exists(param["File Path"]);
        //    switch (param["True When"].ToLower())
        //    {
        //        case "it does exist":
        //            return existCheck;
        //            break;

        //        case "it does not exist":
        //            return !existCheck;
        //            break;

        //        default:
        //            throw new Exception("True When is strange value " + param["True When"]);
        //            break;
        //    }
        //}
        //private bool DetermineStatementTruth_Folder(Engine.AutomationEngineInstance engine)
        //{
        //    //string folderName = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //    //                      where rw.Field<string>("Parameter Name") == "Folder Path"
        //    //                      select rw.Field<string>("Parameter Value")).FirstOrDefault());

        //    //string trueWhenFileExists = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //    //                              where rw.Field<string>("Parameter Name") == "True When"
        //    //                              select rw.Field<string>("Parameter Value")).FirstOrDefault());

        //    var param = DataTableControls.GetFieldValues(v_IfActionParameterTable, "Parameter Name", "Parameter Value", engine);

        //    //var userFolderSelected = folderName.ConvertToUserVariable(sender);

        //    //bool existCheck = false;
        //    //if (trueWhenFileExists == "It Does Exist")
        //    //{
        //    //    existCheck = true;
        //    //}


        //    //if (System.IO.Directory.Exists(folderName) == existCheck)
        //    //{
        //    //    ifResult = true;
        //    //}

        //    bool existCheck = System.IO.Directory.Exists(param["Folder Path"]);
        //    switch(param["True When"].ToLower())
        //    {
        //        case "it does exist":
        //            return existCheck;
        //            break;

        //        case "it does not exist":
        //            return !existCheck;
        //            break;

        //        default:
        //            throw new Exception("True When is strange value " + param["True When"]);
        //            break;
        //    }
        //}

        //private bool DetermineStatementTruth_WebElement(Engine.AutomationEngineInstance engine)
        //{
        //    //string instanceName = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //    //                        where rw.Field<string>("Parameter Name") == "Selenium Instance Name"
        //    //                        select rw.Field<string>("Parameter Value")).FirstOrDefault());

        //    //string parameterName = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //    //                         where rw.Field<string>("Parameter Name") == "Element Search Parameter"
        //    //                         select rw.Field<string>("Parameter Value")).FirstOrDefault());

        //    //string searchMethod = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //    //                        where rw.Field<string>("Parameter Name") == "Element Search Method"
        //    //                        select rw.Field<string>("Parameter Value")).FirstOrDefault());

        //    var param = DataTableControls.GetFieldValues(v_IfActionParameterTable, "Parameter Name", "Parameter Value", engine);

        //    SeleniumBrowserElementActionCommand newElementActionCommand = new SeleniumBrowserElementActionCommand();
        //    newElementActionCommand.v_SeleniumSearchType = param["Element Search Method"];
        //    newElementActionCommand.v_InstanceName = param["Selenium Instance Name"];
        //    bool elementExists = newElementActionCommand.ElementExists(engine, param["Element Search Method"], param["Element Search Parameter"]);
        //    return elementExists;
        //}

        //private bool DetermineStatementTruth_GUIElement(Engine.AutomationEngineInstance engine)
        //{
        //    //string windowName = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //    //                      where rw.Field<string>("Parameter Name") == "Window Name"
        //    //                      select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertToUserVariable(sender));

        //    //string elementSearchParam = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //    //                              where rw.Field<string>("Parameter Name") == "Element Search Parameter"
        //    //                              select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertToUserVariable(sender));

        //    //string elementSearchMethod = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //    //                               where rw.Field<string>("Parameter Name") == "Element Search Method"
        //    //                               select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertToUserVariable(sender));

        //    var param = DataTableControls.GetFieldValues(v_IfActionParameterTable, "Parameter Name", "Parameter Value", engine);
        //    string windowName = param["Window Name"];

        //    if (windowName == engine.engineSettings.CurrentWindowKeyword)
        //    {
        //        windowName = User32Functions.GetActiveWindowTitle();
        //    }

        //    UIAutomationCommand newUIACommand = new UIAutomationCommand();
        //    newUIACommand.v_WindowName = windowName;
        //    newUIACommand.v_UIASearchParameters.Rows.Add(true, param["Element Search Method"], param["Element Search Parameter"]);
        //    var handle = newUIACommand.SearchForGUIElement(engine, windowName);

        //    //if (handle is null)
        //    //{
        //    //    ifResult = false;
        //    //}
        //    //else
        //    //{
        //    //    ifResult = true;
        //    //}
        //    return !(handle is null);
        //}
        //private bool DetermineStatementTruth_Boolean(Engine.AutomationEngineInstance engine)
        //{
        //    //string value = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //    //                 where rw.Field<string>("Parameter Name") == "Variable Name"
        //    //                 select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertToUserVariable(sender));

        //    //string compare = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //    //                   where rw.Field<string>("Parameter Name") == "Value Is"
        //    //                   select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertToUserVariable(sender));

        //    var param = DataTableControls.GetFieldValues(v_IfActionParameterTable, "Parameter Name", "Parameter Value");

        //    bool value = param["Variable Name"].ConvertToUserVariableAsBool("Variable Name", engine);
        //    string compare = param["Value Is"].ConvertToUserVariable(engine);

        //    switch (compare.ToLower())
        //    {
        //        case "true":
        //            return value;
        //            break;
        //        case "false":
        //            return !value;
        //            break;
        //        default:
        //            throw new Exception("Value Is " + param["Value Is"] + " is not support.");
        //            break;
        //    }
        //}


        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //IfGridViewHelper = new DataGridView();
            //IfGridViewHelper.AllowUserToAddRows = true;
            //IfGridViewHelper.AllowUserToDeleteRows = true;
            //IfGridViewHelper.Size = new Size(400, 250);
            //IfGridViewHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //IfGridViewHelper.DataBindings.Add("DataSource", this, "v_IfActionParameterTable", false, DataSourceUpdateMode.OnPropertyChanged);
            //IfGridViewHelper.AllowUserToAddRows = false;
            //IfGridViewHelper.AllowUserToDeleteRows = false;
            IfGridViewHelper = CommandControls.CreateDefaultDataGridViewFor("v_IfActionParameterTable", this, false, false, false, 400, 200);
            IfGridViewHelper.MouseEnter += (sender, e) => IfGridViewHelper_MouseEnter(sender,e, editor);
            //IfGridViewHelper.CellClick += IfGridViewHelper_CellClick;
            //IfGridViewHelper.CellBeginEdit += IfGridViewHelper_CellBeginEdit;
            IfGridViewHelper.CellClick += DataTableControls.FirstColumnReadonlySubsequentEditableDataGridView_CellClick;
            IfGridViewHelper.CellBeginEdit += DataTableControls.FirstColumnReadonlySubsequentEditableDataGridView_CellBeginEdit;

            //var helperTheme = editor.Theme.UIHelper;
            //RecorderControl = new taskt.UI.CustomControls.CommandItemControl();
            //RecorderControl.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            ////RecorderControl.ForeColor = Color.AliceBlue;
            //RecorderControl.Font = new Font(helperTheme.Font, helperTheme.FontSize, helperTheme.Style);
            //RecorderControl.ForeColor = helperTheme.FontColor;
            //RecorderControl.BackColor = helperTheme.BackColor;
            //RecorderControl.Name = "guirecorder_helper";
            //RecorderControl.CommandImage = UI.Images.GetUIImage("ClipboardGetTextCommand");
            //RecorderControl.CommandDisplay = "Element Recorder";
            //RecorderControl.Click += ShowIfElementRecorder;
            //RecorderControl.Hide();

            ActionDropdown = (ComboBox)CommandControls.CreateDefaultDropdownFor("v_IfActionType", this);
            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_IfActionType", this));
            RenderedControls.AddRange(CommandControls.CreateDefaultUIHelpersFor("v_IfActionType", this, ActionDropdown, editor));
            ActionDropdown.SelectionChangeCommitted += (sender, e) => ifAction_SelectionChangeCommitted(sender, e, editor);

            RenderedControls.Add(ActionDropdown);

            ParameterControls = new List<Control>();
            ParameterControls.Add(CommandControls.CreateDefaultLabelFor("v_IfActionParameterTable", this));
            //ParameterControls.Add(RecorderControl);

            var helpers = CommandControls.CreateDefaultUIHelpersFor("v_IfActionParameterTable", this, IfGridViewHelper, editor);

            lnkBrowserInstanceSelector = CommandControls.CreateSimpleUIHelper(nameof(v_IfActionParameterTable) + "_customhelper_0", IfGridViewHelper);
            lnkBrowserInstanceSelector.Name = "v_IfActionParameterTable_helper_WebBrowser";
            lnkBrowserInstanceSelector.CommandDisplay = "Select WebBrowser Instance";
            lnkBrowserInstanceSelector.Click += (sender, e) => linkWebBrowserInstanceSelector_Click(sender, e, editor);
            //RenderedControls.Add(lnkBrowserInstance);
            helpers.Add(lnkBrowserInstanceSelector);

            lnkWindowNameSelector = CommandControls.CreateSimpleUIHelper(nameof(v_IfActionParameterTable) + "_customhelper_1", IfGridViewHelper);
            lnkWindowNameSelector.Name = "v_IfActionParameterTable_helper_WindowName";
            lnkWindowNameSelector.CommandDisplay = "Select Window Name";
            lnkWindowNameSelector.Click += (sender, e) => linkWindowNameSelector_Click(sender, e, editor);
            helpers.Add(lnkWindowNameSelector);

            lnkBooleanSelector = CommandControls.CreateSimpleUIHelper(nameof(v_IfActionParameterTable) + "_customhelper_2", IfGridViewHelper);
            lnkBooleanSelector.Name = "v_IfActionParameterTable_helper_Boolean";
            lnkBooleanSelector.CommandDisplay = "Select Boolean Instance";
            lnkBooleanSelector.Click += (sender, e) => linkBooleanInstanceSelector_Click(sender, e, editor);
            helpers.Add(lnkBooleanSelector);

            ParameterControls.AddRange(helpers);
            ParameterControls.Add(IfGridViewHelper);

            RenderedControls.AddRange(ParameterControls);

            return RenderedControls;
        }

        private void linkWebBrowserInstanceSelector_Click(object sender, EventArgs e, frmCommandEditor editor)
        {
            var instances = editor.instanceList.getInstanceClone(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.WebBrowser);

            using (var frm = new UI.Forms.Supplemental.frmItemSelector(instances.Keys.ToList<string>()))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    string selectedItem = frm.selectedItem.ToString();
                    if (!DataTableControls.SetParameterValue(v_IfActionParameterTable, selectedItem, "Selenium Instance Name", "Parameter Name", "Parameter Value"))
                    {
                        throw new Exception("Fail update Selenium Instance Name");
                    }
                }
            }
        }

        private void linkWindowNameSelector_Click(object sender, EventArgs e, frmCommandEditor editor)
        {
            List<string> windowNames = new List<string>();

            windowNames.Add(editor.appSettings.EngineSettings.CurrentWindowKeyword);

            System.Diagnostics.Process[] processlist = System.Diagnostics.Process.GetProcesses();
            //pull the main window title for each
            foreach (var process in processlist)
            {
                if (!String.IsNullOrEmpty(process.MainWindowTitle))
                {
                    //add to the control list of available windows
                    windowNames.Add(process.MainWindowTitle);
                }
            }

            using (var frm = new UI.Forms.Supplemental.frmItemSelector(windowNames))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    string selectedItem = frm.selectedItem.ToString();
                    if (!DataTableControls.SetParameterValue(v_IfActionParameterTable, selectedItem, "Window Name", "Parameter Name", "Parameter Value"))
                    {
                        throw new Exception("Fail update Window Name");
                    }
                }
            }
        }

        private void linkBooleanInstanceSelector_Click(object sender, EventArgs e, frmCommandEditor editor)
        {
            var instances = editor.instanceList.getInstanceClone(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Boolean);

            using (var frm = new UI.Forms.Supplemental.frmItemSelector(instances.Keys.ToList<string>()))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    string selectedItem = frm.selectedItem.ToString();
                    //int currentRow = IfGridViewHelper.CurrentRow.Index;
                    string parameterName = DataTableControls.GetFieldValue(v_IfActionParameterTable, IfGridViewHelper.CurrentRow.Index, "Parameter Name");

                    switch (v_IfActionType.ToLower())
                    {
                        case "boolean":
                            DataTableControls.SetParameterValue(v_IfActionParameterTable, selectedItem, "Variable Name", "Parameter Name", "Parameter Value");
                            break;

                        case "boolean compare":
                            switch (parameterName)
                            {
                                case "Value1":
                                case "Value2":
                                    DataTableControls.SetParameterValue(v_IfActionParameterTable, selectedItem, parameterName, "Parameter Name", "Parameter Value");
                                    break;
                            }
                            break;
                    }
                }
            }
        }

        private void ifAction_SelectionChangeCommitted(object sender, EventArgs e, frmCommandEditor editor)
        {
            ComboBox ifAction = (ComboBox)ActionDropdown;
            DataGridView ifActionParameterBox = (DataGridView)IfGridViewHelper;

            Core.Automation.Commands.BeginIfCommand cmd = (Core.Automation.Commands.BeginIfCommand)this;
            DataTable actionParameters = cmd.v_IfActionParameterTable;

            //sender is null when command is updating
            if (sender != null)
            {
                actionParameters.Rows.Clear();
            }

            //DataGridViewComboBoxCell comparisonComboBox = new DataGridViewComboBoxCell();

            //recorder control
            //Control recorderControl = (Control)RecorderControl;

            //remove if exists            
            //if (RecorderControl.Visible)
            //{
            //    RecorderControl.Hide();
            //}

            lnkBrowserInstanceSelector.Hide();
            lnkWindowNameSelector.Hide();
            lnkBooleanSelector.Hide();

            switch (ifAction.SelectedItem)
            {
                //case "Value":
                case "Numeric Compare":
                case "Date Compare":
                    ConditionControls.RenderNumericCompare(sender, ifActionParameterBox, actionParameters);
                    break;

                //case "Variable Compare":
                case "Text Compare":
                    ConditionControls.RenderTextCompare(sender, ifActionParameterBox, actionParameters);
                    break;


                case "Variable Has Value":
                case "Variable Is Numeric":
                    ConditionControls.RenderVariableIsHas(sender, ifActionParameterBox, actionParameters);
                    break;

                case "Error Occured":
                case "Error Did Not Occur":
                    ConditionControls.RenderErrorOccur(sender, ifActionParameterBox, actionParameters);
                    break;

                case "Window Name Exists":
                case "Active Window Name Is":
                    ConditionControls.RenderWindowName(sender, ifActionParameterBox, actionParameters);
                    lnkWindowNameSelector.Show();
                    break;

                case "File Exists":
                    ConditionControls.RenderFileExists(sender, ifActionParameterBox, actionParameters);
                    break;

                case "Folder Exists":
                    ConditionControls.RenderFolderExists(sender, ifActionParameterBox, actionParameters);
                    break;

                case "Web Element Exists":
                    ConditionControls.RenderWebElement(sender, ifActionParameterBox, actionParameters, editor.appSettings);
                    lnkBrowserInstanceSelector.Show();
                    break;

                case "GUI Element Exists":
                    ConditionControls.RenderGUIElement(sender, ifActionParameterBox, actionParameters, editor.appSettings);
                    lnkWindowNameSelector.Show();
                    break;

                case "Boolean":
                    ConditionControls.RenderBoolean(sender, ifActionParameterBox, actionParameters);
                    lnkBooleanSelector.Show();
                    break;

                case "Boolean Compare":
                    ConditionControls.RenderBooleanCompare(sender, ifActionParameterBox, actionParameters);
                    lnkBooleanSelector.Show();
                    break;

                case "List Compare":
                    ConditionControls.RenderListCompare(sender, ifActionParameterBox, actionParameters);
                    break;

                case "Dictionary Compare":
                    ConditionControls.RenderDictionaryCompare(sender, ifActionParameterBox, actionParameters);
                    break;

                case "DataTable Compare":
                    ConditionControls.RenderDataTableCompare(sender, ifActionParameterBox, actionParameters);
                    break;

                default:
                    break;
            }
        }

        //private void RenderValueCompare(object sender, DataGridView ifActionParameterBox, DataTable actionParameters)
        //{
        //    ifActionParameterBox.Visible = true;

        //    if (sender != null)
        //    {
        //        actionParameters.Rows.Add("Value1", "");
        //        actionParameters.Rows.Add("Operand", "");
        //        actionParameters.Rows.Add("Value2", "");
        //        ifActionParameterBox.DataSource = actionParameters;
        //    }

        //    //combobox cell for Variable Name
        //    DataGridViewComboBoxCell comparisonComboBox = new DataGridViewComboBoxCell();
        //    comparisonComboBox.Items.Add("is equal to");
        //    comparisonComboBox.Items.Add("is greater than");
        //    comparisonComboBox.Items.Add("is greater than or equal to");
        //    comparisonComboBox.Items.Add("is less than");
        //    comparisonComboBox.Items.Add("is less than or equal to");
        //    comparisonComboBox.Items.Add("is not equal to");

        //    //assign cell as a combobox
        //    ifActionParameterBox.Rows[1].Cells[1] = comparisonComboBox;
        //}

        //private void RenderVariableCompare(object sender, DataGridView ifActionParameterBox, DataTable actionParameters)
        //{
        //    ifActionParameterBox.Visible = true;

        //    if (sender != null)
        //    {
        //        actionParameters.Rows.Add("Value1", "");
        //        actionParameters.Rows.Add("Operand", "");
        //        actionParameters.Rows.Add("Value2", "");
        //        actionParameters.Rows.Add("Case Sensitive", "No");
        //        ifActionParameterBox.DataSource = actionParameters;
        //    }

        //    //combobox cell for Variable Name
        //    DataGridViewComboBoxCell comparisonComboBox = new DataGridViewComboBoxCell();
        //    comparisonComboBox.Items.Add("contains");
        //    comparisonComboBox.Items.Add("does not contain");
        //    comparisonComboBox.Items.Add("is equal to");
        //    comparisonComboBox.Items.Add("is not equal to");

        //    //assign cell as a combobox
        //    ifActionParameterBox.Rows[1].Cells[1] = comparisonComboBox;

        //    DataGridViewComboBoxCell caseSensitiveComboBox = new DataGridViewComboBoxCell();
        //    caseSensitiveComboBox.Items.Add("Yes");
        //    caseSensitiveComboBox.Items.Add("No");

        //    //assign cell as a combobox
        //    ifActionParameterBox.Rows[3].Cells[1] = caseSensitiveComboBox;
        //}

        //private void RenderVariableIsHas(object sender, DataGridView ifActionParameterBox, DataTable actionParameters)
        //{
        //    ifActionParameterBox.Visible = true;
        //    if (sender != null)
        //    {
        //        actionParameters.Rows.Add("Variable Name", "");
        //        ifActionParameterBox.DataSource = actionParameters;
        //    }
        //}

        //private void RenderErrorOccur(object sender, DataGridView ifActionParameterBox, DataTable actionParameters)
        //{
        //    ifActionParameterBox.Visible = true;
        //    if (sender != null)
        //    {
        //        actionParameters.Rows.Add("Line Number", "");
        //        ifActionParameterBox.DataSource = actionParameters;
        //    }
        //}

        //private void RenderWindowName(object sender, DataGridView ifActionParameterBox, DataTable actionParameters)
        //{
        //    ifActionParameterBox.Visible = true;
        //    if (sender != null)
        //    {
        //        actionParameters.Rows.Add("Window Name", "");
        //        ifActionParameterBox.DataSource = actionParameters;
        //    }
        //}

        //private void RenderFileExists(object sender, DataGridView ifActionParameterBox, DataTable actionParameters)
        //{
        //    ifActionParameterBox.Visible = true;
        //    if (sender != null)
        //    {
        //        actionParameters.Rows.Add("File Path", "");
        //        actionParameters.Rows.Add("True When", "It Does Exist");
        //        ifActionParameterBox.DataSource = actionParameters;
        //    }

        //    //combobox cell for Variable Name
        //    DataGridViewComboBoxCell comparisonComboBox = new DataGridViewComboBoxCell();
        //    comparisonComboBox.Items.Add("It Does Exist");
        //    comparisonComboBox.Items.Add("It Does Not Exist");

        //    //assign cell as a combobox
        //    ifActionParameterBox.Rows[1].Cells[1] = comparisonComboBox;
        //}

        //private void RenderFolderExists(object sender, DataGridView ifActionParameterBox, DataTable actionParameters)
        //{
        //    ifActionParameterBox.Visible = true;

        //    if (sender != null)
        //    {
        //        actionParameters.Rows.Add("Folder Path", "");
        //        actionParameters.Rows.Add("True When", "It Does Exist");
        //        ifActionParameterBox.DataSource = actionParameters;
        //    }

        //    //combobox cell for Variable Name
        //    DataGridViewComboBoxCell comparisonComboBox = new DataGridViewComboBoxCell();
        //    comparisonComboBox.Items.Add("It Does Exist");
        //    comparisonComboBox.Items.Add("It Does Not Exist");

        //    //assign cell as a combobox
        //    ifActionParameterBox.Rows[1].Cells[1] = comparisonComboBox;
        //}

        //private void RenderWebElement(object sender, DataGridView ifActionParameterBox, DataTable actionParameters, frmCommandEditor editor)
        //{
        //    ifActionParameterBox.Visible = true;

        //    if (sender != null)
        //    {
        //        actionParameters.Rows.Add("Selenium Instance Name", editor.appSettings.ClientSettings.DefaultBrowserInstanceName);
        //        actionParameters.Rows.Add("Element Search Method", "");
        //        actionParameters.Rows.Add("Element Search Parameter", "");
        //        ifActionParameterBox.DataSource = actionParameters;
        //    }

        //    DataGridViewComboBoxCell comparisonComboBox = new DataGridViewComboBoxCell();
        //    comparisonComboBox.Items.Add("Find Element By XPath");
        //    comparisonComboBox.Items.Add("Find Element By ID");
        //    comparisonComboBox.Items.Add("Find Element By Name");
        //    comparisonComboBox.Items.Add("Find Element By Tag Name");
        //    comparisonComboBox.Items.Add("Find Element By Class Name");
        //    comparisonComboBox.Items.Add("Find Element By CSS Selector");

        //    //assign cell as a combobox
        //    ifActionParameterBox.Rows[1].Cells[1] = comparisonComboBox;
        //}

        //private void RenderGUIElement(object sender, DataGridView ifActionParameterBox, DataTable actionParameters, frmCommandEditor editor)
        //{
        //    ifActionParameterBox.Visible = true;
        //    if (sender != null)
        //    {
        //        actionParameters.Rows.Add("Window Name", editor.appSettings.EngineSettings.CurrentWindowKeyword);
        //        actionParameters.Rows.Add("Element Search Method", "");
        //        actionParameters.Rows.Add("Element Search Parameter", "");
        //        ifActionParameterBox.DataSource = actionParameters;
        //    }

        //    var parameterName = new DataGridViewComboBoxCell();
        //    parameterName.Items.Add("AcceleratorKey");
        //    parameterName.Items.Add("AccessKey");
        //    parameterName.Items.Add("AutomationId");
        //    parameterName.Items.Add("ClassName");
        //    parameterName.Items.Add("FrameworkId");
        //    parameterName.Items.Add("HasKeyboardFocus");
        //    parameterName.Items.Add("HelpText");
        //    parameterName.Items.Add("IsContentElement");
        //    parameterName.Items.Add("IsControlElement");
        //    parameterName.Items.Add("IsEnabled");
        //    parameterName.Items.Add("IsKeyboardFocusable");
        //    parameterName.Items.Add("IsOffscreen");
        //    parameterName.Items.Add("IsPassword");
        //    parameterName.Items.Add("IsRequiredForForm");
        //    parameterName.Items.Add("ItemStatus");
        //    parameterName.Items.Add("ItemType");
        //    parameterName.Items.Add("LocalizedControlType");
        //    parameterName.Items.Add("Name");
        //    parameterName.Items.Add("NativeWindowHandle");
        //    parameterName.Items.Add("ProcessID");

        //    //assign cell as a combobox
        //    ifActionParameterBox.Rows[1].Cells[1] = parameterName;

        //    //RecorderControl.Show();
        //}

        //private void RenderBoolean(object sender, DataGridView ifActionParameterBox, DataTable actionParameters)
        //{
        //    ifActionParameterBox.Visible = true;
        //    if (sender != null)
        //    {
        //        actionParameters.Rows.Add("Variable Name", "");
        //        actionParameters.Rows.Add("Value Is", "True");
        //        ifActionParameterBox.DataSource = actionParameters;
        //    }
        //    //assign cell as a combobox
        //    DataGridViewComboBoxCell booleanParam = new DataGridViewComboBoxCell();
        //    booleanParam.Items.Add("True");
        //    booleanParam.Items.Add("False");
        //    ifActionParameterBox.Rows[1].Cells[1] = booleanParam;

        //    //RecorderControl.Show();
        //}

        //private void ShowIfElementRecorder(object sender, EventArgs e)
        //{
        //    //get command reference
        //    Core.Automation.Commands.UIAutomationCommand cmd = new Core.Automation.Commands.UIAutomationCommand();

        //    //create recorder
        //    UI.Forms.Supplemental.frmThickAppElementRecorder newElementRecorder = new UI.Forms.Supplemental.frmThickAppElementRecorder();
        //    newElementRecorder.searchParameters = cmd.v_UIASearchParameters;

        //    //show form
        //    newElementRecorder.ShowDialog();


        //    var sb = new StringBuilder();
        //    sb.AppendLine("Element Properties Found!");
        //    sb.AppendLine(Environment.NewLine);
        //    sb.AppendLine("Element Search Method - Element Search Parameter");
        //    foreach (DataRow rw in cmd.v_UIASearchParameters.Rows)
        //    {
        //        if (rw.ItemArray[2].ToString().Trim() == string.Empty)
        //            continue;

        //        sb.AppendLine(rw.ItemArray[1].ToString() + " - " + rw.ItemArray[2].ToString());
        //    }

        //    DataGridView ifActionBox = IfGridViewHelper;
        //    ifActionBox.Rows[0].Cells[1].Value = newElementRecorder.cboWindowTitle.Text;

        //    MessageBox.Show(sb.ToString());
        //}
        public override string GetDisplayValue()
        {
            //var param = DataTableControls.GetFieldValues(v_IfActionParameterTable, "Parameter Name", "Parameter Value");

            //switch (v_IfActionType)
            //{
            //    //case "Value":
            //    case "Numeric Compare":
            //    case "Date Compare":
            //    //case "Variable Compare":
            //    case "Text Compare":
            //    case "Boolean Compare":
            //        //string value1 = ((from rw in v_IfActionParameterTable.AsEnumerable()
            //        //                  where rw.Field<string>("Parameter Name") == "Value1"
            //        //                  select rw.Field<string>("Parameter Value")).FirstOrDefault());
            //        //string operand = ((from rw in v_IfActionParameterTable.AsEnumerable()
            //        //                   where rw.Field<string>("Parameter Name") == "Operand"
            //        //                   select rw.Field<string>("Parameter Value")).FirstOrDefault());
            //        //string value2 = ((from rw in v_IfActionParameterTable.AsEnumerable()
            //        //                  where rw.Field<string>("Parameter Name") == "Value2"
            //        //                  select rw.Field<string>("Parameter Value")).FirstOrDefault());

            //        //return "If ([" + v_IfActionType + "] " + value1 + " " + operand + " " + value2 + ")";
            //        return "If ([" + v_IfActionType + "] " + param["Value1"] + " " + param["Operand"] + " " + param["Value2"] + ")";
            //        break;

            //    case "Variable Has Value":
            //        //string variableName = ((from rw in v_IfActionParameterTable.AsEnumerable()
            //        //                  where rw.Field<string>("Parameter Name") == "Variable Name"
            //        //                  select rw.Field<string>("Parameter Value")).FirstOrDefault());

            //        //return "If (Variable " + variableName + " Has Value)";
            //        return "If (Variable " + param["Variable Name"] + " Has Value)";
            //        break;

            //    case "Variable Is Numeric":
            //        //string varName = ((from rw in v_IfActionParameterTable.AsEnumerable()
            //        //                        where rw.Field<string>("Parameter Name") == "Variable Name"
            //        //                        select rw.Field<string>("Parameter Value")).FirstOrDefault());

            //        //return "If (Variable " + varName + " Is Numeric)";
            //        return "If (Variable " + param["Variable Name"] + " Is Numeric)";
            //        break;

            //    case "Error Occured":
            //        //string lineNumber = ((from rw in v_IfActionParameterTable.AsEnumerable()
            //        //                      where rw.Field<string>("Parameter Name") == "Line Number"
            //        //                      select rw.Field<string>("Parameter Value")).FirstOrDefault());

            //        //return "If (Error Occured on Line Number " + lineNumber + ")";
            //        return "If (Error Occured on Line Number " + param["Line Number"] + ")";
            //        break;


            //    case "Error Did Not Occur":
            //        //string lineNum = ((from rw in v_IfActionParameterTable.AsEnumerable()
            //        //                      where rw.Field<string>("Parameter Name") == "Line Number"
            //        //                      select rw.Field<string>("Parameter Value")).FirstOrDefault());

            //        //return "If (Error Did Not Occur on Line Number " + lineNum + ")";
            //        return "If (Error Did Not Occur on Line Number " + param["Line Number"] + ")";
            //        break;

            //    case "Window Name Exists":
            //    case "Active Window Name Is":
            //        //string windowName = ((from rw in v_IfActionParameterTable.AsEnumerable()
            //        //                      where rw.Field<string>("Parameter Name") == "Window Name"
            //        //                      select rw.Field<string>("Parameter Value")).FirstOrDefault());

            //        //return "If " + v_IfActionType + " [Name: " + windowName + "]";
            //        return "If " + v_IfActionType + " [Name: " + param["Window Name"] + "]";
            //        break;

            //    case "File Exists":
            //        //string filePath = ((from rw in v_IfActionParameterTable.AsEnumerable()
            //        //                    where rw.Field<string>("Parameter Name") == "File Path"
            //        //                    select rw.Field<string>("Parameter Value")).FirstOrDefault());

            //        //string fileCompareType = ((from rw in v_IfActionParameterTable.AsEnumerable()
            //        //                           where rw.Field<string>("Parameter Name") == "True When"
            //        //                           select rw.Field<string>("Parameter Value")).FirstOrDefault());


            //        //return "If " + v_IfActionType + " [File: " + filePath + "]";
            //        return "If " + v_IfActionType + " [File: " + param["File Path"] + "]";
            //        break;


            //    case "Folder Exists":
            //        //string folderPath = ((from rw in v_IfActionParameterTable.AsEnumerable()
            //        //                      where rw.Field<string>("Parameter Name") == "Folder Path"
            //        //                      select rw.Field<string>("Parameter Value")).FirstOrDefault());

            //        //string folderCompareType = ((from rw in v_IfActionParameterTable.AsEnumerable()
            //        //                             where rw.Field<string>("Parameter Name") == "True When"
            //        //                             select rw.Field<string>("Parameter Value")).FirstOrDefault());


            //        //return "If " + v_IfActionType + " [Folder: " + folderPath + "]";
            //        return "If " + v_IfActionType + " [Folder: " + param["Folder Path"] + "]";
            //        break;


            //    case "Web Element Exists":
            //        //string parameterName = ((from rw in v_IfActionParameterTable.AsEnumerable()
            //        //                         where rw.Field<string>("Parameter Name") == "Element Search Parameter"
            //        //                         select rw.Field<string>("Parameter Value")).FirstOrDefault());

            //        //string searchMethod = ((from rw in v_IfActionParameterTable.AsEnumerable()
            //        //                        where rw.Field<string>("Parameter Name") == "Element Search Method"
            //        //                        select rw.Field<string>("Parameter Value")).FirstOrDefault());

            //        //return "If Web Element Exists [" + searchMethod + ": " + parameterName + "]";
            //        return "If Web Element Exists [" + param["Element Search Method"] + ": " + param["Element Search Parameter"] + "]";
            //        break;

            //    case "GUI Element Exists":
            //        //string guiWindowName = ((from rw in v_IfActionParameterTable.AsEnumerable()
            //        //                     where rw.Field<string>("Parameter Name") == "Window Name"
            //        //                     select rw.Field<string>("Parameter Value")).FirstOrDefault());

            //        //string guiSearch = ((from rw in v_IfActionParameterTable.AsEnumerable()
            //        //                         where rw.Field<string>("Parameter Name") == "Element Search Parameter"
            //        //                         select rw.Field<string>("Parameter Value")).FirstOrDefault());

            //        //return "If GUI Element Exists [Find " + guiSearch + " Element In " + guiWindowName + "]";
            //        return "If GUI Element Exists [Find " + param["Element Search Parameter"] + " Element In " + param["Window Name"] + "]";
            //        break;

            //    case "Boolean":
            //        //string booleanVariable = ((from rw in v_IfActionParameterTable.AsEnumerable()
            //        //                         where rw.Field<string>("Parameter Name") == "Variable Name"
            //        //                         select rw.Field<string>("Parameter Value")).FirstOrDefault());

            //        //string compareTo = ((from rw in v_IfActionParameterTable.AsEnumerable()
            //        //                     where rw.Field<string>("Parameter Name") == "Value Is"
            //        //                     select rw.Field<string>("Parameter Value")).FirstOrDefault());
            //        //return "If [Boolean] " + booleanVariable + " is " + compareTo;
            //        return "If [Boolean] " + param["Variable Name"] + " is " + param["Value Is"];
            //        break;

            //    case "List Compare":
            //        //var paramList = DataTableControls.GetFieldValues(v_IfActionParameterTable, "Parameter Name", "Parameter Value");
            //        return "If [List Compare] '" + param["List1"] + "' and '" + param["List2"] + "'";

            //    case "Dictionary Compare":
            //        //var paramDic = DataTableControls.GetFieldValues(v_IfActionParameterTable, "Parameter Name", "Parameter Value");
            //        return "If [Dictionary Compare] '" + param["Dictionary1"] + "' and '" + param["Dictionary2"] + "'";

            //    case "DataTable Compare":
            //        //var paramDT = DataTableControls.GetFieldValues(v_IfActionParameterTable, "Parameter Name", "Parameter Value");
            //        return "If [DataTable Compare] '" + param["DataTable1"] + "' and '" + param["DataTable2"] + "'";

            //    default:
            //        return "If .... ";
            //}

            return ConditionControls.GetDisplayValue("If", v_IfActionType, v_IfActionParameterTable);
        }

        //private void IfGridViewHelper_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        //{
        //    if (e.ColumnIndex == 0) 
        //    {
        //        e.Cancel = true;
        //    }
        //}
        //private void IfGridViewHelper_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.ColumnIndex >= 0)
        //    {
        //        if (e.ColumnIndex == 1)
        //        {
        //            var targetCell = IfGridViewHelper.Rows[e.RowIndex].Cells[1];
        //            if (targetCell is DataGridViewTextBoxCell)
        //            {
        //                IfGridViewHelper.BeginEdit(false);
        //            }
        //            else if (targetCell is DataGridViewComboBoxCell && targetCell.Value.ToString() == "")
        //            {
        //                SendKeys.Send("{F4}");
        //            }
        //        }
        //    }
        //    else
        //    {
        //        IfGridViewHelper.EndEdit();
        //    }
        //}

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_IfActionType))
            {
                this.validationResult += "Type is empty.";
                this.IsValid = false;
            }
            else
            {
                string message;
                bool res = true;
                switch (this.v_IfActionType)
                {
                    //case "Value":
                    case "Numeric Compare":
                    case "Date Compare":
                    //case "Variable Compare":
                    case "Text Compare":
                        res = ConditionControls.ValueValidate(v_IfActionParameterTable, out message);
                        break;

                    case "Variable Has Value":
                    case "Variable Is Numeric":
                        res = ConditionControls.VariableValidate(v_IfActionParameterTable, out message);
                        break;

                    case "Window Name Exists":
                    case "Active Window Name Is":
                        res = ConditionControls.WindowValidate(v_IfActionParameterTable, out message);
                        break;

                    case "File Exists":
                        res = ConditionControls.FileValidate(v_IfActionParameterTable, out message);
                        break;

                    case "Folder Exists":
                        res = ConditionControls.FolderValidate(v_IfActionParameterTable, out message);
                        break;

                    case "Web Element Exists":
                        res = ConditionControls.WebValidate(v_IfActionParameterTable, out message);
                        break;

                    case "GUI Element Exists":
                        res = ConditionControls.GUIValidate(v_IfActionParameterTable, out message);
                        break;

                    case "Error Occured":
                    case "Error Did Not Occur":
                        res = ConditionControls.ErrorValidate(v_IfActionParameterTable, out message);
                        break;

                    case "Boolean":
                        res = ConditionControls.BooleanValidate(v_IfActionParameterTable, out message);
                        break;

                    case "Boolean Compare":
                        res = ConditionControls.BooleanCompareValidate(v_IfActionParameterTable, out message);
                        break;

                    case "List Compare":
                        res = ConditionControls.ListCompareValidate(v_IfActionParameterTable, out message);
                        break;

                    case "Dictionary Compare":
                        res = ConditionControls.DictionaryCompareValidate(v_IfActionParameterTable, out message);
                        break;

                    case "DataTable Compare":
                        res = ConditionControls.DataTableCompareValidate(v_IfActionParameterTable, out message);
                        break;

                    default:
                        message = "Strange Action Parameter";
                        res = false;
                        break;
                }

                if (!res)
                {
                    this.validationResult += message;
                    this.IsValid = false;
                }
            }

            return this.IsValid;
        }

        //private void ValueValidate()
        //{
        //    string operand = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //                      where rw.Field<string>("Parameter Name") == "Operand"
        //                      select rw.Field<string>("Parameter Value")).FirstOrDefault());
        //    if (String.IsNullOrEmpty(operand))
        //    {
        //        this.validationResult += "Operand is empty.\n";
        //        this.IsValid = false;
        //    }
        //}

        //private void VariableValidate()
        //{
        //    string v = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //                       where rw.Field<string>("Parameter Name") == "Variable Name"
        //                       select rw.Field<string>("Parameter Value")).FirstOrDefault());
        //    if (String.IsNullOrEmpty(v))
        //    {
        //        this.validationResult += "Variable Name is empty.\n";
        //        this.IsValid = false;
        //    }
        //}

        //private void WindowValidate()
        //{
        //    string windowName = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //                       where rw.Field<string>("Parameter Name") == "Window Name"
        //                       select rw.Field<string>("Parameter Value")).FirstOrDefault());
        //    if (String.IsNullOrEmpty(windowName))
        //    {
        //        this.validationResult += "Window Name is empty.\n";
        //        this.IsValid = false;
        //    }
        //}

        //private void FileValidate()
        //{
        //    string fp = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //                          where rw.Field<string>("Parameter Name") == "File Path"
        //                          select rw.Field<string>("Parameter Value")).FirstOrDefault());
        //    if (String.IsNullOrEmpty(fp))
        //    {
        //        this.validationResult += "File Path is empty.\n";
        //        this.IsValid = false;
        //    }
        //}

        //private void FoloderValidate()
        //{
        //    string fp = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //                  where rw.Field<string>("Parameter Name") == "Folder Path"
        //                  select rw.Field<string>("Parameter Value")).FirstOrDefault());
        //    if (String.IsNullOrEmpty(fp))
        //    {
        //        this.validationResult += "Folder Path is empty.\n";
        //        this.IsValid = false;
        //    }
        //}

        //private void WebValidate()
        //{
        //    string instance = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //                  where rw.Field<string>("Parameter Name") == "Selenium Instance Name"
        //                  select rw.Field<string>("Parameter Value")).FirstOrDefault());

        //    string method = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //                  where rw.Field<string>("Parameter Name") == "Element Search Method"
        //                  select rw.Field<string>("Parameter Value")).FirstOrDefault());

        //    string param = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //                  where rw.Field<string>("Parameter Name") == "Element Search Parameter"
        //                  select rw.Field<string>("Parameter Value")).FirstOrDefault());

        //    if (String.IsNullOrEmpty(instance))
        //    {
        //        this.validationResult += "Browser Instance Name (Selenium Insntance) is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(method))
        //    {
        //        this.validationResult += "Search Method is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(param))
        //    {
        //        this.validationResult += "Search Parameter is empty.\n";
        //        this.IsValid = false;
        //    }
        //}

        //private void GUIValidate()
        //{
        //    string window = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //                        where rw.Field<string>("Parameter Name") == "Window Name"
        //                        select rw.Field<string>("Parameter Value")).FirstOrDefault());

        //    string method = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //                      where rw.Field<string>("Parameter Name") == "Element Search Method"
        //                      select rw.Field<string>("Parameter Value")).FirstOrDefault());

        //    string param = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //                     where rw.Field<string>("Parameter Name") == "Element Search Parameter"
        //                     select rw.Field<string>("Parameter Value")).FirstOrDefault());

        //    if (String.IsNullOrEmpty(window))
        //    {
        //        this.validationResult += "Window Name is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(method))
        //    {
        //        this.validationResult += "Search Method is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(param))
        //    {
        //        this.validationResult += "Search Parameter is empty.\n";
        //        this.IsValid = false;
        //    }
        //}

        //private void ErrorValidate()
        //{
        //    string line = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //                      where rw.Field<string>("Parameter Name") == "Line Number"
        //                      select rw.Field<string>("Parameter Value")).FirstOrDefault());

        //    if (String.IsNullOrEmpty(line))
        //    {
        //        this.validationResult += "Line Number is empty.\n";
        //        this.IsValid = false;
        //    }
        //    else
        //    {
        //        int vLine;
        //        if (int.TryParse(line, out vLine))
        //        {
        //            if (vLine < 1)
        //            {
        //                this.validationResult += "Specify 1 or more to Line Number.\n";
        //                this.IsValid = false;
        //            }
        //        }
        //    }
        //}

        //private void BooleanValidate()
        //{
        //    string variable = ((from rw in v_IfActionParameterTable.AsEnumerable()
        //                    where rw.Field<string>("Parameter Name") == "Variable Name"
        //                    select rw.Field<string>("Parameter Value")).FirstOrDefault());

        //    if (String.IsNullOrEmpty(variable))
        //    {
        //        this.validationResult += "Variable is empty.\n";
        //        this.IsValid = false;
        //    }
        //}

        //private void BooleanCompareValidate()
        //{
        //    var param = DataTableControls.GetFieldValues(v_IfActionParameterTable, "Parameter Name", "Parameter Value");

        //    if (String.IsNullOrEmpty(param["Value1"]))
        //    {
        //        this.validationResult += "Value1 is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(param["Value2"]))
        //    {
        //        this.validationResult += "Value2 is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(param["Operand"]))
        //    {
        //        this.validationResult += "Operand is empty.\n";
        //        this.IsValid = false;
        //    }
        //}

        public override void ConvertToIntermediate(EngineSettings settings, List<Core.Script.ScriptVariable> variables)
        {
            if (this.v_IfActionType == "GUI Element Exists")
            {
                var cnv = new Dictionary<string, string>();
                cnv.Add("v_IfActionParameterTable", "convertToIntermediateWindowName");
                ConvertToIntermediate(settings, cnv, variables);
            }
            else
            {
                base.ConvertToIntermediate(settings, variables);
            }
        }

        public override void ConvertToRaw(EngineSettings settings)
        {
            if (this.v_IfActionType == "GUI Element Exists")
            {
                var cnv = new Dictionary<string, string>();
                cnv.Add("v_IfActionParameterTable", "convertToRawWindowName");
                ConvertToRaw(settings, cnv);
            }
            else
            {
                base.ConvertToRaw(settings);
            }
        }
    }
}