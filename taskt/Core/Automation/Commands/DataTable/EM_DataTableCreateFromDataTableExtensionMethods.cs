using System.Data;

namespace taskt.Core.Automation.Commands
{
    public static class EM_DataTableCreateFromDataTableExtensionMethods
    {
        /// <summary>
        /// Expand User Variable as DataTable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static DataTable ExpandUserVariableAsDataTable(this IDataTableCreateFromDataTableProperties command, Engine.AutomationEngineInstance engine)
        {
            return command.ExpandUserVariableAsDataTable(nameof(command.v_TargetDataTable), engine);
        }

        /// <summary>
        /// Store DataTable in User Variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="table"></param>
        /// <param name="engine"></param>
        public static void StoreDataTableInUserVariable(this IDataTableCreateFromDataTableProperties command, DataTable table, Engine.AutomationEngineInstance engine)
        {
            command.StoreDataTableInUserVariable(table, nameof(command.v_NewDataTable), engine);
        }
    }
}
