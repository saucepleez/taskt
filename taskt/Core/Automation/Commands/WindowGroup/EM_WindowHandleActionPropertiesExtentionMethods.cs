using System;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{
    public static class EM_WindowHandleActionPropertiesExtentionMethods
    {
        /// <summary>
        /// window handle action and wait/activate before execute action
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static void WindowHandleActionBeforeWaitActivate(this IWindowHandleActionPropeties command, AutomationEngineInstance engine, Action<IntPtr> actionFunc, Action<Exception> errorFunc = null)
        {
            command.WindowHandleAction(engine,
                new Action<IntPtr>((w) =>
                {
                    command.ActivateWindowProcess(w, engine);

                    command.WaitAfterFindWindowProcess(engine);
                    actionFunc(w);
                }),
                errorFunc
            );
        }
    }
}
