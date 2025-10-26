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

        /// <summary>
        /// output UIElement property
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store UIElement")]
        [InputSpecification("UIElement Variable Name", true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.UIElement, true)]
        [PropertyValidationRule("UIElement", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Element")]
        public static string v_OutputUIElementName { get; }

        /// <summary>
        /// New output UIElement name
        /// </summary>
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_OutputUIElementName))]
        [PropertyDescription("UIElement Variable Name")]
        [PropertyDetailSampleUsageBehavior(MultiAttributesBehavior.Overwrite)]
        [PropertyDetailSampleUsage("**vNewElement**", PropertyDetailSampleUsage.ValueType.VariableValue)]
        [PropertyDetailSampleUsage("**{{{vNewElement}}}**", PropertyDetailSampleUsage.ValueType.VariableValue)]
        [PropertyValidationRule("New UIElement", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "New UIElement")]
        public static string v_NewOutputUIElementName { get; }

        /// <summary>
        /// wait time before action
        /// </summary>
        [PropertyVirtualProperty(nameof(WaitControls), nameof(WaitControls.v_WaitTime))]
        [PropertyDescription("Wait Time before Action (sec)")]
        [PropertyIsOptional(true, "0")]
        [PropertyFirstValue("0")]
        [PropertyDisplayText(false, "Wait Time Before", "s")]
        public static string v_WaitTimeBeforeAction { get; }

        /// <summary>
        /// wait time after action
        /// </summary>
        [PropertyVirtualProperty(nameof(WaitControls), nameof(WaitControls.v_WaitTime))]
        [PropertyDescription("Wait Time after Action (sec)")]
        [PropertyIsOptional(true, "0")]
        [PropertyFirstValue("0")]
        [PropertyDisplayText(false, "Wait Time Before", "s")]
        public static string v_WaitTimeAfterAction { get; }

        /// <summary>
        /// activate window before action
        /// </summary>
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Activate Window before Action")]
        [PropertyIsOptional(true, "No")]
        public static string v_ActivateWindow { get; }

        /// <summary>
        /// when this action is not supported
        /// </summary>
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_ComboBoxHasErrorIgnore))]
        [PropertyDescription("When Action Is Not Supported")]
        [PropertyIsOptional(true, "Error")]
        public static string v_WhenActionIsNotSupported { get; }

        /// <summary>
        /// wait time for UIElement
        /// </summary>
        [PropertyVirtualProperty(nameof(WaitControls), nameof(WaitControls.v_WaitTime))]
        [PropertyDescription("Wait Time for the UIElement to Exist (sec)")]
        [Remarks("Specify how long to Wait before an Error will occur because the UIElement is Not Found.")]
        [PropertyIsOptional(true, "10")]
        [PropertyFirstValue("10")]
        public static string v_WaitTimeForUIElement { get; }

    }
}
