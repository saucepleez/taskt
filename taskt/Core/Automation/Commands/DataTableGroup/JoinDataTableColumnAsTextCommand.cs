using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable")]
    [Attributes.ClassAttributes.SubGruop("Column Action")]
    [Attributes.ClassAttributes.CommandSettings("Join DataTable Column As Text")]
    [Attributes.ClassAttributes.Description("This command allows you to Join DataTable Column Values as Text")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Join DataTable Column Values as Text.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class JoinDataTableColumnAsTextCommand : ADataTableGetFromDataTableColumnCommands
    {
        //public string v_DataTable { get; set; }

        //public string v_ColumnType { get; set; }

        //public string v_ColumnIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Separator of List Values")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDetailSampleUsage(",", PropertyDetailSampleUsage.ValueType.Value, "Separator")]
        [PropertyDetailSampleUsage("{{{vSep}}}", PropertyDetailSampleUsage.ValueType.VariableValue, "Separator")]
        [PropertyValidationRule("Separator", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Separator")]
        [PropertyParameterOrder(8000)]
        public string v_Separator { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyParameterOrder(9000)]
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
        [PropertyParameterOrder(9000)]
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
        [PropertyParameterOrder(9001)]
        public string v_AfterText { get; set; }

        public JoinDataTableColumnAsTextCommand()
        {      
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            using (var myList = new InnerScriptVariable(engine))
            {
                var convList = new ConvertDataTableColumnToListCommand()
                {
                    v_DataTable = this.v_DataTable,
                    v_ColumnType = this.v_ColumnType,
                    v_ColumnIndex = this.v_ColumnIndex,
                    v_Result = myList.VariableName,
                };
                convList.RunCommand(engine);

                var joinText = new JoinListValuesAsTextCommand()
                {
                    v_List = myList.VariableName,
                    v_Separator = this.v_Separator,
                    v_Result = this.v_Result,
                    v_BeforeText = this.v_BeforeText,
                    v_AfterText = this.v_AfterText,
                };
                joinText.RunCommand(engine);
            }
        }
    }
}