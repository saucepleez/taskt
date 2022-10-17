using System;
using System.Reflection;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    internal static class NumberControls
    {
        public static int ConvertToUserVariableAsInteger(this (string propertyValue, string propertyDescription) nv, Engine.AutomationEngineInstance engine)
        {
            string convertedText = nv.propertyValue.ConvertToUserVariable(engine);
            int v;
            if (int.TryParse(convertedText, out v))
            {
                return v;
            }
            else
            {
                throw new Exception(nv.propertyDescription + " '" + nv.propertyValue + "' is not a integer number.");
            }
        }

        public static int ConvertToUserVariableAsInteger(this string propertyValue, string propertyDescription, Engine.AutomationEngineInstance engine)
        {
            return (propertyValue, propertyDescription).ConvertToUserVariableAsInteger(engine);
        }

        public static decimal ConvertToUserVariableAsDecimal(this (string propertyValue, string propertyDescription) nv, Engine.AutomationEngineInstance engine)
        {
            string convertedText = nv.propertyValue.ConvertToUserVariable(engine);
            decimal v;
            if (decimal.TryParse(convertedText, out v))
            {
                return v;
            }
            else
            {
                throw new Exception(nv.propertyDescription + " '" + nv.propertyValue + "' is not a number.");
            }
        }

        public static decimal ConvertToUserVariableAsDecimal(this string propertyValue, string propertyDescription, Engine.AutomationEngineInstance engine)
        {
            return (propertyValue, propertyDescription).ConvertToUserVariableAsDecimal(engine);
        }

        public static int ConvertToUserVariableAsInteger(this (string propertyValue, string propertyName, string propertyDescription) kvd, Engine.AutomationEngineInstance engine, ScriptCommand command)
        {
            decimal decValue = kvd.ConvertToUserVariableAsDecimal(engine, command);
            int value;
            try
            {
                value = (int)decValue;
                return value;
            }
            catch
            {
                throw new Exception(kvd.propertyDescription + " is out of Integer Range.");
            }
        }

        public static int ConvertToUserVariableAsInteger(this string propertyValue, string propertyName, string propertyDescription, Engine.AutomationEngineInstance engine, ScriptCommand command)
        {
            return (propertyValue, propertyName, propertyDescription).ConvertToUserVariableAsInteger(engine, command);
        }

        public static decimal ConvertToUserVariableAsDecimal(this (string propertyValue, string propertyName, string propertyDescription) kvd, Engine.AutomationEngineInstance engine, ScriptCommand command)
        {
            var tp = command.GetType();
            var myProp = tp.GetProperty(kvd.propertyName);

            if (myProp == null)
            {
                throw new Exception("Property '" + kvd.propertyName + "' does not exists.");
            }

            var optAttr = (PropertyIsOptional)myProp.GetCustomAttribute(typeof(PropertyIsOptional));
            if (optAttr != null)
            {
                if ((optAttr.setBlankToValue != "") && (String.IsNullOrEmpty(kvd.propertyValue)))
                {
                    kvd.propertyValue = optAttr.setBlankToValue;
                }
            }

            decimal v = kvd.propertyValue.ConvertToUserVariableAsDecimal(kvd.propertyName, engine);

            var validateAttr = (PropertyValidationRule)myProp.GetCustomAttribute(typeof(PropertyValidationRule));
            if (validateAttr != null)
            {
                if (CheckValidate(v, validateAttr, kvd.propertyDescription))
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

        public static decimal ConvertToUserVariableAsDecimal(this string propertyValue, string propertyName, string propertyDescription, Engine.AutomationEngineInstance engine, ScriptCommand command)
        {
            return (propertyValue, propertyName, propertyDescription).ConvertToUserVariableAsDecimal(engine, command);
        }

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
