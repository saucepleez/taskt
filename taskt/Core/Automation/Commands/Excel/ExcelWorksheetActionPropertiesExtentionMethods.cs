using Microsoft.Office.Interop.Excel;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{
    public static class ExcelWorksheetActionPropertiesExtentionMethods
    {
        /// <summary>
        /// expand value or variable as worksheet
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static Worksheet ExpandValueOrVariableAsExcelTargetWorksheet(this IExcelWorksheetActionProperties command, AutomationEngineInstance engine)
        {
            return command.ExpandValueOrVariableAsExcelWorksheet(command.v_TargetSheetName, engine);
        }

        /// <summary>
        /// expand value or variable as Excel Instance and Target Worksheet
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static (Application, Worksheet) ExpandValueOrVariableAsExcelInstanceAndTargetWorksheet(this IExcelWorksheetActionProperties command, AutomationEngineInstance engine)
        {
            return (command.ExpandValueOrVariableAsExcelInstance(engine),
                    command.ExpandValueOrVariableAsExcelTargetWorksheet(engine));
        }
    }
}
