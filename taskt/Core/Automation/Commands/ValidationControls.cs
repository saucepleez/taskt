using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    internal static class ValidationControls
    {
        private enum ValidationTarget
        {
            Error = 0,
            Warning = 1,
        };

        private static bool CheckValidateByFlags(string propertyValue, PropertyInfo propInfo, PropertyValidationRule rule, ValidationTarget target)
        {
            // decide check target/method
            Func<PropertyValidationRule.ValidationRuleFlags, bool> checkFunc;
            switch (target)
            {
                case ValidationTarget.Error:
                    checkFunc = rule.IsErrorFlag;
                    break;
                case ValidationTarget.Warning:
                    checkFunc = rule.IsWarningFlag;
                    break;
                default:
                    return true;
            }

            var paramShortName = rule.parameterName;
            string validationResult = "";

            // none
            if (checkFunc(PropertyValidationRule.ValidationRuleFlags.None))
            {
                return true;
            }

            bool result = true;

            // check by flag
            if (checkFunc(PropertyValidationRule.ValidationRuleFlags.Empty))
            {
                if (string.IsNullOrEmpty(propertyValue))
                {
                    validationResult += paramShortName + " is empty.\n";
                    result = false;
                }
            }
            if (checkFunc(PropertyValidationRule.ValidationRuleFlags.LessThanZero))
            {
                if (decimal.TryParse(propertyValue, out decimal v))
                {
                    if (v < 0)
                    {
                        validationResult += paramShortName + " is less than zero.\n";
                        result = false;
                    }
                }
            }
            if (checkFunc(PropertyValidationRule.ValidationRuleFlags.GreaterThanZero))
            {
                if (decimal.TryParse(propertyValue, out decimal v))
                {
                    if (v > 0)
                    {
                        validationResult += paramShortName + " is greater than zero.\n";
                        result = false;
                    }
                }
            }
            if (checkFunc(PropertyValidationRule.ValidationRuleFlags.EqualsZero))
            {
                if (decimal.TryParse(propertyValue, out decimal v))
                {
                    if (v == 0)
                    {
                        validationResult += paramShortName + " is equals zero.\n";
                        result = false;
                    }
                }
            }
            if (checkFunc(PropertyValidationRule.ValidationRuleFlags.NotEqualsZero))
            {
                if (decimal.TryParse(propertyValue, out decimal v))
                {
                    if (v != 0)
                    {
                        validationResult += paramShortName + " is not equals zero.\n";
                        result = false;
                    }
                }
            }
            if (checkFunc(PropertyValidationRule.ValidationRuleFlags.NotSelectionOption))
            {
                if (!IsUISelectionValue(propertyValue, propertyValue, propInfo))
                {
                    validationResult += paramShortName + " is strange value '" + propertyValue + "'.\n";
                    result = false;
                }
            }
            if (checkFunc(PropertyValidationRule.ValidationRuleFlags.Between))
            {
                var rangeAttr = propInfo.GetCustomAttribute<Attributes.PropertyAttributes.PropertyValueRange>();
                if (rangeAttr != null)
                {
                    if (decimal.TryParse(propertyValue, out decimal v))
                    {
                        if (v >= (decimal)rangeAttr.min && v <= (decimal)rangeAttr.max)
                        {
                            validationResult += paramShortName + " is in range.\n";
                            result = false;
                        }
                    }
                }
                else
                {
                    validationResult += paramShortName + " is Between rule. But don't have Range.\n";
                    result = false;
                }
            }
            if (checkFunc(PropertyValidationRule.ValidationRuleFlags.NotBetween))
            {
                var rangeAttr = propInfo.GetCustomAttribute<Attributes.PropertyAttributes.PropertyValueRange>();
                if (rangeAttr != null)
                {
                    if (decimal.TryParse(propertyValue, out decimal v))
                    {
                        if (v < (decimal)rangeAttr.min || v > (decimal)rangeAttr.max)
                        {
                            validationResult += paramShortName + " is out of range.\n";
                            result = false;
                        }
                    }
                }
                else
                {
                    validationResult += paramShortName + " is NotBetween rule. But don't have Range.\n";
                    result = false;
                }
            }

            return result;
        }

        private static bool IsUISelectionValue(string propertyName, string value, PropertyInfo prop)
        {
            var options = (Attributes.PropertyAttributes.PropertyUISelectionOption[])prop.GetCustomAttributes(typeof(Attributes.PropertyAttributes.PropertyUISelectionOption));
            if (options.Length == 0)
            {
                return true;
            }

            var sensitive = (Attributes.PropertyAttributes.PropertySelectionValueSensitive)prop.GetCustomAttribute(typeof(Attributes.PropertyAttributes.PropertySelectionValueSensitive));

            Func<string, string> cnvFunc;
            if (sensitive.caseSensitive && sensitive.whiteSpaceSensitive)
            {
                cnvFunc = (v) =>
                {
                    return v;
                };
            }
            else if (sensitive.caseSensitive && !sensitive.whiteSpaceSensitive)
            {
                cnvFunc = (v) =>
                {
                    return v.Trim().Replace(" ", "").Replace("\t", "");
                };
            }
            else if (!sensitive.caseSensitive && sensitive.whiteSpaceSensitive)
            {
                cnvFunc = (v) =>
                {
                    return v.ToLower();
                };
            }
            else
            {
                cnvFunc = (v) =>
                {
                    return v.ToLower().Trim().Replace(" ", "").Replace("\t", "");
                };
            }

            value = cnvFunc(value);
            foreach (var option in options)
            {
                if (value == cnvFunc(option.uiOption))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
