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
            // all window system variable
            if (command.v_WindowName == VariableNameControls.GetWrappedVariableName(SystemVariables.Window_AllWindows.VariableName, engine))
            {
                return ("", command.ExpandValueOrUserVariableAsWindowWaitTime(engine),
                        new Func<string, string, bool>((a, b) => true));
            }
            else
            {
                return (command.ExpandValueOrUserVariableAsWindowName(engine), command.ExpandValueOrUserVariableAsWindowWaitTime(engine),
                            command.GetWindowNameCheckMethod(engine));
            }
        }

        /// <summary>
        /// check specified current window name keyword
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static bool IsCurrentWindowNameKeyword(this IWindowNameCoreProperties command, AutomationEngineInstance engine)
        {
            return EM_CanHandleWindowNameExtensionMethods.IsCurrentWindowNameKeyword(command.v_WindowName, engine);
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
    }
}
