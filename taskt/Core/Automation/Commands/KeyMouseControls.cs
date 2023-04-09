using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    static internal class KeyMouseControls
    {
        /// <summary>
        /// 
        /// </summary>
        [PropertyDescription("Wait Time after Keys Enter (ms)")]
        [InputSpecification("Wait Time", true)]
        //[SampleUsage("**500** or **{{{vWaitTime}}}")]
        [PropertyDetailSampleUsage("**500**", PropertyDetailSampleUsage.ValueType.Value, "Wait Time")]
        [PropertyDetailSampleUsage("**{{{vWaitTime}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Wait Time")]
        [Remarks("When the Wait Time is less than **100** is specified, it will be **100**")]
        [PropertyIsOptional(true, "500")]
        [PropertyFirstValue("500")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Wait Time", PropertyValidationRule.ValidationRuleFlags.LessThanZero | PropertyValidationRule.ValidationRuleFlags.EqualsZero)]
        public static string v_WaitTimeAfterKeyEnter { get; set; }
    }
}
