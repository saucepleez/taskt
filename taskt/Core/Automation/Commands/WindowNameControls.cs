using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for window name methods
    /// </summary>
    internal static class WindowNameControls
    {
        /// <summary>
        /// get window name compare method (Func)
        /// </summary>
        /// <param name="compareType"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Func<string, string, bool> GetWindowNameCompareMethod(string compareType)
        {
            Func<string, string, bool> ret;
            switch (compareType.ToLower())
            {
                case "starts with":
                    ret = (a, b) => a.StartsWith(b);
                    break;
                case "ends with":
                    ret = (a, b) => a.EndsWith(b);
                    break;
                case "exact match":
                    ret = (a, b) => (a == b);
                    break;
                case "contains":
                    ret = (a, b) => a.Contains(b);
                    break;
                default:
                    throw new Exception("Search method " + compareType + " is not support.");
            }
            return ret;
        }

        /// <summary>
        /// Get Window Match Function
        /// </summary>
        /// <param name="matchType"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static Func<List<IntPtr>, List<IntPtr>> GetWindowMatchMethod(string matchType, int index)
        {
            Func<List<IntPtr>, List<IntPtr>> ret;
            switch(matchType.ToLower())
            {
                case "first":
                    ret = (lst) =>
                    {
                        return (lst.Count > 0 ? new List<IntPtr>() { lst[0] } : throw new Exception("No Matched Windows exists."));
                    };
                    break;
                case "last":
                    ret = (lst) =>
                    {
                        return (lst.Count > 0 ? new List<IntPtr>() { lst[lst.Count - 1] } : throw new Exception("No Matched Windows exists."));
                    };
                    break;
                case "all":
                    ret = (lst) =>
                    {
                        return (lst.Count > 0 ? new List<IntPtr>(lst) : throw new Exception("No Matched Windows exists."));
                    };
                    break;
                case "index":
                    ret = (lst) =>
                    {
                        var count = lst.Count;
                        if (count == 0)
                        {
                            throw new Exception("No Matched Windows exists.");
                        }
                        if (index < 0)
                        {
                            index += count;
                        }
                        if (index >= 0 && index < count)
                        {
                            return new List<IntPtr>() { lst[index] };
                        }
                        else
                        {
                            throw new Exception("No Item Exists. Index: " + index);
                        }
                    };
                    break;
                default:
                    throw new Exception("Match type " + matchType + " is not support.");
            }
            return ret;
        }

        /// <summary>
        /// get window name search method (Func)
        /// </summary>
        /// <param name="window"></param>
        /// <param name="searchMethod"></param>
        /// <param name="matchType"></param>
        /// <param name="index"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        private static Func<List<IntPtr>> GetWindowSearchMethod(string window, string searchMethod, string matchType, int index, Engine.AutomationEngineInstance engine)
        {
            if (window == engine.engineSettings.CurrentWindowKeyword)
            {
                // current window
                return new Func<List<IntPtr>>(() =>
                {
                    return new List<IntPtr>() { GetCurrentWindowHandle() };
                });
            }
            else if (window == engine.engineSettings.AllWindowsKeyword)
            {
                // all windows & match-type
                return new Func<List<IntPtr>>(() =>
                {
                    var matchedWindows = GetAllWindowHandles();
                    var matchFunc = GetWindowMatchMethod(matchType, index);
                    return matchFunc(matchedWindows);
                });
            }
            else
            {
                // matched windows
                return new Func<List<IntPtr>>(() => {
                    var matchedWindows = new List<IntPtr>();
                    var wins = GetAllWindows();
                    var searchFunc = GetWindowNameCompareMethod(searchMethod);
                    foreach (var key in wins.Keys)
                    {
                        if (searchFunc(wins[key], window))
                        {
                            matchedWindows.Add(key);
                        }
                    }
                    var matchFunc = GetWindowMatchMethod(matchType, index);
                    return matchFunc(matchedWindows);
                });
            }
        }


        /// <summary>
        /// get all window names and handles
        /// </summary>
        /// <returns></returns>
        private static Dictionary<IntPtr, string> GetAllWindows()
        {
            return User32.User32Functions.GetWindowNames();
        }

        /// <summary>
        /// get current window name
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentWindowName()
        {
            return User32.User32Functions.GetActiveWindowTitle();
        }

        /// <summary>
        /// get current window handle
        /// </summary>
        /// <returns></returns>
        public static IntPtr GetCurrentWindowHandle()
        {
            return User32.User32Functions.GetActiveWindow();
        }

        public static IntPtr FindWindowHandle(string windowName, string searchMethod, Automation.Engine.AutomationEngineInstance engine)
        {
            if (windowName == engine.engineSettings.CurrentWindowKeyword)
            {
                return User32.User32Functions.GetActiveWindow();
            }
            else
            {
                var windows = GetAllWindows();
                var method = GetWindowNameCompareMethod(searchMethod);
                foreach(var win in windows)
                {
                    if (method(win.Value, windowName))
                    {
                        return win.Key;
                    }
                }
            }
            // not found
            throw new Exception("Window Name '" + windowName + "' not found");
        }
        
        public static List<IntPtr> FindWindowsHandles(string windowName, string searchMethod, Engine.AutomationEngineInstance engine)
        {
            List<IntPtr> ret = new List<IntPtr>();

            if (windowName == engine.engineSettings.CurrentWindowKeyword)
            {
                windowName = GetCurrentWindowName();
            }

            var windows = GetAllWindows();
            var method = GetWindowNameCompareMethod(searchMethod);
            foreach (var win in windows)
            {
                if (method(win.Value, windowName))
                {
                    ret.Add(win.Key);
                }
            }
            if (ret.Count > 0)
            {
                return ret;
            }
            else
            {
                // not found
                throw new Exception("Window Name '" + windowName + "' not found");
            }
        }

        public static List<string> GetAllWindowTitles()
        {
            return new List<string>(GetAllWindows().Values);
        }

        public static List<IntPtr> GetAllWindowHandles()
        {
            return new List<IntPtr>(GetAllWindows().Keys);
        }

        public static void ActivateWindow(IntPtr handle)
        {
            if (User32.User32Functions.IsIconic(handle))
            {
                User32.User32Functions.SetWindowState(handle, User32.User32Functions.WindowState.SW_SHOWNORMAL);
            }
            User32.User32Functions.SetForegroundWindow(handle);
        }

        public static void ActivateWindow(string windowName, string searchMethod, Automation.Engine.AutomationEngineInstance engine)
        {
            IntPtr hwnd = FindWindowHandle(windowName, searchMethod, engine);
            ActivateWindow(hwnd);
        }

        public static string GetMatchedWindowName(string windowName, string searchMethod, Automation.Engine.AutomationEngineInstance engine)
        {
            if (windowName == engine.engineSettings.CurrentWindowKeyword)
            {
                return GetCurrentWindowName();
            }
            else
            {
                var windows = GetAllWindows();
                var method = GetWindowNameCompareMethod(searchMethod);
                foreach (var win in windows)
                {
                    if (method(win.Value, windowName))
                    {
                        return win.Value;
                    }
                }
            }
            // not found
            throw new Exception("Window Name '" + windowName + "' not found");
        }

        /// <summary>
        /// expand variable as Window Name
        /// </summary>
        /// <param name="value"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        private static string ConvertToUserVariableAsWindowName(this string value, Engine.AutomationEngineInstance engine)
        {
            if ((value == engine.engineSettings.CurrentWindowKeyword) || (value == engine.engineSettings.AllWindowsKeyword))
            {
                return value;
            }
            else
            {
                return value.ConvertToUserVariable(engine);
            }
        }

        /// <summary>
        /// expand specified property value as Window Name 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="windowName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        private static string ConvertToUserVariableAsWindowName(this ScriptCommand command, string windowName, Engine.AutomationEngineInstance engine)
        {
            var prop = command.GetProperty(windowName);
            var value = prop.GetValue(command)?.ToString() ?? "";
            return value.ConvertToUserVariableAsWindowName(engine);
        }

        /// <summary>
        /// search & wait window name. this method use argument values, DON'T convert variable.
        /// </summary>
        /// <param name="window"></param>
        /// <param name="searchMethod"></param>
        /// <param name="matchType"></param>
        /// <param name="index"></param>
        /// <param name="waitTime"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static List<IntPtr> FindWindows(string window, string searchMethod, string matchType, int index, int waitTime, Engine.AutomationEngineInstance engine)
        {
            var searchFunc = GetWindowSearchMethod(window, searchMethod, matchType, index, engine);

            var waitFunc = new Func<(bool, object)>(() =>
            {
                try
                {
                    var ret = searchFunc();
                    if (ret.Count > 0)
                    {
                        return (true, ret);
                    }
                    else
                    {
                        return (false, null);
                    }
                }
                catch
                {
                    return (false, null);
                }
            });

            var obj = WaitControls.WaitProcess(waitTime, "Window", waitFunc, engine);

            if (obj is List<IntPtr> lst)
            {
                return lst;
            }
            else
            {
                throw new Exception("Strange Value returned in FindWindows. Type: " + obj.GetType().FullName);
            }
        }

        /// <summary>
        /// search & wait window name. this method use argument values, convert variable.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="windowName"></param>
        /// <param name="compareMethodName"></param>
        /// <param name="matchTypeName"></param>
        /// <param name="indexName"></param>
        /// <param name="waitName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static List<IntPtr> FindWindows(ScriptCommand command, string windowName, string compareMethodName, string matchTypeName, string indexName, string waitName, Engine.AutomationEngineInstance engine)
        {
            var window = command.ConvertToUserVariableAsWindowName(windowName, engine);
            var compareMethod = command.GetUISelectionValue(compareMethodName, engine);
            var matchType = command.GetUISelectionValue(matchTypeName, engine);
            var index = command.ConvertToUserVariableAsInteger(indexName, engine);
            var waitTime = command.ConvertToUserVariableAsInteger(waitName, engine);

            return FindWindows(window, compareMethod, matchType, index, waitTime, engine);
        }


        public static void UpdateWindowTitleCombobox(System.Windows.Forms.ComboBox cmb)
        {
            string currentText = cmb.Text;

            cmb.BeginUpdate();
            cmb.Items.Clear();

            var winList = GetAllWindowTitles();

            foreach(var title in winList)
            {
                cmb.Items.Add(title);
            }

            cmb.EndUpdate();

            if (winList.Contains(currentText))
            {
                cmb.Text = currentText;
            }
        }
    }
}
