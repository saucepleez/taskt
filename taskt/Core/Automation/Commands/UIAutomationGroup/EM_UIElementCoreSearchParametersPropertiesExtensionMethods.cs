using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Automation;
using System.Windows.Forms;

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
        public static List<PropertyCondition> CreateSearchCondition(this IUIElementCoreSearchParametersProperties comamnd, Engine.AutomationEngineInstance engine)
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
        public static List<AutomationElement> SearchChildrenUIElements(this IUIElementCoreSearchParametersProperties command, AutomationElement rootElement, Engine.AutomationEngineInstance engine)
        {
            var searchConditions = command.CreateSearchCondition(engine);

            (var firstChildFunc, var nextChildFunc) = command.GetSiblingNodeFunc(engine);

            var waitTime = command.ExpandValueOrUserVariableAsWaitTimeForUIElement(engine);

            var maxSiblingsFunc = command.GetMaxSiblingsFunc(engine);
            var maxUIElementsFunc = command.GetMaxNumberUIElementsFunc(engine);

            var ret = WaitControls.WaitProcess(waitTime, "Children UIElement", new Func<Func<bool>, (bool, object)>((timeOutFunc) =>
            {
                var walker = TreeWalker.RawViewWalker;
                var elems = new List<AutomationElement>();
                var node = firstChildFunc(rootElement, walker);
                int sibCnt = 0;
                while (node != null)
                {
                    CheckAndAddProcess(node, searchConditions, elems);
                    if (timeOutFunc() || maxUIElementsFunc(elems))
                    {
                        return ((elems.Count > 0), elems);
                    }

                    node = nextChildFunc(node, walker);
                    sibCnt++;
                    if (maxSiblingsFunc(sibCnt))
                    {
                        return ((elems.Count > 0), elems);
                    }
                }
                return ((elems.Count > 0), elems);
            }), engine);

            if (ret is List<AutomationElement> e)
            {
                return e;
            }
            else
            {
                // not found
                return new List<AutomationElement>();
            }
        }

        /// <summary>
        /// expand value or user varaible as Max Number of UIElements to search
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static int ExpandValueOrUserVariableAsMaxNumberUIElements(this IUIElementCoreSearchParametersProperties command, Engine.AutomationEngineInstance engine)
        {
            if (string.IsNullOrEmpty(command.v_MaxNumberUIElements))
            {
                command.v_MaxNumberUIElements = "0";
            }
            return command.ToScriptCommand().ExpandValueOrUserVariableAsInteger(nameof(command.v_MaxNumberUIElements), engine);
        }

        /// <summary>
        /// get check max number of UIElements function
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns>When Func returns true, max UIElements</returns>
        public static Func<List<AutomationElement>, bool> GetMaxNumberUIElementsFunc(this IUIElementCoreSearchParametersProperties command, Engine.AutomationEngineInstance engine)
        {
            var maxElements = command.ExpandValueOrUserVariableAsMaxNumberUIElements(engine);
            if (maxElements == 0)
            {
                return new Func<List<AutomationElement>, bool>((e) => false);
            }
            else
            {
                return new Func<List<AutomationElement>, bool>((e) => (e.Count >= maxElements));
            }
        }

        /// <summary>
        /// get UIElement siblings node search Funcs
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns>(First-Child, Next-Child)</returns>
        public static (Func<AutomationElement, TreeWalker, AutomationElement>, Func<AutomationElement, TreeWalker, AutomationElement>) GetSiblingNodeFunc(this IUIElementCoreSearchParametersProperties command, Engine.AutomationEngineInstance engine)
        {
            switch(command.ToScriptCommand().ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_SiblingsDirection), engine))
            {
                case "last to first":
                    return (
                            new Func<AutomationElement, TreeWalker, AutomationElement>((e, w) =>
                            {
                                return w.GetLastChild(e);
                            }),
                            new Func<AutomationElement, TreeWalker, AutomationElement>((e, w) =>
                            {
                                return w.GetPreviousSibling(e);
                            })
                        );
                case "fist to last":
                default:    // default not works
                    return (
                            new Func<AutomationElement, TreeWalker, AutomationElement>((e, w) =>
                            {
                                return w.GetFirstChild(e);
                            }),
                            new Func<AutomationElement, TreeWalker, AutomationElement>((e, w) =>
                            {
                                return w.GetNextSibling(e);
                            })
                        );
            }
        }

        /// <summary>
        /// check if UIElement matches specified conditions, and when matches add to List
        /// </summary>
        /// <param name="targetElement"></param>
        /// <param name="conditions"></param>
        /// <param name="matchedElements"></param>
        public static void CheckAndAddProcess(AutomationElement targetElement, List<PropertyCondition> conditions, List<AutomationElement> matchedElements)
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
