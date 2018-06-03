using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace taskt.Core
{
    class DatasetCommands
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



    }
}
