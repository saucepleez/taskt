using System;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("JSON Commands")]
    [Attributes.ClassAttributes.SubGruop("Action")]
    [Attributes.ClassAttributes.Description("This command allows you to remove item to JSON Array.")]
    [Attributes.ClassAttributes.UsesDescription("")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class RemoveJSONArrayItemCommand : ScriptCommand
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
        [PropertyDescription("Index to Remove")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        //[SampleUsage("**0** or **1** or **{{{vIndex}}}**")]
        [PropertyDetailSampleUsage("**0**", "Specify the First Index to be Removed")]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Index to Remove")]
        [PropertyDetailSampleUsage("**{{{vIndex}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Index to Remove")]
        [Remarks("")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDisplayText(true, "Index")]
        [PropertyValidationRule("Index", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_RemoveIndex { get; set; }

        public RemoveJSONArrayItemCommand()
        {
            this.CommandName = "RemoveJSONArrayItem";
            this.SelectionName = "Remove JSON Array Item";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            Action<JToken> removeItemFunc = new Action<JToken>((searchResult) =>
            {
                if (!(searchResult is JArray))
                {
                    throw new Exception("Extraction Result is not JSON Array and can not Add Item. Value: '" + searchResult.ToString() + "'");
                }
                JArray ary = (JArray)searchResult;

                var index = this.ConvertToUserVariableAsInteger(nameof(v_RemoveIndex), "Index", engine);

                if ((index < 0) && (index > ary.Count))
                {
                    throw new Exception("Index is Out of Range. Value: " + index);
                }

                ary.RemoveAt(index);
            });
            this.JSONModifyByJSONPath(nameof(v_InputValue), nameof(v_JsonExtractor), removeItemFunc, removeItemFunc, engine);
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