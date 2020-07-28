using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using taskt.Core.Command;
using taskt.Commands;
using taskt.Core.User32;
namespace taskt.Utilities
{
    public class GlobalHook
    {
        private const int _whKeyboardLl = 13;
        private const int _wmKeyDown = 0x0100;
        private static readonly LowLevelKeyboardProc _kbProc = KeyboardHookEvent;
        private static readonly LowLevelMouseProc _mouseProc = MouseHookEvent;
        private static readonly LowLevelMouseProc _mouseLeftUpProc = MouseHookForLeftClickUpEvent;
        private static IntPtr _keyboardHookID = IntPtr.Zero;
        private static IntPtr _mouseHookID = IntPtr.Zero;
        private static Stopwatch _stopWatch;
        private static Stopwatch _lastMouseMove;

        private static bool _performMouseClickCapture;
        private static bool _groupMouseMovesIntoSequence;
        private static bool _performMouseMoveCapture;
        private static bool _performKeyboardCapture;
        private static bool _performWindowCapture;
        private static bool _activateWindowTopLeft;
        private static bool _trackActivatedWindowSizes;
        private static bool _trackWindowOpenLocations;
        private static int _msResolution;
        public static string StopHookKey;
        public static bool StopOnClick;

        private static IntPtr _winEventHook;
        private static SystemEventHandler _winEventHookHandler;
        private static StringBuilder _buffer = new StringBuilder(512);

        public static List<ScriptCommand> GeneratedCommands;

        public static event EventHandler HookStopped = delegate { };

        #region User32 Window 

        private enum _systemEvents
        {
            EventMin = 0x00000001,       //MIN
            EventMax = 0x7FFFFFFF,          //MAX
            EventSystemForeGround = 0x3,  //The foreground window has changed. The system sends this event even if the foreground window has changed to another window in the same thread. Server applications never send this event.
            MinimizeEnd = 0x0017, //A window object is about to be restored. This event is sent by the system, never by servers.
            MinimizeStart = 0x0016 //A window object is about to be minimized. This event is sent by the system, never by servers.
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SetWinEventHook(_systemEvents eventMin, _systemEvents eventMax, IntPtr hmodWinEventProc, SystemEventHandler lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        private delegate void SystemEventHandler(IntPtr hWinEventHook, _systemEvents @event, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        #endregion

        #region User32 KeyboardMouse

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
        private static extern IntPtr WindowFromPoint(Point Point);

        [DllImport("user32.dll")]
        private static extern IntPtr ChildWindowFromPoint(IntPtr hWndParent, Point Point);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int ToUnicode(uint virtualKeyCode, uint scanCode, byte[] keyboardState, StringBuilder receivingBuffer, int bufferSize, uint flags);


        //enums and structs
        private const int _whMouseLl = 14;

        [Flags]
        private enum _keyStates
        {
            None = 0,
            Down = 1,
            Toggled = 2
        }

        private enum _mouseMessages
        {
            WmLButtonDown = 0x0201,
            WmLButtonUp = 0x0202,
            WmMouseMove = 0x0200,
            WmMouseWheel = 0x020A,
            WmRButtonDown = 0x0204,
            WmRButtonUp = 0x0205
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Point
        {
            public int X;
            public int Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MsLlHookStruct
        {
            public Point Pt;
            public uint MouseData;
            public uint Flags;
            public uint Time;
            public IntPtr DwExtraInfo;
        }

        #endregion

        public static void StartEngineCancellationHook(Keys keyName)
        {
            StopHookKey = keyName.ToString();
            //set hook for engine cancellation
            _keyboardHookID = SetKeyboardHook(_kbProc);
        }

        public static void StartElementCaptureHook(bool stopOnFirstClick)
        {
            StopOnClick = stopOnFirstClick;
            //set hook for engine cancellation
            _mouseHookID = SetMouseHook(_mouseLeftUpProc);
        }

        public static void StartScreenRecordingHook(bool captureClick, bool captureMouse, bool groupMouseMoves, bool captureKeyboard, bool captureWindow, bool activateTopLeft, bool trackActivatedWindowSize, bool trackWindowsOpenLocation, int eventResolution, string stopHookHotKey)
        {
            //create new list for commands generated
            GeneratedCommands = new List<ScriptCommand>();

            //setup variables
            _performMouseClickCapture = captureClick;
            _performMouseMoveCapture = captureMouse;
            _performKeyboardCapture = captureKeyboard;
            _groupMouseMovesIntoSequence = groupMouseMoves;
            _performWindowCapture = captureWindow;
            _activateWindowTopLeft = activateTopLeft;
            _trackActivatedWindowSizes = trackActivatedWindowSize;
            _trackWindowOpenLocations = trackWindowsOpenLocation;
            _msResolution = eventResolution;
            StopHookKey = stopHookHotKey;
            //start hook
            _mouseHookID = SetMouseHook(_mouseProc);
            _keyboardHookID = SetKeyboardHook(_kbProc);

            //if user decided to capture window events
            if (_performWindowCapture)
            {
                _winEventHookHandler = new SystemEventHandler(BuildWindowCommand);
                _winEventHook = SetWinEventHook(_systemEvents.EventMin, _systemEvents.EventMax, IntPtr.Zero, _winEventHookHandler, 0, 0, 0);
            }

            //start stopwatch for timing all event occurences
            _stopWatch = new Stopwatch();
            _stopWatch.Start();

            //stopwatch for tracking mouse moves specifically
            _lastMouseMove = new Stopwatch();
            _lastMouseMove.Start();

        }

        //hook end
        public static void StopHook()
        {
            UnhookWindowsHookEx(_keyboardHookID);
            UnhookWindowsHookEx(_mouseHookID);

            if (_performWindowCapture)
            {
                UnhookWinEvent(_winEventHook);
            }

            //BuildCommentCommand();
            HookStopped(null, new EventArgs());
        }

        //mouse and keyboard hook event triggers
        private static IntPtr KeyboardHookEvent(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)_wmKeyDown)
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
                var message = (_mouseMessages)wParam;

                if (message == _mouseMessages.WmLButtonDown)
                {
                    if (StopOnClick)
                    {
                        UnhookWindowsHookEx(_mouseHookID);
                    }

                    //MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                    //System.Windows.Point point = new System.Windows.Point(hookStruct.pt.x, hookStruct.pt.y);

                    System.Windows.Point point = new System.Windows.Point(Cursor.Position.X, Cursor.Position.Y);
                    MouseEvent?.Invoke(null, new MouseCoordinateEventArgs() { MouseCoordinates = point });
                }
            }

            return CallNextHookEx(_mouseHookID, nCode, wParam, lParam);
        }

        private static IntPtr MouseHookEvent(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                BuildMouseCommand(lParam, (_mouseMessages)wParam);
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
            if (selectedKey == StopHookKey)
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

            if (!_performKeyboardCapture)
            {
                return;
            }

            //add braces
            if (selectedKey.Length > 1)
            {
                selectedKey = "{" + selectedKey + "}";
            }

            //generate sendkeys together
            if ((GeneratedCommands.Count > 1) && (GeneratedCommands[GeneratedCommands.Count - 1] is SendKeystrokesCommand))
            {

                var lastCreatedSendKeysCommand = (SendKeystrokesCommand)GeneratedCommands[GeneratedCommands.Count - 1];

                if (lastCreatedSendKeysCommand.v_TextToSend.Contains("{ENTER}"))
                {
                    //append this to a new command because you dont want text to input after user presses enter

                    //build a pause command to track pause since last command
                    BuildPauseCommand();

                    //build keyboard command
                    var keyboardCommand = new SendKeystrokesCommand
                    {
                        v_TextToSend = selectedKey,
                        v_WindowName = "Current Window"
                    };
                    GeneratedCommands.Add(keyboardCommand);
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
                var keyboardCommand = new SendKeystrokesCommand
                {
                    v_TextToSend = selectedKey,
                    v_WindowName = "Current Window"
                };
                GeneratedCommands.Add(keyboardCommand);
            }
        }

        //build mouse command
        private static void BuildMouseCommand(IntPtr lParam, _mouseMessages mouseMessage)
        {
            string mouseEventClickType = string.Empty;
            switch (mouseMessage)
            {
                case _mouseMessages.WmLButtonDown:
                    mouseEventClickType = "Left Down";
                    break;
                case _mouseMessages.WmLButtonUp:
                    mouseEventClickType = "Left Up";
                    break;
                case _mouseMessages.WmMouseMove:
                    mouseEventClickType = "None";

                    if (_lastMouseMove.ElapsedMilliseconds >= _msResolution)
                    {
                        _lastMouseMove.Restart();
                    }
                    else
                    {
                        return;
                    }
                    break;
                case _mouseMessages.WmRButtonDown:
                    mouseEventClickType = "Right Down";
                    break;
                case _mouseMessages.WmRButtonUp:
                    mouseEventClickType = "Right Up";
                    break;
                default:
                    return;
            }

            ////return if non matching event
            //if (mouseEventClickType == string.Empty)
            //    return;


            //return if we do not want to capture mouse moves
            if ((!_performMouseMoveCapture) && (mouseEventClickType == "None"))
            {
                return;
            }

            //return if we do not want to capture mouse clicks
            if ((!_performMouseClickCapture) && (mouseEventClickType != "None"))
            {
                return;
            }

            //build a pause command to track pause since last command
            BuildPauseCommand();

            //define new mouse command
            MsLlHookStruct hookStruct = (MsLlHookStruct)Marshal.PtrToStructure(lParam, typeof(MsLlHookStruct));

            var mouseMove = new SendMouseMoveCommand
            {
                v_XMousePosition = hookStruct.Pt.X.ToString(),
                v_YMousePosition = hookStruct.Pt.Y.ToString(),
                v_MouseClick = mouseEventClickType
            };

            if (mouseEventClickType != "None")
            {
                IntPtr winHandle = WindowFromPoint(hookStruct.Pt);

                int length = GetWindowText(winHandle, _buffer, _buffer.Capacity);
                var windowName = _buffer.ToString();

                mouseMove.v_Comment = "Clicked On Window: " + windowName;
            }

            GeneratedCommands.Add(mouseMove);
        }

        //build window command
        private static void BuildWindowCommand(IntPtr hWinEventHook, _systemEvents @event, 
            IntPtr hwnd, int idObject, int idChild, 
            uint dwEventThread, uint dwmsEventTime)
        {
            switch (@event)
            {
                case _systemEvents.EventMin:
                    return;
                case _systemEvents.EventMax:
                    return;
                case _systemEvents.EventSystemForeGround:
                    break;
                case _systemEvents.MinimizeEnd:
                    return;
                case _systemEvents.MinimizeStart:
                    return;
                default:
                    return;
            }

            int length = GetWindowText(hwnd, _buffer, _buffer.Capacity);
            var windowName = _buffer.ToString();

            //bypass screen recorder and Cortana (Win10) which throws errors
            if ((windowName == "Screen Recorder") || (windowName == "Cortana"))
            {
                return;
            }

            if (length > 0)
            {
                //wait additional for window to initialize
                //System.Threading.Thread.Sleep(250);
                windowName = _buffer.ToString();

                ActivateWindowCommand activateWindowCommand = new ActivateWindowCommand
                {
                    v_WindowName = windowName,
                    v_Comment = "Generated by Screen Recorder @ " + DateTime.Now.ToString()
                };

                GeneratedCommands.Add(activateWindowCommand);

                //detect if tracking window open location or activate windows to top left
                if (_trackWindowOpenLocations)
                {
                    User32Functions.GetWindowRect(hwnd, out Rect windowRect);


                    MoveWindowCommand moveWindowCommand = new MoveWindowCommand
                    {
                        v_WindowName = windowName,
                        v_XWindowPosition = windowRect.left.ToString(),
                        v_YWindowPosition = windowRect.top.ToString(),
                        v_Comment = "Generated by Screen Recorder @ " + DateTime.Now.ToString()

                    };

                    GeneratedCommands.Add(moveWindowCommand);

                }
                else if (_activateWindowTopLeft)
                {
                    //generate command to set window position
                    MoveWindowCommand moveWindowCommand = new MoveWindowCommand
                    {
                        v_WindowName = windowName,
                        v_XWindowPosition = "0",
                        v_YWindowPosition = "0",
                        v_Comment = "Generated by Screen Recorder @ " + DateTime.Now.ToString()

                    };

                    User32Functions.SetWindowPosition(hwnd, 0, 0);

                    GeneratedCommands.Add(moveWindowCommand);
                }

                //if tracking window sizes is set
                if (_trackActivatedWindowSizes)
                {
                    //create rectangle from hwnd
                    User32Functions.GetWindowRect(hwnd, out Rect windowRect);

                    //do math to get height, etc
                    var width = windowRect.right - windowRect.left;
                    var height = windowRect.bottom - windowRect.top;

                    //generate command to set window position
                    ResizeWindowCommand reszWindowCommand = new ResizeWindowCommand
                    {
                        v_WindowName = windowName,
                        v_XWindowSize = width.ToString(),
                        v_YWindowSize = height.ToString(),
                        v_Comment = "Generated by Screen Recorder @ " + DateTime.Now.ToString()

                    };

                    //add to list
                    GeneratedCommands.Add(reszWindowCommand);
                }
            }
        }

        //build pause command
        private static void BuildPauseCommand()
        {

            if (_stopWatch.ElapsedMilliseconds < 1)
            {
                return;
            }

            _stopWatch.Stop();
            var pauseTime = _stopWatch.ElapsedMilliseconds;
            var pauseCommand = new PauseScriptCommand
            {
                v_PauseLength = pauseTime.ToString()
            };
            GeneratedCommands.Add(pauseCommand);
            _stopWatch.Restart();
        }

        private static IntPtr SetKeyboardHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())

            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(_whKeyboardLl, proc,

                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private static IntPtr SetMouseHook(LowLevelMouseProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())

            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(_whMouseLl, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }


        #region User32 Keyboard Mouse
        private static _keyStates GetKeyState(Keys key)
        {
            _keyStates state = _keyStates.None;

            short retVal = GetKeyState((int)key);

            //If the high-order bit is 1, the key is down
            //otherwise, it is up.
            if ((retVal & 0x8000) == 0x8000)
                state |= _keyStates.Down;

            //If the low-order bit is 1, the key is toggled.
            if ((retVal & 1) == 1)
                state |= _keyStates.Toggled;

            return state;
        }

        //helper checks
        public static bool IsKeyDown(Keys key)
        {
            return _keyStates.Down == (GetKeyState(key) & _keyStates.Down);
        }

        public static bool IsKeyToggled(Keys key)
        {
            return _keyStates.Toggled == (GetKeyState(key) & _keyStates.Toggled);
        }
        #endregion
    }
}
