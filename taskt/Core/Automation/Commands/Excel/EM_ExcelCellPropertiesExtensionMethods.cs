using System;
using Microsoft.Office.Interop.Excel;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{
    public static class EM_ExcelCellPropertiesExtensionMethods
    {
        ///// <summary>
        ///// check Correct Cell Location
        ///// </summary>
        ///// <param name="range"></param>
        ///// <param name="excelInstance"></param>
        ///// <returns></returns>
        //public static bool CheckCorrectLocation(string range, Application excelInstance)
        //{
        //    try
        //    {
        //        var rg = excelInstance.Range[range];
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        /// <summary>
        /// expand value or variable as Single Cell Location
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Range ExpandValueOrVariableAsExcelSingleCellLocation(this IExcelCellProperties command, AutomationEngineInstance engine)
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

        /// <summary>
        /// expand value or variable as Excel Instance and Single Cell Location
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static (Application, Range) ExpandValueOrVariableAsExcelInstanceAndSingleCellLocation(this IExcelCellProperties command, AutomationEngineInstance engine)
        {
            return (command.ExpandValueOrVariableAsExcelInstance(engine),
                    command.ExpandValueOrVariableAsExcelSingleCellLocation(engine));
        }
    }
}
