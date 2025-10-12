using System;
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
    }
}
