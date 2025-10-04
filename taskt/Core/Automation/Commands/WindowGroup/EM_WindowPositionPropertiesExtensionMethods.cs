using System;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// window position properties extention methods
    /// </summary>
    public static class EM_WindowPositionPropertiesExtensionMethods
    {
        /// <summary>
        /// get window position
        /// </summary>
        /// <param name="whnd"></param>
        /// <returns>(top, left)</returns>
        public static (int, int) GetWindowPosition(IntPtr whnd)
        {
            var rect = EM_WindowRECTPropertiesExtentionMethods.GetWindowRect(whnd);
            return (rect.top, rect.left);
        }

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
            (var top, var left) = GetWindowPosition(whnd);

            if ((v == VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentPosition.VariableName, engine)) ||
                (v == VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentXPosition.VariableName, engine)))
            {
                return left;
            }
            else if (v == VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentYPosition.VariableName, engine))
            {
                return top;
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
            var v = command.v_YPosition;
            (var top, var left) = GetWindowPosition(whnd);

            if ((v == VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentPosition.VariableName, engine)) ||
                (v == VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentYPosition.VariableName, engine)))
            {
                return top;
            }
            else if (v == VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentXPosition.VariableName, engine))
            {
                return left;
            }
            else
            {
                return v.ExpandValueOrUserVariableAsInteger("Window Y Position", engine);
            }
        }

        /// <summary>
        /// move window
        /// </summary>
        /// <param name="whnd"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void MoveWindow(IntPtr whnd, int x, int y)
        {
            const uint flag = 0x0045; // 0x0001 | 0x0004 | 0x0040;
            EM_WindowRECTPropertiesExtentionMethods.SetWindowPos(whnd, 0, x, y, 0, 0, flag);
        }
    }
}
