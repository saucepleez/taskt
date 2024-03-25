﻿using System;
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
        [PropertyParameterOrder(5000)]
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
        [PropertyParameterOrder(5000)]
        public static string v_OutputDateTime { get; }

        ///// <summary>
        ///// Expand user variable As DateTime
        ///// </summary>
        ///// <param name="variableName"></param>
        ///// <param name="engine"></param>
        ///// <returns></returns>
        ///// <exception cref="Exception">Value is not DateTime</exception>
        //public static DateTime ExpandUserVariableAsDateTime(this string variableName, Core.Automation.Engine.AutomationEngineInstance engine)
        //{
        //    Script.ScriptVariable v = variableName.GetRawVariable(engine);
        //    if (v.VariableValue is DateTime time)
        //    {
        //        return time;
        //    }
        //    else
        //    {
        //        throw new Exception("Variable " + variableName + " is not DateTime");
        //    }
        //}

        //public static void StoreInUserVariable(this DateTime value, Core.Automation.Engine.AutomationEngineInstance engine, string targetVariable)
        //{
        //    ExtensionMethods.StoreInUserVariable(targetVariable, value, engine, false);
        //}

        /// <summary>
        /// Convert value to DateTime
        /// </summary>
        /// <param name="str"></param>
        /// <param name="parameterName"></param>
        /// <param name="sender"></param>
        /// <returns></returns>
        /// <exception cref="Exception">fail convert value to DateTime</exception>
        public static DateTime ConvertValueToDateTime(this string str, string parameterName, Core.Automation.Engine.AutomationEngineInstance engine)
        { 
            string convertedText = str.ExpandValueOrUserVariable(engine);
            if (DateTime.TryParse(convertedText, out DateTime v))
            {
                return v;
            }
            else
            {
                throw new Exception(parameterName + " '" + str + "' is not a DateTime.");
            }
        }
    }
}
