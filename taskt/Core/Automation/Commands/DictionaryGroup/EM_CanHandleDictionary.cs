using System;
using System.Collections.Generic;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    public static class EM_CanHandleDictionary
    {
        /// <summary>
        /// check object is Dictionary
        /// </summary>
        /// <param name="value"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static bool IsDictionary(object value, out Dictionary<string, string> dic)
        {
            // TODO: it's OK?
            dic = default;
            if (value is Dictionary<string, string> d)
            {
                dic = d;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// create new Empty Dictioanry
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> CreateEmptyDictionary(this ICanHandleDictionary command)
        {
            // todo: is it ok?
            return new Dictionary<string, string>();
        }

        /// <summary>
        /// Expand User Variable As Dictioanry
        /// </summary>
        /// <param name="command"></param>
        /// <param name="variable"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Dictionary<string, string> ExpandUserVariableAsDictionary(ScriptVariable variable)
        {
            // TODO: it's ok?
            if (IsDictionary(variable.VariableValue, out Dictionary<string, string> dictionary))
            {
                return dictionary;
            }
            else
            {
                throw new Exception($"Variable '{variable.VariableName}' is not Dictionary");
            }
        }

        /// <summary>
        /// Expand user variable as Dictionary&lt;string, string&gt;
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception">Value is not Dictionary</exception>
        public static Dictionary<string, string> ExpandUserVariableAsDictionary(this ICanHandleDictionary command, string parameterName, Engine.AutomationEngineInstance engine)
        {
            var variableName = command.ToScriptCommand().GetRawPropertyValueAsString(parameterName, "Dictionary Variable");
            //var v = variableName.GetRawVariable(engine);
            //if (v.VariableValue is Dictionary<string, string> dictionary)
            //{
            //    return dictionary;
            //}
            //else
            //{
            //    throw new Exception($"Variable '{variableName}' is not Dictionary");
            //}
            try
            {
                return ExpandUserVariableAsDictionary(variableName.GetRawVariable(engine));
            }
            catch
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
        public static void StoreDictionaryInUserVariable(this ICanHandleDictionary command, Dictionary<string, string> dic, string parameterName, Engine.AutomationEngineInstance engine)
        {
            var variableName = command.ToScriptCommand().GetRawPropertyValueAsString(parameterName, "Dictionary Variable");
            ExtensionMethods.StoreInUserVariable(variableName, dic, engine);
        }
    }
}
