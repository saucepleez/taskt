using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dictionary Commands")]
    [Attributes.ClassAttributes.SubGruop("Convert")]
    [Attributes.ClassAttributes.CommandSettings("Convert Dictionary To DataTable")]
    [Attributes.ClassAttributes.Description("This command allows you to get JSON from Dictionary")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get JSON from Dictionary.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ConvertDictionaryToJSONCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_InputDictionaryName))]
        public string v_InputData { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_OutputJSONName))]
        public string v_OutputVariable { get; set; }

        public ConvertDictionaryToJSONCommand()
        {
            //this.CommandName = "ConvertDictionaryToJSONCommand";
            //this.SelectionName = "Convert Dictionary To JSON";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var dic = v_InputData.GetDictionaryVariable(engine);

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(dic);
            json.StoreInUserVariable(engine, v_OutputVariable);
        }
    }
}