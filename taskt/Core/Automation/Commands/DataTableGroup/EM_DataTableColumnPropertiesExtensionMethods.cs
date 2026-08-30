using System.Data;

namespace taskt.Core.Automation.Commands
{
    public static class EM_DataTableColumnPropertiesExtensionMethods
    {
        /// <summary>
        /// Expand Value Or User Variable As DataTable and DataTable Column
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static (DataTable, int columnIndex, string columnName) ExpandValueOrUserVariableAsDataTableAndColumn(this IDataTableColumnProperties command, Engine.AutomationEngineInstance engine)
        {
            var table = command.ExpandUserVariableAsDataTable(engine);

            (var index, var name) = command.ExpandValueOrUserVariableAsDataTableColumn(table, engine);

            return (table, index, name);
        }
    }
}
