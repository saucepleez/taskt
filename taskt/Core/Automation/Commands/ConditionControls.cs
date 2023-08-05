using System;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using System.Runtime.CompilerServices;

namespace taskt.Core.Automation.Commands
{
    internal static class ConditionControls
    {
        #region Determin Statement Truth
        public static bool DetermineStatementTruth(string actionType, DataTable actionParameterTable, Engine.AutomationEngineInstance engine)
        {
            bool ifResult;

            //string actionType = actionType.ConvertToUserVariable(engine);

            switch (actionType.ConvertToUserVariable(engine).ToLower())
            {
                //case "value":
                case "numeric compare":
                    ifResult = DetermineStatementTruth_NumericCompare(actionParameterTable, engine);
                    break;

                case "date compare":
                    ifResult = DetermineStatementTruth_DateCompare(actionParameterTable, engine);
                    break;

                //case "variable compare":
                case "text compare":
                    ifResult = DetermineStatementTruth_TextCompare(actionParameterTable, engine);
                    break;

                case "variable has value":
                    ifResult = DetermineStatementTruth_VariableHasValue(actionParameterTable, engine);
                    break;

                case "variable is numeric":
                    ifResult = DetermineStatementTruth_VariableIsNumeric(actionParameterTable, engine);
                    break;

                case "error occured":
                    ifResult = DetermineStatementTruth_ErrorOccur(actionParameterTable, engine, false);
                    break;

                case "error did not occur":
                    ifResult = DetermineStatementTruth_ErrorOccur(actionParameterTable, engine, true);
                    break;

                case "window name exists":
                    ifResult = DetermineStatementTruth_WindowNameExists(actionParameterTable, engine);
                    break;

                case "active window name is":
                    ifResult = DetermineStatementTruth_ActiveWindow(actionParameterTable, engine);
                    break;

                case "file exists":
                    ifResult = DetermineStatementTruth_File(actionParameterTable, engine);
                    break;

                case "folder exists":
                    ifResult = DetermineStatementTruth_Folder(actionParameterTable, engine);
                    break;

                case "web element exists":
                    ifResult = DetermineStatementTruth_WebElement(actionParameterTable, engine);
                    break;

                case "gui element exists":
                    ifResult = DetermineStatementTruth_GUIElement(actionParameterTable, engine);
                    break;

                case "boolean":
                    ifResult = DetermineStatementTruth_Boolean(actionParameterTable, engine);
                    break;

                case "boolean compare":
                    ifResult = DetermineStatementTruth_BooleanCompare(actionParameterTable, engine);
                    break;

                case "list compare":
                    ifResult = DetermineStatementTruth_ListCompare(actionParameterTable, engine);
                    break;

                case "dictionary compare":
                    ifResult = DetermineStatementTruth_DictionaryCompare(actionParameterTable, engine);
                    break;

                case "datatable compare":
                    ifResult = DetermineStatementTruth_DataTableCompare(actionParameterTable, engine);
                    break;

                default:
                    throw new Exception("If type not recognized!");
            }
            
            return ifResult;
        }

        private static bool DetermineStatementTruth_NumericCompare(DataTable actionParameterTable, Engine.AutomationEngineInstance engine)
        {
            var param = DataTableControls.GetFieldValues(actionParameterTable, "Parameter Name", "Parameter Value", engine);

            string operand = param["Operand"].ConvertToUserVariable(engine);

            bool isBoolCompare = false;
            decimal value1 = 0;
            decimal value2 = 0;
            switch (operand.ToLower())
            {
                case "is equal to":
                case "is not equal to":
                    bool tempBool;
                    isBoolCompare = bool.TryParse(param["Value1"], out tempBool) && bool.TryParse(param["Value2"], out tempBool);
                    break;
                default:
                    //value1 = param["Value1"].ConvertToUserVariableAsDecimal("Value1", engine);
                    //value2 = param["Value2"].ConvertToUserVariableAsDecimal("Value2", engine);
                    value1 = new PropertyConvertTag(param["Value1"], "Value1").ConvertToUserVariableAsDecimal(engine);
                    value2 = new PropertyConvertTag(param["Value2"], "Value2").ConvertToUserVariableAsDecimal(engine);
                    break;
            }

            bool ifResult;
            switch (operand.ToLower())
            {
                case "is equal to":
                    if (isBoolCompare)
                    {
                        ifResult = (bool.Parse(param["Value1"]) == bool.Parse(param["Value2"]));
                    }
                    else
                    {
                        ifResult = (param["Value1"] == param["Value2"]);
                    }
                    break;

                case "is not equal to":
                    if (isBoolCompare)
                    {
                        ifResult = (bool.Parse(param["Value1"]) != bool.Parse(param["Value2"]));
                    }
                    else
                    {
                        ifResult = (param["Value1"] != param["Value2"]);
                    }
                    break;

                case "is greater than":
                    ifResult = value1 > value2;
                    break;

                case "is greater than or equal to":
                    ifResult = value1 >= value2;
                    break;

                case "is less than":
                    ifResult = value1 < value2;
                    break;

                case "is less than or equal to":
                    ifResult = value1 <= value2;
                    break;
                default:
                    throw new Exception("Strange Operand " + param["Operand"]);
            }
            return ifResult;
        }

        private static bool DetermineStatementTruth_DateCompare(DataTable actionParameterTable, Engine.AutomationEngineInstance engine)
        {
            var param = DataTableControls.GetFieldValues(actionParameterTable, "Parameter Name", "Parameter Value", false);

            string operand = param["Operand"].ConvertToUserVariable(engine);

            DateTime dt1 = param["Value1"].ConvertToUserVariableAsDateTime("Value1", engine);
            DateTime dt2 = param["Value2"].ConvertToUserVariableAsDateTime("Value2", engine);

            bool ifResult;
            switch (operand.ToLower())
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

                default:
                    throw new Exception("Strange Operand " + param["Operand"]);
            }
            return ifResult;
        }

        private static bool DetermineStatementTruth_TextCompare(DataTable actionParameterTable, Engine.AutomationEngineInstance engine)
        {
            var param = DataTableControls.GetFieldValues(actionParameterTable, "Parameter Name", "Parameter Value", engine);

            string value1 = param["Value1"];
            string value2 = param["Value2"];
            if (param["Case Sensitive"].ToLower() == "no")
            {
                value1 = value1.ToLower();
                value2 = value2.ToLower();
            }

            bool ifResult;
            switch (param["Operand"].ToLower())
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

                case "starts with":
                    ifResult = value1.StartsWith(value2);
                    break;

                case "ends with":
                    ifResult = value1.EndsWith(value2);
                    break;

                default:
                    throw new Exception("Strange Operand " + param["Operand"]);
            }
            return ifResult;
        }

        private static bool DetermineStatementTruth_VariableHasValue(DataTable actionParameterTable, Engine.AutomationEngineInstance engine)
        {
            var param = DataTableControls.GetFieldValues(actionParameterTable, "Parameter Name", "Parameter Value", engine);

            string actualVariable = param["Variable Name"].Trim();

            return (!string.IsNullOrEmpty(actualVariable));
        }

        private static bool DetermineStatementTruth_VariableIsNumeric(DataTable actionParameterTable, Engine.AutomationEngineInstance engine)
        {
            var dic = DataTableControls.GetFieldValues(actionParameterTable, "Parameter Name", "Parameter Value", engine);

            var numericTest = decimal.TryParse(dic["Variable Name"], out decimal parsedResult);

            return numericTest;
        }

        private static bool DetermineStatementTruth_ErrorOccur(DataTable actionParameterTable, Engine.AutomationEngineInstance engine, bool inverseResult = false)
        {
            var param = DataTableControls.GetFieldValues(actionParameterTable, "Parameter Name", "Parameter Value", false);
            int lineNumber = param["Line Number"].ConvertToUserVariableAsInteger("Line Number", engine);

            bool result;

            //determine if error happened
            if (engine.ErrorsOccured.Where(f => f.LineNumber == lineNumber).Count() > 0)
            {

                var error = engine.ErrorsOccured.Where(f => f.LineNumber == lineNumber).FirstOrDefault();
                error.ErrorMessage.StoreInUserVariable(engine, "Error.Message");
                error.LineNumber.ToString().StoreInUserVariable(engine, "Error.Line");
                error.StackTrace.StoreInUserVariable(engine, "Error.StackTrace");

                result = true;
            }
            else
            {
                result = false;
            }

            return inverseResult ? !result : result;
        }
        private static bool DetermineStatementTruth_WindowNameExists(DataTable actionParameterTable, Engine.AutomationEngineInstance engine)
        {
            var param = DataTableControls.GetFieldValues(actionParameterTable, "Parameter Name", "Parameter Value", engine);
            try
            {
                IntPtr wHnd = WindowNameControls.FindWindowHandle(param["Window Name"], param["Search Method"], engine);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private static bool DetermineStatementTruth_ActiveWindow(DataTable actionParameterTable, Engine.AutomationEngineInstance engine)
        {
            var param = DataTableControls.GetFieldValues(actionParameterTable, "Parameter Name", "Parameter Value", engine);
            var searchFunc = WindowNameControls.GetWindowNameCompareMethod(param["Search Method"]);
            return (searchFunc(WindowNameControls.GetCurrentWindowName(), param["Window Name"]));
        }
        private static bool DetermineStatementTruth_File(DataTable actionParameterTable, Engine.AutomationEngineInstance engine)
        {
            var param = DataTableControls.GetFieldValues(actionParameterTable, "Parameter Name", "Parameter Value", engine);

            bool existCheck = System.IO.File.Exists(param["File Path"]);
            switch (param["True When"].ToLower())
            {
                case "it does exist":
                    return existCheck;

                case "it does not exist":
                    return !existCheck;

                default:
                    throw new Exception("True When is strange value " + param["True When"]);
            }
        }
        private static bool DetermineStatementTruth_Folder(DataTable actionParamterTable, Engine.AutomationEngineInstance engine)
        {
            var param = DataTableControls.GetFieldValues(actionParamterTable, "Parameter Name", "Parameter Value", engine);

            bool existCheck = System.IO.Directory.Exists(param["Folder Path"]);
            switch (param["True When"].ToLower())
            {
                case "it does exist":
                    return existCheck;

                case "it does not exist":
                    return !existCheck;

                default:
                    throw new Exception("True When is strange value " + param["True When"]);
            }
        }

        private static bool DetermineStatementTruth_WebElement(DataTable actionParameterTable, Engine.AutomationEngineInstance engine)
        {
            var param = DataTableControls.GetFieldValues(actionParameterTable, "Parameter Name", "Parameter Value", engine);

            //SeleniumBrowserElementActionCommand newElementActionCommand = new SeleniumBrowserElementActionCommand();
            //newElementActionCommand.v_SeleniumSearchType = param["Element Search Method"];
            //newElementActionCommand.v_InstanceName = param["WebBrowser Instance Name"];
            //bool elementExists = newElementActionCommand.ElementExists(engine, param["Element Search Method"], param["Element Search Parameter"]);

            var checkWebElement = new SeleniumBrowserCheckWebElementExistsCommand()
            {
                v_InstanceName = param["WebBrowser Instance Name"],
                v_SeleniumSearchType = param["Element Search Method"],
                v_SeleniumSearchParameter = param["Element Search Parameter"],
                v_Result = VariableNameControls.GetInnerVariableName(0, engine),
            };
            checkWebElement.RunCommand(engine);

            return VariableNameControls.GetInnerVariable(0, engine).VariableValue.ToString().ConvertToUserVariableAsBool("Result", engine);
        }

        private static bool DetermineStatementTruth_GUIElement(DataTable actionParameterTable, Engine.AutomationEngineInstance engine)
        {
            var param = DataTableControls.GetFieldValues(actionParameterTable, "Parameter Name", "Parameter Value", engine);
            string windowName = param["Window Name"];

            if (windowName == engine.engineSettings.CurrentWindowKeyword)
            {
                //windowName = User32.User32Functions.GetActiveWindowTitle();
                windowName = WindowNameControls.GetActiveWindowTitle();
            }

            var searchTb = new DataTable();
            searchTb.Columns.Add("Enabled");
            searchTb.Columns.Add("ParameterName");
            searchTb.Columns.Add("ParameterValue");
            searchTb.Rows.Add(true, param["Element Search Method"], param["Element Search Parameter"]);

            var vName = VariableNameControls.GetInnerVariableName(2, engine);

            var actionTb = new DataTable();
            actionTb.Columns.Add("Parameter Name");
            actionTb.Columns.Add("Parameter Value");
            actionTb.Rows.Add("Apply To Variable", vName);

            var checkUI = new UIAutomationUIElementActionCommand();
            checkUI.v_WindowName = windowName;
            checkUI.v_UIASearchParameters = searchTb;
            checkUI.v_AutomationType = "Check UIElement Exists";
            checkUI.v_UIAActionParameters = actionTb;
            checkUI.RunCommand(engine);

            return vName.ConvertToUserVariableAsBool("result", engine);

            //UIAutomationUIElementActionCommand newUIACommand = new UIAutomationUIElementActionCommand();
            //newUIACommand.v_WindowName = windowName;
            //newUIACommand.v_UIASearchParameters.Rows.Add(true, param["Element Search Method"], param["Element Search Parameter"]);
            //newUIACommand.RunCommand(engine);

            //var handle = newUIACommand.SearchForGUIElement(engine, windowName);

            //var chkUIElem = new UIAutomationUIElementActionCommand();
            //chkUIElem.v_WindowName = windowName;
            //chkUIElem.v_AutomationType = "Check UIElement Exists";

            //var searchTable = new DataTable();
            //searchTable.Columns.Add("Enabled");
            //searchTable.Columns.Add("Parameter Name");
            //searchTable.Columns.Add("Parameter Value");
            //searchTable.Rows.Add(true, param["Element Search Method"], param["Element Search Parameter"]);
            //chkUIElem.v_UIASearchParameters = searchTable;
            ////chkUIElem.v_UIAActionParameters.Rows.Add("")

            //var r = VariableNameControls.GetInnerVariableName(2, engine);
            //var actionTable = new DataTable();
            //actionTable.Columns.Add("Parameter Name");
            //actionTable.Columns.Add("Parameter Value");
            //actionTable.Rows.Add("Apply To Variable", r);
            //chkUIElem.v_UIAActionParameters = actionTable;

            //chkUIElem.RunCommand(engine);

            //var x = VariableNameControls.GetInnerVariable(2, engine);

            //object handle = null;

            //return !(handle is null);
        }
        private static bool DetermineStatementTruth_Boolean(DataTable actionParamterTable, Engine.AutomationEngineInstance engine)
        {
            var param = DataTableControls.GetFieldValues(actionParamterTable, "Parameter Name", "Parameter Value", false);

            bool value = param["Variable Name"].ConvertToUserVariableAsBool("Variable Name", engine);
            string compare = param["Value Is"].ConvertToUserVariable(engine);

            switch (compare.ToLower())
            {
                case "true":
                    return value;
                case "false":
                    return !value;
                default:
                    throw new Exception("Value Is " + param["Value Is"] + " is not support.");
            }
        }
        private static bool DetermineStatementTruth_BooleanCompare(DataTable actionParamterTable, Engine.AutomationEngineInstance engine)
        {
            var param = DataTableControls.GetFieldValues(actionParamterTable, "Parameter Name", "Parameter Value", false);

            bool value1 = param["Value1"].ConvertToUserVariableAsBool("Variable Name", engine);
            bool value2 = param["Value2"].ConvertToUserVariableAsBool("Variable Name", engine);
            string operand = param["Operand"].ConvertToUserVariable(engine);

            switch (operand.ToLower())
            {
                case "is equal to":
                    return (value1 == value2);

                case "is not equal to":
                    return (value1 != value2);

                case "both are true":
                    return (value1 & value2);

                case "both or one of them are true":
                    return (value1 | value2);

                default:
                    throw new Exception("Value Is " + param["Value Is"] + " is not support.");
            }
        }

        private static bool DetermineStatementTruth_ListCompare(DataTable actionParamterTable, Engine.AutomationEngineInstance engine)
        {
            var param = DataTableControls.GetFieldValues(actionParamterTable, "Parameter Name", "Parameter Value", false);

            var list1 = param["List1"].GetListVariable(engine);
            var list2 = param["List2"].GetListVariable(engine);

            if (list1.Count == list2.Count)
            {
                for (int i = list1.Count - 1; i >= 0; i--)
                {
                    if (list1[i] != list2[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool DetermineStatementTruth_DictionaryCompare(DataTable actionParamterTable, Engine.AutomationEngineInstance engine)
        {
            var param = DataTableControls.GetFieldValues(actionParamterTable, "Parameter Name", "Parameter Value", false);

            var dic1 = param["Dictionary1"].GetDictionaryVariable(engine);
            var dic2 = param["Dictionary2"].GetDictionaryVariable(engine);

            if (dic1.Count == dic2.Count)
            {
                List<string> keys = dic1.Keys.ToList();
                foreach(var key in keys)
                {
                    if (!dic2.ContainsKey(key))
                    {
                        return false;
                    }
                    else if (dic1[key] != dic2[key])
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool DetermineStatementTruth_DataTableCompare(DataTable actionParamterTable, Engine.AutomationEngineInstance engine)
        {
            var param = DataTableControls.GetFieldValues(actionParamterTable, "Parameter Name", "Parameter Value", false);

            var dt1 = param["DataTable1"].GetDataTableVariable(engine);
            var dt2 = param["DataTable2"].GetDataTableVariable(engine);

            if ((dt1.Rows.Count == dt2.Rows.Count) && (dt1.Columns.Count == dt2.Columns.Count))
            {
                // Get columns name list
                List<string> columns = new List<string>();
                for (int i = 0; i < dt1.Columns.Count; i++)
                {
                    columns.Add(dt1.Columns[i].ColumnName);
                }
                // check columns exists
                for (int i = 0; i < columns.Count; i++)
                {
                    int j;
                    for (j = 0; j < dt2.Columns.Count; j++)
                    {
                        if (columns[i] == dt2.Columns[j].ColumnName)
                        {
                            break;
                        }
                    }
                    if (j == columns.Count)
                    {
                        return false;
                    }
                }

                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    for (int j = 0; j < columns.Count; j++)
                    {
                        string value1 = (dt1.Rows[i][columns[j]] == null) ? "" : dt1.Rows[i][columns[j]].ToString();
                        string value2 = (dt2.Rows[i][columns[j]] == null) ? "" : dt2.Rows[i][columns[j]].ToString();
                        if (value1 != value2)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Render

        public static void RenderNumericCompare(object sender, DataGridView actionParameterBox, DataTable actionParameters)
        {
            actionParameterBox.Visible = true;

            if (sender != null)
            {
                actionParameters.Rows.Add("Value1", "");
                actionParameters.Rows.Add("Operand", "");
                actionParameters.Rows.Add("Value2", "");
                actionParameterBox.DataSource = actionParameters;
            }

            //combobox cell for Variable Name
            DataGridViewComboBoxCell comparisonComboBox = new DataGridViewComboBoxCell();
            comparisonComboBox.Items.Add("is equal to");
            comparisonComboBox.Items.Add("is greater than");
            comparisonComboBox.Items.Add("is greater than or equal to");
            comparisonComboBox.Items.Add("is less than");
            comparisonComboBox.Items.Add("is less than or equal to");
            comparisonComboBox.Items.Add("is not equal to");

            //assign cell as a combobox
            actionParameterBox.Rows[1].Cells[1] = comparisonComboBox;
        }

        public static void RenderTextCompare(object sender, DataGridView actionParameterBox, DataTable actionParameters)
        {
            actionParameterBox.Visible = true;

            if (sender != null)
            {
                actionParameters.Rows.Add("Value1", "");
                actionParameters.Rows.Add("Operand", "");
                actionParameters.Rows.Add("Value2", "");
                actionParameters.Rows.Add("Case Sensitive", "No");
                actionParameterBox.DataSource = actionParameters;
            }

            //combobox cell for Variable Name
            DataGridViewComboBoxCell comparisonComboBox = new DataGridViewComboBoxCell();
            comparisonComboBox.Items.Add("contains");
            comparisonComboBox.Items.Add("does not contain");
            comparisonComboBox.Items.Add("is equal to");
            comparisonComboBox.Items.Add("is not equal to");
            comparisonComboBox.Items.Add("starts with");
            comparisonComboBox.Items.Add("ends with");

            //assign cell as a combobox
            actionParameterBox.Rows[1].Cells[1] = comparisonComboBox;

            DataGridViewComboBoxCell caseSensitiveComboBox = new DataGridViewComboBoxCell();
            caseSensitiveComboBox.Items.Add("Yes");
            caseSensitiveComboBox.Items.Add("No");

            //assign cell as a combobox
            actionParameterBox.Rows[3].Cells[1] = caseSensitiveComboBox;
        }

        public static void RenderVariableIsHas(object sender, DataGridView actionParameterBox, DataTable actionParameters)
        {
            actionParameterBox.Visible = true;
            if (sender != null)
            {
                actionParameters.Rows.Add("Variable Name", "");
                actionParameterBox.DataSource = actionParameters;
            }
        }

        public static void RenderErrorOccur(object sender, DataGridView actionParameterBox, DataTable actionParameters)
        {
            actionParameterBox.Visible = true;
            if (sender != null)
            {
                actionParameters.Rows.Add("Line Number", "");
                actionParameterBox.DataSource = actionParameters;
            }
        }

        public static void RenderWindowName(object sender, DataGridView actionParameterBox, DataTable actionParameters)
        {
            actionParameterBox.Visible = true;
            if (sender != null)
            {
                actionParameters.Rows.Add("Window Name", "");
                actionParameters.Rows.Add("Search Method", "Contains");
                actionParameterBox.DataSource = actionParameters;
            }

            //combobox cell for Variable Name
            DataGridViewComboBoxCell comparisonComboBox = new DataGridViewComboBoxCell();
            comparisonComboBox.Items.Add("Contains");
            comparisonComboBox.Items.Add("Starts with");
            comparisonComboBox.Items.Add("Ends with");
            comparisonComboBox.Items.Add("Exact match");

            //assign cell as a combobox
            actionParameterBox.Rows[1].Cells[1] = comparisonComboBox;
        }

        public static void RenderFileExists(object sender, DataGridView actionParameterBox, DataTable actionParameters)
        {
            actionParameterBox.Visible = true;
            if (sender != null)
            {
                actionParameters.Rows.Add("File Path", "");
                actionParameters.Rows.Add("True When", "It Does Exist");
                actionParameterBox.DataSource = actionParameters;
            }

            //combobox cell for Variable Name
            DataGridViewComboBoxCell comparisonComboBox = new DataGridViewComboBoxCell();
            comparisonComboBox.Items.Add("It Does Exist");
            comparisonComboBox.Items.Add("It Does Not Exist");

            //assign cell as a combobox
            actionParameterBox.Rows[1].Cells[1] = comparisonComboBox;
        }

        public static void RenderFolderExists(object sender, DataGridView ifActionParameterBox, DataTable actionParameters)
        {
            ifActionParameterBox.Visible = true;

            if (sender != null)
            {
                actionParameters.Rows.Add("Folder Path", "");
                actionParameters.Rows.Add("True When", "It Does Exist");
                ifActionParameterBox.DataSource = actionParameters;
            }

            //combobox cell for Variable Name
            DataGridViewComboBoxCell comparisonComboBox = new DataGridViewComboBoxCell();
            comparisonComboBox.Items.Add("It Does Exist");
            comparisonComboBox.Items.Add("It Does Not Exist");

            //assign cell as a combobox
            ifActionParameterBox.Rows[1].Cells[1] = comparisonComboBox;
        }

        public static void RenderWebElement(object sender, DataGridView actionParameterBox, DataTable actionParameters, ApplicationSettings settings)
        {
            actionParameterBox.Visible = true;

            if (sender != null)
            {
                actionParameters.Rows.Add("WebBrowser Instance Name", settings.ClientSettings.DefaultBrowserInstanceName);
                actionParameters.Rows.Add("Element Search Method", "");
                actionParameters.Rows.Add("Element Search Parameter", "");
                actionParameterBox.DataSource = actionParameters;
            }

            DataGridViewComboBoxCell comparisonComboBox = new DataGridViewComboBoxCell();
            comparisonComboBox.Items.Add("Find Element By XPath");
            comparisonComboBox.Items.Add("Find Element By ID");
            comparisonComboBox.Items.Add("Find Element By Name");
            comparisonComboBox.Items.Add("Find Element By Tag Name");
            comparisonComboBox.Items.Add("Find Element By Class Name");
            comparisonComboBox.Items.Add("Find Element By CSS Selector");

            //assign cell as a combobox
            actionParameterBox.Rows[1].Cells[1] = comparisonComboBox;
        }

        public static void RenderGUIElement(object sender, DataGridView actionParameterBox, DataTable actionParameters, ApplicationSettings settings)
        {
            actionParameterBox.Visible = true;
            if (sender != null)
            {
                actionParameters.Rows.Add("Window Name", settings.EngineSettings.CurrentWindowKeyword);
                actionParameters.Rows.Add("Element Search Method", "");
                actionParameters.Rows.Add("Element Search Parameter", "");
                actionParameterBox.DataSource = actionParameters;
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
            actionParameterBox.Rows[1].Cells[1] = parameterName;
        }

        public static void RenderBoolean(object sender, DataGridView actionParameterBox, DataTable actionParameters)
        {
            actionParameterBox.Visible = true;
            if (sender != null)
            {
                actionParameters.Rows.Add("Variable Name", "");
                actionParameters.Rows.Add("Value Is", "True");
                actionParameterBox.DataSource = actionParameters;
            }
            //assign cell as a combobox
            DataGridViewComboBoxCell booleanParam = new DataGridViewComboBoxCell();
            booleanParam.Items.Add("True");
            booleanParam.Items.Add("False");
            actionParameterBox.Rows[1].Cells[1] = booleanParam;
        }

        public static void RenderBooleanCompare(object sender, DataGridView actionParameterBox, DataTable actionParameters)
        {
            actionParameterBox.Visible = true;
            if (sender != null)
            {
                actionParameters.Rows.Add("Value1", "");
                actionParameters.Rows.Add("Operand", "");
                actionParameters.Rows.Add("Value2", "");
                actionParameterBox.DataSource = actionParameters;
            }
            //assign cell as a combobox
            DataGridViewComboBoxCell booleanParam = new DataGridViewComboBoxCell();
            booleanParam.Items.Add("is equal to");
            booleanParam.Items.Add("is not equal to");
            booleanParam.Items.Add("both are True");
            booleanParam.Items.Add("both or one of them are True");
            actionParameterBox.Rows[1].Cells[1] = booleanParam;
        }

        public static void RenderListCompare(object sender, DataGridView actionParameterBox, DataTable actionParameters)
        {
            actionParameterBox.Visible = true;
            if (sender != null)
            {
                actionParameters.Rows.Add("List1", "");
                actionParameters.Rows.Add("List2", "");
                actionParameterBox.DataSource = actionParameters;
            }
        }

        public static void RenderDictionaryCompare(object sender, DataGridView actionParameterBox, DataTable actionParameters)
        {
            actionParameterBox.Visible = true;
            if (sender != null)
            {
                actionParameters.Rows.Add("Dictionary1", "");
                actionParameters.Rows.Add("Dictionary2", "");
                actionParameterBox.DataSource = actionParameters;
            }
        }

        public static void RenderDataTableCompare(object sender, DataGridView actionParameterBox, DataTable actionParameters)
        {
            actionParameterBox.Visible = true;
            if (sender != null)
            {
                actionParameters.Rows.Add("DataTable1", "");
                actionParameters.Rows.Add("DataTable2", "");
                actionParameterBox.DataSource = actionParameters;
            }
        }
        #endregion

        #region Validate

        public static bool ValueValidate(DataTable actionParameters, out string result)
        {
            string operand = DataTableControls.GetFieldValue(actionParameters, "Operand", "Parameter Name", "Parameter Value");
            result = "";
            if (String.IsNullOrEmpty(operand))
            {
                result += "Operand is empty.\n";
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool VariableValidate(DataTable actionParameters, out string result)
        {
            string variableName = DataTableControls.GetFieldValue(actionParameters, "Variable Name", "Parameter Name", "Parameter Value");
            result = "";
            if (String.IsNullOrEmpty(variableName))
            {
                result += "Variable Name is empty.\n";
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool WindowValidate(DataTable actionParameters, out string result)
        {
            string windowName = DataTableControls.GetFieldValue(actionParameters, "Window Name", "Parameter Name", "Parameter Value");
            result = "";
            if (String.IsNullOrEmpty(windowName))
            {
                result += "Window Name is empty.\n";
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool FileValidate(DataTable actionParameters, out string result)
        {
            string filePath = DataTableControls.GetFieldValue(actionParameters, "File Path", "Parameter Name", "Parameter Value");
            result = "";
            if (String.IsNullOrEmpty(filePath))
            {
                result += "File Path is empty.\n";
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool FolderValidate(DataTable actionParameters, out string result)
        {
            string folderPath = DataTableControls.GetFieldValue(actionParameters, "Folder Path", "Parameter Name", "Parameter Value");
            result = "";
            if (String.IsNullOrEmpty(folderPath))
            {
                result += "Folder Path is empty.\n";
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool WebValidate(DataTable actionParameters, out string result)
        {
            var param = DataTableControls.GetFieldValues(actionParameters, "Parameter Name", "Parameter Value", false);
            result = "";
            if (String.IsNullOrEmpty(param["WebBrowser Instance Name"]))
            {
                result += "WebBrowser Instance Name is empty.\n";
            }
            if (String.IsNullOrEmpty(param["Element Search Method"]))
            {
                result += "Search Method is empty.\n";
            }
            if (String.IsNullOrEmpty(param["Element Search Parameter"]))
            {
                result += "Search Parameter is empty.\n";
            }

            return (result == "");
        }

        public static bool GUIValidate(DataTable actionParameters, out string result)
        {
            var param = DataTableControls.GetFieldValues(actionParameters, "Parameter Name", "Parameter Value", false);
            result = "";

            if (String.IsNullOrEmpty(param["Window Name"]))
            {
                result += "Window Name is empty.\n";
            }
            if (String.IsNullOrEmpty(param["Element Search Method"]))
            {
                result += "Search Method is empty.\n";
            }
            if (String.IsNullOrEmpty(param["Element Search Parameter"]))
            {
                result += "Search Parameter is empty.\n";
            }

            return (result == "");
        }

        public static bool ErrorValidate(DataTable actionParameters, out string result)
        {
            string line = DataTableControls.GetFieldValue(actionParameters, "Line Number", "Parameter Name", "Parameter Value");
            result = "";

            if (String.IsNullOrEmpty(line))
            {
                result += "Line Number is empty.\n";
            }
            else
            {
                int vLine;
                if (int.TryParse(line, out vLine))
                {
                    if (vLine < 1)
                    {
                        result += "Specify 1 or more to Line Number.\n";
                    }
                }
            }
            return (result == "");
        }

        public static bool BooleanValidate(DataTable actionParameters, out string result)
        {
            string variable = DataTableControls.GetFieldValue(actionParameters, "Variable Name", "Parameter Name", "Parameter Value");
            result = "";

            if (String.IsNullOrEmpty(variable))
            {
                result += "Variable is empty.\n";
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool BooleanCompareValidate(DataTable actionParameters, out string result)
        {
            var param = DataTableControls.GetFieldValues(actionParameters, "Parameter Name", "Parameter Value", false);
            result = "";

            if (String.IsNullOrEmpty(param["Value1"]))
            {
                result += "Value1 is empty.\n";
            }
            if (String.IsNullOrEmpty(param["Value2"]))
            {
                result += "Value2 is empty.\n";
            }
            if (String.IsNullOrEmpty(param["Operand"]))
            {
                result += "Operand is empty.\n";
            }

            return (result == "");
        }

        public static bool ListCompareValidate(DataTable actionParameters, out string result)
        {
            var param = DataTableControls.GetFieldValues(actionParameters, "Parameter Name", "Parameter Value", false);
            result = "";

            if (String.IsNullOrEmpty(param["List1"]))
            {
                result += "List1 is empty.\n";
            }
            if (String.IsNullOrEmpty(param["List2"]))
            {
                result += "List2 is empty.\n";
            }

            return (result == "");
        }

        public static bool DictionaryCompareValidate(DataTable actionParameters, out string result)
        {
            var param = DataTableControls.GetFieldValues(actionParameters, "Parameter Name", "Parameter Value", false);
            result = "";

            if (String.IsNullOrEmpty(param["Dictionary1"]))
            {
                result += "Dictionary1 is empty.\n";
            }
            if (String.IsNullOrEmpty(param["Dictionary2"]))
            {
                result += "Dictionary2 is empty.\n";
            }

            return (result == "");
        }

        public static bool DataTableCompareValidate(DataTable actionParameters, out string result)
        {
            var param = DataTableControls.GetFieldValues(actionParameters, "Parameter Name", "Parameter Value", false);
            result = "";

            if (String.IsNullOrEmpty(param["DataTable1"]))
            {
                result += "DataTable1 is empty.\n";
            }
            if (String.IsNullOrEmpty(param["DataTable2"]))
            {
                result += "DataTable2 is empty.\n";
            }

            return (result == "");
        }
        #endregion

        #region Display Value

        public static string GetDisplayValue(string commandPrefix, string actionType, DataTable parameterTable, string parameterNameColumn = "Parameter Name", string parameterValueColumn = "Parameter Value")
        {
            var param = DataTableControls.GetFieldValues(parameterTable, parameterNameColumn, parameterValueColumn, false);

            if (String.IsNullOrEmpty(actionType))
            {
                actionType = "";
            }

            switch (actionType.ToLower())
            {
                case "numeric compare":
                    return commandPrefix + " [Numeric Compare] (" + param["Value1"] + " " + param["Operand"] + " " + param["Value2"] + ")";

                case "date compare":
                    return commandPrefix + " [Date Compare] (" + param["Value1"] + " " + param["Operand"] + " " + param["Value2"] + ")";

                case "text compare":
                    return commandPrefix + " [Text Compare] (" + param["Value1"] + " " + param["Operand"] + " " + param["Value2"] + ")";

                case "variable has value":
                    return commandPrefix + " (Variable " + param["Variable Name"] + " Has Value)";

                case "variable is numeric":
                    return commandPrefix + " (Variable " + param["Variable Name"] + " Is Numeric)";

                case "error occured":
                    return commandPrefix + " (Error Occured on Line Number " + param["Line Number"] + ")";


                case "error did not occur":
                    return commandPrefix + " (Error Did Not Occur on Line Number " + param["Line Number"] + ")";

                case "window name exists":
                    return commandPrefix + " [Window Name Exists] (Name: " + param["Window Name"] + ")";

                case "active window name is":
                    return commandPrefix + " [Active Window Name Is] (Name: " + param["Window Name"] + ")";

                case "file exists":
                    return commandPrefix + " [File Exists] (File: " + param["File Path"] + ")";


                case "folder exists":
                    return commandPrefix + " [Folder Exists] (Folder: " + param["Folder Path"] + ")";


                case "web element exists":
                    return commandPrefix + " [Web Element Exists] (" + param["Element Search Method"] + ": " + param["Element Search Parameter"] + ")";

                case "gui element exists":
                    return commandPrefix + " [GUI Element Exists] (Find " + param["Element Search Parameter"] + " Element In " + param["Window Name"] + ")";

                case "boolean":
                    return commandPrefix + " [Boolean] (" + param["Variable Name"] + " is " + param["Value Is"] + ")";

                case "boolean compare":
                    return commandPrefix + " [Boolean Compare] (" + param["Value1"] + " " + param["Operand"] + " " + param["Value2"] + ")";

                case "list compare":
                    return commandPrefix + " [List Compare] ('" + param["List1"] + "' and '" + param["List2"] + "')";

                case "dictionary compare":
                    return commandPrefix + " [Dictionary Compare] ('" + param["Dictionary1"] + "' and '" + param["Dictionary2"] + "')";

                case "datatable compare":
                    return commandPrefix + " [DataTable Compare] ('" + param["DataTable1"] + "' and '" + param["DataTable2"] + "')";

                default:
                    return commandPrefix + " .... ";
            }
        }
        #endregion

        #region Properties Filter

        /// <summary>
        /// filter type property, please specify ProeprtySelectionChangeEvent in commands
        /// </summary>
        [PropertyDescription("Type of Values to be Filterd")]
        [InputSpecification("", true)]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Text")]
        [PropertyUISelectionOption("Numeric")]
        [PropertyValidationRule("Type", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Type")]
        [PropertyFirstValue("Text")]
        [PropertyDetailSampleUsage("**Text**", PropertyDetailSampleUsage.ValueType.Value, "Type of Values")]
        [PropertyDetailSampleUsage("**Numeric**", PropertyDetailSampleUsage.ValueType.Value, "Type of Values")]
        [PropertyDetailSampleUsage("**{{{vType}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Type of Values")]

        public static string v_FilterValueType { get; }

        /// <summary>
        /// filter action property, please specify PropertySelectionChangeEvent in commands
        /// </summary>
        [PropertyDescription("Filter Action")]
        [InputSpecification("", true)]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("Filter Action", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Action")]
        public static string v_FilterAction { get; }

        /// <summary>
        /// filter/replace action parameters property
        /// </summary>
        [PropertyDescription("Additional Parameters")]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.DataGridView)]
        [PropertyDataGridViewSetting(false, false, true, 400, 120)]
        [PropertyDataGridViewColumnSettings("ParameterName", "Parameter Name", true)]
        [PropertyDataGridViewColumnSettings("ParameterValue", "Parameter Value", false)]
        [PropertyDataGridViewCellEditEvent(nameof(DataTableControls) + "+" + nameof(DataTableControls.FirstColumnReadonlySubsequentEditableDataGridView_CellBeginEdit), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellBeginEdit)]
        [PropertyDataGridViewCellEditEvent(nameof(DataTableControls) + "+" + nameof(DataTableControls.FirstColumnReadonlySubsequentEditableDataGridView_CellClick), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellClick)]
        public static string v_ActionParameterTable { get; }

        /// <summary>
        /// Replace value type property, please specify PropertySelectionChangeEvent in commands
        /// </summary>
        [PropertyDescription("Type of Values to be Replaced")]
        [InputSpecification("", true)]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Text")]
        [PropertyUISelectionOption("Numeric")]
        [PropertyValidationRule("Target Type", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Type")]
        [PropertyDetailSampleUsage("**Text**", PropertyDetailSampleUsage.ValueType.Value, "Type of Values")]
        [PropertyDetailSampleUsage("**Numeric**", PropertyDetailSampleUsage.ValueType.Value, "Type of Values")]
        [PropertyDetailSampleUsage("**{{{vType}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Type of Values")]
        [PropertyFirstValue("Text")]
        public static string v_ReplaceValueType { get; }

        /// <summary>
        /// Replace action property, please specify PropertySelectionChangeEvent in commands
        /// </summary>
        [PropertyDescription("Replace Action")]
        [InputSpecification("", true)]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("Replace Action", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Action")]
        public static string v_ReplaceAction { get; }

        /// <summary>
        /// Replace value property
        /// </summary>
        [PropertyDescription("Replace Value")]
        [InputSpecification("Replace Value", true)]
        [PropertyDetailSampleUsage("**1**", "Replace with **1**")]
        [PropertyDetailSampleUsage("**a**", "Replace with **a**")]
        [PropertyDetailSampleUsage("**{{{vValue}}}**", "Replace with the Value of Variable **{{{vValue}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDisplayText(true, "Replace Value")]
        [PropertyIsOptional(true, "Empty")]
        public static string v_ReplaceValue { get; }

        #endregion

        #region ComboBox Items Filter

        public static void AddFilterActionItems(System.Windows.Forms.ComboBox cmbType, System.Windows.Forms.ComboBox cmbAction)
        {
            switch (cmbType.SelectedItem?.ToString().ToLower() ?? "")
            {
                case "text":
                    AddFilterActionItems_Text(cmbAction);
                    break;

                case "numeric":
                    AddFilterActionItems_Numeric(cmbAction);
                    break;
            }
        }

        private static void AddFilterActionItems_Text(System.Windows.Forms.ComboBox cmb)
        {
            cmb.BeginUpdate();
            cmb.Items.Clear();

            cmb.Items.AddRange(new string[]
            {
                "Contains", "Not Contains",
                "Starts with", "Not starts with",
                "Ends With", "Not Ends with",
                "Is Equal To", "Is Not Equal To",
                "Is Numeric", "Is Not Numeric"
            });

            cmb.EndUpdate();
        }

        private static void AddFilterActionItems_Numeric(System.Windows.Forms.ComboBox cmb)
        {
            cmb.BeginUpdate();
            cmb.Items.Clear();

            cmb.Items.AddRange(new string[]
            {
                "Is Equal To", "Is Not Equal To",
                "Is Greater than", "Is Greater Than or Equal To",
                "Is Less than", "Is Less Than or Equal To",
                "Between", "Not Between"
            });

            cmb.EndUpdate();
        }

        public static void RenderFilter(DataTable actionParameters, DataGridView actionParametersBox, ComboBox actionType, ComboBox dataType)
        {
            switch (dataType.SelectedItem?.ToString().ToLower() ?? "")
            {
                case "text":
                    RenderFilter_Text(actionParameters, actionParametersBox, actionType);
                    break;
                case "numeric":
                    RenderFilter_Numeric(actionParameters, actionParametersBox, actionType);
                    break;
            }
        }

        private static void RenderFilter_Text(DataTable actionParameters, DataGridView actionParametersBox, ComboBox actionType)
        {
            string actionValue;
            if (actionType.SelectedItem != null)
            {
                actionValue = actionType.SelectedItem.ToString().ToLower();
            }
            else
            {
                actionValue = actionType.Text.ToLower();
            }

            switch (actionValue)
            {
                case "is numeric":
                case "is not numeric":
                    // no parameters
                    if (!DataTableControls.IsParameterNamesExists(actionParameters, new List<string>()))
                    {
                        actionParameters.Rows.Clear();
                    }
                    break;
                default:
                    if (!DataTableControls.IsParameterNamesExists(actionParameters, new List<string>() { "Value", "Case Sensitive" }))
                    {
                        actionParameters.Rows.Clear();
                        actionParameters.Rows.Add("Value", "");
                        actionParameters.Rows.Add("Case Sensitive", "No");
                    }
                    break;
            }

            actionParametersBox.DataSource = actionParameters;

            switch (actionValue)
            {
                case "is numeric":
                case "is not numeric":
                    break;
                default:
                    int x = actionParametersBox.Rows.Count;
                    var cmb = new DataGridViewComboBoxCell();
                    cmb.Items.AddRange(new string[] { "Yes", "No" });
                    actionParametersBox.Rows[1].Cells[1] = cmb;
                    break;
            }
        }

        private static void RenderFilter_Numeric(DataTable actionParameters, DataGridView actionParametersBox, ComboBox actionType)
        {
            string actionValue;
            if (actionType.SelectedItem != null)
            {
                actionValue = actionType.SelectedItem.ToString().ToLower();
            }
            else
            {
                actionValue = actionType.Text.ToLower();
            }

            switch (actionValue)
            {
                case "between":
                case "not between":
                    if (!DataTableControls.IsParameterNamesExists(actionParameters, new List<string>() { "Value1", "Value2", "If Not Numeric" }))
                    {
                        actionParameters.Rows.Clear();
                        actionParameters.Rows.Add("Value1", "");
                        actionParameters.Rows.Add("Value2", "");
                        actionParameters.Rows.Add("If Not Numeric", "Error");
                    }
                    break;
                default:
                    if (!DataTableControls.IsParameterNamesExists(actionParameters, new List<string>() { "Value", "If Not Numeric" }))
                    {
                        actionParameters.Rows.Clear();
                        actionParameters.Rows.Add("Value", "");
                        actionParameters.Rows.Add("If Not Numeric", "Error");
                    }
                    break;
            }

            actionParametersBox.DataSource = actionParameters;

            var cmb = new DataGridViewComboBoxCell();
            cmb.Items.AddRange(new string[] { "Error", "Ignore"});
            switch (actionValue)
            {
                case "between":
                case "not between":
                    actionParametersBox.Rows[2].Cells[1] = cmb;
                    break;
                default:
                    actionParametersBox.Rows[1].Cells[1] = cmb;
                    break;
            }
        }

        #endregion

        #region Filter Determin Statement

        /// <summary>
        /// Get Check Methods to Filter/Replace for List/Dictionary/DataTable commands
        /// </summary>
        /// <param name="targetTypeName"></param>
        /// <param name="filterActionName"></param>
        /// <param name="parameters"></param>
        /// <param name="engine"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public static Func<string, Dictionary<string, string>, bool> GetFilterDeterminStatementTruthFunc(string targetTypeName, string filterActionName, Dictionary<string, string> parameters, taskt.Core.Automation.Engine.AutomationEngineInstance engine, ScriptCommand command)
        {
            string tp = command.GetUISelectionValue(targetTypeName, "Target Type", engine);
            switch (tp)
            {
                case "text":
                    return GetFilterDeterminStatementTruthFunc_Text(filterActionName, parameters, engine, command);

                default:
                    return GetFilterDeterminStatementTruthFunc_Numeric(filterActionName, parameters, engine, command); ;
            }
        }

        private static Func<string, Dictionary<string, string>, bool> GetFilterDeterminStatementTruthFunc_Text(string filterActionName, Dictionary<string, string> parameters, taskt.Core.Automation.Engine.AutomationEngineInstance engine, ScriptCommand command)
        {
            string filterAction = command.ConvertToUserVariable(filterActionName, "Filter Action", engine).ToLower();
            Func<string, string, bool> checkFunc = null;
            switch (filterAction)
            {
                case "contains":
                    checkFunc = (a, b) =>
                    {
                        return a.Contains(b);
                    };
                    break;
                case "not contains":
                    checkFunc = (a, b) =>
                    {
                        return !a.Contains(b);
                    };
                    break;
                case "starts with":
                    checkFunc = (a, b) =>
                    {
                        return a.StartsWith(b);
                    };
                    break;
                case "not starts with":
                    checkFunc = (a, b) =>
                    {
                        return !a.StartsWith(b);
                    };
                    break;
                case "ends with":
                    checkFunc = (a, b) =>
                    {
                        return a.EndsWith(b);
                    };
                    break;
                case "not ends with":
                    checkFunc = (a, b) =>
                    {
                        return !a.EndsWith(b);
                    };
                    break;
                case "is equal to":
                    checkFunc = (a, b) =>
                    {
                        return (a == b);
                    };
                    break;
                case "is not equal to":
                    checkFunc = (a, b) =>
                    {
                        return (a != b);
                    };
                    break;

                case "is numeric":
                    checkFunc = (a, b) =>
                    {
                        return decimal.TryParse(a, out _);
                    };
                    break;
                case "is not numeric":
                    checkFunc = (a, b) =>
                    {
                        return !decimal.TryParse(a, out _);
                    };
                    break;
                default:
                    throw new Exception("Strange Filter Type '" + filterAction + "'.");
            }

            Func<string, Dictionary<string, string>, bool> retFunc;
            switch (filterAction)
            {
                case "is numeric":
                case "is not numeric":
                    retFunc = (targetText, p) =>
                    {
                        return checkFunc(targetText, "");
                    };
                    break;
                default:
                    if (parameters["Case Sensitive"].ToLower() == "no")
                    {
                        retFunc = (targetText, p) =>
                        {
                            return checkFunc(targetText.ToLower(), p["Value"].ToLower());
                        };
                    }
                    else
                    {
                        retFunc = (targetText, p) =>
                        {
                            return checkFunc(targetText, p["Value"]);
                        };
                    }
                    break;
            }
            return retFunc;
        }
        private static Func<string, Dictionary<string, string>, bool> GetFilterDeterminStatementTruthFunc_Numeric(string filterActionName, Dictionary<string, string> parameters, taskt.Core.Automation.Engine.AutomationEngineInstance engine, ScriptCommand command)
        {
            string filterAction = command.ConvertToUserVariable(filterActionName, "Filter Action", engine).ToLower();

            Func<string, Dictionary<string, string>, (decimal trgValue, decimal value1, decimal value2)> convFunc;
            switch (filterAction)
            {
                case "between":
                case "not between":
                    convFunc = (txt, p) =>
                    {
                        decimal tv = new PropertyConvertTag(txt, "Value").ConvertToUserVariableAsDecimal(engine);
                        decimal v1 = new PropertyConvertTag(p["Value1"], "Compared Value1").ConvertToUserVariableAsDecimal(engine);
                        decimal v2 = new PropertyConvertTag(p["Value2"], "Compared Value2").ConvertToUserVariableAsDecimal(engine);
                        return (tv, v1, v2);
                    };
                    break;
                default:
                    convFunc = (txt, p) =>
                    {
                        decimal tv = new PropertyConvertTag(txt, "Value").ConvertToUserVariableAsDecimal(engine);
                        decimal v1 = new PropertyConvertTag(p["Value"], "Compared Value").ConvertToUserVariableAsDecimal(engine);
                        return (tv, v1, 0);
                    };
                    break;
            }

            Func<string, Dictionary<string, string>, bool> checkFunc = null;
            switch (filterAction)
            {
                case "is equal to":
                    checkFunc = (targetText, p) =>
                    {
                        (decimal trgValue, decimal value1, _) = convFunc(targetText, p);
                        return (trgValue == value1);
                    };
                    break;
                case "is not equal to":
                    checkFunc = (targetText, p) =>
                    {
                        (decimal trgValue, decimal value1, _) = convFunc(targetText, p);
                        return (trgValue != value1);
                    };
                    break;
                case "is greater than":
                    checkFunc = (targetText, p) =>
                    {
                        (decimal trgValue, decimal value1, _) = convFunc(targetText, p);
                        return (trgValue > value1);
                    };
                    break;
                case "is greater than or equal to":
                    checkFunc = (targetText, p) =>
                    {
                        (decimal trgValue, decimal value1, _) = convFunc(targetText, p);
                        return (trgValue >= value1);
                    };
                    break;
                case "is less than":
                    checkFunc = (targetText, p) =>
                    {
                        (decimal trgValue, decimal value1, _) = convFunc(targetText, p);
                        return (trgValue < value1);
                    };
                    break;
                case "is less than or equal to":
                    checkFunc = (targetText, p) =>
                    {
                        (decimal trgValue, decimal value1, _) = convFunc(targetText, p);
                        return (trgValue <= value1);
                    };
                    break;

                case "between":
                    checkFunc = (targetText, p) =>
                    {
                        (decimal trgValue, decimal value1, decimal value2) = convFunc(targetText, p);
                        return ((trgValue >= value1) && (trgValue <= value2));
                    };
                    break;
                case "not between":
                    checkFunc = (targetText, p) =>
                    {
                        (decimal trgValue, decimal value1, decimal value2) = convFunc(targetText, p);
                        return ((trgValue < value1) || (trgValue > value2));
                    };
                    break;
                default:
                    throw new Exception("Strange Filter Type '" + filterAction + "'.");
            }

            Func<string, Dictionary<string, string>, bool> retFunc = null;
            switch(parameters["If Not Numeric"].ToLower())
            {
                case "error":
                    retFunc = (targetText, p) =>
                    {
                        try 
                        {
                            return checkFunc(targetText, p);
                        }
                        catch(Exception ex)
                        {
                            throw ex;
                        }
                    };
                    break;
                case "ignore":
                    retFunc = (targetText, p) =>
                    {
                        try
                        {
                            return checkFunc(targetText, p);
                        }
                        catch
                        {
                            return false;
                        }
                    };
                    break;
            }

            return retFunc;
        }
        #endregion
    }
}
