﻿using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Column")]
    [Attributes.ClassAttributes.CommandSettings("Get Column Values As List")]
    [Attributes.ClassAttributes.Description("This command get Column values as List.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get Column values as List.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelGetColumnValuesAsListCommand : AExcelColumnRangeGetCommands
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
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        //[PropertyParameterOrder(6004)]
        public override string v_Result { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ValueType))]
        //[PropertyParameterOrder(6005)]
        //public string v_ValueType { get; set; }

        public ExcelGetColumnValuesAsListCommand()
        {
            //this.CommandName = "ExcelGetColumnValuesAsListCommand";
            //this.SelectionName = "Get Column Values As List";
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

            //(_, var excelSheet) = this.ExpandValueOrVariableAsExcelInstanceAndCurrentWorksheet(engine);
            //(var columnIndex, var rowStartIndex, var rowEndIndex) = this.ExpandValueOrVariableAsExcelRangeIndicies(engine);
            //var getFunc = this.ExpandValueOrVariableAsGetValueFunction(engine);

            //var newList = new List<string>();

            //int max = rowEndIndex - rowStartIndex + 1;
            //for (int i = 0; i < max; i++)
            //{
            //    newList.Add(getFunc(excelSheet, columnIndex, rowStartIndex + i));
            //}

            //newList.StoreInUserVariable(engine, v_Result);

            var newList = new List<string>();

            this.ColumnRangeAction(
                new Action<Microsoft.Office.Interop.Excel.Worksheet, string, int, int, int>((sheet, value, column, row, count) =>
                {
                    newList.Add(value);
                }), engine
            );

            newList.StoreInUserVariable(engine, v_Result);
        }
    }
}