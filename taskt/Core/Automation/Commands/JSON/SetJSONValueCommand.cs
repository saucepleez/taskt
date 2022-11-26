using System;
using System.Xml.Serialization;
using System.Windows.Forms;
using taskt.UI.CustomControls;
using Newtonsoft.Json.Linq;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("JSON Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to set value in JSON.")]
    [Attributes.ClassAttributes.UsesDescription("")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SetJSONValueCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please Specify the JSON Variable Name")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**{{{vSomeVariable}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.JSON)]
        [PropertyValidationRule("JSON", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "JSON")]
        public string v_InputValue { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Specify a JSON extractor (JSONPath)")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Input a JSON token extractor")]
        [SampleUsage("**$.id**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyCustomUIHelper("JSONPath Helper", nameof(lnkJsonPathHelper_Click))]
        [PropertyValidationRule("JSON extractor", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Extractor")]
        public string v_JsonExtractor { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Specify Value to Set")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**Hello** or **{{{vNewValue}}}**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.MultiLineTextBox)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDisplayText(true, "Value")]
        public string v_ValueToSet { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Specify Value Type to Set")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**Text** or **Number** or **bool** or **Object** or **Array**")]
        [Remarks("")]
        [PropertyUISelectionOption("Auto")]
        [PropertyUISelectionOption("Text")]
        [PropertyUISelectionOption("Number")]
        [PropertyUISelectionOption("null")]
        [PropertyUISelectionOption("bool")]
        [PropertyUISelectionOption("Object")]
        [PropertyUISelectionOption("Array")]
        [PropertyIsOptional(true, "Auto")]
        [PropertyDisplayText(true, "Value Type")]
        public string v_ValueType { get; set; }

        public SetJSONValueCommand()
        {
            this.CommandName = "SetJSONValue";
            this.SelectionName = "Set JSON Value";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //var forbiddenMarkers = new List<string> { "[", "]" };

            //if (forbiddenMarkers.Any(f => f == engine.engineSettings.VariableStartMarker) || (forbiddenMarkers.Any(f => f == engine.engineSettings.VariableEndMarker)))
            //{
            //    throw new Exception("Cannot use Parse JSON command with square bracket variable markers [ ]");
            //}

            //var jsonText = v_InputValue.ConvertToUserVariable(sender).Trim();

            //var jsonSearchToken = v_JsonExtractor.ConvertToUserVariable(sender);

            //Newtonsoft.Json.Linq.JToken searchResult = null;
            //Action setValueFunc = new Action(() =>
            //{
            //    if (searchResult == null)
            //    {
            //        throw new Exception("No Value found JSONPath '" + v_JsonExtractor + "', parsed '" + jsonSearchToken + "'");
            //    }

            //    var valueType = this.GetUISelectionValue(nameof(v_ValueType), "Value Type", engine);
            //    switch (valueType)
            //    {
            //        case "text":
            //            var newValueText = v_ValueToSet.ConvertToUserVariable(engine);
            //            searchResult.Replace(Newtonsoft.Json.Linq.JToken.FromObject(newValueText));
            //            break;
            //        case "number":
            //            var newValueNum = this.ConvertToUserVariableAsDecimal(nameof(v_ValueToSet), "Value", engine);
            //            searchResult.Replace(Newtonsoft.Json.Linq.JToken.FromObject(newValueNum));
            //            break;
            //        case "bool":
            //            var newValueBool = v_ValueToSet.ConvertToUserVariable(engine);
            //            switch (newValueBool.ToLower())
            //            {
            //                case "true":
            //                case "false":
            //                    searchResult.Replace(Newtonsoft.Json.Linq.JToken.FromObject(bool.Parse(newValueBool)));
            //                    break;
            //                default:
            //                    throw new Exception("Value To Set is NOT bool. Value '" + newValueBool + "'");
            //            }
            //            break;
            //        case "object":
            //            var newObjectText = v_ValueToSet.ConvertToUserVariable(engine);
            //            var o = Newtonsoft.Json.Linq.JObject.Parse(newObjectText);
            //            searchResult.Replace(Newtonsoft.Json.Linq.JToken.FromObject(o));
            //            break;
            //        case "array":
            //            var newArrayText = v_ValueToSet.ConvertToUserVariable(engine);
            //            var a = Newtonsoft.Json.Linq.JArray.Parse(newArrayText);
            //            searchResult.Replace(Newtonsoft.Json.Linq.JToken.FromObject(a));
            //            break;
            //    }
            //});

            //if (jsonText.StartsWith("{") && jsonText.EndsWith("}"))
            //{
            //    try
            //    {
            //        var o = Newtonsoft.Json.Linq.JObject.Parse(jsonText);
            //        searchResult = o.SelectToken(jsonSearchToken);
            //        setValueFunc();
            //        o.ToString().StoreInUserVariable(engine, v_InputValue);
            //    }
            //    catch (Exception ex)
            //    {
            //        throw new Exception("Fail Parse JSON Object: " + ex.ToString());
            //    }
            //}
            //else if(jsonText.StartsWith("[") && jsonText.EndsWith("]"))
            //{
            //    try
            //    {
            //        var a = Newtonsoft.Json.Linq.JArray.Parse(jsonText);
            //        searchResult = a.SelectToken(jsonSearchToken);
            //        setValueFunc();
            //        a.ToString().StoreInUserVariable(engine, v_InputValue);
            //    }
            //    catch(Exception ex)
            //    {
            //        throw new Exception("Fail Parse JSON Array: " + ex.ToString());
            //    }
            //}
            //else
            //{
            //    throw new Exception("Strange JSON. First 10 chars '" + jsonText.Substring(0, 10) + "'");
            //}

            Action<JToken> setValueFunc = new Action<JToken>((searchResult) =>
            {
                //var valueType = this.GetUISelectionValue(nameof(v_ValueType), "Value Type", engine);
                //var valueToSet = v_ValueToSet.ConvertToUserVariable(engine);
                //if (valueType == "auto")
                //{
                //    valueType = JSONControls.GetJSONType(valueToSet).ToLower();
                //}
                //switch (valueType)
                //{
                //    case "text":
                //        searchResult.Replace(JToken.FromObject(valueToSet));
                //        break;
                //    case "number":
                //        var newValueNum = this.ConvertToUserVariableAsDecimal(nameof(v_ValueToSet), "Value", engine);
                //        searchResult.Replace(JToken.FromObject(newValueNum));
                //        break;
                //    case "bool":
                //        switch (valueToSet.ToLower())
                //        {
                //            case "true":
                //            case "false":
                //                searchResult.Replace(JToken.FromObject(bool.Parse(valueToSet)));
                //                break;
                //            default:
                //                throw new Exception("Value To Set is NOT bool. Value '" + valueToSet + "'");
                //        }
                //        break;
                //    case "object":
                //        try
                //        {
                //            var o = JObject.Parse(valueToSet);
                //            searchResult.Replace(JToken.FromObject(o));
                //        }
                //        catch
                //        {
                //            throw new Exception("Value To Set is NOT Object. Value '" + valueToSet + "'");
                //        }
                //        break;
                //    case "array":
                //        try
                //        {
                //            var a = JArray.Parse(valueToSet);
                //            searchResult.Replace(JToken.FromObject(a));
                //        }
                //        catch
                //        {
                //            throw new Exception("Value To Set is NOT Array. Value '" + valueToSet + "'");
                //        }
                //        break;
                //}
                var valueToSet = this.GetJSONValue(nameof(v_ValueToSet), nameof(v_ValueType), "Set", engine);
                searchResult.Replace(JToken.FromObject(valueToSet));
            });
            this.JSONModifyByJSONPath(nameof(v_InputValue), nameof(v_JsonExtractor), setValueFunc, setValueFunc, engine);
        }

        public void lnkJsonPathHelper_Click(object sender, EventArgs e)
        {
            using (var fm = new UI.Forms.Supplement_Forms.frmJSONPathHelper())
            {
                if (fm.ShowDialog() == DialogResult.OK)
                {
                    //v_JsonExtractor = fm.JSONPath;
                    ((TextBox)((CommandItemControl)sender).Tag).Text = fm.JSONPath;
                }
            }
        }
    }
}