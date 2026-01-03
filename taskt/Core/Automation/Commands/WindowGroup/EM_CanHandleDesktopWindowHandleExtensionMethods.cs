using System;
using System.Runtime.InteropServices;

namespace taskt.Core.Automation.Commands.WindowGroup
{
    public static class EM_CanHandleDesktopWindowHandleExtensionMethods
    {
        [DllImport("user32.dll", SetLastError = false)]
        private static extern IntPtr GetDesktopWindow();

        /// <summary>
        /// get Desktop Window Handle
        /// </summary>
        /// <returns></returns>
        public static IntPtr GetDesktopWindowHandle()
        {
            return GetDesktopWindow();
        }
    }
}
