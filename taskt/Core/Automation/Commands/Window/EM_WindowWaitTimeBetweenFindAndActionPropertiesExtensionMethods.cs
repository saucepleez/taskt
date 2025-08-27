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
        public static int ExpandValueOrUserVariableAsWaitTimeBetweenFindAndAction(this IWindowWaitTimeBetweenFindAndActionProperties command, AutomationEngineInstance engine)
        {
            return ((ScriptCommand)command).ExpandValueOrUserVariableAsInteger(nameof(command.v_WaitTimeBetweenFindAndAction), engine);
        }
    }
}
