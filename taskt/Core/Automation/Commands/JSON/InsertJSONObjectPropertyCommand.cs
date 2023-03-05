using System;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("JSON Commands")]
    [Attributes.ClassAttributes.SubGruop("Action")]
    [Attributes.ClassAttributes.CommandSettings("Insert JSON Object Property")]
    [Attributes.ClassAttributes.Description("This command allows you to add property to JSON Object.")]
    [Attributes.ClassAttributes.UsesDescription("")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class InsertJSONObjectPropertyCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_BothJSONName))]
        [PropertyDescription("JSON Object Variable Name")]
        public string v_InputValue { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_JSONPath))]
        public string v_JsonExtractor { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Property Name to Insert")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("")]
        //[PropertyDetailSampleUsage("**Name**", PropertyDetailSampleUsage.ValueType.Value, "Property Name")]
        //[PropertyDetailSampleUsage("**{{{vName}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Property Name")]
        //[Remarks("")]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyValidationRule("Property Name", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Property Name")]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_PropertyName))]
        public string v_PropertyName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_ValueToAdd))]
        public string v_PropertyValue { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_ValueType))]
        public string v_ValueType { get; set; }

        public InsertJSONObjectPropertyCommand()
        {
            //this.CommandName = "InsertJSONObjectProperty";
            //this.SelectionName = "Insert JSON Object Property";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
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
    }
}