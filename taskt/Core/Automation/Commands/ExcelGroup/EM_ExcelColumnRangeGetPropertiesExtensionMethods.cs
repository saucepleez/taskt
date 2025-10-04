using System;
using Microsoft.Office.Interop.Excel;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{
    public static class EM_ExcelColumnRangeGetPropertiesExtensionMethods
    {
        /// <summary>
        /// Column Range Action
        /// </summary>
        /// <param name="command"></param>
        /// <param name="loopAction">args is (Worksheet, value, column, row, count)</param>
        /// <param name="engine"></param>
        public static void ColumnRangeAction(this IExcelColumnRangeGetProperties command, Action<Worksheet, string, int, int, int> loopAction, AutomationEngineInstance engine)
        {
            (_, var sheet) = command.ExpandValueOrVariableAsExcelInstanceAndCurrentWorksheet(engine);

            (var columnIndex, var rowStartIndex, var rowEndIndex) = command.ExpandValueOrVariableAsExcelRangeIndicies(engine);
            var getFunc = command.ExpandValueOrVariableAsGetValueFunction(engine);

            int max = rowEndIndex - rowStartIndex + 1;
            for (int i = 0; i < max; i++)
            {
                var row = rowStartIndex + i;
                loopAction(sheet, getFunc(sheet, columnIndex, row), columnIndex, row, i);
            }
        }
    }
}
