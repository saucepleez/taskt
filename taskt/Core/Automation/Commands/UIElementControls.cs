﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Automation;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Security;
using System.Windows.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using System.Diagnostics;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for AutomationElement methods
    /// </summary>
    internal static class UIElementControls
    {
        #region fields

        /// <summary>
        /// UIElement type for Reflection
        /// </summary>
        private static readonly Type TypeOfAutomationElement = typeof(AutomationElement);

        /// <summary>
        /// ControlType type for Reflection
        /// </summary>
        private static readonly Type TypeOfControlType = typeof(ControlType);

        private static readonly string[] TargetControlTypes = new string[]
        {
            "AcceleratorKey", "AccessKey", "AutomationId", "ClassName", "ControlType",
            "FrameworkId", "HasKeyboardFocus", "HelpText", "IsContentElement",
            "IsControlElement", "IsEnabled", "IsKeyboardFocusable", "IsOffscreen",
            "IsPassword", "IsRequiredForForm", "ItemStatus", "ItemType",
            "LocalizedControlType", "Name", "NativeWindowHandle", "ProcessId",
        };
        #endregion

        #region VirtualProperties
        /// <summary>
        /// input UIElement property
        /// </summary>
        [PropertyDescription("UIElement Variable Name")]
        [InputSpecification("UIElement Variable Name", true)]
        [PropertyDetailSampleUsage("**vElement**", PropertyDetailSampleUsage.ValueType.VariableValue)]
        [PropertyDetailSampleUsage("**{{{vElement}}}**", PropertyDetailSampleUsage.ValueType.VariableValue)]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.UIElement, true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyValidationRule("UIElement", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Element")]
        [PropertyParameterOrder(5000)]
        public static string v_InputUIElementName { get; }

        /// <summary>
        /// output UIElement property
        /// </summary>
        [PropertyDescription("UIElement Variable Name")]
        [InputSpecification("UIElement Variable Name", true)]
        [PropertyDetailSampleUsage("**vElement**", PropertyDetailSampleUsage.ValueType.VariableValue)]
        [PropertyDetailSampleUsage("**{{{vElement}}}**", PropertyDetailSampleUsage.ValueType.VariableValue)]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsVariablesList(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.UIElement, true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyValidationRule("UIElement", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Element")]
        [PropertyParameterOrder(5000)]
        public static string v_OutputUIElementName { get; }

        /// <summary>
        /// New output UIElement name
        /// </summary>
        [PropertyDescription("UIElement Variable Name")]
        [InputSpecification("UIElement Variable Name", true)]
        [PropertyDetailSampleUsage("**vNewElement**", PropertyDetailSampleUsage.ValueType.VariableValue)]
        [PropertyDetailSampleUsage("**{{{vNewElement}}}**", PropertyDetailSampleUsage.ValueType.VariableValue)]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsVariablesList(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.UIElement, true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyValidationRule("New UIElement", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "New UIElement")]
        [PropertyParameterOrder(5000)]
        public static string v_NewOutputUIElementName { get; }

        /// <summary>
        /// xpath property
        /// </summary>
        [PropertyDescription("Search XPath")]
        [InputSpecification("Search XPath", true)]
        [PropertyDetailSampleUsage("**//Button[@Name=\"OK\"]**", "Specify a Button whose **Name** Attribute is **OK** in descendant node of the criteria AutomationElement")]
        [PropertyDetailSampleUsage("**/Pane[1]/Button[2]**", "Specify the **second** Button of the **first** Pane child node of the child node of the criteria AutomationElement")]
        [PropertyDetailSampleUsage("**{{{vXPath}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "XPath")]
        [Remarks("XPath does not support to use parent, following-sibling, and preceding-sibling for root element.")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.TextBox)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("XPath", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "XPath")]
        [PropertyCustomUIHelper("GUI Inspect Tool", nameof(UIElementControls) + "+" + nameof(lnkGUIInspectTool_UsedByXPath_Click))]
        [PropertyParameterOrder(5000)]
        public static string v_XPath { get; }

        /// <summary>
        /// Search paramters property
        /// </summary>
        [PropertyDescription("Search Parameters")]
        [PropertyCustomUIHelper("GUI Inspect Tool", nameof(UIElementControls) + "+" + nameof(lnkGUIInspectTool_UsedByInspectResult_Click))]
        [PropertyCustomUIHelper("Inspect Tool Parser", nameof(UIElementControls) + "+" + nameof(lnkInspectToolParser_Click))]
        [PropertyCustomUIHelper("Add Empty Parameters", nameof(UIElementControls) + "+" + nameof(lnkAddEmptyParameter_Click))]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Search Paramters", true)]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.DataGridView)]
        [PropertyDataGridViewSetting(false, false, true)]
        [PropertyDataGridViewColumnSettings("Enabled", "Enabled", false, PropertyDataGridViewColumnSettings.DataGridViewColumnType.CheckBox)]
        [PropertyDataGridViewColumnSettings("ParameterName", "Parameter Name", true, PropertyDataGridViewColumnSettings.DataGridViewColumnType.TextBox)]
        [PropertyDataGridViewColumnSettings("ParameterValue", "Parameter Value", false, PropertyDataGridViewColumnSettings.DataGridViewColumnType.TextBox)]
        [PropertyDataGridViewCellEditEvent(nameof(UIElementControls) + "+" + nameof(UIAutomationDataGridView_CellBeginEdit), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellBeginEdit)]
        [PropertyDataGridViewCellEditEvent(nameof(UIElementControls) + "+" + nameof(UIAutomationDataGridView_CellClick), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellClick)]
        [PropertyDataGridViewInitMethod(nameof(UIElementControls) + "+" + nameof(CreateEmptyParamters))]
        [PropertyParameterOrder(5000)]
        public static string v_SearchParameters { get; }

        /// <summary>
        /// AutomationElement wait time
        /// </summary>
        [PropertyDescription("Wait Time for the UIElement to Exist (sec)")]
        [InputSpecification("Wait Time", true)]
        [Remarks("Specify how long to Wait before an Error will occur because the UIElement is Not Found.")]
        [PropertyDetailSampleUsage("**10**", PropertyDetailSampleUsage.ValueType.Value, "Wait Time")]
        [PropertyDetailSampleUsage("**{{{vTime}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Wait Time")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyIsOptional(true, "10")]
        [PropertyFirstValue("10")]
        [PropertyParameterOrder(5000)]
        public static string v_WaitTime { get; }
        #endregion

        #region variable methods

        /// <summary>
        /// expand user variable as UIElement
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static AutomationElement ExpandUserVariableAsUIElement(this string variableName, Engine.AutomationEngineInstance engine)
        {
            Script.ScriptVariable v = variableName.GetRawVariable(engine);
            if (v.VariableValue is AutomationElement e)
            {
                return e;
            }
            else
            {
                throw new Exception("Variable " + variableName + " is not UIElement");
            }
        }

        /// <summary>
        /// expand user variable as UIElement
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static AutomationElement ExpandUserVariableAsUIElement(this ScriptCommand command, string parameterName, Engine.AutomationEngineInstance engine)
        {
            var prop = command.GetProperty(parameterName);
            var value = prop?.GetValue(command)?.ToString() ?? "";
            return ExpandUserVariableAsUIElement(value, engine);
        }

        /// <summary>
        /// expand user variable string as XPath
        /// </summary>
        /// <param name="value"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static string ExpandUserVariableAsXPath(this string value, Engine.AutomationEngineInstance engine)
        {
            var p = value.ExpandValueOrUserVariable(engine);
            if (!p.StartsWith("."))
            {
                p = "." + p;
            }
            return p;
        }

        /// <summary>
        /// expand variable string as XPath
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static string ExpandUserVariableAsXPath(this ScriptCommand command, string parameterName, Engine.AutomationEngineInstance engine)
        {
            var prop = command.GetProperty(parameterName);
            var value = prop?.GetValue(command)?.ToString() ?? "";
            return ExpandUserVariableAsXPath(value, engine);
        }

        /// <summary>
        /// store AutomationElement
        /// </summary>
        /// <param name="value"></param>
        /// <param name="engine"></param>
        /// <param name="targetVariable"></param>
        public static void StoreInUserVariable(this AutomationElement value, Engine.AutomationEngineInstance engine, string targetVariable)
        {
            ExtensionMethods.StoreInUserVariable(targetVariable, value, engine, false);
        }
        #endregion


        public static AutomationElement GetFromWindowName(string windowName, Engine.AutomationEngineInstance engine)
        {
            var windowSearchConditions = new AndCondition(
                    new PropertyCondition(AutomationElement.NameProperty, windowName),
                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window)
                );

            var paneSearchConditions = new AndCondition(
                    new PropertyCondition(AutomationElement.NameProperty, windowName),
                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Pane)
                );

            var windowElement = AutomationElement.RootElement.FindFirst(TreeScope.Children, windowSearchConditions);
            if (windowElement == null)
            {
                // Pane search
                windowElement = AutomationElement.RootElement.FindFirst(TreeScope.Children, paneSearchConditions);
            }
            if (windowElement == null)
            {
                // try window handle
                try
                {
                    IntPtr wHnd = WindowControls.FindWindowHandle(windowName, "exact match", engine);
                    windowElement = AutomationElement.FromHandle(wHnd);
                }
                catch
                {
                    // nothing to do
                }
            }
            if (windowElement == null)
            {
                // more deep search window
                windowElement = AutomationElement.RootElement.FindFirst(TreeScope.Subtree, windowSearchConditions);
            }
            if (windowElement == null)
            {
                // more deep search pane
                windowElement = AutomationElement.RootElement.FindFirst(TreeScope.Subtree, paneSearchConditions);
            }

            if (windowElement == null)
            {
                throw new Exception("Window Name '" + windowName + "' not found AutomationElement");
            }
            else
            {
                return windowElement;
            }
        }

        public static void CreateEmptyParamters(DataTable table)
        {
            table.Rows.Clear();
            foreach(var n in TargetControlTypes)
            {
                table.Rows.Add(false, n, "");
            }
        }

        private static void parseInspectToolResult(string result, DataTable table, ComboBox windowNames = null)
        {
            string[] results = result.Split(new[] { "\r\n" }, StringSplitOptions.None);

            if ((results.Length >= 1) && (result != ""))
            {
                CreateEmptyParamters(table);

                List<string> ancestors = new List<string>();
                string currentParam = "";
                foreach (string res in results)
                {
                    string[] spt = res.Split('\t');
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
                                DataTableControls.SetParameterValue(table, parseControlTypeInspectToolResult(value), name, "ParameterName", "ParameterValue");
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
                        setComboBoxWindowNameFromInspectAncestors(ancestors, windowNames);
                    }
                }
            }
            else
            {
                var f = new UI.Forms.General.frmDialog("No Inspect Tool Results", "Fail Parse", UI.Forms.General.frmDialog.DialogType.OkOnly, 0);
                f.ShowDialog();
            }
        }

        private static string parseControlTypeInspectToolResult(string value)
        {
            var spt = value.Split(' ');
            return spt[0].Replace("UIA_", "").Replace("ControlTypeId", "");
        }

        private static void setComboBoxWindowNameFromInspectAncestors(List<string> ancestors, ComboBox cmb)
        {
            if (ancestors.Count > 0)
            {
                string[] windows = new string[cmb.Items.Count];
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

        public static string GetControlTypeText(ControlType control)
        {
            var fullName = control.ProgrammaticName;
            return fullName.Substring(fullName.LastIndexOf('.') + 1);
        }

        #region GUI Search by conditions

        /// <summary>
        /// create AutomationElement search condition
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static PropertyCondition CreatePropertyCondition(string propertyName, object propertyValue)
        {
            var conditionProp = (AutomationProperty)TypeOfAutomationElement.GetField(propertyName + "Property")?.GetValue(null) ??
                                    throw new Exception("Property '" + propertyName + "' does not Exists");

            switch (propertyName)
            {
                case "ControlType":
                    var controlValue = TypeOfControlType.GetField(propertyValue.ToString())?.GetValue(null) ?? throw new Exception("ControlType '" + propertyValue.ToString() + "' does not Exists");
                    return new PropertyCondition(conditionProp, controlValue);

                default:
                    return new PropertyCondition(conditionProp, propertyValue);
            }
        }

        /// <summary>
        /// create AutomationElement search condition from DataTable
        /// </summary>
        /// <param name="table"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        private static Condition CreateSearchCondition(DataTable table, Engine.AutomationEngineInstance engine)
        {
            //create and populate condition list
            var conditionList = new List<Condition>();
            foreach(DataRow row in table.Rows)
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
                var parameterValue = row.Field<string>("ParameterValue") ?? ""; ;

                // value correction
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
        /// Deep Search GUI Element by Specified Condition
        /// </summary>
        /// <param name="rootElement"></param>
        /// <param name="searchCondition"></param>
        /// <returns></returns>
        private static AutomationElement DeepSearchGUIElement(AutomationElement rootElement, Condition searchCondition, DateTime endTime)
        {
            TreeWalker walker = TreeWalker.RawViewWalker;

            // format conditions
            PropertyCondition[] conditions;
            if (searchCondition is AndCondition andConds)
            {
                conditions = andConds.GetConditions().Cast<PropertyCondition>().ToArray();
            }
            else
            {
                conditions = new PropertyCondition[1]
                {
                    (PropertyCondition)searchCondition
                };
            }

            var ret = WalkerSearch_DepthFirst(rootElement, conditions, walker, endTime);
            //var ret = WalkerSearch_WidthFirst_Reverse(rootElement, conditions, walker, endTime);
            return ret;
        }

        /// <summary>
        /// Search GUI Element used by TreeWalker (Depth First)
        /// </summary>
        /// <param name="rootElement"></param>
        /// <param name="searchConditions"></param>
        /// <param name="walker"></param>
        /// <returns></returns>
        private static AutomationElement WalkerSearch_DepthFirst(AutomationElement rootElement, PropertyCondition[] searchConditions, TreeWalker walker, DateTime endTime)
        {
            AutomationElement node = walker.GetFirstChild(rootElement);
            AutomationElement ret = null;

            while (node != null)
            {
                // DBG
                //Console.WriteLine($"# Node: {node.Current.Name}");

                bool result = true;
                foreach (var c in searchConditions)
                {
                    object p = node.GetCurrentPropertyValue(c.Property);

                    switch (c.Property.ProgrammaticName)
                    {
                        case "AutomationElementIdentifiers.ControlTypeProperty":
                            // ControlType compare
                            result &= (c.Value.ToString() == ((ControlType)p).Id.ToString());
                            // DBG
                            //Console.WriteLine($"Property: '{c.Property.ProgrammaticName}', Value Cond: '{c.Value.ToString()}', Value Node: '{((ControlType)p).Id.ToString()}'");
                            break;

                        default:
                            // normal compare
                            result &= (c.Value.ToString() == p.ToString());
                            // DBG
                            //Console.WriteLine($"Property: '{c.Property.ProgrammaticName}', Value Cond: '{c.Value.ToString()}', Value Node: '{p.ToString()}'");
                            break;
                    }

                    if (!result)
                    {
                        break;
                    }
                }

                if (result)
                {
                    ret = node;
                    break;
                }
                // Time up! not found.
                if (DateTime.Now > endTime)
                {
                    break;
                }

                // search child node
                if (walker.GetFirstChild(node) != null)
                {
                    ret = WalkerSearch_DepthFirst(node, searchConditions, walker, endTime);
                    if (ret != null)
                    {
                        break;
                    }
                }

                // next sibling
                node = walker.GetNextSibling(node);
            }

            return ret;
        }

        /// <summary>
        /// Search GUI Element used by TreeWalker (Depth First, Reverse)
        /// </summary>
        /// <param name="rootElement"></param>
        /// <param name="searchConditions"></param>
        /// <param name="walker"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        private static AutomationElement WalkerSearch_DepthFirst_Reverse(AutomationElement rootElement, PropertyCondition[] searchConditions, TreeWalker walker, DateTime endTime)
        {
            AutomationElement node = walker.GetLastChild(rootElement);
            AutomationElement ret = null;

            while (node != null)
            {
                // DBG
                //Console.WriteLine($"# Node: {node.Current.Name}");

                bool result = true;
                foreach (var c in searchConditions)
                {
                    object p = node.GetCurrentPropertyValue(c.Property);

                    switch (c.Property.ProgrammaticName)
                    {
                        case "AutomationElementIdentifiers.ControlTypeProperty":
                            // ControlType compare
                            result &= (c.Value.ToString() == ((ControlType)p).Id.ToString());
                            // DBG
                            //Console.WriteLine($"Property: '{c.Property.ProgrammaticName}', Value Cond: '{c.Value.ToString()}', Value Node: '{((ControlType)p).Id.ToString()}'");
                            break;

                        default:
                            // normal compare
                            result &= (c.Value.ToString() == p.ToString());
                            // DBG
                            //Console.WriteLine($"Property: '{c.Property.ProgrammaticName}', Value Cond: '{c.Value.ToString()}', Value Node: '{p.ToString()}'");
                            break;
                    }

                    if (!result)
                    {
                        break;
                    }
                }

                if (result)
                {
                    ret = node;
                    break;
                }
                // Time up! not found.
                if (DateTime.Now > endTime)
                {
                    break;
                }

                // search child node
                if (walker.GetLastChild(node) != null)
                {
                    ret = WalkerSearch_DepthFirst_Reverse(node, searchConditions, walker, endTime);
                    if (ret != null)
                    {
                        break;
                    }
                }

                // previous sibling
                node = walker.GetPreviousSibling(node);
            }

            return ret;
        }

        /// <summary>
        /// Search GUI Element by specified conditions DataTable
        /// </summary>
        /// <param name="rootElement"></param>
        /// <param name="conditionTable"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        private static AutomationElement SearchGUIElement(AutomationElement rootElement, DataTable conditionTable, Engine.AutomationEngineInstance engine, DateTime endTime)
        {
            Condition searchConditions = CreateSearchCondition(conditionTable, engine);

            // NOTE: stop hung up
            //var element = rootElement.FindFirst(TreeScope.Descendants, searchConditions) ??
            //                rootElement.FindFirst(TreeScope.Subtree, searchConditions) ??
            //                DeepSearchGUIElement(rootElement, searchConditions);

            var element = DeepSearchGUIElement(rootElement, searchConditions, endTime);

            // if element not found, don't throw exception here
            return element;
        }

        /// <summary>
        /// Search GUI Element from specified arguments
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="conditionTable"></param>
        /// <param name="waitTime"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static AutomationElement SearchGUIElement(AutomationElement elem, DataTable conditionTable, int waitTime, Engine.AutomationEngineInstance engine)
        {
            var ret = WaitControls.WaitProcess(waitTime, "AutomationElement",
                new Func<(bool, object)>(() =>
                {
                    var endTime = DateTime.Now.AddSeconds(waitTime);
                    var e = SearchGUIElement(elem, conditionTable, engine, endTime);
                    if (e != null)
                    {
                        return (true, e);
                    }
                    else
                    {
                        return (false, null);
                    }
                }), engine
            );
            if (ret is AutomationElement foundElem)
            {
                return foundElem;
            }
            else
            {
                throw new Exception("AutomationElement Not Found");
            }
        }

        /// <summary>
        /// Search GUI Element from specified parameter names
        /// </summary>
        /// <param name="command"></param>
        /// <param name="elementName"></param>
        /// <param name="conditionName"></param>
        /// <param name="waitTimeName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static AutomationElement SearchGUIElement(ScriptCommand command, string elementName, string conditionName, string waitTimeName, Engine.AutomationEngineInstance engine)
        {
            var elem = command.ExpandUserVariableAsUIElement(elementName, engine);
            var table = command.ConvertParameterToDataTable(conditionName, engine);
            var waitTime = command.ExpandValueOrUserVariableAsInteger(waitTimeName, engine);

            return SearchGUIElement(elem, table, waitTime, engine);
        }

        /// <summary>
        /// Search GUI Element. this method use VirtualProperty to get parameter names
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static AutomationElement SearchGUIElement(ScriptCommand command, Engine.AutomationEngineInstance engine)
        {
            var elementName = PropertyControls.GetProperty(command, new PropertyVirtualProperty(nameof(UIElementControls), nameof(v_InputUIElementName)))?.Name ?? "";
            var conditionName = PropertyControls.GetProperty(command, new PropertyVirtualProperty(nameof(UIElementControls), nameof(v_SearchParameters)))?.Name ?? "";
            var waitTimeName = PropertyControls.GetProperty(command, new PropertyVirtualProperty(nameof(UIElementControls), nameof(v_WaitTime))).Name ?? "";

            return SearchGUIElement(command, elementName, conditionName, waitTimeName, engine);
        }

        #endregion

        #region Winodow Search Methods

        /// <summary>
        /// get window UIElement. this method use PropertyVirtualProperty
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <param name="resultName"></param>
        /// <returns></returns>
        public static AutomationElement GetWindowUIElement(ScriptCommand command, Engine.AutomationEngineInstance engine, string resultName = "")
        {
            AutomationElement ret = null;

            WindowControls.WindowAction(command, engine,
                new Action<List<(IntPtr, string)>>(wins =>
                {
                    ret = AutomationElement.FromHandle(wins[0].Item1);

                    if (!string.IsNullOrEmpty(resultName))
                    {
                        //var resultValue = command.ConvertToUserVariable(resultName, "Result", engine);
                        var resultValue = command.GetRawPropertyValueAsString(resultName, "Result");

                        ret.StoreInUserVariable(engine, resultValue);
                    }
                })
            );

            return ret;
        }

        /// <summary>
        /// get window UIElement
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static AutomationElement GetWindowUIElement(ScriptCommand command, Engine.AutomationEngineInstance engine)
        {
            var resultName = command.GetProperty(new PropertyVirtualProperty(nameof(UIElementControls), nameof(v_OutputUIElementName)))?.Name ?? "";

            return GetWindowUIElement(command, engine, resultName);
        }
        #endregion

        #region search element node

        public static List<AutomationElement> GetChildrenUIElements(AutomationElement rootElement, DataTable conditionTable, Engine.AutomationEngineInstance engine)
        {
            Condition searchConditions = CreateSearchCondition(conditionTable, engine);

            if (searchConditions != null)
            {
                AutomationElementCollection elements = rootElement.FindAll(TreeScope.Children, searchConditions);

                // if element not found, don't throw exception here
                List<AutomationElement> ret = new List<AutomationElement>();
                foreach (AutomationElement element in elements)
                {
                    ret.Add(element);
                }
                return ret;
            }
            else
            {
                return GetAllChildrenUIElements(rootElement);
            }
        }

        private static List<AutomationElement> GetAllChildrenUIElements(AutomationElement rootElement)
        {
            TreeWalker walker = TreeWalker.RawViewWalker;

            var elems = new List<AutomationElement>();

            var node = walker.GetFirstChild(rootElement);
            while (node != null)
            {
                elems.Add(node);
                node = walker.GetNextSibling(node);
            }

            return elems;
        }

        /// <summary>
        /// get parent element
        /// </summary>
        /// <param name="targetElement"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static AutomationElement GetParentUIElement(AutomationElement targetElement)
        {
            TreeWalker walker = TreeWalker.RawViewWalker;

            var parent = walker.GetParent(targetElement);
            if (parent != null)
            {
                return parent;
            }
            else
            {
                throw new Exception("Parent UIElement not exists");
            }
        }

        /// <summary>
        /// get window name from UIElement
        /// </summary>
        /// <param name="targetElement"></param>
        /// <returns></returns>
        public static string GetWindowName(AutomationElement targetElement)
        {
            TreeWalker walker = TreeWalker.RawViewWalker;

            if (targetElement.Current.ControlType == ControlType.Window)
            {
                return targetElement.Current.Name;
            }

            try
            {
                var parent = walker.GetParent(targetElement);
                while (parent.Current.ControlType != ControlType.Window)
                {
                    parent = walker.GetParent(parent);
                }
                return parent.Current.Name;
            }
            catch
            {
                // try other method
                var windowNames = WindowControls.GetAllWindowTitles();
                if ((targetElement.Current.NativeWindowHandle != 0) && (windowNames.Contains(targetElement.Current.Name)))
                {
                    return targetElement.Current.Name;
                }

                try
                {
                    var parent = walker.GetParent(targetElement);
                    while((parent.Current.NativeWindowHandle == 0) || (!windowNames.Contains(parent.Current.Name)))
                    {
                        parent = walker.GetParent(parent);
                    }
                    return parent.Current.Name;
                }
                catch
                {
                    throw new Exception("Fail Get Window Name from UIElement");
                }
            }
        }

        /// <summary>
        /// get window handle from UIElement
        /// </summary>
        /// <param name="targetElement"></param>
        /// <returns></returns>
        public static int GetWindowHandle(AutomationElement targetElement)
        {
            TreeWalker walker = TreeWalker.RawViewWalker;

            var hnd = targetElement.GetCurrentPropertyValue(AutomationElement.NativeWindowHandleProperty);
            if(hnd != AutomationElement.NotSupported)
            {
                return (int)hnd;
            }

            var parent = walker.GetParent(targetElement);
            while (true)
            {
                hnd = parent.GetCurrentPropertyValue(AutomationElement.NativeWindowHandleProperty);
                if (hnd != AutomationElement.NotSupported)
                {
                    return (int)hnd;
                }
                parent = walker.GetParent(parent);
            }
        }

        #endregion

        public static string GetTextValue(AutomationElement targetElement)
        {
            //object patternObj;
            if (targetElement.TryGetCurrentPattern(RangeValuePattern.Pattern, out object rPtn))
            {
                // bar
                return ((RangeValuePattern)rPtn).Current.Value.ToString();
            }
            else if (targetElement.TryGetCurrentPattern(ValuePattern.Pattern, out object vPtn))
            {
                // TextBox
                return ((ValuePattern)vPtn).Current.Value;
            }
            else if (targetElement.TryGetCurrentPattern(TextPattern.Pattern, out object tPtn))
            {
                // TextBox Multilune
                return ((TextPattern)tPtn).DocumentRange.GetText(-1);
            }
            else if (targetElement.TryGetCurrentPattern(SelectionPattern.Pattern, out object sPtn))
            {
                // combobox
                AutomationElement selElem = ((SelectionPattern)sPtn).Current.GetSelection()[0];
                return selElem.Current.Name;
            }
            else
            {
                // others
                return targetElement.Current.Name;
            }
        }

        public static AutomationElement GetTableUIElement(AutomationElement targetElement, int row, int column)
        {
            if (targetElement.TryGetCurrentPattern(GridPattern.Pattern, out object gridObj))
            {
                var cosutomRows = targetElement.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Custom));
                if (cosutomRows.Count > 0)
                {
                    // DataGridView (.net)
                    if (cosutomRows.Count > row)
                    {
                        var r = cosutomRows[row + 1];
                        var cols = r.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));
                        if (cols.Count > column)
                        {
                            return cols[column];
                        }
                    }

                    throw new Exception("Table Row: '" + row + "', Column: '" + column + "' does not exists");
                }
                else
                {
                    // listView
                    AutomationElement cellElem = ((GridPattern)gridObj).GetItem(row, column);
                    if (cellElem == null)
                    {
                        throw new Exception("Table Row: '" + row + "', Column: '" + column + "' does not exists");
                    }
                    return cellElem;
                }
            }
            else
            {
                throw new Exception("UIElement is not Table Element");
            }
        }

        public static List<AutomationElement> GetSelectionItems(AutomationElement targetElement)
        {
            var getListItemFunc = new Func<AutomationElement, List<AutomationElement>>( el => {
                var elems = el.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.IsSelectionItemPatternAvailableProperty, true));
                List<AutomationElement> ret = new List<AutomationElement>();
                foreach (AutomationElement elem in elems)
                {
                    ret.Add(elem);
                }
                return ret;
            });

            AutomationElement rootElement = targetElement;

            if ((bool)rootElement.GetCurrentPropertyValue(AutomationElement.IsGridPatternAvailableProperty) ||
                (bool)rootElement.GetCurrentPropertyValue(AutomationElement.IsSelectionPatternAvailableProperty))
            {
                // DataGridView-ComboBox, ListBox
                return getListItemFunc(rootElement);
            }
            else
            {
                // ComboBox, TreeView
                bool isCmb = (bool)rootElement.GetCurrentPropertyValue(AutomationElement.IsExpandCollapsePatternAvailableProperty);

                if (!isCmb)
                {
                    rootElement = GetParentUIElement(rootElement);
                    isCmb = (bool)rootElement.GetCurrentPropertyValue(AutomationElement.IsExpandCollapsePatternAvailableProperty);
                }

                if ((bool)isCmb)
                {
                    object selPtn = rootElement.GetCurrentPattern(ExpandCollapsePattern.Pattern);

                    ExpandCollapsePattern ecPtn = (ExpandCollapsePattern)selPtn;
                    ecPtn.Expand();
                    System.Threading.Thread.Sleep(500);

                    // dbg
                    //System.Threading.Thread.Sleep(1000);
                    //Console.WriteLine("Expanded");

                    var ret = getListItemFunc(rootElement);

                    //if (collapseAfter)
                    //{
                    //    ecPtn.Collapse();
                    //}
                    return ret;
                }
                else
                {
                    throw new Exception("This UIElement does not have Selection Items");
                }
            }
        }

        #region create XElement methods

        public static (XElement, Dictionary<string, AutomationElement>) GetElementXml(AutomationElement targetElement)
        {
            XElement root = CreateXmlElement(targetElement);

            var dic = new Dictionary<string, AutomationElement>()
            {
                { root.GetHashCode().ToString(), targetElement }
            };

            TreeWalker walker = TreeWalker.RawViewWalker;

            GetChildNodeFromElement(root, targetElement, dic, walker);

            return (root, dic);
        }

        private static XElement GetChildNodeFromElement(XElement rootNode, AutomationElement rootElement, Dictionary<string, AutomationElement> elemsDic, TreeWalker walker)
        {
            var node = walker.GetFirstChild(rootElement);
            while (node != null)
            {
                string hash = node.GetHashCode().ToString();
                if (elemsDic.ContainsKey(hash))
                {
                    int i = 1;
                    while(elemsDic.ContainsKey(hash + "-" + i))
                    {
                        i++;
                    }
                    hash += "-" + i;
                }

                var childNode = CreateXmlElement(node, hash);
                rootNode.Add(childNode);
                elemsDic.Add(hash, node);

                if (walker.GetFirstChild(node) != null)
                {
                    GetChildNodeFromElement(childNode, node, elemsDic, walker);
                }

                node = walker.GetNextSibling(node);
            }

            return rootNode;
        }

        private static XElement CreateXmlElement(AutomationElement targetElement, string hash = "")
        {
            XElement node = new XElement(GetControlTypeText(targetElement.Current.ControlType));

            var tp = targetElement.Current.GetType();

            foreach(var t in TargetControlTypes)
            {
                node.SetAttributeValue(t, tp.GetProperty(t)?.GetValue(targetElement.Current)?.ToString() ?? "");
            }

            node.SetAttributeValue("Hash", (hash == "") ? targetElement.GetHashCode().ToString() : hash);
            
            return node;
        }
        #endregion

        #region AutomationElement XPath search methods

        public static AutomationElement SearchGUIElementByXPath(AutomationElement rootElement, string xpath, int waitTime, Engine.AutomationEngineInstance engine)
        {
            object ret;
            ret = WaitControls.WaitProcess(waitTime, "AutomationElement",
                new Func<(bool, object)>(() =>
                {
                    (var xml, var dic) = GetElementXml(rootElement);

                    var e = xml.XPathSelectElement(xpath) ?? null;
                    
                    if (e != null)
                    {
                        return (true, dic[e.Attribute("Hash").Value]);
                    }
                    else
                    {
                        return (false, null);
                    }
                }), engine
            );

            if (ret is AutomationElement elem)
            {
                return elem;
            }
            else
            {
                throw new Exception("UIElement not Found");
            }
        }

        public static AutomationElement SearchGUIElementByXPath(ScriptCommand command, string rootElementName, string xpathName, string waitTimeName, Engine.AutomationEngineInstance engine)
        {
            var element = command.ExpandUserVariableAsUIElement(rootElementName, engine);
            var xpath = command.ExpandUserVariableAsXPath(xpathName, engine);
            var wait = command.ExpandValueOrUserVariableAsInteger(waitTimeName, engine);

            return SearchGUIElementByXPath(element, xpath, wait, engine);
        }

        public static AutomationElement SearchGUIElementByXPath(ScriptCommand command, Engine.AutomationEngineInstance engine)
        {
            var elemName = command.GetProperty(new PropertyVirtualProperty(nameof(UIElementControls), nameof(v_InputUIElementName)))?.Name ?? "";
            var xpathName = command.GetProperty(new PropertyVirtualProperty(nameof(UIElementControls), nameof(v_XPath)))?.Name ?? "";
            var waitTimeName = command.GetProperty(new PropertyVirtualProperty(nameof(UIElementControls), nameof(v_WaitTime)))?.Name ?? "";

            return SearchGUIElementByXPath(command, elemName, xpathName, waitTimeName, engine);
        }

        #endregion

        public static TreeNode GetElementTreeNode(string windowName, Engine.AutomationEngineInstance engine, out XElement xml)
        {
            AutomationElement root = GetFromWindowName(windowName, engine);

            TreeWalker walker = TreeWalker.RawViewWalker;

            var tree = CreateTreeNodeFromAutomationElement(root);
            xml = CreateXmlElement(root);

            GetChildElementTreeNode(tree, xml, root, walker, 1, engine);

            return tree;
        }

        private static void GetChildElementTreeNode(TreeNode tree, XElement xml, AutomationElement rootElement, TreeWalker walker, int depth, Engine.AutomationEngineInstance engine)
        {
            AutomationElement node = walker.GetFirstChild(rootElement);

            int siblingCount = 0;
            while(node != null)
            {
                var item = CreateTreeNodeFromAutomationElement(node);
                tree.Nodes.Add(item);

                var childXml = CreateXmlElement(node);
                xml.Add(childXml);

                if ((walker.GetFirstChild(node) != null) && (depth < engine.engineSettings.MaxUIElementInpectDepth))
                {
                    GetChildElementTreeNode(item, childXml, node, walker, (depth + 1), engine);
                }

                siblingCount++;
                if (siblingCount >= engine.engineSettings.MaxUIElementInspectSiblingNodes)
                {
                    break;
                }

                node = walker.GetNextSibling(node);
            }
        }
        

        private static TreeNode CreateTreeNodeFromAutomationElement(AutomationElement element)
        {
            TreeNode node = new TreeNode();
            node.Text = "\"" + element.Current.Name + "\" " + element.Current.LocalizedControlType;
            node.Tag = element;
            return node;
        }

        public static string GetInspectResultFromAutomationElement(AutomationElement elem)
        {
            string res = "";

            try
            {
                res += "Name:\t\"" + elem.Current.Name + "\"\r\n";
                res += "ControlType:\t" + GetControlTypeText(elem.Current.ControlType) + "\r\n";
                res += "LocalizedControlType:\t\"" + elem.Current.LocalizedControlType + "\"\r\n";
                res += "IsEnabled:\t" + elem.Current.IsEnabled.ToString() + "\r\n";
                res += "IsOffscreen:\t" + elem.Current.IsOffscreen.ToString() + "\r\n";
                res += "IsKeyboardFocusable:\t" + elem.Current.IsKeyboardFocusable.ToString() + "\r\n";
                res += "HasKeyboardFocusable:\t" + elem.Current.HasKeyboardFocus.ToString() + "\r\n";
                res += "AccessKey:\t\"" + elem.Current.AccessKey + "\"\r\n";
                res += "ProcessId:\t" + elem.Current.ProcessId.ToString() + "\r\n";
                res += "AutomationId:\t\"" + elem.Current.AutomationId + "\"\r\n";
                res += "FrameworkId:\t\"" + elem.Current.FrameworkId + "\"\r\n";
                res += "ClassName:\t\"" + elem.Current.ClassName + "\"\r\n";
                res += "IsContentElement:\t" + elem.Current.IsContentElement.ToString() + "\r\n";
                res += "IsPassword:\t" + elem.Current.IsPassword.ToString() + "\r\n";

                res += "AcceleratorKey:\t\"" + elem.Current.AcceleratorKey + "\"\r\n";
                res += "HelpText:\t\"" + elem.Current.HelpText + "\"\r\n";
                res += "IsControlElement:\t" + elem.Current.IsControlElement.ToString() + "\r\n";
                res += "IsRequiredForForm:\t" + elem.Current.IsRequiredForForm.ToString() + "\r\n";
                res += "ItemStatus:\t\"" + elem.Current.ItemStatus + "\"\r\n";
                res += "ItemType:\t\"" + elem.Current.ItemType + "\"\r\n";
                res += "NativeWindowHandle:\t" + elem.Current.NativeWindowHandle.ToString() + "\r\n";

                res += "IsDockPatternAvailableProperty:\t" + (bool)elem.GetCurrentPropertyValue(AutomationElement.IsDockPatternAvailableProperty) + "\r\n";
                res += "IsExpandCollapsePatternAvailableProperty:\t" + (bool)elem.GetCurrentPropertyValue(AutomationElement.IsExpandCollapsePatternAvailableProperty) + "\r\n";
                res += "IsGridPatternAvailableProperty:\t" + (bool)elem.GetCurrentPropertyValue(AutomationElement.IsGridPatternAvailableProperty) + "\r\n";
                res += "IsGridItemPatternAvailableProperty:\t" + (bool)elem.GetCurrentPropertyValue(AutomationElement.IsGridItemPatternAvailableProperty) + "\r\n";
                res += "IsInvokePatternAvailableProperty:\t" + (bool)elem.GetCurrentPropertyValue(AutomationElement.IsInvokePatternAvailableProperty) + "\r\n";
                res += "IsMultipleViewPatternAvailableProperty:\t" + (bool)elem.GetCurrentPropertyValue(AutomationElement.IsMultipleViewPatternAvailableProperty) + "\r\n";
                res += "IsRangeValuePatternAvailableProperty:\t" + (bool)elem.GetCurrentPropertyValue(AutomationElement.IsRangeValuePatternAvailableProperty) + "\r\n";
                res += "IsScrollPatternAvailableProperty:\t" + (bool)elem.GetCurrentPropertyValue(AutomationElement.IsScrollPatternAvailableProperty) + "\r\n";
                res += "IsScrollItemPatternAvailableProperty:\t" + (bool)elem.GetCurrentPropertyValue(AutomationElement.IsScrollItemPatternAvailableProperty) + "\r\n";
                res += "IsSelectionPatternAvailableProperty:\t" + (bool)elem.GetCurrentPropertyValue(AutomationElement.IsSelectionPatternAvailableProperty) + "\r\n";
                res += "IsSelectionItemPatternAvailableProperty:\t" + (bool)elem.GetCurrentPropertyValue(AutomationElement.IsSelectionItemPatternAvailableProperty) + "\r\n";
                res += "IsTablePatternAvailableProperty:\t" + (bool)elem.GetCurrentPropertyValue(AutomationElement.IsTablePatternAvailableProperty) + "\r\n";
                res += "IsTableItemPatternAvailableProperty:\t" + (bool)elem.GetCurrentPropertyValue(AutomationElement.IsTableItemPatternAvailableProperty) + "\r\n";
                res += "IsTextPatternAvailableProperty:\t" + (bool)elem.GetCurrentPropertyValue(AutomationElement.IsTextPatternAvailableProperty) + "\r\n";
                res += "IsTogglePatternAvailableProperty:\t" + (bool)elem.GetCurrentPropertyValue(AutomationElement.IsTogglePatternAvailableProperty) + "\r\n";
                res += "IsTransformPatternAvailableProperty:\t" + (bool)elem.GetCurrentPropertyValue(AutomationElement.IsTransformPatternAvailableProperty) + "\r\n";
                res += "IsValuePatternAvailableProperty:\t" + (bool)elem.GetCurrentPropertyValue(AutomationElement.IsValuePatternAvailableProperty) + "\r\n";
                res += "IsWindowPatternAvailableProperty:\t" + (bool)elem.GetCurrentPropertyValue(AutomationElement.IsWindowPatternAvailableProperty) + "\r\n";
            }
            catch(Exception ex)
            {
                res += "Error: " + ex.Message;
            }

            return res;
        }

        public static string GetXPath(XElement xml, AutomationElement elem, bool useNameAttribute = true, bool useAutomationIdAttribute = false)
        {
            string searchPath = "//" + GetControlTypeText(elem.Current.ControlType) + "[@Hash=\"" + elem.GetHashCode() + "\"]";
            XElement trgElement = xml.XPathSelectElement(searchPath);

            if (trgElement == null)
            {
                return "";
            }

            string xpath = "";
            while (trgElement.Parent != null)
            {
                xpath = CreateXPath(trgElement, useNameAttribute, useAutomationIdAttribute) + xpath;
                trgElement = trgElement.Parent;
            }

            return xpath;
        }

        public static string GetXPath(XElement xml, AutomationElement trgElem, AutomationElement curElem, bool useNameAttribute = true, bool useAutomationIdAttribute = false)
        {
            string searchPath = "//" + GetControlTypeText(trgElem.Current.ControlType) + "[@Hash=\"" + trgElem.GetHashCode() + "\"]";
            XElement trgElement = xml.XPathSelectElement(searchPath);

            searchPath = GetControlTypeText(curElem.Current.ControlType) + "[@Hash=\"" + curElem.GetHashCode() + "\"]";
            XElement curElement = xml.XPathSelectElement("//" + searchPath);
            if (curElement == null)
            {
                // curElem is root-window-node ?
                if (xml.Attribute("Hash").Value == curElem.GetHashCode().ToString())
                {
                    curElement = xml;
                }
            }

            if ((trgElement == null) || (curElement == null))
            {
                return "";
            }

            string xpath = "";
            while (trgElement.Parent != null)
            {
                xpath = CreateXPath(trgElement, useNameAttribute, useAutomationIdAttribute) + xpath;
                trgElement = trgElement.Parent;

                if (trgElement == curElement)
                {
                    break;
                }
            }

            if (trgElement == curElement)
            {
                return "/" + xpath;
            }
            else
            {
                return "";
            }
        }

        private static string CreateXPath(XElement elemNode, bool useNameAttribute = true, bool useAutomationIdAttribute = false)
        {
            XElement parentNode = elemNode.Parent;

            string elemType = elemNode.Name.ToString();
            string elemHash = elemNode.Attribute("Hash").Value;
            string xpath;
            
            if (useAutomationIdAttribute && (elemNode.Attribute("AutomationId").Value != ""))
            {
                xpath = "/" + elemType + "[@AutomationId=\"" + SecurityElement.Escape(elemNode.Attribute("AutomationId").Value) + "\"]";
                XElement idNode = parentNode.XPathSelectElement("." + xpath);
                if (idNode != null)
                {
                    if (idNode.Attribute("Hash").Value == elemHash)
                    {
                        return xpath;
                    }
                }
            }

            if (useNameAttribute && (elemNode.Attribute("Name").Value != ""))
            {
                xpath = "/" + elemType + "[@Name=\"" + SecurityElement.Escape(elemNode.Attribute("Name").Value) + "\"]";
                XElement nameNode = parentNode.XPathSelectElement("." + xpath);
                if (nameNode != null)
                {
                    if (nameNode.Attribute("Hash").Value == elemHash)
                    {
                        return xpath;
                    }
                }
            }

            xpath = "/" + elemType;
            IEnumerable<XElement> typeNodes = parentNode.XPathSelectElements("." + xpath);
            int idx = 1;
            foreach(XElement nd in typeNodes)
            {
                if (nd.Attribute("Hash").Value == elemHash)
                {
                    return xpath + "[" + idx + "]";
                }
                idx++;
            }

            throw new Exception("Fail Create UIElement XPath");
        }

        #region events

        /// <summary>
        /// show GUI Inspect Tool and get XPath
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void lnkGUIInspectTool_UsedByXPath_Click(object sender, EventArgs e)
        {
            using (var fm = new UI.Forms.ScriptBuilder.CommandEditor.Supplemental.frmGUIInspect())
            {
                var trgCtrl = (Control)sender;
                if (fm.ShowDialog(trgCtrl.FindForm()) == DialogResult.OK)
                {
                    object ctrl = trgCtrl.Tag;
                    if (ctrl is TextBox txt)
                    {
                        txt.Text = fm.XPath;
                    }
                    else if (ctrl is ComboBox cmb)
                    {
                        cmb.Text = fm.XPath;
                    }
                    else if (ctrl is DataGridView dgv)
                    {
                        dgv.CurrentCell.Value = fm.XPath;
                    }
                }
            }
        }

        private static void dgvUpdateProcess(object sender, Action<DataTable> updateFunc)
        {
            var ctrl = ((Control)sender).Tag;
            if (ctrl is DataGridView dgv)
            {
                var propName = dgv.DataBindings[0].BindingMemberInfo.BindingField;
                var command = (ScriptCommand)dgv.DataBindings[0].DataSource;
                var propInfo = command.GetType().GetProperty(propName);
                var source = propInfo.GetValue(command);
                if (source is DataTable tbl)
                {
                    updateFunc(tbl);
                    RenderSearchParameterDataGridView(dgv);
                }
            }
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
                    dgvUpdateProcess(sender, new Action<DataTable>((tbl) =>
                    {
                        parseInspectToolResult(fm.InspectResult, tbl);
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
            dgvUpdateProcess(sender, new Action<DataTable>((tbl) =>
            {
                CreateEmptyParamters(tbl);
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
                    dgvUpdateProcess(sender, new Action<DataTable>((tbl) =>
                    {
                        parseInspectToolResult(fm.inspectResult, tbl);
                    }));
                }
            }
        }

        public static void UIAutomationDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
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
        public static void UIAutomationDataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                e.Cancel = true;
            }
        }
        #endregion

        public static void RenderSearchParameterDataGridView(DataGridView dgv)
        {
            DataGridViewRow r = null;
            foreach(DataGridViewRow row in dgv.Rows)
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
    }
}
