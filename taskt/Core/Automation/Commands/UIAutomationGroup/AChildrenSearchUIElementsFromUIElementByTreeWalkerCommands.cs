using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    /// <summary>
    /// for children search UIElements from UIElements by TreeWalker commands
    /// </summary>
    public abstract class AChildrenSearchUIElementsFromUIElementByTreeWalkerCommands : AChildrenSearchUIElementsFromSomethingByTreeWalkerCommands, IDoSomethingUIElementProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_InputUIElementName))]
        [PropertyParameterOrder(5000)]
        public virtual string v_TargetElement { get; set; }
    }
}
