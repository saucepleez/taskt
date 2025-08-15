using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    public static class ShowDialogControls
    {
        /// <summary>
        /// behavior when click cancel in dialog
        /// </summary>
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_ComboBoxHasErrorIgnore))]
        [PropertyDescription("When Dialog Result Is Cancel")]
        [PropertyUISelectionOption("Set Empty")]
        [PropertyUISelectionOption("Show Dialog Again")]
        [PropertyDetailSampleUsage("**Ignore**", "Nothing to do. The Result Variable is not Changed.")]
        [PropertyDetailSampleUsage("**Set Empty**", "Result Variable value is Empty")]
        [PropertyDetailSampleUsage("**Show Dialog Again", "Show Dialog Again")]
        [PropertyIsOptional(true, "Show Dialog Again")]
        [PropertyValidationRule("When Dialog Result Is Cancel", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "When Dialog Result Is Cancel")]
        public static string v_WhenCancel { get; }

        /// <summary>
        /// variable name to store dialog result
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name To Store Dislog Result")]
        [Remarks("Value is **OK** or **Cancel**")]
        [PropertyValidationRule("DialogResult", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "DialogResult")]
        public static string v_DialogResult { get; }
    }
}
