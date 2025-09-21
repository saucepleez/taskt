using System;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for IFileExistsFilePathBeforeAfterPathResultProperties Extension methods
    /// </summary>
    public static class EM_FileExistsFilePathBeforeAfterPathResultPropertiesExtensionMethods
    {
        /// <summary>
        /// general file action. This method search target file before execute actionFunc, and try store Found File Path after execute actionFunc.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <param name="actionFunc">return processed file path</param>
        /// <param name="errorFunc"></param>
        public static void FileAction(this IFileExistsFilePathBeforeAfterPathResultProperties command, Engine.AutomationEngineInstance engine, Func<string, string> actionFunc, Action<Exception> errorFunc = null)
        {
            command.FileAction(engine, actionFunc,
                new Action<string, string>((before, after) =>
                {
                    command.StoreBeforeAfterFilePathsResultInUserVariables(before, after, engine);
                }),
                errorFunc
            );
        }

        /// <summary>
        /// store before and after file paths result in user variables
        /// </summary>
        /// <param name="command"></param>
        /// <param name="beforePath"></param>
        /// <param name="afterPath"></param>
        /// <param name="engine"></param>
        public static void StoreBeforeAfterFilePathsResultInUserVariables(this IFileExistsFilePathBeforeAfterPathResultProperties command, string beforePath, string afterPath, Engine.AutomationEngineInstance engine)
        {
            command.StoreBeforeFilePathResultInUserVariable(beforePath, engine);
            command.StoreAfterFilePathResultInUserVariable(afterPath, engine);
        }

        /// <summary>
        /// store before file path result in user variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="path"></param>
        /// <param name="engine"></param>
        public static void StoreBeforeFilePathResultInUserVariable(this IFileExistsFilePathBeforeAfterPathResultProperties command, string path, Engine.AutomationEngineInstance engine)
        {
            command.StoreSpecifiedPathResultInUserVariable(path, nameof(command.v_BeforeFilePathResult), "Before Result Path", engine);
        }

        /// <summary>
        /// store after file path result in user variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="path"></param>
        /// <param name="engine"></param>
        public static void StoreAfterFilePathResultInUserVariable(this IFileExistsFilePathBeforeAfterPathResultProperties command, string path, Engine.AutomationEngineInstance engine)
        {
            command.StoreSpecifiedPathResultInUserVariable(path, nameof(command.v_AfterFilePathResult), "After Result Path", engine);
        }

        /// <summary>
        /// store before/after file path result in user variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="path"></param>
        /// <param name="parameterName"></param>
        /// <param name="parameterDescription"></param>
        /// <param name="engine"></param>
        private static void StoreSpecifiedPathResultInUserVariable(this IFileExistsFilePathBeforeAfterPathResultProperties command, string path, string parameterName, string parameterDescription, Engine.AutomationEngineInstance engine)
        {
            var variableName = command.ToScriptCommand().GetRawPropertyValueAsString(parameterName, parameterDescription);
            if (!string.IsNullOrEmpty(variableName))
            {
                command.StoreFilePathInUserVariable(path, parameterName, engine);
            }
        }
    }
}
