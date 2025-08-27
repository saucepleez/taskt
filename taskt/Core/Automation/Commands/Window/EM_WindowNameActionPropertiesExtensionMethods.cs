using System;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{
    public static class EM_WindowNameActionPropertiesExtensionMethods
    {
        /// <summary>
        /// window name action, and wait before execute ation
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <param name="actionFunc"></param>
        /// <param name="errorFunc"></param>
        public static void WindowNameActionAndWait(this IWindowNameActionProperties command, AutomationEngineInstance engine, Action<IntPtr, string> actionFunc, Action<Exception> errorFunc = null)
        {
            command.WindowNameAction(engine, 
                new Action<IntPtr, string>((whnd, name) =>
                {
                    command.WaitAfterFindWindowProcess(engine);
                    actionFunc(whnd, name);
                }), 
                errorFunc
            );
        }
    }
}
