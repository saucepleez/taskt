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
    [Attributes.ClassAttributes.Description("This command allows you to add property to JSON Object.")]
    [Attributes.ClassAttributes.UsesDescription("")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class InsertJSONObjectPropertyCommand : ScriptCommand
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
        [PropertyDescription("Please Specify Property Name to Insert")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**Name** or **{{{vName}}}**")]
        [Remarks("")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Name", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Name")]
        public string v_PropertyName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Specify Property Value to Insert")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**Hello** or **{{{vValue}}}**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.MultiLineTextBox)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDisplayText(true, "Value")]
        public string v_PropertyValue { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Specify Value Type to Insert")]
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

        public InsertJSONObjectPropertyCommand()
        {
            this.CommandName = "InsertJSONObjectProperty";
            this.SelectionName = "Insert JSON Object Property";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            Action<JToken> addPropertyFunc = new Action<JToken>((searchResult) =>
            {
                if (!(searchResult.Parent is JProperty))
                {
                    throw new Exception("Extraction Result is not JSON Object and can not Add JSON Property. Value: '" + searchResult.ToString() + "'");
                }
                
                var propertyValue = this.GetJSONValue(nameof(v_PropertyValue), nameof(v_ValueType), "Insert", engine);
                var propertyName = v_PropertyName.ConvertToUserVariable(engine);
                searchResult.Parent.AddAfterSelf(new JProperty(propertyName, propertyValue));
            });
            this.JSONModifyByJSONPath(nameof(v_InputValue), nameof(v_JsonExtractor), addPropertyFunc, addPropertyFunc, engine);
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