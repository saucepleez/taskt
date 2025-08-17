using System;
using System.Runtime.InteropServices;

namespace taskt.Core.Automation.Commands
{
    public static class EM_WindowRECTPropertiesExtentionMethods
    {
        /// <summary>
        /// get window rect
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lpRect"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        /// <summary>
        /// get window rect
        /// </summary>
        /// <param name="whnd"></param>
        /// <returns></returns>
        public static RECT GetWindowRect(IntPtr whnd)
        {
            GetWindowRect(whnd, out RECT r);
            return r;
        }
    }
}
