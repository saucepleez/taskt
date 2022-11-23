using System;
using Newtonsoft.Json.Linq;

namespace taskt.Core.Automation.Commands
{
    static internal class JSONControls
    {
        /// <summary>
        /// get JSON text from text value or variable contains text. this method returns root type "object" or "array".
        /// </summary>
        /// <param name="jsonValue"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static (string json, string rootType) ConvertToUserVariableAsJSON(this string jsonValue, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            var jsonText = jsonValue.ConvertToUserVariable(engine).Trim();
            if (jsonText.StartsWith("{") && jsonText.EndsWith("}"))
            {
                try
                {
                    var _ = JObject.Parse(jsonText);
                    return (jsonText, "object");
                }
                catch (Exception ex)
                {
                    throw new Exception("Fail parse JSON Object. " + ex.ToString());
                }
            }
            else if (jsonText.StartsWith("[") && jsonText.EndsWith("]"))
            {
                try
                {
                    var _ = JArray.Parse(jsonText);
                    return (jsonText, "array");
                }
                catch (Exception ex)
                {
                    throw new Exception("Fail parse JSON Object. " + ex.ToString());
                }
            }
            else
            {
                throw new Exception("Strange JSON. First 10 chars '" + jsonText.Substring(0, ((jsonText.Length) >= 10 ? 10 : jsonText.Length)) + "'");
            }
        }

        /// <summary>
        /// get JSON root type from string
        /// </summary>
        /// <param name="jsonText"></param>
        /// <returns></returns>
        public static string GetJSONType(string jsonText)
        {
            jsonText = jsonText.Trim();
            if (jsonText.StartsWith("{") || jsonText.EndsWith("}"))
            {
                return "Object";
            }
            else if (jsonText.StartsWith("[") || jsonText.EndsWith("]"))
            {
                return "Array";
            }
            else if (jsonText.ToLower() == "true" || jsonText.ToLower() == "false")
            {
                return "bool";
            }
            else if (decimal.TryParse(jsonText, out _))
            {
                return "Number";
            }
            else
            {
                return "Text";
            }
        }

        /// <summary>
        /// edit JSON by methods passed as arguments, specify a target by JSONPath
        /// </summary>
        /// <param name="command"></param>
        /// <param name="jsonName"></param>
        /// <param name="extractorName"></param>
        /// <param name="objectAction"></param>
        /// <param name="arrayAction"></param>
        /// <param name="engine"></param>
        /// <exception cref="Exception"></exception>
        public static void JSONModifyByJSONPath(this ScriptCommand command, string jsonName, string extractorName, Action<JToken> objectAction, Action<JToken> arrayAction, Engine.AutomationEngineInstance engine)
        {
            string jsonVariableName = command.ConvertToUserVariable(jsonName, "JSON", engine);
            if (!engine.engineSettings.isWrappedVariableMarker(jsonVariableName))
            {
                jsonVariableName = engine.engineSettings.wrapVariableMarker(jsonVariableName);
            }
            string extractor = command.ConvertToUserVariable(extractorName, "Extractor", engine);
            (var jsonText, var rootType) = jsonVariableName.ConvertToUserVariableAsJSON(engine);
            switch(rootType)
            {
                case "object":
                    var obj = JObject.Parse(jsonText);
                    var objResult = obj.SelectToken(extractor);
                    if (objResult == null)
                    {
                        throw new Exception("No Property found JSONPath '" + extractor + "'");
                    }
                    objectAction(objResult);
                    obj.ToString().StoreInUserVariable(engine, jsonVariableName);
                    break;
                case "array":
                    var ary = JArray.Parse(jsonText);
                    var aryResult = ary.SelectToken(extractor);
                    if (aryResult == null)
                    {
                        throw new Exception("No Property found JSONPath '" + extractor + "'");
                    }
                    arrayAction(aryResult);
                    ary.ToString().StoreInUserVariable(engine, jsonVariableName);
                    break;
            }
        }
    }
}
