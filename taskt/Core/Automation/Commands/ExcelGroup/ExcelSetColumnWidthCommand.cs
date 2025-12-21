using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel")]
    [Attributes.ClassAttributes.SubGruop("Column")]
    [Attributes.ClassAttributes.CommandSettings("Set Column Width")]
    [Attributes.ClassAttributes.Description("This command set Column Width")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set Column Width")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ExcelSetColumnWidthCommand : AExcelColumnSpecifiedCommands
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Column Width")]
        [InputSpecification("Number")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDetailSampleUsage("**10**", PropertyDetailSampleUsage.ValueType.Value)]
        [PropertyDetailSampleUsage("**{{{vWdith}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyValidationRule("Width", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(true, "Width")]
        [PropertyParameterOrder(8500)]
        public string v_Width { get; set; }

        public ExcelSetColumnWidthCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            (var sheet, var column) = this.ExpandValueOrVariableAsExcelCurrentWorksheetAndColumnIndex(engine);

            var width = this.ExpandValueOrUserVariableAsInteger(nameof(v_Width), engine);

            sheet.Columns[column].ColumnWidth = width;
        }
    }
}