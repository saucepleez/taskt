using System;

namespace taskt.Core.Automation.Commands
{
    public static class EM_InverseTrignometricPropertiesExtentionMethods
    {
        /// <summary>
        /// Inverse Trignometic Function Action
        /// </summary>
        /// <param name="command"></param>
        /// <param name="actionFunc"></param>
        /// <param name="rangeFunc">when out of range, rise a exception</param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static void InverseTrignometicFunctionAction(this IInverseTrignometricProperties command, Func<double, double> actionFunc, Func<double, bool> rangeFunc, Engine.AutomationEngineInstance engine)
        {
            //var v = (double)command.ExpandValueOrUserVariableAsDecimal(nameof(command.v_Value), engine);
            var v = command.ExpandValueOrVariableAsValue(engine);

            if (((ScriptCommand)command).ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_WhenValueIsOutOfRange), engine) == "error")
            {
                if (!rangeFunc(v))
                {
                    throw new Exception($"Value is Out of Range. Value: '{command.v_Value}', Expand Value: '{v}'");
                }
            }

            //var r = actionFunc(v);
            var r = command.ConvertAngleValueToRadian(actionFunc(v), engine);
            r.StoreInUserVariable(engine, command.v_Result);
        }
    }
}
