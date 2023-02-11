using System;
using System.Collections.Generic;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for window name methods
    /// </summary>
    internal static class WindowNameControls
    {
        /// <summary>
        /// get window name search method (Func)
        /// </summary>
        /// <param name="searchMethod"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Func<string, string, bool> GetWindowSearchMethod(string searchMethod)
        {
            Func<string, string, bool> ret;
            switch (searchMethod.ToLower())
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
                    throw new Exception("Search method " + searchMethod + " is not support.");
            }
            return ret;
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
                var method = GetWindowSearchMethod(searchMethod);
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
            var method = GetWindowSearchMethod(searchMethod);
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
                var method = GetWindowSearchMethod(searchMethod);
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
