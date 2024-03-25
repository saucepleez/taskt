﻿using System;
using System.Data;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Column")]
    [Attributes.ClassAttributes.CommandSettings("Get Column Values As DataTable")]
    [Attributes.ClassAttributes.Description("This command get Column values as DataTable.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get Column values as DatTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelGetColumnValuesAsDataTableCommand : AExcelColumnRangeGetCommands
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
            //this.CommandName = "ExcelGetColumnValuesAsDataTableCommand";
            //this.SelectionName = "Get Column Values As DataTable";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //(var excelInstance, var excelSheet) = v_InstanceName.ExpandValueOrUserVariableAsExcelInstanceAndWorksheet(engine);

            //(int columnIndex, int rowStart, int rowEnd, string valueType) =
            //    ExcelControls.GetRangeIndeiesColumnDirection(
            //        nameof(v_ColumnIndex), nameof(v_ColumnType),
            //        nameof(v_RowStart), nameof(v_RowEnd), nameof(v_ValueType),
            //        engine, excelSheet, this
            //    );

            //Func<Microsoft.Office.Interop.Excel.Worksheet, int, int, string> getFunc = ExcelControls.GetCellValueFunction(valueType);

            //DataTable newDT = new DataTable();
            //newDT.Columns.Add(ExcelControls.GetColumnName(excelSheet, columnIndex));

            //int tblRow = 0;
            //for (int i = rowStart; i <= rowEnd; i++)
            //{
            //    newDT.Rows.Add();
            //    newDT.Rows[tblRow][0] = getFunc(excelSheet, columnIndex, i);
            //    tblRow++;
            //}

            //newDT.StoreInUserVariable(engine, v_Result);

            //(_, var sheet) = this.ExpandValueOrVariableAsExcelInstanceAndCurrentWorksheet(engine);
            //(var columnIndex, var rowStartIndex, var rowEndIndex) = this.ExpandValueOrVariableAsExcelRangeIndicies(engine);
            //var getFunc = this.ExpandValueOrVariableAsGetValueFunction(engine);

            //var newDT = new DataTable();
            //newDT.Columns.Add(sheet.ToColumnName(columnIndex));

            //int max = rowEndIndex - rowStartIndex + 1;
            ////int rowCnt = 0;
            //for (int i = 0; i < max; i++)
            //{
            //    newDT.Rows.Add();
            //    newDT.Rows[i][0] = getFunc(sheet, columnIndex, rowStartIndex + i);
            //    //rowCnt++;
            //}
            //newDT.StoreInUserVariable(engine, v_Result);

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
            newDT.StoreInUserVariable(engine, v_Result);
        }
    }
}