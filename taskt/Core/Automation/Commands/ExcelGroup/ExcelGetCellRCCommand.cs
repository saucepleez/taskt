using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel")]
    [Attributes.ClassAttributes.SubGruop("Cell")]
    [Attributes.ClassAttributes.CommandSettings("Get Cell RC")]
    [Attributes.ClassAttributes.Description("This command gets text from a specified Excel Cell.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get a value from a specific cell.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Excel Interop' to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ExcelGetCellRCCommand : AExcelRCLocationActionCommands
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

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyParameterOrder(8000)]
        public string v_Result { get; set; }

        ////[XmlAttribute]
        ////[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ValueType))]
        ////[PropertyParameterOrder(6003)]
        ////public string v_ValueType { get; set; }

        public ExcelGetCellRCCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.RCLocationAction(new Action<Microsoft.Office.Interop.Excel.Worksheet, int, int>((sheet, column, row) =>
            {
                var getFunc = this.ExpandValueOrVariableAsGetValueFunction(engine);
                getFunc(sheet, column, row).StoreInUserVariable(engine, v_Result);
            }), engine);
        }
    }
}