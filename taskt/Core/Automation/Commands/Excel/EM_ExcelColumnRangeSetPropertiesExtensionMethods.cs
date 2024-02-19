using System;

namespace taskt.Core.Automation.Commands
{
    public static class EM_ExcelColumnRangeSetPropertiesExtensionMethods
    {
        /// <summary>
        /// expand value or variable as Excel Range Indicies (Column Index, Row Start Index, Row End Index)
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <param name="objectSizeFunc"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static (int columnIndex, int rowStartIndex, int rowEndIndex) ExpandValueOrVariableAsExcelRangeIndicies(this IExcelColumnRangeSetProperties command, Engine.AutomationEngineInstance engine, Func<int> objectSizeFunc)
        {
            (var columnIndex, var rowStartIndex, var rowEndIndex) = ((IExcelColumnRangeProperties)command).ExpandValueOrVariableAsExcelRangeIndicies(engine, objectSizeFunc);

            var whenItemNotEnough = ((ScriptCommand)command).ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_WhenItemNotEnough), "When Item Not Enough", engine);

            var range = rowEndIndex - rowStartIndex + 1;
            var size = objectSizeFunc();
            if (range > objectSizeFunc())
            {
                switch (whenItemNotEnough)
                {
                    case "error":
                        throw new Exception("Size of Object Item(s) to Set is Not Enough.");
                    default:
                        rowEndIndex = rowStartIndex + size - 1;
                        break;
                }
            }
            return (columnIndex, rowStartIndex, rowEndIndex);
        }
    }
}
