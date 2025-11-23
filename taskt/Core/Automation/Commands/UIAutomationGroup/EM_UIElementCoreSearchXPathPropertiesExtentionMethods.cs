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
        public static string ExpandValueOrUserVariableAsXPath(this IUIElementCoreSearchXPathProperties command, Engine.AutomationEngineInstance engine)
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
    }
}
