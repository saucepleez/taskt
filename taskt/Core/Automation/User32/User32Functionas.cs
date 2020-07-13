//Copyright (c) 2019 Jason Bayldon
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Commands;

namespace taskt.Core.Automation.User32
{
    public static class User32Functions
    {
        [DllImport("user32.dll")]
        public static extern bool LockWorkStation();

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

        public static bool WindowsLogOff()
        {
            return ExitWindowsEx(0, 0);
        }

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindowNative(string className, string windowName);
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
                    System.Threading.Thread.Sleep(1000);
                    hWnd = FindWindowNative(null, windowName);
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

        public static List<IntPtr> FindTargetWindows(string windowName)
        {
            //create list of hwnds to target
            List<IntPtr> targetWindows = new List<IntPtr>();
            if (windowName == "All Windows")
            {
                //target each available window
                foreach (var prc in System.Diagnostics.Process.GetProcesses())
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
                    hwnd = User32Functions.GetActiveWindow();
                }
                else
                {
                    //find window by name
                    hwnd = User32Functions.FindWindow(windowName);
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
        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("User32.dll", EntryPoint = "SetForegroundWindow")]
        private static extern IntPtr SetForegroundWindowNative(IntPtr hWnd);
        public static void SetForegroundWindow(IntPtr hWnd)
        {
            SetForegroundWindowNative(hWnd);
        }

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        public static void SetWindowState(IntPtr hWnd, WindowState windowState)
        {
            ShowWindow(hWnd, (int)windowState);
        }

        public enum WindowState
        {
            [Description("Minimizes a window, even if the thread that owns the window is not responding. This flag should only be used when minimizing windows from a different thread.")]
            SW_FORCEMINIMIZE = 11,
            [Description("Hides the window and activates another window.")]
            SW_HIDE = 0,
            [Description("Maximizes the specified window.")]
            SW_MAXIMIZE = 3,
            [Description("Minimizes the specified window and activates the next top-level window in the Z order.")]
            SW_MINIMIZE = 6,
            [Description("Activates and displays the window. If the window is minimized or maximized, the system restores it to its original size and position. An application should specify this flag when restoring a minimized window.")]
            SW_RESTORE = 9,
            [Description("Activates the window and displays it in its current size and position.")]
            SW_SHOW = 5,
            [Description("Sets the show state based on the SW_ value specified in the STARTUPINFO structure passed to the CreateProcess function by the program that started the application.")]
            SW_SHOWDEFAULT = 10,
            [Description("Activates the window and displays it as a maximized window.")]
            SW_SHOWMAXIMIZED = 3,
            [Description("Activates the window and displays it as a minimized window.")]
            SW_SHOWMINIMIZED = 2,
            [Description("Displays the window as a minimized window. This value is similar to SW_SHOWMINIMIZED, except the window is not activated.")]
            SW_SHOWMINNOACTIVE = 7,
            [Description("Displays the window in its current size and position. This value is similar to SW_SHOW, except that the window is not activated.")]
            SW_SHOWNA = 8,
            [Description("Displays a window in its most recent size and position. This value is similar to SW_SHOWNORMAL, except that the window is not activated.")]
            SW_SHOWNOACTIVATE = 4,
            [Description("Activates and displays a window. If the window is minimized or maximized, the system restores it to its original size and position. An application should specify this flag when displaying the window for the first time.")]
            SW_SHOWNORMAL = 1,
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        private static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
        public static void SetWindowPosition(IntPtr hWnd, int newXPosition, int newYPosition)
        {
            const short SWP_NOSIZE = 1;
            const short SWP_NOZORDER = 0X4;
            const int SWP_SHOWWINDOW = 0x0040;

            SetWindowPos(hWnd, 0, newXPosition, newYPosition, 0, 0, SWP_NOZORDER | SWP_NOSIZE | SWP_SHOWWINDOW);
        }
        public static void SetWindowSize(IntPtr hWnd, int newXSize, int newYSize)
        {

            const short SWP_NOZORDER = 0X4;
            const int SWP_SHOWWINDOW = 0x0040;

            GetWindowRect(hWnd, out RECT windowRect);

            SetWindowPos(hWnd, 0, windowRect.left, windowRect.top, newXSize, newYSize, SWP_NOZORDER | SWP_SHOWWINDOW);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        public static void CloseWindow(IntPtr hWnd)
        {
            const UInt32 WM_CLOSE = 0x0010;
            SendMessage(hWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        }

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);
        public static void SetCursorPosition(int newXPosition, int newYPosition)
        {
            SetCursorPos(newXPosition, newYPosition);
        }

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
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

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        public static void SendMouseClick(string clickType, int xMousePosition, int yMousePosition)
        {
            switch (clickType)
            {
                case "Double Left Click":
                    mouse_event((int)MouseEvents.MOUSEEVENTF_LEFTDOWN, xMousePosition, yMousePosition, 0, 0);
                    mouse_event((int)MouseEvents.MOUSEEVENTF_LEFTUP, xMousePosition, yMousePosition, 0, 0);
                    mouse_event((int)MouseEvents.MOUSEEVENTF_LEFTDOWN, xMousePosition, yMousePosition, 0, 0);
                    mouse_event((int)MouseEvents.MOUSEEVENTF_LEFTUP, xMousePosition, yMousePosition, 0, 0);
                    break;

                case "Left Click":
                    mouse_event((int)MouseEvents.MOUSEEVENTF_LEFTDOWN, xMousePosition, yMousePosition, 0, 0);
                    mouse_event((int)MouseEvents.MOUSEEVENTF_LEFTUP, xMousePosition, yMousePosition, 0, 0);
                    break;

                case "Right Click":
                    mouse_event((int)MouseEvents.MOUSEEVENTF_RIGHTDOWN, xMousePosition, yMousePosition, 0, 0);
                    mouse_event((int)MouseEvents.MOUSEEVENTF_RIGHTUP, xMousePosition, yMousePosition, 0, 0);
                    break;

                case "Middle Click":
                    mouse_event((int)MouseEvents.MOUSEEVENTF_MIDDLEDOWN, xMousePosition, yMousePosition, 0, 0);
                    mouse_event((int)MouseEvents.MOUSEEVENTF_MIDDLEUP, xMousePosition, yMousePosition, 0, 0);
                    break;

                case "Left Down":
                    mouse_event((int)MouseEvents.MOUSEEVENTF_LEFTDOWN, xMousePosition, yMousePosition, 0, 0);
                    break;

                case "Right Down":
                    mouse_event((int)MouseEvents.MOUSEEVENTF_RIGHTDOWN, xMousePosition, yMousePosition, 0, 0);
                    break;

                case "Middle Down":
                    mouse_event((int)MouseEvents.MOUSEEVENTF_MIDDLEDOWN, xMousePosition, yMousePosition, 0, 0);
                    break;

                case "Left Up":
                    mouse_event((int)MouseEvents.MOUSEEVENTF_LEFTUP, xMousePosition, yMousePosition, 0, 0);
                    break;

                case "Right Up":
                    mouse_event((int)MouseEvents.MOUSEEVENTF_RIGHTUP, xMousePosition, yMousePosition, 0, 0);
                    break;

                case "Middle Up":
                    mouse_event((int)MouseEvents.MOUSEEVENTF_MIDDLEUP, xMousePosition, yMousePosition, 0, 0);
                    break;

                default:
                    break;
            }
        }

        enum MouseEvents
        {
            MOUSEEVENTF_LEFTDOWN = 0x02,
            MOUSEEVENTF_LEFTUP = 0x04,
            MOUSEEVENTF_RIGHTDOWN = 0x08,
            MOUSEEVENTF_RIGHTUP = 0x10,
            MOUSEEVENTF_MIDDLEDOWN = 0x20,
            MOUSEEVENTF_MIDDLEUP = 0x40
        }

        public static void KeyDownKeyUp(Keys[] keys)
        {
            foreach (var key in keys)
            {
                KeyDown(key);
            }

            foreach (var key in keys)
            {
                KeyUp(key);
            }
        }


        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
        private const int KEYEVENTF_EXTENDEDKEY = 1;
        private const int KEYEVENTF_KEYUP = 2;
        public static void KeyDown(Keys vKey)
        {
            keybd_event((byte)vKey, 0, KEYEVENTF_EXTENDEDKEY, 0);
        }

        public static void KeyUp(Keys vKey)
        {
            keybd_event((byte)vKey, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }

        [DllImport("user32.dll", EntryPoint = "GetWindowRect")]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        public static RECT GetWindowPosition(IntPtr hWnd)
        {
            RECT clientArea = new RECT();
            GetWindowRect(hWnd, out clientArea);
            return clientArea;
        }
        public struct RECT
        {
            public int left, top, right, bottom;
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetClipboardData(uint uFormat);
        [DllImport("user32.dll")]
        internal static extern bool SetClipboardData(uint uFormat, IntPtr data);
        [DllImport("user32.dll")]
        static extern bool IsClipboardFormatAvailable(uint format);
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool OpenClipboard(IntPtr hWndNewOwner);
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool CloseClipboard();
        [DllImport("kernel32.dll")]
        static extern IntPtr GlobalLock(IntPtr hMem);
        [DllImport("kernel32.dll")]
        static extern bool GlobalUnlock(IntPtr hMem);

        const uint CF_UNICODETEXT = 13;
        public static void SetClipboardText(string textToSet)
        {
            OpenClipboard(IntPtr.Zero);
            var ptr = Marshal.StringToHGlobalUni(textToSet);
            SetClipboardData(13, ptr);
            CloseClipboard();
        }
        public static string GetClipboardText()
        {
            if (!IsClipboardFormatAvailable(CF_UNICODETEXT))
                return null;
            if (!OpenClipboard(IntPtr.Zero))
                return null;

            string data = null;
            var hGlobal = GetClipboardData(CF_UNICODETEXT);
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

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        public static IntPtr GetActiveWindow()
        {
            return GetForegroundWindow();
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetDesktopWindow();

        private delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr lParam);

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
                SetWindowState(hWnd, WindowState.SW_RESTORE);
                SetForegroundWindow(hWnd);
            }

            var rect = new RECT();

            //sleep to allow repaint
            System.Threading.Thread.Sleep(500);

            GetWindowRect(hWnd, out rect);
            var bounds = new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
            var screenshot = new Bitmap(bounds.Width, bounds.Height);

            using (var graphics = Graphics.FromImage(screenshot))
            {
                graphics.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
            }

            return screenshot;
        }

        public class GlobalHook
        {
            private const int WH_KEYBOARD_LL = 13;
            private const int WM_KEYDOWN = 0x0100;
            private static readonly LowLevelKeyboardProc _kbProc = KeyboardHookEvent;
            private static readonly LowLevelMouseProc _mouseProc = MouseHookEvent;
            private static readonly LowLevelMouseProc _mouseLeftUpProc = MouseHookForLeftClickUpEvent;
            private static IntPtr _keyboardHookID = IntPtr.Zero;
            private static IntPtr _mouseHookID = IntPtr.Zero;
            private static Stopwatch sw;

            private static bool performMouseClickCapture;
            private static bool groupMouseMovesIntoSequence;
            private static bool performMouseMoveCapture;
            private static bool performKeyboardCapture;
            private static bool performWindowCapture;
            private static bool activateWindowTopLeft;
            private static bool trackActivatedWindowSizes;
            private static bool trackWindowOpenLocations;
            private static int msResolution;
            public static string stopHookKey;
            public static bool stopOnClick;
            private static Stopwatch lastMouseMove;

            public static List<Core.Automation.Commands.ScriptCommand> generatedCommands;

            public static event EventHandler HookStopped = delegate { };

            public static void StartEngineCancellationHook(Keys keyName)
            {
                stopHookKey = keyName.ToString();
                //set hook for engine cancellation
                _keyboardHookID = SetKeyboardHook(_kbProc);
            }
            public static void StartElementCaptureHook(bool stopOnFirstClick)
            {
                stopOnClick = stopOnFirstClick;
                //set hook for engine cancellation
                _mouseHookID = SetMouseHook(_mouseLeftUpProc);
            }

            public static void StartScreenRecordingHook(bool captureClick, bool captureMouse, bool groupMouseMoves, bool captureKeyboard, bool captureWindow, bool activateTopLeft, bool trackActivatedWindowSize, bool trackWindowsOpenLocation, int eventResolution, string stopHookHotKey)
            {
                //create new list for commands generated
                generatedCommands = new List<ScriptCommand>();

                //setup variables
                performMouseClickCapture = captureClick;
                performMouseMoveCapture = captureMouse;
                performKeyboardCapture = captureKeyboard;
                groupMouseMovesIntoSequence = groupMouseMoves;
                performWindowCapture = captureWindow;
                activateWindowTopLeft = activateTopLeft;
                trackActivatedWindowSizes = trackActivatedWindowSize;
                trackWindowOpenLocations = trackWindowsOpenLocation;
                msResolution = eventResolution;
                stopHookKey = stopHookHotKey;
                //start hook
                _mouseHookID = SetMouseHook(_mouseProc);
                _keyboardHookID = SetKeyboardHook(_kbProc);

                //if user decided to capture window events
                if (performWindowCapture)
                {
                    _WinEventHookHandler = new SystemEventHandler(BuildWindowCommand);
                    _WinEventHook = SetWinEventHook(SystemEvents.EVENT_MIN, SystemEvents.EVENT_MAX,IntPtr.Zero, _WinEventHookHandler, 0, 0, 0);
                }
              

                //start stopwatch for timing all event occurences
                sw = new Stopwatch();
                sw.Start();

                //stopwatch for tracking mouse moves specifically
                lastMouseMove = new Stopwatch();
                lastMouseMove.Start();

            }

            //hook end
            public static void StopHook()
            {
                UnhookWindowsHookEx(_keyboardHookID);
                UnhookWindowsHookEx(_mouseHookID);

                if (performWindowCapture)
                {
                    UnhookWinEvent(_WinEventHook);
                }
               
                //BuildCommentCommand();

                HookStopped(null, new EventArgs());

            }


            //mouse and keyboard hook event triggers
            private static IntPtr KeyboardHookEvent(int nCode, IntPtr wParam, IntPtr lParam)
           {

                if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)

                {

                    int vkCode = Marshal.ReadInt32(lParam);

                    BuildKeyboardCommand((Keys)vkCode);


                }

                return CallNextHookEx(_keyboardHookID, nCode, wParam, lParam);

            }
            public static event EventHandler<MouseCoordinateEventArgs> MouseEvent;
            private static IntPtr MouseHookForLeftClickUpEvent(int nCode, IntPtr wParam, IntPtr lParam)

            {

                if (nCode >= 0)
                {
                    var message = (MouseMessages)wParam;

                    if (message == MouseMessages.WM_LBUTTONDOWN)
                    {
                        if (stopOnClick)
                        {
                            UnhookWindowsHookEx(_mouseHookID);
                        }

                        MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                        System.Windows.Point point = new System.Windows.Point(hookStruct.pt.x, hookStruct.pt.y);
                        MouseEvent?.Invoke(null, new MouseCoordinateEventArgs() { MouseCoordinates = point });
                    }

                }

                return CallNextHookEx(_mouseHookID, nCode, wParam, lParam);

            }

            private static IntPtr MouseHookEvent(int nCode, IntPtr wParam, IntPtr lParam)

            {

                if (nCode >= 0)
                {
                    BuildMouseCommand(lParam, (MouseMessages)wParam);
                }

                return CallNextHookEx(_mouseHookID, nCode, wParam, lParam);

            }

            //build keyboard command
            private static void BuildKeyboardCommand(Keys key)
            {
                bool toUpperCase = false;

                //determine if casing is needed
                if (IsKeyDown(Keys.ShiftKey) && IsKeyToggled(Keys.Capital))
                {
                    toUpperCase = false;
                }
                else if (!IsKeyDown(Keys.ShiftKey) && IsKeyToggled(Keys.Capital))
                {
                    toUpperCase = true;
                }
                else if (IsKeyDown(Keys.ShiftKey) && !IsKeyToggled(Keys.Capital))
                {
                    toUpperCase = true;
                }
                else if (!IsKeyDown(Keys.ShiftKey) && !IsKeyToggled(Keys.Capital))
                {
                    toUpperCase = false;
                }


                var buf = new StringBuilder(256);
                var keyboardState = new byte[256];


                if (toUpperCase)
                {
                    keyboardState[(int)Keys.ShiftKey] = 0xff;
                }

                ToUnicode((uint)key, 0, keyboardState, buf, 256, 0);

                var selectedKey = buf.ToString();



                if ((selectedKey == "") || (selectedKey == "\r"))
                {
                    selectedKey = key.ToString();
                }


                //translate key press to sendkeys identifier
                if (selectedKey == stopHookKey)
                {
                    //STOP HOOK
                    StopHook();
                    return;
                }
                else if (selectedKey == "Return")
                {
                    selectedKey = "ENTER";
                }
                else if (selectedKey == "Space")
                {
                    selectedKey = " ";
                }
                else if (selectedKey == "OemPeriod")
                {
                    selectedKey = ".";
                }
                else if (selectedKey == "Oemcomma")
                {
                    selectedKey = ",";
                }
                else if (selectedKey == "OemQuestion")
                {
                    selectedKey = "?";
                }
                else if (selectedKey.Contains("ShiftKey"))
                {
                    return;
                }


                if (!performKeyboardCapture)
                {
                    return;
                }


                //add braces
                if (selectedKey.Length > 1)
                {
                    selectedKey = "{" + selectedKey + "}";
                }

                //generate sendkeys together
                if ((generatedCommands.Count > 1) && (generatedCommands[generatedCommands.Count - 1] is SendKeysCommand))
                {

                    var lastCreatedSendKeysCommand = (SendKeysCommand)generatedCommands[generatedCommands.Count - 1];

                    if (lastCreatedSendKeysCommand.v_TextToSend.Contains("{ENTER}"))
                    {
                        //append this to a new command because you dont want text to input after user presses enter

                        //build a pause command to track pause since last command
                        BuildPauseCommand();

                        //build keyboard command
                        var keyboardCommand = new SendKeysCommand
                        {
                            v_TextToSend = selectedKey,
                            v_WindowName = "Current Window"
                        };
                        generatedCommands.Add(keyboardCommand);
                    }
                    else
                    {
                        //append chars to previously created command
                        //this makes editing easier for the user because only 1 command is issued rather than multiples
                        var previouslyInputChars = lastCreatedSendKeysCommand.v_TextToSend;
                        lastCreatedSendKeysCommand.v_TextToSend = previouslyInputChars + selectedKey;
                    }


                }
                else
                {
                    //build a pause command to track pause since last command
                    BuildPauseCommand();

                    //build keyboard command
                    var keyboardCommand = new SendKeysCommand
                    {
                        v_TextToSend = selectedKey,
                        v_WindowName = "Current Window"
                    };
                    generatedCommands.Add(keyboardCommand);
                }





            }
            //build mouse command
            private static void BuildMouseCommand(IntPtr lParam, MouseMessages mouseMessage)
            {

                string mouseEventClickType = string.Empty;
                switch (mouseMessage)
                {
                    case MouseMessages.WM_LBUTTONDOWN:
                        mouseEventClickType = "Left Down";
                        break;
                    case MouseMessages.WM_LBUTTONUP:
                        mouseEventClickType = "Left Up";
                        break;
                    case MouseMessages.WM_MOUSEMOVE:
                        mouseEventClickType = "None";

                       

                        if (lastMouseMove.ElapsedMilliseconds >= msResolution)
                        {
                            lastMouseMove.Restart();
                        }
                        else
                        {
                            return;
                        }

         
                        break;
                    case MouseMessages.WM_RBUTTONDOWN:
                        mouseEventClickType = "Right Down";
                        break;
                    case MouseMessages.WM_RBUTTONUP:
                        mouseEventClickType = "Right Up";
                        break;
                    default:
                        return;
                }

                ////return if non matching event
                //if (mouseEventClickType == string.Empty)
                //    return;


                   
                //return if we do not want to capture mouse moves
                if ((!performMouseMoveCapture) && (mouseEventClickType == "None"))
                {
                    return;
                }

                //return if we do not want to capture mouse clicks
                if ((!performMouseClickCapture) && (mouseEventClickType != "None"))
                {
                    return;
                }

                //build a pause command to track pause since last command
                BuildPauseCommand();


                //define new mouse command
                MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));

               


                var mouseMove = new Core.Automation.Commands.SendMouseMoveCommand
                {
                    v_XMousePosition = hookStruct.pt.x.ToString(),
                    v_YMousePosition = hookStruct.pt.y.ToString(),
                    v_MouseClick = mouseEventClickType
            };

                if (mouseEventClickType != "None")
                {
                    IntPtr winHandle = WindowFromPoint(hookStruct.pt);

                    int length = GetWindowText(winHandle, _Buffer, _Buffer.Capacity);
                    var windowName = _Buffer.ToString();

                    mouseMove.v_Comment = "Clicked On Window: " + windowName;


                }

                generatedCommands.Add(mouseMove);



            }
            //build window command
            private static void BuildWindowCommand(IntPtr hWinEventHook, SystemEvents @event, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
            {


                switch (@event)
                {
                    case SystemEvents.EVENT_MIN:
                        return;
                    case SystemEvents.EVENT_MAX:
                        return;
                    case SystemEvents.EVENT_SYSTEM_FOREGROUND:
                        break;
                    case SystemEvents.MINIMIZE_END:
                        return;
                    case SystemEvents.MINIMIZE_START:
                        return;
                    default:
                        return;
                }


                int length = GetWindowText(hwnd, _Buffer, _Buffer.Capacity);
                var windowName = _Buffer.ToString();



                //bypass screen recorder and Cortana (Win10) which throws errors
                if ((windowName == "Screen Recorder") || (windowName == "Cortana"))
                {
                    return;
                }

           

                if (length > 0)
                {
                    //wait additional for window to initialize
                    //System.Threading.Thread.Sleep(250);
                    windowName = _Buffer.ToString();
                 
                    Automation.Commands.ActivateWindowCommand activateWindowCommand = new ActivateWindowCommand
                    {
                        v_WindowName = windowName,
                        v_Comment = "Generated by Screen Recorder @ " + DateTime.Now.ToString()
                    };

                    generatedCommands.Add(activateWindowCommand);

                    //detect if tracking window open location or activate windows to top left
                    if (trackWindowOpenLocations)
                    {
                        GetWindowRect(hwnd, out RECT windowRect);


                        Automation.Commands.MoveWindowCommand moveWindowCommand = new MoveWindowCommand
                        {
                            v_WindowName = windowName,
                            v_XWindowPosition = windowRect.left.ToString(),
                            v_YWindowPosition = windowRect.top.ToString(),
                            v_Comment = "Generated by Screen Recorder @ " + DateTime.Now.ToString()

                        };

                        generatedCommands.Add(moveWindowCommand);

                    }
                   else if (activateWindowTopLeft)
                    {
                        //generate command to set window position
                        Automation.Commands.MoveWindowCommand moveWindowCommand = new MoveWindowCommand
                        {
                            v_WindowName = windowName,
                            v_XWindowPosition = "0",
                            v_YWindowPosition = "0",
                            v_Comment = "Generated by Screen Recorder @ " + DateTime.Now.ToString()

                        };

                        SetWindowPosition(hwnd, 0, 0);

                        generatedCommands.Add(moveWindowCommand);
                    }

                    //if tracking window sizes is set
                    if (trackActivatedWindowSizes)
                    {
                        //create rectangle from hwnd
                        GetWindowRect(hwnd, out RECT windowRect);

                        //do math to get height, etc
                        var width = windowRect.right - windowRect.left;
                        var height = windowRect.bottom - windowRect.top;

                        //generate command to set window position
                        Automation.Commands.ResizeWindowCommand reszWindowCommand = new ResizeWindowCommand
                        {
                            v_WindowName = windowName,
                            v_XWindowSize = width.ToString(),
                            v_YWindowSize = height.ToString(),
                            v_Comment = "Generated by Screen Recorder @ " + DateTime.Now.ToString()

                        };
                        

                        //add to list
                        generatedCommands.Add(reszWindowCommand);


                    }


                }

            }
            //build pause command
            private static void BuildPauseCommand()
            {

                if (sw.ElapsedMilliseconds < 1)
                {
                    return;
                }

                sw.Stop();
                var pauseTime = sw.ElapsedMilliseconds;
                var pauseCommand = new Core.Automation.Commands.PauseCommand
                {
                    v_PauseLength = pauseTime.ToString()
                };
                generatedCommands.Add(pauseCommand);
                sw.Restart();
            }




            private static IntPtr SetKeyboardHook(LowLevelKeyboardProc proc)

            {

                using (Process curProcess = Process.GetCurrentProcess())

                using (ProcessModule curModule = curProcess.MainModule)

                {

                    return SetWindowsHookEx(WH_KEYBOARD_LL, proc,

                        GetModuleHandle(curModule.ModuleName), 0);

                }

            }
            private static IntPtr SetMouseHook(LowLevelMouseProc proc)

            {

                using (Process curProcess = Process.GetCurrentProcess())

                using (ProcessModule curModule = curProcess.MainModule)

                {

                    return SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(curModule.ModuleName), 0);

                }

            }
            private static IntPtr _WinEventHook;
            private static SystemEventHandler _WinEventHookHandler;
            private static StringBuilder _Buffer = new StringBuilder(512);


            #region User32 Keyboard Mouse
            private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
            private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]

            private static extern bool UnhookWindowsHookEx(IntPtr hhk);
            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]

            private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]

            private static extern IntPtr GetModuleHandle(string lpModuleName);

            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            private static extern short GetKeyState(int keyCode);

            [DllImport("user32.dll")]
            static extern IntPtr WindowFromPoint(POINT Point);

            [DllImport("user32.dll")]
            static extern IntPtr ChildWindowFromPoint(IntPtr hWndParent, POINT Point);

            [DllImport("user32.dll", CharSet = CharSet.Unicode)]
            public static extern int ToUnicode(uint virtualKeyCode,uint scanCode, byte[] keyboardState, StringBuilder receivingBuffer, int bufferSize, uint flags);


            //enums and structs
            private const int WH_MOUSE_LL = 14;
            private enum MouseMessages
            {
                WM_LBUTTONDOWN = 0x0201,
                WM_LBUTTONUP = 0x0202,
                WM_MOUSEMOVE = 0x0200,
                WM_MOUSEWHEEL = 0x020A,
                WM_RBUTTONDOWN = 0x0204,
                WM_RBUTTONUP = 0x0205
            }
            [StructLayout(LayoutKind.Sequential)]
            private struct POINT
            {
                public int x;
                public int y;
            }
            [StructLayout(LayoutKind.Sequential)]
            private struct MSLLHOOKSTRUCT
            {
                public POINT pt;
                public uint mouseData;
                public uint flags;
                public uint time;
                public IntPtr dwExtraInfo;
            }
            [Flags]
            private enum KeyStates
            {
                None = 0,
                Down = 1,
                Toggled = 2
            }
            private static KeyStates GetKeyState(Keys key)
            {
                KeyStates state = KeyStates.None;

                short retVal = GetKeyState((int)key);

                //If the high-order bit is 1, the key is down
                //otherwise, it is up.
                if ((retVal & 0x8000) == 0x8000)
                    state |= KeyStates.Down;

                //If the low-order bit is 1, the key is toggled.
                if ((retVal & 1) == 1)
                    state |= KeyStates.Toggled;

                return state;
            }



            //helper checks
            public static bool IsKeyDown(Keys key)
            {
                return KeyStates.Down == (GetKeyState(key) & KeyStates.Down);
            }
            public static bool IsKeyToggled(Keys key)
            {
                return KeyStates.Toggled == (GetKeyState(key) & KeyStates.Toggled);
            }
            #endregion

            #region User32 Window 

            enum SystemEvents
            {
                EVENT_MIN = 0x00000001,       //MIN
                EVENT_MAX = 0x7FFFFFFF,          //MAX
                EVENT_SYSTEM_FOREGROUND = 0x3,  //The foreground window has changed. The system sends this event even if the foreground window has changed to another window in the same thread. Server applications never send this event.
                MINIMIZE_END = 0x0017, //A window object is about to be restored. This event is sent by the system, never by servers.
                MINIMIZE_START = 0x0016 //A window object is about to be minimized. This event is sent by the system, never by servers.
            }

            [DllImport("user32.dll")]
            static extern IntPtr SetWinEventHook(SystemEvents eventMin, SystemEvents eventMax, IntPtr hmodWinEventProc, SystemEventHandler lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            static extern bool UnhookWinEvent(IntPtr hWinEventHook);

            [DllImport("user32.dll", CharSet = CharSet.Unicode)]
            static extern int GetWindowText(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

            delegate void SystemEventHandler(IntPtr hWinEventHook, SystemEvents @event, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

            #endregion


        }

      
    }
    public class WindowHandleInfo
    {
        private delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr lParam);

        private IntPtr _MainHandle;

        public WindowHandleInfo(IntPtr handle)
        {
            this._MainHandle = handle;
        }

        public List<IntPtr> GetAllChildHandles()
        {
            List<IntPtr> childHandles = new List<IntPtr>();

            GCHandle gcChildhandlesList = GCHandle.Alloc(childHandles);
            IntPtr pointerChildHandlesList = GCHandle.ToIntPtr(gcChildhandlesList);

            try
            {
                EnumWindowProc childProc = new EnumWindowProc(EnumWindow);
                EnumChildWindows(this._MainHandle, childProc, pointerChildHandlesList);
            }
            finally
            {
                gcChildhandlesList.Free();
            }

            return childHandles;
        }

        private bool EnumWindow(IntPtr hWnd, IntPtr lParam)
        {
            GCHandle gcChildhandlesList = GCHandle.FromIntPtr(lParam);

            if (gcChildhandlesList == null || gcChildhandlesList.Target == null)
            {
                return false;
            }

            List<IntPtr> childHandles = gcChildhandlesList.Target as List<IntPtr>;
            childHandles.Add(hWnd);

            return true;
        }
    }
    public class MouseCoordinateEventArgs : EventArgs
    {
        public System.Windows.Point MouseCoordinates { get; set; }
    }
}
