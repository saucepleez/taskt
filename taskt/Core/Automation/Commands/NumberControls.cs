using Microsoft.Office.Core;
using System;
using System.Reflection;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    internal static class NumberControls
    {
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
            var propInfo = command.GetType().GetProperty(propertyName) ?? throw new Exception("Property '" + propertyName + "' is not exists."); ;
            string valueStr = propInfo.GetValue(command)?.ToString() ?? "";

            return new PropertyConvertTag(valueStr, propertyName, propertyDescription).ConvertToUserVariableAsDecimal(propInfo, engine);
        }

        /// <summary>
        /// convert property value to integer from PropertyConvertTag that specified property name, etc.
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
            //return (propertyValue, propertyDescription).ConvertToUserVariableAsInteger(engine);
            return new PropertyConvertTag(propertyValue, propertyDescription).ConvertToUserVariableAsInteger(engine);
        }

        /// <summary>
        /// convert property value to decimal from PropertyConvertTag that specified property name, etc.
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

        //public static int ConvertToUserVariableAsInteger(this PropertyConvertTag prop, ScriptCommand command, Engine.AutomationEngineInstance engine)
        //{
        //    decimal decValue = prop.ConvertToUserVariableAsDecimal(command, engine);
        //    try
        //    {
        //        int value = (int)decValue;
        //        return value;
        //    }
        //    catch
        //    {
        //        throw new Exception(prop.Description + " is out of Integer Range.");
        //    }
        //}
        //public static int ConvertToUserVariableAsInteger(this string propertyValue, string propertyName, string propertyDescription, ScriptCommand command, Engine.AutomationEngineInstance engine)
        //{
        //    return new PropertyConvertTag(propertyValue, propertyName, propertyDescription).ConvertToUserVariableAsInteger(command, engine);
        //}

        //public static int ConvertToUserVariableAsInteger(this string propertyValue, string propertyName, string propertyDescription, Engine.AutomationEngineInstance engine, ScriptCommand command)
        //{
        //    //return (propertyValue, propertyName, propertyDescription).ConvertToUserVariableAsInteger(engine, command);
        //    return new PropertyConvertTag(propertyValue, propertyName, propertyDescription).ConvertToUserVariableAsInteger(command, engine);
        //}

        //public static decimal ConvertToUserVariableAsDecimal(this PropertyConvertTag prop, ScriptCommand command, Engine.AutomationEngineInstance engine)
        //{
        //    if (!prop.HasName)
        //    {
        //        throw new Exception("Property name does not specified.");
        //    }

        //    var tp = command.GetType();
        //    var myProp = tp.GetProperty(prop.Name);

        //    //if (myProp == null)
        //    //{
        //    //    throw new Exception("Property '" + prop.Name + "' does not exists.");
        //    //}

        //    //var optAttr = (PropertyIsOptional)myProp.GetCustomAttribute(typeof(PropertyIsOptional));
        //    //if (optAttr != null)
        //    //{
        //    //    if ((optAttr.setBlankToValue != "") && (String.IsNullOrEmpty(prop.Value)))
        //    //    {
        //    //        //prop.Value = optAttr.setBlankToValue;
        //    //        prop.SetNewValue(optAttr.setBlankToValue);
        //    //    }
        //    //}

        //    //decimal v = prop.ConvertToUserVariableAsDecimal(engine);

        //    //var validateAttr = (PropertyValidationRule)myProp.GetCustomAttribute(typeof(PropertyValidationRule));
        //    //if (validateAttr != null)
        //    //{
        //    //    if (CheckValidate(v, validateAttr, prop.Description))
        //    //    {
        //    //        return v;
        //    //    }
        //    //    else
        //    //    {
        //    //        throw new Exception("Validation Error");
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    return v;
        //    //}

        //    return prop.ConvertToUserVariableAsDecimal(myProp, engine);
        //}

        /// <summary>
        /// convert property value to decimal from PropertyConvertTag that specified property name, etc. This method supports validate, first value.
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="propInfo"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static decimal ConvertToUserVariableAsDecimal(this PropertyConvertTag prop, PropertyInfo propInfo, Engine.AutomationEngineInstance engine)
        {
            var optAttr = propInfo.GetCustomAttribute<PropertyIsOptional>();
            if (optAttr != null)
            {
                if ((optAttr.setBlankToValue != "") && (String.IsNullOrEmpty(prop.Value)))
                {
                    //prop.Value = optAttr.setBlankToValue;
                    prop.SetNewValue(optAttr.setBlankToValue);
                }
            }

            decimal v = prop.ConvertToUserVariableAsDecimal(engine);

            var validateAttr = propInfo.GetCustomAttribute<PropertyValidationRule>();
            var rangeAttr = propInfo.GetCustomAttribute<PropertyValueRange>();
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

        private static bool CheckValidate(decimal value, PropertyValidationRule validateAttr, string parameterDescription, PropertyValueRange rangeAttr)
        {
            var rule = validateAttr.errorRule;
            if ((rule & PropertyValidationRule.ValidationRuleFlags.EqualsZero) == PropertyValidationRule.ValidationRuleFlags.EqualsZero)
            {
                if (value == 0)
                {
                    throw new Exception(parameterDescription + " is Equals Zero.");
                }
            }
            if ((rule & PropertyValidationRule.ValidationRuleFlags.NotEqualsZero) == PropertyValidationRule.ValidationRuleFlags.NotEqualsZero)
            {
                if (value != 0)
                {
                    throw new Exception(parameterDescription + " is Not Equals Zero.");
                }
            }
            if ((rule & PropertyValidationRule.ValidationRuleFlags.LessThanZero) == PropertyValidationRule.ValidationRuleFlags.LessThanZero)
            {
                if (value < 0)
                {
                    throw new Exception(parameterDescription + " is Less Than Zero.");
                }
            }
            if ((rule & PropertyValidationRule.ValidationRuleFlags.GreaterThanZero) == PropertyValidationRule.ValidationRuleFlags.GreaterThanZero)
            {
                if  (value > 0)
                {
                    throw new Exception(parameterDescription + " is Greater Than Zero.");
                }
            }
            if ((rule & PropertyValidationRule.ValidationRuleFlags.Between) == PropertyValidationRule.ValidationRuleFlags.Between)
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
            if ((rule & PropertyValidationRule.ValidationRuleFlags.NotBetween) == PropertyValidationRule.ValidationRuleFlags.NotBetween)
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
