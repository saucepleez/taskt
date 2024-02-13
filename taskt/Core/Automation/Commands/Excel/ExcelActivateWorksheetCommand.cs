using System;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Worksheet")]
    [Attributes.ClassAttributes.CommandSettings("Activate Worksheet")]
    [Attributes.ClassAttributes.Description("This command allows you to activate a specific worksheet in a workbook")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to switch to a specific worksheet")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelActivateWorksheetCommand : AExcelSheetCommand
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        //public string v_InstanceName { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_SheetName))]
        //public string v_SheetName { get; set; }

        public ExcelActivateWorksheetCommand()
        {
            //this.CommandName = "ExcelActivateSheetCommand";
            //this.SelectionName = "Activate Sheet";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //(var excelInstance, var currentSheet) = v_InstanceName.ExpandValueOrUserVariableAsExcelInstanceAndWorksheet(engine);

            //Microsoft.Office.Interop.Excel.Worksheet targetSheet = v_SheetName.ExpandValueOrUserVariableAsExcelWorksheet(engine, excelInstance);

            (_, var targetSheet, var currentSheet) = this.ExpandValueOrVariableAsExcelInstnaceAndWorksheetAndCurrentSheet(engine);

            if (currentSheet.Name != targetSheet.Name)
            {
                targetSheet.Select();
            }
        }
    }
}