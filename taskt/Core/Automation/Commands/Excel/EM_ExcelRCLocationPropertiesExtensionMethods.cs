using System;
using Microsoft.Office.Interop.Excel;

namespace taskt.Core.Automation.Commands
{
    public static class EM_ExcelRCLocationPropertiesExtensionMethods
    {
        /// <summary>
        /// expand value or variable as Cell Row Index and Column Index (row, column)
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static (int, int) ExpandValueOrVariableAsCellRowAndColumnIndex(this IExcelRCLocationProperties command, Engine.AutomationEngineInstance engine)
        {
            var row = ((ScriptCommand)command).ExpandValueOrUserVariableAsInteger(nameof(command.v_CellRow), "Cell Row", engine);
            var column = ((ScriptCommand)command).ExpandValueOrUserVariableAsInteger(nameof(command.v_CellColumn), "Cell Column", engine);

            var excelInstance = command.ExpandValueOrVariableAsExcelInstance(engine);
            if (excelInstance.RCLocationTryParse(row, column, out _))
            {
                return (row, column);
            }
            else
            {
                throw new Exception($"Invalid Cell Location. Row: '{command.v_CellRow}', Column: '{command.v_CellColumn}', Row Expanded: '{row}', Column Expanded: '{command}'");
            }
        }

        /// <summary>
        /// RC Location Action
        /// </summary>
        /// <param name="command"></param>
        /// <param name="actionFunc">arguments (sheet, column, row)</param>
        /// <param name="engine"></param>
        public static void RCLocationAction(this IExcelRCLocationProperties command, Action<Worksheet, int, int> actionFunc, Engine.AutomationEngineInstance engine)
        {
            (_, var sheet) = command.ExpandValueOrVariableAsExcelInstanceAndCurrentWorksheet(engine);
            (var row, var column) = command.ExpandValueOrVariableAsCellRowAndColumnIndex(engine);
            actionFunc(sheet, column, row);
        }
    }
}
