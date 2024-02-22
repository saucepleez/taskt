using System;
using Microsoft.Office.Interop.Excel;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands.Excel
{
    public static class EM_ExcelRowRangeGetPropertiesExtensionMethods
    {
        /// <summary>
        /// row range action
        /// </summary>
        /// <param name="command"></param>
        /// <param name="loopAction">args is (Worksheet, value, column, row, count)</param>
        /// <param name="engine"></param>
        public static void RowRangeAction(this IExcelRowRangeGetProperties command, Action<Worksheet, string, int, int, int> loopAction, AutomationEngineInstance engine)
        {
            (_, var sheet) = command.ExpandValueOrVariableAsExcelInstanceAndCurrentWorksheet(engine);
            (int rowIndex, int columnStartIndex, int columnEndIndex) = command.ExpandValueOrVariableAsExcelRangeIndecies(engine);

            var getFunc = command.ExpandValueOrVariableAsGetValueFunction(engine);

            int max = columnEndIndex - columnStartIndex + 1;
            for (int i = 0; i < max; i++)
            {
                var pos = columnStartIndex + i;
                loopAction(sheet, getFunc(sheet, pos, rowIndex), pos, rowIndex, i);
            }
        }
    }
}
