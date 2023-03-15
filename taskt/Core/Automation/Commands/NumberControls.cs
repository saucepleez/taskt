using System;
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
        public static string v_BothNumericalVariableName { get; }

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
        public static string v_Value { get; }


        /// <summary>
        /// Convert property value to integer from property name. This method supports validate, first value. This method may use PropertyValidationRule, PropertyDisplayText, PropertyDescription attributes.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="propertyName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref=""></exception>
        public static int ConvertToUserVariableAsInteger(this ScriptCommand command, string propertyName, Engine.AutomationEngineInstance engine)
        {
            return ConvertToUserVariableAsInteger(command, propertyName, "", engine);
        }

        /// <summary>
        /// Convert property value to integer from property name. This method supports validate, first value.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyDescription"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static int ConvertToUserVariableAsInteger(this ScriptCommand command, string propertyName, string propertyDescription, Engine.AutomationEngineInstance engine)
        {
            decimal decValue = command.ConvertToUserVariableAsDecimal(propertyName, propertyDescription, engine);
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
        /// Convert property value to decimal from property name. This method supports validate, first value. This method may use PropertyValidationRule, PropertyDisplayText, PropertyDescription attributes.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="propertyName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref=""></exception>
        public static decimal ConvertToUserVariableAsDecimal(this ScriptCommand command, string propertyName, Engine.AutomationEngineInstance engine)
        {
            return ConvertToUserVariableAsDecimal(command, propertyName, "", engine);
        }

        /// <summary>
        /// convert property value to decimal from property name. This method supports validate, first value.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyDescription"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static decimal ConvertToUserVariableAsDecimal(this ScriptCommand command, string propertyName, string propertyDescription, Engine.AutomationEngineInstance engine)
        {
            var propInfo = command.GetProperty(propertyName);
            string valueStr = propInfo.GetValue(command)?.ToString() ?? "";

            return new PropertyConvertTag(valueStr, propertyName, propertyDescription).ConvertToUserVariableAsDecimal(propInfo, engine);
        }

        /// <summary>
        /// convert specified value to int from PropertyConvertTag that specified value, description.
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static int ConvertToUserVariableAsInteger(this PropertyConvertTag prop, Engine.AutomationEngineInstance engine)
        {
            string convertedText = prop.Value.ConvertToUserVariable(engine);
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
        /// convert specified value to integer.
        /// </summary>
        /// <param name="propertyValue"></param>
        /// <param name="propertyDescription"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static int ConvertToUserVariableAsInteger(this string propertyValue, string propertyDescription, Engine.AutomationEngineInstance engine)
        {
            return new PropertyConvertTag(propertyValue, propertyDescription).ConvertToUserVariableAsInteger(engine);
        }

        /// <summary>
        /// convert specified value to decimal from PropertyConvertTag that specified value, description.
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static decimal ConvertToUserVariableAsDecimal(this PropertyConvertTag prop, Engine.AutomationEngineInstance engine)
        {
            string convertedText = prop.Value.ConvertToUserVariable(engine);
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
        /// convert property value to decimal from PropertyConvertTag that specified property name, etc. This method supports validate, first value. This method may use PropertyValidationRule, PropertyDisplayText, PropertyDescription attributes.
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="propInfo"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static decimal ConvertToUserVariableAsDecimal(this PropertyConvertTag prop, PropertyInfo propInfo, Engine.AutomationEngineInstance engine)
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

            decimal v = prop.ConvertToUserVariableAsDecimal(engine);

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
    }
}
