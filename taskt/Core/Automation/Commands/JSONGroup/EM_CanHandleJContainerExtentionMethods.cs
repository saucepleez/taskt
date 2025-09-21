using System;
using Newtonsoft.Json.Linq;

namespace taskt.Core.Automation.Commands
{
    public static class EM_CanHandleJContainerExtentionMethods
    {
        public enum JSONType
        {
            NotJSON,
            Object,
            Array
        };

        /// <summary>
        /// Detect JSON Type
        /// </summary>
        /// <param name="json"></param>
        /// <returns>(jsonText, JContainer, jsonType)</returns>
        public static (string, JContainer, JSONType) DetectJSONType(string json, Engine.AutomationEngineInstance engine)
        {
            if (EM_CanHandleJSONObjectExtensionMethods.IsJSONObject(json, engine, out (string str, JObject json) to))
            {
                return (to.str, to.json, JSONType.Object);
            }
            else if (EM_CanHandleJSONArrayExtentionMethods.IsJSONArray(json, engine, out (string str, JArray json) ta))
            {
                return (ta.str, ta.json, JSONType.Array);
            }
            else
            {
                return ("", null, JSONType.NotJSON);
            }
        }

        /// <summary>
        /// Expand Value or User Variable As JSON
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        /// <returns>(jsonText, JContainer, jsonType)</returns>
        public static (string, JContainer, string) ExpandValueOrUserVariableAsJSON(this ICanHandleJContainer command, string parameterName, Engine.AutomationEngineInstance engine)
        {
            var text = command.ToScriptCommand().ExpandValueOrUserVariable(parameterName, "JSON", engine);
            (var jsonText, var jCon, var jsonType) = DetectJSONType(text, engine);
            switch (jsonType)
            {
                case JSONType.Object:
                case JSONType.Array:
                    return (jsonText, jCon, jsonType.ToString().ToLower());

                default:
                    throw new Exception($"Specified Value or User Variable is Not JSON.");
            }
        }
    }
}
