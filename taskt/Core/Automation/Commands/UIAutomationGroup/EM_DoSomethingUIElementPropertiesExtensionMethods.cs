using System;
using System.Windows.Automation;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    public static class EM_DoSomethingUIElementPropertiesExtensionMethods
    {
        /// <summary>
        /// store Window name result
        /// </summary>
        /// <param name="command"></param>
        /// <param name="windowName"></param>
        /// <param name="engine"></param>
        public static void StoreWindowNameResultInUserVariable(this IDoSomethingUIElementProperties command, string windowName, AutomationEngineInstance engine)
        {
            if (!string.IsNullOrEmpty(command.v_WindowNameResult))
            {
                windowName.StoreInUserVariable(engine, command.v_WindowNameResult);
            }
        }

        /// <summary>
        /// store window handle result
        /// </summary>
        /// <param name="command"></param>
        /// <param name="whnd"></param>
        /// <param name="engine"></param>
        public static void StoreWindowHandleResultInUserVariable(this IDoSomethingUIElementProperties command, IntPtr whnd, AutomationEngineInstance engine)
        {
            if (!string.IsNullOrEmpty(command.v_WindowHandleResult))
            {
                whnd.StoreInUserVariable(engine, command.v_WindowHandleResult);
            }
        }

        /// <summary>
        /// UIElement action
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <param name="actionFunc"></param>
        public static void UIElementAction(this IDoSomethingUIElementProperties command, AutomationEngineInstance engine, Action<AutomationElement> actionFunc)
        {
            var targetElement = command.ExpandUserVariableAsUIElement(engine);

            // core process
            actionFunc(targetElement);

            if ((!string.IsNullOrEmpty(command.v_WindowNameResult)) || (!string.IsNullOrEmpty(command.v_WindowHandleResult)))
            {
                // get window name and window handle
                (var windowName, var whnd) = EM_CanHandleUIElementExtentionMethods.GetWindowNameAndHandle(targetElement);

                // store window name, handle
                command.StoreWindowNameResultInUserVariable(windowName, engine);
                command.StoreWindowHandleResultInUserVariable(whnd, engine);
            }
        }
    }
}
