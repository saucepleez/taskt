using System;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{
    public static class EM_CanHandleDateTimExtensionMethods
    {
        /// <summary>
        /// expand value or variable as DateTime
        /// </summary>
        /// <param name="command"></param>
        /// <param name="paramterName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static DateTime ExpandValueOrVariableAsDateTime(this ICanHandleDateTime command, string paramterName, AutomationEngineInstance engine)
        {
            var variableName = ((ScriptCommand)command).GetRawPropertyValueAsString(paramterName, "DateTime");
            var v = variableName.GetRawVariable(engine);
            if (v.VariableValue is DateTime time)
            {
                return time;
            }
            else
            {
                throw new Exception($"Variable '{variableName}' is not DateTime");
            }
        }

        /// <summary>
        /// store DateTime in User Variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="c"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        public static void StoreDateTimeInUserVariable(this ICanHandleDateTime command, DateTime c, string parameterName, Engine.AutomationEngineInstance engine)
        {
            var variableName = ((ScriptCommand)command).GetRawPropertyValueAsString(parameterName, "DateTime Variable");
            ExtensionMethods.StoreInUserVariable(variableName, c, engine);
        }
    }
}
