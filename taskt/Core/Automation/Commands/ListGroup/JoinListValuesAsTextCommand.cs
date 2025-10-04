using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List")]
    [Attributes.ClassAttributes.SubGruop("Convert")]
    [Attributes.ClassAttributes.CommandSettings("Join List Values As Text")]
    [Attributes.ClassAttributes.Description("This command allows you to Join List Values. This Result is Text value.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Join List Values. This Result is Text value.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class JoinListValuesAsTextCommand : AListGetFromListCommands
    {
        //[XmlAttribute]
        //public string v_List { get; set; }

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

        public JoinListValuesAsTextCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var list = this.ExpandUserVariableAsList(engine);

            var sep = this.ExpandValueOrUserVariable(nameof(v_Separator), "Separator", engine);
            var bef = this.ExpandValueOrUserVariable(nameof(v_BeforeText), "Text before", engine);
            var aft = this.ExpandValueOrUserVariable(nameof(v_AfterText), "Text after", engine);
            
            string txt = "";
            var last = list.Count - 1;
            for (int i = 0; i < last; i++)
            {
                txt += $"{bef}{list[i]}{aft}{sep}";
            }
            txt += $"{bef}{list[last]}{aft}";
            txt.StoreInUserVariable(engine, v_Result);
        }
    }
}
