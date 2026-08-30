using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable")]
    [Attributes.ClassAttributes.SubGruop("File")]
    [Attributes.ClassAttributes.CommandSettings("Export DataTable Row As Text File")]
    [Attributes.ClassAttributes.Description("This command allows you to Export DataTable Row as Text File.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Export DataTable Row as Text File.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ExportDataTableRowAsTextFileCommand : ADataTableRowCommands
    {
        //public string v_DataTable { get; set; }

        //public string v_RowIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePath))]
        [PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.RequiredExtension, PropertyFilePathSetting.FileCounterBehavior.NoSupport, "txt")]
        [PropertyParameterOrder(7000)]
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

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("When Exporting Row Index, Export the Actual Row Index")]
        [PropertyIsOptional(true, "Yes")]
        [PropertyFirstValue("Yes")]
        [PropertyValidationRule("Actual Row Index", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        [PropertyParameterOrder(10000)]
        public string v_ActualRowIndex { get; set; }

        public ExportDataTableRowAsTextFileCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            using (var myTxt = new InnerScriptVariable(engine))
            {
                var rowText = new ConvertDataTableRowToTextCommand()
                {
                    v_DataTable = this.v_DataTable,
                    v_RowIndex = this.v_RowIndex,
                    v_ExportHeader = this.v_ExportHeader,
                    v_ExportIndex = this.v_ExportIndex,
                    v_ActualRowIndex = this.v_ActualRowIndex,
                    v_Result = myTxt.VariableName,
                };
                rowText.RunCommand(engine);

                var writeText = new WriteTextFileCommand()
                {
                    v_TextToWrite = VariableNameControls.GetWrappedVariableName(myTxt.VariableName, engine),
                    v_FilePath = this.v_FilePath,
                };
                writeText.RunCommand(engine);
            }
        }
    }
}
