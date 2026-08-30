using System.Collections.Generic;
using System.Windows.Automation;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    public static class EM_GetUIElementsInformationPropertiesExtentionMethods
    {
        /// <summary>
        /// store UIElements Information in user variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="elems"></param>
        /// <param name="engine"></param>
        public static void StoreUIElementsInformationInUserVariable(this IGetUIElementsInformationProperties command, List<AutomationElement> elems, Engine.AutomationEngineInstance engine)
        {
            string result = "";

            int counts = elems.Count;
            for (int i = 0; i < counts; i++)
            {
                var elem = elems[i];
                result += $"Index: {i}, Name: {elem.Current.Name}, LocalizedControlType: {elem.Current.LocalizedControlType}, ControlType: {EM_CanHandleUIElementExtentionMethods.GetControlTypeText(elem)}\n";
            }
            result.Trim().StoreInUserVariable(engine, command.v_Result);
        }
    }
}
