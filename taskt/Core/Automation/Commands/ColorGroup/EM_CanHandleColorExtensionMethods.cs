using System;
using System.Drawing;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    public static class EM_CanHandleColorExtensionMethods
    {
        /// <summary>
        /// Check object is Color
        /// </summary>
        /// <param name="value"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsColor(object value, out Color c)
        {
            // TODO: it's ok? not extension methods
            c = default;
            if (value is Color x)
            {
                c = x;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Expand User Variable as Color
        /// </summary>
        /// <param name="command"></param>
        /// <param name="variable"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Color ExpandUserVariableAsColor(ScriptVariable variable)
        {
            // TODO: it's ok? not extension methods
            if (IsColor(variable.VariableValue, out Color color))
            {
                return color;
            }
            else
            {
                throw new Exception($"Variable '{variable.VariableName}' is not Color");
            }
        }

        /// <summary>
        /// expand variable as Color
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Color ExpandUserVariableAsColor(this ICanHandleColor command, string parameterName, Engine.AutomationEngineInstance engine)
        {
            var variableName = command.ToScriptCommand().GetRawPropertyValueAsString(parameterName, "Color Variable");
            //var v = variableName.GetRawVariable(engine);
            //if (v.VariableValue is Color color)
            //{
            //    return color;
            //}
            //else
            //{
            //    throw new Exception($"Variable '{variableName}' is not Color");
            //}
            try
            {
                return ExpandUserVariableAsColor(variableName.GetRawVariable(engine));
            }
            catch
            {
                throw new Exception($"Variable '{variableName}' is not Color");
            }
        }

        /// <summary>
        /// store color in User Variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="c"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        public static void StoreColorInUserVariable(this ICanHandleColor command, Color c, string parameterName, Engine.AutomationEngineInstance engine)
        {
            var variableName = command.ToScriptCommand().GetRawPropertyValueAsString(parameterName, "Color Variable");
            ExtensionMethods.StoreInUserVariable(variableName, c, engine);
        }
    }
}
