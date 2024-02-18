using System;

namespace taskt.Core.Automation.Commands
{
    public static class EM_ExcelRowRangePropertiesExtensionMethods
    {
        /// <summary>
        /// expand value or variable As Range Indecies (Row-Start, Column-Start, Row-End, Column-End)
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <param name="objectSizeFunc"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static (int rowIndex, int columnStartIndex, int columnEndIndex) ExpandValueOrVariableAsExcelRangeIndecies(this IExcelRowRangeProperties command, Engine.AutomationEngineInstance engine, Func<int> objectSizeFunc = null)
        {
            (_, var sheet) = command.ExpandValueOrVariableAsExcelInstanceAndCurrentWorksheet(engine);

            var rowIndex = ((ScriptCommand)command).ExpandValueOrUserVariableAsInteger(nameof(command.v_RowIndex), "Row Index", engine);

            var innerLastColumnFunc = new Func<int, int, int>((rowStart, columnStart) =>
            {
                if (objectSizeFunc != null)
                {
                    return columnStart + objectSizeFunc() - 1;
                }
                else
                {
                    var valueType = ((ScriptCommand)command).ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_ValueType), "Value Type", engine);
                    return sheet.GetLastColumnIndex(rowStart, columnStart, valueType);
                }
            });

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
                        //columnEndIndex = (lastColumnFunc != null) ? lastColumnFunc() : sheetLastColumn(rowIndex, columnStartIndex);
                        columnEndIndex = innerLastColumnFunc(rowIndex, columnStartIndex);
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
                        //columnEndIndex = (lastColumnFunc != null) ? lastColumnFunc() : sheetLastColumn(rowIndex, columnStartIndex);
                        columnEndIndex = innerLastColumnFunc(rowIndex, columnStartIndex);
                    }
                    else
                    {
                        columnEndIndex = columnEndValue.ExpandValueOrUserVariableAsInteger("Column End", engine);
                    }
                    break;
            }

            // swap values
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
