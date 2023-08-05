using System;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    internal static class ColorControls
    {
        /// <summary>
        /// color variable name virtual property
        /// </summary>
        [PropertyDescription("Color Variable Name")]
        [InputSpecification("")]
        [Remarks("")]
        [PropertyDetailSampleUsage("**vColor**", PropertyDetailSampleUsage.ValueType.VariableValue)]
        [PropertyDetailSampleUsage("**{{{vColor}}}**", PropertyDetailSampleUsage.ValueType.VariableValue)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Color, true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Variable")]
        public static string v_InputColorVariableName { get; }

        /// <summary>
        /// color value property
        /// </summary>
        [PropertyDescription("Color Value")]
        [InputSpecification("")]
        [PropertyDetailSampleUsage("**0**", "Specify value **0**. **0** is min value of range")]
        [PropertyDetailSampleUsage("**255**", "Specify value **255**. **255** is max value of range")]
        [Remarks("Values range from 0 to 255")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Value", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.NotBetween)]
        [PropertyValueRange(0, 255)]
        [PropertyDisplayText(true, "Value")]
        public static string v_ColorValue { get; }

        /// <summary>
        /// Get Color variable from Variable name. This type is System.Drawing.Color.
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception">Variable not Color</exception>
        public static System.Drawing.Color GetColorVariable(this string variableName, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            Script.ScriptVariable v = variableName.GetRawVariable(engine);
            if (v.VariableValue is System.Drawing.Color color)
            {
                return color;
            }
            else
            {
                throw new Exception("Variable " + variableName + " is not Color");
            }
        }

        public static void StoreInUserVariable(this System.Drawing.Color value, Core.Automation.Engine.AutomationEngineInstance sender, string targetVariable)
        {
            ExtensionMethods.StoreInUserVariable(targetVariable, value, sender);
        }
    }
}
