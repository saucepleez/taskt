using System;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("JSON Commands")]
    [Attributes.ClassAttributes.SubGruop("Get/Set")]
    [Attributes.ClassAttributes.Description("This command allows you to set value in JSON.")]
    [Attributes.ClassAttributes.UsesDescription("")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SetJSONValueCommand : ScriptCommand
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
        [PropertyDetailSampleUsage("**$.names[0]**", "Specify the First item in the Array of **names** Property")]
        public string v_JsonExtractor { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please Specify Value to Set")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("")]
        //[SampleUsage("**Hello** or **{{{vNewValue}}}**")]
        //[Remarks("")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.MultiLineTextBox)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyDisplayText(true, "Value")]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_ValueToAdd))]
        [PropertyDescription("Value to Set")]
        public string v_ValueToSet { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please Specify Value Type to Set")]
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
        [PropertyDescription("Value Type to Set")]
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

            Action<JToken> setValueFunc = new Action<JToken>((searchResult) =>
            {
                var valueToSet = this.GetJSONValue(nameof(v_ValueToSet), nameof(v_ValueType), "Set", engine);
                searchResult.Replace(JToken.FromObject(valueToSet));
            });
            this.JSONModifyByJSONPath(nameof(v_InputValue), nameof(v_JsonExtractor), setValueFunc, setValueFunc, engine);
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