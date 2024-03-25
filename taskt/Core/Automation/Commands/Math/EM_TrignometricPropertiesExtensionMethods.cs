using System;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{
    public static class EM_TrignometricPropertiesExtensionMethods
    {
        /// <summary>
        /// convert angle value to radian
        /// </summary>
        /// <param name="command"></param>
        /// <param name="value"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static double ConvertAngleValueToRadian(this ITrignometricProperties command, double value, AutomationEngineInstance engine)
        {
            if (((ScriptCommand)command).ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_AngleType), engine) != "degree")
            {
                value = value * Math.PI / 180.0;
            }
            return value;
        }

        /// <summary>
        /// expand value or variable as Angle Value
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static double ExpandValueOrVariableAsAngleValue(this ITrignometricProperties command, AutomationEngineInstance engine)
        {
            var v = (double)((ScriptCommand)command).ExpandValueOrUserVariableAsDecimal(nameof(command.v_Value), engine);
            //if (command.ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_AngleType), engine) != "degree")
            //{
            //    v = v * Math.PI / 180.0;
            //}
            //return v;
            return command.ConvertAngleValueToRadian(v, engine);
        }

        /// <summary>
        /// Trignometic Function Action
        /// </summary>
        /// <param name="command"></param>
        /// <param name="func"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static void TrignometicFunctionAction(this ITrignometricProperties command, Func<double, double> func, Engine.AutomationEngineInstance engine)
        {
            var r = func(command.ExpandValueOrVariableAsAngleValue(engine));
            r.StoreInUserVariable(engine, command.v_Result);
        }
    }
}
