using System.Runtime.InteropServices;

namespace taskt.Core.Automation.Commands
{
    static internal class SystemControls
    {
        [DllImport("user32.dll")]
        private static extern bool LockWorkStation();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

        public static void UserLock()
        {
            LockWorkStation();
        }

        public static bool WindowsLogOff()
        {
            return ExitWindowsEx(0, 0);
        }
    }
}
