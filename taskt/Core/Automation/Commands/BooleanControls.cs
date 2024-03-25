﻿using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// methods for Boolean
    /// </summary>
    internal static class BooleanControls
    {
        /// <summary>
        /// output variable property
        /// </summary>
        [XmlAttribute]
        [PropertyDescription("Variable Name to Store Result")]
        [InputSpecification("")]
        [PropertyDetailSampleUsage("**vResult**", "Specify Variable Name **vResult**")]
        [PropertyDetailSampleUsage("**{{{vResult}}}**", "Specify Variable Name **vResult**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsVariablesList(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Result")]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Boolean, true)]
        [PropertyParameterOrder(5000)]
        public static string v_Result { get; }

        /// <summary>
        /// expand value or user variable as Boolean Value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception">value is not Boolean</exception>
        public static bool ExpandValueOrUserVariableAsBool(this string value, string parameterName, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            string convertedText = value.ExpandValueOrUserVariable(engine);
            if (bool.TryParse(convertedText, out bool v))
            {
                return v;
            }
            else
            {
                throw new Exception(parameterName + " '" + value + "' is not a boolean.");
            }
        }

        public static void StoreInUserVariable(this bool value, Engine.AutomationEngineInstance engine, string targetVariable)
        {
            ExtensionMethods.StoreInUserVariable(targetVariable, value ? "TRUE" : "FALSE", engine, false);
        }
    }
}
