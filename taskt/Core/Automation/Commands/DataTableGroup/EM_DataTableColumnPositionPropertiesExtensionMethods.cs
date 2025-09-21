using System;
using System.Data;

namespace taskt.Core.Automation.Commands
{
    public static class EM_DataTableColumnPositionPropertiesExtensionMethods
    {
        /// <summary>
        /// Expand Value Or User Variable as DataTable Column Index and Name
        /// </summary>
        /// <param name="command"></param>
        /// <param name="table"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static (int, string) ExpandValueOrUserVariableAsDataTableColumn(this IDataTableColumnPositionProperties command, DataTable table, Engine.AutomationEngineInstance engine)
        {
            var sc = command.ToScriptCommand();

            switch (sc.ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_ColumnType), "Column Type", engine))
            {
                case "column name":
                    var name = sc.ExpandValueOrUserVariable(nameof(command.v_ColumnIndex), "Column Name", engine);
                    for (int i = table.Columns.Count - 1; i >= 0; i--)
                    {
                        if (table.Columns[i].ColumnName == name)
                        {
                            return (i, name);
                        }
                    }
                    throw new Exception($"Strange DataTable Column Name. Value: '{command.v_ColumnIndex}', Expand Value: '{name}'");

                case "index":
                    var index = sc.ExpandValueOrUserVariableAsInteger(nameof(command.v_ColumnIndex), "Column Index", engine);
                    if (index < 0)
                    {
                        index += table.Columns.Count;
                    }
                    if (index >= 0 && index < table.Columns.Count)
                    {
                        return (index, table.Columns[index].ColumnName);
                    }
                    else
                    {
                        throw new Exception($"Strange DataTable Column Index. Index: '{command.v_ColumnIndex}', Expand Value: '{index}'");
                    }

                default:
                    // but not come here
                    throw new Exception("error");
            }
        }
    }
}
