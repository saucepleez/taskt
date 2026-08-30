using System.Windows.Automation;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    public static class EM_WindowUIElementResultPropertiesExtensionMethods
    {
        /// <summary>
        /// Store Window UIElement In User varialbe
        /// </summary>
        /// <param name="command"></param>
        /// <param name="elem"></param>
        /// <param name="engine"></param>
        public static void StoreWindowUIElementInUserVariable(this IWindowUIElementResultProperties command, AutomationElement elem, Engine.AutomationEngineInstance engine)
        {
            if (!string.IsNullOrEmpty(command.v_WindowUIElement))
            {
                elem.StoreInUserVariable(engine, command.v_WindowUIElement);
            }
        }
    }
}
