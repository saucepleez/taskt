using System;
using System.Xml.Serialization;
using System.Windows.Forms;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using Newtonsoft.Json.Linq;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("JSON Commands")]
    [Attributes.ClassAttributes.SubGruop("Action")]
    [Attributes.ClassAttributes.Description("This command allows you to remove a property in JSON")]
    [Attributes.ClassAttributes.UsesDescription("")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class RemoveJSONPropertyCommand : ScriptCommand
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

        public RemoveJSONPropertyCommand()
        {
            this.CommandName = "Remove JSON Property";
            this.SelectionName = "Remove JSON Property";
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
            //Action removeFunc = new Action(() =>
            //{
            //    if (searchResult == null)
            //    {
            //        throw new Exception("No Property found JSONPath '" + v_JsonExtractor + "', parsed '" + jsonSearchToken + "'");
            //    }

            //    var p = searchResult.Parent;
            //    if (p is Newtonsoft.Json.Linq.JProperty prop)
            //    {
            //        prop.Remove();
            //    }
            //    else if (p is Newtonsoft.Json.Linq.JArray ary)
            //    {
            //        ary.Remove(searchResult);
            //    }
            //    else
            //    {
            //        throw new Exception("Strange Search Result. Fail Remove. Value: '" + searchResult.ToString() + "'");
            //    }
            //});

            //if (jsonText.StartsWith("{") && jsonText.EndsWith("}"))
            //{
            //    try
            //    {
            //        var o = Newtonsoft.Json.Linq.JObject.Parse(jsonText);
            //        searchResult = o.SelectToken(jsonSearchToken);
            //        removeFunc();
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
            //        removeFunc();
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

            Action<JToken> removeFunc = new Action<JToken>((searchResult) =>
            {
                var p = searchResult.Parent;
                if (p is JProperty prop)
                {
                    prop.Remove();
                }
                else if (p is JArray ary)
                {
                    ary.Remove(searchResult);
                }
                else
                {
                    throw new Exception("Strange Search Result. Fail Remove. Value: '" + searchResult.ToString() + "'");
                }
            });
            this.JSONModifyByJSONPath(nameof(v_InputValue), nameof(v_JsonExtractor), removeFunc, removeFunc, engine);
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