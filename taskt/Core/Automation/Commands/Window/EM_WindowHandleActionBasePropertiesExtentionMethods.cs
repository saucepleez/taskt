using System;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{
    public static class EM_WindowHandleActionBasePropertiesExtentionMethods
    {
        /// <summary>
        /// expand value or user variable as wait time between find and action
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static int ExpandValueOrUserVariableAsWaitTimeBetweenFindAndAction(this IWindowHandleActionBasePropeties command, AutomationEngineInstance engine)
        {
            return ((ScriptCommand)command).ExpandValueOrUserVariableAsInteger(nameof(command.v_WaitTimeBetweenFindAndAction), engine);
        }

        /// <summary>
        /// get window handle and wait before execution
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static IntPtr GetWindowHandleAndWait(this IWindowHandleActionBasePropeties command, AutomationEngineInstance engine)
        {
            var whnd = command.GetWindowHandle(engine);
            var waitTime = command.ExpandValueOrUserVariableAsWaitTimeBetweenFindAndAction(engine);
            if (waitTime > 0)
            {
                System.Threading.Thread.Sleep(waitTime * 1000);
            }
            return whnd;
        }
    }
}
