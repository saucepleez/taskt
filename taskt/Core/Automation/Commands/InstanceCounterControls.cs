using System;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using static taskt.Core.Automation.Commands.PropertyControls;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// count up/down to instance counter
    /// </summary>
    internal static class InstanceCounterControls
    {
        /// <summary>
        /// general method to add instance. this method use PropertyInstanceType, PropertyParameterDirection attributes.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="counter"></param>
        public static void AddInstance(ScriptCommand command, InstanceCounter counter)
        {
            ActionInstance(command, new Action<string, PropertyInstanceType, bool>((instanceName, instanceType, isUsed) =>
            {
                counter.addInstance(instanceName, instanceType, isUsed);
            }));
        }

        /// <summary>
        /// general method to remove instance. this method use PropertyInstanceType, PropertyParameterDirection attributes.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="counter"></param>
        public static void RemoveInstance(ScriptCommand command, InstanceCounter counter)
        {
            ActionInstance(command, new Action<string, PropertyInstanceType, bool>((instanceName, instanceType, isUsed) =>
            {
                counter.removeInstance(instanceName, instanceType, isUsed);
            }));
        }

        /// <summary>
        /// general method to do anything instance by argument Action<>. this method use PropertyInstanceType, PropertyParameterDirection attributes.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="countAction"></param>
        private static void ActionInstance(ScriptCommand command, Action<string, PropertyInstanceType, bool> countAction)
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
