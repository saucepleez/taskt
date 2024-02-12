using System;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// window position properties extention methods
    /// </summary>
    public static class EM_WindowPositionPropertiesExtentionMethods
    {
        /// <summary>
        /// expand value or variable as Window X Position
        /// </summary>
        /// <param name="command"></param>
        /// <param name="whnd"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static int ExpandValueOrVariableAsWindowXPosition(this IWindowPositionProperties command, IntPtr whnd, AutomationEngineInstance engine)
        {
            var v = command.v_XPosition;
            var rect = WindowControls.GetWindowRect(whnd);

            if ((v == VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentPosition.VariableName, engine)) ||
                (v == VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentXPosition.VariableName, engine)) ||
                (v == engine.engineSettings.CurrentWindowPositionKeyword) ||
                (v == engine.engineSettings.CurrentWindowXPositionKeyword))
            {
                return rect.left;
            }
            else if ((v == VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentYPosition.VariableName, engine)) ||
                        (v == engine.engineSettings.CurrentWindowYPositionKeyword))
            {
                return rect.top;
            }
            else
            {
                return v.ExpandValueOrUserVariableAsInteger("Window X Position", engine);
            }
        }

        /// <summary>
        /// expand value or variable as Window Y Position
        /// </summary>
        /// <param name="command"></param>
        /// <param name="whnd"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static int ExpandValueOrVariableAsWindowYPosition(this IWindowPositionProperties command, IntPtr whnd, AutomationEngineInstance engine)
        {
            var v = command.v_XPosition;
            var rect = WindowControls.GetWindowRect(whnd);

            if ((v == VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentPosition.VariableName, engine)) ||
                (v == VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentYPosition.VariableName, engine)) ||
                (v == engine.engineSettings.CurrentWindowPositionKeyword) ||
                (v == engine.engineSettings.CurrentWindowYPositionKeyword))
            {
                return rect.top;
            }
            else if ((v == VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentXPosition.VariableName, engine)) ||
                        (v == engine.engineSettings.CurrentWindowXPositionKeyword))
            {
                return rect.left;
            }
            else
            {
                return v.ExpandValueOrUserVariableAsInteger("Window Y Position", engine);
            }
        }
    }
}
