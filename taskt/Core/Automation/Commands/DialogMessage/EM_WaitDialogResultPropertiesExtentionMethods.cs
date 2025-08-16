using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{
    public static class EM_WaitDialogResultPropertiesExtentionMethods
    {
        /// <summary>
        /// expand value or variable as When Cancel
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        public static string ExpandValueOrUserVariableAsWhenCancel(this IWaitDialogResultProperties command, AutomationEngineInstance engine)
        {
            return ((ScriptCommand)command).ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_WhenCancel), engine);
        }
    }
}
