﻿using System;
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

                default:
                    throw new Exception("If type not recognized!");
                    break;
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
                    break;
            }
            return ifResult;
        }

        private static bool DetermineStatementTruth_DateCompare(DataTable actionParameterTable, Engine.AutomationEngineInstance engine)
        {
            var param = DataTableControls.GetFieldValues(actionParameterTable, "Parameter Name", "Parameter Value");

            string operand = param["Operand"].ConvertToUserVariable(engine);

            DateTime dt1 = param["Value1"].ConvertToUserVariableAsDate("Value1", engine);
            DateTime dt2 = param["Value2"].ConvertToUserVariableAsDate("Value2", engine);

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
                    break;
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
                    break;
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
                    break;

                case "it does not exist":
                    return !existCheck;
                    break;

                default:
                    throw new Exception("True When is strange value " + param["True When"]);
                    break;
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
                    break;

                case "it does not exist":
                    return !existCheck;
                    break;

                default:
                    throw new Exception("True When is strange value " + param["True When"]);
                    break;
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
                    break;
                case "false":
                    return !value;
                    break;
                default:
                    throw new Exception("Value Is " + param["Value Is"] + " is not support.");
                    break;
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
                    break;

                case "is not equal to":
                    return (value1 != value2);
                    break;

                case "both are true":
                    return (value1 & value2);
                    break;

                case "both or one of them are true":
                    return (value1 | value2);
                    break;

                default:
                    throw new Exception("Value Is " + param["Value Is"] + " is not support.");
                    break;
            }
        }

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
    }
}
