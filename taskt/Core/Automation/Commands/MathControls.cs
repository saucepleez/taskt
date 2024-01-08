using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    internal static class MathControls
    {
        /// <summary>
        /// angle type
        /// </summary>
        [PropertyDescription("Angle Value Type")]
        [InputSpecification("Angle Value Type", true)]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Radian")]
        [PropertyUISelectionOption("Degree")]
        [PropertyDisplayText(true, "Angle Value Type")]
        [PropertyIsOptional(true, "Radian")]
        [PropertyParameterOrder(5000)]
        public static string v_AngleType { get; }

        /// <summary>
        /// when value is out of range
        /// </summary>
        [XmlAttribute]
        [PropertyDescription("When Value is Out of Range")]
        [InputSpecification("When Value is Out of Range", true)]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Error")]
        [PropertyDisplayText(false, "")]
        [PropertyIsOptional(true, "Ignore")]
        [PropertyParameterOrder(5000)]
        public static string v_WhenValueIsOutOfRange { get; }

        /// <summary>
        /// convert angle value to radian value
        /// </summary>
        /// <param name="command"></param>
        /// <param name="value"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static double ConvertAngleValueToRadian(ITrignometricProperties command, double value, Engine.AutomationEngineInstance engine)
        {
            if (((ScriptCommand)command).ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_AngleType), engine) == "degree")
            {
                value = value * Math.PI / 180.0;
            }
            return value;
        }

        /// <summary>
        /// Trignometic Function Action
        /// </summary>
        /// <param name="command"></param>
        /// <param name="func"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static double TrignometicFunctionAction(ATrignometricCommand command, Func<double, double> func, Engine.AutomationEngineInstance engine)
        {
            var v = (double)command.ExpandValueOrUserVariableAsDecimal(nameof(command.v_Value), engine);

            v = ConvertAngleValueToRadian(command, v, engine);

            return func(v);
        }

        /// <summary>
        /// Inverse Trignometic Function Action
        /// </summary>
        /// <param name="command"></param>
        /// <param name="actionFunc"></param>
        /// <param name="rangeFunc">when out of range, rise a exception</param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static double InverseTrignometicFunctionAction(AInverseTrignometricCommand command, Func<double, double> actionFunc, Func<double, bool> rangeFunc, Engine.AutomationEngineInstance engine)
        {
            var v = (double)command.ExpandValueOrUserVariableAsDecimal(nameof(command.v_Value), engine);

            if (command.ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_WhenValueIsOutOfRange), engine) == "error")
            {
                if (!rangeFunc(v))
                {
                    throw new Exception("Value is Out of Range");
                }
            }

            var r = actionFunc(v);
            r = ConvertAngleValueToRadian(command, r, engine);
            return r;
        }

        /// <summary>
        /// Acos, Asin range check
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static bool CheckAcosAsinRange(double v)
        {
            return (v >= -1 && v <= 1);
        }
    }
}
