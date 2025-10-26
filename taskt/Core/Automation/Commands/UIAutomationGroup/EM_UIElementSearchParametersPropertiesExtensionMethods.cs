using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Automation;
using System.Windows.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    public static class EM_UIElementSearchParametersPropertiesExtensionMethods
    {
        /// <summary>
        /// Search paramters property (DataTable)
        /// </summary>
        [PropertyDescription("Search Parameters")]
        [PropertyCustomUIHelper("GUI Inspect Tool", nameof(EM_UIElementSearchParametersPropertiesExtensionMethods) + "+" + nameof(lnkGUIInspectTool_UsedByInspectResult_Click))]
        [PropertyCustomUIHelper("Inspect Tool Parser", nameof(EM_UIElementSearchParametersPropertiesExtensionMethods) + "+" + nameof(lnkInspectToolParser_Click))]
        [PropertyCustomUIHelper("Add Empty Parameters", nameof(EM_UIElementSearchParametersPropertiesExtensionMethods) + "+" + nameof(lnkAddEmptyParameter_Click))]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Search Paramters", true)]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.DataGridView)]
        [PropertyDataGridViewSetting(false, false, true)]
        [PropertyDataGridViewColumnSettings("Enabled", "Enabled", false, PropertyDataGridViewColumnSettings.DataGridViewColumnType.CheckBox)]
        [PropertyDataGridViewColumnSettings("ParameterName", "Parameter Name", true, PropertyDataGridViewColumnSettings.DataGridViewColumnType.TextBox)]
        [PropertyDataGridViewColumnSettings("ParameterValue", "Parameter Value", false, PropertyDataGridViewColumnSettings.DataGridViewColumnType.TextBox)]
        [PropertyDataGridViewCellEditEvent(nameof(EM_UIElementSearchParametersPropertiesExtensionMethods) + "+" + nameof(UIElementSearchDGV_CellBeginEdit), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellBeginEdit)]
        [PropertyDataGridViewCellEditEvent(nameof(EM_UIElementSearchParametersPropertiesExtensionMethods) + "+" + nameof(UIElementSearchDGV_CellClick), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellClick)]
        [PropertyDataGridViewInitMethod(nameof(EM_UIElementSearchParametersPropertiesExtensionMethods) + "+" + nameof(CreateEmptySearchParamters))]
        [PropertyParameterOrder(5000)]
        public static string v_SearchParameters { get; }

        /// <summary>
        /// UIElement type for Reflection
        /// </summary>
        private static readonly Type TypeOfAutomationElement = typeof(AutomationElement);

        /// <summary>
        /// ControlType type for Reflection
        /// </summary>
        private static readonly Type TypeOfControlType = typeof(ControlType);

        /// <summary>
        /// UIElement search DataGridView target properties
        /// </summary>
        private static readonly string[] TargetControlProperties = new string[]
        {
            "AcceleratorKey", "AccessKey", "AutomationId", "ClassName", "ControlType",
            "FrameworkId", "HasKeyboardFocus", "HelpText", "IsContentElement",
            "IsControlElement", "IsEnabled", "IsKeyboardFocusable", "IsOffscreen",
            "IsPassword", "IsRequiredForForm", "ItemStatus", "ItemType",
            "LocalizedControlType", "Name", "NativeWindowHandle", "ProcessId",
        };

        /// <summary>
        /// get interface, dgv from control
        /// </summary>
        /// <param name="ctl"></param>
        /// <returns></returns>
        private static (IUIElementSearchParametersProperties, DataGridView) GetCommandAndSearchDataGridView(Control ctl)
        {
            var editor = FormUIControls.GetCommandEditorFromControl(ctl);
            var command = (IUIElementSearchParametersProperties)editor.editingCommand;
            var dgv = FormUIControls.GetPropertyControl<DataGridView>(editor.editingCommand.ControlsList, nameof(IUIElementSearchParametersProperties.v_SearchParameters));

            return (command, dgv);
        }

        /// <summary>
        /// search parameters update process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="updateFunc"></param>
        private static void SearchParametersUpdateProcess(this IUIElementSearchParametersProperties command, DataGridView dgv, Action<DataTable> updateFunc)
        {
            updateFunc(command.v_SearchParameters);
            RenderUIElementSearchParameter(dgv);
        }

        /// <summary>
        /// show GUI InspectTool and get InspectTool like result
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void lnkGUIInspectTool_UsedByInspectResult_Click(object sender, EventArgs e)
        {
            using (var fm = new UI.Forms.ScriptBuilder.CommandEditor.Supplemental.frmGUIInspect())
            {
                if (fm.ShowDialog(((Control)sender).FindForm()) == DialogResult.OK)
                {
                    (var command, var dgv) = GetCommandAndSearchDataGridView((Control)sender);
                    command.SearchParametersUpdateProcess(dgv, new Action<DataTable>((tbl) =>
                    {
                        ParseInspectToolResult(fm.InspectResult, tbl);
                    }));
                }
            }
        }

        /// <summary>
        /// set Empty Paramter in DGV
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void lnkAddEmptyParameter_Click(object sender, EventArgs e)
        {
            (var command, var dgv) = GetCommandAndSearchDataGridView((Control)sender);
            command.SearchParametersUpdateProcess(dgv, new Action<DataTable>((tbl) =>
            {
                CreateEmptySearchParamters(tbl);
            }));
        }

        /// <summary>
        /// show InspectTool Parser and set result
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void lnkInspectToolParser_Click(object sender, EventArgs e)
        {
            using (var fm = new UI.Forms.ScriptBuilder.CommandEditor.Supplemental.frmInspectParser())
            {
                if (fm.ShowDialog(((Control)sender).FindForm()) == DialogResult.OK)
                {
                    (var command, var dgv) = GetCommandAndSearchDataGridView((Control)sender);
                    command.SearchParametersUpdateProcess(dgv, new Action<DataTable>((tbl) =>
                    {
                        ParseInspectToolResult(fm.inspectResult, tbl);
                    }));
                }
            }
        }

        /// <summary>
        /// UIElement search DGV cell click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void UIElementSearchDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var myDGV = (DataGridView)sender;

            if (e.RowIndex < 0)
            {
                return;
            }

            if (e.ColumnIndex >= 0)
            {
                if (e.ColumnIndex != 1)
                {
                    var targetCell = myDGV.Rows[e.RowIndex].Cells[1];
                    if (targetCell is DataGridViewTextBoxCell)
                    {
                        myDGV.BeginEdit(false);
                    }
                    else if (targetCell is DataGridViewComboBoxCell)
                    {
                        SendKeys.Send("{F4}");
                    }
                }
            }
            else
            {
                myDGV.EndEdit();
            }
        }

        /// <summary>
        /// UIElement search DGV cell begin edit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void UIElementSearchDGV_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// render UIElement search parameter
        /// </summary>
        /// <param name="dgv"></param>
        public static void RenderUIElementSearchParameter(DataGridView dgv)
        {
            DataGridViewRow r = null;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if ((row.Cells[1].Value?.ToString() ?? "") == "ControlType")
                {
                    r = row;
                    break;
                }
            }

            if (r != null)
            {
                var cmb = new DataGridViewComboBoxCell();

                var fields = typeof(ControlType).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Select(f => f.Name).ToList();

                cmb.Items.Add("");
                cmb.Items.AddRange(fields.ToArray());

                r.Cells[2] = cmb;
            }
        }

        /// <summary>
        /// create empty UIElement search parameters
        /// </summary>
        /// <param name="table"></param>
        public static void CreateEmptySearchParamters(DataTable table)
        {
            table.Rows.Clear();
            foreach (var n in TargetControlProperties)
            {
                table.Rows.Add(false, n, "");
            }
        }

        /// <summary>
        /// parse inspect tool result for UIElement Search DGV
        /// </summary>
        /// <param name="result"></param>
        /// <param name="table"></param>
        /// <param name="windowNames"></param>
        private static void ParseInspectToolResult(string result, DataTable table, ComboBox windowNames = null)
        {
            var results = result.Split(new[] { "\r\n" }, StringSplitOptions.None);

            if ((results.Length >= 1) && (result != ""))
            {
                CreateEmptySearchParamters(table);

                var ancestors = new List<string>();
                string currentParam = "";
                foreach (string res in results)
                {
                    var spt = res.Split('\t');
                    string value = (spt.Length >= 2) ? spt[1] : "";
                    if (value.StartsWith("\"") && value.EndsWith("\""))
                    {
                        value = value.Substring(1, value.Length - 2);
                    }
                    if (spt[0] != "")
                    {
                        string name = spt[0].Substring(0, spt[0].Length - 1);
                        currentParam = name;

                        switch (name)
                        {
                            case "AcceleratorKey":
                            case "AccessKey":
                            case "AutomationId":
                            case "ClassName":
                            case "FrameworkId":
                            case "HasKeyboardFocus":
                            case "HelpText":
                            case "IsContentElement":
                            case "IsControlElement":
                            case "IsEnabled":
                            case "IsKeyboardFocusable":
                            case "IsOffscreen":
                            case "IsPassword":
                            case "IsRequiredForForm":
                            case "ItemStatus":
                            case "ItemType":
                            case "LocalizedControlType":
                            case "Name":
                            case "NativeWindowHandle":
                            case "ProcessId":
                                DataTableControls.SetParameterValue(table, value, name, "ParameterName", "ParameterValue");
                                break;

                            case "ControlType":
                                DataTableControls.SetParameterValue(table, ParseControlTypeInspectToolResult(value), name, "ParameterName", "ParameterValue");
                                break;

                            case "Ancestors":
                                ancestors.Add(value);
                                break;
                        }
                    }
                    else
                    {
                        if (currentParam == "Ancestors")
                        {
                            ancestors.Add(value);
                        }
                    }
                    if (windowNames != null)
                    {
                        SetComboBoxWindowNameFromInspectAncestors(ancestors, windowNames);
                    }
                }
            }
            else
            {
                var f = new UI.Forms.General.frmDialog("No Inspect Tool Results", "Fail Parse", UI.Forms.General.frmDialog.DialogType.OkOnly, 0);
                f.ShowDialog();
            }
        }

        /// <summary>
        /// parse ControlType in InspectTool result
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string ParseControlTypeInspectToolResult(string value)
        {
            var spt = value.Split(' ');
            return spt[0].Replace("UIA_", "").Replace("ControlTypeId", "");
        }

        /// <summary>
        /// set combobox window name from Inspect Tool result Ancenstors value
        /// </summary>
        /// <param name="ancestors"></param>
        /// <param name="cmb"></param>
        private static void SetComboBoxWindowNameFromInspectAncestors(List<string> ancestors, ComboBox cmb)
        {
            if (ancestors.Count > 0)
            {
                var windows = new string[cmb.Items.Count];
                cmb.Items.CopyTo(windows, 0);

                bool isFound = false;
                foreach (string ancestor in ancestors)
                {
                    // get quoted " " text
                    int pos = ancestor.IndexOf("\"");
                    if (pos < 0)
                    {
                        continue;
                    }

                    string winName = ancestor.Substring(pos + 1);
                    pos = winName.IndexOf("\"");
                    if (pos < 0)
                    {
                        continue;
                    }
                    winName = winName.Substring(0, pos);

                    foreach (string win in windows)
                    {
                        if (winName == win)
                        {
                            cmb.Text = winName;
                            isFound = true;
                            break;
                        }
                    }
                    if (isFound)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// create AutomationElement search condition
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static PropertyCondition CreatePropertyCondition(string propertyName, object propertyValue)
        {
            if (TargetControlProperties.Contains(propertyName))
            {
                var conditionProp = (AutomationProperty)TypeOfAutomationElement.GetField($"{propertyName}Property").GetValue(null);

                switch (propertyName)
                {
                    case "ControlType":
                        var controlValue = TypeOfControlType.GetField(propertyValue.ToString())?.GetValue(null) ?? throw new Exception($"ControlType '{propertyValue.ToString()}' does not Exists");
                        return new PropertyCondition(conditionProp, controlValue);

                    default:
                        return new PropertyCondition(conditionProp, propertyValue);
                }
            }
            else
            {
                throw new Exception($"Property '{propertyName}' does not Exists or not Supported property");
            }
        }

        /// <summary>
        /// create AutomationElement search condition from DataTable
        /// </summary>
        /// <param name="table"></param>
        /// <param name="engine"></param>
        /// <returns>when no conditions return null</returns>
        private static Condition CreateSearchCondition(this IUIElementSearchParametersProperties comamnd, AutomationEngineInstance engine)
        {
            var table = comamnd.v_SearchParameters;

            //create and populate condition list
            var conditionList = new List<Condition>();
            foreach (DataRow row in table.Rows)
            {
                var isEnabled = row.Field<string>("Enabled") ?? "false";
                if (bool.TryParse(isEnabled, out bool res))
                {
                    if (!res)
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }

                var parameterName = row.Field<string>("ParameterName") ?? "";
                var parameterValue = (row.Field<string>("ParameterValue") ?? "").ExpandValueOrUserVariable(engine);

                // value collection
                switch (parameterName)
                {
                    case "HasKeyboardFocus":
                    case "IsContentElement":
                    case "IsControlElement":
                    case "IsEnabled":
                    case "IsKeyboardFocusable":
                    case "IsOffscreen":
                    case "IsPassword":
                    case "IsRequiredForForm":
                        if (string.IsNullOrEmpty(parameterValue))
                        {
                            parameterValue = "False";
                        }
                        else
                        {
                            switch (parameterValue.ToLower())
                            {
                                case "yes":
                                    parameterValue = "True";
                                    break;
                                case "no":
                                    parameterValue = "False";
                                    break;
                            }
                        }
                        if (!bool.TryParse(parameterValue, out _))
                        {
                            throw new Exception($"Invalid ParamterValue. Value must be 'True' or 'False'. ParameterName: '{parameterName}', ParameteValue: '{parameterValue}'");
                        }
                        break;

                    case "NativeWindowHandle":
                    case "ProcessId":
                        if (string.IsNullOrEmpty(parameterValue))
                        {
                            parameterValue = "0";
                        }
                        if (!Int32.TryParse(parameterValue, out _))
                        {
                            throw new Exception($"Invalid ParamterValue. Value must be Int32 value. ParameterName: '{parameterName}', ParameteValue: '{parameterValue}'");
                        }
                        break;
                }

                // DBG
                //Debug.WriteLine($"Name: '{parameterName}', Value: '{parameterValue}'");

                PropertyCondition propCondition = null;

                switch (parameterName)
                {
                    case "HasKeyboardFocus":
                    case "IsContentElement":
                    case "IsControlElement":
                    case "IsEnabled":
                    case "IsKeyboardFocusable":
                    case "IsOffscreen":
                    case "IsPassword":
                    case "IsRequiredForForm":
                        propCondition = CreatePropertyCondition(parameterName, bool.Parse(parameterValue));
                        break;

                    case "NativeWindowHandle":
                    case "ProcessId":
                        propCondition = CreatePropertyCondition(parameterName, Int32.Parse(parameterValue));
                        break;

                    case "AcceleratorKey":
                    case "AccessKey":
                    case "AutomationId":
                    case "ClassName":
                    case "ControlType":
                    case "FrameworkId":
                    case "HelpText":
                    case "ItemStatus":
                    case "ItemType":
                    case "LocalizedControlType":
                    case "Name":
                        propCondition = CreatePropertyCondition(parameterName, parameterValue);
                        break;
                }

                conditionList.Add(propCondition);
            }

            switch (conditionList.Count)
            {
                case 0:
                    return null;    // no conditions

                case 1:
                    return conditionList[0];    // 1 condition

                default:
                    return new AndCondition(conditionList.ToArray());   // 2+ conditions
            }
        }

        /// <summary>
        /// search children UIElement
        /// </summary>
        /// <param name="command"></param>
        /// <param name="rootElement"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static List<AutomationElement> SearchChildrenUIElements(this IUIElementSearchParametersProperties command, AutomationElement rootElement, AutomationEngineInstance engine)
        {
            var searchConditions = command.CreateSearchCondition(engine);

            if (searchConditions != null)
            {
                var waitTime = command.ExpandValueOrUserVariableAsWaitTimeForUIElement(engine);

                var r = WaitControls.WaitProcess(waitTime, "Children UIElement", new Func<(bool, object)>(() =>
                {
                    var elements = rootElement.FindAll(TreeScope.Children, searchConditions);
                    if (elements.Count > 0)
                    {
                        var ret = new List<AutomationElement>();
                        foreach (AutomationElement element in elements)
                        {
                            ret.Add(element);
                        }
                        return (true, ret);
                    }
                    else
                    {
                        return (false, null);
                    }
                }), engine);
                if (r is List<AutomationElement> list)
                {
                    return list;
                }
                else
                {
                    return new List<AutomationElement>();
                }
            }
            else
            {
                var walker = TreeWalker.RawViewWalker;
                var elems = new List<AutomationElement>();

                var node = walker.GetFirstChild(rootElement);
                while (node != null)
                {
                    elems.Add(node);
                    node = walker.GetNextSibling(node);
                }
                return elems;
            }
        }

        /// <summary>
        /// expand value or user variable as wait time for UIElement
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static int ExpandValueOrUserVariableAsWaitTimeForUIElement(this IUIElementSearchParametersProperties command, Engine.AutomationEngineInstance engine)
        {
            return command.ToScriptCommand().ExpandValueOrUserVariableAsInteger(nameof(command.v_WaitTimeForUIElement), engine);
        }
    }
}
