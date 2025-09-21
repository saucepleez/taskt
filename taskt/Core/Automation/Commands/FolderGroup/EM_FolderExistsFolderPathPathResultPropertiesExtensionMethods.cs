using System;

namespace taskt.Core.Automation.Commands
{
    public static class EM_FolderExistsFolderPathPathResultPropertiesExtensionMethods
    {
        /// <summary>
        /// folder action
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <param name="actionFunc">return process folder path</param>
        /// <param name="errorFunc"></param>
        public static void FolderAction(this IFolderExistsFolderPathPathResultProperties command, Engine.AutomationEngineInstance engine, Func<string, string> actionFunc, Action<Exception> errorFunc = null)
        {
            command.FolderAction(engine, actionFunc,
                new Action<string, string>((before, after) =>
                {
                    command.StoreResultFolderPathInUserVariable(after, engine);
                }),
                errorFunc
            );
        }

        /// <summary>
        /// store reuslt folder path in user variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="path"></param>
        /// <param name="engine"></param>
        public static void StoreResultFolderPathInUserVariable(this IFolderExistsFolderPathPathResultProperties command, string path, Engine.AutomationEngineInstance engine)
        {
            var variableName = command.ToScriptCommand().GetRawPropertyValueAsString(nameof(command.v_ResultPath), "Result Path");
            if (!string.IsNullOrEmpty(variableName))
            {
                command.StoreFolderPathInUserVariable(path, nameof(command.v_ResultPath), engine);
            }
        }
    }
}
