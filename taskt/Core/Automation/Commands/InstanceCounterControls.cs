using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using static taskt.Core.Automation.Commands.PropertyControls;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// count up/down to instance counter
    /// </summary>
    internal static class InstanceCounterControls
    {
        public static void AddInstance(ScriptCommand command, InstanceCounter counter)
        {
            //var props = command.GetParameterProperties();
            //foreach (var prop in props)
            //{
            //    if (prop.GetValue(command) != null)
            //    {
            //        var virtualProp = prop.GetVirtualProperty();
            //        string insValue = prop.GetValue(command).ToString();

            //        var insType = GetCustomAttributeWithVirtual<PropertyInstanceType>(prop, virtualProp);
            //        var direction = GetCustomAttributeWithVirtual<PropertyParameterDirection>(prop, virtualProp);

            //        if ((insType != null) && (direction != null) &&
            //                (insType.instanceType != PropertyInstanceType.InstanceType.none))
            //        {
            //            if (direction.porpose == PropertyParameterDirection.ParameterDirection.Output)
            //            {
            //                counter.addInstance(insValue, insType, false);
            //            }
            //            counter.addInstance(insValue, insType, true);
            //        }
            //        else if ((insType != null) && (insType.instanceType != PropertyInstanceType.InstanceType.none))
            //        {
            //            counter.addInstance(insValue, insType, true);
            //        }
            //    }
            //}
            AddRemoveInstance(command, new Action<string, PropertyInstanceType, bool>((instanceName, instanceType, isUsed) =>
            {
                counter.addInstance(instanceName, instanceType, isUsed);
            }));
        }

        public static void RemoveInstance(ScriptCommand command, InstanceCounter counter)
        {
            //var props = command.GetParameterProperties();
            //foreach (var prop in props)
            //{

            //    if (prop.GetValue(command) != null)
            //    {
            //        var virtualProp = prop.GetVirtualProperty();
            //        string insValue = prop.GetValue(command).ToString();

            //        var insType = GetCustomAttributeWithVirtual<PropertyInstanceType>(prop, virtualProp);
            //        var direction = GetCustomAttributeWithVirtual<PropertyParameterDirection>(prop, virtualProp);

            //        if ((insType != null) && (direction != null) &&
            //                (insType.instanceType != PropertyInstanceType.InstanceType.none))
            //        {
            //            if (direction.porpose == PropertyParameterDirection.ParameterDirection.Output)
            //            {
            //                counter.removeInstance(insValue, insType, false);
            //            }
            //            counter.removeInstance(insValue, insType, true);
            //        }
            //        else if ((insType != null) && (insType.instanceType != PropertyInstanceType.InstanceType.none))
            //        {
            //            counter.removeInstance(insValue, insType, true);
            //        }
            //    }
            //}
            AddRemoveInstance(command, new Action<string, PropertyInstanceType, bool>((instanceName, instanceType, isUsed) =>
            {
                counter.removeInstance(instanceName, instanceType, isUsed);
            }));
        }

        private static void AddRemoveInstance(ScriptCommand command, Action<string, PropertyInstanceType, bool> countAction)
        {
            var props = command.GetParameterProperties();
            foreach (var prop in props)
            {

                if (prop.GetValue(command) != null)
                {
                    var virtualProp = prop.GetVirtualProperty();
                    string insValue = prop.GetValue(command).ToString();

                    var insType = GetCustomAttributeWithVirtual<PropertyInstanceType>(prop, virtualProp);
                    var direction = GetCustomAttributeWithVirtual<PropertyParameterDirection>(prop, virtualProp);

                    if ((insType != null) && (direction != null) &&
                            (insType.instanceType != PropertyInstanceType.InstanceType.none))
                    {
                        if (direction.porpose == PropertyParameterDirection.ParameterDirection.Output)
                        {
                            countAction(insValue, insType, false);
                        }
                        countAction(insValue, insType, true);
                    }
                    else if ((insType != null) && (insType.instanceType != PropertyInstanceType.InstanceType.none))
                    {
                        countAction(insValue, insType, true);
                    }
                }
            }
        }
    }
}
