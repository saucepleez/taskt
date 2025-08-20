using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{
    public static class EM_WindowHandlePropertiesExtentionMethods
    {
        /// <summary>
        /// check window handle exists
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern bool IsWindow(IntPtr hWnd);

        /// <summary>
        /// get window title length
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowTextLengthW(IntPtr hWnd);

        /// <summary>
        /// get window title
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="text"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowTextW(IntPtr hWnd, StringBuilder text, int count);

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
            title.StoreInUserVariable(engine, nameof(command.v_WindowNameResult));
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
                actionFunc(handle);

                int titleLengthA = GetWindowTextLengthW(whnd);
                StringBuilder title = new StringBuilder(titleLengthA + 1);
                GetWindowTextW(whnd, title, title.Capacity);
                command.StoreWindowTitleInUserVariable(title.ToString(), engine);
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
