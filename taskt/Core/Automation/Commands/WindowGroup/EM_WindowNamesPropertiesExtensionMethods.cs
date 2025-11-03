using System;
using System.Collections.Generic;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{
    public static class EM_WindowNamesPropertiesExtensionMethods
    {

        /// <summary>
        /// store window name result to user variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="result"></param>
        /// <param name="engine"></param>
        public static void StoreWindowNameResultInUserVariable(this IWindowNamesProperties command, List<string> result, AutomationEngineInstance engine)
        {
            if (!string.IsNullOrEmpty(command.v_WindowNameResult))
            {
                result.StoreInUserVariable(engine, command.v_WindowNameResult);
            }
        }

        /// <summary>
        /// store window handle result in user variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="result"></param>
        /// <param name="engine"></param>
        public static void StoreWindowHandleResultInUserVariable(this IWindowNamesProperties command, List<string> result, AutomationEngineInstance engine)
        {
            if (!string.IsNullOrEmpty(command.v_WindowHandleResult))
            {
                result.StoreInUserVariable(engine, command.v_WindowHandleResult);
            }
        }

        /// <summary>
        /// store window name and handle in user variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="names"></param>
        /// <param name="handles"></param>
        /// <param name="engine"></param>
        public static void StoreWindowNameAndHandleResultInUserVariable(this IWindowNamesProperties command, List<string> names, List<string> handles, AutomationEngineInstance engine)
        {
            command.StoreWindowNameResultInUserVariable(names, engine);
            command.StoreWindowHandleResultInUserVariable(handles, engine);
        }

        /// <summary>
        /// convert match window to window name and window handle list
        /// </summary>
        /// <param name="windows"></param>
        /// <returns>(name, handle)</returns>
        private static (List<string>, List<string>) ConvertWindowNameAndWindowHandleList(List<(IntPtr, string)> windows)
        {
            var handles = new List<string>();
            var names = new List<string>();
            foreach ((var whnd, var name) in windows)
            {
                handles.Add(whnd.ToString());
                names.Add(name);
            }
            return (names, handles);
        }

        /// <summary>
        /// window names action
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        public static void WindowNamesAction(this IWindowNamesProperties command, AutomationEngineInstance engine, Action<List<(IntPtr, string)>> actionFunc, Action<Exception> errorFunc = null)
        {
            try
            {
                var wins = command.WaitForWindowNames(engine);

                actionFunc(wins);

                (var names, var handles) = ConvertWindowNameAndWindowHandleList(wins);
                command.StoreWindowNameAndHandleResultInUserVariable(names, handles, engine);
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
