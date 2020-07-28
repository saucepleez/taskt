using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using taskt.Core.Enums;

namespace taskt.Core.User32
{
    public static class User32Functions
    {
        private const int _keyEventFExtendedKey = 1;
        private const int _keyEventFKeyUp = 2;
        private const uint _cfUnicodeText = 13;

        [DllImport("user32.dll")]
        public static extern bool LockWorkStation();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindowNative(string className, string windowName);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter,
            string lpszClass, string lpszWindow);

        [DllImport("User32.dll", EntryPoint = "SetForegroundWindow")]
        private static extern IntPtr SetForegroundWindowNative(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        private static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter,
            int x, int Y,
            int cx, int cy,
            int wFlags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        [DllImport("user32.dll", EntryPoint = "GetWindowRect")]
        public static extern bool GetWindowRect(IntPtr hWnd, out Rect lpRect);

        [DllImport("user32.dll")]
        private static extern IntPtr GetClipboardData(uint uFormat);

        [DllImport("user32.dll")]
        private static extern bool SetClipboardData(uint uFormat, IntPtr data);

        [DllImport("user32.dll")]
        private static extern bool IsClipboardFormatAvailable(uint format);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool CloseClipboard();

        [DllImport("kernel32.dll")]
        private static extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport("kernel32.dll")]
        private static extern bool GlobalUnlock(IntPtr hMem);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetDesktopWindow();

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr lParam);

        private enum _mouseEvents
        {
            MouseEventFLeftDown = 0x02,
            MouseEventFLeftUp = 0x04,
            MouseEventFRightDown = 0x08,
            MouseEventFRightUp = 0x10,
            MouseEventFMiddleDown = 0x20,
            MouseEventFMiddleUp = 0x40
        }

        private delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);

        public static bool WindowsLogOff()
        {
            return ExitWindowsEx(0, 0);
        }

        public static IntPtr FindWindow(string windowName)
        {
            if (windowName.Contains("Windows Explorer -"))
            {
                var windowLocationName = windowName.Split('-')[1].Trim();

                SHDocVw.ShellWindows shellWindows = new SHDocVw.ShellWindows();

                foreach (SHDocVw.InternetExplorer window in shellWindows)
                {
                    if (window.LocationName.Contains(windowLocationName))
                    {
                        return new IntPtr(window.HWND);
                    }
                }

                return IntPtr.Zero;
            }
            else
            {
                //try to find exact window name
                IntPtr hWnd = FindWindowNative(null, windowName);

                if (hWnd == IntPtr.Zero)
                {
                    //potentially wait for some additional initialization
                    Thread.Sleep(1000);
                    hWnd = FindWindowNative(null, windowName);
                }

                //if exact window was not found, try partial match
                if (hWnd == IntPtr.Zero)
                {
                    var potentialWindow = Process.GetProcesses().Where(
                        prc => prc.MainWindowTitle.Contains(windowName)
                        ).FirstOrDefault();

                    if (potentialWindow != null)
                        hWnd = potentialWindow.MainWindowHandle;
                }

                //return hwnd
                return hWnd;
            }
        }

        public static List<IntPtr> FindTargetWindows(string windowName)
        {
            //create list of hwnds to target
            List<IntPtr> targetWindows = new List<IntPtr>();
            if (windowName == "All Windows")
            {
                //target each available window
                foreach (var prc in Process.GetProcesses())
                {
                    targetWindows.Add(prc.MainWindowHandle);
                }
            }
            else
            {
                //target current or specific window
                IntPtr hwnd;
                if (windowName == "Current Window")
                {
                    //get active window
                    hwnd = GetActiveWindow();
                }
                else
                {
                    //find window by name
                    hwnd = FindWindow(windowName);
                }

                //check if hwnd was found
                if (hwnd == IntPtr.Zero)
                {
                    //throw
                    throw new Exception("Window not found");
                }
                else
                {
                    //add to list
                    targetWindows.Add(hwnd);
                }

            }

            return targetWindows;
        }

        public static void SetForegroundWindow(IntPtr hWnd)
        {
            SetForegroundWindowNative(hWnd);
        }

        public static void SetWindowState(IntPtr hWnd, WindowState windowState)
        {
            ShowWindow(hWnd, (int)windowState);
        }

        public static void SetWindowPosition(IntPtr hWnd, int newXPosition, int newYPosition)
        {
            const short SwpNoSize = 1;
            const short SwpNozOrder = 0X4;
            const int SwpShowWindow = 0x0040;

            SetWindowPos(hWnd, 0, newXPosition, newYPosition, 0, 0, SwpNozOrder | SwpNoSize | SwpShowWindow);
        }

        public static void SetWindowSize(IntPtr hWnd, int newXSize, int newYSize)
        {

            const short SwpNozOrder = 0X4;
            const int SwpShowWindow = 0x0040;

            GetWindowRect(hWnd, out Rect windowRect);

            SetWindowPos(hWnd, 0, windowRect.left, windowRect.top, newXSize, newYSize, SwpNozOrder | SwpShowWindow);
        }

        public static void CloseWindow(IntPtr hWnd)
        {
            const UInt32 WmClose = 0x0010;
            SendMessage(hWnd, WmClose, IntPtr.Zero, IntPtr.Zero);
        }

        public static void SetCursorPosition(int newXPosition, int newYPosition)
        {
            SetCursorPos(newXPosition, newYPosition);
        }

        public static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return "";
        }

        public static void SendMouseClick(string clickType, int xMousePosition, int yMousePosition)
        {
            switch (clickType)
            {
                case "Double Left Click":
                    mouse_event((int)_mouseEvents.MouseEventFLeftDown, xMousePosition, yMousePosition, 0, 0);
                    mouse_event((int)_mouseEvents.MouseEventFLeftUp, xMousePosition, yMousePosition, 0, 0);
                    mouse_event((int)_mouseEvents.MouseEventFLeftDown, xMousePosition, yMousePosition, 0, 0);
                    mouse_event((int)_mouseEvents.MouseEventFLeftUp, xMousePosition, yMousePosition, 0, 0);
                    break;

                case "Left Click":
                    mouse_event((int)_mouseEvents.MouseEventFLeftDown, xMousePosition, yMousePosition, 0, 0);
                    mouse_event((int)_mouseEvents.MouseEventFLeftUp, xMousePosition, yMousePosition, 0, 0);
                    break;

                case "Right Click":
                    mouse_event((int)_mouseEvents.MouseEventFRightDown, xMousePosition, yMousePosition, 0, 0);
                    mouse_event((int)_mouseEvents.MouseEventFRightUp, xMousePosition, yMousePosition, 0, 0);
                    break;

                case "Middle Click":
                    mouse_event((int)_mouseEvents.MouseEventFMiddleDown, xMousePosition, yMousePosition, 0, 0);
                    mouse_event((int)_mouseEvents.MouseEventFMiddleUp, xMousePosition, yMousePosition, 0, 0);
                    break;

                case "Left Down":
                    mouse_event((int)_mouseEvents.MouseEventFLeftDown, xMousePosition, yMousePosition, 0, 0);
                    break;

                case "Right Down":
                    mouse_event((int)_mouseEvents.MouseEventFRightDown, xMousePosition, yMousePosition, 0, 0);
                    break;

                case "Middle Down":
                    mouse_event((int)_mouseEvents.MouseEventFMiddleDown, xMousePosition, yMousePosition, 0, 0);
                    break;

                case "Left Up":
                    mouse_event((int)_mouseEvents.MouseEventFLeftUp, xMousePosition, yMousePosition, 0, 0);
                    break;

                case "Right Up":
                    mouse_event((int)_mouseEvents.MouseEventFRightUp, xMousePosition, yMousePosition, 0, 0);
                    break;

                case "Middle Up":
                    mouse_event((int)_mouseEvents.MouseEventFMiddleUp, xMousePosition, yMousePosition, 0, 0);
                    break;

                default:
                    break;
            }
        }

        public static void KeyDown(Keys vKey)
        {
            keybd_event((byte)vKey, 0, _keyEventFExtendedKey, 0);
        }

        public static void KeyUp(Keys vKey)
        {
            keybd_event((byte)vKey, 0, _keyEventFExtendedKey | _keyEventFKeyUp, 0);
        }

        public static Rect GetWindowPosition(IntPtr hWnd)
        {
            Rect clientArea = new Rect();
            GetWindowRect(hWnd, out clientArea);
            return clientArea;
        }

        public static void SetClipboardText(string textToSet)
        {
            OpenClipboard(IntPtr.Zero);
            var ptr = Marshal.StringToHGlobalUni(textToSet);
            SetClipboardData(13, ptr);
            CloseClipboard();
        }

        public static string GetClipboardText()
        {
            if (!IsClipboardFormatAvailable(_cfUnicodeText))
                return null;
            if (!OpenClipboard(IntPtr.Zero))
                return null;

            string data = null;
            var hGlobal = GetClipboardData(_cfUnicodeText);
            if (hGlobal != IntPtr.Zero)
            {
                var lpwcstr = GlobalLock(hGlobal);
                if (lpwcstr != IntPtr.Zero)
                {
                    data = Marshal.PtrToStringUni(lpwcstr);
                    GlobalUnlock(lpwcstr);
                }
            }
            CloseClipboard();

            return data;
        }

        private static IntPtr GetActiveWindow()
        {
            return GetForegroundWindow();
        }

        public static Bitmap CaptureWindow(string windowName)
        {
            IntPtr hWnd;
            if (windowName == "Desktop")
            {
                hWnd = GetDesktopWindow();
            }
            else
            {
                hWnd = FindWindow(windowName);
                SetWindowState(hWnd, WindowState.SwRestore);
                SetForegroundWindow(hWnd);
            }

            var rect = new Rect();

            //sleep to allow repaint
            Thread.Sleep(500);

            GetWindowRect(hWnd, out rect);
            var bounds = new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
            var screenshot = new Bitmap(bounds.Width, bounds.Height);

            using (var graphics = Graphics.FromImage(screenshot))
            {
                graphics.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
            }

            return screenshot;
        }
    }
}
