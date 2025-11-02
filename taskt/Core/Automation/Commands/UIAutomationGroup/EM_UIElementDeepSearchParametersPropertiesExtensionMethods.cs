using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    public static class EM_UIElementDeepSearchParametersPropertiesExtensionMethods
    {
        /// <summary>
        /// deep search UIElements
        /// </summary>
        /// <param name="rootElement"></param>
        /// <param name="conditions"></param>
        /// <param name="maxDepth"></param>
        /// <param name="maxSibling"></param>
        /// <param name="timeoutFunc"></param>
        /// <returns></returns>
        private static List<AutomationElement> DeepSearchUIElements(AutomationElement rootElement, List<PropertyCondition> conditions, int maxDepth, int maxSibling, Func<bool> timeoutFunc)
        {
            // MEMO: for specify search direction
            Func<AutomationElement, TreeWalker, AutomationElement> firstChildFunc = new Func<AutomationElement, TreeWalker, AutomationElement>((el, wa) =>
            {
                return wa.GetFirstChild(el);
            });
            Func<AutomationElement, TreeWalker, AutomationElement> nextChildFunc = new Func<AutomationElement, TreeWalker, AutomationElement>((el, wa) =>
            {
                return wa.GetNextSibling(el);
            });

            var walker = TreeWalker.RawViewWalker;

            var ret = new List<AutomationElement>();
            EM_UIElementCoreSearchParametersPropertiesExtensionMethods.CheckAndAddProcess(rootElement, conditions, ret);

            return DeepSearchUIElements_DepthFirst(rootElement, conditions, 0, maxDepth, maxSibling, walker, firstChildFunc, nextChildFunc, timeoutFunc, ret);
        }

        /// <summary>
        /// deep Search GUI Element used by TreeWalker (Depth First)
        /// </summary>
        /// <param name="rootElement"></param>
        /// <param name="searchConditions"></param>
        /// <param name="currentDepth">current depth</param>
        /// <param name="maxDepath">depth limit</param>
        /// <param name="maxSibling">sibling limit</param>
        /// <param name="walker"></param>
        /// <param name="firstChildFunc"></param>
        /// <param name="nextChildFunc"></param>
        /// <param name="timeoutFunc"></param>
        /// <param name="matchedElements">if returns true time-out</param>
        /// <returns></returns>
        private static List<AutomationElement> DeepSearchUIElements_DepthFirst(AutomationElement rootElement, List<PropertyCondition> searchConditions,
                            int currentDepth, int maxDepath, int maxSibling,
                            TreeWalker walker,
                            Func<AutomationElement, TreeWalker, AutomationElement> firstChildFunc, Func<AutomationElement, TreeWalker, AutomationElement> nextChildFunc,
                            Func<bool> timeoutFunc, List<AutomationElement> matchedElements)
        {
            var node = firstChildFunc(rootElement, walker);
            int sibCount = 0;
            while (node != null)
            {
                EM_UIElementCoreSearchParametersPropertiesExtensionMethods.CheckAndAddProcess(node, searchConditions, matchedElements);
                if (timeoutFunc())
                {
                    return matchedElements;
                }

                // check UIElement has child elements
                if ((walker.GetFirstChild(node) != null) && ((currentDepth + 1) < maxDepath))
                {
                    DeepSearchUIElements_DepthFirst(node, searchConditions, (currentDepth + 1), maxDepath, maxSibling, walker, firstChildFunc, nextChildFunc, timeoutFunc, matchedElements);
                }
                if (timeoutFunc())
                {
                    return matchedElements;
                }

                // next sibling
                node = nextChildFunc(node, walker);
                sibCount++;
                if (sibCount >= maxSibling)
                {
                    break;
                }
            }

            return matchedElements;
        }
    }
}
