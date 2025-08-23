using System;
using System.Runtime.InteropServices;
using System.Text;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{
    public static class EM_WindowHandlePropertiesExtentionMethods
    {
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

        /// <summary>
        /// store window title to user variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="title"></param>
        /// <param name="engine"></param>
        public static void StoreWindowTitleInUserVariable(this IWindowHandleProperties command, string title, AutomationEngineInstance engine)
        {
            title.StoreInUserVariable(engine, command.v_WindowNameResult);
        }

        /// <summary>
        /// get window handle
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static IntPtr GetWindowHandle(this IWindowHandleProperties command, AutomationEngineInstance engine, Action<IntPtr> actionFunc, Action errorFunc = null)
        {
            (var whnd, var waitTime) = command.ExpandValueOrUserVariableAsWindowHandleAndWaitTime(engine);
            var ret = WaitControls.WaitProcess(waitTime, "WindowHandle",
                new Func<(bool, object)>(() =>
                {
                    if (EM_CanHandleWindowHandleExtentionMethods.CheckWindowHandleExists(whnd))
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
                if (!string.IsNullOrEmpty(command.v_WindowNameResult))
                {
                    command.StoreWindowTitleInUserVariable(EM_CanHandleWindowHandleExtentionMethods.GetWindowName(handle), engine);
                }

                actionFunc(handle);

                return handle;
            }
            else
            {
                if (errorFunc != null)
                {
                    throw new Exception($"Window Handle does not Exists. Value: '{command.v_WindowHandle}', Expand Value: '{whnd}'");
                }
                else
                {
                    errorFunc();
                    return IntPtr.Zero;
                }
            }
        }
    }
}
