using System;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    internal static class NLGControls
    {
        [PropertyDescription("NLG Instance Name")]
        [InputSpecification("NLG Instance Name", true)]
        [PropertyDetailSampleUsage("**nlgInstance**", PropertyDetailSampleUsage.ValueType.Value, "Instance")]
        [PropertyDetailSampleUsage("**{{{vInstance}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Instance")]
        [PropertyShowSampleUsageInDescription(true)]
        [Remarks("Failure to enter the correct instance name or failure to first call **Create NLG Instance** command will cause an error")]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.NLG)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyValidationRule("NLG Instance", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Instance")]
        [PropertyFirstValue("%kwd_default_nlg_instance%")]
        public static string v_InstanceName { get; }
    }
}
