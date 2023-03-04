using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dictionary Commands")]
    [Attributes.ClassAttributes.SubGruop("Dictionary Key")]
    [Attributes.ClassAttributes.CommandSettings("Get Dictionary Key From Value")]
    [Attributes.ClassAttributes.Description("This command allows you to get Dictionary key Name from Value")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get Dictionary key Name from Value.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetDictionaryKeyFromValueCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_InputDictionaryName))]
        public string v_InputData { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_Value))]
        public string v_Value { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Key name")]
        [Remarks("When value not found, Result is Empty")]
        public string v_OutputVariable { get; set; }

        public GetDictionaryKeyFromValueCommand()
        {
            //this.CommandName = "GetDictionaryKeyFromValueCommand";
            //this.SelectionName = "Get Dictionary Key From Value";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;
            var vValue = v_Value.ConvertToUserVariable(sender);

            var dic = v_InputData.GetDictionaryVariable(engine);
            
            foreach(KeyValuePair<string, string> item in dic)
            {
                if (item.Value == vValue)
                {
                    item.Key.StoreInUserVariable(engine, v_OutputVariable);
                    return;
                }
            }
            "".StoreInUserVariable(engine, v_OutputVariable);
        }
    }
}