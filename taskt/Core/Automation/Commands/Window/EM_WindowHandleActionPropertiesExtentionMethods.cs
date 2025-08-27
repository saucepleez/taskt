using System;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{
    public static class EM_WindowHandleActionPropertiesExtentionMethods
    {
        ///// <summary>
        ///// expand value or user variable as wait time between find and action
        ///// </summary>
        ///// <param name="command"></param>
        ///// <param name="engine"></param>
        ///// <returns></returns>
        //public static int ExpandValueOrUserVariableAsWaitTimeBetweenFindAndAction(this IWindowHandleActionPropeties command, AutomationEngineInstance engine)
        //{
        //    return ((ScriptCommand)command).ExpandValueOrUserVariableAsInteger(nameof(command.v_WaitTimeBetweenFindAndAction), engine);
        //}

        /// <summary>
        /// window handle action and wait before execute action
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static void WindowHandleActionBeforeWait(this IWindowHandleActionPropeties command, AutomationEngineInstance engine, Action<IntPtr> actionFunc, Action<Exception> errorFunc = null)
        {
            //var whnd = command.GetWindowHandle(engine);
            //var waitTime = command.ExpandValueOrUserVariableAsWaitTimeBetweenFindAndAction(engine);
            //if (waitTime > 0)
            //{
            //    System.Threading.Thread.Sleep(waitTime * 1000);
            //}
            command.WindowHandleAction(engine,
                new Action<IntPtr>((w) =>
                {
                    //var waitTime = command.ExpandValueOrUserVariableAsWaitTimeBetweenFindAndAction(engine);
                    //if (waitTime > 0)
                    //{
                    //    System.Threading.Thread.Sleep(waitTime * 1000);
                    //}
                    command.WaitAfterFindWindowProcess(engine);
                    actionFunc(w);
                }),
                errorFunc
            );
        }
    }
}
