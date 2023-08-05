using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("JSON Commands")]
    [Attributes.ClassAttributes.SubGruop("Action")]
    [Attributes.ClassAttributes.CommandSettings("Create JSON Variable")]
    [Attributes.ClassAttributes.Description("This command allows you to create JSON Variable.")]
    [Attributes.ClassAttributes.UsesDescription("")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class CreateJSONVariableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_OutputJSONName))]
        public string v_JsonVariable { get; set; }

        [XmlAttribute]
        [PropertyDescription("JSON Value")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [PropertyDetailSampleUsage("**{ \"id\": 1, \"name\": \"John\" }**", "Specify JSON Object")]
        [PropertyDetailSampleUsage("**[ 1, 2, \"Hello\" ]**", "Specify JSON Array")]
        [PropertyDetailSampleUsage("**{{{vJSONValue}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "JSON Value")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.MultiLineTextBox)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDisplayText(true, "JSON Value")]
        public string v_JsonValue { get; set; }

        public CreateJSONVariableCommand()
        {
            //this.CommandName = "CreateJSONVariable";
            //this.SelectionName = "Create JSON Variable";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            (var jsonText, _) = v_JsonValue.ConvertToUserVariableAsJSON(engine);
            jsonText.StoreInUserVariable(engine, v_JsonVariable);
        }
    }
}