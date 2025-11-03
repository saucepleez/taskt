using System;
using System.Collections.Generic;
using System.Windows.Automation;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    public static class EM_UIElementDeepSearchAnyUIElementPropertiesExtensionMethods
    {
        /// <summary>
        /// get Check Found func, max UIElements func
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns>(found-func, max-func)</returns>
        public static (Func<List<AutomationElement>, bool>, Func<List<AutomationElement>, bool>) GetCheckFoundAndMaxUIElementsFunc(this IUIElementDeepSearchAnyUIElementProperties command, Engine.AutomationEngineInstance engine)
        {
            var index = command.ExpandValueOrUserVariableAsUIElementIndex(engine);
            if (string.IsNullOrEmpty(command.v_MaxNumberUIElements))
            {
                if (index >= 0)
                {
                    command.v_MaxNumberUIElements = (index + 1).ToString();
                }
                else
                {
                    command.v_MaxNumberUIElements = "0";    // all UIElements
                }
            }
            // max uielements
            var maxFunc = command.GetMaxNumberUIElementsFunc(engine);


            Func<List<AutomationElement>, bool> foundFunc;
            if (index >= 0)
            {
                foundFunc = new Func<List<AutomationElement>, bool>(elems =>
                {
                    return (elems.Count >= index);
                });
            }
            else
            {
                foundFunc = new Func<List<AutomationElement>, bool>(elems =>
                {
                    return (elems.Count > 0);
                });
            }
            return (foundFunc, maxFunc);
        }

        /// <summary>
        /// Deep search UIElements
        /// </summary>
        /// <param name="command"></param>
        /// <param name="rootElement"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static List<AutomationElement> DeepSearchUIElements(this IUIElementDeepSearchAnyUIElementProperties command, AutomationElement rootElement, Engine.AutomationEngineInstance engine)
        {
            (var foundFunc, var maxElementsFunc) = command.GetCheckFoundAndMaxUIElementsFunc(engine);

            return EM_UIElementDeepSearchParametersPropertiesExtensionMethods.DeepSearchUIElementsCore(command, rootElement, foundFunc, maxElementsFunc, engine);
        }
    }
}
