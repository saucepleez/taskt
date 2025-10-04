using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable")]
    [Attributes.ClassAttributes.SubGruop("File")]
    [Attributes.ClassAttributes.CommandSettings("Export DataTable Column As Text File")]
    [Attributes.ClassAttributes.Description("This command allows you to Export DataTable Column as Text File")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Export DataTable Column as Text File.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ExportDataTableColumnAsTextFileCommand : ADataTableColumnCommands
    {
        //public string v_DataTable { get; set; }

        //public string v_ColumnType { get; set; }

        //public string v_ColumnIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePath))]
        [PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.RequiredExtension, PropertyFilePathSetting.FileCounterBehavior.NoSupport, "txt")]
        [PropertyParameterOrder(8000)]
        public string v_FilePath { get; set; }

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

        public ExportDataTableColumnAsTextFileCommand()
        {  
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
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

                var exportText = new ExportDataTableAsTextFileCommand()
                {
                    v_DataTable = myDT.VariableName,
                    v_ExportHeader = this.v_ExportHeader,
                    v_ExportIndex = this.v_ExportIndex,
                    v_FilePath = this.v_FilePath,
                };
                exportText.RunCommand(engine);
            }
        }
    }
}