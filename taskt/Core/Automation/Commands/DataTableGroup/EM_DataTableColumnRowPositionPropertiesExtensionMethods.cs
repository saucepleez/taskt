using System.Data;

namespace taskt.Core.Automation.Commands
{
    public static class EM_DataTableColumnRowPositionPropertiesExtensionMethods
    {
        /// <summary>
        /// Expand value or User Variable as DataTable, Column, and Row
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns>(DataTable, Column Index, Column Name, Row Index)</returns>
        public static (DataTable, int, string, int) ExpandValueOrUserVariableAsDataTableAndColumnRow(this IDataTableColumnRowPositionProperties command, Engine.AutomationEngineInstance engine)
        {
            var myDT = command.ExpandUserVariableAsDataTable(engine);
            (var colIndex, var colName) = command.ExpandValueOrUserVariableAsDataTableColumn(myDT, engine);
            var rowIndex = command.ExpandValueOrUserVariableAsDataTableRow(myDT, engine);
            return (myDT, colIndex, colName, rowIndex);
        }
    }
}
