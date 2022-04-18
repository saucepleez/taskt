using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskt.Core.Automation.Commands
{
    internal class WindowNameControls
    {

        private static Func<string, string, bool> getWindowSearchMethod(string searchMethod)
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
                    break;
            }
            return ret;
        }

        public static IntPtr FindWindow(string windowName, string searchMethod)
        {
            var searchFunc = getWindowSearchMethod(searchMethod);

            if (windowName.StartsWith("Windows Explorer -"))
            {
                var windowLocationName = windowName.Split('-')[1].Trim();

                SHDocVw.ShellWindows shellWindows = new SHDocVw.ShellWindows();

                foreach (SHDocVw.InternetExplorer window in shellWindows)
                {
                    //if (window.LocationName.Contains(windowLocationName))
                    if (searchFunc(window.LocationName, windowName))
                    {
                        return new IntPtr(window.HWND);
                    }
                }

                return IntPtr.Zero;
            }
            else
            {
                //try to find exact window name
                IntPtr hWnd = User32.User32Functions.FindWindowNative(null, windowName);
                if (hWnd == IntPtr.Zero)
                {
                    //potentially wait for some additional initialization
                    System.Threading.Thread.Sleep(1000);
                    hWnd = User32.User32Functions.FindWindowNative(null, windowName);
                }

                //if exact window was not found, try partial match
                if (hWnd == IntPtr.Zero)
                {
                    var potentialWindow = System.Diagnostics.Process.GetProcesses().Where(prc => prc.MainWindowTitle.Contains(windowName)).FirstOrDefault();
                    if (potentialWindow != null)
                        hWnd = potentialWindow.MainWindowHandle;
                }
                //return hwnd
                return hWnd;
            }
        }
        public static List<IntPtr> FindWindows(string windowName, string searchMethod)
        {
            var searchFunc = getWindowSearchMethod(searchMethod);

            List<IntPtr> ret = new List<IntPtr>();
            if (windowName.Contains("Windows Explorer -"))
            {
                var windowLocationName = windowName.Split('-')[1].Trim();

                SHDocVw.ShellWindows shellWindows = new SHDocVw.ShellWindows();

                foreach (SHDocVw.InternetExplorer window in shellWindows)
                {
                    //if (window.LocationName.Contains(windowLocationName))
                    if (searchFunc(window.LocationName, windowName))
                    {
                        ret.Add((IntPtr)window.HWND);
                    }
                }
                return ret;
            }
            else
            {
                //try to find exact window name
                IntPtr hWnd = User32.User32Functions.FindWindowNative(null, windowName);

                if (hWnd == IntPtr.Zero)
                {
                    //potentially wait for some additional initialization
                    System.Threading.Thread.Sleep(1000);
                    hWnd = User32.User32Functions.FindWindowNative(null, windowName);
                    if (hWnd != IntPtr.Zero)
                    {
                        ret.Add(hWnd);
                    }
                }
                else
                {
                    ret.Add(hWnd);
                }
                //if exact window was not found, try partial match
                var potentialWindows = System.Diagnostics.Process.GetProcesses().Where(prc => prc.MainWindowTitle.Contains(windowName)).ToList();
                foreach (var potentialWindow in potentialWindows)
                {
                    ret.Add(potentialWindow.MainWindowHandle);
                }
                //return hwnd
                return ret;
            }
        }
    }
}
