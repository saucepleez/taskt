using System;
using System.Collections.Generic;

namespace taskt.Core.Automation.Commands
{
    public static class EM_DictionaryKeyPropertiesExtensionMethods
    {
        /// <summary>
        /// Expand value or UserVariable as Dictionary, Key, and Value
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns>(Dictionary, key, value)</returns>
        /// <exception cref="Exception"></exception>
        public static (Dictionary<string, string> dic, string key, string value) ExpandValueOrUserVariableAsDictionaryKeyAndValue(this IDictionaryKeyProperties command, Engine.AutomationEngineInstance engine)
        {
            var dic = command.ExpandUserVariableAsDictionary(nameof(command.v_Dictionary), engine);
            var key = command.ToScriptCommand().ExpandValueOrUserVariable(nameof(command.v_Key), "Dictionary Key", engine);
            if (dic.ContainsKey(key))
            {
                return (dic, key, dic[key]);
            }
            else
            {
                throw new Exception($"Key does not Exists in Dictionary. Key: '{command.v_Key}', Expand Value: '{key}'");
            }
        }
    }
}
