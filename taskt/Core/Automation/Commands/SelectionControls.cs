using System;
using System.Linq;
using System.Reflection;

namespace taskt.Core.Automation.Commands
{
    internal static class SelectionControls
    {
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

        public static string GetUISelectionValue(this ScriptCommand command, string propertyName, string propertyDescription, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            var propInfo = command.GetType().GetProperty(propertyName) ?? throw new Exception("Property '" + propertyName + "' does not exists");

            string propertyValue = propInfo.GetValue(command)?.ToString() ?? "";

            return GetUISelectionValue(propInfo, propertyValue, propertyDescription, engine);
        }

        public static string GetUISelectionValue(this PropertyConvertTag p, Core.Automation.Commands.ScriptCommand command, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            //var propInfo = command.GetType().GetProperty(p.Name);
            //var propIsOpt = (Core.Automation.Attributes.PropertyAttributes.PropertyIsOptional)propInfo.GetCustomAttribute(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyIsOptional));

            //string convText = (propIsOpt == null) ? "" : propIsOpt.setBlankToValue;
            //if (!string.IsNullOrEmpty(p.Value))
            //{
            //    convText = p.Value.ConvertToUserVariable(engine);
            //}

            //var options = (Core.Automation.Attributes.PropertyAttributes.PropertyUISelectionOption[])propInfo.GetCustomAttributes(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyUISelectionOption));
            //if (options.Length > 0)
            //{
            //    var propCaseSensitive = (Core.Automation.Attributes.PropertyAttributes.PropertyValueSensitive)propInfo.GetCustomAttribute(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyValueSensitive));
            //    bool isCaseSensitive = (propCaseSensitive == null) ? false : propCaseSensitive.caseSensitive;

            //    if (isCaseSensitive)
            //    {
            //        foreach (var opt in options)
            //        {
            //            if (convText == opt.uiOption)
            //            {
            //                return convText;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        convText = convText.ToLower();
            //        foreach (var opt in options)
            //        {
            //            if (convText == opt.uiOption.ToLower())
            //            {
            //                return convText;
            //            }
            //        }
            //    }

            //    // not found, throw error
            //    string description = p.Description;
            //    if (string.IsNullOrEmpty(description))
            //    {
            //        var desc = (Core.Automation.Attributes.PropertyAttributes.PropertyDescription)propInfo.GetCustomAttribute(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyDescription));
            //        description = desc.propertyDescription;
            //    }
            //    throw new Exception("Parameter '" + description + "' has strange value '" + p.Value + "'");
            //}
            //else
            //{
            //    return convText;
            //}

            return GetUISelectionValue(command, p.Name, p.Description, engine);
        }

        public static string GetUISelectionValue(this string text, string propertyName, Core.Automation.Commands.ScriptCommand command, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            //var prop = command.GetType().GetProperty(propertyName);
            //var propIsOpt = (Core.Automation.Attributes.PropertyAttributes.PropertyIsOptional)prop.GetCustomAttribute(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyIsOptional));

            //string convText = (propIsOpt == null) ? "" : propIsOpt.setBlankToValue;
            //if (!String.IsNullOrEmpty(text))
            //{
            //    convText = text.ConvertToUserVariable(engine);
            //}

            //var options = (Core.Automation.Attributes.PropertyAttributes.PropertyUISelectionOption[])prop.GetCustomAttributes(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyUISelectionOption));
            //if (options.Length > 0)
            //{
            //    var propCaseSensitive = (Core.Automation.Attributes.PropertyAttributes.PropertyValueSensitive)prop.GetCustomAttribute(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyValueSensitive));
            //    bool isCaseSensitive = (propCaseSensitive == null) ? false : propCaseSensitive.caseSensitive;

            //    if (isCaseSensitive)
            //    {
            //        foreach (var opt in options)
            //        {
            //            if (convText == opt.uiOption)
            //            {
            //                return convText;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        convText = convText.ToLower();
            //        foreach(var opt in options)
            //        {
            //            if (convText == opt.uiOption.ToLower())
            //            {
            //                return convText;
            //            }
            //        }
            //    }

            //    var desc = (Core.Automation.Attributes.PropertyAttributes.PropertyDescription)prop.GetCustomAttribute(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyDescription));
            //    throw new Exception("Parameter '" + desc.propertyDescription + "' has strange value '" + text + "'");
            //}
            //else
            //{
            //    return convText;
            //}
            return new PropertyConvertTag(text, propertyName, "").GetUISelectionValue(command, engine);
        }
    }
}
