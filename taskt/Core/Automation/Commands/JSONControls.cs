using System;
using Newtonsoft.Json.Linq;

namespace taskt.Core.Automation.Commands
{
    static internal class JSONControls
    {
        /// <summary>
        /// get JSON text from text value or variable contains text. this method returns root type "object" or "array".
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static (string json, string rootType) ConvertToUserVariableAsJSON(this string variableName, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            var jsonText = variableName.ConvertToUserVariable(engine).Trim();
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
                throw new Exception("Strange JSON. First 10 chars '" + jsonText.Substring(0, 10));
            }
        }

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
            else if (jsonText == "true" || jsonText == "false")
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
    }
}
