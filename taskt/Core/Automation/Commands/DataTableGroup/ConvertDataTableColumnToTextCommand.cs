using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable")]
    [Attributes.ClassAttributes.SubGruop("Convert Column")]
    [Attributes.ClassAttributes.CommandSettings("Convert DataTable Column To Text")]
    [Attributes.ClassAttributes.Description("This command allows you to convert DataTable Column to Text")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to convert DataTable Column to Text.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ConvertDataTableColumnToTextCommand : ADataTableGetFromDataTableColumnCommands
    {
        //public string v_DataTable { get; set; }

        //public string v_ColumnType { get; set; }

        //public string v_ColumnIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyParameterOrder(8000)]
        public override string v_Result { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Export Header")]
        [PropertyIsOptional(true, "No")]
        [PropertyFirstValue("No")]
        [PropertyValidationRule("Export Header", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        [PropertyParameterOrder(9000)]
        public string v_ExportHeader { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Export Row Index")]
        [PropertyIsOptional(true, "No")]
        [PropertyFirstValue("No")]
        [PropertyValidationRule("Export Row Index", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        [PropertyParameterOrder(9001)]
        public string v_ExportIndex { get; set; }

        public ConvertDataTableColumnToTextCommand()
        {  
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //(var srcDT, var colIndex, _) = this.ExpandValueOrUserVariableAsDataTableAndColumn(engine);

            using(var myDT = new InnerScriptVariable(engine))
            {
                var createTable = new ConvertDataTableColumnToDataTableCommand()
                {
                    v_DataTable = this.v_DataTable,
                    v_ColumnType = this.v_ColumnType,
                    v_ColumnIndex = this.v_ColumnIndex,
                    v_Result = myDT.VariableName,
                };
                createTable.RunCommand(engine);

                var convText = new ConvertDataTableToTextCommand()
                {
                    v_DataTable = myDT.VariableName,
                    v_ExportHeader = this.v_ExportHeader,
                    v_ExportIndex = this.v_ExportIndex,
                    v_Result = this.v_Result,
                };
                convText.RunCommand(engine);
            }
        }
    }
}