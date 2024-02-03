using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for window methods
    /// </summary>
    internal static class WindowControls
    {

        #region fields

        /// <summary>
        /// old intermediate current window keyword
        /// </summary>
        public const string INTERMEDIATE_CURRENT_WINDOW_KEYWORD = "%kwd_current_window%";
        /// <summary>
        /// old intermediate Desktop keyword
        /// </summary>
        public const string INTERMEDIATE_DESKTOP_KEYWORD = "%kwd_desktop%";
        /// <summary>
        /// old intermediate all windows keyword
        /// </summary>
        public const string INTERMEDIATE_ALL_WINDOWS_KEYWORD = "%kwd_all_windows%";
        /// <summary>
        /// old intermediate current window position keyword
        /// </summary>
        public const string INTERMEDIATE_CURRENT_WINDOW_POSITION_KEYWORD = "%kwd_current_position%";
        /// <summary>
        /// old intermediate current window X position keyword
        /// </summary>
        public const string INTERMEDIATE_CURRENT_WINDOW_X_POSITION_KEYWORD = "%kwd_current_xposition%";
        /// <summary>
        /// old intermediate current window Y position keyword
        /// </summary>
        public const string INTERMEDIATE_CURRENT_WINDOW_Y_POSITION_KEYWORD = "%kwd_current_yposition%";
        #endregion

        #region virtualproperty

        /// <summary>
        /// windows name property
        /// </summary>
        [PropertyDescription("Window Name")]
        [InputSpecification("Window Name", true)]
        [PropertyDetailSampleUsage("**Untitled - Notepad**", "Specify the **Notepad**")]
        [PropertyDetailSampleUsage("**%kwd_current_window%**", "Specify the Current Activate Window")]
        [PropertyDetailSampleUsage("**{{{vWindow}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Window Name")]
        [Remarks("")]
        [PropertyIsWindowNamesList(true)]
        [PropertyCustomUIHelper("Up-to-date", nameof(WindowControls) + "+" + nameof(WindowControls.lnkWindowNameUpToDate_Click))]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIntermediateConvert(nameof(ApplicationSettings.EngineSettings.convertToIntermediateWindowName), nameof(ApplicationSettings.EngineSettings.convertToRawWindowName))]
        [PropertyValidationRule("Window Name", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Name")]
        [PropertyParameterOrder(5000)]
        public static string v_WindowName { get; }

        /// <summary>
        /// windows name compare(search) method
        /// </summary>
        [PropertyDescription("Compare Method for the Window Name")]
        [InputSpecification("", true)]
        [PropertyUISelectionOption("Contains")]
        [PropertyUISelectionOption("Starts with")]
        [PropertyUISelectionOption("Ends with")]
        [PropertyUISelectionOption("Exact match")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsOptional(true, "Contains")]
        [PropertyDisplayText(true, "Compare Method")]
        [PropertyParameterOrder(5000)]
        public static string v_CompareMethod { get; }

        /// <summary>
        /// match method, please specify PropertySelectionChangeEvent
        /// </summary>
        [PropertyDescription("Match Method for the Window Name")]
        [InputSpecification("", true)]
        [PropertyUISelectionOption("First")]
        [PropertyUISelectionOption("Last")]
        [PropertyUISelectionOption("All")]
        [PropertyUISelectionOption("Index")]
        [PropertyDetailSampleUsage("**First**", "Specify the First Window")]
        [PropertyDetailSampleUsage("**Last**", "Specify the Last Window")]
        [PropertyDetailSampleUsage("**All**", "Specify the All Windows")]
        [PropertyDetailSampleUsage("**Index**", "the Window specifed by Index. **0** means First Window")]
        [Remarks("Specify when there are Multiple Matching Windows")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsOptional(true, "First")]
        [PropertyParameterOrder(5000)]
        public static string v_MatchMethod { get; }

        /// <summary>
        /// match method get one window, please specify PropertySelectionChangeEvent
        /// </summary>
        [PropertyDescription("Match Method for the Window Name")]
        [InputSpecification("", true)]
        [PropertyUISelectionOption("First")]
        [PropertyUISelectionOption("Last")]
        [PropertyUISelectionOption("Index")]
        [PropertyDetailSampleUsage("**First**", "Specify the First Window")]
        [PropertyDetailSampleUsage("**Last**", "Specify the Last Window")]
        [PropertyDetailSampleUsage("**Index**", "the Window specifed by Index. **0** means First Window")]
        [Remarks("Specify when there are Multiple Matching Windows")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsOptional(true, "First")]
        [PropertyParameterOrder(5000)]
        public static string v_MatchMethod_Single { get; }

        /// <summary>
        /// window index for match
        /// </summary>
        [PropertyDescription("Window Index")]
        [InputSpecification("Window Index", true)]
        [Remarks("")]
        [PropertyDetailSampleUsage("**0**", "Specify the First Window")]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Window Index")]
        [PropertyDetailSampleUsage("**{{{vIndex}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Window Index")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyIsOptional(true, "0")]
        [PropertyFirstValue("0")]
        [PropertyParameterOrder(5000)]
        public static string v_TargetWindowIndex { get; }

        /// <summary>
        /// window wait time
        /// </summary>
        [PropertyDescription("Wait Time for the Window to Exist (sec)")]
        [InputSpecification("Wait Time", true)]
        [Remarks("Specify how long to Wait before an Error will occur because the Window is Not Found.")]
        [PropertyDetailSampleUsage("**60**", PropertyDetailSampleUsage.ValueType.Value, "Wait Time")]
        [PropertyDetailSampleUsage("**{{{vTime}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Wait Time")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyIsOptional(true, "60")]
        [PropertyFirstValue("60")]
        [PropertyParameterOrder(5000)]
        public static string v_WaitTime { get; }

        /// <summary>
        /// window name result
        /// </summary>
        [PropertyDescription("Variable Name to Store Window Name Result")]
        [InputSpecification("Variable Name", true)]
        [PropertyDetailSampleUsage("**vWin**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vWin}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [Remarks("When Match Method is **All**, data type is LIST, otherwise it is BASIC")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsVariablesList(true)]
        [PropertyIsOptional(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyValidationRule("Window Name Result", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        [PropertyParameterOrder(5000)]
        public static string v_WindowNameResult { get; }

        /// <summary>
        /// output window handle result
        /// </summary>
        [PropertyDescription("Variable Name to Store Window Handle Result")]
        [InputSpecification("Variable Name", true)]
        [PropertyDetailSampleUsage("**vHandle**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vHandle}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [Remarks("When Match Method is **All**, data type is LIST, otherwise it is BASIC")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsVariablesList(true)]
        [PropertyIsOptional(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.WindowHandle, true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyValidationRule("Window Handle Result", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        [PropertyParameterOrder(5000)]
        public static string v_OutputWindowHandle { get; }

        /// <summary>
        /// input window handle result
        /// </summary>
        [PropertyDescription("Window Handle Variable Name")]
        [InputSpecification("Variable Name", true)]
        [PropertyDetailSampleUsage("**vHandle**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vHandle}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.WindowHandle, true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyValidationRule("Window Handle Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Window Handle")]
        [PropertyParameterOrder(5000)]
        public static string v_InputWindowHandle { get; }

        /// <summary>
        /// input window width
        /// </summary>
        [PropertyDescription("Window Width (Pixcel)")]
        [InputSpecification("Window Width", true)]
        [PropertyDetailSampleUsage("**640**", PropertyDetailSampleUsage.ValueType.Value, "Width")]
        [PropertyDetailSampleUsage("**{{{vWidth}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Width")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Width", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.EqualsZero | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(true, "Width")]
        [PropertyAvailableSystemVariable(Engine.SystemVariables.LimitedSystemVariableNames.Window_Size)]
        [PropertyParameterOrder(5000)]
        public static string v_InputWidth { get; }

        /// <summary>
        /// input window height
        /// </summary>
        [PropertyDescription("Window Height (Pixcel)")]
        [InputSpecification("Window Height", true)]
        [PropertyDetailSampleUsage("**480**", PropertyDetailSampleUsage.ValueType.Value, "Height")]
        [PropertyDetailSampleUsage("**{{{vHeight}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Height")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Height", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.EqualsZero | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(true, "Height")]
        [PropertyParameterOrder(5000)]
        public static string v_InputHeight { get; }
        #endregion

        #region enum, struct
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

        ///// <summary>
        ///// Window Rect
        ///// </summary>
        //public struct RECT
        //{
        //    public int left;
        //    public int top;
        //    public int right;
        //    public int bottom;
        //}

        public struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public int showCmd;
            Point ptMinPosition;
            Point ptMaxPosition;
            RECT rcNormalPosition;
            RECT rcDevice;
        }
        #endregion

        #region win api

        private static List<(IntPtr, string)> windowTitles;

        private delegate bool EnumWindowsDelegate(IntPtr hWnd, IntPtr lparam);

        [DllImport("user32.dll")]
        private static extern int EnumWindows(EnumWindowsDelegate lpEnumFunc, IntPtr lparam);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowTextLengthW(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowTextW(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("User32.dll", EntryPoint = "SetForegroundWindow")]
        private static extern IntPtr SetForegroundWindowNative(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "GetWindowRect")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        private static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, WindowState nCmdShow);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetDesktopWindow();

        /// <summary>
        /// get all window name and handle as Dictionary. Key is WindowHandle, Value is Window name
        /// </summary>
        /// <returns></returns>
        public static List<(IntPtr, string)> GetAllWindowNamesAndHandles()
        {
            windowTitles = new List<(IntPtr, string)>();

            EnumWindows(new EnumWindowsDelegate(EnumerateWindow), IntPtr.Zero);

            return new List<(IntPtr, string)>(windowTitles);
        }

        /// <summary>
        /// enum windows
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private static bool EnumerateWindow(IntPtr hWnd, IntPtr lParam)
        {
            int titleLengthA = GetWindowTextLengthW(hWnd);
            if (IsWindowVisible(hWnd) && (titleLengthA > 0))
            {
                StringBuilder title = new StringBuilder(titleLengthA + 1);
                GetWindowTextW(hWnd, title, title.Capacity);

                windowTitles.Add((hWnd, title.ToString()));
            }
            return true;
        }

        /// <summary>
        /// get window name from window handle
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public static string GetWindowTitle(IntPtr hWnd)
        {
            int titleLengthA = GetWindowTextLengthW(hWnd);
            StringBuilder title = new StringBuilder(titleLengthA + 1);
            GetWindowTextW(hWnd, title, title.Capacity);
            return title.ToString();
        }

        /// <summary>
        /// get active window handlw
        /// </summary>
        /// <returns></returns>
        public static IntPtr GetActiveWindowHandle()
        {
            return GetForegroundWindow();
        }

        /// <summary>
        /// set window state
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="windowState"></param>
        public static void SetWindowState(IntPtr hWnd, WindowState windowState)
        {
            ShowWindow(hWnd, (int)windowState);
        }

        /// <summary>
        /// set foreground to window
        /// </summary>
        /// <param name="hWnd"></param>
        public static void SetToForegroundWindow(IntPtr hWnd)
        {
            SetForegroundWindowNative(hWnd);
        }

        /// <summary>
        /// get window RECT
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public static RECT GetWindowRect(IntPtr hWnd)
        {
            GetWindowRect(hWnd, out RECT clientArea);
            return clientArea;
        }

        /// <summary>
        /// get window state
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public static int GetWindowState(IntPtr hWnd)
        {
            var wInfo = new WINDOWPLACEMENT();
            GetWindowPlacement(hWnd, ref wInfo);
            return wInfo.showCmd;
        }

        /// <summary>
        /// close window. send SendMessage
        /// </summary>
        /// <param name="hWnd"></param>
        public static void CloseWindow(IntPtr hWnd)
        {
            const UInt32 WM_CLOSE = 0x0010;
            SendMessage(hWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// set window position
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="newXPosition"></param>
        /// <param name="newYPosition"></param>
        public static void SetWindowPosition(IntPtr hWnd, int newXPosition, int newYPosition)
        {
            const short SWP_NOSIZE = 1;
            const short SWP_NOZORDER = 0X4;
            const int SWP_SHOWWINDOW = 0x0040;

            SetWindowPos(hWnd, 0, newXPosition, newYPosition, 0, 0, SWP_NOZORDER | SWP_NOSIZE | SWP_SHOWWINDOW);
        }

        /// <summary>
        /// set window size
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="newXSize"></param>
        /// <param name="newYSize"></param>
        public static void SetWindowSize(IntPtr hWnd, int newXSize, int newYSize)
        {
            const short SWP_NOZORDER = 0X4;
            const int SWP_SHOWWINDOW = 0x0040;

            GetWindowRect(hWnd, out RECT windowRect);

            SetWindowPos(hWnd, 0, windowRect.left, windowRect.top, newXSize, newYSize, SWP_NOZORDER | SWP_SHOWWINDOW);
        }

        /// <summary>
        /// ?
        /// </summary>
        /// <param name="hWind"></param>
        public static void ShowIconicWindow(IntPtr hWind)
        {
            ShowWindowAsync(hWind, WindowState.SW_SHOWNORMAL);
        }

        /// <summary>
        /// get active window name (title)
        /// </summary>
        /// <returns></returns>
        public static string GetActiveWindowTitle()
        {
            var whnd = GetActiveWindowHandle();
            return GetWindowTitle(whnd);
        }
        #endregion

        #region Func<>

        /// <summary>
        /// get window name compare method (Func)
        /// </summary>
        /// <param name="compareType"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Func<string, string, bool> GetWindowNameCompareMethod(string compareType)
        {
            Func<string, string, bool> ret;
            switch (compareType.ToLower())
            {
                case "starts with":
                    ret = (a, b) => a.StartsWith(b);
                    break;
                case "ends with":
                    ret = (a, b) => a.EndsWith(b);
                    break;
                case "exact match":
                    ret = (a, b) => (a == b);
                    break;
                case "contains":
                    ret = (a, b) => a.Contains(b);
                    break;
                default:
                    throw new Exception("Search method " + compareType + " is not support.");
            }
            return ret;
        }

        /// <summary>
        /// Get Window Match Function
        /// </summary>
        /// <param name="matchType"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static Func<List<(IntPtr, string)>, List<(IntPtr, string)>> GetWindowMatchMethod(string matchType, int index)
        {
            Func<List<(IntPtr, string)>, List<(IntPtr, string)>> ret;
            switch(matchType.ToLower())
            {
                case "first":
                    ret = (lst) =>
                    {
                        return (lst.Count > 0 ? new List<(IntPtr, string)>() { lst[0] } : throw new Exception("No Matched Windows exists."));
                    };
                    break;
                case "last":
                    ret = (lst) =>
                    {
                        return (lst.Count > 0 ? new List<(IntPtr, string)>() { lst[lst.Count - 1] } : throw new Exception("No Matched Windows exists."));
                    };
                    break;
                case "all":
                    ret = (lst) =>
                    {
                        return (lst.Count > 0 ? new List<(IntPtr, string)>(lst) : throw new Exception("No Matched Windows exists."));
                    };
                    break;
                case "index":
                    ret = (lst) =>
                    {
                        var count = lst.Count;
                        if (count == 0)
                        {
                            throw new Exception("No Matched Windows exists.");
                        }
                        if (index < 0)
                        {
                            index += count;
                        }
                        if (index >= 0 && index < count)
                        {
                            return new List<(IntPtr, string)>() { lst[index] };
                        }
                        else
                        {
                            throw new Exception("No Item Exists. Index: " + index);
                        }
                    };
                    break;
                default:
                    throw new Exception("Match type " + matchType + " is not support.");
            }
            return ret;
        }

        /// <summary>
        /// get window name search method (Func)
        /// </summary>
        /// <param name="window"></param>
        /// <param name="searchMethod"></param>
        /// <param name="matchType"></param>
        /// <param name="index"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        private static Func<List<(IntPtr, string)>> GetWindowSearchMethod(string window, string searchMethod, string matchType, int index, Engine.AutomationEngineInstance engine)
        {
            if (window == engine.engineSettings.CurrentWindowKeyword)
            {
                // current window
                var whnd = GetActiveWindowHandle();
                var title = GetWindowTitle(whnd);

                return new Func<List<(IntPtr, string)>>(() =>
                {
                    return new List<(IntPtr, string)>() { (whnd, title) };
                });
            }
            else if ((window == engine.engineSettings.DesktopKeyword) ||
                     (window == VariableNameControls.GetWrappedVariableName(SystemVariables.Window_Desktop.VariableName, engine)))
            {
                // Desktop
                var whnd = GetDesktopWindow();
                return new Func<List<(IntPtr, string)>>(() =>
                {
                    return new List<(IntPtr, string)>() { (whnd, "") };
                });
            }
            else if ((window == engine.engineSettings.AllWindowsKeyword) ||
                     (window == VariableNameControls.GetWrappedVariableName(SystemVariables.Window_AllWindows.VariableName, engine)))
            {
                // all windows & match-type
                return new Func<List<(IntPtr, string)>>(() =>
                {
                    var matchedWindows = GetAllWindowNamesAndHandles();
                    var matchFunc = GetWindowMatchMethod(matchType, index);
                    return matchFunc(matchedWindows);
                });
            }
            else
            {
                // matched windows
                return new Func<List<(IntPtr, string)>>(() => {
                    var wins = GetAllWindowNamesAndHandles();
                    var searchFunc = GetWindowNameCompareMethod(searchMethod);

                    var matchedWindows = wins.Where(w => searchFunc(w.Item2, window)).ToList();

                    var matchFunc = GetWindowMatchMethod(matchType, index);
                    return matchFunc(matchedWindows);
                });
            }
        }

        #endregion

        #region methods

        /// <summary>
        /// get current window name
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentWindowName()
        {
            var whnd = GetActiveWindowHandle();
            return GetWindowTitle(whnd);
        }

        /// <summary>
        /// find window handle from specified from args
        /// </summary>
        /// <param name="windowName"></param>
        /// <param name="searchMethod"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static IntPtr FindWindowHandle(string windowName, string searchMethod, Automation.Engine.AutomationEngineInstance engine)
        {
            if (windowName == engine.engineSettings.CurrentWindowKeyword)
            {
                return GetActiveWindowHandle();
            }
            else
            {
                var wins = GetAllWindowNamesAndHandles();
                var method = GetWindowNameCompareMethod(searchMethod);

                try
                {
                    var whnd = wins.Where(w => method(w.Item2, windowName)).First();
                    return whnd.Item1;
                }
                catch
                {
                    // not found
                    throw new Exception("Window Name '" + windowName + "' not found");
                }
            }
        }
    
        /// <summary>
        /// get all window titles
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllWindowTitles()
        {
            return GetAllWindowNamesAndHandles().Select(w => w.Item2).ToList();
        }

        /// <summary>
        /// get all window handles
        /// </summary>
        /// <returns></returns>
        public static List<IntPtr> GetAllWindowHandles()
        {
            return GetAllWindowNamesAndHandles().Select(w => w.Item1).ToList();
        }

        /// <summary>
        /// get all window names for frmCommandEditor ComboBox
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="addCurrentWindow"></param>
        /// <param name="addAllWindows"></param>
        /// <param name="addDesktop"></param>
        /// <returns></returns>
        public static List<string> GetAllWindowTitles(ApplicationSettings settings, bool addCurrentWindow = true, bool addAllWindows = false, bool addDesktop = false)
        {
            var lst = new List<string>();

            if (addCurrentWindow)
            {
                lst.Add(VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentWindowName.VariableName, settings));
            }

            // TODO: use system variable like keyword
            if (addAllWindows)
            {
                lst.Add(settings?.EngineSettings.AllWindowsKeyword ?? "All Windows");
            }
            
            if (addDesktop)
            {
                lst.Add(settings?.EngineSettings.DesktopKeyword ?? "Desktop");
            }

            lst.AddRange(GetAllWindowTitles());

            return lst;
        }

        /// <summary>
        /// Activate Window
        /// </summary>
        /// <param name="handle"></param>
        public static void ActivateWindow(IntPtr handle)
        {
            if (IsIconic(handle))
            {
                SetWindowState(handle, WindowState.SW_SHOWNORMAL);
            }
            SetToForegroundWindow(handle);
        }

        /// <summary>
        /// convert processId to Window Handle
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static IntPtr ConvertProcessIdToWindowHandle(int pid)
        {
            var whnds = GetAllWindowHandles();
            foreach(var whnd in whnds)
            {
                int myPid;
                GetWindowThreadProcessId(whnd, out myPid);
                if (myPid == pid)
                {
                    return whnd;
                }
            }
            throw new Exception("ProcessID: " + pid + " does not found.");
        }
        
        //public static Bitmap CaptureWindow(string windowName, Engine.AutomationEngineInstance engine)
        //{
        //    IntPtr hWnd;
        //    if (windowName == "Desktop")
        //    {
        //        hWnd = GetDesktopWindow();
        //    }
        //    else
        //    {
        //        //hWnd = FindWindow(windowName);
        //        var wins = FindWindows(windowName, "", "", 0, 60, engine);
        //        hWnd = wins[0].Item1;

        //        SetWindowState(hWnd, WindowState.SW_RESTORE);
        //        SetToForegroundWindow(hWnd);
        //    }

        //    var rect = new RECT();

        //    //sleep to allow repaint
        //    System.Threading.Thread.Sleep(500);

        //    GetWindowRect(hWnd, out rect);
        //    var bounds = new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
        //    var screenshot = new Bitmap(bounds.Width, bounds.Height);

        //    using (var graphics = Graphics.FromImage(screenshot))
        //    {
        //        graphics.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
        //    }

        //    return screenshot;
        //}

        /// <summary>
        /// capture window
        /// </summary>
        /// <param name="whnd"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Bitmap CaptureWindow(IntPtr whnd)
        {
            if (GetWindowRect(whnd, out RECT r))
            {
                var bounds = new Rectangle(r.left, r.top, r.right - r.left, r.bottom - r.top);
                var screenshot = new Bitmap(bounds.Width, bounds.Height);

                using (var graphics = Graphics.FromImage(screenshot))
                {
                    graphics.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                }

                return screenshot;
            }
            else
            {
                throw new Exception($"Fail Capture Window. WindowHandle: {whnd}");
            }
        }

        /// <summary>
        /// search & wait window name. this method use argument values, DON'T convert variable.
        /// </summary>
        /// <param name="window"></param>
        /// <param name="searchMethod"></param>
        /// <param name="matchType"></param>
        /// <param name="index"></param>
        /// <param name="waitTime"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static List<(IntPtr, string)> FindWindows(string window, string searchMethod, string matchType, int index, int waitTime, Engine.AutomationEngineInstance engine)
        {
            var searchFunc = GetWindowSearchMethod(window, searchMethod, matchType, index, engine);

            var waitFunc = new Func<(bool, object)>(() =>
            {
                try
                {
                    var ret = searchFunc();
                    if (ret.Count > 0)
                    {
                        return (true, ret);
                    }
                    else
                    {
                        return (false, null);
                    }
                }
                catch
                {
                    return (false, null);
                }
            });

            var obj = WaitControls.WaitProcess(waitTime, "Window", waitFunc, engine);

            if (obj is List<(IntPtr, string)> lst)
            {
                return lst;
            }
            else
            {
                throw new Exception("Strange Value returned in FindWindows. Type: " + obj.GetType().FullName);
            }
        }

        /// <summary>
        /// search & wait window name. this method use argument values, convert variable.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="windowName"></param>
        /// <param name="compareMethodName"></param>
        /// <param name="matchTypeName"></param>
        /// <param name="indexName"></param>
        /// <param name="waitName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static List<(IntPtr, string)> FindWindows(ScriptCommand command, string windowName, string compareMethodName, string matchTypeName, string indexName, string waitName, Engine.AutomationEngineInstance engine)
        {
            var window = command.ExpandValueOrUserVariableAsWindowName(windowName, engine);
            var compareMethod = command.ExpandValueOrUserVariableAsSelectionItem(compareMethodName, engine);
            var matchType = command.ExpandValueOrUserVariableAsSelectionItem(matchTypeName, engine);
            var index = command.ExpandValueOrUserVariableAsInteger(indexName, engine);
            var waitTime = command.ExpandValueOrUserVariableAsInteger(waitName, engine);

            return FindWindows(window, compareMethod, matchType, index, waitTime, engine);
        }

        /// <summary>
        /// search & wait window name. this method use argument values, convert variable.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="windowName"></param>
        /// <param name="compareMethodName"></param>
        /// <param name="waitName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static List<(IntPtr, string)> FindWindows(ScriptCommand command, string windowName, string compareMethodName, string waitName, Engine.AutomationEngineInstance engine)
        {
            var window = command.ExpandValueOrUserVariableAsWindowName(windowName, engine);
            var compareMethod = command.ExpandValueOrUserVariableAsSelectionItem(compareMethodName, engine);
            var waitTime = command.ExpandValueOrUserVariableAsInteger(waitName, engine);

            return FindWindows(window, compareMethod, "All", 60, waitTime, engine);
        }

        /// <summary>
        /// general window action. This method search window before execute actionFunc, and try store Found Window Name and Handle after execute actionFunc.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="matchType"></param>
        /// <param name="searchFunc"></param>
        /// <param name="engine"></param>
        /// <param name="actionFunc"></param>
        /// <param name="nameResultName"></param>
        /// <param name="handleResultName"></param>
        /// <param name="errorFunc"></param>
        private static void WindowAction(ScriptCommand command, string matchType, Func<List<(IntPtr, string)>> searchFunc, Engine.AutomationEngineInstance engine, Action<List<(IntPtr, string)>> actionFunc, string nameResultName = "", string handleResultName = "", Action<Exception> errorFunc = null)
        {
            try
            {
                var wins = searchFunc();
                actionFunc(wins);

                matchType = matchType.ToLower();

                if (!string.IsNullOrEmpty(nameResultName))
                {
                    var nameResult = command.GetRawPropertyValueAsString(nameResultName, "Window Name Result");
                    if (!string.IsNullOrEmpty(nameResult))
                    {
                        if (matchType == "all")
                        {
                            wins.Select(w => w.Item2).ToList().StoreInUserVariable(engine, nameResult);
                        }
                        else
                        {
                            wins[0].Item2.StoreInUserVariable(engine, nameResult);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(handleResultName))
                {
                    var handleResult = command.GetRawPropertyValueAsString(handleResultName, "Window Handle Result");
                    if (!string.IsNullOrEmpty(handleResult))
                    {
                        if (matchType == "all")
                        {
                            wins.Select(w => w.Item1.ToString()).ToList().StoreInUserVariable(engine, handleResult);
                        }
                        else
                        {
                            //wins[0].Item1.ToString().StoreInUserVariable(engine, handleResult);
                            wins[0].Item1.StoreInUserVariable(engine, handleResult);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (errorFunc == null)
                {
                    throw ex;
                }
                else
                {
                    errorFunc(ex);
                }
            }
        }

        /// <summary>
        /// general window action. This method search window before execute actionFunc, and try store Found Window Name and Handle after execute actionFunc.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="windowName"></param>
        /// <param name="compareMethodName"></param>
        /// <param name="matchTypeName"></param>
        /// <param name="indexName"></param>
        /// <param name="waitName"></param>
        /// <param name="engine"></param>
        /// <param name="actionFunc"></param>
        /// <param name="nameResultName"></param>
        /// <param name="handleResultName"></param>
        /// <param name="errorFunc"></param>
        private static void WindowAction(ScriptCommand command, string windowName, string compareMethodName, string matchTypeName, string indexName, string waitName, Engine.AutomationEngineInstance engine, Action<List<(IntPtr, string)>> actionFunc, string nameResultName = "", string handleResultName = "", Action<Exception> errorFunc = null)
        {
            var matchType = command.ExpandValueOrUserVariableAsSelectionItem(matchTypeName, engine);

            WindowAction(command, matchType, new Func<List<(IntPtr, string)>>(() =>
            {
                return FindWindows(command, windowName, compareMethodName, matchTypeName, indexName, waitName, engine);
            }), engine, actionFunc, nameResultName, handleResultName, errorFunc);
        }

        /// <summary>
        /// general window action. This method search window before execute actionFunc, and try store Found Window Name and Handle after execute actionFunc.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="windowName"></param>
        /// <param name="compareMethodName"></param>
        /// <param name="waitName"></param>
        /// <param name="engine"></param>
        /// <param name="actionFunc"></param>
        /// <param name="nameResultName"></param>
        /// <param name="handleResultName"></param>
        /// <param name="errorFunc"></param>
        private static void WindowAction(ScriptCommand command, string windowName, string compareMethodName, string waitName, Engine.AutomationEngineInstance engine, Action<List<(IntPtr, string)>> actionFunc, string nameResultName = "", string handleResultName = "", Action<Exception> errorFunc = null)
        {
            WindowAction(command, "all", new Func<List<(IntPtr, string)>>(() =>
            {
                return FindWindows(command, windowName, compareMethodName, waitName, engine);
            }), engine, actionFunc, nameResultName, handleResultName, errorFunc);
        }

        /// <summary>
        /// window general action. This method search window before execute actionFunc, and try store Found Window Name and Handle after execute actionFunc. This method specifies the parameter names from the value of PropertyVirtualProperty
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <param name="actionFunc"></param>
        /// <param name="errorFunc"></param>
        public static void WindowAction(ScriptCommand command, Engine.AutomationEngineInstance engine, Action<List<(IntPtr, string)>> actionFunc, Action<Exception> errorFunc = null)
        {
            var props = command.GetParameterProperties();
            var windowName = props.GetProperty(new PropertyVirtualProperty(nameof(WindowControls), nameof(v_WindowName)))?.Name ?? "";
            var compareMethod = props.GetProperty(new PropertyVirtualProperty(nameof(WindowControls), nameof(v_CompareMethod)))?.Name ?? "";
            var waitTime = props.GetProperty(new PropertyVirtualProperty(nameof(WindowControls), nameof(v_WaitTime)))?.Name ?? "";
            var nameResult = props.GetProperty(new PropertyVirtualProperty(nameof(WindowControls), nameof(v_WindowNameResult)))?.Name ?? "";
            var handleResult = props.GetProperty(new PropertyVirtualProperty(nameof(WindowControls), nameof(v_OutputWindowHandle)))?.Name ?? "";

            var matchType = props.GetProperty(new PropertyVirtualProperty(nameof(WindowControls), nameof(v_MatchMethod)))?.Name ??
                                props.GetProperty(new PropertyVirtualProperty(nameof(WindowControls), nameof(v_MatchMethod_Single)))?.Name ?? "";
            var index = props.GetProperty(new PropertyVirtualProperty(nameof(WindowControls), nameof(v_TargetWindowIndex)))?.Name ?? "";

            if (matchType == "")
            {
                WindowAction(command, windowName, compareMethod, waitTime, engine, actionFunc, nameResult, handleResult, errorFunc);
            }
            else
            {
                WindowAction(command, windowName, compareMethod, matchType, index, waitTime, engine, actionFunc, nameResult, handleResult, errorFunc);
            }
        }

        /// <summary>
        /// window general action. This method search window before execute actionFunc, and try store Found Window Name and Handle after execute actionFunc. This method specifies the parameter names from interface
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <param name="actionFunc"></param>
        /// <param name="errorFunc"></param>
        public static void WindowAction(AWindowNameCommand command, Engine.AutomationEngineInstance engine, Action<List<(IntPtr, string)>> actionFunc, Action<Exception> errorFunc = null)
        {
            WindowAction(command, 
                nameof(command.v_WindowName), nameof(command.v_CompareMethod), nameof(command.v_MatchMethod), 
                nameof(command.v_TargetWindowIndex), nameof(command.v_WaitTimeForWindow), engine, actionFunc, 
                nameof(command.v_NameResult), nameof(command.v_HandleResult), errorFunc);
        }

        /// <summary>
        /// window general action. This method search window before execute actionFunc, and try store Found Window Name and Handle after execute actionFunc. This method specifies the parameter names from interface
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <param name="actionFunc"></param>
        /// <param name="errorFunc"></param>
        public static void WindowAction(AAnyWindowNameCommand command, Engine.AutomationEngineInstance engine, Action<List<(IntPtr, string)>> actionFunc, Action<Exception> errorFunc = null)
        {
            WindowAction(command, 
                nameof(command.v_WindowName), nameof(command.v_CompareMethod), nameof(command.v_WaitTimeForWindow), 
                engine, actionFunc, nameof(command.v_NameResult), nameof(command.v_HandleResult), errorFunc);
        }

        /// <summary>
        /// inner window handle action
        /// </summary>
        /// <param name="handleValue"></param>
        /// <param name="waitTime"></param>
        /// <param name="engine"></param>
        /// <param name="actionFunc"></param>
        /// <param name="errorFunc"></param>
        private static void WindowHandleAction(string handleValue, int waitTime, Engine.AutomationEngineInstance engine, Action<IntPtr> actionFunc, Action<Exception> errorFunc = null)
        {
            try
            {
                var whnd = handleValue.ExpandUserVariableAsWindowHandle(engine);

                var obj = WaitControls.WaitProcess(waitTime, "Window Handle", new Func<(bool, object)>(() =>
                {
                    if (IsWindow(whnd))
                    {
                        return (true, whnd);
                    }
                    else
                    {
                        return (false, null);
                    }
                    
                }), engine);

                if (obj is IntPtr ptr)
                {
                    actionFunc(ptr);
                }
            }
            catch (Exception ex)
            {
                if (errorFunc != null)
                {
                    errorFunc(ex);
                }
                else
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// window handle action. specified Window Handle, Wait Time parameter names
        /// </summary>
        /// <param name="command"></param>
        /// <param name="handleName"></param>
        /// <param name="waitTimeName"></param>
        /// <param name="engine"></param>
        /// <param name="actionFunc"></param>
        /// <param name="errorFunc"></param>
        public static void WindowHandleAction(ScriptCommand command, string handleName, string waitTimeName, Engine.AutomationEngineInstance engine, Action<IntPtr> actionFunc, Action<Exception> errorFunc = null)
        {
            //var handle = command.ExpandValueOrUserVariable(handleName, "Window Handle", engine);
            var handle = command.GetRawPropertyValueAsString(handleName, "Window Handle");
            var waitTime = command.ExpandValueOrUserVariableAsInteger(waitTimeName, "Wait Time", engine);
            WindowHandleAction(handle, waitTime, engine, actionFunc, errorFunc);
        }

        /// <summary>
        /// general window handle action. Infer parameter names from VirtualProperty
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <param name="actionFunc"></param>
        /// <param name="errorFunc"></param>
        public static void WindowHandleAction(ScriptCommand command, Engine.AutomationEngineInstance engine, Action<IntPtr> actionFunc, Action<Exception> errorFunc = null)
        {
            var props = command.GetParameterProperties();
            var handleName = props.GetProperty(new PropertyVirtualProperty(nameof(WindowControls), nameof(v_InputWindowHandle)))?.Name ?? "";
            var waitTimeName = props.GetProperty(new PropertyVirtualProperty(nameof(WindowControls), nameof(v_WaitTime)))?.Name ?? "";
            WindowHandleAction(command, handleName, waitTimeName, engine, actionFunc, errorFunc);
        }
        

        #endregion

        #region variable

        /// <summary>
        /// expand variable as Window Name
        /// </summary>
        /// <param name="value"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        private static string ExpandValueOrUserVariableAsWindowName(this string value, Engine.AutomationEngineInstance engine)
        {
            if ((value == engine.engineSettings.CurrentWindowKeyword) || 
                (value == engine.engineSettings.AllWindowsKeyword) ||
                (value == engine.engineSettings.DesktopKeyword) ||
                (value == VariableNameControls.GetWrappedVariableName(SystemVariables.Window_AllWindows.VariableName, engine)) ||
                (value == VariableNameControls.GetWrappedVariableName(SystemVariables.Window_Desktop.VariableName, engine)))
            {
                return value;
            }
            else
            {
                return value.ExpandValueOrUserVariable(engine);
            }
        }

        /// <summary>
        /// expand specified property value as Window Name 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="windowName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        private static string ExpandValueOrUserVariableAsWindowName(this ScriptCommand command, string windowName, Engine.AutomationEngineInstance engine)
        {
            var prop = command.GetProperty(windowName);
            var value = prop.GetValue(command)?.ToString() ?? "";
            return value.ExpandValueOrUserVariableAsWindowName(engine);
        }

        /// <summary>
        /// expand variable as WindowHandle specified by parameter value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static IntPtr ExpandUserVariableAsWindowHandle(this string value, Engine.AutomationEngineInstance engine)
        {
            var v = value.GetRawVariable(engine);
            var vv = v.VariableValue;
            string handleStr;
            if (vv is string str)
            {
                handleStr = str;
            }
            else if (vv is List<string> lst)
            {
                handleStr = lst[v.CurrentPosition];
            }
            else
            {
                throw new Exception($"Value '{value}' is not Window Handle value type.");
            }

            if (int.TryParse(handleStr, out int whnd))
            {
                return (IntPtr)whnd;
            }
            else
            {
                throw new Exception($"Value '{value}' is not Window Handle value type.");
            }
        }

        /// <summary>
        /// expand variable as WindowHanle specified by parameter name
        /// </summary>
        /// <param name="command"></param>
        /// <param name="windowName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static IntPtr ExpandUserVariableAsWindowHandle(this ScriptCommand command, string windowName, Engine.AutomationEngineInstance engine)
        {
            var prop = command.GetProperty(windowName);
            var value = prop.GetValue(command)?.ToString() ?? "";
            return value.ExpandUserVariableAsWindowHandle(engine);
        }

        /// <summary>
        /// store IntPtr In User Variable
        /// </summary>
        /// <param name="value"></param>
        /// <param name="engine"></param>
        /// <param name="targetVariable"></param>
        public static void StoreInUserVariable(this IntPtr value, Engine.AutomationEngineInstance engine, string targetVariable)
        {
            ExtensionMethods.StoreInUserVariable(targetVariable, value.ToString(), engine, false);
        }

        #endregion

        #region Events

        public static void lnkWindowNameUpToDate_Click(object sender, EventArgs e)
        {
            ComboBox cmb = (ComboBox)((CommandItemControl)sender).Tag;
            string currentText = cmb.Text;

            cmb.BeginUpdate();
            cmb.Items.Clear();

            var winList = GetAllWindowTitles();

            cmb.Items.AddRange(winList.ToArray());

            cmb.EndUpdate();

            if (winList.Contains(currentText))
            {
                cmb.Text = currentText;
            }
        }

        public static void MatchMethodComboBox_SelectionChangeCommitted(Dictionary<string, Control> controlsList, ComboBox matchMethodComboBox, string indexParameterName)
        {
            string item = matchMethodComboBox.SelectedItem?.ToString().ToLower() ?? "";
            FormUIControls.SetVisibleParameterControlGroup(controlsList, indexParameterName, (item == "index"));
        }

        #endregion
    }
}
