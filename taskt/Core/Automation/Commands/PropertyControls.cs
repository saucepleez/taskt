using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// PropertyInfo methods to ScriptCommand
    /// </summary>
    internal static class PropertyControls
    {
        public const string LabelPrefix = "lbl_";
        public const string Label2ndPrefix = "lbl2_";
        public const string HelperInfix = "_helper_";
        public const string CustomHelperInfix = "_customhelper_";
        public const string GroupPrefix = "group_";

        #region Property methods
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
                //if (vp != null)
                //{
                //    return vp.Equals(vProp);
                //}
                //else
                //{
                //    return false;
                //}
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
        public static List<T> GetCustomAttributesWithVirtual<T>(PropertyInfo propInfo, PropertyInfo virtualPropInfo)
            where T : System.Attribute
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
            }

            if (behavior == MultiAttributesBehavior.Merge)
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
        #endregion

        #region control search method
        public static List<Control> GetControlGroup(this List<Control> ctrls, string parameterName, string nextParameterName = "")
        {
            List<Control> ret = new List<Control>();

            int index;
            index = ctrls.FindIndex(t => (t.Name == GroupPrefix + parameterName));
            if (index >= 0)
            {
                ret.Add(ctrls[index]);
            }
            else
            {
                index = ctrls.FindIndex(t => (t.Name == LabelPrefix + parameterName));
                int last = (nextParameterName == "") ? ctrls.Count : ctrls.FindIndex(t => (t.Name == LabelPrefix + nextParameterName));

                for (int i = index; i < last; i++)
                {
                    ret.Add(ctrls[i]);
                }
            }

            return ret;
        }

        public static T GetPropertyControl<T>(this Dictionary<string, Control> controls, string propertyName) where T : Control
        {
            if (controls.ContainsKey(propertyName))
            {
                return (T)controls[propertyName];
            }
            else
            {
                throw new Exception("Control '" + propertyName + "' does not exists.");
            }
        }

        public static Label GetPropertyControlLabel(this Dictionary<string, Control> controls, string propertyName)
        {
            if (controls.ContainsKey(LabelPrefix + propertyName))
            {
                return (Label)controls[LabelPrefix + propertyName];
            }
            else
            {
                throw new Exception("Label '" + LabelPrefix + propertyName + "' does not exists.");
            }
        }

        public static Label GetPropertyControl2ndLabel(this Dictionary<string, Control> controls, string propertyName)
        {
            if (controls.ContainsKey(Label2ndPrefix + propertyName))
            {
                return (Label)controls[Label2ndPrefix + propertyName];
            }
            else
            {
                throw new Exception("2nd Label '" + Label2ndPrefix + propertyName + "' does not exists.");
            }
        }

        public static (T body, Label label, Label label2nd) GetAllPropertyControl<T>(this Dictionary<string, Control> controls, string propertyName, bool throwWhenLabelNotExists = true, bool throwWhen2ndLabelNotExists = false) where T : Control
        {
            T body = controls.GetPropertyControl<T>(propertyName);

            Label label;
            try
            {
                label = controls.GetPropertyControlLabel(propertyName);
            }
            catch (Exception ex)
            {
                if (throwWhenLabelNotExists)
                {
                    throw ex;
                }
                else
                {
                    label = null;
                }
            }
            Label label2nd;
            try
            {
                label2nd = controls.GetPropertyControl2ndLabel(propertyName);
            }
            catch (Exception ex)
            {
                if (throwWhen2ndLabelNotExists)
                {
                    throw ex;
                }
                else
                {
                    label2nd = null;
                }
            }
            return (body, label, label2nd);
        }

        public static Dictionary<string, string> Get2ndLabelText(this Dictionary<string, Control> controls, string propertyName)
        {
            return controls.GetPropertyControlLabel(propertyName).Get2ndLabelTexts();
        }

        public static Dictionary<string, string> Get2ndLabelTexts(this Label lbl)
        {
            if (lbl.Tag is Dictionary<string, string> dic)
            {
                return dic;
            }
            else
            {
                throw new Exception(lbl.Name + " does not has Dictionary item for 2nd-Label");
            }
        }

        public static void SecondLabelProcess(this Dictionary<string, Control> controls, string labelTextName, string label2ndName, string key)
        {
            var dic = controls.Get2ndLabelText(labelTextName);
            var lbl = controls.GetPropertyControl2ndLabel(label2ndName);

            lbl.Text = dic.ContainsKey(key) ? dic[key] : "";
        }
        #endregion
    }
}
