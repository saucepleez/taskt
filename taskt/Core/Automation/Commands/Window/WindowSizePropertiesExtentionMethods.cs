using System;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for IWindowSizeProperties Extension methods
    /// </summary>
    public static class WindowSizePropertiesExtentionMethods
    {
        /// <summary>
        /// expand value or variable as Window Width
        /// </summary>
        /// <param name="command"></param>
        /// <param name="whnd"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static int ExpandValueOrVariableAsWindowWidth(this IWindowSizeProperties command, IntPtr whnd, AutomationEngineInstance engine)
        {
            var w = command.v_Width;
            var rect = WindowControls.GetWindowRect(whnd);

            if ((w == VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentSize.VariableName, engine)) ||
                 (w == VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentWidth.VariableName, engine)))
            {
                return rect.GetWidth();
            }
            else if (w == VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentHeight.VariableName, engine))
            {
                return rect.GetHeight();
            }
            else
            {
                return w.ExpandValueOrUserVariableAsInteger("Window Width", engine);
            }
        }

        /// <summary>
        /// expand value or variable as Window Height
        /// </summary>
        /// <param name="command"></param>
        /// <param name="whnd"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static int ExpandValueOrVariableAsWindowHeight(this IWindowSizeProperties command, IntPtr whnd, AutomationEngineInstance engine)
        {
            var w = command.v_Height;
            var rect = WindowControls.GetWindowRect(whnd);

            if ((w == VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentSize.VariableName, engine)) ||
                 (w == VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentHeight.VariableName, engine)))
            {
                return rect.GetHeight();
            }
            else if (w == VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentWidth.VariableName, engine))
            {
                return rect.GetWidth();
            }
            else
            {
                return w.ExpandValueOrUserVariableAsInteger("Window Height", engine);
            }
        }
    }
}
