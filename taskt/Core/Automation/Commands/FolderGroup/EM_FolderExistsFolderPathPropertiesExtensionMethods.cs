using System;
using System.IO;

namespace taskt.Core.Automation.Commands
{
    public static class EM_FolderExistsFolderPathPropertiesExtensionMethods
    {
        /// <summary>
        /// expand value or user variable as wait time for folder
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static int ExpandValueOrUserVariableAsWaitTimeForFolder(this IFolderExistsFolderPathProperties command, Engine.AutomationEngineInstance engine)
        {
            return command.ToScriptCommand().ExpandValueOrUserVariableAsInteger(nameof(command.v_WaitTimeForFolder), "Wait Time For Folder", engine);
        }

        /// <summary>
        /// Expand value or user variable as Folder Path and Wait Time for folder
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static (string, int) ExpandValueOrUserVariableAsFolderPathAndWaitTimeForFolder(this IFolderExistsFolderPathProperties command, Engine.AutomationEngineInstance engine)
        {
            var f = command.ExpandValueOrUserVariableAsFolderPath(engine);
            var w = command.ExpandValueOrUserVariableAsWaitTimeForFolder(engine);
            return (f, w);
        }

        /// <summary>
        /// wait for folder
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string WaitForFolder(this IFolderExistsFolderPathProperties command, Engine.AutomationEngineInstance engine)
        {
            (var path, var waitTime) = command.ExpandValueOrUserVariableAsFolderPathAndWaitTimeForFolder(engine);

            var ret = WaitControls.WaitProcess(waitTime, "Folder Path", new Func<(bool, object)>(() =>
            {
                if (Directory.Exists(path))
                {
                    return (true, path);
                }
                else
                {
                    return (false, null);
                }
            }), engine);

            if (ret is string returnPath)
            {
                return returnPath;
            }
            else
            {
                throw new Exception($"Strange Value returned in WaitForFolder. Type: {ret.GetType().FullName}");
            }
        }

        /// <summary>
        /// general folder action
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <param name="actionFunc"></param>
        /// <param name="postActionFunc">action after process (before-path, after-path)</param>
        /// <param name="errorFunc"></param>
        public static void FolderAction(this IFolderExistsFolderPathProperties command, Engine.AutomationEngineInstance engine, Func<string, string> actionFunc, Action<string, string> postActionFunc = null, Action<Exception> errorFunc = null)
        {
            try
            {
                var beforePath = command.WaitForFolder(engine);

                var afterPath = actionFunc(beforePath);

                postActionFunc?.Invoke(beforePath, afterPath);
            }
            catch (Exception ex)
            {
                if (errorFunc != null)
                {
                    errorFunc(ex);
                }
                else
                {
                    throw ex;
                }
            }
        }
    }
}
