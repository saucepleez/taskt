using System;
using System.Windows.Automation;
using taskt.Core.Automation.Commands.WindowGroup;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    public static class EM_UIElementWindowNameResultsFromUIElementPropertiesExtensionMethods
    {
        /// <summary>
        /// store window name and window handle in User variables from UIElement
        /// </summary>
        /// <param name="command"></param>
        /// <param name="targetElement"></param>
        /// <param name="engine"></param>
        public static void StoreWindowNameAndWindowHandleInUserVariablesFromUIElement(this IUIElementWindowResultsFromUIElementProperties command, AutomationElement targetElement, Engine.AutomationEngineInstance engine)
        {
            if (command.IsWindowNameOrWindowHandleResultsSpecified())
            {
                (var name, var whnd) = EM_CanHandleUIElementExtentionMethods.GetWindowNameAndHandle(targetElement);
                command.StoreWindowNameAndWindowHandleResultsInUserVariables(name, whnd, engine);
            }
        }
    }
}
