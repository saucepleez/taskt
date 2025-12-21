using System;
using System.Windows.Automation;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    public static class EM_GetUIElementsTreeXMLFromSomethingPropertiesExtensionMethods
    {
        /// <summary>
        /// store UIElements XML Tree in User Variable from UIElement
        /// </summary>
        /// <param name="command"></param>
        /// <param name="targetElement"></param>
        /// <param name="engine"></param>
        public static void StoreUIElementsTreeXMLInUserVariableFromUIElement(this IGetUIElementsXMLTreeFromSomethingProperties command, AutomationElement targetElement, AutomationEngineInstance engine)
        {
            var maxTime = command.ExpandValueOrUserVariableAsWaitTimeForUIElement(engine);
            var finishTime = (maxTime > 0) ? DateTime.Now.AddSeconds(maxTime) : DateTime.Now;
            Func<bool> timeFunc = (maxTime <= 0) ?
                            new Func<bool>(() => false) :
                            new Func<bool>(() =>
                            {
                                return (DateTime.Now >= finishTime);
                            });

            var cmd = new UIAutomationSearchUIElementFromUIElementByXPathCommand()
            {
                v_MaxDepth = command.v_MaxDepth,
                v_MaxSiblings = command.v_MaxSiblings,
                v_WaitTimeForUIElement = command.v_WaitTimeForUIElement,
                v_WindowNameResult = command.v_WindowNameResult,
                v_WindowHandleResult = command.v_WindowHandleResult,
            };
            (var xml, _) = cmd.DeepCreateUIElementXMLCore(targetElement, timeFunc, engine);

            using (var sw = new System.IO.StringWriter())
            {
                xml.Save(sw);
                sw.ToString().StoreInUserVariable(engine, command.v_Result);
            }

            cmd.StoreWindowNameAndWindowHandleInUserVariablesFromUIElement(targetElement, engine);
        }
    }
}
