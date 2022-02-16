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
    }
}
