using System.Windows.Automation;
using System.Xml.Linq;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    public static class EM_CanHandleUIElementXMLExtentionMethods
    {
        /// <summary>
        /// create XML Element from UIElement
        /// </summary>
        /// <param name="targetElement"></param>
        /// <param name="hash">specify hash if dup</param>
        /// <returns></returns>
        public static XElement CreateXmlElement(AutomationElement targetElement, string hash = "")
        {
            var node = new XElement(EM_CanHandleUIElementExtentionMethods.GetControlTypeText(targetElement));

            var cur = targetElement.Current;

            node.SetAttributeValue("AcceleratorKey", cur.AcceleratorKey);
            node.SetAttributeValue("AccessKey", cur.AccessKey);
            node.SetAttributeValue("AutomationId", cur.AutomationId);
            node.SetAttributeValue("ClassName", cur.ClassName);
            node.SetAttributeValue("FrameworkId", cur.FrameworkId);
            node.SetAttributeValue("HasKeyboardFocus", cur.HasKeyboardFocus.ToString());
            node.SetAttributeValue("HelpText", cur.HelpText);
            node.SetAttributeValue("IsContentElement", cur.IsContentElement.ToString());
            node.SetAttributeValue("IsControlElement", cur.IsControlElement.ToString());
            node.SetAttributeValue("IsEnabled", cur.IsEnabled.ToString());
            node.SetAttributeValue("IsKeyboardFocusable", cur.IsKeyboardFocusable.ToString());
            node.SetAttributeValue("IsOffscreen", cur.IsOffscreen.ToString());
            node.SetAttributeValue("IsPassword", cur.IsPassword.ToString());
            node.SetAttributeValue("IsRequiredForForm", cur.IsRequiredForForm.ToString());
            node.SetAttributeValue("ItemStatus", cur.ItemStatus);
            node.SetAttributeValue("LocalizedControlType", cur.LocalizedControlType);
            node.SetAttributeValue("Name", cur.Name);
            node.SetAttributeValue("NativeWindowHandle", cur.NativeWindowHandle.ToString());
            node.SetAttributeValue("ProcessId", cur.ProcessId.ToString());

            node.SetAttributeValue("Hash", (string.IsNullOrEmpty(hash) ? targetElement.GetHashCode().ToString() : hash));

            return node;
        }
    }
}
