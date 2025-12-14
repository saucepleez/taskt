using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    /// <summary>
    /// for descendants search UIElements from Something by TreeWalker commands
    /// </summary>
    public abstract class ADescendantsSearchUIElementsFromSomethingByTreeWalkerCommands : AChildrenSearchUIElementsFromSomethingByTreeWalkerCommands, IUIElementDescendantsSearchParametersProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_MaxDepth))]
        [PropertyParameterOrder(7992)]
        public virtual string v_MaxDepth { get; set; }
    }
}
