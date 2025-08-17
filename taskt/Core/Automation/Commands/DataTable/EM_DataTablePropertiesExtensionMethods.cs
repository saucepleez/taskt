using System.Data;

namespace taskt.Core.Automation.Commands
{
    public static class EM_DataTablePropertiesExtensionMethods
    {
        /// <summary>
        /// Expand User Variable As DataTable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static DataTable ExpandUserVariableAsDataTable(this IDataTableProperties command, Engine.AutomationEngineInstance engine)
        {
            return command.ExpandUserVariableAsDataTable(nameof(command.v_DataTable), engine);
        }
    }
}
