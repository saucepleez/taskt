using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using System.Linq;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dictionary Commands")]
    [Attributes.ClassAttributes.SubGruop("Convert")]
    [Attributes.ClassAttributes.CommandSettings("Convert Dictionary To List")]
    [Attributes.ClassAttributes.Description("This command allows you to get List from Dictionary")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get List from Dictionary.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ConvertDictionaryToListCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_InputDictionaryName))]
        public string v_InputData { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        public string v_OutputVariable { get; set; }

        public ConvertDictionaryToListCommand()
        {
            //this.CommandName = "ConvertDictionaryToListCommand";
            //this.SelectionName = "Convert Dictionary To List";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var dic = v_InputData.GetDictionaryVariable(engine);

            dic.Values.ToList().StoreInUserVariable(engine, v_OutputVariable);
        }
    }
}