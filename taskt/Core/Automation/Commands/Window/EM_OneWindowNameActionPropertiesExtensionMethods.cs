using System;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{
    public static class EM_OneWindowNameActionPropertiesExtensionMethods
    {
        /// <summary>
        /// window name action, and wait/activate before execute ation
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <param name="actionFunc"></param>
        /// <param name="errorFunc"></param>
        public static void WindowNameActionAndWaitActivate(this IOneWindowNameActionProperties command, AutomationEngineInstance engine, Action<IntPtr, string> actionFunc, Action<Exception> errorFunc = null)
        {
            command.WindowNameAction(engine, 
                new Action<IntPtr, string>((whnd, name) =>
                {
                    command.ActivateWindowProcess(whnd, engine);

                    command.WaitAfterFindWindowProcess(engine);
                    actionFunc(whnd, name);
                }), 
                errorFunc
            );
        }
    }
}
