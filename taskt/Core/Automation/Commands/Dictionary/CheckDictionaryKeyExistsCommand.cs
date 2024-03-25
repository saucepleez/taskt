﻿using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dictionary Commands")]
    [Attributes.ClassAttributes.SubGruop("Dictionary Key")]
    [Attributes.ClassAttributes.CommandSettings("Check Dictionary Key Exists")]
    [Attributes.ClassAttributes.Description("This command allows you to check key existance in Dictionary")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to check key existance in Dictionary.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_dictionary))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class CheckDictionaryKeyExistsCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_InputDictionaryName))]
        public string v_Dictionary { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_Key))]
        [PropertyDescription("Name of the Dictionary Key to Check")]
        public string v_Key { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(BooleanControls), nameof(BooleanControls.v_Result))]
        [Remarks("When the Key Exists, the Result is **True**")]
        public string v_Result { get; set; }

        public CheckDictionaryKeyExistsCommand()
        {
            //this.CommandName = "CheckDictionaryKeyExistsCommand";
            //this.SelectionName = "Check Dictionary Key Exists";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var vKey = v_Key.ExpandValueOrUserVariable(engine);

            var dic = v_Dictionary.ExpandUserVariableAsDictinary(engine);
            dic.ContainsKey(vKey).StoreInUserVariable(engine, v_Result);
        }
    }
}