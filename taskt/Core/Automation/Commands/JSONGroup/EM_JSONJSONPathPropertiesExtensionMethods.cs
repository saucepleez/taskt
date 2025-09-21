using Newtonsoft.Json.Linq;
using System;

namespace taskt.Core.Automation.Commands
{
    public static class EM_JSONJSONPathPropertiesExtensionMethods
    {
        /// <summary>
        /// Expand Value or User Variable as JSON by JSONPath
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns>(JContainer(all JSON), JToken, type)</returns>
        /// <exception cref="Exception"></exception>
        public static (JContainer, JToken, string) ExpandValueOrUserVariableAsJSONByJSONPath(this IJSONJSONPathProperties command, Engine.AutomationEngineInstance engine)
        {
            (_, var json, _) = command.ExpandValueOrUserVariableAsJSON(engine);
            var path = command.ToScriptCommand().ExpandValueOrUserVariable(nameof(command.v_JsonExtractor), "JSONPath", engine);

            var token = json.SelectToken(path);
            if (token is JObject obj)
            {
                return (json, obj, "object");
            }
            else if (token is JArray ary)
            {
                return (json, ary, "array");
            }
            else if (token is JValue v)
            {
                return (json, v, "value");
            }
            else
            {
                throw new Exception($"Nothing found in JSONPath. Path: '{command.v_JsonExtractor}', ExtractPath: '{path}'");
            }
        }

        /// <summary>
        /// User Variable as JSON by JSONPath
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns>(JContainer(all JSON), JToken, type)</returns>
        /// <exception cref="Exception"></exception>
        public static (JContainer, JToken, string) ExpandUserVariableAsJSONByJSONPath(this IJSONJSONPathProperties command, Engine.AutomationEngineInstance engine)
        {
            var sc = command.ToScriptCommand();

            // force wrapped json variable
            var jsonVariable = sc.GetRawPropertyValueAsString(nameof(command.v_Json), "JSON");
            if (!VariableNameControls.IsWrappedVariableMarker(jsonVariable, engine))
            {
                command.v_Json = VariableNameControls.GetWrappedVariableName(jsonVariable, engine);
            }

            (_, var json, _) = command.ExpandValueOrUserVariableAsJSON(engine);
            if (string.IsNullOrEmpty(command.v_JsonExtractor))
            {
                command.v_JsonExtractor = "$";
            }
            var path = sc.ExpandValueOrUserVariable(nameof(command.v_JsonExtractor), "JSONPath", engine);

            var token = json.SelectToken(path);
            if (token is JObject obj)
            {
                return (json, obj, "object");
            }
            else if (token is JArray ary)
            {
                return (json, ary, "array");
            }
            else if (token is JValue v)
            {
                return (json, v, "value");
            }
            else if (token is JProperty prop)
            {
                return (json, prop, "property");
            }
            else
            {
                throw new Exception($"Nothing found in JSONPath. Path: '{command.v_JsonExtractor}', ExtractPath: '{path}'");
            }
        }

        /// <summary>
        /// store JSON in User variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        public static void StoreJSONInUserVariable(this IJSONJSONPathProperties command, JContainer json, Engine.AutomationEngineInstance engine)
        {
            json.ToString().StoreInUserVariable(engine, command.v_Json);
        }
    }
}
