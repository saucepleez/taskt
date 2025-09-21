using System.Collections.Generic;
using System.Data;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Get***FromWindowNamesAsList commands
    /// </summary>
    public abstract class GetFromWindowNamesAsListCommands : AWindowNamesCommands, ICanHandleList
    {
        // nothing, result paramters will be multiple

        /// <summary>
        /// get datatable column values as list
        /// </summary>
        /// <param name="table"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        protected static List<string> GetColumnValues(ScriptVariable tableVar, int columnIndex)
        {
            var table = (DataTable)tableVar.VariableValue;

            var ret = new List<string>();

            int rows = table.Rows.Count;
            for (int i = 0; i < rows; i++)
            {
                ret.Add(table.Rows[i][columnIndex]?.ToString() ?? string.Empty);
            }

            return ret;
        }
    }
}
