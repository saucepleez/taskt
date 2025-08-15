using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{
    public static class EM_DialogResultPropertiesExtentionMethods
    {
        /// <summary>
        /// expand value or variable as When Cancel
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        public static string ExpandValueOrUserVariableAsWhenCancel(this IDialogResultProperties command, AutomationEngineInstance engine)
        {
            return ((ScriptCommand)command).ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_WhenCancel), engine);
        }

        /// <summary>
        /// store dialog result in user variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="result"></param>
        /// <param name="engine"></param>
        public static void StoreDialogResultInUserVariable(this IDialogResultProperties command, string result, AutomationEngineInstance engine)
        {
            if (!string.IsNullOrEmpty(command.v_DialogResult))
            {
                result.StoreInUserVariable(engine, command.v_DialogResult);
            }
        }
    }
}
