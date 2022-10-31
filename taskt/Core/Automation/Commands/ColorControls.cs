using System;

namespace taskt.Core.Automation.Commands
{
    internal static class ColorControls
    {
        /// <summary>
        /// Get Color variable from Variable name. This type is System.Drawing.Color.
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception">Variable not Color</exception>
        public static System.Drawing.Color GetColorVariable(this string variableName, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            Script.ScriptVariable v = variableName.GetRawVariable(engine);
            if (v.VariableValue is System.Drawing.Color color)
            {
                return color;
            }
            else
            {
                throw new Exception("Variable " + variableName + " is not Color");
            }
        }

        public static void StoreInUserVariable(this System.Drawing.Color value, Core.Automation.Engine.AutomationEngineInstance sender, string targetVariable)
        {
            ExtensionMethods.StoreInUserVariable(targetVariable, value, sender);
        }
    }
}
