using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel")]
    [Attributes.ClassAttributes.SubGruop("Cell")]
    [Attributes.ClassAttributes.CommandSettings("Go To Cell RC")]
    [Attributes.ClassAttributes.Description("This command moves to a specific cell.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to move to a specific cell.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Excel Interop' to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ExcelGoToCellRCCommand : AExcelRCLocationCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        //public string v_InstanceName { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_RowLocation))]
        //[PropertyParameterOrder(6000)]
        //public string v_CellRow { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnLocation))]
        //[PropertyParameterOrder(6001)]
        //public string v_CellColumn { get; set; }

        public ExcelGoToCellRCCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.RCLocationAction(new Action<Microsoft.Office.Interop.Excel.Worksheet, int, int>((sheet, column, row) =>
            {
                sheet.CellRange(row, column).Select();
            }), engine);
        }
    }
}