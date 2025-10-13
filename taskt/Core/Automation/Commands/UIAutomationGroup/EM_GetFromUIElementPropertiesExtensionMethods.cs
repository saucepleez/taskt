using System;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    public static class EM_GetFromUIElementPropertiesExtensionMethods
    {
        /// <summary>
        /// value(s) can not retrieved process
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <param name="setEmptyFunc"></param>
        public static void ValueCanNotRetrievedProcess(this IGetFromUIElementProperties command, string valueName, Action setEmptyFunc, AutomationEngineInstance engine)
        {
            switch(command.ToScriptCommand().ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_WhenValueCanNotRetrieved), engine))
            {
                case "error":
                    throw new Exception($"Error. Fail get {valueName} Value(s).");

                case "ignore":
                    return;

                case "set empty":
                    setEmptyFunc();
                    break;
            }
        }
    }
}
