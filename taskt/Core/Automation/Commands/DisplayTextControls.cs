using System.Reflection;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using static taskt.Core.Automation.Commands.VirtualPropertyControls;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// this class supports display text in editor
    /// </summary>
    internal static class DisplayTextControls
    {
        /// <summary>
        /// get paramters display text for editor from command, this method use PropertyDisplayText, PropertyVirtualProperty attributes.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static string GetParametersDisplayText(ScriptCommand command)
        {
            string t = "";
            var props = ScriptCommand.GetParameterProperties(command);
            foreach (var prop in props)
            {
                t += GetParameterDisplayValue(prop, command);
            }

            if (t != "")
            {
                t = t.Trim();
                t = t.Substring(0, t.Length - 1);
            }
            return t;
        }

        /// <summary>
        /// get parameter display value for editor, this method use PropertyDisplayText, PropertyVirtualProperty attributes.
        /// </summary>
        /// <param name="propInfo"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        private static string GetParameterDisplayValue(PropertyInfo propInfo, ScriptCommand command)
        {
            var virtualPropInfo = propInfo.GetVirtualProperty();
            
            var attrDisp = GetCustomAttributeWithVirtual<PropertyDisplayText>(propInfo, virtualPropInfo);

            if (attrDisp?.parameterDisplay ?? false)
            {
                object value = propInfo.GetValue(command);
                string dispValue;
                if (value == null)
                {
                    dispValue = "''";
                }
                else if (value is System.Data.DataTable table)
                {
                    dispValue = table.Rows.Count + " items";
                }
                else if (!(value is string))
                {
                    dispValue = "'" + value.ToString() + "'";
                }
                else
                {
                    dispValue = "'" + value + "'";
                }

                if (attrDisp.afterText != "")
                {
                    return attrDisp.parameterName + ": " + dispValue + " " + attrDisp.afterText + ", ";
                }
                else
                {
                    return attrDisp.parameterName + ": " + dispValue + ", ";
                }
            }
            else
            {
                return "";
            }
        }
    }
}
