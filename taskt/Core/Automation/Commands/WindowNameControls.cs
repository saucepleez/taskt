using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskt.Core.Automation.Commands
{
    internal class WindowNameControls
    {

        public static Func<string, string, bool> getWindowSearchMethod(string searchMethod)
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

        private static Dictionary<IntPtr, string> GetAllWindows()
        {
            return User32.User32Functions.GetWindowNames();
        }

        public static string GetCurrentWindowName()
        {
            return User32.User32Functions.GetActiveWindowTitle();
        }

        public static IntPtr GetCurrentWindowHandle()
        {
            return User32.User32Functions.GetActiveWindow();
        }

        //public static IntPtr FindWindow(string windowName, string searchMethod)
        //{
        //    var searchFunc = getWindowSearchMethod(searchMethod);

        //    if (windowName.StartsWith("Windows Explorer -"))
        //    {
        //        var windowLocationName = windowName.Split('-')[1].Trim();

        //        SHDocVw.ShellWindows shellWindows = new SHDocVw.ShellWindows();

        //        foreach (SHDocVw.InternetExplorer window in shellWindows)
        //        {
        //            //if (window.LocationName.Contains(windowLocationName))
        //            if (searchFunc(window.LocationName, windowName))
        //            {
        //                return new IntPtr(window.HWND);
        //            }
        //        }

        //        return IntPtr.Zero;
        //    }
        //    else
        //    {
        //        //try to find exact window name
        //        IntPtr hWnd = User32.User32Functions.FindWindowNative(null, windowName);
        //        if (hWnd == IntPtr.Zero)
        //        {
        //            //potentially wait for some additional initialization
        //            System.Threading.Thread.Sleep(1000);
        //            hWnd = User32.User32Functions.FindWindowNative(null, windowName);
        //        }

        //        //if exact window was not found, try partial match
        //        if (hWnd == IntPtr.Zero)
        //        {
        //            var potentialWindow = System.Diagnostics.Process.GetProcesses().Where(prc => prc.MainWindowTitle.Contains(windowName)).FirstOrDefault();
        //            if (potentialWindow != null)
        //                hWnd = potentialWindow.MainWindowHandle;
        //        }
        //        //return hwnd
        //        return hWnd;
        //    }
        //}
        public static IntPtr FindWindow(string windowName, string searchMethod, Automation.Engine.AutomationEngineInstance engine)
        {
            if (windowName == engine.engineSettings.CurrentWindowKeyword)
            {
                return User32.User32Functions.GetActiveWindow();
            }
            else
            {
                var windows = GetAllWindows();
                var method = getWindowSearchMethod(searchMethod);
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
        //public static List<IntPtr> FindWindows(string windowName, string searchMethod)
        //{
        //    var searchFunc = getWindowSearchMethod(searchMethod);

        //    List<IntPtr> ret = new List<IntPtr>();
        //    if (windowName.Contains("Windows Explorer -"))
        //    {
        //        var windowLocationName = windowName.Split('-')[1].Trim();

        //        SHDocVw.ShellWindows shellWindows = new SHDocVw.ShellWindows();

        //        foreach (SHDocVw.InternetExplorer window in shellWindows)
        //        {
        //            //if (window.LocationName.Contains(windowLocationName))
        //            if (searchFunc(window.LocationName, windowName))
        //            {
        //                ret.Add((IntPtr)window.HWND);
        //            }
        //        }
        //        return ret;
        //    }
        //    else
        //    {
        //        //try to find exact window name
        //        IntPtr hWnd = User32.User32Functions.FindWindowNative(null, windowName);

        //        if (hWnd == IntPtr.Zero)
        //        {
        //            //potentially wait for some additional initialization
        //            System.Threading.Thread.Sleep(1000);
        //            hWnd = User32.User32Functions.FindWindowNative(null, windowName);
        //            if (hWnd != IntPtr.Zero)
        //            {
        //                ret.Add(hWnd);
        //            }
        //        }
        //        else
        //        {
        //            ret.Add(hWnd);
        //        }
        //        //if exact window was not found, try partial match
        //        var potentialWindows = System.Diagnostics.Process.GetProcesses().Where(prc => prc.MainWindowTitle.Contains(windowName)).ToList();
        //        foreach (var potentialWindow in potentialWindows)
        //        {
        //            ret.Add(potentialWindow.MainWindowHandle);
        //        }
        //        //return hwnd
        //        return ret;
        //    }
        //}
        public static List<IntPtr> FindWindows(string windowName, string searchMethod, Engine.AutomationEngineInstance engine)
        {
            List<IntPtr> ret = new List<IntPtr>();

            if (windowName == engine.engineSettings.CurrentWindowKeyword)
            {
                //ret.Add(User32.User32Functions.GetActiveWindow());
                //return ret;
                windowName = GetCurrentWindowName();
            }
            //else
            //{
            //    var windows = GetAllWindows();
            //    var method = getWindowSearchMethod(searchMethod);
            //    foreach (var win in windows)
            //    {
            //        if (method(win.Value, windowName))
            //        {
            //            ret.Add(win.Key);
            //        }
            //    }
            //    if (ret.Count > 0)
            //    {
            //        return ret;
            //    }
            //    else
            //    {
            //        // not found
            //        throw new Exception("Window Name '" + windowName + "' not found");
            //    }
            //}
            var windows = GetAllWindows();
            var method = getWindowSearchMethod(searchMethod);
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
            List<string> ret = new List<string>();
            var windows = GetAllWindows();
            foreach(var win in windows)
            {
                ret.Add(win.Value);
            }
            return ret;
        }

        public static List<IntPtr> GetAllWindowHandles()
        {
            List<IntPtr> ret = new List<IntPtr>();
            var windows = GetAllWindows();
            foreach (var win in windows)
            {
                ret.Add(win.Key);
            }
            return ret;
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
            IntPtr hwnd = FindWindow(windowName, searchMethod, engine);
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
                var method = getWindowSearchMethod(searchMethod);
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
