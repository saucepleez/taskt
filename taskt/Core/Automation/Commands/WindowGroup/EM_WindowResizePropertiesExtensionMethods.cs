using System;

namespace taskt.Core.Automation.Commands
{
    public static class EM_WindowResizePropertiesExtensionMethods
    {

        /// <summary>
        /// resize window
        /// </summary>
        /// <param name="whnd"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void ResizeWindow(IntPtr whnd, int width, int height)
        {
            uint flag = 0x0046; // 0x0002 | 0x0004 | 0x0040;
            EM_WindowRECTPropertiesExtentionMethods.SetWindowPos(whnd, 0, 0, 0, width, height, flag);
        }
    }
}
