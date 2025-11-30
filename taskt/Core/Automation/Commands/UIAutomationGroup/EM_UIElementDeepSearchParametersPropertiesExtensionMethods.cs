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
        public static List<AutomationElement> DeepSearchUIElements(this IUIElementDeepSearchParametersProperties command, AutomationElement rootElement, Engine.AutomationEngineInstance engine)
        {
            var foundFunc = new Func<List<AutomationElement>, bool>(elems => (elems.Count > 0));
            var maxElementsFunc = command.GetMaxNumberUIElementsFunc(engine);

            return DeepSearchUIElementsCore(command, rootElement, foundFunc, maxElementsFunc, engine);
        }

        /// <summary>
        /// Deep Search UIElements core process
        /// </summary>
        /// <param name="command"></param>
        /// <param name="rootElement"></param>
        /// <param name="foundFunc">when func returns true, found</param>
        /// <param name="maxElementsFunc">when func returns true, max UIElements</param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static List<AutomationElement> DeepSearchUIElementsCore(this IUIElementDeepSearchParametersProperties command, AutomationElement rootElement, 
                        Func<List<AutomationElement>, bool> foundFunc, Func<List<AutomationElement>, bool> maxElementsFunc,
                        Engine.AutomationEngineInstance engine)
        {
            var conditions = command.CreateSearchCondition(engine);
            (var firstChildFunc, var nextChildFunc) = command.GetSiblingNodeFunc(engine);
            var maxSiblingsFunc = command.GetMaxSiblingsFunc(engine);
            var maxDepthFunc = command.GetMaxDepthFunc(engine);

            var waitTime = command.ExpandValueOrUserVariableAsWaitTimeForUIElement(engine);

            var r = WaitControls.WaitProcess(waitTime, "UIElements", new Func<Func<bool>, (bool, object)>((timeoutFunc) =>
            {
                var walker = TreeWalker.RawViewWalker;

                var elems = new List<AutomationElement>();
                EM_UIElementCoreSearchParametersPropertiesExtensionMethods.CheckAndAddProcess(rootElement, conditions, elems);
                if (timeoutFunc() || maxElementsFunc(elems))
                {
                    return (foundFunc(elems), elems);
                }

                int depth = 1;
                if (maxDepthFunc(depth))
                {
                    return (foundFunc(elems), elems);
                }
                else
                {
                    DeepSearchUIElements_DepthFirst(rootElement, conditions, depth, walker, firstChildFunc, nextChildFunc, maxSiblingsFunc, maxDepthFunc, maxElementsFunc, timeoutFunc, elems);
                    return (foundFunc(elems), elems);
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
                // DBG
                //Console.WriteLine(node.Current.AutomationId);

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
        /// deep search UIElements Action
        /// </summary>
        /// <param name="command"></param>
        /// <param name="rootElement"></param>
        /// <param name="engine"></param>
        /// <param name="actionFunc"></param>
        /// <param name="errorFunc"></param>
        public static void DeepSearchUIElementsAction(this IUIElementDeepSearchParametersProperties command, AutomationElement rootElement, Engine.AutomationEngineInstance engine, Action<List<AutomationElement>> actionFunc, Action<Exception> errorFunc = null)
        {
            try
            {
                var elems = DeepSearchUIElements(command, rootElement, engine);
                actionFunc(elems);
                command.StoreWindowNameAndWindowHandleInUserVariablesFromUIElement(rootElement, engine);
            }
            catch (Exception ex)
            {
                {
                    if (errorFunc != null)
                    {
                        errorFunc(ex);
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }
        }
    }
}
