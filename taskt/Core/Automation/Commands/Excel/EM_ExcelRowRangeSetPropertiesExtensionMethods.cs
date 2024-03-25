using System;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{
    public static class EM_ExcelRowRangeSetPropertiesExtensionMethods
    {
        /// <summary>
        /// expand value or variable as Range Indecies (Row-Start, Column-Start, Row-End, Column-End)
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <param name="objectSizeFunc"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static (int rowIndex, int columnStartIndex, int columnEndIndex) ExpandValueOrVariableAsExcelRangeIndecies(this IExcelRowRangeSetProperties command, Engine.AutomationEngineInstance engine, Func<int> objectSizeFunc)
        {
            (var row, var columnStart, var columnEnd) = ((IExcelRowRangeProperties)command).ExpandValueOrVariableAsExcelRangeIndecies(engine, objectSizeFunc);

            var whenItemNotEnough = ((ScriptCommand)command).ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_WhenItemNotEnough), "When Item Not Enough", engine);

            var range = columnEnd - columnStart + 1;
            var size = objectSizeFunc();
            if (range > objectSizeFunc())
            {
                switch (whenItemNotEnough)
                {
                    case "error":
                        throw new Exception("Size of Object Item(s) to Set is Not Enough.");
                    default:
                        columnEnd = columnStart + size - 1;
                        break;
                }
            }
            return (row, columnStart, columnEnd);
        }

        /// <summary>
        /// row range index
        /// </summary>
        /// <param name="command"></param>
        /// <param name="objectSizeFunc"></param>
        /// <param name="valueFunc">args is (count)</param>
        /// <param name="engine"></param>
        public static void RowRangeAction(this IExcelRowRangeSetProperties command, Func<int> objectSizeFunc, Func<int, string> valueFunc, AutomationEngineInstance engine)
        {
            (_, var excelSheet) = command.ExpandValueOrVariableAsExcelInstanceAndCurrentWorksheet(engine);

            (var rowIndex, var columnStartIndex, var columnEndIndex) = command.ExpandValueOrVariableAsExcelRangeIndecies(engine, objectSizeFunc);

            var setFunc = command.ExpandValueOrVaribleAsSetValueAction(engine);
            var max = columnEndIndex - columnStartIndex + 1;
            for (int i = 0; i < max; i++)
            {
                setFunc(valueFunc(i), excelSheet, columnStartIndex + i, rowIndex);
            }
        }
    }
}
