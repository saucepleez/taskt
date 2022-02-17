using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace taskt.Core
{
    class DataTableControl
    {
        public System.Data.DataTable CreateDataTable(string connection, string query)
        {
            //create vars
            var dataTable = new DataTable();
            var oleConnection = new System.Data.OleDb.OleDbConnection(connection);
            var oleCommand = new System.Data.OleDb.OleDbCommand(query, oleConnection);

            //get data
            System.Data.OleDb.OleDbDataAdapter adapter = new System.Data.OleDb.OleDbDataAdapter(oleCommand);
            oleConnection.Open();
            adapter.Fill(dataTable);
            oleConnection.Close();

            //clean up
            oleConnection.Dispose();
            adapter.Dispose();
            oleCommand.Dispose();

            //foreach (var rw in dataTable.Rows)
            //{

            //}

            return dataTable;
        }

        public static int GetRowIndex(string dataTableName, string rowIndex, Automation.Engine.AutomationEngineInstance engine)
        {
            var srcDT = dataTableName.GetDataTableVariable(engine);

            int index;
            if (String.IsNullOrEmpty(rowIndex))
            {
                index = dataTableName.GetRawVariable(engine).CurrentPosition;
            }
            else
            {
                index = int.Parse(rowIndex.ConvertToUserVariable(engine));
                if (index < 0)
                {
                    index = srcDT.Rows.Count + index;
                }
            }

            if ((index < 0) || (index >= srcDT.Rows.Count))
            {
                throw new Exception("Strange Row Index " + rowIndex + ", parsed " + index);
            }

            return index;
        }

        public static bool isColumnExists(DataTable table, string columnName)
        {
            foreach(string column in table.Columns)
            {
                if (column == columnName)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool isColumnExists(DataTable table, int columnIndex)
        {
            if (columnIndex >= 0)
            {
                return (columnIndex < table.Columns.Count);
            }
            else
            {
                int idx = table.Columns.Count + columnIndex;
                return ((idx >= 0) && (idx < table.Columns.Count));
            }
        }

        public static string GetColumnName(string tableName, string columnName, Automation.Engine.AutomationEngineInstance engine)
        {
            DataTable table = tableName.GetDataTableVariable(engine);
            string col = columnName.ConvertToUserVariable(engine);
            if (isColumnExists(table, col))
            {
                return col;
            }
            else
            {
                throw new Exception("Strange Column Name " + columnName);
            }
        }

        public static int GetColumnIndex(string tableName, string columnIndex, Automation.Engine.AutomationEngineInstance engine)
        {
            DataTable table = tableName.GetDataTableVariable(engine);

            int index;
            if (int.TryParse(columnIndex.ConvertToUserVariable(engine), out index))
            {
                if (index < 0)
                {
                    index = table.Columns.Count + index;
                }
                if (isColumnExists(table, index))
                {
                    return index;
                }
                else
                {
                    throw new Exception("Strange Column Index " + columnIndex + ", parse " + index);
                }
            }
            else
            {
                throw new Exception("Strange Column Index " + columnIndex + ", parse " + index);
            }
        }
    }
}
