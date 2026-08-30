using System;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for IFileExistsFilePathPathResult Extension methods
    /// </summary>
    public static class EM_FileExistsFilePathPathResultPropertiesExtensionMethods
    {
        /// <summary>
        /// general file action. This method search target file before execute actionFunc, and try store Found File Path after execute actionFunc.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <param name="actionFunc">return process file path</param>
        /// <param name="errorFunc"></param>
        public static void FileAction(this IFileExistsFilePathPathResultProperties command, Engine.AutomationEngineInstance engine, Func<string, string> actionFunc, Action<Exception> errorFunc = null)
        {
            command.FileAction(engine, actionFunc,
                new Action<string, string>((before, after) =>
                {
                    command.StoreResultFilePathInUserVariable(after, engine);
                }),
                errorFunc
            );
        }

        /// <summary>
        /// store file path result in User variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="path"></param>
        /// <param name="engine"></param>
        public static void StoreResultFilePathInUserVariable(this IFileExistsFilePathPathResultProperties command, string path, Engine.AutomationEngineInstance engine)
        {
            var variableName = command.ToScriptCommand().GetRawPropertyValueAsString(nameof(command.v_ResultPath), "Result Path");
            if (!string.IsNullOrEmpty(variableName))
            {
                command.StoreFilePathInUserVariable(path, nameof(command.v_ResultPath), engine);
            }
        }
    }
}
