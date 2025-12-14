using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    /// <summary>
    /// for descendants search UIElement from something by XPath commands
    /// </summary>
    public abstract class ADescendantsSearchUIElementsFromSomethingByXPathCommands : AChildrenSearchUIElementsFromSomethingByXPathCommands, IUIElementDescendantsSearchXPathProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_MaxDepth))]
        [PropertyParameterOrder(7992)]
        public virtual string v_MaxDepth { get; set; }
    }
}
