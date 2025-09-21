using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable")]
    [Attributes.ClassAttributes.SubGruop("Row Action")]
    [Attributes.ClassAttributes.CommandSettings("Join DataTable Row As Text")]
    [Attributes.ClassAttributes.Description("This command allows you to join DataTable Row Values as Text")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to join DataTable Row Values as Text.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class JoinDataTableRowAsTextCommand : ADataTableGetFromDataTableRowCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_InputDataTableName))]
        //public string v_DataTable { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_RowIndex))]
        //public string v_RowIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Separator of List Values")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDetailSampleUsage(",", PropertyDetailSampleUsage.ValueType.Value, "Separator")]
        [PropertyDetailSampleUsage("{{{vSep}}}", PropertyDetailSampleUsage.ValueType.VariableValue, "Separator")]
        [PropertyValidationRule("Separator", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Separator")]
        [PropertyParameterOrder(7000)]
        public string v_Separator { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyParameterOrder(8000)]
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

        public JoinDataTableRowAsTextCommand()
        {      
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            using (var myList = new InnerScriptVariable(engine))
            {
                var convList = new ConvertDataTableRowToListCommand()
                {
                    v_DataTable = this.v_DataTable,
                    v_RowIndex = this.v_RowIndex,
                    v_Result = myList.VariableName,
                };
                convList.RunCommand(engine);

                var joinList = new JoinListValuesAsTextCommand()
                {
                    v_List = myList.VariableName,
                    v_Separator = this.v_Separator,
                    v_Result = this.v_Result,
                    v_BeforeText = this.v_BeforeText,
                    v_AfterText = this.v_AfterText,
                };
                joinList.RunCommand(engine);
            }
        }
    }
}