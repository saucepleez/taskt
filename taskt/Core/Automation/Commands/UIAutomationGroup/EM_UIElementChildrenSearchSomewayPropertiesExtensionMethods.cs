using System;
using System.Windows.Automation;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    public static class EM_UIElementChildrenSearchSomewayPropertiesExtensionMethods
    {
        /// <summary>
        /// expand value or user variable as wait time for UIElement
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static int ExpandValueOrUserVariableAsWaitTimeForUIElement(this IUIElementChildrenSearchSomewayProperties command, Engine.AutomationEngineInstance engine)
        {
            return command.ToScriptCommand().ExpandValueOrUserVariableAsInteger(nameof(command.v_WaitTimeForUIElement), engine);
        }

        /// <summary>
        /// expand value or user variable as Max Siblings
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static int ExpandValueOrUserVariableAsMaxSiblings(this IUIElementChildrenSearchSomewayProperties command, Engine.AutomationEngineInstance engine)
        {
            if (string.IsNullOrEmpty(command.v_MaxSiblings))
            {
                command.v_MaxSiblings = "64";
            }
            return command.ToScriptCommand().ExpandValueOrUserVariableAsInteger(nameof(command.v_MaxSiblings), engine);
        }

        /// <summary>
        /// get check max siblings function
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns>When Func returns true, max siblings</returns>
        public static Func<int, bool> GetMaxSiblingsFunc(this IUIElementChildrenSearchSomewayProperties command, Engine.AutomationEngineInstance engine)
        {
            var maxSiblings = command.ExpandValueOrUserVariableAsMaxSiblings(engine);
            if (maxSiblings == 0)
            {
                return new Func<int, bool>((n) => false);
            }
            else
            {
                return new Func<int, bool>((n) => (n > maxSiblings));
            }
        }

        /// <summary>
        /// search someway any-one UIElement Action core
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <param name="searchFunc"></param>
        /// <param name="actionFunc"></param>
        /// <param name="errorFunc"></param>
        public static void SearchAnyUIElementActionCore(this IUIElementChildrenSearchSomewayProperties command, Engine.AutomationEngineInstance engine, Func<AutomationElement> searchFunc, Action<AutomationElement> actionFunc, Action<Exception> errorFunc = null)
        {
            try
            {
                var elem = searchFunc();
                actionFunc(elem);
                command.StoreWindowNameAndWindowHandleInUserVariablesFromUIElement(elem, engine);
            }
            catch (Exception ex)
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
