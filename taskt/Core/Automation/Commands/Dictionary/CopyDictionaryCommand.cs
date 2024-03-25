﻿using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dictionary Commands")]
    [Attributes.ClassAttributes.SubGruop("Dictionary Action")]
    [Attributes.ClassAttributes.CommandSettings("Copy Dictionary")]
    [Attributes.ClassAttributes.Description("This command allows you to copy a Dictionary.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to copy a Dictionary.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_dictionary))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class CopyDictionaryCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_InputDictionaryName))]
        [PropertyDescription("Dictionary Variable Name to Copy")]
        [PropertyValidationRule("Dictionary to Copy", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Dictionary to Copy")]
        public string v_TargetDictionary { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_NewOutputDictionaryName))]
        public string v_NewDictionary { get; set; }

        public CopyDictionaryCommand()
        {
            //this.CommandName = "CopyDictionaryCommand";
            //this.SelectionName = "Copy Dictionary";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var srcDic = v_TargetDictionary.ExpandUserVariableAsDictinary(engine);

            var newDic = new Dictionary<string, string>(srcDic);

            newDic.StoreInUserVariable(engine, v_NewDictionary);
        }
    }
}