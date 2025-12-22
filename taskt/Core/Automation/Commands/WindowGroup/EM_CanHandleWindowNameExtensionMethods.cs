using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace taskt.Core.Automation.Commands
{
    public static class EM_CanHandleWindowNameExtensionMethods
    {
        /// <summary>
        /// templorary window handle name pair
        /// </summary>
        private static List<(IntPtr, string)> windowHandleNamePair = null;

        /// <summary>
        /// enum window delegate
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lparam"></param>
        /// <returns></returns>
        private delegate bool EnumWindowsDelegate(IntPtr hWnd, IntPtr lparam);

        /// <summary>
        /// enum all windows
        /// </summary>
        /// <param name="lpEnumFunc"></param>
        /// <param name="lparam"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern int EnumWindows(EnumWindowsDelegate lpEnumFunc, IntPtr lparam);

        /// <summary>
        /// check window is visible
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        /// <summary>
        /// enum windows
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private static bool EnumerateWindow(IntPtr hWnd, IntPtr lParam)
        {
            if (IsWindowVisible(hWnd))
            {
                var title = EM_CanHandleWindowHandleExtentionMethods.GetWindowName(hWnd);
                windowHandleNamePair.Add((hWnd, title));
            }
            return true;
        }

        /// <summary>
        /// get window names
        /// </summary>
        /// <returns></returns>
        public static List<(IntPtr, string)> GetAllWindowNamesAndHandles()
        {
            windowHandleNamePair = CreateEmptyWindowNameAndHandleList();

            EnumWindows(new EnumWindowsDelegate(EnumerateWindow), IntPtr.Zero);

            var ret = new List<(IntPtr, string)>(windowHandleNamePair);
            windowHandleNamePair = null;
            return ret;
        }

        /// <summary>
        /// get all window names
        /// </summary>
        /// <param name="useDistinct"></param>
        /// <returns></returns>
        public static List<string> GetAllWindowNames(bool useDistinct = true)
        {
            var t = GetAllWindowNamesAndHandles().Select(item => item.Item2);
            if (useDistinct)
            {
                t = t.Distinct();
            }
            return t.ToList();
        }

        /// <summary>
        /// get active window name
        /// </summary>
        /// <returns></returns>
        public static string GetActiveWindowName()
        {
            return EM_CanHandleWindowHandleExtentionMethods.GetWindowName(EM_CanHandleWindowHandleExtentionMethods.GetActiveWindowHandle());
        }

        /// <summary>
        /// create empty window name and handle list
        /// </summary>
        /// <returns></returns>
        public static List<(IntPtr, string)> CreateEmptyWindowNameAndHandleList()
        {
            return new List<(IntPtr, string)>();
        }

        /// <summary>
        /// check text is current window name keyword
        /// </summary>
        /// <param name="str"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static bool IsCurrentWindowNameKeyword(string str, Engine.AutomationEngineInstance engine)
        {
            return (str == VariableNameControls.GetWrappedVariableName(Engine.SystemVariables.Window_CurrentWindowName.VariableName, engine));
        }
    }
}
