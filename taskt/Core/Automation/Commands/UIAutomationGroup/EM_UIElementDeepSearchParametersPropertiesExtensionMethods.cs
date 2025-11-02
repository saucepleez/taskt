using System;
using System.Collections.Generic;
using System.Windows.Automation;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    public static class EM_UIElementDeepSearchParametersPropertiesExtensionMethods
    {
        /// <summary>
        /// Deep search UIElements
        /// </summary>
        /// <param name="command"></param>
        /// <param name="rootElement"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        private static List<AutomationElement> DeepSearchUIElements(this IUIElementDeepSearchParametersProperties command, AutomationElement rootElement, Engine.AutomationEngineInstance engine)
        {
            var conditions = command.CreateSearchCondition(engine);
            (var firstChildFunc, var nextChildFunc) = command.GetSiblingNodeFunc(engine);
            var maxSiblingsFunc = command.GetMaxSiblingsFunc(engine);
            var maxElementsFunc = command.GetMaxNumberUIElementsFunc(engine);
            var maxDepthFunc = command.GetMaxDepthFunc(engine);

            var waitTime = command.ExpandValueOrUserVariableAsWaitTimeForUIElement(engine);

            var r = WaitControls.WaitProcess(waitTime, "UIElements", new Func<Func<bool>, (bool, object)>((timeoutFunc) =>
            {
                var walker = TreeWalker.RawViewWalker;

                var elems = new List<AutomationElement>();
                EM_UIElementCoreSearchParametersPropertiesExtensionMethods.CheckAndAddProcess(rootElement, conditions, elems);
                if (timeoutFunc() || maxElementsFunc(elems))
                {
                    return (true, elems);
                }

                int depth = 1;
                if (maxDepthFunc(depth))
                {
                    return (true, elems);
                }
                else
                {
                    DeepSearchUIElements_DepthFirst(rootElement, conditions, depth, walker, firstChildFunc, nextChildFunc, maxSiblingsFunc, maxDepthFunc, maxElementsFunc, timeoutFunc, elems);
                    return (true, elems);
                }

            }), engine);

            if (r is List<AutomationElement> e)
            {
                return e;
            }
            else
            {
                // not found
                return new List<AutomationElement>();
            }
        }

        /// <summary>
        /// recursive Deep Search UIElements (depth first)
        /// </summary>
        /// <param name="rootElement">root UIElement (this UIElement not check)</param>
        /// <param name="searchConditions"></param>
        /// <param name="currentDepth"></param>
        /// <param name="walker"></param>
        /// <param name="firstChildFunc"></param>
        /// <param name="nextChildFunc"></param>
        /// <param name="maxSiblingsFunc">when Func returns true, max Siblings</param>
        /// <param name="maxDepthFunc">when Func returns true, max Depth</param>
        /// <param name="maxElementsFunc">when Func returns true, max Elements</param>
        /// <param name="timeoutFunc"></param>
        /// <param name="matchedElements"></param>
        private static void DeepSearchUIElements_DepthFirst(AutomationElement rootElement, List<PropertyCondition> searchConditions,
                            int currentDepth,
                            TreeWalker walker,
                            Func<AutomationElement, TreeWalker, AutomationElement> firstChildFunc, Func<AutomationElement, TreeWalker, AutomationElement> nextChildFunc,
                            Func<int, bool> maxSiblingsFunc, Func<int, bool> maxDepthFunc,
                            Func<List<AutomationElement>, bool> maxElementsFunc,
                            Func<bool> timeoutFunc, List<AutomationElement> matchedElements)
        {
            var node = firstChildFunc(rootElement, walker);
            int sibCount = 0;
            while (node != null)
            {
                EM_UIElementCoreSearchParametersPropertiesExtensionMethods.CheckAndAddProcess(node, searchConditions, matchedElements);
                if (timeoutFunc() || maxElementsFunc(matchedElements))
                {
                    return;
                }

                // check UIElement has child elements
                if (walker.GetFirstChild(node) != null)
                {
                    // has child UIElement
                    currentDepth++;
                    if (maxDepthFunc(currentDepth))
                    {
                        return;
                    }
                    else
                    {
                        DeepSearchUIElements_DepthFirst(node, searchConditions, currentDepth, walker, firstChildFunc, nextChildFunc, maxSiblingsFunc, maxDepthFunc, maxElementsFunc, timeoutFunc, matchedElements);
                    }
                }
                if (timeoutFunc() || maxElementsFunc(matchedElements))
                {
                    return;
                }

                // next sibling
                node = nextChildFunc(node, walker);
                sibCount++;
                if (maxSiblingsFunc(sibCount))
                {
                    return;
                }
            }
        }

        /// <summary>
        /// expand value or user variable as Max Depth
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static int ExpandValueOrUserVariableAsMaxDepth(this IUIElementDeepSearchParametersProperties command, Engine.AutomationEngineInstance engine)
        {
            if (string.IsNullOrEmpty(command.v_MaxDepth))
            {
                command.v_MaxSiblings = "16";
            }
            return command.ToScriptCommand().ExpandValueOrUserVariableAsInteger(nameof(command.v_MaxDepth), engine);
        }

        /// <summary>
        /// get check Max Depth func
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns>when Func returns true, max depth</returns>
        public static Func<int, bool> GetMaxDepthFunc(this IUIElementDeepSearchParametersProperties command, Engine.AutomationEngineInstance engine)
        {
            var maxDepth = command.ExpandValueOrUserVariableAsMaxDepth(engine);
            if (maxDepth == 0)
            {
                return new Func<int, bool>((d) => false);
            }
            else
            {
                return new Func<int, bool>((d) => (d > maxDepth));
            }
        }
    }
}
