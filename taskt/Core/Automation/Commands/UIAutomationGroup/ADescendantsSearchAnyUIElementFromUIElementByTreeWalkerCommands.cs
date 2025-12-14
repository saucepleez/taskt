using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    /// <summary>
    /// for descendants search any-one UIElement from UIElement by TreeWalker commands
    /// </summary>
    public abstract class ADescendantsSearchAnyUIElementFromUIElementByTreeWalkerCommands : ADescendantsSearchAnyUIElementFromSomethingByTreeWalkerCommands, IDoSomethingUIElementProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_InputUIElementName))]
        [PropertyParameterOrder(5000)]
        public virtual string v_TargetElement { get; set; }
    }
}
