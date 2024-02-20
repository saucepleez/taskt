using System;
using Microsoft.Office.Interop.Excel;

namespace taskt.Core.Automation.Commands
{
    public static class EM_ExcelWorksheetPropertiesExtensionMethods
    {
        /// <summary>
        /// expand value or variable as Excel Worksheet
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Worksheet ExpandValueOrVariableAsExcelWorksheet(this IExcelWorksheetProperties command, Engine.AutomationEngineInstance engine)
        {
            return command.ExpandValueOrVariableAsExcelWorksheet(command.v_SheetName, engine);
        }

        /// <summary>
        /// expand value or variable as Excel Instance, Worksheet
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static (Application, Worksheet) ExpandValueOrVariableAsExcelInstanceAndWorksheet(this IExcelWorksheetProperties command, Engine.AutomationEngineInstance engine)
        {
            return (command.ExpandValueOrVariableAsExcelInstance(engine),
                    command.ExpandValueOrVariableAsExcelWorksheet(engine));
        }

        /// <summary>
        /// expand value or variable as Excel Instance, Worksheet, and CurrentWorksheet
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static (Application instance, Worksheet targetSheet, Worksheet currentSheet) ExpandValueOrVariableAsExcelInstnaceAndWorksheetAndCurrentSheet(this IExcelWorksheetProperties command, Engine.AutomationEngineInstance engine)
        {
            (var ins, var current) = command.ExpandValueOrVariableAsExcelInstanceAndCurrentWorksheet(engine);
            var sht = command.ExpandValueOrVariableAsExcelWorksheet(engine);
            return (ins, sht, current);
        }
    }
}
