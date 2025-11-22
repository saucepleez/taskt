using System;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    public static class EM_UIElementDeepSearchSomewayPropertiesExtensionMethods
    {

        /// <summary>
        /// expand value or user variable as Max Depth
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static int ExpandValueOrUserVariableAsMaxDepth(this IUIElementDeepSearchSomewayProperties command, Engine.AutomationEngineInstance engine)
        {
            if (string.IsNullOrEmpty(command.v_MaxDepth))
            {
                command.v_MaxDepth = "16";
            }
            return command.ToScriptCommand().ExpandValueOrUserVariableAsInteger(nameof(command.v_MaxDepth), engine);
        }

        /// <summary>
        /// get check Max Depth func
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns>when Func returns true, max depth</returns>
        public static Func<int, bool> GetMaxDepthFunc(this IUIElementDeepSearchSomewayProperties command, Engine.AutomationEngineInstance engine)
        {
            var maxDepth = command.ExpandValueOrUserVariableAsMaxDepth(engine);
            if (maxDepth == 0)
            {
                return new Func<int, bool>((d) => false);
            }
            else
            {
                return new Func<int, bool>((d) => (d > maxDepth));
            }
        }
    }
}
