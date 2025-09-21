using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{
    public static class EM_DialogResultPropertiesExtentionMethods
    {

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
