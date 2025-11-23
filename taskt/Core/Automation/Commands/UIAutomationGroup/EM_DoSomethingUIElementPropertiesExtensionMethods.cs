using System;
using System.Windows.Automation;
using taskt.Core.Automation.Commands.WindowGroup;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    public static class EM_DoSomethingUIElementPropertiesExtensionMethods
    {
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

            //if ((!string.IsNullOrEmpty(command.v_WindowNameResult)) || (!string.IsNullOrEmpty(command.v_WindowHandleResult)))
            if (command.IsWindowNameOrWindowHandleResultsSpecified())
            {
                // get window name and window handle
                (var windowName, var whnd) = EM_CanHandleUIElementExtentionMethods.GetWindowNameAndHandle(targetElement);

                //// store window name, handle
                //command.StoreWindowNameResultInUserVariable(windowName, engine);
                //command.StoreWindowHandleResultInUserVariable(whnd, engine);
                command.StoreWindowNameAndWindowHandleResultsInUserVariables(windowName, whnd, engine);
            }
        }
    }
}
