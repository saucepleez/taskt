using System;
using Microsoft.Office.Interop.Excel;

namespace taskt.Core.Automation.Commands
{
    public static class EM_ExcelRCLocationPropertiesExtensionMethods
    {
        /// <summary>
        /// expand value or variable as Excel Cell Location
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Range ExpandValueOrVariableAsExcelCellLocation(this IExcelRCLocationProperties command, Engine.AutomationEngineInstance engine)
        {
            var row = ((ScriptCommand)command).ExpandValueOrUserVariableAsInteger(nameof(command.v_CellRow), "Cell Row", engine);
            var column = ((ScriptCommand)command).ExpandValueOrUserVariableAsInteger(nameof(command.v_CellColumn), "Cell Column", engine);

            var excelInstance = command.ExpandValueOrVariableAsExcelInstance(engine);

            if (excelInstance.RCLocationTryParse(row, column, out Range rg))
            {
                return rg;
            }
            else
            {
                throw new Exception($"Invalid Cell Location. Row: '{command.v_CellRow}', Column: '{command.v_CellColumn}', Row Expanded: '{row}', Column Expanded: '{command}'");
            }
        }

        ///// <summary>
        ///// expand value or variable as Excel Cell Row, Column, and Range
        ///// </summary>
        ///// <param name="command"></param>
        ///// <param name="engine"></param>
        ///// <returns></returns>
        ///// <exception cref="Exception"></exception>
        //public static (int row, int column, Range rg) ExpandValueOrVariableAsExcelCellRowAndCellColumnAndRange(this IExcelRCLocationProperties command, Engine.AutomationEngineInstance engine)
        //{
        //    var row = ((ScriptCommand)command).ExpandValueOrUserVariableAsInteger(nameof(command.v_CellRow), "Cell Row", engine);
        //    var column = ((ScriptCommand)command).ExpandValueOrUserVariableAsInteger(nameof(command.v_CellColumn), "Cell Column", engine);

        //    var excelInstance = command.ExpandValueOrVariableAsExcelInstance(engine);

        //    if (excelInstance.RCLocationTryParse(row, column, out Range rg))
        //    {
        //        return (row, column, rg);
        //    }
        //    else
        //    {
        //        throw new Exception($"Invalid Cell Location. Row: '{command.v_CellRow}', Column: '{command.v_CellColumn}', Row Expanded: '{row}', Column Expanded: '{command}'");
        //    }
        //}
    }
}
