using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace taskt.Core.Automation.Commands
{
    internal static class DisplayTextControls
    {
        public static string GetPropertyDisplayValue(PropertyInfo prop, ScriptCommand command)
        {
            object value = prop.GetValue(command);

            Attributes.PropertyAttributes.PropertyDisplayText dispProp = (Attributes.PropertyAttributes.PropertyDisplayText)prop.GetCustomAttribute(typeof(Attributes.PropertyAttributes.PropertyDisplayText));
            if ((dispProp == null) || (!dispProp.parameterDisplay))
            {
                return "";
            }
            else
            {
                string dispValue;
                if (value == null)
                {
                    dispValue = "''";
                }
                else if (value is System.Data.DataTable)
                {
                    dispValue = ((System.Data.DataTable)value).Rows.Count + " items";
                }
                else if (!(value is string))
                {
                    dispValue = "'" + value.ToString() + "'";
                }
                else
                {
                    dispValue = "'" + value + "'";
                }

                if (dispProp.afterText != "")
                {
                    return dispProp.parameterName + ": " + dispValue + " " + dispProp.afterText + ", ";
                }
                else
                {
                    return dispProp.parameterName + ": " + dispValue + ", ";
                }
            }
        }
    }
}
