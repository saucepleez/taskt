using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    /// <summary>
    /// for descendants search any-one UIElement From Window handle by TreeWalker commands
    /// </summary>
    public abstract class ADescendantsSearchAnyUIElementFromWindowHandleByTreeWalkerCommands : ADescendantsSearchUIElementsFromWindowHandleByTreeWalkerCommands, IUIElementIndexProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_TargetUIElementIndex))]
        [PropertyParameterOrder(6100)]
        public virtual string v_TargetUIElementIndex { get; set; }
    }
}
