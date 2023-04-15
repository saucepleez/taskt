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
        /// wait after key enter
        /// </summary>
        [PropertyDescription("Wait Time after Keys Enter (ms)")]
        [InputSpecification("Wait Time", true)]
        [PropertyDetailSampleUsage("**500**", PropertyDetailSampleUsage.ValueType.Value, "Wait Time")]
        [PropertyDetailSampleUsage("**{{{vWaitTime}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Wait Time")]
        [Remarks("When the Wait Time is less than **100** is specified, it will be **100**")]
        [PropertyIsOptional(true, "500")]
        [PropertyFirstValue("500")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Wait Time", PropertyValidationRule.ValidationRuleFlags.LessThanZero | PropertyValidationRule.ValidationRuleFlags.EqualsZero)]
        public static string v_WaitTimeAfterKeyEnter { get; }

        /// <summary>
        /// mouse click
        /// </summary>
        [PropertyDescription("Mouse Click Type")]
        [PropertyUISelectionOption("Left Click")]
        [PropertyUISelectionOption("Middle Click")]
        [PropertyUISelectionOption("Right Click")]
        [PropertyUISelectionOption("Left Down")]
        [PropertyUISelectionOption("Middle Down")]
        [PropertyUISelectionOption("Right Down")]
        [PropertyUISelectionOption("Left Up")]
        [PropertyUISelectionOption("Middle Up")]
        [PropertyUISelectionOption("Right Up")]
        [PropertyUISelectionOption("Double Left Click")]
        [PropertyUISelectionOption("None")]
        [InputSpecification("", true)]
        [SampleUsage("")]
        [Remarks("You can simulate custom click by using multiple mouse click commands in succession, adding **Pause Command** in between where required.")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("Mouse Click", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Mouse Click")]
        public static string v_MouseClickType { get; }

        /// <summary>
        /// wait after key enter
        /// </summary>
        [PropertyDescription("Wait Time after Mouse Click (ms)")]
        [InputSpecification("Wait Time", true)]
        [PropertyDetailSampleUsage("**500**", PropertyDetailSampleUsage.ValueType.Value, "Wait Time")]
        [PropertyDetailSampleUsage("**{{{vWaitTime}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Wait Time")]
        [Remarks("When the Wait Time is less than **100** is specified, it will be **100**")]
        [PropertyIsOptional(true, "500")]
        [PropertyFirstValue("500")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Wait Time", PropertyValidationRule.ValidationRuleFlags.LessThanZero | PropertyValidationRule.ValidationRuleFlags.EqualsZero)]
        public static string v_WaitTimeAfterMouseClick { get; }
    }
}
