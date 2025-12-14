using System;
using System.Collections.Generic;
using System.Windows.Automation;
using System.Xml.Linq;
using System.Xml.XPath;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    public static class EM_UIElementCoreSearchXPathPropertiesExtentionMethods
    {
        /// <summary>
        /// expand value or user variable string as XPath
        /// </summary>
        /// <param name="value"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static string ExpandValueOrUserVariableAsXPath(this IUIElementChildrenSearchXPathProperties command, Engine.AutomationEngineInstance engine)
        {
            var p = command.ToScriptCommand().ExpandValueOrUserVariable(nameof(command.v_SearchXPath), "XPath", engine);
            if (!p.StartsWith("."))
            {
                p = $".{p}";
            }
            return p;
        }

        /// <summary>
        /// search UIElement by xpath
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="xml">UIElement XML</param>
        /// <param name="elemsDic">UIElement dic</param>
        /// <returns></returns>
        public static AutomationElement SearchUIElementByXPath(string xpath, XElement xml, Dictionary<string, AutomationElement> elemsDic)
        {
            var e = xml.XPathSelectElement(xpath);
            if (e != null)
            {
                return elemsDic[e.Attribute("Hash").Value];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// searchi children UIlement action
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <param name="targetElement"></param>
        /// <param name="actionFunc"></param>
        /// <param name="errorFunc"></param>
        public static void SearchChildrenUIElementAction(this IUIElementChildrenSearchXPathProperties command, Engine.AutomationEngineInstance engine, AutomationElement targetElement, Action<AutomationElement> actionFunc, Action<Exception> errorFunc = null)
        {
            try
            {
                var elem = command.SearchChildrenUIElementByXPath(targetElement, engine);
                actionFunc(elem);
                command.StoreWindowNameAndWindowHandleInUserVariablesFromUIElement(elem, engine);
            }
            catch (Exception ex)
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

        /// <summary>
        /// chidren search UIElement by XPath
        /// </summary>
        /// <param name="command"></param>
        /// <param name="rootElement"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static AutomationElement SearchChildrenUIElementByXPath(this IUIElementChildrenSearchXPathProperties command, AutomationElement rootElement, Engine.AutomationEngineInstance engine)
        {
            var waitTime = command.ExpandValueOrUserVariableAsWaitTimeForUIElement(engine);

            var xpath = command.ExpandValueOrUserVariableAsXPath(engine);

            var r = WaitControls.WaitProcess(waitTime, "UIElement", new Func<Func<bool>, (bool, object)>(waitFunc =>
            {
                (var xml, var dic) = command.CreateChildrenXMLCore(rootElement, waitFunc, engine);
                var elem = SearchUIElementByXPath(xpath, xml, dic);
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
        /// create children xml
        /// </summary>
        /// <param name="command"></param>
        /// <param name="rootElement"></param>
        /// <param name="waitFunc">when Func returns true, time out</param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static (XElement, Dictionary<string, AutomationElement>) CreateChildrenXMLCore(this IUIElementChildrenSearchXPathProperties command, AutomationElement rootElement, Func<bool> waitFunc, Engine.AutomationEngineInstance engine)
        {
            var parentXMLNode = EM_CanHandleUIElementXMLExtentionMethods.CreateXmlElement(rootElement);
            var elemsDic = new Dictionary<string, AutomationElement>()
            {
                { parentXMLNode.GetHashCode().ToString(), rootElement }
            };

            var siblingFunc = command.GetMaxSiblingsFunc(engine);
            var walker = TreeWalker.RawViewWalker;

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
                if (siblingFunc(sibCnt) || waitFunc())
                {
                    break;
                }

                targetElement = walker.GetNextSibling(targetElement);
            }

            return (parentXMLNode, elemsDic);
        }
    }
}
