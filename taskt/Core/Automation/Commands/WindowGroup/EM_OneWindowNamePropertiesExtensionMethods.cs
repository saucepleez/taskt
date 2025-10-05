using System;
using System.Linq;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{
    public static class EM_OneWindowNamePropertiesExtensionMethods
    {
        /// <summary>
        /// expand value or user variable as window index
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static int ExpandValueOrUserVariableAsWindowIndex(this IOneWindowNameProperties command, AutomationEngineInstance engine)
        {
            return ((ScriptCommand)command).ExpandValueOrUserVariableAsInteger(nameof(command.v_TargetWindowIndex), engine);
        }

        /// <summary>
        /// store window name result
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="engine"></param>
        public static void StoreWindowNameResultInUserVariable(this IOneWindowNameProperties command, string name, AutomationEngineInstance engine)
        {
            if (!string.IsNullOrEmpty(command.v_NameResult))
            {
                name.StoreInUserVariable(engine, command.v_NameResult);
            }
        }

        /// <summary>
        /// store window handle result
        /// </summary>
        /// <param name="command"></param>
        /// <param name="whnd"></param>
        /// <param name="engine"></param>
        public static void StoreWindowHandleResultInUserVariable(this IOneWindowNameProperties command, IntPtr whnd, AutomationEngineInstance engine)
        {
            if (!string.IsNullOrEmpty(command.v_HandleResult))
            {
                whnd.StoreInUserVariable(engine, command.v_HandleResult);
            }
        }

        /// <summary>
        /// wait for window name
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static (IntPtr whnd, string name) WaitForWindowName(this IOneWindowNameProperties command, AutomationEngineInstance engine)
        {
            var wins = command.WaitForWindowNames(engine);
            (IntPtr whnd, string name) windowSet = (IntPtr.Zero, null);
            switch(((ScriptCommand)command).ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_SelectionMethod), engine))
            {
                case "first":
                    windowSet = wins[0];
                    break;
                case "last":
                    windowSet = wins[wins.Count() - 1];
                    break;
                case "index":
                    var idx = command.ExpandValueOrUserVariableAsWindowIndex(engine);
                    if (idx < 0)
                    {
                        idx += wins.Count();
                    }
                    if (idx >= 0 && idx < wins.Count())
                    {
                        windowSet = wins[idx];
                    }
                    else
                    {
                        throw new Exception($"Strange Window Index. Value: '{command.v_TargetWindowIndex}_', Expand Value: '{idx}', Windows Count: '{wins.Count()}'");
                    }
                    break;
            }
            return windowSet;
        }

        /// <summary>
        /// window name action
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <param name="actionFunc"></param>
        /// <param name="errorFunc"></param>
        public static void WindowNameAction(this IOneWindowNameProperties command, AutomationEngineInstance engine, Action<IntPtr, string> actionFunc, Action<Exception> errorFunc = null)
        {
            try
            {
                (var whnd, var name) = command.WaitForWindowName(engine);
                actionFunc(whnd, name);
                command.StoreWindowNameResultInUserVariable(name, engine);
                command.StoreWindowHandleResultInUserVariable(whnd, engine);
            }
            catch (Exception ex)
            {
                if (errorFunc != null)
                {
                    errorFunc(ex);
                }
                else
                {
                    throw ex;
                }
            }
        }
    }
}
