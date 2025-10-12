using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    public static class VP_UIElementControls
    {
        /// <summary>
        /// input UIElement
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_InputInstanceName))]
        [PropertyDescription("UIElement Variable Name")]
        [InputSpecification("UIElement Variable Name", true)]
        [PropertyDetailSampleUsage("**vElement**", PropertyDetailSampleUsage.ValueType.VariableValue)]
        [PropertyDetailSampleUsage("**{{{vElement}}}**", PropertyDetailSampleUsage.ValueType.VariableValue)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.UIElement, true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyValidationRule("UIElement", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Element")]
        public static string v_InputUIElementName { get; }
    }
}
