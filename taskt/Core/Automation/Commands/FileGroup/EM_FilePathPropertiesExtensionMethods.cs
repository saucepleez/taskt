namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// extension methods for IFilePathProperties
    /// </summary>
    public static class EM_FilePathPropertiesExtensionMethods
    {
        /// <summary>
        /// expand value or user variable to FilePath. this method use PropertyFilePathSetting
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static string ExpandValueOrUserVariableAsFilePath(this IFilePathProperties command, Engine.AutomationEngineInstance engine)
        {
            return command.ExpandValueOrUserVariableAsFilePath(nameof(command.v_TargetFilePath), engine);
        }
    }
}
