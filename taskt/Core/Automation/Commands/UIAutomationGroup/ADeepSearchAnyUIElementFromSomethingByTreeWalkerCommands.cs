using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    /// <summary>
    /// for Deep Search any-one UIElement from UIElement by TreeWalker commands
    /// </summary>
    public abstract class ADeepSearchAnyUIElementFromSomethingByTreeWalkerCommands : ADeepSearchUIElementsFromSomethingByTreeWalkerCommands, IUIElementDeepSearchAnyUIElementProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_TargetUIElementIndex))]
        [PropertyParameterOrder(6100)]
        public virtual string v_TargetUIElementIndex { get; set; }
        
        [XmlAttribute]
        [PropertyIsOptional(true, "Same Value of UIElement Index")]
        public override string v_MaxNumberUIElements { get; set; }
    }
}
