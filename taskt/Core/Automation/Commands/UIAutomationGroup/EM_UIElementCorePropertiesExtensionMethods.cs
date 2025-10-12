using System;
using System.Windows.Automation;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    public static class EM_UIElementCorePropertiesExtensionMethods
    {
        /// <summary>
        /// expand user variable as UIElement
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static AutomationElement ExpandUserVariableAsUIElement(this IUIElementCoreProperties command, Engine.AutomationEngineInstance engine)
        {
            var variableName = command.v_TargetElement;
            var v = variableName.GetRawVariable(engine);
            if (EM_CanHandleUIElementExtentionMethods.IsUIElement(v.VariableValue, out AutomationElement e))
            {
                return e;
            }
            else
            {
                throw new Exception($"Variable '{variableName}' is not UIElement");
            }
        }
    }
}
