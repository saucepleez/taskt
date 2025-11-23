using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Xml.Linq;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    public static class EM_UIElementDeepSearchXPathPropertiesExtensionMethods
    {
        /// <summary>
        /// deep search UIElement by XPath
        /// </summary>
        /// <param name="command"></param>
        /// <param name="rootElement"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static AutomationElement DeepSearchUIElementByXPath(this IUIElementDeepSearchXPathProperties command, AutomationElement rootElement, Engine.AutomationEngineInstance engine)
        {
            var waitTime = command.ExpandValueOrUserVariableAsWaitTimeForUIElement(engine);

            var xpath = command.ExpandValueOrUserVariableAsXPath(engine);

            var r = WaitControls.WaitProcess(waitTime, "UIElement", new Func<Func<bool>, (bool, object)>(waitFunc =>
            {
                (var xml, var dic) = command.DeepCreateUIElementXMLCore(rootElement, waitFunc, engine);
                var elem = EM_UIElementCoreSearchXPathPropertiesExtentionMethods.SearchUIElementByXPath(xpath, xml, dic);
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
                throw new Exception($"AutomationElement not Found");
            }
        }

        /// <summary>
        /// deep create UIElement xml
        /// </summary>
        /// <param name="command"></param>
        /// <param name="targetElement"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static (XElement, Dictionary<string, AutomationElement>) DeepCreateUIElementXMLCore(this IUIElementDeepSearchXPathProperties command, AutomationElement targetElement, Func<bool> timeFunc, Engine.AutomationEngineInstance engine)
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
