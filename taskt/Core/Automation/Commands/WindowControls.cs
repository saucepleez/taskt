using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.TextGroup;
using taskt.Core.Automation.Engine;
using taskt.UI.CustomControls;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for window methods
    /// </summary>
    internal static class WindowControls
    {

        #region fields

        /// <summary>
        /// internal current window keyword
        /// </summary>
        public const string INTERNAL_CURRENT_WINDOW_KEYWORD = "%kwd_current_window%";
        /// <summary>
        /// internal Desktop keyword
        /// </summary>
        public const string INTERNAL_DESKTOP_KEYWORD = "%kwd_desktop%";
        /// <summary>
        /// internal all windows keyword
        /// </summary>
        public const string INTERNAL_ALL_WINDOWS_KEYWORD = "%kwd_all_windows%";
        /// <summary>
        /// internal current window position keyword
        /// </summary>
        public const string INTERNAL_CURRENT_WINDOW_POSITION_KEYWORD = "%kwd_current_window_position%";
        /// <summary>
        /// internal current window X position keyword
        /// </summary>
        public const string INTERNAL_CURRENT_WINDOW_X_POSITION_KEYWORD = "%kwd_current_window_xposition%";
        /// <summary>
        /// internal current window Y position keyword
        /// </summary>
        public const string INTERNAL_CURRENT_WINDOW_Y_POSITION_KEYWORD = "%kwd_current_window_yposition%";
        /// <summary>
        /// internal current window size keyword
        /// </summary>
        public const string INTERNAL_CURRENT_WINDOW_SIZE_KEYWORD = "%kwd_current_window_size%";
        /// <summary>
        /// internal current window width keyword
        /// </summary>
        public const string INTERNAL_CURRENT_WINDOW_WIDTH_KEYWORD = "%kwd_current_window_width%";
        /// <summary>
        /// internal current window height keyword
        /// </summary>
        public const string INTERNAL_CURRENT_WINDOW_HEIGHT_KEYWORD = "%kwd_current_window_height%";
        /// <summary>
        /// internal current window handle keyword
        /// </summary>
        public const string INTERNAL_CURRENT_WINDOW_HANDLE_KEYWORD = "%kwd_current_window_handle%";
        #endregion

        #region virtualproperty

        /// <summary>
        /// windows name property
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Window Name")]
        [InputSpecification("Window Name", true)]
        [PropertyDetailSampleUsage("**Untitled - Notepad**", "Specify the **Notepad**")]
        [PropertyDetailSampleUsage("**%kwd_current_window%**", "Specify the Current Activate Window")]
        [PropertyDetailSampleUsage("**{{{vWindow}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Window Name")]
        [PropertyCustomUIHelper("Up-to-date", nameof(WindowControls) + "+" + nameof(WindowControls.lnkWindowNameUpToDate_Click))]
        [PropertyIsWindowNamesList(true)]
        [PropertyValidationRule("Window Name", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Window Name")]
        public static string v_WindowName { get; }

        /// <summary>
        /// windows name check(search) method
        /// </summary>
        [PropertyVirtualProperty(nameof(VP_TextCheckMethodControls), nameof(VP_TextCheckMethodControls.v_CheckMethod))]
        [PropertyDescription("Check Method for the Window Name")]
        [PropertyIsOptional(true, "Contains")]
        [PropertyDisplayText(true, "Check Method")]
        public static string v_CheckMethod { get; }

        /// <summary>
        /// window check case sensitive
        /// </summary>
        [PropertyVirtualProperty(nameof(VP_TextCheckMethodControls), nameof(VP_TextCheckMethodControls.v_CaseSensitiveNo))]
        [PropertyDescription("Case Sensitive Checking for Window Names")]
        public static string v_CaseSensitive { get; }

        /// <summary>
        /// trim before check window name
        /// </summary>
        [PropertyVirtualProperty(nameof(VP_TextCheckMethodControls), nameof(VP_TextCheckMethodControls.v_TrimBeforeCheck))]
        [PropertyDescription("Trim before Check Window Names")]
        public static string v_TrimBeforeCheck { get; }

        /// <summary>
        /// match method get one window, please specify PropertySelectionChangeEvent
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Selection Method for the Window Name")]
        [PropertyUISelectionOption("First")]
        [PropertyUISelectionOption("Last")]
        [PropertyUISelectionOption("Index")]
        [PropertyDetailSampleUsage("**First**", "Specify the First Window")]
        [PropertyDetailSampleUsage("**Last**", "Specify the Last Window")]
        [PropertyDetailSampleUsage("**Index**", "the Window specifed by Index. **0** means First Window")]
        [Remarks("Specify when there are Multiple Matching Windows")]
        [PropertyIsOptional(true, "First")]
        [PropertyDisplayText(true, "Select")]
        public static string v_SelectionMethod_Single { get; }

        /// <summary>
        /// window index for match
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Window Index")]
        [InputSpecification("Window Index", true)]
        [PropertyDetailSampleUsage("**0**", "Specify the First Window")]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Window Index")]
        [PropertyDetailSampleUsage("**{{{vIndex}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Window Index")]
        [PropertyIsOptional(true, "0")]
        [PropertyFirstValue("0")]
        [PropertyDisplayText(true, "Window Index")]
        public static string v_TargetWindowIndex { get; }

        /// <summary>
        /// window wait time
        /// </summary>
        [PropertyVirtualProperty(nameof(WaitControls), nameof(WaitControls.v_WaitTime))]
        [PropertyDescription("Wait Time for the Window to Exist (sec)")]
        [Remarks("Specify how long to Wait before an Error will occur because the Window is Not Found.")]
        [PropertyIsOptional(true, "60")]
        [PropertyFirstValue("60")]
        public static string v_WaitTime { get; }

        /// <summary>
        /// wait time between find the window and execute action
        /// </summary>
        [PropertyVirtualProperty(nameof(WaitControls), nameof(WaitControls.v_WaitTime))]
        [PropertyDescription("Wait Time between Finding the Window and Executing Action (sec)")]
        [PropertyIsOptional(true, "0")]
        [PropertyFirstValue("0")]
        [PropertyDisplayText(false, "Wait Time Between", "s")]
        public static string v_WaitTimeBetweenFindAndAction { get; }

        ///// <summary>
        ///// window wait time allows 0
        ///// </summary>
        //[PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WaitTime))]
        //[PropertyIsOptional(true, "0")]
        //[PropertyFirstValue("0")]
        //[PropertyValidationRule("Wait Time", PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        //public static string v_ZeroWaitTime { get; }

        /// <summary>
        /// window name result
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Window Name Result")]
        [InputSpecification("Variable Name", true)]
        [PropertyDetailSampleUsage("**vWin**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vWin}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyIsOptional(true)]
        [Remarks("When Match Method is **All**, data type is LIST, otherwise it is BASIC")]
        [PropertyValidationRule("Window Name Result", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "Window Name Result")]
        public static string v_WindowNameResult { get; }

        /// <summary>
        /// output window handle result
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Window Handle Result")]
        [InputSpecification("Variable Name", true)]
        [PropertyDetailSampleUsage("**vHandle**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vHandle}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [Remarks("When Match Method is **All**, data type is LIST, otherwise it is BASIC")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Window Handle Result", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "Window Handle Result")]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.WindowHandle, true)]
        public static string v_OutputWindowHandle { get; }

        /// <summary>
        /// input window handle result
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_InputInstanceName))]
        [PropertyDescription("Window Handle Variable Name")]
        [InputSpecification("Variable Name", true)]
        [PropertyDetailSampleUsage("**vHandle**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vHandle}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.WindowHandle, true)]
        [PropertyValidationRule("Window Handle Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Window Handle")]
        public static string v_InputWindowHandle { get; }

        /// <summary>
        /// input window width
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Window Width (Pixcel)")]
        [InputSpecification("Window Width", true)]
        [PropertyDetailSampleUsage("**640**", PropertyDetailSampleUsage.ValueType.Value, "Width")]
        [PropertyDetailSampleUsage("**{{{vWidth}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Width")]
        [PropertyDetailSampleUsage("**%kwd_current_window_size%**", "Specify Current Window Width for Window Width")]
        [PropertyDetailSampleUsage("**%kwd_current_window_width%**", "Specify Current Window Width for Window Width", false)]
        [PropertyValidationRule("Width", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.EqualsZero | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(true, "Width")]
        [PropertyAvailableSystemVariable(SystemVariables.LimitedSystemVariableNames.Window_Size)]
        public static string v_InputWidth { get; }

        /// <summary>
        /// input window height
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Window Height (Pixcel)")]
        [InputSpecification("Window Height", true)]
        [PropertyDetailSampleUsage("**480**", PropertyDetailSampleUsage.ValueType.Value, "Height")]
        [PropertyDetailSampleUsage("**{{{vHeight}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Height")]
        [PropertyDetailSampleUsage("**%kwd_current_window_size%**", "Specify Current Window Height for Window Height")]
        [PropertyDetailSampleUsage("**%kwd_current_window_height%**", "Specify Current Window Height for Window Height", false)]
        [PropertyValidationRule("Height", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.EqualsZero | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyAvailableSystemVariable(SystemVariables.LimitedSystemVariableNames.Window_Size)]
        [PropertyDisplayText(true, "Height")]
        public static string v_InputHeight { get; }

        /// <summary>
        /// input window X position
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("X horizontal coordinate (pixel) for the Window's Location")]
        [InputSpecification("X Window Location", true)]
        [PropertyDetailSampleUsage("**0**", "Specify X Top Position")]
        [PropertyDetailSampleUsage("**100**", PropertyDetailSampleUsage.ValueType.Value, "X Position")]
        [PropertyDetailSampleUsage("**{{{vXPos}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "X Position")]
        [PropertyDetailSampleUsage("**%kwd_current_window_position%**", "Specify Current Position for X Position")]
        [PropertyDetailSampleUsage("**%kwd_current_window_xposition%**", "Specify Current X Position for X Position", false)]
        [PropertyDetailSampleUsage("**%kwd_current_window_yposition%**", "Specify Current Y Position for X Position", false)]
        [Remarks("This number is the pixel location on screen. Maximum value should be the maximum value allowed by your resolution. For 1920x1080, the valid range could be 0-1920")]
        [PropertyValidationRule("X Position", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyAvailableSystemVariable(SystemVariables.LimitedSystemVariableNames.Window_Position)]
        [PropertyDisplayText(true, "X Position")]
        public static string v_InputXPosition { get; }

        /// <summary>
        /// intpu window Y position
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Y vertical coordinate (pixel) for the Window's Location")]
        [InputSpecification("Y Window Location", true)]
        [PropertyDetailSampleUsage("**0**", "Specify Y Left Position")]
        [PropertyDetailSampleUsage("**100**", PropertyDetailSampleUsage.ValueType.Value, "Y Position")]
        [PropertyDetailSampleUsage("**{{{vYPos}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Y Position")]
        [PropertyDetailSampleUsage("**%kwd_current_window_position%**", "Specify Current Position for Y Position")]
        [PropertyDetailSampleUsage("**%kwd_current_window_xposition%**", "Specify Current X Position for Y Position", false)]
        [PropertyDetailSampleUsage("**%kwd_current_window_yposition%**", "Specify Current Y Position for Y Position", false)]
        [Remarks("This number is the pixel location on screen. Maximum value should be the maximum value allowed by your resolution. For 1920x1080, the valid range could be 0-1080")]
        [PropertyValidationRule("Y Position", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyAvailableSystemVariable(SystemVariables.LimitedSystemVariableNames.Window_Position)]
        [PropertyDisplayText(true, "Y Position")]
        public static string v_InputYPosition { get; }

        /// <summary>
        /// window state
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("State of the Window")]
        [PropertyUISelectionOption("Maximize")]
        [PropertyUISelectionOption("Minimize")]
        [PropertyUISelectionOption("Restore")]
        [PropertyUISelectionOption("3")]
        [PropertyUISelectionOption("2")]
        [PropertyUISelectionOption("1")]
        [PropertyDetailSampleUsage("**Maximize**", "Specifiy Maximize")]
        [PropertyDetailSampleUsage("**3**", "Specifiy Maximize")]
        [PropertyDetailSampleUsage("**2**", "Specifiy Minimize")]
        [PropertyDetailSampleUsage("**1**", "Specifiy Restore")]
        [PropertyDetailSampleUsage("**{{{vState}}}**", PropertyDetailSampleUsage.ValueType.VariableValue)]
        [InputSpecification("Window State Text or Number", true)]
        [PropertyValidationRule("Window State", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "State")]
        public static string v_WindowState { get; }

        /// <summary>
        /// when target window is minimized for Set-commands
        /// </summary>
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_ComboBoxHasErrorIgnore))]
        [PropertyDescription("When Window Is Minimized")]
        [PropertyUISelectionOption("Execute")]
        [PropertyUISelectionOption("Restore")]
        [PropertyIsOptional(true, "Restore")]
        [PropertyDisplayText(false, "When Window Is Minimized")]
        public static string v_WhenWindowIsMinimizedForSet { get; }

        /// <summary>
        /// when target window is minimized for Get-commands
        /// </summary>
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WhenWindowIsMinimizedForSet))]
        [PropertyDescription("When Window Is Minimized")]
        [PropertyUISelectionOption("Set Zero")]
        [PropertyDisplayText(false, "When Window Is Minimized")]
        public static string v_WhenWindowIsMinimizedForGet { get; }

        /// <summary>
        /// when target window is maximized for Set-commands
        /// </summary>
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WhenWindowIsMinimizedForSet))]
        [PropertyDescription("When Window Is Maximized")]
        [PropertyDisplayText(false, "When Window Is Maximized")]
        public static string v_WhenWindowIsMaximizedForSet { get; }

        /// <summary>
        /// when target window is maximized for Get-commands
        /// </summary>
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WhenWindowIsMinimizedForGet))]
        [PropertyDescription("When Window Is Maximized")]
        [PropertyDisplayText(false, "When Window Is Maximized")]
        public static string v_WhenWindowIsMaximizedForGet { get; }

        /// <summary>
        /// activate window before action
        /// </summary>
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Activate Window Before Action")]
        [PropertyIsOptional(true, "No")]
        [PropertyValidationRule("Activate Window", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "Activate Window")]
        public static string v_ActivateBeforeAction { get; }

        /// <summary>
        /// base position
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Base Position")]
        [PropertyUISelectionOption("Top Left")]
        [PropertyUISelectionOption("Bottom Right")]
        [PropertyUISelectionOption("Top Right")]
        [PropertyUISelectionOption("Bottom Left")]
        [PropertyUISelectionOption("Center")]
        [PropertyIsOptional(true, "Top Left")]
        [PropertyDisplayText(false, "Base Position")]
        public static string v_PositionBase { get; }

        #endregion


        [DllImport("user32.dll")]
        private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);


        /// <summary>
        /// convert processId to Window Handle
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static IntPtr ConvertProcessIdToWindowHandle(int pid)
        {
            var whnds = EM_CanHandleWindowHandleExtentionMethods.GetAllWindowHandles();
            foreach (var whnd in whnds)
            {
                int myPid;
                GetWindowThreadProcessId(whnd, out myPid);
                if (myPid == pid)
                {
                    return whnd;
                }
            }
            throw new Exception($"ProcessID: {pid} does not found.");
        }

        /// <summary>
        /// get all window names for frmCommandEditor ComboBox
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="addCurrentWindow"></param>
        /// <param name="addAllWindows"></param>
        /// <param name="addDesktop"></param>
        /// <returns></returns>
        public static List<string> GetAllWindowTitles(SafeApplicationSettings settings, bool addCurrentWindow = true, bool addAllWindows = false, bool addDesktop = false)
        {
            var lst = new List<string>();

            if (addCurrentWindow)
            {
                lst.Add(VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentWindowName.VariableName, settings));
            }

            if (addAllWindows)
            {
                lst.Add(VariableNameControls.GetWrappedVariableName(SystemVariables.Window_AllWindows.VariableName, settings));
            }
            
            if (addDesktop)
            {
                lst.Add(VariableNameControls.GetWrappedVariableName(SystemVariables.Window_Desktop.VariableName, settings));
            }

            lst.AddRange(EM_CanHandleWindowNameExtensionMethods.GetAllWindowNames());

            return lst;
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

        public static void lnkWindowNameUpToDate_Click(object sender, EventArgs e)
        {
            ComboBox cmb = (ComboBox)((CommandItemControl)sender).Tag;
            string currentText = cmb.Text;

            cmb.BeginUpdate();
            cmb.Items.Clear();

            var winList = EM_CanHandleWindowNameExtensionMethods.GetAllWindowNames();

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

        /// <summary>
        /// Replace Internal Keywords to SystemVariable Names
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static string ReplaceKeywordsToSystemVariable(string txt, Engine.AutomationEngineInstance engine)
        {
            return txt.Replace(INTERNAL_CURRENT_WINDOW_KEYWORD, VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentWindowName.VariableName, engine))
                        .Replace(INTERNAL_ALL_WINDOWS_KEYWORD, VariableNameControls.GetWrappedVariableName(SystemVariables.Window_AllWindows.VariableName, engine))
                        .Replace(INTERNAL_DESKTOP_KEYWORD, VariableNameControls.GetWrappedVariableName(SystemVariables.Window_Desktop.VariableName, engine))
                        .Replace(INTERNAL_CURRENT_WINDOW_SIZE_KEYWORD, VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentSize.VariableName, engine))
                        .Replace(INTERNAL_CURRENT_WINDOW_WIDTH_KEYWORD, VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentWidth.VariableName, engine))
                        .Replace(INTERNAL_CURRENT_WINDOW_HEIGHT_KEYWORD, VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentHeight.VariableName, engine))
                        .Replace(INTERNAL_CURRENT_WINDOW_POSITION_KEYWORD, VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentPosition.VariableName, engine))
                        .Replace(INTERNAL_CURRENT_WINDOW_X_POSITION_KEYWORD, VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentXPosition.VariableName, engine))
                        .Replace(INTERNAL_CURRENT_WINDOW_Y_POSITION_KEYWORD, VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentYPosition.VariableName, engine))
                        .Replace(INTERNAL_CURRENT_WINDOW_HANDLE_KEYWORD, VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentWindowHandle.VariableName, engine));
        }

        /// <summary>
        /// Replace Internal Keywords to SystemVariable Names
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static string ReplaceKeywordsToSystemVariable(string txt, IApplicationSettings settings)
        {
            return txt.Replace(INTERNAL_CURRENT_WINDOW_KEYWORD, VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentWindowName.VariableName, settings))
                        .Replace(INTERNAL_ALL_WINDOWS_KEYWORD, VariableNameControls.GetWrappedVariableName(SystemVariables.Window_AllWindows.VariableName, settings))
                        .Replace(INTERNAL_DESKTOP_KEYWORD, VariableNameControls.GetWrappedVariableName(SystemVariables.Window_Desktop.VariableName, settings))
                        .Replace(INTERNAL_CURRENT_WINDOW_SIZE_KEYWORD, VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentSize.VariableName, settings))
                        .Replace(INTERNAL_CURRENT_WINDOW_WIDTH_KEYWORD, VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentWidth.VariableName, settings))
                        .Replace(INTERNAL_CURRENT_WINDOW_HEIGHT_KEYWORD, VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentHeight.VariableName, settings))
                        .Replace(INTERNAL_CURRENT_WINDOW_POSITION_KEYWORD, VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentPosition.VariableName, settings))
                        .Replace(INTERNAL_CURRENT_WINDOW_X_POSITION_KEYWORD, VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentXPosition.VariableName, settings))
                        .Replace(INTERNAL_CURRENT_WINDOW_Y_POSITION_KEYWORD, VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentYPosition.VariableName, settings))
                        .Replace(INTERNAL_CURRENT_WINDOW_HANDLE_KEYWORD, VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentWindowHandle.VariableName, settings));
        }
    }
}
