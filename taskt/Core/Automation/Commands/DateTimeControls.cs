using System;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// methods for DataTime
    /// </summary>
    internal static class DateTimeControls
    {
        /// <summary>
        /// input DateTime Variable property
        /// </summary>
        [PropertyDescription("DateTime Variable Name")]
        [InputSpecification("")]
        [PropertyDetailSampleUsage("**{{{vDateTime}}}**", PropertyDetailSampleUsage.ValueType.VariableValue)]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DateTime, true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyValidationRule("DateTime Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Variable")]
        public static string v_InputDateTime { get; }

        /// <summary>
        /// output DateTime Variable property
        /// </summary>
        [PropertyDescription("DateTime Variable Name")]
        [InputSpecification("")]
        [PropertyDetailSampleUsage("**vDateTime**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vDateTime}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DateTime, true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyValidationRule("DateTime Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Variable")]
        public static string v_OutputDateTime { get; }

        /// <summary>
        /// Get DateTime variable from Variable Name.
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception">Variable not DateTime</exception>
        public static DateTime GetDateTimeVariable(this string variableName, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            Script.ScriptVariable v = variableName.GetRawVariable(engine);
            if (v.VariableValue is DateTime time)
            {
                return time;
            }
            else
            {
                throw new Exception("Variable " + variableName + " is not DateTime");
            }
        }

        /// <summary>
        /// expand user variable as DateTime
        /// </summary>
        /// <param name="str"></param>
        /// <param name="parameterName"></param>
        /// <param name="sender"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static DateTime ConvertToUserVariableAsDateTime(this string str, string parameterName, object sender)
        {
            string convertedText = str.ConvertToUserVariable(sender);
            if (DateTime.TryParse(convertedText, out DateTime v))
            {
                return v;
            }
            else
            {
                throw new Exception(parameterName + " '" + str + "' is not a DateTime.");
            }
        }

        public static void StoreInUserVariable(this DateTime value, Core.Automation.Engine.AutomationEngineInstance sender, string targetVariable)
        {
            ExtensionMethods.StoreInUserVariable(targetVariable, value, sender, false);
        }
    }
}
