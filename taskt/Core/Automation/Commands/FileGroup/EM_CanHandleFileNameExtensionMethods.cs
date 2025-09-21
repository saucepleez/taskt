using System;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// Handle File Name Extension methods
    /// </summary>
    public static class EM_CanHandleFileNameExtensionMethods
    {
        /// <summary>
        /// expand value or User Variable As File name
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string ExpandValueOrUserVariableAsFileName(this ICanHandleFileName command, string parameterName, Engine.AutomationEngineInstance engine)
        {
            return command.ExpandValueOrUserVariableAsFileOrFolderName(parameterName, engine, EM_CanHandleFileOrFolderName.ExpandValueType.File);
        }

        /// <summary>
        /// check valid filename
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool IsValidFileName(string fileName)
        {
            return EM_CanHandleFileOrFolderName.IsValidFileOrFolderName(fileName);
        }
    }
}
