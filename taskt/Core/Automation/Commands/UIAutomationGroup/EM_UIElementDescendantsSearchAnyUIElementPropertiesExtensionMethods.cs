using System;
using System.Collections.Generic;
using System.Windows.Automation;
using taskt.Core.Automation.Commands.WindowGroup;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    public static class EM_UIElementDescendantsSearchAnyUIElementPropertiesExtensionMethods
    {
        /// <summary>
        /// get Check Found func, max UIElements func
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns>(found-func, max-func)</returns>
        public static (Func<List<AutomationElement>, bool>, Func<List<AutomationElement>, bool>) GetCheckFoundAndMaxUIElementsFunc(this IUIElementDescendantsSearchAnyUIElementProperties command, Engine.AutomationEngineInstance engine)
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
        public static List<AutomationElement> DeepSearchUIElements(this IUIElementDescendantsSearchAnyUIElementProperties command, AutomationElement rootElement, Engine.AutomationEngineInstance engine)
        {
            (var foundFunc, var maxElementsFunc) = command.GetCheckFoundAndMaxUIElementsFunc(engine);

            return EM_UIElementDescendantsSearchParametersPropertiesExtensionMethods.DeepSearchUIElementsCore(command, rootElement, foundFunc, maxElementsFunc, engine);
        }

        /// <summary>
        /// Get UIElement from Deep Search UIElements Result
        /// </summary>
        /// <param name="command"></param>
        /// <param name="rootElement"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static AutomationElement GetUIElementFromDeepSearchUIElements(this IUIElementDescendantsSearchAnyUIElementProperties command, AutomationElement rootElement, Engine.AutomationEngineInstance engine)
        {
            var elems = command.DeepSearchUIElements(rootElement, engine);
            return command.GetUIElementFromList(elems, engine);
        }

        /// <summary>
        /// deep seach any-one UIElement action
        /// </summary>
        /// <param name="command"></param>
        /// <param name="rootElement"></param>
        /// <param name="engine"></param>
        /// <param name="actionFunc"></param>
        /// <param name="errorFunc"></param>
        public static void DeepSearchAnyUIElementAction(this IUIElementDescendantsSearchAnyUIElementProperties command, AutomationElement rootElement, Engine.AutomationEngineInstance engine, Action<AutomationElement> actionFunc, Action<Exception> errorFunc = null)
        {
            try
            {
                var elem = GetUIElementFromDeepSearchUIElements(command, rootElement, engine);
                actionFunc(elem);

                if (command.IsWindowNameOrWindowHandleResultsSpecified())
                {
                    command.StoreWindowNameAndWindowHandleInUserVariablesFromUIElement(rootElement, engine);
                }
            }
            catch (Exception ex)
            {
                {
                    if (errorFunc != null)
                    {
                        errorFunc(ex);
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// deep search UIElements action
        /// </summary>
        /// <param name="command"></param>
        /// <param name="rootElement"></param>
        /// <param name="engine"></param>
        /// <param name="actionFunc"></param>
        /// <param name="errorFunc"></param>
        public static void DeepSearchUIElementsAction(this IUIElementDescendantsSearchAnyUIElementProperties command, AutomationElement rootElement, Engine.AutomationEngineInstance engine, Action<List<AutomationElement>> actionFunc, Action<Exception> errorFunc = null)
        {
            try
            {
                var elem = DeepSearchUIElements(command, rootElement, engine);
                actionFunc(elem);
                command.StoreWindowNameAndWindowHandleInUserVariablesFromUIElement(rootElement, engine);
            }
            catch (Exception ex)
            {
                {
                    if (errorFunc != null)
                    {
                        errorFunc(ex);
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }
        }
    }
}
