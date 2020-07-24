using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace taskt.Core.Script
{
    public static class ScriptElementExtensions
    {
        public static string Description(this Enum value)
        {
            var enumType = value.GetType();
            var field = enumType.GetField(value.ToString());
            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute),
                                                       false);
            return attributes.Length == 0
                ? value.ToString()
                : ((DescriptionAttribute)attributes[0]).Description;
        }

        public static List<string> Descriptions(this Array values)
        {
            List<string> descriptions =  new List<string>();
            string description;
            foreach (var value in values)
            {
                var enumType = value.GetType();
                var field = enumType.GetField(value.ToString());
                var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute),
                                                           false);
                description = attributes.Length == 0 ? value.ToString() : ((DescriptionAttribute)attributes[0]).Description;
                descriptions.Add(description);
            }
            return descriptions;
        }
    }
}
