using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    /// <summary>
    ///  for descendants search UIElement from UIElement by XPath commands
    /// </summary>
    public abstract class ADescendantsSearchUIElementsFromUIElementByXPathCommands : ADescendantsSearchUIElementsFromSomethingByXPathCommands, IUIElementCoreProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_InputUIElementName))]
        [PropertyParameterOrder(5000)]
        public virtual string v_TargetElement { get; set; }
    }
}
