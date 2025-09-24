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
    public static class PropertyControls
    {
        #region Property methods
        /// <summary>
        /// get parameters property info
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static List<PropertyInfo> GetParameterProperties(this ScriptCommand command, bool containsComment = false)
        {
            var props = command.GetType().GetProperties();
            IEnumerable<PropertyInfo> ps;
            if (containsComment)
            {
                ps = props.Where(p => (p.Name.StartsWith("v_")));
            }
            else
            {
                ps = props.Where(p => (p.Name.StartsWith("v_") && (p.Name != "v_Comment")));
            }
            return ps.OrderBy(p => p.GetCustomAttribute<PropertyParameterOrder>()?.order ?? new PropertyParameterOrder().order).ToList();
        }


        /// <summary>
        /// get PropertyInfo specified property name as argument
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static PropertyInfo GetProperty(this ScriptCommand command, string propertyName)
        {
            return command.GetType().GetProperty(propertyName) ?? throw new Exception($"Property '{propertyName}' does not exists. Command: '{command.CommandName}'");
        }

        /// <summary>
        /// get PropertyInfo that has PropertyVirtualProperty specified by argument
        /// </summary>
        /// <param name="command"></param>
        /// <param name="vProp"></param>
        /// <returns></returns>
        public static PropertyInfo GetProperty(this ScriptCommand command, PropertyVirtualProperty vProp)
        {
            var props = GetParameterProperties(command);
            return props.Where(p =>
            {
                var vp = p.GetCustomAttribute<PropertyVirtualProperty>();

                return vp?.Equals(vProp) ?? false;
            }).FirstOrDefault();
        }

        /// <summary>
        /// get PropertyInfo that has PropertyVirtualProperty specified by argument
        /// </summary>
        /// <param name="props"></param>
        /// <param name="vProp"></param>
        /// <returns></returns>
        public static PropertyInfo GetProperty(this List<PropertyInfo> props, PropertyVirtualProperty vProp)
        {
            return props.Where(p =>
            {
                return p.GetCustomAttribute<PropertyVirtualProperty>()?.Equals(vProp) ?? false;
            }).FirstOrDefault();
        }

        /// <summary>
        /// Get Property Full Name, 'ClassName+PropertyName'
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static string GetPropertyFullName(PropertyInfo prop)
        {
            return $"{prop.DeclaringType.FullName}+{prop.Name}";
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
            //var tp = Type.GetType("taskt.Core.Automation.Commands." + attrVP.className);
            var tp = Type.GetType(CommandClassesControl.GetClassFullName(attrVP.className));
            return tp.GetProperty(attrVP.propertyName, BindingFlags.Public | BindingFlags.Static);
        }

        /// <summary>
        /// get CustomAttribute from Property or VirtualProperty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propInfo"></param>
        /// <param name="virtualPropInfo"></param>
        /// <returns></returns>
        public static T GetCustomAttributeWithVirtual<T>(PropertyInfo propInfo, PropertyInfo virtualPropInfo = null)
            where T : ASingleComamndPropertyAttribute
        {
            //return propInfo.GetCustomAttribute<T>() ?? virtualPropInfo?.GetCustomAttribute<T>() ?? null;

            var searchedProps = new List<string>();
            var currentProp = propInfo;
            while (true)
            {
                var fullName = GetPropertyFullName(currentProp);
                if (!searchedProps.Contains(fullName))
                {
                    searchedProps.Add(fullName);
                    var attr = currentProp.GetCustomAttribute<T>();
                    if (attr != null)
                    {
                        return attr;
                    }
                    else
                    {
                        var vPropInfo = currentProp.GetVirtualProperty();
                        if (vPropInfo != null)
                        {
                            currentProp = vPropInfo;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                else
                {
                    throw new Exception($"Property is cycled. Property: '{typeof(T).Name}', PropertyName: '{currentProp.Name}'");
                }
            }
        }

        /// <summary>
        /// get CustomAttributes List from Property or VirtualProperty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propInfo"></param>
        /// <param name="virtualPropInfo"></param>
        /// <param name="margeAttributes">when the value is true, return value is merged attribute get from two PropertyInfo. when the value is false, return value is propInfo attribute has priority.</param>
        /// <returns></returns>
        public static List<T> GetCustomAttributesWithVirtual<T>(PropertyInfo propInfo, PropertyInfo virtualPropInfo = null)
            where T : AMultiCommandPropertyAttribute
        {
            // DBG
            //Console.WriteLine("### wv : " + typeof(T).Name);

            MultiAttributesBehavior behavior = MultiAttributesBehavior.Merge;
            switch (typeof(T).Name)
            {
                case nameof(PropertyDetailSampleUsage):
                    behavior = GetCustomAttributeWithVirtual<PropertyDetailSampleUsageBehavior>(propInfo, virtualPropInfo)?.behavior ?? MultiAttributesBehavior.Merge;
                    break;
                case nameof(PropertyAddtionalParameterInfo):
                    behavior = GetCustomAttributeWithVirtual<PropertyAddtionalParameterInfoBehavior>(propInfo, virtualPropInfo)?.behavior ?? MultiAttributesBehavior.Merge;
                    break;
                case nameof(PropertyUIHelper):
                    behavior = GetCustomAttributeWithVirtual<PropertyUIHelperBehavior>(propInfo, virtualPropInfo)?.behavior ?? MultiAttributesBehavior.Merge;
                    break;
                case nameof(PropertyCustomUIHelper):
                    behavior = GetCustomAttributeWithVirtual<PropertyCustomUIHelperBehavior>(propInfo, virtualPropInfo)?.behavior ?? MultiAttributesBehavior.Merge;
                    break;
                case nameof(PropertyUISelectionOption):
                    behavior = GetCustomAttributeWithVirtual<PropertyUISelectionOptionBehavior>(propInfo, virtualPropInfo)?.behavior ?? MultiAttributesBehavior.Merge;
                    break;
                case nameof(PropertyAvailableSystemVariable):
                    behavior = GetCustomAttributeWithVirtual<PropertyAvailableSystemVariableBehavior>(propInfo, virtualPropInfo)?.behavior ?? MultiAttributesBehavior.Merge;
                    break;
            }

            if (behavior == MultiAttributesBehavior.Merge)
            {
                var a = new List<T>();
                //var attrV = virtualPropInfo?.GetCustomAttributes<T>().ToList() ?? new List<T>();
                //if (attrV.Count > 0)
                //{
                //    a.AddRange(attrV);
                //}
                //var attrP = propInfo.GetCustomAttributes<T>().ToList();
                //if (attrP.Count > 0)
                //{
                //    a.AddRange(attrP);
                //}

                var searchedProps = new List<string>();
                var currentProp = propInfo;
                while (true)
                {
                    var fullName = GetPropertyFullName(currentProp);
                    if (!searchedProps.Contains(fullName))
                    {
                        searchedProps.Add(fullName);
                        var attrs = currentProp.GetCustomAttributes<T>().ToList() ?? new List<T>();
                        if (attrs.Count > 0)
                        {
                            a.AddRange(attrs);
                        }

                        var vPropInfo = currentProp.GetVirtualProperty();
                        if (vPropInfo != null)
                        {
                            currentProp = vPropInfo;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        throw new Exception($"Property is cycled. Property: '{typeof(T).Name}', PropertyName: '{currentProp.Name}'");
                    }
                }

                return a;
            }
            else
            {
                //var a = propInfo.GetCustomAttributes<T>().ToList();
                //if (a.Count == 0)
                //{
                //    return virtualPropInfo?.GetCustomAttributes<T>().ToList() ?? new List<T>();
                //}
                //else
                //{
                //    return a;
                //}
                var searchedProps = new List<string>();
                var currentProp = propInfo;
                while (true)
                {
                    var fullName = GetPropertyFullName(currentProp);
                    if (!searchedProps.Contains(fullName))
                    {
                        searchedProps.Add(fullName);
                        var attrs = currentProp.GetCustomAttributes<T>().ToList() ?? new List<T>();
                        if (attrs.Count > 0)
                        {
                            return attrs;
                        }

                        var vPropInfo = currentProp.GetVirtualProperty();
                        if (vPropInfo != null)
                        {
                            currentProp = vPropInfo;
                        }
                        else
                        {
                            return new List<T>();
                        }
                    }
                    else
                    {
                        throw new Exception($"Property is cycled. Property: '{typeof(T).Name}', PropertyName: '{currentProp.Name}'");
                    }
                }
            }
        }
        #endregion
    }
}
