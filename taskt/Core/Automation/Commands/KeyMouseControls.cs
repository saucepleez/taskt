using System;
using System.Data;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    static internal class KeyMouseControls
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
        public static string v_YOffsetAdjustment { get; }
        #endregion

        #region enum, struct, const
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
        #endregion
    }
}
