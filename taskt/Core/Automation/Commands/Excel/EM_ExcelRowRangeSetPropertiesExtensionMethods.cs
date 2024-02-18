using System;

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
        public static (int rowIndex, int columnStartIndex, int columnEndIndex) ExpandValueOrVariableAsRangeIndecies(this IExcelRowRangeSetProperties command, Engine.AutomationEngineInstance engine, Func<int> objectSizeFunc)
        {
            (var row, var columnStart, var columnEnd) = ((IExcelRowRangeProperties)command).ExpandValueOrVariableAsRangeIndecies(engine, objectSizeFunc);

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
    }
}
