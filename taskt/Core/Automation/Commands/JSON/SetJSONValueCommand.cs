using System;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("JSON Commands")]
    [Attributes.ClassAttributes.SubGruop("Get/Set")]
    [Attributes.ClassAttributes.CommandSettings("Set JSON Value")]
    [Attributes.ClassAttributes.Description("This command allows you to set value in JSON.")]
    [Attributes.ClassAttributes.UsesDescription("")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SetJSONValueCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_BothJSONName))]
        public string v_InputValue { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_JSONPath))]
        [PropertyDetailSampleUsage("**$.names[0]**", "Specify the First item in the Array of **names** Property")]
        public string v_JsonExtractor { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_ValueToAdd))]
        [PropertyDescription("Value to Set")]
        public string v_ValueToSet { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_ValueType))]
        [PropertyDescription("Value Type to Set")]
        public string v_ValueType { get; set; }

        public SetJSONValueCommand()
        {
            //this.CommandName = "SetJSONValue";
            //this.SelectionName = "Set JSON Value";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
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
    }
}