using System.Collections.Generic;
using System.Windows.Automation;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    public static class EM_UIElementIndexPropertiesExtensionMethods
    {
        /// <summary>
        /// Expand value or user variable as UIElement Index
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static int ExpandValueOrUserVariableAsUIElementIndex(this IUIElementIndexProperties command, Engine.AutomationEngineInstance engine)
        {
            if (string.IsNullOrEmpty(command.v_TargetUIElementIndex))
            {
                command.v_TargetUIElementIndex = "0";
            }
            return command.ToScriptCommand().ExpandValueOrUserVariableAsInteger(nameof(command.v_TargetUIElementIndex), "UIElement Index", engine);
        }

        /// <summary>
        /// get UIElement from List-UIElement
        /// </summary>
        /// <param name="command"></param>
        /// <param name="elems"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public static AutomationElement GetUIElementFromLis(this IUIElementIndexProperties command, List<AutomationElement> elems, Engine.AutomationEngineInstance engine)
        {
            var index = command.ExpandValueOrUserVariableAsUIElementIndex(engine);
            if (index < 0)
            {
                index += elems.Count;
            }
            if (index >= 0 && index < elems.Count)
            {
                return elems[index];
            }
            else
            {
                throw new System.Exception($"UIElement does not Exists. Index: {command.v_TargetUIElementIndex}, Expand Value: {index}");
            }
        }
    }
}
