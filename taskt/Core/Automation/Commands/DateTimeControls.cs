using System;

namespace taskt.Core.Automation.Commands
{
    internal static class DateTimeControls
    {
        /// <summary>
        /// Convert Property Value to DateTime
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static DateTime GetDateTimeVariable(this string variableName, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            Script.ScriptVariable v = variableName.GetRawVariable(engine);
            if (v.VariableValue is DateTime time)
            {
                return time;
            }
            else
            {
                throw new Exception("Variable " + variableName + " is not DateTime");
            }
        }
        public static void StoreInUserVariable(this DateTime value, Core.Automation.Engine.AutomationEngineInstance sender, string targetVariable)
        {
            ExtensionMethods.StoreInUserVariable(targetVariable, value, sender, false);
        }
    }
}
