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
        /// move or resize window
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="hWndInsertAfter"></param>
        /// <param name="x"></param>
        /// <param name="Y"></param>
        /// <param name="cx"></param>
        /// <param name="cy"></param>
        /// <param name="wFlags"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, uint wFlags);

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
