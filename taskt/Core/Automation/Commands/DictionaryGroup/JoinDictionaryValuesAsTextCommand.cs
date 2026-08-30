using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dictionary")]
    [Attributes.ClassAttributes.SubGruop("Dictionary Item")]
    [Attributes.ClassAttributes.CommandSettings("Join Dictionary Values As Text")]
    [Attributes.ClassAttributes.Description("This command allows you to join Dictionary Values")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to join Dictionary Values")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_dictionary))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class JoinDictionaryValuesAsTextCommand : ADictionaryGetFromDictionaryCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_InputDictionaryName))]
        //public string v_Dictionary { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Separator of List Values")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDetailSampleUsage(",", PropertyDetailSampleUsage.ValueType.Value, "Separator")]
        [PropertyDetailSampleUsage("{{{vSep}}}", PropertyDetailSampleUsage.ValueType.VariableValue, "Separator")]
        [PropertyValidationRule("Separator", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Separator")]
        [PropertyParameterOrder(6000)]
        public string v_Separator { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyParameterOrder(7000)]
        public override string v_Result { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Text before Values")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true)]
        [PropertyDetailSampleUsage("\"", PropertyDetailSampleUsage.ValueType.Value, "Text before Values")]
        [PropertyDetailSampleUsage("{{{vText}}}", PropertyDetailSampleUsage.ValueType.VariableValue, "Text before Values")]
        [PropertyValidationRule("Text before Values", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        [PropertyParameterOrder(8000)]
        public string v_BeforeText { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Text after Values")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true)]
        [PropertyDetailSampleUsage("\"", PropertyDetailSampleUsage.ValueType.Value, "Text after Values")]
        [PropertyDetailSampleUsage("{{{vText}}}", PropertyDetailSampleUsage.ValueType.VariableValue, "Text after Values")]
        [PropertyValidationRule("Text after Values", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        [PropertyParameterOrder(8001)]
        public string v_AfterText { get; set; }

        public JoinDictionaryValuesAsTextCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            using (var myList = new InnerScriptVariable(engine))
            {
                var convList = new ConvertDictionaryToListCommand()
                {
                    v_Dictionary = this.v_Dictionary,
                    v_Result = myList.VariableName,
                };
                convList.RunCommand(engine);

                var joinList = new JoinListValuesAsTextCommand()
                {
                    v_List = myList.VariableName,
                    v_Result = this.v_Result,
                    v_Separator = this.v_Separator,
                    v_BeforeText = this.v_BeforeText,
                    v_AfterText = this.v_AfterText,
                };
                joinList.RunCommand(engine);
            }
        }
    }
}
