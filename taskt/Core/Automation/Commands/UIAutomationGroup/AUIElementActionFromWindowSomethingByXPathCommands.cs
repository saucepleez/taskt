using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    public abstract class AUIElementActionFromWindowSomethingByXPathCommands : AUIElementActionFromSomethingByXPathCommands, IWindowUIElementResultProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_WindowUIElementName))]
        [PropertyParameterOrder(10200)]
        public string v_WindowUIElement { get; set; }
    }
}
