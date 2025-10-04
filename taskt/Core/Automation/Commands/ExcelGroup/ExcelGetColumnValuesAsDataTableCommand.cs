using System;
using System.Data;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel")]
    [Attributes.ClassAttributes.SubGruop("Column")]
    [Attributes.ClassAttributes.CommandSettings("Get Column Values As DataTable")]
    [Attributes.ClassAttributes.Description("This command get Column values as DataTable.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get Column values as DatTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ExcelGetColumnValuesAsDataTableCommand : AExcelColumnRangeGetCommands, IDataTableResultProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        //public string v_InstanceName { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnType))]
        //[PropertyParameterOrder(6000)]
        //public string v_ColumnType { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnNameOrIndex))]
        //[PropertyParameterOrder(6001)]
        //public string v_ColumnIndex { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_RowStart))]
        //[PropertyParameterOrder(6002)]
        //public string v_RowStart { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_RowEnd))]
        //[PropertyParameterOrder(6003)]
        //public string v_RowEnd { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_OutputDataTableName))]
        //[PropertyParameterOrder(6004)]
        public override string v_Result { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ValueType))]
        //[PropertyParameterOrder(6005)]
        //public string v_ValueType { get; set; }

        public ExcelGetColumnValuesAsDataTableCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var newDT = new DataTable();

            this.ColumnRangeAction(
                new Action<Microsoft.Office.Interop.Excel.Worksheet, string, int, int, int>((sheet, value, column, row, count) =>
                {
                    if (count == 0)
                    {
                        newDT.Columns.Add(sheet.ToColumnName(column));
                    }
                    newDT.Rows.Add();
                    newDT.Rows[count][0] = value;
                }), engine
            );

            this.StoreDataTableInUserVariable(newDT, engine);
        }
    }
}