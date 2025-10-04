using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("JSON")]
    [Attributes.ClassAttributes.SubGruop("Get/Set")]
    [Attributes.ClassAttributes.CommandSettings("Get JSON Values As List")]
    [Attributes.ClassAttributes.Description("This command allows you to Get JSON Values From JSON and Result Values is List.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Get Values From JSON and Result Values is List")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class GetJSONValuesAsListCommand : AJSONGetFromJContainerCommands, IListResultProperties
    {
        //[XmlAttribute]
        //[PropertyDescription("Supply the JSON text or variable requiring extraction")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Select or provide a variable or text value")]
        //[SampleUsage("**{\"id\":2}** or **{{{vSomeVariable}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.JSON)]
        //[PropertyValidationRule("JSON", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "JSON")]
        //public string v_Json { get; set; }

        //[XmlAttribute]
        //[PropertyDescription("Specify a JSON extractor (JSONPath)")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Input a JSON token extractor")]
        //[SampleUsage("**$.id**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyCustomUIHelper("JSONPath Helper", nameof(lnkJsonPathHelper_Click))]
        //[PropertyValidationRule("JSON extractor", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Extractor")]
        //public string v_JsonExtractor { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please select the variable to receive the extracted Result")]
        //[InputSpecification("Select or provide a variable from the variable list")]
        //[SampleUsage("**vSomeVariable**")]
        //[Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyIsVariablesList(true)]
        //[PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        //[PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Result")]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        public override string v_Result { get; set; }

        public GetJSONValuesAsListCommand()
        {
            //this.CommandName = "GetJSONValueListCommand";
            //this.SelectionName = "Get JSON Value List";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var forbiddenMarkers = new List<string> { "[", "]" };

            //if (forbiddenMarkers.Any(f => f == engine.engineSettings.VariableStartMarker) || (forbiddenMarkers.Any(f => f == engine.engineSettings.VariableEndMarker)))
            //{
            //    throw new Exception("Cannot use Parse JSON command with square bracket variable markers [ ]");
            //}

            ////get variablized input
            //var jsonText = v_Json.ExpandValueOrUserVariable(engine).Trim();

            ////get variablized token
            //var jsonSearchToken = v_JsonExtractor.ExpandValueOrUserVariable(engine);

            //IEnumerable<Newtonsoft.Json.Linq.JToken> searchResults;
            //if (jsonText.StartsWith("{") && jsonText.EndsWith("}"))
            //{
            //    try
            //    {
            //        var o = Newtonsoft.Json.Linq.JObject.Parse(jsonText);
            //        searchResults = o.SelectTokens(jsonSearchToken);
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
            //        searchResults = a.SelectTokens(jsonSearchToken);
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

            ////add results to result list since list<string> is supported
            //List<string> resultList = new List<string>();
            //foreach (Newtonsoft.Json.Linq.JToken result in searchResults)
            //{
            //    resultList.Add(result.ToString());
            //}

            ////resultList.StoreInUserVariable(engine, v_applyToVariableName);
            //this.StoreListInUserVariable(resultList, nameof(v_Result), engine);

            (_, var root, _) = this.ExpandValueOrUserVariableAsJSON(engine);
            var jsonPath = v_JsonExtractor.ExpandValueOrUserVariable(engine);

            var elems = root.SelectTokens(jsonPath);

            var list = this.CreateEmptyList();
            foreach(var e in elems)
            {
                list.Add(e.ToString());
            }
            this.StoreListInUserVariable(list, engine);
        }

        //public void lnkJsonPathHelper_Click(object sender, EventArgs e)
        //{
        //    using (var fm = new UI.Forms.ScriptBuilder.CommandEditor.Supplemental.frmJSONPathHelper())
        //    {
        //        var item = (CommandItemControl)sender;
        //        if (fm.ShowDialog(item.FindForm()) == DialogResult.OK)
        //        {
        //            //v_JsonExtractor = fm.JSONPath;
        //            ((TextBox)(item.Tag)).Text = fm.JSONPath;
        //        }
        //    }
        //}
    }
}