using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Automation;
using System.Windows.Forms;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    public static class EM_UIElementCoreSearchParametersPropertiesExtensionMethods
    {
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
        public static readonly string[] TargetControlProperties = new string[]
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
        public static (IUIElementCoreSearchParametersProperties, DataGridView) GetCommandAndSearchDataGridView(Control ctl)
        {
            var editor = FormUIControls.GetCommandEditorFromControl(ctl);
            var command = (IUIElementCoreSearchParametersProperties)editor.selectedCommand;
            //var dgv = FormUIControls.GetPropertyControl<DataGridView>(editor.us.ControlsList, nameof(IUIElementSearchParametersProperties.v_SearchParameters));
            var dgv = (DataGridView)editor.ParameterBindingControls[nameof(command.v_SearchParameters)];

            return (command, dgv);
        }

        /// <summary>
        /// search parameters update process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="updateFunc"></param>
        public static void SearchParametersUpdateProcess(this IUIElementCoreSearchParametersProperties command, DataGridView dgv, Action<DataTable> updateFunc)
        {
            updateFunc(command.v_SearchParameters);
            RenderUIElementSearchParameter(dgv);
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
        /// <returns></returns>
        private static List<PropertyCondition> CreateSearchCondition(this IUIElementCoreSearchParametersProperties comamnd, AutomationEngineInstance engine)
        {
            var table = comamnd.v_SearchParameters;

            // create and populate condition list
            var conditionList = new List<PropertyCondition>();
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

            return conditionList;
        }

        /// <summary>
        /// search children UIElement (!!children elements only!!)
        /// </summary>
        /// <param name="command"></param>
        /// <param name="rootElement"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static List<AutomationElement> SearchChildrenUIElements(this IUIElementCoreSearchParametersProperties command, AutomationElement rootElement, AutomationEngineInstance engine)
        {
            // TODO: use treewalker
            var searchConditions = command.CreateSearchCondition(engine);

            // MEMO: for specify search direction
            Func<AutomationElement, TreeWalker, AutomationElement> firstChildFunc = new Func<AutomationElement, TreeWalker, AutomationElement>((el, wa) =>
            {
                return wa.GetFirstChild(el);
            });
            Func<AutomationElement, TreeWalker, AutomationElement> nextChildFunc = new Func<AutomationElement, TreeWalker, AutomationElement>((el, wa) =>
            {
                return wa.GetNextSibling(el);
            });

            var waitTime = command.ExpandValueOrUserVariableAsWaitTimeForUIElement(engine);

            var ret = WaitControls.WaitProcess(waitTime, "Children UIElement", new Func<Func<bool>, (bool, object)>((timeOutFunc) =>
            {
                var walker = TreeWalker.RawViewWalker;
                var elems = new List<AutomationElement>();
                var node = firstChildFunc(rootElement, walker);
                int sibCnt = 0;
                while (node != null)
                {
                    CheckAndAddProcess(node, searchConditions, elems);
                    if (timeOutFunc())
                    {
                        return (true, elems);
                    }

                    node = nextChildFunc(rootElement, walker);
                    sibCnt++;
                    if (sibCnt >= maxSibling)
                    {
                        return (true, elems);
                    }
                }
                return (true, elems);
            }), engine);

            if (ret is List<AutomationElement> e)
            {
                return e;
            }
        }

        //public static List<AutomationElement> SearchChildrenUIElements(this IUIElementSearchParametersProperties command, AutomationElement rootElement, AutomationEngineInstance engine)
        //{
        //    // TODO: use treewalker
        //    var searchConditions = command.CreateSearchCondition(engine);

        //    if (searchConditions != null)
        //    {
        //        var waitTime = command.ExpandValueOrUserVariableAsWaitTimeForUIElement(engine);

        //        var r = WaitControls.WaitProcess(waitTime, "Children UIElement", new Func<(bool, object)>(() =>
        //        {
        //            var elements = rootElement.FindAll(TreeScope.Children, searchConditions);
        //            if (elements.Count > 0)
        //            {
        //                var ret = new List<AutomationElement>();
        //                foreach (AutomationElement element in elements)
        //                {
        //                    ret.Add(element);
        //                }
        //                return (true, ret);
        //            }
        //            else
        //            {
        //                return (false, null);
        //            }
        //        }), engine);
        //        if (r is List<AutomationElement> list)
        //        {
        //            return list;
        //        }
        //        else
        //        {
        //            return new List<AutomationElement>();
        //        }
        //    }
        //    else
        //    {
        //        var walker = TreeWalker.RawViewWalker;
        //        var elems = new List<AutomationElement>();

        //        var node = walker.GetFirstChild(rootElement);
        //        while (node != null)
        //        {
        //            elems.Add(node);
        //            node = walker.GetNextSibling(node);
        //        }
        //        return elems;
        //    }
        //}

        /// <summary>
        /// expand value or user variable as wait time for UIElement
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static int ExpandValueOrUserVariableAsWaitTimeForUIElement(this IUIElementCoreSearchParametersProperties command, Engine.AutomationEngineInstance engine)
        {
            return command.ToScriptCommand().ExpandValueOrUserVariableAsInteger(nameof(command.v_WaitTimeForUIElement), engine);
        }

        /// <summary>
        /// deep search UIElements
        /// </summary>
        /// <param name="rootElement"></param>
        /// <param name="searchCondition"></param>
        /// <param name="maxDepth"></param>
        /// <param name="maxSibling"></param>
        /// <param name="timeoutFunc"></param>
        /// <returns></returns>
        private static List<AutomationElement> DeepSearchUIElements(AutomationElement rootElement, List<PropertyCondition> searchCondition, int maxDepth, int maxSibling, Func<bool> timeoutFunc)
        {
            // MEMO: for specify search direction
            Func<AutomationElement, TreeWalker, AutomationElement> firstChildFunc = new Func<AutomationElement, TreeWalker, AutomationElement>((el, wa) =>
            {
                return wa.GetFirstChild(el);
            });
            Func<AutomationElement, TreeWalker, AutomationElement> nextChildFunc = new Func<AutomationElement, TreeWalker, AutomationElement>((el, wa) =>
            {
                return wa.GetNextSibling(el);
            });

            var walker = TreeWalker.RawViewWalker;

            var ret = new List<AutomationElement>();
            if (CheckAutomationElementCondition(rootElement, searchCondition))
            {
                ret.Add(rootElement);
            }

            return DeepSearchUIElements_DepthFirst(rootElement, searchCondition, 0, maxDepth, maxSibling, walker, firstChildFunc, nextChildFunc, timeoutFunc, ret);
        }

        /// <summary>
        /// deep Search GUI Element used by TreeWalker (Depth First)
        /// </summary>
        /// <param name="rootElement"></param>
        /// <param name="searchConditions"></param>
        /// <param name="currentDepth">current depth</param>
        /// <param name="maxDepath">depth limit</param>
        /// <param name="maxSibling">sibling limit</param>
        /// <param name="walker"></param>
        /// <param name="firstChildFunc"></param>
        /// <param name="nextChildFunc"></param>
        /// <param name="timeoutFunc"></param>
        /// <param name="matchedElements">if returns true time-out</param>
        /// <returns></returns>
        private static List<AutomationElement> DeepSearchUIElements_DepthFirst(AutomationElement rootElement, List<PropertyCondition> searchConditions, 
                            int currentDepth, int maxDepath, int maxSibling,
                            TreeWalker walker, 
                            Func<AutomationElement, TreeWalker, AutomationElement> firstChildFunc, Func<AutomationElement, TreeWalker, AutomationElement> nextChildFunc, 
                            Func<bool> timeoutFunc, List<AutomationElement> matchedElements)
        {
            var node = firstChildFunc(rootElement, walker);
            int sibCount = 0;
            while (node != null)
            {
                CheckAndAddProcess(node, searchConditions, matchedElements);
                if (timeoutFunc())
                {
                    return matchedElements;
                }

                // check UIElement has child elements
                if ((walker.GetFirstChild(node) != null) && ((currentDepth + 1) < maxDepath))
                {
                    DeepSearchUIElements_DepthFirst(node, searchConditions, (currentDepth + 1), maxDepath, maxSibling, walker, firstChildFunc, nextChildFunc, timeoutFunc, matchedElements);
                }
                if (timeoutFunc())
                {
                    return matchedElements;
                }

                // next sibling
                node = nextChildFunc(node, walker);
                sibCount++;
                if (sibCount >= maxSibling)
                {
                    break;
                }
            }

            return matchedElements;
        }

        /// <summary>
        /// check if UIElement matches specified conditions, and when matches add to List
        /// </summary>
        /// <param name="targetElement"></param>
        /// <param name="conditions"></param>
        /// <param name="matchedElements"></param>
        private static void CheckAndAddProcess(AutomationElement targetElement, List<PropertyCondition> conditions, List<AutomationElement> matchedElements)
        {
            if (CheckAutomationElementCondition(targetElement, conditions))
            {
                matchedElements.Add(targetElement);
            }
        }

        /// <summary>
        /// check if UIElement matches specified PropertyConditions
        /// </summary>
        /// <param name="targetElement"></param>
        /// <param name="conditions"></param>
        /// <returns></returns>
        private static bool CheckAutomationElementCondition(AutomationElement targetElement, List<PropertyCondition> conditions)
        {
            bool result = true;
            foreach (var c in conditions)
            {
                object p = targetElement.GetCurrentPropertyValue(c.Property);

                switch (c.Property.ProgrammaticName)
                {
                    case "AutomationElementIdentifiers.ControlTypeProperty":
                        // ControlType compare
                        result &= (c.Value.ToString() == ((ControlType)p).Id.ToString());
                        break;

                    default:
                        // normal compare
                        result &= (c.Value.ToString() == p.ToString());
                        break;
                }

                if (!result)
                {
                    break;
                }
            }
            return result;
        }
    }
}
