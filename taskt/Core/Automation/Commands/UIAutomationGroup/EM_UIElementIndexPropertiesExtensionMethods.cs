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
            return command.ToScriptCommand().ExpandValueOrUserVariableAsInteger(nameof(command.v_TargetUIElementIndex), engine);
        }
    }
}
