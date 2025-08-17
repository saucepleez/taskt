using System;
using System.Runtime.InteropServices;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands.Window
{
    public static class EM_WindowHandlePropertiesExtentionMethods
    {
        /// <summary>
        /// check window handle exists
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool IsWindow(IntPtr hWnd);

        /// <summary>
        /// expand value or user variable as WindowHandle
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static IntPtr ExpandValueOrUserVariableAsWindowHandle(this IWindowHandleProperties command, AutomationEngineInstance engine)
        {
            var whnd = ((ScriptCommand)command).ExpandValueOrUserVariableAsInteger(nameof(command.v_WindowHandle), "WindowHandle", engine);
            if (whnd >= 0)
            {
                return (IntPtr)whnd;
            }
            else
            {
                throw new Exception($"Strange WindowHandle Value. Value: '{whnd}'");
            }
        }

        /// <summary>
        /// expand value or user variable as WidowHandle wait time
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static int ExpandValueOrUserVariableAsWaitTimeForWindowHandle(this IWindowHandleProperties command, AutomationEngineInstance engine)
        {
            return ((ScriptCommand)command).ExpandValueOrUserVariableAsInteger(nameof(command.v_WaitTimeForWindow), "Window Wait Time", engine);
        }

        /// <summary>
        /// expand value or user variable as WindowHandle and WaitTime for window handle
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns>(handle, wait time)</returns>
        public static (IntPtr, int) ExpandValueOrUserVariableAsWindowHandleAndWaitTime(this IWindowHandleProperties command, AutomationEngineInstance engine)
        {
            return (command.ExpandValueOrUserVariableAsWindowHandle(engine), command.ExpandValueOrUserVariableAsWaitTimeForWindowHandle(engine));
        }

        public static IntPtr GetWindowHandle(this IWindowHandleProperties command, AutomationEngineInstance engine)
        {
            (var whnd, var waitTime) = command.ExpandValueOrUserVariableAsWindowHandleAndWaitTime(engine);
            var ret = WaitControls.WaitProcess(waitTime, "WindowHandle",
                new Func<(bool, object)>(() =>
                {
                    if (IsWindow(whnd))
                    {
                        return (true, whnd);
                    }
                    else
                    {
                        return (false, null);
                    }
                }), engine
            );
            if (ret is IntPtr handle)
            {
                return handle;
            }
            else
            {
                throw new Exception($"Window Handle does not Exists. Value: '{command.v_WindowHandle}', Expand Value: '{whnd}'");
            }
        }
    }
}
