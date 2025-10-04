using System;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{
    public static class EM_WindowActivatePropertiesExtensionMethods
    {
        /// <summary>
        /// window activate process
        /// </summary>
        /// <param name="command"></param>
        /// <param name="whnd"></param>
        public static void ActivateWindowProcess(this IWindowActivateProperties command, IntPtr whnd, AutomationEngineInstance engine)
        {
            if (((ScriptCommand)command).ExpandValueOrUserVariableAsYesNo(nameof(command.v_ActivateBeforeAction), engine))
            {
                var activate = new ActivateWindowByWindowHandleCommand()
                {
                    v_WindowHandle = whnd.ToString(),
                };
                activate.RunCommand(engine);
            }
        }
    }
}
