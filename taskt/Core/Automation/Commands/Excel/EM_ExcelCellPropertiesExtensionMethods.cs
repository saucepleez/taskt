using System;
using Microsoft.Office.Interop.Excel;

namespace taskt.Core.Automation.Commands
{
    public static class EM_ExcelCellPropertiesExtensionMethods
    {
        /// <summary>
        /// expand value or variable as Single Cell Location
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Range ExpandValueOrVariableAsExcelSingleCellLocation(this IExcelCellProperties command, Engine.AutomationEngineInstance engine)
        {
            var excelInstance = command.ExpandValueOrVariableAsExcelInstance(engine);

            var r = command.v_CellLocation.ExpandValueOrUserVariable(engine);
            if (excelInstance.CellLocationTryParse(r, out Range rg))
            {
                if (rg.Count == 1)
                {
                    return rg;
                }
                else
                {
                    throw new Exception($"Cell Location '{r}' is not single Cell.");
                }
            }
            else
            {
                throw new Exception($"Invalid Cell Location. Value: '{command.v_CellLocation}', Expand Value: '{r}'");
            }
        }
    }
}
