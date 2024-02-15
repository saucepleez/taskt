using System;
using Microsoft.Office.Interop.Excel;

namespace taskt.Core.Automation.Commands
{
    public static class EM_ExcelRCLocationPropertiesExtensionMethods
    {
        /// <summary>
        /// try parse RC Location
        /// </summary>
        /// <param name="excelInstance"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="rg"></param>
        /// <returns></returns>
        public static bool RCLocationTryParse(this Application excelInstance, int row, int column, out Range rg)
        {
            try
            {
                rg = excelInstance.Cells[row, column];
                return true;
            }
            catch
            {
                rg = null;
                return false;
            }
        }

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
