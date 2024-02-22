using System;
using taskt.Core.Automation.Engine;

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

        /// <summary>
        /// column range action
        /// </summary>
        /// <param name="command"></param>
        /// <param name="objectSizeFunc"></param>
        /// <param name="valueFunc">arg is (index)</param>
        /// <param name="engine"></param>
        public static void ColumnRangeAction(this IExcelColumnRangeSetProperties command, Func<int> objectSizeFunc, Func<int, string> valueFunc, AutomationEngineInstance engine)
        {
            (_, var excelSheet) = command.ExpandValueOrVariableAsExcelInstanceAndCurrentWorksheet(engine);

            (var columnIndex, var rowStartIndex, var rowEndIndex) = command.ExpandValueOrVariableAsExcelRangeIndicies(engine, objectSizeFunc);
            var setFunc = command.ExpandValueOrVaribleAsSetValueAction(engine);

            int max = rowEndIndex - rowStartIndex + 1;
            for (int i = 0; i < max; i++)
            {
                setFunc(valueFunc(i), excelSheet, columnIndex, rowStartIndex + i);
            }
        }
    }
}
