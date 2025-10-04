using System;
using Newtonsoft.Json.Linq;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    public static class EM_CanHandleJSONArrayExtentionMethods
    {
        /// <summary>
        /// Check object is JSONArray
        /// </summary>
        /// <param name="value"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public static bool IsJSONArray(object value, Engine.AutomationEngineInstance engine, out (string str, JArray json) r)
        {
            r = ("", default);
            if (value is string str)
            {
                str = str.Trim().ExpandValueOrUserVariable(engine);
                if (str.StartsWith("[") && str.EndsWith("]"))
                {
                    try
                    {
                        r.str = str;
                        r.json = JArray.Parse(str.Trim());
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Expand Value or User Variable as JSONArray
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static (string, JArray) ExpandValueOrUserVariableAsJSONArray(ScriptVariable variable, Engine.AutomationEngineInstance engine)
        {
            if (IsJSONArray(variable.VariableValue, engine, out (string, JArray) r))
            {
                return r;
            }
            else
            {
                throw new Exception($"Variable '{variable.VariableName}' is not JSONArray.");
            }
        }

        /// <summary>
        /// Expand Value or User Variable As JSONArray
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static (string, JArray) ExpandValueOrUserVariableAsJSONArray(this ICanHandleJSONArray command, string parameterName, Engine.AutomationEngineInstance engine)
        {
            var variableName = command.ToScriptCommand().GetRawPropertyValueAsString(parameterName, "JSON");
            try
            {
                return ExpandValueOrUserVariableAsJSONArray(variableName.GetRawVariable(engine), engine);
            }
            catch
            {
                throw new Exception($"Variable '{variableName}' is not JSON.");
            }
        }

        /// <summary>
        /// Store JSONArray in User Variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="json"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        public static void StoreJSONArrayInUserVariable(this ICanHandleJSONArray command, JArray json, string parameterName, Engine.AutomationEngineInstance engine)
        {
            command.StoreJSONArrayInUserVariable(json.ToString(), parameterName, engine);
        }

        /// <summary>
        /// Store JSONArray in User Variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="json"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        public static void StoreJSONArrayInUserVariable(this ICanHandleJSONArray command, string json, string parameterName, Engine.AutomationEngineInstance engine)
        {
            if (IsJSONArray(json.Trim(), engine, out (string str, JArray _) r))
            {
                r.str.StoreInUserVariable(engine, parameterName);
            }
            else
            {
                throw new Exception($"This value is not JSON Array. Value: '{json}'");
            }
        }
    }
}
