using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{
    public static class EM_WindowWaitTimeBetweenFindAndActionPropertiesExtensionMethods
    {
        /// <summary>
        /// expand value or user variable as wait time between find and action
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static double ExpandValueOrUserVariableAsWaitTimeBetweenFindAndAction(this IWindowWaitTimeBetweenFindAndActionProperties command, AutomationEngineInstance engine)
        {
            return (double)((ScriptCommand)command).ExpandValueOrUserVariableAsDecimal(nameof(command.v_WaitTimeBetweenFindAndAction), engine);
        }

        /// <summary>
        /// wait process after window found
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        public static void WaitAfterFindWindowProcess(this IWindowWaitTimeBetweenFindAndActionProperties command, AutomationEngineInstance engine)
        {
            var waitTime = command.ExpandValueOrUserVariableAsWaitTimeBetweenFindAndAction(engine);
            if (waitTime > 0)
            {
                System.Threading.Thread.Sleep((int)(waitTime * 1000));
            }
        }
    }
}
