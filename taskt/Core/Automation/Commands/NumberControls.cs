using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    internal static class NumberControls
    {
        public static int ConvertToUserVariableAsInteger(this string str, string parameterName, Engine.AutomationEngineInstance sender)
        {
            string convertedText = str.ConvertToUserVariable(sender);
            int v;
            if (int.TryParse(convertedText, out v))
            {
                return v;
            }
            else
            {
                throw new Exception(parameterName + " '" + str + "' is not a integer number.");
            }
        }

        public static decimal ConvertToUserVariableAsDecimal(this string str, string parameterName, Engine.AutomationEngineInstance sender)
        {
            string convertedText = str.ConvertToUserVariable(sender);
            decimal v;
            if (decimal.TryParse(convertedText, out v))
            {
                return v;
            }
            else
            {
                throw new Exception(parameterName + " '" + str + "' is not a number.");
            }
        }

        public static int ConvertToUserVariableAsInteger(this string variable, string propertyName, string parameterDescription, Engine.AutomationEngineInstance engine, ScriptCommand command)
        {
            decimal decValue = variable.ConvertToUserVariableAsDecimal(propertyName, parameterDescription, engine, command);
            int value;
            try
            {
                value = (int)decValue;
                return value;
            }
            catch
            {
                throw new Exception(parameterDescription + " is out of Integer Range.");
            }
        }
        public static decimal ConvertToUserVariableAsDecimal(this string variable, string propertyName, string parameterDescription, Engine.AutomationEngineInstance engine, ScriptCommand command)
        {
            var tp = command.GetType();
            var myProp = tp.GetProperty(propertyName);

            if (myProp == null)
            {
                throw new Exception("Property '" + propertyName + "' does not exists.");
            }

            var optAttr = (PropertyIsOptional)myProp.GetCustomAttribute(typeof(PropertyIsOptional));
            if (optAttr != null)
            {
                if ((optAttr.setBlankToValue != "") && (String.IsNullOrEmpty(variable)))
                {
                    variable = optAttr.setBlankToValue;
                }
            }

            decimal v = variable.ConvertToUserVariableAsDecimal(propertyName, engine);

            var validateAttr = (PropertyValidationRule)myProp.GetCustomAttribute(typeof(PropertyValidationRule));
            if (validateAttr != null)
            {
                if (CheckValidate(v, validateAttr, parameterDescription))
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
