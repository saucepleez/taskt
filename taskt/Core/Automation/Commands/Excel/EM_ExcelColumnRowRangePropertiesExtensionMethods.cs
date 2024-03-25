using System;

namespace taskt.Core.Automation.Commands
{
    public static class EM_ExcelColumnRowRangePropertiesExtensionMethods
    {
        /// <summary>
        /// expand value or variable as Excel Range Indicies (rowStart, columnStart, rowEnd, columnEnd)
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <param name="rowSizeFunc"></param>
        /// <param name="columnSizeFunc"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static (int rowStartIndex, int columnStartIndex, int rowEndIndex, int columnEndIndex) ExpandValueOrVariableAsExcelRangeIndicies(this IExcelColumnRowRangeProperties command, Engine.AutomationEngineInstance engine, Func<int> rowSizeFunc = null, Func<int> columnSizeFunc = null)
        {
            (_, var excelSheet) = command.ExpandValueOrVariableAsExcelInstanceAndCurrentWorksheet(engine);

            var valueType = ((ScriptCommand)command).ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_ValueType), "Value Type", engine);

            var rowStartIndex = command.v_RowStart.ExpandValueOrUserVariableAsInteger("Start Row", engine);

            var columnFunc = new Func<int, int, string, int>((row, column, tp) =>
            {
                if (columnSizeFunc != null)
                {
                    return columnSizeFunc();
                }
                else
                {
                    return excelSheet.GetLastColumnIndex(row, column, tp);
                }
            });

            int columnStartIndex = 0;
            int columnEndIndex = 0;
            switch (((ScriptCommand)command).ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_ColumnType), "Column Type", engine))
            {
                case "range":
                    columnStartIndex = excelSheet.ToColumnIndex(command.v_ColumnStart.ExpandValueOrUserVariable(engine));
                    if (string.IsNullOrEmpty(command.v_ColumnEnd))
                    {
                        columnEndIndex = columnFunc(rowStartIndex, columnStartIndex, valueType);
                    }
                    else
                    {
                        columnEndIndex = excelSheet.ToColumnIndex(command.v_ColumnEnd.ExpandValueOrUserVariable(engine));
                    }
                    break;

                case "rc":
                    columnStartIndex = command.v_ColumnStart.ExpandValueOrUserVariableAsInteger("Column Start", engine);
                    if (string.IsNullOrEmpty(command.v_ColumnEnd))
                    {
                        columnEndIndex = columnFunc(rowStartIndex, columnStartIndex, valueType);
                    }
                    else
                    {
                        columnEndIndex = command.v_ColumnEnd.ExpandValueOrUserVariableAsInteger("Column End", engine);
                    }
                    break;
            }

            // swap values
            if (columnStartIndex > columnEndIndex)
            {
                (columnEndIndex, columnStartIndex) = (columnStartIndex, columnEndIndex);
            }

            int rowEndIndex;
            if (string.IsNullOrEmpty(command.v_RowEnd))
            {
                rowEndIndex = (rowSizeFunc != null) ? rowSizeFunc() : excelSheet.LastRowIndex(columnStartIndex, rowStartIndex, valueType);
            }
            else
            {
                rowEndIndex = command.v_RowEnd.ExpandValueOrUserVariableAsInteger("Row End", engine);
            }

            // swap values
            if (rowStartIndex > rowEndIndex)
            {
                (rowEndIndex, rowStartIndex) = (rowStartIndex, rowEndIndex);
            }

            if (!excelSheet.RCRangeTryParse(rowStartIndex, columnStartIndex, rowEndIndex, columnEndIndex, out _))
            {
                throw new Exception($"Invalid Range. Row Start Index: '{rowStartIndex}', Row End Index: '{rowEndIndex}', Column Start Index: '{columnStartIndex}', Column End Index: '{columnEndIndex}'");
            }

            return (rowStartIndex, columnStartIndex, rowEndIndex, columnEndIndex);
        }
    }
}
