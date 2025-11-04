using System;

namespace taskt.Core.Automation.Commands.WindowGroup
{
    public static class EM_FromWindowNameResultPropertiesExtensionMethods
    {
        /// <summary>
        /// store window handle in user variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="windowHandle"></param>
        /// <param name="engine"></param>
        public static void StoreWindowHandleInUserVariable(this IFromWindowNameResultsProperties command, IntPtr windowHandle, Engine.AutomationEngineInstance engine)
        {
            if (!string.IsNullOrEmpty(command.v_WindowHandleResult))
            {
                windowHandle.StoreInUserVariable(engine, command.v_WindowHandleResult);
            }
        }

        /// <summary>
        /// store window name and window handle results in user variables
        /// </summary>
        /// <param name="command"></param>
        /// <param name="windowName"></param>
        /// <param name="windowHandle"></param>
        /// <param name="engine"></param>
        public static void StoreWindowNameAndWindowHandleResultsInUserVariables(this IFromWindowNameResultsProperties command, string windowName, IntPtr windowHandle, Engine.AutomationEngineInstance engine)
        {
            command.StoreWindowNameResultInUserVariable(windowName, engine);
            command.StoreWindowHandleInUserVariable(windowHandle, engine);
        }

        /// <summary>
        /// check window name or window handle results is specified
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static bool IsWindowNameOrWindowHandleResultsSpecified(this IFromWindowNameResultsProperties command)
        {
            return (!string.IsNullOrEmpty(command.v_WindowNameResult)) || (!string.IsNullOrEmpty(command.v_WindowHandleResult));
        }
    }
}
