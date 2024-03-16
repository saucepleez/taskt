using System;
using System.Collections.Generic;

namespace taskt.Core.Automation.Commands.Dictionary
{
    public static class EM_CanHandleDictionary
    {
        /// <summary>
        /// Expand user variable as Dictionary&lt;string, string&gt;
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception">Value is not Dictionary</exception>
        public static Dictionary<string, string> ExpandUserVariableAsDictinary(this ICanHandleDictionary command, string parameterName, Engine.AutomationEngineInstance engine)
        {
            var variableName = ((ScriptCommand)command).GetRawPropertyValueAsString(parameterName, "Dictionary Variable");
            var v = variableName.GetRawVariable(engine);
            if (v.VariableValue is Dictionary<string, string> dictionary)
            {
                return dictionary;
            }
            else
            {
                throw new Exception($"Variable '{variableName}' is not Dictionary");
            }
        }

        /// <summary>
        /// store Dictionary in User Variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="dic"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        public static void StoreDictionaryInUserVariable(this ICanHandleColor command, Dictionary<string, string> dic, string parameterName, Engine.AutomationEngineInstance engine)
        {
            var variableName = ((ScriptCommand)command).GetRawPropertyValueAsString(parameterName, "Dictionary Variable");
            ExtensionMethods.StoreInUserVariable(variableName, dic, engine);
        }
    }
}
