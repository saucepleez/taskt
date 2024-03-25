﻿using System;
using System.Data;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Row")]
    [Attributes.ClassAttributes.CommandSettings("Get Row Values As DataTable")]
    [Attributes.ClassAttributes.Description("This command get Row values as DataTable.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get a Row values as DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelGetRowValuesAsDataTableCommand : AExcelRowRangeGetCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        //public string v_InstanceName { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_RowLocation))]
        //[PropertyParameterOrder(6000)]
        //public string v_RowIndex { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnType))]
        //[PropertyParameterOrder(6001)]
        //public string v_ColumnType { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnStart))]
        //[PropertyParameterOrder(6002)]
        //public string v_ColumnStart { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnEnd))]
        //[PropertyParameterOrder(6003)]
        //public string v_ColumnEnd { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_OutputDataTableName))]
        public override string v_Result { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ValueType))]
        //[PropertyParameterOrder(6005)]
        //public string v_ValueType { get; set; }

        public ExcelGetRowValuesAsDataTableCommand()
        {
            //this.CommandName = "ExcelGetRowValuesAsDataTableCommand";
            //this.SelectionName = "Get Row Values As DataTable";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //(var excelInstance, var excelSheet) = v_InstanceName.ExpandValueOrUserVariableAsExcelInstanceAndWorksheet(engine);

            //(int rowIndex, int columnStartIndex, int columnEndIndex, string valueType) =
            //    ExcelControls.GetRangeIndeiesRowDirection(
            //        nameof(v_RowIndex),
            //        nameof(v_ColumnType), nameof(v_ColumnStart), nameof(v_ColumnEnd),
            //        nameof(v_ValueType), engine, excelSheet, this
            //    );

            //Func<Microsoft.Office.Interop.Excel.Worksheet, int, int, string> getFunc = ExcelControls.GetCellValueFunction(valueType);

            //(_, var excelSheet) = this.ExpandValueOrVariableAsExcelInstanceAndCurrentWorksheet(engine);
            //(int rowIndex, int columnStartIndex, int columnEndIndex) = this.ExpandValueOrVariableAsExcelRangeIndecies(engine);

            //var getFunc = this.ExpandValueOrVariableAsGetValueFunction(engine);

            //DataTable newDT = new DataTable();
            //newDT.Rows.Add();

            //int max = columnEndIndex - columnStartIndex + 1;
            ////int colCnt = 0;
            //for (int i = 0; i < max; i++)
            //{
            //    //newDT.Columns.Add(ExcelControls.GetColumnName(excelSheet, i));
            //    var pos = columnStartIndex + i;
            //    newDT.Columns.Add(excelSheet.ToColumnName(pos));
            //    newDT.Rows[0][i] = getFunc(excelSheet, pos, rowIndex);
            //    //colCnt++;
            //}

            //newDT.StoreInUserVariable(engine, v_Result);

            DataTable newDT = new DataTable();
            newDT.Rows.Add();

            this.RowRangeAction(
                new Action<Microsoft.Office.Interop.Excel.Worksheet, string, int, int, int>((sheet, value, column, row, count) => 
                {
                    newDT.Columns.Add(sheet.ToColumnName(column));
                    newDT.Rows[0][count] = value;
                }), engine
            );

            newDT.StoreInUserVariable(engine, v_Result);
        }
    }
}