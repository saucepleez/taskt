namespace taskt.Core.Automation.Commands
{
    public static class EM_FolderPathPropertiesExtensionMethods
    {
        /// <summary>
        /// expand value or user variable as folder path
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static string ExpandValueOrUserVariableAsFolderPath(this IFolderPathProperties command, Engine.AutomationEngineInstance engine)
        {
            return command.ExpandValueOrUserVariableAsFolderPath(nameof(command.v_TargetFolderPath), engine);
        }
    }
}
