using System;
using System.Reflection;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    internal static class NumberControls
    {
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

        //public static int ConvertToUserVariableAsInteger(this (string propertyValue, string propertyDescription) tpValueName, Engine.AutomationEngineInstance engine)
        //{
        //    string convertedText = tpValueName.propertyValue.ConvertToUserVariable(engine);
        //    if (int.TryParse(convertedText, out int v))
        //    {
        //        return v;
        //    }
        //    else
        //    {
        //        throw new Exception(tpValueName.propertyDescription + " '" + tpValueName.propertyValue + "' is not a integer number.");
        //    }
        //}

        public static int ConvertToUserVariableAsInteger(this string propertyValue, string propertyDescription, Engine.AutomationEngineInstance engine)
        {
            //return (propertyValue, propertyDescription).ConvertToUserVariableAsInteger(engine);
            return new PropertyConvertTag(propertyValue, propertyDescription).ConvertToUserVariableAsInteger(engine);
        }

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

        //public static decimal ConvertToUserVariableAsDecimal(this (string propertyValue, string propertyDescription) tpValueName, Engine.AutomationEngineInstance engine)
        //{
        //    return new PropertyData(tpValueName.propertyValue, tpValueName.propertyDescription).ConvertToUserVariableAsDecimal(engine);
        //}

        //public static decimal ConvertToUserVariableAsDecimal(this string propertyValue, string propertyDescription, Engine.AutomationEngineInstance engine)
        //{
        //    //return (propertyValue, propertyDescription).ConvertToUserVariableAsDecimal(engine);
        //    return new PropertyConvertTag(propertyValue, propertyDescription).ConvertToUserVariableAsDecimal(engine);
        //}

        public static int ConvertToUserVariableAsInteger(this PropertyConvertTag prop, Engine.AutomationEngineInstance engine, ScriptCommand command)
        {
            decimal decValue = prop.ConvertToUserVariableAsDecimal(engine, command);
            try
            {
                int value = (int)decValue;
                return value;
            }
            catch
            {
                throw new Exception(prop.Description + " is out of Integer Range.");
            }
        }

        //public static int ConvertToUserVariableAsInteger(this (string propertyValue, string propertyName, string propertyDescription) tpValueNameDesc, Engine.AutomationEngineInstance engine, ScriptCommand command)
        //{
        //    //decimal decValue = tpValueNameDesc.ConvertToUserVariableAsDecimal(engine, command);
        //    decimal decValue = new PropertyConvertTag(tpValueNameDesc.propertyValue, tpValueNameDesc.propertyName, tpValueNameDesc.propertyDescription).ConvertToUserVariableAsDecimal(engine, command);
        //    int value;
        //    try
        //    {
        //        value = (int)decValue;
        //        return value;
        //    }
        //    catch
        //    {
        //        throw new Exception(tpValueNameDesc.propertyDescription + " is out of Integer Range.");
        //    }
        //}

        public static int ConvertToUserVariableAsInteger(this string propertyValue, string propertyName, string propertyDescription, Engine.AutomationEngineInstance engine, ScriptCommand command)
        {
            //return (propertyValue, propertyName, propertyDescription).ConvertToUserVariableAsInteger(engine, command);
            return new PropertyConvertTag(propertyValue, propertyName, propertyDescription).ConvertToUserVariableAsInteger(engine, command);
        }

        public static decimal ConvertToUserVariableAsDecimal(this PropertyConvertTag prop, Engine.AutomationEngineInstance engine, ScriptCommand command)
        {
            var tp = command.GetType();
            var myProp = tp.GetProperty(prop.Name);

            if (myProp == null)
            {
                throw new Exception("Property '" + prop.Name + "' does not exists.");
            }

            var optAttr = (PropertyIsOptional)myProp.GetCustomAttribute(typeof(PropertyIsOptional));
            if (optAttr != null)
            {
                if ((optAttr.setBlankToValue != "") && (String.IsNullOrEmpty(prop.Value)))
                {
                    prop.Value = optAttr.setBlankToValue;
                }
            }

            decimal v = prop.ConvertToUserVariableAsDecimal(engine);

            var validateAttr = (PropertyValidationRule)myProp.GetCustomAttribute(typeof(PropertyValidationRule));
            if (validateAttr != null)
            {
                if (CheckValidate(v, validateAttr, prop.Description))
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

        //public static decimal ConvertToUserVariableAsDecimal(this (string propertyValue, string propertyName, string propertyDescription) tpValueNameDesc, Engine.AutomationEngineInstance engine, ScriptCommand command)
        //{
        //    var tp = command.GetType();
        //    var myProp = tp.GetProperty(tpValueNameDesc.propertyName);

        //    if (myProp == null)
        //    {
        //        throw new Exception("Property '" + tpValueNameDesc.propertyName + "' does not exists.");
        //    }

        //    var optAttr = (PropertyIsOptional)myProp.GetCustomAttribute(typeof(PropertyIsOptional));
        //    if (optAttr != null)
        //    {
        //        if ((optAttr.setBlankToValue != "") && (String.IsNullOrEmpty(tpValueNameDesc.propertyValue)))
        //        {
        //            tpValueNameDesc.propertyValue = optAttr.setBlankToValue;
        //        }
        //    }

        //    decimal v = tpValueNameDesc.propertyValue.ConvertToUserVariableAsDecimal(tpValueNameDesc.propertyName, engine);

        //    var validateAttr = (PropertyValidationRule)myProp.GetCustomAttribute(typeof(PropertyValidationRule));
        //    if (validateAttr != null)
        //    {
        //        if (CheckValidate(v, validateAttr, tpValueNameDesc.propertyDescription))
        //        {
        //            return v;
        //        }
        //        else
        //        {
        //            throw new Exception("Validation Error");
        //        }
        //    }
        //    else
        //    {
        //        return v;
        //    }
        //}

        //public static decimal ConvertToUserVariableAsDecimal(this string propertyValue, string propertyName, string propertyDescription, Engine.AutomationEngineInstance engine, ScriptCommand command)
        //{
        //    return (propertyValue, propertyName, propertyDescription).ConvertToUserVariableAsDecimal(engine, command);
        //}

        private static bool CheckValidate(decimal value, PropertyValidationRule validateAttr, string parameterDescription)
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
            return true;
        }
    }
}
