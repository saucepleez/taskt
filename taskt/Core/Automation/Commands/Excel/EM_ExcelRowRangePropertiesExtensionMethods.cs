using System;
using Microsoft.Office.Interop.Excel;

namespace taskt.Core.Automation.Commands
{
    public static class EM_ExcelRowRangePropertiesExtensionMethods
    {
        public static (int rowIndex, int columnStartIndex, int columnEndIndex) ExpandValueOrVariableAsRangeIndecies(this IExcelRowRangeProperties command, Engine.AutomationEngineInstance engine)
        {
            (_, var sheet) = command.ExpandValueOrVariableAsExcelInstanceAndCurrentWorksheet(engine);

            var rowIndex = ((ScriptCommand)command).ExpandValueOrUserVariableAsInteger(nameof(command.v_RowIndex), "Row Index", engine);

            var valueType = ((ScriptCommand)command).ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_ValueType), "Value Type", engine);

            int columnStartIndex = 0;
            int columnEndIndex = 0;

            string columnStartValue = ((ScriptCommand)command).GetRawPropertyValueAsString(nameof(command.v_ColumnStart), "Start Column");
            string columnEndValue = ((ScriptCommand)command).GetRawPropertyValueAsString(nameof(command.v_ColumnEnd), "End Column");

            switch (((ScriptCommand)command).ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_ColumnType), "Column Type", engine))
            {
                case "range":
                    if (string.IsNullOrEmpty(columnStartValue))
                    {
                        columnStartValue = "A";
                    }
                    columnStartIndex = sheet.ToColumnIndex(columnStartValue.ExpandValueOrUserVariable(engine));


                    if (string.IsNullOrEmpty(columnEndValue))
                    {
                        columnEndIndex = sheet.GetLastColumnIndex(rowIndex, columnStartIndex, valueType);
                    }
                    else
                    {
                        columnEndIndex = sheet.ToColumnIndex(columnEndValue.ExpandValueOrUserVariable(engine));
                    }
                    break;

                case "rc":
                    if (string.IsNullOrEmpty(columnStartValue))
                    {
                        columnStartValue = "1";
                    }
                    columnStartIndex = columnStartValue.ExpandValueOrUserVariableAsInteger("Start Column", engine);

                    if (string.IsNullOrEmpty(columnEndValue))
                    {
                        columnEndIndex = sheet.GetLastColumnIndex(rowIndex, columnStartIndex, valueType);
                    }
                    else
                    {
                        columnEndIndex = columnEndValue.ExpandValueOrUserVariableAsInteger("Column End", engine);
                    }
                    break;
            }

            // swap column
            if (columnStartIndex > columnEndIndex)
            {
                (columnEndIndex, columnStartIndex) = (columnStartIndex, columnEndIndex);
            }

            if (!sheet.RCRangeTryParse(rowIndex, columnStartIndex, rowIndex, columnEndIndex, out _))
            {
                throw new Exception($"Invalid Range. Row Index: '{rowIndex}', Column Start Index: '{columnStartIndex}', Column End Index: '{columnEndIndex}'");
            }

            return (rowIndex, columnStartIndex, columnEndIndex);
        }
    }
}
