using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    /// <summary>
    /// for depp search any-one UIElement from UIElement by TreeWalker commands
    /// </summary>
    public abstract class ADeepSearchAnyUIElementFromUIElementByTreeWalkerCommands : ADeepSearchAnyUIElementFromSomethingByTreeWalkerCommands, IDoSomethingUIElementProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_InputUIElementName))]
        [PropertyParameterOrder(5000)]
        public virtual string v_TargetElement { get; set; }
    }
}
