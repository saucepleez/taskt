using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskt.Core.Automation.Commands
{
    internal static class DictionaryControls
    {
        /// <summary>
        /// get Dictionary&lt;string, string&gt; Variable from variable name
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Dictionary<string, string> GetDictionaryVariable(this string variableName, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            Script.ScriptVariable v = variableName.GetRawVariable(engine);
            if (v.VariableValue is Dictionary<string, string> dictionary)
            {
                return dictionary;
            }
            else
            {
                throw new Exception("Variable " + variableName + " is not Dictionary");
            }
        }

        public static void StoreInUserVariable(this Dictionary<string, string> value, Core.Automation.Engine.AutomationEngineInstance sender, string targetVariable)
        {
            ExtensionMethods.StoreInUserVariable(targetVariable, value, sender, false);
        }
    }
}
