using System;

namespace taskt.Core.Automation.Commands
{
    public static class EM_FolderExistsFolderPathBeforeAfterPathResultPropertiesExtentionMethods
    {
        /// <summary>
        /// general folder action. This method search target fodler before execute actionFunc, and try store Found folder Path after execute actionFunc.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <param name="actionFunc"></param>
        /// <param name="errorFunc"></param>
        public static void FolderAction(this IFolderExistsFolderPathBeforeAfterPathResultProperties command, Engine.AutomationEngineInstance engine, Func<string, string> actionFunc, Action<Exception> errorFunc = null)
        {
            command.FolderAction(engine, actionFunc,
                new Action<string, string>((before, after) =>
                {
                    command.StoreBeforeAfterFolderPathsResultInUserVariables(before, after, engine);
                }),
                errorFunc
            );
        }

        /// <summary>
        /// store before and after folder path in user variables
        /// </summary>
        /// <param name="command"></param>
        /// <param name="beforePath"></param>
        /// <param name="afterPath"></param>
        /// <param name="engine"></param>
        public static void StoreBeforeAfterFolderPathsResultInUserVariables(this IFolderExistsFolderPathBeforeAfterPathResultProperties command, string beforePath, string afterPath, Engine.AutomationEngineInstance engine)
        {
            command.StoreBeforeFolderPathResultInUserVariable(beforePath, engine);
            command.StoreAfterFolderPathResultInUserVariable(afterPath, engine);
        }

        /// <summary>
        /// store before folder path result in user variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="path"></param>
        /// <param name="engine"></param>
        public static void StoreBeforeFolderPathResultInUserVariable(this IFolderExistsFolderPathBeforeAfterPathResultProperties command, string path, Engine.AutomationEngineInstance engine)
        {
            command.StoreSpecifiedPathResultInUserVariable(path, nameof(command.v_BeforeFolderPathResult), "Before Result Path", engine);
        }

        /// <summary>
        /// store after folder path result in user variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="path"></param>
        /// <param name="engine"></param>
        public static void StoreAfterFolderPathResultInUserVariable(this IFolderExistsFolderPathBeforeAfterPathResultProperties command, string path, Engine.AutomationEngineInstance engine)
        {
            command.StoreSpecifiedPathResultInUserVariable(path, nameof(command.v_AfterFolderPathResult), "After Result Path", engine);
        }

        /// <summary>
        /// store before/after folder path in user varaiable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="path"></param>
        /// <param name="parameterName"></param>
        /// <param name="parameterDescription"></param>
        /// <param name="engine"></param>
        private static void StoreSpecifiedPathResultInUserVariable(this IFolderExistsFolderPathBeforeAfterPathResultProperties command, string path, string parameterName, string parameterDescription, Engine.AutomationEngineInstance engine)
        {
            var variableName = command.ToScriptCommand().GetRawPropertyValueAsString(parameterName, parameterDescription);
            if (!string.IsNullOrEmpty(variableName))
            {
                command.StoreFolderPathInUserVariable(path, parameterName, engine);
            }
        }
    }
}
