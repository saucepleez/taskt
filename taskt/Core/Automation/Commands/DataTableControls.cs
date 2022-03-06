using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace taskt.Core
{
    internal class DataTableControls
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
            for (int i = 0; i < table.Columns.Count; i++)
            {
                if (table.Columns[i].ColumnName == columnName)
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

        public static string GetColumnName(DataTable table, string columnName, Automation.Engine.AutomationEngineInstance engine)
        {
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

        public static int GetColumnIndex(DataTable table, string columnIndex, Automation.Engine.AutomationEngineInstance engine)
        {
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

        public static string GetFieldValue(DataTable dt, string parameterName, string parameterColumnName = "ParameterName", string valueColumnName = "ParameterValue")
        {
            if ((!isColumnExists(dt, parameterColumnName)) || (!isColumnExists(dt, valueColumnName)))
            {
                throw new Exception("Parameter Column or Value Column does not exists");
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][parameterColumnName].ToString() == parameterName)
                {
                    return dt.Rows[i][valueColumnName] == null ? "" : dt.Rows[i][valueColumnName].ToString();
                }
            }
            return "";
        }

        public static string GetFieldValue(DataTable dt, int rowIndex, string columnName = "ParameterValue")
        {
            if (!isColumnExists(dt, columnName))
            {
                throw new Exception("Column Name does not exists");
            }
            if (rowIndex < 0)
            {
                rowIndex = dt.Rows.Count + rowIndex;
            }
            if (rowIndex > dt.Rows.Count)
            {
                throw new Exception("Strange Row Index " + rowIndex);
            }

            return dt.Rows[rowIndex][columnName] == null ? "" : dt.Rows[rowIndex][columnName].ToString();
        }

        public static Dictionary<string, string> GetFieldValues(DataTable dt, string parameterColumnName = "ParameterName", string valueColumnName = "ParameterValue", Automation.Engine.AutomationEngineInstance engine = null)
        {
            if ((!isColumnExists(dt, parameterColumnName)) || (!isColumnExists(dt, valueColumnName)))
            {
                throw new Exception("Parameter Column or Value Column does not exists");
            }

            Dictionary<string, string> dic = new Dictionary<string, string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][parameterColumnName] != null)
                {
                    string value = dt.Rows[i][valueColumnName] == null ? "" : dt.Rows[i][valueColumnName].ToString();
                    dic.Add(dt.Rows[i][parameterColumnName].ToString(), value);
                }
            }

            if (engine != null)
            {
                var keys = dic.Keys.ToArray();
                foreach (string key in keys)
                {
                    dic[key] = dic[key].ConvertToUserVariable(engine);
                }
            }

            return dic;
        }

        public static bool SetParameterValue(DataTable dt, string newValue, string parameterName, string parameterColumnName = "ParameterName", string valueColumnName = "ParameterValue")
        {
            if ((!isColumnExists(dt, parameterColumnName)) || (!isColumnExists(dt, valueColumnName)))
            {
                return false;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][parameterColumnName].ToString() == parameterName)
                {
                    dt.Rows[i][valueColumnName] = newValue;
                    return true;
                }
            }
            return false;
        }
    }
}
