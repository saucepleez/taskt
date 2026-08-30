using System;
using System.Collections.Generic;
using System.Windows.Automation;
using System.Xml.Linq;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    public static class EM_UIElementDescendantsSearchXPathPropertiesExtensionMethods
    {
        /// <summary>
        /// Deep Search UIElement Action
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <param name="targetElement"></param>
        /// <param name="actionFunc"></param>
        /// <param name="errorFunc"></param>
        public static void DeepSearchUIElementAction(this IUIElementDescendantsSearchXPathProperties command, Engine.AutomationEngineInstance engine, AutomationElement targetElement, Action<AutomationElement> actionFunc, Action<Exception> errorFunc = null)
        {
            //try
            //{
            //    var elem = command.DeepSearchUIElementByXPath(targetElement, engine);
            //    actionFunc(elem);
            //    command.StoreWindowNameAndWindowHandleInUserVariablesFromUIElement(elem, engine);
            //}
            //catch (Exception ex)
            //{
            //    if (errorFunc != null)
            //    {
            //        errorFunc(ex);
            //    }
            //    else
            //    {
            //        throw ex;
            //    }
            //}

            command.SearchAnyUIElementActionCore(engine,
                new Func<AutomationElement>(() => command.DeepSearchUIElementByXPath(targetElement, engine)),
                actionFunc, errorFunc);
        }

        /// <summary>
        /// deep search UIElement by XPath
        /// </summary>
        /// <param name="command"></param>
        /// <param name="rootElement"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static AutomationElement DeepSearchUIElementByXPath(this IUIElementDescendantsSearchXPathProperties command, AutomationElement rootElement, Engine.AutomationEngineInstance engine)
        {
            var waitTime = command.ExpandValueOrUserVariableAsWaitTimeForUIElement(engine);

            var xpath = command.ExpandValueOrUserVariableAsXPath(engine);

            var r = WaitControls.WaitProcess(waitTime, "UIElement", new Func<Func<bool>, (bool, object)>(waitFunc =>
            {
                (var xml, var dic) = command.DeepCreateUIElementXMLCore(rootElement, waitFunc, engine);
                var elem = EM_UIElementChildrenSearchXPathPropertiesExtentionMethods.SearchUIElementByXPath(xpath, xml, dic);
                if (elem != null)
                {
                    return (true, elem);
                }
                else
                {
                    return (false, null);
                }
            }), engine);
            if (r is AutomationElement e)
            {
                return e;
            }
            else
            {
                throw new Exception($"AutomationElement not Found. XPath: '{command.v_SearchXPath}', Expand Value: '{xpath}'");
            }
        }

        /// <summary>
        /// deep create UIElement xml
        /// </summary>
        /// <param name="command"></param>
        /// <param name="targetElement"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static (XElement, Dictionary<string, AutomationElement>) DeepCreateUIElementXMLCore(this IUIElementDescendantsSearchXPathProperties command, AutomationElement targetElement, Func<bool> timeFunc, Engine.AutomationEngineInstance engine)
        {
            var siblingFunc = command.GetMaxSiblingsFunc(engine);
            var depthFunc = command.GetMaxDepthFunc(engine);

            var rootXML = EM_CanHandleUIElementXMLExtentionMethods.CreateXmlElement(targetElement);

            var hashDic = new Dictionary<string, AutomationElement>()
            {
                { rootXML.GetHashCode().ToString(), targetElement }
            };

            var walker = TreeWalker.RawViewWalker;
            DeepCreateUIElementXML_DepthFirst(rootXML, targetElement, hashDic, walker, 1, siblingFunc, depthFunc, timeFunc);

            // DBG
            //Console.WriteLine("!!XML Tree");
            //foreach(var item in hashDic)
            //{
            //    var k = item.Key;
            //    var v = item.Value;
            //    Console.WriteLine($"{k} {v.Current.Name} {v.Current.GetHashCode()}");
            //}

            return (rootXML, hashDic);
        }

        /// <summary>
        /// deep create UIElement XML depth first
        /// </summary>
        /// <param name="parentXMLNode"></param>
        /// <param name="rootElement"></param>
        /// <param name="elemsDic"></param>
        /// <param name="walker"></param>
        /// <param name="depth"></param>
        /// <param name="siblingFunc">when Func returns true, max siblings</param>
        /// <param name="depthFunc">when Func returns true, max depth</param>
        /// <param name="timeFunc">when Func returns true, time out</param>
        private static void DeepCreateUIElementXML_DepthFirst(XElement parentXMLNode, AutomationElement rootElement, Dictionary<string, AutomationElement> elemsDic, 
                                TreeWalker walker, int depth, Func<int, bool> siblingFunc, Func<int, bool> depthFunc, Func<bool> timeFunc)
        {
            var targetElement = walker.GetFirstChild(rootElement);
            int sibCnt = 0;
            while (targetElement != null)
            {
                // check hash dup
                string hash = targetElement.GetHashCode().ToString();
                if (elemsDic.ContainsKey(hash))
                {
                    int i = 1;
                    while (elemsDic.ContainsKey($"{hash}-{i}"))
                    {
                        i++;
                    }
                    hash += $"-{i}";
                }

                var childNode = EM_CanHandleUIElementXMLExtentionMethods.CreateXmlElement(targetElement, hash);
                parentXMLNode.Add(childNode);
                elemsDic.Add(hash, targetElement);

                sibCnt++;
                if (siblingFunc(sibCnt) || timeFunc())
                {
                    return;
                }

                if (walker.GetFirstChild(targetElement) != null)
                {
                    depth++;
                    if (depthFunc(depth))
                    {
                        return;
                    }
                    else
                    {
                        DeepCreateUIElementXML_DepthFirst(childNode, targetElement, elemsDic, walker, depth, siblingFunc, depthFunc, timeFunc);
                    }
                }
                if (timeFunc())
                {
                    return;
                }

                targetElement = walker.GetNextSibling(targetElement);
            }
        }
    }
}
