using Microsoft.Office.Interop.Word;
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

        /// <summary>
        /// get Dictionary&lt;string, string&gt; and key name from property names. It supports current position to key.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="dictionaryName"></param>
        /// <param name="keyName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static (Dictionary<string, string>, string) GetDictionaryVariableAndKey(this ScriptCommand command, string dictionaryName, string keyName, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            string dicVariable = command.ConvertToUserVariable(dictionaryName, "Dictionary", engine);
            var v = dicVariable.GetRawVariable(engine);
            if (v.VariableValue is Dictionary<string, string> dictionary)
            {
                string keyVariable = command.ConvertToUserVariable(keyName, "Key", engine);
                string key;
                if (String.IsNullOrEmpty(keyVariable))
                {
                    int pos = v.CurrentPosition;
                    string[] keys = dictionary.Keys.ToArray();
                    if ((pos >= 0) && (pos < keys.Length))
                    {
                        key = keys[pos];
                    }
                    else
                    {
                        throw new Exception("Strange Current Position in Dictionary " + pos);
                    }
                }
                else
                {
                    key = keyVariable.ConvertToUserVariable(engine);
                }
                return (dictionary, key);
            }
            else
            {
                throw new Exception("Variable " + dicVariable + " is not Dictionary");
            }
        }

        public static void StoreInUserVariable(this Dictionary<string, string> value, Core.Automation.Engine.AutomationEngineInstance sender, string targetVariable)
        {
            ExtensionMethods.StoreInUserVariable(targetVariable, value, sender, false);
        }
    }
}
