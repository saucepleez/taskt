using System;
using System.Linq;
using System.Reflection;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using static taskt.Core.Automation.Commands.PropertyControls;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// methods for PropertyUISelection attributes
    /// </summary>
    internal static class SelectionControls
    {
        /// <summary>
        /// private GetUISelectionValue method. This method supports check selection value, first value, case sensitive.
        /// </summary>
        /// <param name="propInfo"></param>
        /// <param name="propertyValue"></param>
        /// <param name="propertyDescription"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception">value is not PropertyUISelectionOption</exception>
        private static string GetUISelectionValue(PropertyInfo propInfo, string propertyValue, string propertyDescription, Engine.AutomationEngineInstance engine)
        {
            var virtualPropInfo = propInfo.GetVirtualProperty();

            var attrIsOpt = GetCustomAttributeWithVirtual<PropertyIsOptional>(propInfo, virtualPropInfo);
            if (string.IsNullOrEmpty(propertyValue))
            {
                propertyValue = attrIsOpt?.setBlankToValue ?? "";
            }

            string value = propertyValue.ConvertToUserVariable(engine);

            var options = GetCustomAttributesWithVirtual<PropertyUISelectionOption>(propInfo, virtualPropInfo);
            if (options.Count() > 0)
            {
                var attrIsSensitive = GetCustomAttributeWithVirtual<PropertySelectionValueSensitive>(propInfo, virtualPropInfo);
                bool isSensitive = attrIsSensitive?.caseSensitive ?? false;

                Func<string, string, bool> chkFunc;
                if (isSensitive)
                {
                    chkFunc = (a, b) =>
                    {
                        return (a == b);
                    };
                }
                else
                {
                    value = value.ToLower();
                    chkFunc = (a, b) =>
                    {
                        return (a == b.ToLower());
                    };
                }
                foreach(var opt in options)
                {
                    if (chkFunc(value, opt.uiOption))
                    {
                        return value;
                    }
                }

                throw new Exception("Parameter '" + propertyDescription + "' has strange value '" + propertyValue + "'");
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// Get selected item value from property name.  This method supports check selection value, first value, case sensitive.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyDescription"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception">value is not PropertyUISelectionOption</exception>
        public static string GetUISelectionValue(this ScriptCommand command, string propertyName, string propertyDescription, Engine.AutomationEngineInstance engine)
        {
            var propInfo = command.GetProperty(propertyName);

            string propertyValue = propInfo.GetValue(command)?.ToString() ?? "";

            return GetUISelectionValue(propInfo, propertyValue, propertyDescription, engine);
        }

        /// <summary>
        /// Get selected item value from PropertyConvertTag that specified property name, etc.  This method supports check selection value, first value, case sensitive.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception">value is not PropertyUISelectionOption</exception>
        public static string GetUISelectionValue(this PropertyConvertTag p, ScriptCommand command, Engine.AutomationEngineInstance engine)
        {
            return GetUISelectionValue(command, p.Name, p.Description, engine);
        }

        /// <summary>
        /// Get or Convert to selected item value by specified target value, target property name.  This method supports check selection value, first value, case sensitive.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception">value is not PropertyUISelectionOption</exception>
        public static string GetUISelectionValue(this string text, string propertyName, ScriptCommand command, Engine.AutomationEngineInstance engine)
        {
            return new PropertyConvertTag(text, propertyName, "").GetUISelectionValue(command, engine);
        }
    }
}
