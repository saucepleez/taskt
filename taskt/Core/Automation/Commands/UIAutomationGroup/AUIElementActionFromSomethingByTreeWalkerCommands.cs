using System.Data;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    /// <summary>
    /// for UIElement Action from Something, Search By TreeWalker commands
    /// </summary>
    public abstract class AUIElementActionFromSomethingByTreeWalkerCommands : AUIElementActionSomewayCommands, IUIElementChildrenSearchParametersProperties, IUIElementDescendantsSearchAnyUIElementProperties
    {
        [XmlElement]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_SearchParameters))]
        [PropertyParameterOrder(6000)]
        public DataTable v_SearchParameters { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_TargetUIElementIndex))]
        [PropertyParameterOrder(6100)]
        public string v_TargetUIElementIndex { get; set; }

        [XmlAttribute]
        [PropertyParameterOrder(6200)]
        public override string v_AutomationType { get; set; }

        [XmlElement]
        [PropertyParameterOrder(7900)]
        public override DataTable v_UIAActionParameters { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_MaxNumberUIElements))]
        [PropertyParameterOrder(7995)]
        public string v_MaxNumberUIElements { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_SiblingsDirection))]
        [PropertyParameterOrder(7996)]
        public string v_SiblingsDirection { get; set; }
    }
}
