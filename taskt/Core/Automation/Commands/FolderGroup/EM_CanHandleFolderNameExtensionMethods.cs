namespace taskt.Core.Automation.Commands
{
    public static class EM_CanHandleFolderNameExtensionMethods
    {
        /// <summary>
        /// expand value or variable as folder name
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static string ExpandValueOrUserVariableAsFolderName(this ICanHandleFolderName command, string parameterName, Engine.AutomationEngineInstance engine)
        {
            return command.ExpandValueOrUserVariableAsFileOrFolderName(parameterName, engine, EM_CanHandleFileOrFolderName.ExpandValueType.Folder);
        }

        /// <summary>
        /// check valid folder name
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public static bool IsValidFolderName(string folderName)
        {
            return EM_CanHandleFileOrFolderName.IsValidFileOrFolderName(folderName);
        }
    }
}
