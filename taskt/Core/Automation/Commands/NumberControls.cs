﻿using System;
using System.Reflection;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using static taskt.Core.Automation.Commands.PropertyControls;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// convert text to number, store number to variable
    /// </summary>
    internal static class NumberControls
    {
        #region Virtual Property
        /// <summary>
        /// for both numerical variable name
        /// </summary>
        [PropertyDescription("Numerical Variable Name")]
        [InputSpecification("")]
        [Remarks("")]
        [PropertyDetailSampleUsage("**vNum**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vNum}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Both)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Numerical Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Numerical Name")]
        [PropertyParameterOrder(5000)]
        public static string v_BothNumericalVariableName { get; }

        /// <summary>
        /// for both numerical variable name
        /// </summary>
        [PropertyDescription("Numerical Variable Name")]
        [InputSpecification("")]
        [Remarks("")]
        [PropertyDetailSampleUsage("**vNum**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vNum}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Numerical Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Numerical Name")]
        [PropertyParameterOrder(5000)]
        public static string v_InputNumericalVariableName { get; }

        /// <summary>
        /// for output numerical variable name
        /// </summary>
        [PropertyDescription("Numerical Variable Name to Store Result")]
        [InputSpecification("")]
        [Remarks("")]
        [PropertyDetailSampleUsage("**vNum**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vNum}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Result")]
        [PropertyParameterOrder(5000)]
        public static string v_OutputNumericalVariableName { get; }

        /// <summary>
        /// for numerical value
        /// </summary>
        [PropertyDescription("Numerical Value")]
        [InputSpecification("Numerical Value", true)]
        [Remarks("")]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Value")]
        [PropertyDetailSampleUsage("**{{{vValue}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Value")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyValidationRule("Value", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Value")]
        [PropertyParameterOrder(5000)]
        public static string v_Value { get; }
        #endregion

        ///// <summary>
        ///// expand value or variable as Integer
        ///// </summary>
        ///// <param name="value"></param>
        ///// <param name="engine"></param>
        ///// <returns></returns>
        ///// <exception cref="Exception"></exception>
        //public static int ExpandValueOrVariableAsInteger(string value, Engine.AutomationEngineInstance engine)
        //{
        //    if (int.TryParse(value.ExpandValueOrUserVariable(engine), out int r))
        //    {
        //        return r;
        //    }
        //    else
        //    {
        //        throw new Exception($"'{value}' is not Integer (Numerical) value.");
        //    }
        //}

        ///// <summary>
        ///// expand value or variable as Double
        ///// </summary>
        ///// <param name="value"></param>
        ///// <param name="engine"></param>
        ///// <returns></returns>
        ///// <exception cref="Exception"></exception>
        //public static decimal ExpandValueOrVariableAsDecimal(string value, Engine.AutomationEngineInstance engine)
        //{
        //    if (decimal.TryParse(value.ExpandValueOrUserVariable(engine), out decimal r))
        //    {
        //        return r;
        //    }
        //    else
        //    {
        //        throw new Exception($"'{value}' is not Decimal (Numerical) value.");
        //    }
        //}

        ///// <summary>
        ///// expand value or variable as Decimal
        ///// </summary>
        ///// <param name="value"></param>
        ///// <param name="engine"></param>
        ///// <returns></returns>
        ///// <exception cref="Exception"></exception>
        //public static double ExpandValueOrVariableAsDouble(string value, Engine.AutomationEngineInstance engine)
        //{
        //    if (double.TryParse(value.ExpandValueOrUserVariable(engine), out double r))
        //    {
        //        return r;
        //    }
        //    else
        //    {
        //        throw new Exception($"'{value}' is not Double (Numerical) value.");
        //    }
        //}

        /// <summary>
        /// expand value or user variable as Integer specified by parameterName. This method supports validate, first value. This method may use PropertyValidationRule, PropertyDisplayText, PropertyDescription attributes.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="propertyName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref=""></exception>
        public static int ExpandValueOrUserVariableAsInteger(this ScriptCommand command, string propertyName, Engine.AutomationEngineInstance engine)
        {
            return ExpandValueOrUserVariableAsInteger(command, propertyName, "", engine);
        }

        /// <summary>
        /// expand value or user variable as integer specified by parameter name. This method supports validate, first value.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyDescription"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static int ExpandValueOrUserVariableAsInteger(this ScriptCommand command, string propertyName, string propertyDescription, Engine.AutomationEngineInstance engine)
        {
            decimal decValue = command.ExpandValueOrUserVariableAsDecimal(propertyName, propertyDescription, engine);
            try
            {
                int value = (int)decValue;
                return value;
            }
            catch
            {
                throw new Exception(propertyDescription + " is out of Integer Range.");
            }
        }

        /// <summary>
        /// expand value or user variable as Decimal specified by parameter name. This method supports validate, first value. This method may use PropertyValidationRule, PropertyDisplayText, PropertyDescription attributes.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="propertyName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref=""></exception>
        public static decimal ExpandValueOrUserVariableAsDecimal(this ScriptCommand command, string propertyName, Engine.AutomationEngineInstance engine)
        {
            return ExpandValueOrUserVariableAsDecimal(command, propertyName, "", engine);
        }

        /// <summary>
        /// expand value or user variable as Decimal specified by parameter name. This method supports validate, first value.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyDescription"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static decimal ExpandValueOrUserVariableAsDecimal(this ScriptCommand command, string propertyName, string propertyDescription, Engine.AutomationEngineInstance engine)
        {
            var propInfo = command.GetProperty(propertyName);
            string valueStr = propInfo.GetValue(command)?.ToString() ?? "";

            return new PropertyConvertTag(valueStr, propertyName, propertyDescription).ExpandValueOrUserVariableAsDecimal(propInfo, engine);
        }

        /// <summary>
        /// expand value or user variable as specified by PropertyConvertTag that specified value, description.
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static int ExpandValueOrUserVariableAsInteger(this PropertyConvertTag prop, Engine.AutomationEngineInstance engine)
        {
            string convertedText = prop.Value.ExpandValueOrUserVariable(engine);
            if (int.TryParse(convertedText, out int v))
            {
                return v;
            }
            else
            {
                throw new Exception(prop.Description + " '" + prop.Value + "' is not a integer number.");
            }
        }

        /// <summary>
        /// expand value or user variable as integer
        /// </summary>
        /// <param name="propertyValue"></param>
        /// <param name="propertyDescription"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static int ExpandValueOrUserVariableAsInteger(this string propertyValue, string propertyDescription, Engine.AutomationEngineInstance engine)
        {
            return new PropertyConvertTag(propertyValue, propertyDescription).ExpandValueOrUserVariableAsInteger(engine);
        }

        /// <summary>
        /// expand value or user variable as decimal specified by PropertyConvertTag that specified value, description.
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static decimal ExpandValueOrUserVariableAsDecimal(this PropertyConvertTag prop, Engine.AutomationEngineInstance engine)
        {
            string convertedText = prop.Value.ExpandValueOrUserVariable(engine);
            if (decimal.TryParse(convertedText, out decimal v))
            {
                return v;
            }
            else
            {
                throw new Exception(prop.Description + " '" + prop.Value + "' is not a number.");
            }
        }

        /// <summary>
        /// expand value or user variable as Decimal specified by PropertyConvertTag that specified property name, etc. This method supports validate, first value. This method may use PropertyValidationRule, PropertyDisplayText, PropertyDescription attributes.
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="propInfo"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static decimal ExpandValueOrUserVariableAsDecimal(this PropertyConvertTag prop, PropertyInfo propInfo, Engine.AutomationEngineInstance engine)
        {
            var virtualPropInfo = propInfo.GetVirtualProperty();

            var optAttr = GetCustomAttributeWithVirtual<PropertyIsOptional>(propInfo, virtualPropInfo);
            if (optAttr != null)
            {
                if ((optAttr.setBlankToValue != "") && (String.IsNullOrEmpty(prop.Value)))
                {
                    prop.SetNewValue(optAttr.setBlankToValue);
                }
            }

            if (prop.Description == "")
            {
                // try get description
                var attrValidate = GetCustomAttributeWithVirtual<PropertyValidationRule>(propInfo, virtualPropInfo);
                if (attrValidate != null)
                {
                    prop.SetNewDescription(attrValidate.parameterName);
                }
                else
                {
                    var attrDisp = GetCustomAttributeWithVirtual<PropertyDisplayText>(propInfo, virtualPropInfo);
                    if (attrDisp != null)
                    {
                        prop.SetNewDescription(attrDisp.parameterName);
                    }
                    else
                    {
                        prop.SetNewDescription(GetCustomAttributeWithVirtual<PropertyDescription>(propInfo, virtualPropInfo).propertyDescription);
                    }
                }
            }

            decimal v = prop.ExpandValueOrUserVariableAsDecimal(engine);

            var validateAttr = GetCustomAttributeWithVirtual<PropertyValidationRule>(propInfo, virtualPropInfo);
            var rangeAttr = GetCustomAttributeWithVirtual<PropertyValueRange>(propInfo, virtualPropInfo);
            if (validateAttr != null)
            {
                if (CheckValidate(v, validateAttr, prop.Description, rangeAttr))
                {
                    return v;
                }
                else
                {
                    throw new Exception("Validation Error");
                }
            }
            else
            {
                return v;
            }
        }

        /// <summary>
        /// check value validate by PropertyValidationRule
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validateAttr"></param>
        /// <param name="parameterDescription"></param>
        /// <param name="rangeAttr"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static bool CheckValidate(decimal value, PropertyValidationRule validateAttr, string parameterDescription, PropertyValueRange rangeAttr)
        {
            if (validateAttr.IsErrorFlag(PropertyValidationRule.ValidationRuleFlags.EqualsZero))
            {
                if (value == 0)
                {
                    throw new Exception(parameterDescription + " is Equals Zero.");
                }
            }
            if (validateAttr.IsErrorFlag(PropertyValidationRule.ValidationRuleFlags.NotEqualsZero))
            {
                if (value != 0)
                {
                    throw new Exception(parameterDescription + " is Not Equals Zero.");
                }
            }
            if (validateAttr.IsErrorFlag(PropertyValidationRule.ValidationRuleFlags.LessThanZero))
            {
                if (value < 0)
                {
                    throw new Exception(parameterDescription + " is Less Than Zero.");
                }
            }
            if (validateAttr.IsErrorFlag(PropertyValidationRule.ValidationRuleFlags.GreaterThanZero))
            {
                if  (value > 0)
                {
                    throw new Exception(parameterDescription + " is Greater Than Zero.");
                }
            }
            if (validateAttr.IsErrorFlag(PropertyValidationRule.ValidationRuleFlags.Between))
            {
                if (rangeAttr == null)
                {
                    throw new Exception(parameterDescription + " has no range attribute.");
                }
                else
                {
                    if (value >= (decimal)rangeAttr.min && value <= (decimal)rangeAttr.max)
                    {
                        throw new Exception(parameterDescription + " is in range (" + rangeAttr.min + " to " + rangeAttr.max + ")");
                    }
                }
            }
            if (validateAttr.IsErrorFlag(PropertyValidationRule.ValidationRuleFlags.NotBetween))
            {
                if (rangeAttr == null)
                {
                    throw new Exception(parameterDescription + " has no range attribute.");
                }
                else
                {
                    if (value < (decimal)rangeAttr.min || value > (decimal)rangeAttr.max)
                    {
                        throw new Exception(parameterDescription + " is out of range (" + rangeAttr.min + " to " + rangeAttr.max + ")");
                    }
                }
            }
            return true;
        }

        public static void StoreInUserVariable(this int value, Engine.AutomationEngineInstance engine, string targetVariable)
        {
            ExtensionMethods.StoreInUserVariable(targetVariable, value.ToString(), engine, false);
        }

        public static void StoreInUserVariable(this double value, Engine.AutomationEngineInstance engine, string targetVariable)
        {
            ExtensionMethods.StoreInUserVariable(targetVariable, value.ToString(), engine, false);
        }

        public static void StoreInUserVariable(this decimal value, Engine.AutomationEngineInstance engine, string targetVariable)
        {
            ExtensionMethods.StoreInUserVariable(targetVariable, value.ToString(), engine, false);
        }
    }
}
