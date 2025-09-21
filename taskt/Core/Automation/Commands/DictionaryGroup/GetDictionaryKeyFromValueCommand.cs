using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dictionary")]
    [Attributes.ClassAttributes.SubGruop("Dictionary Key")]
    [Attributes.ClassAttributes.CommandSettings("Get Dictionary Key From Value")]
    [Attributes.ClassAttributes.Description("This command allows you to get Dictionary key Name from Value")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get Dictionary key Name from Value.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_dictionary))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class GetDictionaryKeyFromValueCommand : ADictionaryGetFromDictionaryCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_InputDictionaryName))]
        //public string v_Dictionary { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_Value))]
        [PropertyParameterOrder(6000)]
        public string v_Value { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Key name")]
        [Remarks("When value not found, Result is Empty")]
        public override string v_Result { get; set; }

        public GetDictionaryKeyFromValueCommand()
        {
            //this.CommandName = "GetDictionaryKeyFromValueCommand";
            //this.SelectionName = "Get Dictionary Key From Value";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var dic = v_Dictionary.ExpandUserVariableAsDictinary(engine);
            var dic = this.ExpandUserVariableAsDictionary(engine);

            var vValue = v_Value.ExpandValueOrUserVariable(engine);
            foreach (KeyValuePair<string, string> item in dic)
            {
                if (item.Value == vValue)
                {
                    item.Key.StoreInUserVariable(engine, v_Result);
                    return;
                }
            }
            "".StoreInUserVariable(engine, v_Result);
        }
    }
}