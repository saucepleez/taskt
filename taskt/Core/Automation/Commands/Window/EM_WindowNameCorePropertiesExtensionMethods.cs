using System;
using System.Collections.Generic;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{
    public static class EM_WindowNameCorePropertiesExtensionMethods
    {
        /// <summary>
        /// get window name check method
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns>Func(a,b) is a.Contains(b)</returns>
        /// <exception cref="Exception"></exception>
        public static Func<string, string, bool> GetWindowNameCheckMethod(this IWindowNameCoreProperties command, AutomationEngineInstance engine)
        {
            switch (((ScriptCommand)command).ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_CompareMethod), engine))
            {
                case "contains":
                    return new Func<string, string, bool>((a, b) => { return a.Contains(b); });

                case "starts with":
                    return new Func<string, string, bool>((a, b) => { return a.StartsWith(b); });

                case "ends with":
                    return new Func<string, string, bool>((a, b) => { return a.EndsWith(b); });

                case "exact match":
                    return new Func<string, string, bool>((a, b) => { return (a == b); });
                default:
                    throw new Exception($"Error. Strange method. Value: '{command.v_CompareMethod}'");
            }
        }

        /// <summary>
        /// expand value or user variable as window name
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static string ExpandValueOrUserVariableAsWindowName(this IWindowNameCoreProperties command, AutomationEngineInstance engine)
        {
            if (command.v_WindowName == VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentWindowName.VariableName, engine))
            {
                return EM_CanHandleWindowNameExtensionMethods.GetActiveWindowName();
            }
            else
            {
                return ((ScriptCommand)command).ExpandValueOrUserVariable(nameof(command.v_WindowName), "Window Name", engine);
            }
        }

        /// <summary>
        /// expand value or user variable as Window Wait Time
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static int ExpandValueOrUserVariableAsWindowWaitTime(this IWindowNameCoreProperties command, AutomationEngineInstance engine)
        {
            var wait = ((ScriptCommand)command).ExpandValueOrUserVariableAsInteger(nameof(command.v_WaitTimeForWindow), engine);
            if (wait >= 0)
            {
                return wait;
            }
            else
            {
                throw new Exception($"Strange Window Wait Time. Value: '{command.v_WaitTimeForWindow}', Expand Value: '{wait}'");
            }
        }

        /// <summary>
        /// expand value or user variable as window name, window wait time, and check method
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns>Func(a,b) is like a.Contains(b)</returns>
        public static (string, int, Func<string, string, bool>) ExpandValueOrUserVariableAsWindowNameWindowWaitTimeAndMethod(this IWindowNameCoreProperties command, AutomationEngineInstance engine)
        {
            return (command.ExpandValueOrUserVariableAsWindowName(engine), command.ExpandValueOrUserVariableAsWindowWaitTime(engine),
                        command.GetWindowNameCheckMethod(engine));
        }

        /// <summary>
        /// store window name result to user variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="result"></param>
        /// <param name="engine"></param>
        public static void StoreWindowNameResultInUserVariable(this IWindowNameCoreProperties command, List<string> result, AutomationEngineInstance engine)
        {
            if (!string.IsNullOrEmpty(command.v_NameResult))
            {
                result.StoreInUserVariable(engine, command.v_NameResult);
            }
        }

        /// <summary>
        /// store window handle result in user variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="result"></param>
        /// <param name="engine"></param>
        public static void StoreWindowHandleResultInUserVariable(this IWindowNameCoreProperties command, List<string> result, AutomationEngineInstance engine)
        {
            if (!string.IsNullOrEmpty(command.v_HandleResult))
            {
                result.StoreInUserVariable(engine, command.v_HandleResult);
            }
        }

        /// <summary>
        /// store window name and handle in user variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="names"></param>
        /// <param name="handles"></param>
        /// <param name="engine"></param>
        public static void StoreWindowNameAndHandleResultInUserVariable(this IWindowNameCoreProperties command, List<string> names, List<string> handles, AutomationEngineInstance engine)
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
            foreach((var whnd, var name) in windows)
            {
                handles.Add(whnd.ToString());
                names.Add(name);
            }
            return (names, handles);
        }

        /// <summary>
        /// wait for window names
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<(IntPtr, string)> WaitForWindowNames(this IWindowNameCoreProperties command, AutomationEngineInstance engine)
        {
            (var windowName, var waitTime, var checkFunc) = command.ExpandValueOrUserVariableAsWindowNameWindowWaitTimeAndMethod(engine);

            var ret = WaitControls.WaitProcess(waitTime, "Window Name", new Func<(bool, object)>(() =>
            {
                var allWindows = EM_CanHandleWindowNameExtensionMethods.GetAllWindowNamesAndHandles();
                var matchWindows = new List<(IntPtr, string)>();
                foreach ((var whnd, var name) in allWindows)
                {
                    if (checkFunc(name, windowName))
                    {
                        matchWindows.Add((whnd, name));
                    }
                }
                if (matchWindows.Count > 0)
                {
                    return (true, matchWindows);
                }
                else
                {
                    return (false, null);
                }
            }), engine);

            if (ret is List<(IntPtr, string)> wins)
            {
                if (wins.Count > 0)
                {
                    return wins;
                }
                else
                {
                    throw new Exception($"Target Window not Found. Value: '{command.v_WindowName}', Expand Value: '{windowName}'");
                }
            }
            else
            {
                throw new Exception($"Target Window not Found. Value: '{command.v_WindowName}', Expand Value: '{windowName}'");
            }
        }

        /// <summary>
        /// window names action
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        public static void WindowNamesAction(this IWindowNameCoreProperties command, AutomationEngineInstance engine, Action<List<(IntPtr, string)>> actionFunc, Action<Exception> errorFunc = null)
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
