using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Windows.Forms;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("JSON Commands")]
    [Attributes.ClassAttributes.SubGruop("Get/Set")]
    [Attributes.ClassAttributes.Description("This command allows you to parse a JSON object into a list.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to extract data from a JSON object")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetJSONValueListCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Supply the JSON text or variable requiring extraction")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Select or provide a variable or text value")]
        [SampleUsage("**{\"id\":2}** or **{{{vSomeVariable}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.JSON)]
        [PropertyValidationRule("JSON", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "JSON")]
        public string v_InputValue { get; set; }

        [XmlAttribute]
        [PropertyDescription("Specify a JSON extractor (JSONPath)")]
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
        [PropertyDescription("Please select the variable to receive the extracted Result")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Result")]
        public string v_applyToVariableName { get; set; }

        public GetJSONValueListCommand()
        {
            this.CommandName = "GetJSONValueListCommand";
            this.SelectionName = "Get JSON Value List";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var forbiddenMarkers = new List<string> { "[", "]" };

            if (forbiddenMarkers.Any(f => f == engine.engineSettings.VariableStartMarker) || (forbiddenMarkers.Any(f => f == engine.engineSettings.VariableEndMarker)))
            {
                throw new Exception("Cannot use Parse JSON command with square bracket variable markers [ ]");
            }

            //get variablized input
            var jsonText = v_InputValue.ConvertToUserVariable(sender).Trim();

            //get variablized token
            var jsonSearchToken = v_JsonExtractor.ConvertToUserVariable(sender);

            ////create objects
            //Newtonsoft.Json.Linq.JObject o;
            //IEnumerable<Newtonsoft.Json.Linq.JToken> searchResults;
            //List<string> resultList = new List<string>();

            ////parse json
            //try
            //{
            //     o = Newtonsoft.Json.Linq.JObject.Parse(jsonText);
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("Error Occured Parsing Tokens: " + ex.ToString());
            //}

            IEnumerable<Newtonsoft.Json.Linq.JToken> searchResults;
            if (jsonText.StartsWith("{") && jsonText.EndsWith("}"))
            {
                try
                {
                    var o = Newtonsoft.Json.Linq.JObject.Parse(jsonText);
                    searchResults = o.SelectTokens(jsonSearchToken);
                }
                catch (Exception ex)
                {
                    throw new Exception("Fail Parse JSON Object: " + ex.ToString());
                }
            }
            else if(jsonText.StartsWith("[") && jsonText.EndsWith("]"))
            {
                try
                {
                    var a = Newtonsoft.Json.Linq.JArray.Parse(jsonText);
                    searchResults = a.SelectTokens(jsonSearchToken);
                }
                catch(Exception ex)
                {
                    throw new Exception("Fail Parse JSON Array: " + ex.ToString());
                }
            }
            else
            {
                throw new Exception("Strange JSON. First 10 chars '" + jsonText.Substring(0, 10) + "'");
            }

            ////select results
            //try
            //{
            //    searchResults = o.SelectTokens(jsonSearchToken);
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("Error Occured Selecting Tokens: " + ex.ToString());
            //}

            //List<Newtonsoft.Json.Linq.JToken> sr = searchResults.ToList();

            //add results to result list since list<string> is supported
            List<string> resultList = new List<string>();
            foreach (Newtonsoft.Json.Linq.JToken result in searchResults)
            {
                resultList.Add(result.ToString());
            }

            ////get variable
            //var requiredComplexVariable = engine.VariableList.Where(x => x.VariableName == v_applyToVariableName).FirstOrDefault();

            ////create if var does not exist
            //if (requiredComplexVariable == null)
            //{
            //    engine.VariableList.Add(new Script.ScriptVariable() { VariableName = v_applyToVariableName, CurrentPosition = 0 });
            //    requiredComplexVariable = engine.VariableList.Where(x => x.VariableName == v_applyToVariableName).FirstOrDefault();
            //}

            ////assign value to variable
            //requiredComplexVariable.VariableValue = resultList;

            resultList.StoreInUserVariable(engine, v_applyToVariableName);
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