using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace taskt.Core.Automation.Commands
{
    public static class EM_DataTableExtensionMethods
    {
        /// <summary>
        /// Get DataTable Column Name List
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static List<string> GetColumnNameList(this DataTable table)
        {
            return table.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
        }

        /// <summary>
        /// Check Column Name Exists
        /// </summary>
        /// <param name="table"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsColumnNameExists(this DataTable table, string name)
        {
            return table.GetColumnNameList().Contains(name);
        }

        /// <summary>
        /// Clone DataTable Only Columns
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static DataTable CloneDataTableOnlyColumns(this DataTable table)
        {
            var newDT = new DataTable();

            //table.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToArray()

            //newDT.Columns.AddRange(table.Columns.Cast<DataColumn>().ToArray());

            var cols = table.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToArray();
            foreach(var c in cols)
            {
                newDT.Columns.Add(c);
            }

            return newDT;
        }
    }
}
