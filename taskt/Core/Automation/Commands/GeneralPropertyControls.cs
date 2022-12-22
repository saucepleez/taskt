using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// general virtual properties
    /// </summary>
    internal static class GeneralPropertyControls
    {
        /// <summary>
        /// specify variable name to store result property
        /// </summary>
        [PropertyDescription("Variable Name to Store Result")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [PropertyDetailSampleUsage("**vResult**", "Specify Variable Name **vResult**")]
        [PropertyDetailSampleUsage("**{{{vResult}}}**", "Specify Variable Name **vResult**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsVariablesList(true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Result")]
        public static string v_Result { get; }
    }
}
