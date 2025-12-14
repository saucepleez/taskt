using System;

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
    }
}
