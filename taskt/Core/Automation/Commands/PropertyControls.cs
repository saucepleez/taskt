using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// PropertyInfo methods to ScriptCommand
    /// </summary>
    internal static class PropertyControls
    {
        /// <summary>
        /// get parameters property info
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static List<PropertyInfo> GetParameterProperties(this ScriptCommand command, bool containsComment = false)
        {
            var props = command.GetType().GetProperties();
            if (containsComment)
            {
                return props.Where(p => (p.Name.StartsWith("v_"))).ToList();
            }
            else
            {
                return props.Where(p => (p.Name.StartsWith("v_") && (p.Name != "v_Comment"))).ToList();
            }
        }


        /// <summary>
        /// get PropertyInfo specified property name as argument
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static PropertyInfo GetProperty(this ScriptCommand command, string propertyName)
        {
            return command.GetType().GetProperty(propertyName) ?? throw new Exception("Property '" + propertyName + "' does not exists. Command: " + command.CommandName);
        }

        /// <summary>
        /// get PropertyInfo and VirtualPropertyInfo specified property name as argument
        /// </summary>
        /// <param name="command"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static (PropertyInfo propInfo, PropertyInfo virtualPropertyInfo) GetPropertyAndVirturalProperty(this ScriptCommand command, string propertyName)
        {
            var propInfo = command.GetProperty(propertyName);
            return (propInfo, GetVirtualProperty(propInfo));
        }

        /// <summary>
        /// get PropertyInfo from VirtualProperty
        /// </summary>
        /// <param name="propInfo"></param>
        /// <returns></returns>
        public static PropertyInfo GetVirtualProperty(this PropertyInfo propInfo)
        {
            var attrVP = propInfo.GetCustomAttribute<PropertyVirtualProperty>();
            if (attrVP == null)
            {
                return null;
            }
            var tp = Type.GetType("taskt.Core.Automation.Commands." + attrVP.className);
            return tp.GetProperty(attrVP.propertyName, BindingFlags.Public | BindingFlags.Static);
        }

        /// <summary>
        /// get CustomAttribute from Property or VirtualProperty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propInfo"></param>
        /// <param name="virtualPropInfo"></param>
        /// <returns></returns>
        public static T GetCustomAttributeWithVirtual<T>(PropertyInfo propInfo, PropertyInfo virtualPropInfo)
            where T : System.Attribute
        {
            return propInfo.GetCustomAttribute<T>() ?? virtualPropInfo?.GetCustomAttribute<T>() ?? null;
        }

        /// <summary>
        /// get CustomAttributes List from Property or VirtualProperty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propInfo"></param>
        /// <param name="virtualPropInfo"></param>
        /// <param name="margeAttributes">when the value is true, return value is merged attribute get from two PropertyInfo. when the value is false, return value is propInfo attribute has priority.</param>
        /// <returns></returns>
        public static List<T> GetCustomAttributesWithVirtual<T>(PropertyInfo propInfo, PropertyInfo virtualPropInfo, bool margeAttributes = true)
            where T : System.Attribute
        {
            if (margeAttributes)
            {
                var a = new List<T>();
                var attrV = virtualPropInfo?.GetCustomAttributes<T>().ToList() ?? new List<T>();
                if (attrV.Count > 0)
                {
                    a.AddRange(attrV);
                }
                var attrP = propInfo.GetCustomAttributes<T>().ToList();
                if (attrP.Count > 0)
                {
                    a.AddRange(attrP);
                }
                return a;
            }
            else
            {
                var a = propInfo.GetCustomAttributes<T>().ToList();
                if (a.Count == 0)
                {
                    return virtualPropInfo?.GetCustomAttributes<T>().ToList() ?? new List<T>();
                }
                else
                {
                    return a;
                }
            }
        }
    }
}
