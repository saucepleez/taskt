using System;
using System.IO;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for IFileExistsFilePathProperties extension methods
    /// </summary>
    public static class EM_FileExistsFilePathPropertiesExtentionMethods
    {
        /// <summary>
        /// expand value or User variable as Wait Time for File
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static int ExpandValueOrUserVariableAsWaitTimeForFile(this IFileExistsFilePathProperties command, Engine.AutomationEngineInstance engine)
        {
            return command.ToScriptCommand().ExpandValueOrUserVariableAsInteger(nameof(command.v_WaitTimeForFile), "Wait Time For File", engine);
        }

        /// <summary>
        /// Expand Value or User Variable as File Path and Wait Time for File
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static (string, int) ExpandValueOrUserVariableAsFilePathAndWaitTimeForFile(this IFileExistsFilePathProperties command, Engine.AutomationEngineInstance engine)
        {
            var f = command.ExpandValueOrUserVariableAsFilePath(engine);
            var w = command.ExpandValueOrUserVariableAsWaitTimeForFile(engine);
            return (f, w);
        }

        /// <summary>
        /// Wait For File
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns>file path</returns>
        /// <exception cref="Exception"></exception>
        public static string WaitForFile(this IFileExistsFilePathProperties command, Engine.AutomationEngineInstance engine)
        {
            (var path, var waitTime) = command.ExpandValueOrUserVariableAsFilePathAndWaitTimeForFile(engine);

            var ret = WaitControls.WaitProcess(waitTime, "File Path", new Func<(bool, object)>(() =>
            {
                if (EM_CanHandleFilePathExtentionMethods.IsURL(path))
                {
                    // if path is URL, don't check existance
                    return (true, path);
                }
                else
                {
                    if (File.Exists(path))
                    {
                        return (true, path);
                    }
                    else
                    {
                        return (false, null);
                    }
                }
            }), engine);

            if (ret is string returnPath)
            {
                return returnPath;
            }
            else
            {
                throw new Exception($"Strange Value returned in WaitForFile. Type: {ret.GetType().FullName}");
            }
        }

        /// <summary>
        /// general file action.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <param name="actionFunc">process for file</param>
        /// <param name="postActionFunc">action after process (before-path, after-path)</param>
        /// <param name="errorFunc">action when error</param>
        public static void FileAction(this IFileExistsFilePathProperties command, Engine.AutomationEngineInstance engine, Func<string, string> actionFunc, Action<string, string> postActionFunc = null, Action<Exception> errorFunc = null)
        {
            try
            {
                var beforePath = command.WaitForFile(engine);

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
