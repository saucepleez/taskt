using System;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.UI.CustomControls;

namespace taskt.Core.Automation.Commands
{
    static internal class JSONControls
    {
        /// <summary>
        /// input JSON Variable or Value
        /// </summary>
        [PropertyDescription("JSON Variable Name or JSON Value")]
        [InputSpecification("JSON Value or JSON Variable Name", true)]
        [PropertyDetailSampleUsage("**{ \"id\": 3, \"value\": \"Hello\" }**", "Specify the JSON Object Text")]
        [PropertyDetailSampleUsage("**[ 1, 2, \"Hello\" ]**", "Specify the JSON Array Text")]
        [PropertyDetailSampleUsage("**{{{vJSON}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "JSON")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.JSON)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyValidationRule("JSON", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "JSON")]
        public static string v_InputJSONName { get; }

        /// <summary>
        /// input JSON Variable (Variable Only)
        /// </summary>
        [PropertyDescription("JSON Variable Name")]
        [InputSpecification("JSON **Variable Name**", true)]
        [PropertyDetailSampleUsage("**{{{vJSON}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "JSON")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.JSON)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyValidationRule("JSON", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "JSON")]
        public static string v_InputJSONVariableName { get; }

        /// <summary>
        /// output JSON Variable
        /// </summary>
        [PropertyDescription("JSON Variable Name")]
        [InputSpecification("JSON **Variable Name**", true)]
        [PropertyDetailSampleUsage("**vJSON**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vJSON}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyShowSampleUsageInDescription(true)]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.JSON, true)]
        [PropertyValidationRule("JSON", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "JSON")]
        public static string v_OutputJSONName { get; }

        /// <summary>
        /// input & output JSON Variable
        /// </summary>
        [PropertyDescription("JSON Variable Name")]
        [InputSpecification("JSON **Variable Name**", true)]
        [PropertyDetailSampleUsage("**vJSON**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vJSON}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyShowSampleUsageInDescription(true)]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Both)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.JSON, true)]
        [PropertyValidationRule("JSON", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "JSON")]
        public static string v_BothJSONName { get; }

        /// <summary>
        /// JSON path
        /// </summary>
        [PropertyDescription("JSON Extractor (JSONPath)")]
        [InputSpecification("JSONPath", true)]
        [PropertyDetailSampleUsage("**$.id**", "Specify **id** for Root child node")]
        [PropertyDetailSampleUsage("**$..id**", "Specify Anywhere **id**")]
        [PropertyDetailSampleUsage("**{{{vPath}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "JSON Extractor")]
        [Remarks("See this URL for details. https://github.com/json-path/JsonPath")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyCustomUIHelper("JSONPath Helper", nameof(JSONControls) + "+" + nameof(lnkJsonPathHelper_Click))]
        [PropertyValidationRule("JSON Extractor", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Extractor")]
        public static string v_JSONPath { get; }

        /// <summary>
        /// Value type to Add JSON
        /// </summary>
        [PropertyDescription("Value Type to Add")]
        [InputSpecification("", true)]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Auto")]
        [PropertyUISelectionOption("Text")]
        [PropertyUISelectionOption("Number")]
        [PropertyUISelectionOption("null")]
        [PropertyUISelectionOption("bool")]
        [PropertyUISelectionOption("Object")]
        [PropertyUISelectionOption("Array")]
        [PropertyDetailSampleUsage("**Auto**", "Automatically determines the Value Type")]
        [PropertyDetailSampleUsage("**Text**", PropertyDetailSampleUsage.ValueType.Value, "Value Type")]
        [PropertyDetailSampleUsage("**Number**", PropertyDetailSampleUsage.ValueType.Value, "Value Type")]
        [PropertyDetailSampleUsage("**bool**", PropertyDetailSampleUsage.ValueType.Value, "Value Type")]
        [PropertyDetailSampleUsage("**Object**", "Specify JSON Object for Value Type")]
        [PropertyDetailSampleUsage("**Array**", "Specify Array Object for Value Type")]
        [PropertyIsOptional(true, "Auto")]
        [PropertyDisplayText(true, "Value Type")]
        public static string v_ValueType { get; }

        /// <summary>
        /// value to add property
        /// </summary>
        [PropertyDescription("Value to Add")]
        [InputSpecification("Value", true)]
        [PropertyDetailSampleUsage("**Hello**", "Add Text **Hello**")]
        [PropertyDetailSampleUsage("**1**", "Add Number **Hello**")]
        [PropertyDetailSampleUsage("**{{{vValue}}}**", "Add Value of Variable **vValue**")]
        [PropertyDetailSampleUsage("**{ \"id\": 1, \"value\": \"Hello\" }**", "Add JSON Object", false)]
        [PropertyDetailSampleUsage("**[ 1, 2, \"Hello\" ]**", "Add JSON Array", false)]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.MultiLineTextBox)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDisplayText(true, "Value")]
        public static string v_ValueToAdd { get; }

        /// <summary>
        /// property name property
        /// </summary>
        [PropertyDescription("Property Name")]
        [InputSpecification("Property Name", true)]
        [PropertyDetailSampleUsage("**Name**", PropertyDetailSampleUsage.ValueType.Value, "Property Name")]
        [PropertyDetailSampleUsage("**{{{vName}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Property Name")]
        [Remarks("")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Property Name", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Property Name")]
        public static string v_PropertyName { get; }

        /// <summary>
        /// array index property
        /// </summary>
        [PropertyDescription("Array Index")]
        [InputSpecification("")]
        [PropertyDetailSampleUsage("**0**", "Specify the First Index")]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Index")]
        [PropertyDetailSampleUsage("**{{{vIndex}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Index")]
        [Remarks("")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true, "Last Item")]
        [PropertyDisplayText(true, "Index")]
        public static string v_ArrayIndex { get; }

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
        /// edit JSON by methods passed as arguments, specify a target by JSONPath. JSON specified by Variable Name.
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

        /// <summary>
        /// do something to JSON (object, array) by methods passed as arguments. by default, JSON is text.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="jsonName"></param>
        /// <param name="objectAction"></param>
        /// <param name="arrayAction"></param>
        /// <param name="engine"></param>
        public static void JSONProcess(this ScriptCommand command, string jsonName, Action<JObject> objectAction, Action<JArray> arrayAction, Engine.AutomationEngineInstance engine, bool forceJSONVariable = false)
        {
            string jsonVariableName = command.ConvertToUserVariable(jsonName, "JSON", engine);
            if (forceJSONVariable)
            {
                if (!engine.engineSettings.isWrappedVariableMarker(jsonVariableName))
                {
                    jsonVariableName = engine.engineSettings.wrapVariableMarker(jsonVariableName);
                }
            }
            (var jsonText, var rootType) = jsonVariableName.ConvertToUserVariableAsJSON(engine);
            switch (rootType)
            {
                case "object":
                    objectAction(JObject.Parse(jsonText));
                    break;
                case "array":
                    arrayAction(JArray.Parse(jsonText));
                    break;
            }
        }

        /// <summary>
        /// get JSON value to add, insert, etc.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="jsonValueName"></param>
        /// <param name="valueTypeName"></param>
        /// <param name="purpose"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static object GetJSONValue(this ScriptCommand command, string jsonValueName, string valueTypeName, string purpose, Engine.AutomationEngineInstance engine)
        {
            string jsonValue = command.ConvertToUserVariable(jsonValueName, "Value to " + purpose, engine);
            string valueType = command.GetUISelectionValue(valueTypeName, "Value Type", engine);
            if (valueType == "auto")
            {
                valueType = GetJSONType(jsonValue).ToLower();
            }

            object ret = null;
            switch (valueType)
            {
                case "text":
                    ret = jsonValue;
                    break;

                case "number":
                    ret =  new PropertyConvertTag(jsonValue, "Value to " + purpose).ConvertToUserVariableAsDecimal(engine);
                    break;

                case "bool":
                    switch (jsonValue.ToLower())
                    {
                        case "true":
                        case "false":
                            ret = bool.Parse(jsonValue);
                            break;
                        default:
                            throw new Exception("Value To Set is NOT bool. Value '" + jsonValue + "'");
                    }
                    break;

                case "object":
                    try
                    {
                        ret = JObject.Parse(jsonValue);
                    }
                    catch
                    {
                        throw new Exception("Value To Set is NOT Object. Value '" + jsonValue + "'");
                    }
                    break;

                case "array":
                    try
                    {
                        ret= JArray.Parse(jsonValue);
                    }
                    catch
                    {
                        throw new Exception("Value To Set is NOT Array. Value '" + jsonValue + "'");
                    }
                    break;
            }
            return ret;
        }

        public static void lnkJsonPathHelper_Click(object sender, EventArgs e)
        {
            using (var fm = new UI.Forms.Supplement_Forms.frmJSONPathHelper())
            {
                if (fm.ShowDialog() == DialogResult.OK)
                {
                    var ctrl = ((CommandItemControl)sender).Tag;
                    if (ctrl is TextBox txt)
                    {
                        txt.Text = fm.JSONPath;
                    }
                    else if (ctrl is ComboBox cmb)
                    {
                        cmb.Text = fm.JSONPath;
                    }
                    else if (ctrl is DataGridView dgv)
                    {
                        dgv.CurrentCell.Value = fm.JSONPath;
                    }
                }
            }
        }
    }
}
