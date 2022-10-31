using System;
using System.Linq;
using System.Reflection;

namespace taskt.Core.Automation.Commands
{
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
        /// <exception cref="Exception"></exception>
        private static string GetUISelectionValue(PropertyInfo propInfo, string propertyValue, string propertyDescription, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            var attrIsOpt = propInfo.GetCustomAttribute<Core.Automation.Attributes.PropertyAttributes.PropertyIsOptional>();
            if (string.IsNullOrEmpty(propertyValue))
            {
                propertyValue = attrIsOpt?.setBlankToValue ?? "";
            }

            string value = propertyValue.ConvertToUserVariable(engine);

            var options = propInfo.GetCustomAttributes<Core.Automation.Attributes.PropertyAttributes.PropertyUISelectionOption>();
            if (options.Count() > 0)
            {
                var attrIsSensitive = propInfo.GetCustomAttribute<Core.Automation.Attributes.PropertyAttributes.PropertyValueSensitive>();
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
        /// Get selected value from property name.  This method supports check selection value, first value, case sensitive.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyDescription"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string GetUISelectionValue(this ScriptCommand command, string propertyName, string propertyDescription, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            var propInfo = command.GetType().GetProperty(propertyName) ?? throw new Exception("Property '" + propertyName + "' does not exists");

            string propertyValue = propInfo.GetValue(command)?.ToString() ?? "";

            return GetUISelectionValue(propInfo, propertyValue, propertyDescription, engine);
        }

        /// <summary>
        /// Get selected value from PropertyConvertTag that specified property name, etc.  This method supports check selection value, first value, case sensitive.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static string GetUISelectionValue(this PropertyConvertTag p, Core.Automation.Commands.ScriptCommand command, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            return GetUISelectionValue(command, p.Name, p.Description, engine);
        }

        /// <summary>
        /// Get & Convert selected value by specified target value, target property name.  This method supports check selection value, first value, case sensitive.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static string GetUISelectionValue(this string text, string propertyName, Core.Automation.Commands.ScriptCommand command, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            return new PropertyConvertTag(text, propertyName, "").GetUISelectionValue(command, engine);
        }
    }
}
