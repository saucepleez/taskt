using System;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("JSON Commands")]
    [Attributes.ClassAttributes.SubGruop("Action")]
    [Attributes.ClassAttributes.Description("This command allows you to add item to JSON Array.")]
    [Attributes.ClassAttributes.UsesDescription("")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class AddJSONArrayItemCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please Specify the JSON Variable Name")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("")]
        //[SampleUsage("**{{{vSomeVariable}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.JSON)]
        //[PropertyValidationRule("JSON", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "JSON")]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_BothJSONName))]
        [PropertyDescription("JSON Array Variable Name")]
        public string v_InputValue { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please Specify a JSON extractor (JSONPath)")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Input a JSON token extractor")]
        //[SampleUsage("**$.id**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyCustomUIHelper("JSONPath Helper", nameof(lnkJsonPathHelper_Click))]
        //[PropertyValidationRule("JSON extractor", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Extractor")]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_JSONPath))]
        public string v_JsonExtractor { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Value to Add")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("")]
        //[PropertyDetailSampleUsage("**Hello**", "Add Text **Hello**")]
        //[PropertyDetailSampleUsage("**1**", "Add Number **Hello**")]
        //[PropertyDetailSampleUsage("**{{{vValue}}}**", "Add Value of Variable **vValue**")]
        //[PropertyDetailSampleUsage("**{ \"id\": 1, \"value\": \"Hello\" }**", "Add JSON Object", false)]
        //[PropertyDetailSampleUsage("**[ 1, 2, \"Hello\" ]**", "Add JSON Array", false)]
        //[Remarks("")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.MultiLineTextBox)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyDisplayText(true, "Value")]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_ValueToAdd))]
        public string v_ArrayItem { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please Specify Value Type to Add")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("")]
        //[SampleUsage("**Text** or **Number** or **bool** or **Object** or **Array**")]
        //[Remarks("")]
        //[PropertyUISelectionOption("Auto")]
        //[PropertyUISelectionOption("Text")]
        //[PropertyUISelectionOption("Number")]
        //[PropertyUISelectionOption("null")]
        //[PropertyUISelectionOption("bool")]
        //[PropertyUISelectionOption("Object")]
        //[PropertyUISelectionOption("Array")]
        //[PropertyIsOptional(true, "Auto")]
        //[PropertyDisplayText(true, "Value Type")]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_ValueType))]
        public string v_ValueType { get; set; }

        public AddJSONArrayItemCommand()
        {
            this.CommandName = "AddJSONArrayItem";
            this.SelectionName = "Add JSON Array Item";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            Action<JToken> addItemFunc = new Action<JToken>((searchResult) =>
            {
                if (!(searchResult is JArray))
                {
                    throw new Exception("Extraction Result is not JSON Array and can not Add Item. Value: '" + searchResult.ToString() + "'");
                }
                JArray ary = (JArray)searchResult;

                var addItem = this.GetJSONValue(nameof(v_ArrayItem), nameof(v_ValueType), "Add", engine);
                ary.Add(addItem);
            });
            this.JSONModifyByJSONPath(nameof(v_InputValue), nameof(v_JsonExtractor), addItemFunc, addItemFunc, engine);
        }

        //public void lnkJsonPathHelper_Click(object sender, EventArgs e)
        //{
        //    using (var fm = new UI.Forms.Supplement_Forms.frmJSONPathHelper())
        //    {
        //        if (fm.ShowDialog() == DialogResult.OK)
        //        {
        //            //v_JsonExtractor = fm.JSONPath;
        //            ((TextBox)((CommandItemControl)sender).Tag).Text = fm.JSONPath;
        //        }
        //    }
        //}
    }
}