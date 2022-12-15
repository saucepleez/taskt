using System;
using System.Reflection;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using static taskt.Core.Automation.Commands.PropertyControls;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// parameter value validation methods to ScriptCommand
    /// </summary>
    internal static class ValidationControls
    {
        private enum ValidationTarget
        {
            Error = 0,
            Warning = 1,
        };

        /// <summary>
        /// check validation in parameters value in command. this method use PropertyValidationRule, PropertyVirtualProperty attributes.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static (bool isValid, bool isWarning, string validationMessage) CheckValidation(ScriptCommand command)
        {
            bool isValid = true;
            bool isWarn = true;
            string message = "";

            var props = command.GetParameterProperties();

            foreach(var propInfo in props)
            {
                var virtualPropInfo = propInfo.GetVirtualProperty();
                var attrValidate = GetCustomAttributeWithVirtual<PropertyValidationRule>(propInfo, virtualPropInfo);
                if (attrValidate != null)
                {
                    object propValue = propInfo.GetValue(command);
                    if (propValue is System.Data.DataTable)
                    {
                        continue;   // DataTable is not checked
                    }
                    else
                    {
                        string value = propValue?.ToString() ?? "";
                        (var v, var mes) = CheckValidateByFlags(value, propInfo, virtualPropInfo, attrValidate, ValidationTarget.Error);
                        isValid &= v;
                        message += mes;

                        (var w, _) = CheckValidateByFlags(value, propInfo, virtualPropInfo, attrValidate, ValidationTarget.Warning);
                        isWarn &= w;
                    }
                }
            }
            return (isValid, isWarn, message);
        }

        /// <summary>
        /// check validation. this method use PropertyValidationRule, PropertyValueRange attributes.
        /// </summary>
        /// <param name="propertyValue"></param>
        /// <param name="propInfo"></param>
        /// <param name="virtualPropInfo"></param>
        /// <param name="rule"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        private static (bool result, string messasge) CheckValidateByFlags(string propertyValue, PropertyInfo propInfo, PropertyInfo virtualPropInfo, PropertyValidationRule rule, ValidationTarget target)
        {
            // Todo: use numbercontrols, selectioncontrols

            // decide check target/method
            Func<PropertyValidationRule.ValidationRuleFlags, bool> checkFunc;
            switch (target)
            {
                case ValidationTarget.Error:
                    checkFunc = rule.IsErrorFlag;
                    if (rule.errorRule == 0)
                    {
                        return (true, "");
                    }
                    break;
                case ValidationTarget.Warning:
                    checkFunc = rule.IsWarningFlag;
                    if (rule.warningRule==0)
                    {
                        return (true, "");
                    }
                    break;
                default:
                    return (true, "");
            }

            var paramShortName = rule.parameterName;
            string validationResult = "";

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
                if (!IsUISelectionValue(propertyValue, propInfo, virtualPropInfo))
                {
                    validationResult += paramShortName + " is strange value '" + propertyValue + "'.\n";
                    result = false;
                }
            }
            if (checkFunc(PropertyValidationRule.ValidationRuleFlags.Between))
            {
                var rangeAttr = GetCustomAttributeWithVirtual<PropertyValueRange>(propInfo, virtualPropInfo);
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
                var rangeAttr = GetCustomAttributeWithVirtual<PropertyValueRange>(propInfo, virtualPropInfo);
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

            return (result, validationResult);
        }

        /// <summary>
        /// check ui selection value. this method use PropertyUISelectionOption, PropertySelectionValueSensitive attributes.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <param name="propInfo"></param>
        /// <param name="virtualPropInfo"></param>
        /// <returns></returns>
        private static bool IsUISelectionValue(string value, PropertyInfo propInfo, PropertyInfo virtualPropInfo)
        {
            var options = GetCustomAttributesWithVirtual<PropertyUISelectionOption>(propInfo, virtualPropInfo);
            if (options.Count == 0)
            {
                return true;
            }

            var sensitive = GetCustomAttributeWithVirtual<PropertySelectionValueSensitive>(propInfo, virtualPropInfo) ?? new PropertySelectionValueSensitive();

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
