using System;

namespace taskt.Core.Automation.Commands
{
    public static class EM_ExcelColumnRangePropertiesExtensionMethods
    {
        /// <summary>
        /// expand value or variable as Excel Range Indicies (Column Index, Row Start Index, Row End Index)
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <param name="excelSheet"></param>
        /// <param name="objectSizeFunc"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static (int columnIndex, int rowStartIndex, int rowEndIndex) ExpandValueOrVariableAsExcelRangeIndicies(this IExcelColumnRangeProperties command, Engine.AutomationEngineInstance engine, Func<int> objectSizeFunc = null)
        {
            (_, var sheet) = command.ExpandValueOrVariableAsExcelInstanceAndCurrentWorksheet(engine);

            // get column index
            int columnIndex = 0;
            switch (((ScriptCommand)command).ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_ColumnType), "Column Type", engine))
            {
                case "range":
                    columnIndex = sheet.ToColumnIndex(command.v_ColumnIndex.ExpandValueOrUserVariable(engine));
                    break;
                case "rc":
                    //columnIndex = command.ExpandValueOrUserVariableAsInteger(columnValueName, "Column", engine);
                    columnIndex = ((ScriptCommand)command).ExpandValueOrUserVariableAsInteger(nameof(command.v_ColumnIndex), "Column", engine);
                    break;
            }

            //int rowStartIndex = command.ExpandValueOrUserVariableAsInteger(rowStartName, "Start Row", engine);
            var rowStartIndex = ((ScriptCommand)command).ExpandValueOrUserVariableAsInteger(nameof(command.v_RowStart), "Start Row", engine);

            //string rowEndValue = command.GetRawPropertyValueAsString(rowEndName, "End Row");
            var rowEndValue = ((ScriptCommand)command).GetRawPropertyValueAsString(nameof(command.v_RowEnd), "End Row");
            int rowEndIndex;
            if (string.IsNullOrEmpty(rowEndValue))
            {
                if (objectSizeFunc == null)
                {
                    var valueType = ((ScriptCommand)command).ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_ValueType), "Value Type", engine);
                    rowEndIndex = sheet.LastRowIndex(columnIndex, rowStartIndex, valueType);
                }
                else
                {
                    rowEndIndex = rowStartIndex + objectSizeFunc() - 1;
                }
            }
            else
            {
                rowEndIndex = rowEndValue.ExpandValueOrUserVariableAsInteger("End Row", engine);
            }

            // swap values
            if (rowStartIndex > rowEndIndex)
            {
                (rowEndIndex, rowStartIndex) = (rowStartIndex, rowEndIndex);
            }

            if (!sheet.RCRangeTryParse(rowStartIndex, columnIndex, rowEndIndex, columnIndex, out _))
            {
                throw new Exception($"Invalid Range. Row Start Index: '{rowStartIndex}', Row End Index: '{rowEndIndex}', Column Index: '{columnIndex}'");
            }

            return (columnIndex, rowStartIndex, rowEndIndex);
        }
    }
}
