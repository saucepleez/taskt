using System;
using System.Linq;
using System.Xml.Serialization;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dictionary Commands")]
    [Attributes.ClassAttributes.SubGruop("Dictionary Key")]
    [Attributes.ClassAttributes.CommandSettings("Get Dictionary Keys List")]
    [Attributes.ClassAttributes.Description("This command allows you to get Keys List in Dictionary")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get Keys List in Dictionary.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetDictionaryKeysListCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_InputDictionaryName))]
        public string v_InputData { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        public string v_OutputVariable { get; set; }

        public GetDictionaryKeysListCommand()
        {
            //this.CommandName = "GetDictionaryKeysListCommand";
            //this.SelectionName = "Get Dictionary Keys List";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var dic = v_InputData.GetDictionaryVariable(engine);
            dic.Keys.ToList().StoreInUserVariable(engine, v_OutputVariable);
        }
    }
}