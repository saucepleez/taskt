using System;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel")]
    [Attributes.ClassAttributes.SubGruop("Worksheet")]
    [Attributes.ClassAttributes.CommandSettings("Activate Worksheet")]
    [Attributes.ClassAttributes.Description("This command allows you to activate a specific worksheet in a workbook")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to switch to a specific worksheet")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ExcelActivateWorksheetCommand : AExcelSheetCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        //public string v_InstanceName { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_SheetName))]
        //public string v_SheetName { get; set; }

        public ExcelActivateWorksheetCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            (_, var targetSheet, var currentSheet) = this.ExpandValueOrVariableAsExcelInstnaceAndWorksheetAndCurrentSheet(engine);

            if (currentSheet.Name != targetSheet.Name)
            {
                targetSheet.Select();
            }
        }
    }
}