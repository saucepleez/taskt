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
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using taskt.Core.Automation.Commands;

namespace taskt.Core.Automation.User32
{
    public static class User32Functions
    {
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        private static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
        public static void SetWindowPosition(IntPtr hWnd, int newXPosition, int newYPosition)
        {
            const short SWP_NOSIZE = 1;
            const short SWP_NOZORDER = 0X4;
            const int SWP_SHOWWINDOW = 0x0040;

            SetWindowPos(hWnd, 0, newXPosition, newYPosition, 0, 0, SWP_NOZORDER | SWP_NOSIZE | SWP_SHOWWINDOW);
        }

        [DllImport("user32.dll", EntryPoint = "GetWindowRect")]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
      
        public struct RECT
        {
            public int left, top, right, bottom;
        }

        private delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr lParam);     

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
                var diff = DateTime.Now - keyTime;
                keyTime = DateTime.Now;

                if (diff.Milliseconds < 50 && LastKey != null && LastKey == key)
                {
                    return;
                }
                else
                {
                    LastKey = key;
                }

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
                if ((generatedCommands.Count > 1) && (generatedCommands[generatedCommands.Count - 1] is EnterKeysCommand))
                {

                    var lastCreatedSendKeysCommand = (EnterKeysCommand)generatedCommands[generatedCommands.Count - 1];

                    if (lastCreatedSendKeysCommand.v_TextToSend.Contains("{ENTER}"))
                    {
                        //append this to a new command because you dont want text to input after user presses enter

                        //build a pause command to track pause since last command
                        BuildPauseCommand();

                        //build keyboard command
                        var keyboardCommand = new EnterKeysCommand
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
                    var keyboardCommand = new EnterKeysCommand
                    {
                        v_TextToSend = selectedKey,
                        v_WindowName = "Current Window"
                    };
                    generatedCommands.Add(keyboardCommand);
                }
            }

            public static DateTime keyTime { get; set; }
            public static Keys? LastKey { get; set; }

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

                var mouseMove = new Core.Automation.Commands.MoveMouseCommand
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
                var pauseCommand = new Core.Automation.Commands.PauseScriptCommand
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
                                GetModuleHandle(curModule.ModuleName), 0
                            );
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
