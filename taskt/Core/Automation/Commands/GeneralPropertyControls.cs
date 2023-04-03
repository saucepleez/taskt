using System.Collections.Generic;
using System.Windows.Forms;
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
        [InputSpecification("")]
        [PropertyDetailSampleUsage("**vResult**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vResult}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsVariablesList(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Result")]
        public static string v_Result { get; }

        /// <summary>
        /// One line textbox property, new line not allow
        /// </summary>
        [PropertyDescription("Value")]
        [InputSpecification("")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.TextBox)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyDisplayText(true, "Value")]
        public static string v_DisallowNewLine_OneLineTextBox { get; }

        /// <summary>
        /// One line textbox property, new line allow
        /// </summary>
        [PropertyDescription("Value")]
        [InputSpecification("")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.TextBox)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDisplayText(true, "Value")]
        public static string v_OneLineTextBox { get; }

        /// <summary>
        /// Multi lines textbox property, new line allow
        /// </summary>
        [PropertyDescription("Value")]
        [InputSpecification("")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.MultiLineTextBox)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDisplayText(true, "Value")]
        public static string v_MultiLinesTextBox { get; }

        /// <summary>
        /// combobox (dropdown)
        /// </summary>
        [PropertyDescription("Value")]
        [InputSpecification("", true)]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyDisplayText(true, "Value")]
        public static string v_ComboBox { get; }

        /// <summary>
        /// show/hide Command parameter groups
        /// </summary>
        /// <param name="controlsList"></param>
        /// <param name="parameterName"></param>
        /// <param name="visible"></param>
        public static void SetVisibleParameterControlGroup(Dictionary<string, Control> controlsList, string parameterName, bool visible)
        {
            foreach (var ctrl in controlsList)
            {
                if (ctrl.Key.Contains(parameterName))
                {
                    ctrl.Value.Visible = visible;
                }
            }
        }
    }
}
