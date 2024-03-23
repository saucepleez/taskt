using System;
using System.Data;

namespace taskt.Core.Automation.Commands
{
    public static class EM_CanHandleDataTable
    {
        /// <summary>
        /// expand user variable as DataTable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static DataTable ExpandUserVariableAsDataTable(this ICanHandleDictionary command, string parameterName, Engine.AutomationEngineInstance engine)
        {
            var variableName = ((ScriptCommand)command).GetRawPropertyValueAsString(parameterName, "DataTable Variable");
            var v = variableName.GetRawVariable(engine);
            if (v.VariableValue is DataTable table)
            {
                return table;
            }
            else
            {
                throw new Exception($"Variable '{variableName}' is not DataTable");
            }
        }

        /// <summary>
        /// store DataTable in User Variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="table"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        public static void StoreDataTableInUserVariable(this ICanHandleColor command, DataTable table, string parameterName, Engine.AutomationEngineInstance engine)
        {
            var variableName = ((ScriptCommand)command).GetRawPropertyValueAsString(parameterName, "DataTable Variable");
            ExtensionMethods.StoreInUserVariable(variableName, table, engine);
        }
    }
}
