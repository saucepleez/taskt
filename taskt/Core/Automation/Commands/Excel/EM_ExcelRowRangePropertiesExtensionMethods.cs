using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace taskt.Core.Automation.Commands
{
    public static class EM_ExcelRowRangePropertiesExtensionMethods
    {
        public static (int rowIndex, int columnStartIndex, int columnEndIndex, string valueType) GetRangeIndeiesRowDirection(this IExcelRowRangeProperties command, Engine.AutomationEngineInstance engine)
        {
            (var excelInstance, var sheet) = command.ExpandValueOrVariableAsExcelInstanceAndCurrentWorksheet(engine);

            //int rowIndex = command.ExpandValueOrUserVariableAsInteger(rowValueName, "Row Index", engine);
            var rowIndex = ((ScriptCommand)command).ExpandValueOrUserVariableAsInteger(nameof(command.v_RowIndex), "Row Index", engine);

            //string valueType = command.ExpandValueOrUserVariableAsSelectionItem(valueTypeName, "Value Type", engine);
            var valueType = ((ScriptCommand)command).ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_ValueType), "Value Type", engine);

            int columnStartIndex = 0;
            int columnEndIndex = 0;

            string columnStartValue = command.GetRawPropertyValueAsString(columnStartName, "Start Column");
            string columnEndValue = command.GetRawPropertyValueAsString(columnEndName, "End Column");

            string columnType = command.ExpandValueOrUserVariableAsSelectionItem(columnTypeName, "Column Type", engine);
            switch (columnType)
            {
                case "range":
                    if (String.IsNullOrEmpty(columnStartValue))
                    {
                        columnStartValue = "A";
                    }
                    columnStartIndex = ExcelControls.GetColumnIndex(excelSheet, columnStartValue.ExpandValueOrUserVariable(engine));


                    if (String.IsNullOrEmpty(columnEndValue))
                    {
                        columnEndIndex = ExcelControls.GetLastColumnIndex(excelSheet, rowIndex, columnStartIndex, valueType);
                    }
                    else
                    {
                        columnEndIndex = ExcelControls.GetColumnIndex(excelSheet, columnEndValue.ExpandValueOrUserVariable(engine));
                    }
                    break;

                case "rc":
                    if (String.IsNullOrEmpty(columnStartValue))
                    {
                        columnStartValue = "1";
                    }
                    columnStartIndex = columnStartValue.ExpandValueOrUserVariableAsInteger("Start Column", engine);

                    if (String.IsNullOrEmpty(columnEndValue))
                    {
                        columnEndIndex = ExcelControls.GetLastColumnIndex(excelSheet, rowIndex, columnStartIndex, valueType);
                    }
                    else
                    {
                        columnEndIndex = columnEndValue.ExpandValueOrUserVariableAsInteger("Column End", engine);
                    }
                    break;
            }

            if (columnStartIndex > columnEndIndex)
            {
                int t = columnStartIndex;
                columnStartIndex = columnEndIndex;
                columnEndIndex = t;
            }

            CheckCorrectRCRange(rowIndex, columnStartIndex, rowIndex, columnEndIndex, excelSheet);

            return (rowIndex, columnStartIndex, columnEndIndex, valueType);
        }
    }
}
