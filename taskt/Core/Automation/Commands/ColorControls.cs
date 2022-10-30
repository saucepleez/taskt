using System;

namespace taskt.Core.Automation.Commands
{
    internal static class ColorControls
    {
        public static System.Drawing.Color GetColorVariable(this string variableName, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            Script.ScriptVariable v = variableName.GetRawVariable(engine);
            if (v.VariableValue is System.Drawing.Color)
            {
                return (System.Drawing.Color)v.VariableValue;
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
