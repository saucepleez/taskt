﻿using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Column")]
    [Attributes.ClassAttributes.CommandSettings("Get Column Values As Dictionary")]
    [Attributes.ClassAttributes.Description("This command get Column values as Dictionary.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get Column values as Dictionary.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelGetColumnValuesAsDictionaryCommand : AExcelColumnRangeGetCommands
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
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_OutputDictionaryName))]
        //[PropertyParameterOrder(6004)]
        public override string v_Result { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ValueType))]
        //[PropertyParameterOrder(6005)]
        //public string v_ValueType { get; set; }

        public ExcelGetColumnValuesAsDictionaryCommand()
        {
            //this.CommandName = "ExcelGetColumnValuesAsDictionaryCommand";
            //this.SelectionName = "Get Column Values As Dictionary";
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

            //Dictionary<string, string> newDic = new Dictionary<string, string>();

            //for (int i = rowStart; i <= rowEnd; i++)
            //{
            //    string address = ExcelControls.GetAddress(excelSheet, i, columnIndex);
            //    newDic.Add(address, getFunc(excelSheet, columnIndex, i));
            //}

            //newDic.StoreInUserVariable(engine, v_Result);

            //(_, var excelSheet) = this.ExpandValueOrVariableAsExcelInstanceAndCurrentWorksheet(engine);
            //(var columnIndex, var rowStartIndex, var rowEndIndex) = this.ExpandValueOrVariableAsExcelRangeIndicies(engine);

            //var getFunc = this.ExpandValueOrVariableAsGetValueFunction(engine);

            //var newDic = new Dictionary<string, string>();

            //int max = rowEndIndex - rowStartIndex + 1;
            //for (int i = 0; i < max; i++)
            //{
            //    var pos = rowStartIndex + i;
            //    string address = excelSheet.ToCellLocation(pos, columnIndex);
            //    newDic.Add(address, getFunc(excelSheet, columnIndex, pos));
            //}

            //newDic.StoreInUserVariable(engine, v_Result);

            var newDic = new Dictionary<string, string>();

            this.ColumnRangeAction(
                new Action<Microsoft.Office.Interop.Excel.Worksheet, string, int, int, int>((sheet, value, column, row, count) =>
                {
                    string address = sheet.ToCellLocation(row, column);
                    newDic.Add(address, value);
                }), engine
            );

            newDic.StoreInUserVariable(engine, v_Result);
        }
    }
}