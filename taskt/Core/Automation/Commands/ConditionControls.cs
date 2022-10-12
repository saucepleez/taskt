using System;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskt.Core.Automation.Commands
{
    internal class ConditionControls
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
                    value1 = param["Value1"].ConvertToUserVariableAsDecimal("Value1", engine);
                    value2 = param["Value2"].ConvertToUserVariableAsDecimal("Value2", engine);
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
            var param = DataTableControls.GetFieldValues(actionParameterTable, "Parameter Name", "Parameter Value");

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
            var param = DataTableControls.GetFieldValues(actionParameterTable, "Parameter Name", "Parameter Value");
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
            //var param = DataTableControls.GetFieldValues(actionParamterTable, "Parameter Name", "Parameter Value", engine);

            ////search for window
            //IntPtr windowPtr = User32.User32Functions.FindWindow(param["Window Name"]);

            //return (windowPtr != IntPtr.Zero);

            var param = DataTableControls.GetFieldValues(actionParameterTable, "Parameter Name", "Parameter Value", engine);
            try
            {
                IntPtr wHnd = WindowNameControls.FindWindow(param["Window Name"], param["Search Method"], engine);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private static bool DetermineStatementTruth_ActiveWindow(DataTable actionParameterTable, Engine.AutomationEngineInstance engine)
        {
            //var param = DataTableControls.GetFieldValues(actionParameterTable, "Parameter Name", "Parameter Value", engine);

            //var currentWindowTitle = User32.User32Functions.GetActiveWindowTitle();

            //return (currentWindowTitle == param["Window Name"]);

            var param = DataTableControls.GetFieldValues(actionParameterTable, "Parameter Name", "Parameter Value", engine);
            var searchFunc = WindowNameControls.getWindowSearchMethod(param["Search Method"]);
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

            SeleniumBrowserElementActionCommand newElementActionCommand = new SeleniumBrowserElementActionCommand();
            newElementActionCommand.v_SeleniumSearchType = param["Element Search Method"];
            newElementActionCommand.v_InstanceName = param["WebBrowser Instance Name"];
            bool elementExists = newElementActionCommand.ElementExists(engine, param["Element Search Method"], param["Element Search Parameter"]);
            return elementExists;
        }

        private static bool DetermineStatementTruth_GUIElement(DataTable actionParameterTable, Engine.AutomationEngineInstance engine)
        {
            var param = DataTableControls.GetFieldValues(actionParameterTable, "Parameter Name", "Parameter Value", engine);
            string windowName = param["Window Name"];

            if (windowName == engine.engineSettings.CurrentWindowKeyword)
            {
                windowName = User32.User32Functions.GetActiveWindowTitle();
            }

            UIAutomationCommand newUIACommand = new UIAutomationCommand();
            newUIACommand.v_WindowName = windowName;
            newUIACommand.v_UIASearchParameters.Rows.Add(true, param["Element Search Method"], param["Element Search Parameter"]);
            var handle = newUIACommand.SearchForGUIElement(engine, windowName);

            return !(handle is null);
        }
        private static bool DetermineStatementTruth_Boolean(DataTable actionParamterTable, Engine.AutomationEngineInstance engine)
        {
            var param = DataTableControls.GetFieldValues(actionParamterTable, "Parameter Name", "Parameter Value");

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
            var param = DataTableControls.GetFieldValues(actionParamterTable, "Parameter Name", "Parameter Value");

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
            var param = DataTableControls.GetFieldValues(actionParamterTable, "Parameter Name", "Parameter Value");

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
            var param = DataTableControls.GetFieldValues(actionParamterTable, "Parameter Name", "Parameter Value");

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
            var param = DataTableControls.GetFieldValues(actionParamterTable, "Parameter Name", "Parameter Value");

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
            //string operand = ((from rw in v_IfActionParameterTable.AsEnumerable()
            //                   where rw.Field<string>("Parameter Name") == "Operand"
            //                   select rw.Field<string>("Parameter Value")).FirstOrDefault());
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
            //string v = ((from rw in v_IfActionParameterTable.AsEnumerable()
            //             where rw.Field<string>("Parameter Name") == "Variable Name"
            //             select rw.Field<string>("Parameter Value")).FirstOrDefault());
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
            //string windowName = ((from rw in v_IfActionParameterTable.AsEnumerable()
            //                      where rw.Field<string>("Parameter Name") == "Window Name"
            //                      select rw.Field<string>("Parameter Value")).FirstOrDefault());
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
            //string fp = ((from rw in v_IfActionParameterTable.AsEnumerable()
            //              where rw.Field<string>("Parameter Name") == "File Path"
            //              select rw.Field<string>("Parameter Value")).FirstOrDefault());
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
            //string fp = ((from rw in v_IfActionParameterTable.AsEnumerable()
            //              where rw.Field<string>("Parameter Name") == "Folder Path"
            //              select rw.Field<string>("Parameter Value")).FirstOrDefault());
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
            //string instance = ((from rw in v_IfActionParameterTable.AsEnumerable()
            //                    where rw.Field<string>("Parameter Name") == "Selenium Instance Name"
            //                    select rw.Field<string>("Parameter Value")).FirstOrDefault());

            //string method = ((from rw in v_IfActionParameterTable.AsEnumerable()
            //                  where rw.Field<string>("Parameter Name") == "Element Search Method"
            //                  select rw.Field<string>("Parameter Value")).FirstOrDefault());

            //string param = ((from rw in v_IfActionParameterTable.AsEnumerable()
            //                 where rw.Field<string>("Parameter Name") == "Element Search Parameter"
            //                 select rw.Field<string>("Parameter Value")).FirstOrDefault());

            var param = DataTableControls.GetFieldValues(actionParameters, "Parameter Name", "Parameter Value");
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
            //string window = ((from rw in v_IfActionParameterTable.AsEnumerable()
            //                  where rw.Field<string>("Parameter Name") == "Window Name"
            //                  select rw.Field<string>("Parameter Value")).FirstOrDefault());

            //string method = ((from rw in v_IfActionParameterTable.AsEnumerable()
            //                  where rw.Field<string>("Parameter Name") == "Element Search Method"
            //                  select rw.Field<string>("Parameter Value")).FirstOrDefault());

            //string param = ((from rw in v_IfActionParameterTable.AsEnumerable()
            //                 where rw.Field<string>("Parameter Name") == "Element Search Parameter"
            //                 select rw.Field<string>("Parameter Value")).FirstOrDefault());

            var param = DataTableControls.GetFieldValues(actionParameters, "Parameter Name", "Parameter Value");
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
            //string line = ((from rw in v_IfActionParameterTable.AsEnumerable()
            //                where rw.Field<string>("Parameter Name") == "Line Number"
            //                select rw.Field<string>("Parameter Value")).FirstOrDefault());

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
            //string variable = ((from rw in v_IfActionParameterTable.AsEnumerable()
            //                    where rw.Field<string>("Parameter Name") == "Variable Name"
            //                    select rw.Field<string>("Parameter Value")).FirstOrDefault());

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
            var param = DataTableControls.GetFieldValues(actionParameters, "Parameter Name", "Parameter Value");
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
            var param = DataTableControls.GetFieldValues(actionParameters, "Parameter Name", "Parameter Value");
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
            var param = DataTableControls.GetFieldValues(actionParameters, "Parameter Name", "Parameter Value");
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
            var param = DataTableControls.GetFieldValues(actionParameters, "Parameter Name", "Parameter Value");
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
            var param = DataTableControls.GetFieldValues(parameterTable, parameterNameColumn, parameterValueColumn);

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

        #region ComboBox Items Filter

        public static void AddFilterActionItems(System.Windows.Forms.ComboBox cmbType, System.Windows.Forms.ComboBox cmbAction)
        {
            switch (cmbType.SelectedItem.ToString().ToLower())
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
            switch (dataType.SelectedItem.ToString().ToLower())
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

        public static bool FilterDeterminStatementTruth(string value, string targetType, string filterAction, DataTable filterParameters, taskt.Core.Automation.Engine.AutomationEngineInstance engine)
        {
            var paramDic = DataTableControls.GetFieldValues(filterParameters, "ParameterName", "ParameterValue", engine);

            value = value.ConvertToUserVariable(engine);
            targetType = targetType.ConvertToUserVariable(engine);
            filterAction = filterAction.ConvertToUserVariable(engine);

            switch (targetType.ToLower())
            {
                case "text":
                    return FilterDeterminStatementTruth_Text(value, filterAction, paramDic, engine);

                case "numeric":
                    return FilterDeterminStatementTruth_Numeric(value, filterAction, paramDic, engine);

                default:
                    throw new Exception("Strange Filter Target Type '" + targetType + "'");
            }
        }

        private static bool FilterDeterminStatementTruth_Text(string trgText, string filterAction, Dictionary<string, string> parameters, taskt.Core.Automation.Engine.AutomationEngineInstance engine)
        {
            string value = "";

            filterAction = filterAction.ToLower();
            switch (filterAction)
            {
                case "is numeric":
                case "is not numeric":
                    break;
                default:
                    if (parameters["Case Sensitive"].ToLower() == "no")
                    {
                        trgText = trgText.ToLower();
                        value = parameters["Value"].ToLower();
                    }
                    else
                    {
                        value = parameters["Value"];
                    }
                    break;
            }

            switch (filterAction)
            {
                case "contains":
                    return trgText.Contains(value);
                case "not contains":
                    return !trgText.Contains(value);
                case "starts with":
                    return trgText.StartsWith(value);
                case "not starts with":
                    return !trgText.StartsWith(value);
                case "ends with":
                    return trgText.EndsWith(value);
                case "not ends with":
                    return !trgText.EndsWith(value);
                case "is equal to":
                    return (trgText == value);
                case "is not equal to":
                    return (trgText != value);

                case "is numeric":
                    return decimal.TryParse(trgText, out _);
                case "is not numeric":
                    return !decimal.TryParse(trgText, out _);

                default:
                    return false;
            }
        }
        private static bool FilterDeterminStatementTruth_Numeric(string trgText, string filterAction, Dictionary<string, string> parameters, taskt.Core.Automation.Engine.AutomationEngineInstance engine)
        {
            decimal trgValue, value1 = 0, value2 = 0;

            try
            {
                trgValue = trgText.ConvertToUserVariableAsDecimal("Value", engine);
            }
            catch(Exception ex)
            {
                if (ex.Message.EndsWith(" is not a number."))
                {
                    switch(parameters["If Not Numeric"].ToLower())
                    {
                        case "ignore":
                            return false;
                        default:
                            throw ex;
                    }
                }
                else
                {
                    throw ex;
                }
            }
            
            filterAction = filterAction.ToLower();
            switch (filterAction)
            {
                case "between":
                case "not between":
                    value1 = parameters["Value1"].ConvertToUserVariableAsDecimal("Value1", engine);
                    value2 = parameters["Value2"].ConvertToUserVariableAsDecimal("Value2", engine);
                    if (value1 > value2)
                    {
                        decimal t = value1;
                        value1 = value2;
                        value2 = t;
                    }
                    break;
                default:
                    value1 = parameters["Value"].ConvertToUserVariableAsDecimal("Value", engine);
                    break;
            }

            switch (filterAction)
            {
                case "is equal to":
                    return (trgValue == value1);
                case "is not equal to":
                    return (trgValue != value1);
                case "is greater than":
                    return (trgValue > value1);
                case "is greater than or equal to":
                    return (trgValue >= value1);
                case "is less than":
                    return (trgValue < value1);
                case "is less than or equal to":
                    return (trgValue <= value1);

                case "between":
                    return ((trgValue >= value1) && (trgValue <= value2));
                case "not between":
                    return ((trgValue < value1) || (trgValue > value2));

                default:
                    return false;
            }
        }

        #endregion
    }
}
