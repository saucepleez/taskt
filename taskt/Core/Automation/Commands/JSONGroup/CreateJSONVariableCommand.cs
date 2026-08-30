using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("JSON")]
    [Attributes.ClassAttributes.SubGruop("Action")]
    [Attributes.ClassAttributes.CommandSettings("Create JSON Variable")]
    [Attributes.ClassAttributes.Description("This command allows you to create JSON Variable.")]
    [Attributes.ClassAttributes.UsesDescription("")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class CreateJSONVariableCommand : AJSONValueActionCommands
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_OutputJSONName))]
        public override string v_Json { get; set; }

        [XmlAttribute]
        //[PropertyDescription("JSON Value")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("")]
        //[PropertyDetailSampleUsage("**{ \"id\": 1, \"name\": \"John\" }**", "Specify JSON Object")]
        //[PropertyDetailSampleUsage("**[ 1, 2, \"Hello\" ]**", "Specify JSON Array")]
        //[PropertyDetailSampleUsage("**{{{vJSONValue}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "JSON Value")]
        //[Remarks("")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.MultiLineTextBox)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyDisplayText(true, "JSON Value")]
        [PropertyDescription("JSON Value")]
        [PropertyDisplayText(true, "JSON Value")]
        public override string v_Value { get; set; }

        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_ValueType))]
        //[PropertyParameterOrder(7000)]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_ValueTypeSimple))]
        public override string v_ValueType { get; set; }

        public CreateJSONVariableCommand()
        {
            //this.CommandName = "CreateJSONVariable";
            //this.SelectionName = "Create JSON Variable";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //(var jsonText, _) = v_Value.ExpandValueOrUserVariableAsJSON(engine);
            //jsonText.StoreInUserVariable(engine, v_Json);
            (var jsonText, _) = this.ExpandValueOrVariableValueAsJSONInJSONValue(engine);
            jsonText.StoreInUserVariable(engine, v_Json);
        }
    }
}