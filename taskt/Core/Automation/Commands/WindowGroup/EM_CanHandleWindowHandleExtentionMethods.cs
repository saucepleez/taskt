using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace taskt.Core.Automation.Commands
{
    public class EM_CanHandleWindowHandleExtentionMethods
    {
        /// <summary>
        /// templorary window handles list
        /// </summary>
        private static List<IntPtr> windowHandle = null;

        /// <summary>
        /// check window handle exists
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool IsWindow(IntPtr hWnd);

        /// <summary>
        /// get window title length
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern int GetWindowTextLengthW(IntPtr hWnd);

        /// <summary>
        /// get window title
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="text"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowTextW(IntPtr hWnd, StringBuilder text, int count);

        /// <summary>
        /// check window is minimized
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);

        /// <summary>
        /// check window is maximized
        /// </summary>
        /// <param name="hWhnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool IsZoomed(IntPtr hWhnd);

        /// <summary>
        /// get active window handle
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// enum window delegate
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lparam"></param>
        /// <returns></returns>
        public delegate bool EnumWindowsDelegate(IntPtr hWnd, IntPtr lparam);

        /// <summary>
        /// enum all windows
        /// </summary>
        /// <param name="lpEnumFunc"></param>
        /// <param name="lparam"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int EnumWindows(EnumWindowsDelegate lpEnumFunc, IntPtr lparam);

        /// <summary>
        /// check window is visible
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        /// <summary>
        /// check window handle exists
        /// </summary>
        /// <param name="wHnd"></param>
        /// <returns></returns>
        public static bool CheckWindowHandleExists(IntPtr wHnd)
        {
            return IsWindow(wHnd);
        }

        /// <summary>
        /// check window is minimized
        /// </summary>
        /// <param name="wHnd"></param>
        /// <returns></returns>
        public static bool IsWindowMinimized(IntPtr wHnd)
        {
            return IsIconic(wHnd);
        }

        /// <summary>
        /// check window is maximized
        /// </summary>
        /// <param name="wHnd"></param>
        /// <returns></returns>
        public static bool IsWindowMaximized(IntPtr wHnd)
        {
            return IsZoomed(wHnd);
        }

        /// <summary>
        /// get window name
        /// </summary>
        /// <param name="wHnd"></param>
        /// <returns></returns>
        public static string GetWindowName(IntPtr wHnd)
        {
            int titleLengthA = GetWindowTextLengthW(wHnd);
            StringBuilder title = new StringBuilder(titleLengthA + 1);
            GetWindowTextW(wHnd, title, title.Capacity);
            return title.ToString();
        }

        /// <summary>
        /// get window handle
        /// </summary>
        /// <returns></returns>
        public static IntPtr GetActiveWindowHandle()
        {
            return GetForegroundWindow();
        }

        /// <summary>
        /// check text is current window handle keyword
        /// </summary>
        /// <param name="str"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static bool IsCurrentWindowHandleKeyword(string str, Engine.AutomationEngineInstance engine)
        {
            return (str == VariableNameControls.GetWrappedVariableName(Engine.SystemVariables.Window_CurrentWindowHandle.VariableName, engine));
        }

        /// <summary>
        /// enum windows
        /// </summary>
        /// <param name="wHnd"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private static bool EnumerateWindowHandle(IntPtr wHnd, IntPtr lParam)
        {
            if (IsWindowVisible(wHnd))
            {
                windowHandle.Add(wHnd);
            }
            return true;
        }

        /// <summary>
        /// get all window handles
        /// </summary>
        /// <returns></returns>
        public static List<IntPtr> GetAllWindowHandles()
        {
            windowHandle = new List<IntPtr>();

            EnumWindows(new EnumWindowsDelegate(EnumerateWindowHandle), IntPtr.Zero);

            var ret = new List<IntPtr>(windowHandle);
            windowHandle = null;
            return ret;
        }
    }
}
