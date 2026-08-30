using Newtonsoft.Json.Linq;
using System;

namespace taskt.Core.Automation.Commands
{
    public static class EM_JSONValueActionPropertiesExtentionMethods
    {
        /// <summary>
        /// Expand Value or Variable Value As JSON In JSON Value
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static (string, JContainer) ExpandValueOrVariableValueAsJSONInJSONValue(this IJSONValueActionProperties command, Engine.AutomationEngineInstance engine)
        {
            (var str, var json, _) = command.ExpandValueOrUserVariableAsJSON(nameof(command.v_Value), engine);
            var t = command.ToScriptCommand().ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_ValueType), engine);
            switch (t)
            {
                case "auto":
                    return (str, json);
                    
                case "array":
                    if (json is JArray)
                    {
                        return (str, json);
                    }
                    else
                    {
                        throw new Exception($"Spceified Value is NOT JSON Array. Value: '{str}'");
                    }

                case "object":
                    if (json is JObject)
                    {
                        return (str, json);
                    }
                    else
                    {
                        throw new Exception($"Spceified Value is NOT JSON Object. Value: '{str}'");
                    }

                default:
                    throw new Exception($"Spceified Value is NOT JSON. Value: '{str}'");
            }
        }

        /// <summary>
        /// Expand Value or User Variable as JSON-Supported value in JSON Value
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static JToken ExpandValueOrVariableValueAsJSONSupportedValueInJSONValue(this IJSONValueActionProperties command, Engine.AutomationEngineInstance engine)
        {
            string str;
            JContainer json = null;
            try
            {
                (str, json, _) = command.ExpandValueOrUserVariableAsJSON(nameof(command.v_Value), engine);
            }
            catch
            {
                str = command.ToScriptCommand().ExpandValueOrUserVariable(nameof(command.v_Value), "Value", engine);
            }

            var t = command.ToScriptCommand().ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_ValueType), engine);

            if (json is JArray ary)
            {
                switch (t)
                {
                    case "array":
                    case "auto":
                        return ary;

                    default:
                        throw new Exception($"Spceified Value is NOT JSON Array. Value: '{str}'");
                }
            }
            else if (json is JObject obj)
            {
                switch (t)
                {
                    case "object":
                    case "auto":
                        return obj;

                    default:
                        throw new Exception($"Spceified Value is NOT JSON Object. Value: '{str}'");
                }
                
            }
            else if (decimal.TryParse(str, out decimal num))
            {
                switch (t)
                {
                    case "number":
                    case "auto":
                        return new JValue(num);

                    default:
                        throw new Exception($"Spceified Value is NOT Number. Value: '{str}'");
                }
            }
            else if (bool.TryParse(str, out bool tf))
            {
                switch (t)
                {
                    case "boolean":
                    case "auto":
                        return new JValue(tf);

                    default:
                        throw new Exception($"Spceified Value is NOT Boolean. Value: '{str}'");
                }
            }
            else if (str.ToLower() == "null")
            {
                switch (t)
                {
                    case "null":
                    case "auto":
                        return null;

                    default:
                        throw new Exception($"Spceified Value is NOT Null. Value: '{str}'");
                }
            }
            else
            {
                switch (t)
                {
                    case "text":
                    case "auto":
                        return new JValue(str);

                    default:
                        throw new Exception($"Spceified Value is NOT Text. Value: '{str}'");
                }
            }
        }
    }
}
