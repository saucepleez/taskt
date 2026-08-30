using System.Data;

namespace taskt.Core.Automation.Commands
{
    public static class EM_DataTableRowPropertiesExtensionMethods
    {
        /// <summary>
        /// Expand Value Or User Variable As DataTable and Row
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static (DataTable, int) ExpandValueOrUserVariableAsDataTableAndRow(this IDataTableRowProperties command, Engine.AutomationEngineInstance engine)
        {
            var table = command.ExpandUserVariableAsDataTable(engine);
            var index = command.ExpandValueOrUserVariableAsDataTableRow(table, engine);
            return (table, index);
        }
    }
}
