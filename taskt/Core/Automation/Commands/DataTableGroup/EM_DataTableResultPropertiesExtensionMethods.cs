using System.Data;

namespace taskt.Core.Automation.Commands
{
    public static class EM_DataTableResultPropertiesExtensionMethods
    {
        /// <summary>
        /// Store DataTable in User Variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="table"></param>
        /// <param name="engine"></param>
        public static void StoreDataTableInUserVariable(this IDataTableResultProperties command, DataTable table, Engine.AutomationEngineInstance engine)
        {
            command.StoreDataTableInUserVariable(table, nameof(command.v_Result), engine);
        }
    }
}
