using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    static public class KeyMouseControls
    {
        #region VirtualProperty
        /// <summary>
        /// wait after key enter
        /// </summary>
        [PropertyDescription("Wait Time after Keys Enter (ms)")]
        [InputSpecification("Wait Time", true)]
        [PropertyDetailSampleUsage("**500**", PropertyDetailSampleUsage.ValueType.Value, "Wait Time")]
        [PropertyDetailSampleUsage("**{{{vWaitTime}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Wait Time")]
        [Remarks("When the Wait Time is less than **100** is specified, it will be **100**")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true, "500")]
        [PropertyFirstValue("500")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Wait Time", PropertyValidationRule.ValidationRuleFlags.LessThanZero | PropertyValidationRule.ValidationRuleFlags.EqualsZero)]
        [PropertyParameterOrder(5000)]
        public static string v_WaitTimeAfterKeyEnter { get; }

        /// <summary>
        /// mouse click
        /// </summary>
        [PropertyDescription("Mouse Click Type")]
        [PropertyUISelectionOption("Left Click")]
        [PropertyUISelectionOption("Middle Click")]
        [PropertyUISelectionOption("Right Click")]
        [PropertyUISelectionOption("Left Down")]
        [PropertyUISelectionOption("Middle Down")]
        [PropertyUISelectionOption("Right Down")]
        [PropertyUISelectionOption("Left Up")]
        [PropertyUISelectionOption("Middle Up")]
        [PropertyUISelectionOption("Right Up")]
        [PropertyUISelectionOption("Double Left Click")]
        [PropertyUISelectionOption("None")]
        [InputSpecification("", true)]
        [SampleUsage("")]
        [Remarks("You can simulate custom click by using multiple mouse click commands in succession, adding **Pause Command** in between where required.")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("Mouse Click", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Mouse Click")]
        [PropertyParameterOrder(5000)]
        public static string v_MouseClickType { get; }

        /// <summary>
        /// wait after key enter
        /// </summary>
        [PropertyDescription("Wait Time after Mouse Click (ms)")]
        [InputSpecification("Wait Time", true)]
        [PropertyDetailSampleUsage("**500**", PropertyDetailSampleUsage.ValueType.Value, "Wait Time")]
        [PropertyDetailSampleUsage("**{{{vWaitTime}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Wait Time")]
        [Remarks("When the Wait Time is less than **100** is specified, it will be **100**")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true, "500")]
        [PropertyFirstValue("500")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Wait Time", PropertyValidationRule.ValidationRuleFlags.LessThanZero | PropertyValidationRule.ValidationRuleFlags.EqualsZero)]
        [PropertyParameterOrder(5000)]
        public static string v_WaitTimeAfterMouseClick { get; }

        /// <summary>
        /// offset x
        /// </summary>
        [PropertyDescription("Offset X Coordinate")]
        [InputSpecification("Offset X", true)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDetailSampleUsage("**0**", PropertyDetailSampleUsage.ValueType.Value, "Offset X")]
        [PropertyDetailSampleUsage("**100**", PropertyDetailSampleUsage.ValueType.Value, "Offset X")]
        [PropertyDetailSampleUsage("**{{{vXOffset}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Offset X")]
        [Remarks("This will move the mouse X pixels to the right of the location of the target")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyIsOptional(true, "0")]
        [PropertyFirstValue("0")]
        [PropertyDisplayText(true, "Offset X")]
        [PropertyParameterOrder(5000)]
        public static string v_XOffsetAdjustment { get; }

        /// <summary>
        /// offset y
        /// </summary>
        [PropertyDescription("Offset Y Coordinate")]
        [InputSpecification("Offset Y")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDetailSampleUsage("**0**", PropertyDetailSampleUsage.ValueType.Value, "Offset Y")]
        [PropertyDetailSampleUsage("**100**", PropertyDetailSampleUsage.ValueType.Value, "Offset Y")]
        [PropertyDetailSampleUsage("**{{{vYOffset}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Offset Y")]
        [Remarks("This will move the mouse Y pixels down from the top of the location of the target")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyIsOptional(true, "0")]
        [PropertyFirstValue("0")]
        [PropertyDisplayText(true, "Offset Y")]
        [PropertyParameterOrder(5000)]
        public static string v_YOffsetAdjustment { get; }
        #endregion

        #region enum, struct, const, field, property
        private enum MouseEvents
        {
            MOUSEEVENTF_LEFTDOWN = 0x02,
            MOUSEEVENTF_LEFTUP = 0x04,
            MOUSEEVENTF_RIGHTDOWN = 0x08,
            MOUSEEVENTF_RIGHTUP = 0x10,
            MOUSEEVENTF_MIDDLEDOWN = 0x20,
            MOUSEEVENTF_MIDDLEUP = 0x40
        }

        private const int KEYEVENTF_EXTENDEDKEY = 0x1;
        private const int KEYEVENTF_KEYUP = 0x2;

        private static Dictionary<string, string> _keysDescription = new Dictionary<string, string>();

        public static Dictionary<string, string> KeysDescription
        {
            get
            {
                if (_keysDescription.Count == 0)
                {
                    CreateKeysDescription();
                }

                return _keysDescription;
            }
        }

        private static List<string> _keysList = new List<string>();

        public static List<string> KeysList
        {
            get
            {
                if (_keysList.Count == 0)
                {
                    CreateKeysList();
                }
                return _keysList;
            }
        }

        #endregion

        #region api
        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        public static void SetCursorPosition(int newXPosition, int newYPosition)
        {
            SetCursorPos(newXPosition, newYPosition);
        }

        public static void SendMouseClick(string clickType, int xMousePosition, int yMousePosition)
        {
            var actions = new List<MouseEvents>();

            switch (clickType.ToLower())
            {
                case "double left click":
                    //mouse_event((int)MouseEvents.MOUSEEVENTF_LEFTDOWN, xMousePosition, yMousePosition, 0, 0);
                    //mouse_event((int)MouseEvents.MOUSEEVENTF_LEFTUP, xMousePosition, yMousePosition, 0, 0);
                    //mouse_event((int)MouseEvents.MOUSEEVENTF_LEFTDOWN, xMousePosition, yMousePosition, 0, 0);
                    //mouse_event((int)MouseEvents.MOUSEEVENTF_LEFTUP, xMousePosition, yMousePosition, 0, 0);
                    actions.AddRange(new MouseEvents[] { MouseEvents.MOUSEEVENTF_LEFTDOWN, MouseEvents.MOUSEEVENTF_LEFTUP, MouseEvents.MOUSEEVENTF_LEFTDOWN, MouseEvents.MOUSEEVENTF_LEFTUP });
                    break;

                case "left click":
                    //mouse_event((int)MouseEvents.MOUSEEVENTF_LEFTDOWN, xMousePosition, yMousePosition, 0, 0);
                    //mouse_event((int)MouseEvents.MOUSEEVENTF_LEFTUP, xMousePosition, yMousePosition, 0, 0);
                    actions.AddRange(new MouseEvents[] { MouseEvents.MOUSEEVENTF_LEFTDOWN, MouseEvents.MOUSEEVENTF_LEFTUP });
                    break;

                case "right click":
                    //mouse_event((int)MouseEvents.MOUSEEVENTF_RIGHTDOWN, xMousePosition, yMousePosition, 0, 0);
                    //mouse_event((int)MouseEvents.MOUSEEVENTF_RIGHTUP, xMousePosition, yMousePosition, 0, 0);
                    actions.AddRange(new MouseEvents[] { MouseEvents.MOUSEEVENTF_RIGHTDOWN, MouseEvents.MOUSEEVENTF_RIGHTUP });
                    break;

                case "middle click":
                    //mouse_event((int)MouseEvents.MOUSEEVENTF_MIDDLEDOWN, xMousePosition, yMousePosition, 0, 0);
                    //mouse_event((int)MouseEvents.MOUSEEVENTF_MIDDLEUP, xMousePosition, yMousePosition, 0, 0);
                    actions.AddRange(new MouseEvents[] { MouseEvents.MOUSEEVENTF_MIDDLEDOWN, MouseEvents.MOUSEEVENTF_MIDDLEUP });
                    break;

                case "left down":
                    //mouse_event((int)MouseEvents.MOUSEEVENTF_LEFTDOWN, xMousePosition, yMousePosition, 0, 0);
                    actions.Add(MouseEvents.MOUSEEVENTF_LEFTDOWN);
                    break;

                case "right down":
                    //mouse_event((int)MouseEvents.MOUSEEVENTF_RIGHTDOWN, xMousePosition, yMousePosition, 0, 0);
                    actions.Add(MouseEvents.MOUSEEVENTF_RIGHTDOWN);
                    break;

                case "middle down":
                    //mouse_event((int)MouseEvents.MOUSEEVENTF_MIDDLEDOWN, xMousePosition, yMousePosition, 0, 0);
                    actions.Add(MouseEvents.MOUSEEVENTF_MIDDLEDOWN);
                    break;

                case "left up":
                    //mouse_event((int)MouseEvents.MOUSEEVENTF_LEFTUP, xMousePosition, yMousePosition, 0, 0);
                    actions.Add(MouseEvents.MOUSEEVENTF_LEFTUP);
                    break;

                case "right up":
                    //mouse_event((int)MouseEvents.MOUSEEVENTF_RIGHTUP, xMousePosition, yMousePosition, 0, 0);
                    actions.Add(MouseEvents.MOUSEEVENTF_RIGHTUP);
                    break;

                case "middle up":
                    //mouse_event((int)MouseEvents.MOUSEEVENTF_MIDDLEUP, xMousePosition, yMousePosition, 0, 0);
                    actions.Add(MouseEvents.MOUSEEVENTF_MIDDLEUP);
                    break;

                default:
                    break;
            }

            foreach(var mouse in actions)
            {
                mouse_event((int)mouse, xMousePosition, yMousePosition, 0, 0);
            }
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

        public static void KeyDown(Keys vKey)
        {
            keybd_event((byte)vKey, 0, KEYEVENTF_EXTENDEDKEY, 0);
        }

        public static void KeyUp(Keys vKey)
        {
            keybd_event((byte)vKey, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }

        private static void CreateKeysDescription()
        {
            _keysDescription.Clear();
            foreach(Keys k in Enum.GetValues(typeof(Keys)))
            {
                if (_keysDescription.ContainsKey(k.ToString()))
                {
                    continue;
                }

                switch (k)
                {
                    //letters
                    case Keys.A:
                    case Keys.B:
                    case Keys.C:
                    case Keys.D:
                    case Keys.E:
                    case Keys.F:
                    case Keys.G:
                    case Keys.H:
                    case Keys.I:
                    case Keys.J:
                    case Keys.K:
                    case Keys.L:
                    case Keys.M:
                    case Keys.N:
                    case Keys.O:
                    case Keys.P:
                    case Keys.Q:
                    case Keys.R:
                    case Keys.S:
                    case Keys.T:
                    case Keys.U:
                    case Keys.V:
                    case Keys.W:
                    case Keys.X:
                    case Keys.Y:
                    case Keys.Z:
                        _keysDescription.Add(k.ToString(), k.ToString());
                        break;

                    //digits
                    case Keys.D0:
                        _keysDescription.Add(k.ToString(), "0");
                        break;

                    case Keys.NumPad0:
                        _keysDescription.Add(k.ToString(), "Number Pad 0");
                        break;
                    case Keys.D1:
                        _keysDescription.Add(k.ToString(), "1");
                        break;
                    case Keys.NumPad1:
                        _keysDescription.Add(k.ToString(), "Number Pad 1");
                        break;
                    case Keys.D2:
                        _keysDescription.Add(k.ToString(), "2");
                        break;
                    case Keys.NumPad2:
                        _keysDescription.Add(k.ToString(), "Number Pad 2");
                        break;
                    case Keys.D3:
                        _keysDescription.Add(k.ToString(), "3");
                        break;
                    case Keys.NumPad3:
                        _keysDescription.Add(k.ToString(), "Number Pad 3");
                        break;
                    case Keys.D4:
                        _keysDescription.Add(k.ToString(), "4");
                        break;
                    case Keys.NumPad4:
                        _keysDescription.Add(k.ToString(), "Number Pad 4");
                        break;
                    case Keys.D5:
                        _keysDescription.Add(k.ToString(), "5");
                        break;
                    case Keys.NumPad5:
                        _keysDescription.Add(k.ToString(), "Number Pad 5");
                        break;
                    case Keys.D6:
                        _keysDescription.Add(k.ToString(), "6");
                        break;
                    case Keys.NumPad6:
                        _keysDescription.Add(k.ToString(), "Number Pad 6");
                        break;
                    case Keys.D7:
                        _keysDescription.Add(k.ToString(), "7");
                        break;
                    case Keys.NumPad7:
                        _keysDescription.Add(k.ToString(), "Number Pad 7");
                        break;
                    case Keys.D8:
                        _keysDescription.Add(k.ToString(), "8");
                        break;
                    case Keys.NumPad8:
                        _keysDescription.Add(k.ToString(), "Number Pad 8");
                        break;
                    case Keys.D9:
                        _keysDescription.Add(k.ToString(), "9");
                        break;
                    case Keys.NumPad9:
                        _keysDescription.Add(k.ToString(), "Number Pad 9");
                        break;

                    //punctuation
                    case Keys.Add:
                        _keysDescription.Add(k.ToString(), "Number Pad +");
                        break;
                    case Keys.Subtract:
                        _keysDescription.Add(k.ToString(), "Number Pad -");
                        break;
                    case Keys.Divide:
                        _keysDescription.Add(k.ToString(), "Number Pad /");
                        break;
                    case Keys.Multiply:
                        _keysDescription.Add(k.ToString(), "Number Pad *");
                        break;
                    case Keys.Space:
                        _keysDescription.Add(k.ToString(), "Spacebar");
                        break;
                    case Keys.Decimal:
                        _keysDescription.Add(k.ToString(), "Number Pad .");
                        break;

                    //function
                    case Keys.F1:
                    case Keys.F2:
                    case Keys.F3:
                    case Keys.F4:
                    case Keys.F5:
                    case Keys.F6:
                    case Keys.F7:
                    case Keys.F8:
                    case Keys.F9:
                    case Keys.F10:
                    case Keys.F11:
                    case Keys.F12:
                    case Keys.F13:
                    case Keys.F14:
                    case Keys.F15:
                    case Keys.F16:
                    case Keys.F17:
                    case Keys.F18:
                    case Keys.F19:
                    case Keys.F20:
                    case Keys.F21:
                    case Keys.F22:
                    case Keys.F23:
                    case Keys.F24:
                        _keysDescription.Add(k.ToString(), k.ToString());
                        break;

                    //navigation
                    case Keys.Up:
                        _keysDescription.Add(k.ToString(), "Up Arrow");
                        break;
                    case Keys.Down:
                        _keysDescription.Add(k.ToString(), "Down Arrow");
                        break;
                    case Keys.Left:
                        _keysDescription.Add(k.ToString(), "Left Arrow");
                        break;
                    case Keys.Right:
                        _keysDescription.Add(k.ToString(), "Right Arrow");
                        break;
                    case Keys.Prior:
                        _keysDescription.Add(k.ToString(), "Page Up");
                        break;
                    case Keys.Next:
                        _keysDescription.Add(k.ToString(), "Page Down");
                        break;
                    case Keys.Home:
                        _keysDescription.Add(k.ToString(), "Home");
                        break;
                    case Keys.End:
                        _keysDescription.Add(k.ToString(), "End");
                        break;

                    //control keys
                    case Keys.Back:
                        _keysDescription.Add(k.ToString(), "Backspace");
                        break;
                    case Keys.Tab:
                        _keysDescription.Add(k.ToString(), "Tab");
                        break;
                    case Keys.Escape:
                        _keysDescription.Add(k.ToString(), "Escape");
                        break;
                    case Keys.Enter:
                        _keysDescription.Add(k.ToString(), "Enter");
                        break;
                    case Keys.Shift:
                    case Keys.ShiftKey:
                        _keysDescription.Add(k.ToString(), "Shift");
                        break;
                    case Keys.LShiftKey:
                        _keysDescription.Add(k.ToString(), "Shift (Left)");
                        break;
                    case Keys.RShiftKey:
                        _keysDescription.Add(k.ToString(), "Shift (Right)");
                        break;
                    case Keys.Control:
                    case Keys.ControlKey:
                        _keysDescription.Add(k.ToString(), "Control");
                        break;
                    case Keys.LControlKey:
                        _keysDescription.Add(k.ToString(), "Control (Left)");
                        break;
                    case Keys.RControlKey:
                        _keysDescription.Add(k.ToString(), "Control (Right)");
                        break;
                    case Keys.Menu:
                    case Keys.Alt:
                        _keysDescription.Add(k.ToString(), "Alt");
                        break;
                    case Keys.LMenu:
                        _keysDescription.Add(k.ToString(), "Alt (Left)");
                        break;
                    case Keys.RMenu:
                        _keysDescription.Add(k.ToString(), "Alt (Right)");
                        break;
                    case Keys.Pause:
                        _keysDescription.Add(k.ToString(), "Pause");
                        break;
                    case Keys.CapsLock:
                        _keysDescription.Add(k.ToString(), "Caps Lock");
                        break;
                    case Keys.NumLock:
                        _keysDescription.Add(k.ToString(), "Num Lock");
                        break;
                    case Keys.Scroll:
                        _keysDescription.Add(k.ToString(), "Scroll Lock");
                        break;
                    case Keys.PrintScreen:
                        _keysDescription.Add(k.ToString(), "Print Screen");
                        break;
                    case Keys.Insert:
                        _keysDescription.Add(k.ToString(), "Insert");
                        break;
                    case Keys.Delete:
                        _keysDescription.Add(k.ToString(), "Delete");
                        break;
                    case Keys.Help:
                        _keysDescription.Add(k.ToString(), "Help");
                        break;
                    case Keys.LWin:
                        _keysDescription.Add(k.ToString(), "Windows (Left)");
                        break;
                    case Keys.RWin:
                        _keysDescription.Add(k.ToString(), "Windows (Right)");
                        break;
                    case Keys.Apps:
                        _keysDescription.Add(k.ToString(), "Context Menu");
                        break;

                    //browser keys
                    case Keys.BrowserBack:
                        _keysDescription.Add(k.ToString(), "Browser Back");
                        break;
                    case Keys.BrowserFavorites:
                        _keysDescription.Add(k.ToString(), "Browser Favorites");
                        break;
                    case Keys.BrowserForward:
                        _keysDescription.Add(k.ToString(), "Browser Forward");
                        break;
                    case Keys.BrowserHome:
                        _keysDescription.Add(k.ToString(), "Browser Home");
                        break;
                    case Keys.BrowserRefresh:
                        _keysDescription.Add(k.ToString(), "Browser Refresh");
                        break;
                    case Keys.BrowserSearch:
                        _keysDescription.Add(k.ToString(), "Browser Search");
                        break;
                    case Keys.BrowserStop:
                        _keysDescription.Add(k.ToString(), "Browser Stop");
                        break;

                    //media keys
                    case Keys.VolumeDown:
                        _keysDescription.Add(k.ToString(), "Volume Down");
                        break;
                    case Keys.VolumeMute:
                        _keysDescription.Add(k.ToString(), "Volume Mute");
                        break;
                    case Keys.VolumeUp:
                        _keysDescription.Add(k.ToString(), "Volume Up");
                        break;
                    case Keys.MediaNextTrack:
                        _keysDescription.Add(k.ToString(), "Next Track");
                        break;
                    case Keys.Play:
                    case Keys.MediaPlayPause:
                        _keysDescription.Add(k.ToString(), "Play");
                        break;
                    case Keys.MediaPreviousTrack:
                        _keysDescription.Add(k.ToString(), "Previous Track");
                        break;
                    case Keys.MediaStop:
                        _keysDescription.Add(k.ToString(), "Stop");
                        break;
                    case Keys.SelectMedia:
                        _keysDescription.Add(k.ToString(), "Select Media");
                        break;

                    //special keys
                    case Keys.LaunchMail:
                        _keysDescription.Add(k.ToString(), "Launch Mail");
                        break;
                    case Keys.LaunchApplication1:
                        _keysDescription.Add(k.ToString(), "Launch Favorite Application 1");
                        break;
                    case Keys.LaunchApplication2:
                        _keysDescription.Add(k.ToString(), "Launch Favorite Application 2");
                        break;
                    case Keys.Zoom:
                        _keysDescription.Add(k.ToString(), "Zoom");
                        break;

                    //oem keys 
                    case Keys.OemSemicolon: //oem1
                        _keysDescription.Add(k.ToString(), ");");
                        break;
                    case Keys.OemQuestion:  //oem2
                        _keysDescription.Add(k.ToString(), "?");
                        break;
                    case Keys.Oemtilde:     //oem3
                        _keysDescription.Add(k.ToString(), "~");
                        break;
                    case Keys.OemOpenBrackets:  //oem4
                        _keysDescription.Add(k.ToString(), "[");
                        break;
                    case Keys.OemPipe:  //oem5
                        _keysDescription.Add(k.ToString(), "|");
                        break;
                    case Keys.OemCloseBrackets:    //oem6
                        _keysDescription.Add(k.ToString(), "]");
                        break;
                    case Keys.OemQuotes:        //oem7
                        _keysDescription.Add(k.ToString(), "'");
                        break;
                    case Keys.OemBackslash: //oem102
                        _keysDescription.Add(k.ToString(), "/");
                        break;
                    case Keys.Oemplus:
                        _keysDescription.Add(k.ToString(), "+");
                        break;
                    case Keys.OemMinus:
                        _keysDescription.Add(k.ToString(), "-");
                        break;
                    case Keys.Oemcomma:
                        _keysDescription.Add(k.ToString(), ",");
                        break;
                    case Keys.OemPeriod:
                        _keysDescription.Add(k.ToString(), ".");
                        break;

                    //unsupported oem keys
                    case Keys.Oem8:
                    case Keys.OemClear:
                        break;


                    //IME keys (not support SendKeys)
                    case Keys.HanjaMode:
                    case Keys.JunjaMode:
                    case Keys.HangulMode:
                    case Keys.FinalMode:    //duplicate values: Hanguel, Kana, Kanji  
                    case Keys.IMEAccept:
                    case Keys.IMEConvert:   //duplicate: IMEAceept
                    case Keys.IMEModeChange:
                    case Keys.IMENonconvert:
                        break;

                    //unsupported other keys
                    case Keys.None:
                    case Keys.LButton:
                    case Keys.RButton:
                    case Keys.MButton:
                    case Keys.XButton1:
                    case Keys.XButton2:
                    case Keys.Clear:
                    case Keys.Sleep:
                    case Keys.Cancel:
                    case Keys.LineFeed:
                    case Keys.Select:
                    case Keys.Print:
                    case Keys.Execute:
                    case Keys.Separator:
                    case Keys.ProcessKey:
                    case Keys.Packet:
                    case Keys.Attn:
                    case Keys.Crsel:
                    case Keys.Exsel:
                    case Keys.EraseEof:
                    case Keys.NoName:
                    case Keys.Pa1:
                    case Keys.KeyCode:
                    case Keys.Modifiers:
                    default:
                        break;
                }
            }
        }

        public static void CreateKeysList()
        {
            if (_keysDescription.Count == 0)
            {
                CreateKeysDescription();
            }

            if (_keysList.Count == 0)
            {
                foreach(var kv in _keysDescription)
                {
                    _keysList.Add(string.Concat(kv.Value, " [", kv.Key, "]"));
                }
            }
        }
        #endregion
    }
}
