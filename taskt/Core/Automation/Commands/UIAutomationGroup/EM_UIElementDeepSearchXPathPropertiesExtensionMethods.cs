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

        public static (XElement, Dictionary<string, AutomationElement>) DeepCreateUIElementXMLCore(this IUIElementDeepSearchXPathProperties command, AutomationElement targetElement, SafeAutomationEngineInstanceApplicationSettings engine)
        {
            var rootXML = EM_CanHandleUIElementXMLExtentionMethods.CreateXmlElement(targetElement);

            var hashDic = new Dictionary<string, AutomationElement>()
            {
                { rootXML.GetHashCode().ToString(), targetElement }
            };

            var walker = TreeWalker.RawViewWalker;
            DeepCreateUIElementXML_DepthFirst(rootXML, targetElement, hashDic, walker);

            return (rootXML, hashDic);
        }

        private static void DeepCreateUIElementXML_DepthFirst(XElement rootNode, AutomationElement rootElement, Dictionary<string, AutomationElement> elemsDic, TreeWalker walker)
        {
            var node = walker.GetFirstChild(rootElement);
            while (node != null)
            {
                // check hash dup
                string hash = node.GetHashCode().ToString();
                if (elemsDic.ContainsKey(hash))
                {
                    int i = 1;
                    while (elemsDic.ContainsKey($"{hash}-{i}"))
                    {
                        i++;
                    }
                    hash += $"-{i}";
                }

                var childNode = EM_CanHandleUIElementXMLExtentionMethods.CreateXmlElement(node, hash);
                rootNode.Add(childNode);
                elemsDic.Add(hash, node);

                if (walker.GetFirstChild(node) != null)
                {
                    DeepCreateUIElementXML_DepthFirst(childNode, node, elemsDic, walker);
                }

                node = walker.GetNextSibling(node);
            }
        }
    }
}
