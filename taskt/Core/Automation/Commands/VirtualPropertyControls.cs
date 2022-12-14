using System;
using System.Reflection;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    internal static class VirtualPropertyControls
    {
        /// <summary>
        /// get PropertyInfo from VirtualProperty
        /// </summary>
        /// <param name="propInfo"></param>
        /// <returns></returns>
        public static PropertyInfo GetVirtualProperty(PropertyInfo propInfo)
        {
            var attrVP = propInfo.GetCustomAttribute<PropertyVirtualProperty>();
            if (attrVP == null)
            {
                return null;
            }
            var tp = Type.GetType("taskt.Core.Automation.Commands." + attrVP.className);
            return tp.GetProperty(attrVP.propertyName, BindingFlags.Public | BindingFlags.Static);
        }
    }
}
