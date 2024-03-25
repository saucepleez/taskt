﻿using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dictionary Commands")]
    [Attributes.ClassAttributes.SubGruop("Dictionary Item")]
    [Attributes.ClassAttributes.CommandSettings("Set Dictionary Value")]
    [Attributes.ClassAttributes.Description("This command allows you to set value in Dictionary")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set value in Dictionary.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_dictionary))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SetDictionaryValueCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_BothDictionaryName))]
        public string v_Dictionary { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_Key))]
        public string v_Key { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_Value))]
        public string v_Value { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_WhenKeyDoesNotExists))]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Add")]
        [PropertyDetailSampleUsage("**Ignore**", "Don't Set the Dictionary Item")]
        [PropertyDetailSampleUsage("**Add**", "Add New Dictionary Item")]
        public string v_WhenKeyDoesNotExists { get; set; }

        public SetDictionaryValueCommand()
        {
            //this.CommandName = "SetDictionaryValueCommand";
            //this.SelectionName = "Set Dictionary Value";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            (var dic, var vKey) = this.ExpandUserVariablesAsDictionaryAndKey(nameof(v_Dictionary), nameof(v_Key), engine);

            string valueToSet = v_Value.ExpandValueOrUserVariable(engine);
            if (dic.ContainsKey(vKey))
            {
                dic[vKey] = valueToSet;
            }
            else
            {
                string ifNotExits = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WhenKeyDoesNotExists), "Key Not Exists", engine);
                switch (ifNotExits)
                {
                    case "error":
                        throw new Exception("Key " + v_Key + " does not exists in the Dictionary");

                    case "ignore":
                        break;
                    case "add":
                        dic.Add(vKey, valueToSet);
                        break;
                }
            }
        }
    }
}