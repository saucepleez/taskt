using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using taskt.Core.Automation.Commands;

namespace taskt.Core.Automation.Commands
{
    internal static class DataTableControls
    {
        /// <summary>
        /// get DataTable variable from variable name
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static DataTable GetDataTableVariable(this string variableName, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            Script.ScriptVariable v = variableName.GetRawVariable(engine);
            if (v.VariableValue is DataTable table)
            {
                return table;
            }
            else
            {
                throw new Exception("Variable " + variableName + " is not DataTable");
            }
        }

        /// <summary>
        /// get DataTable variable and Column Index from variable name property and column properies
        /// </summary>
        /// <param name="command"></param>
        /// <param name="tableName"></param>
        /// <param name="columnTypeName"></param>
        /// <param name="columnName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static (DataTable table, int columnIndex) GetDataTableVariableAndColumnIndex(this ScriptCommand command, string tableName, string columnTypeName, string columnName, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            var targetTable = command.ConvertToUserVariable(tableName, "DataTable", engine);
            var table = targetTable.GetDataTableVariable(engine);
            var index = command.GetColumnIndex(table, columnTypeName, columnName, engine);
            return (table, index);
        }

        /// <summary>
        /// get DataTable variable and Row Index from variable name property and row name property. If row index is empty, return value is current position.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="tableName"></param>
        /// <param name="rowName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static (DataTable table, int rowIndex) GetDataTableVariableAndRowIndex(this ScriptCommand command, string tableName, string rowName, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            var targetTable = command.ConvertToUserVariable(tableName, "DataTable", engine);
            var table = targetTable.GetDataTableVariable(engine);

            var rowValue = command.ConvertToUserVariable(rowName, "Row Index", engine);
            int index;
            if (String.IsNullOrEmpty(rowValue))
            {
                index = targetTable.GetRawVariable(engine).CurrentPosition;
            }
            else
            {
                index = command.ConvertToUserVariableAsInteger(rowName, "Row Index", engine);
            }
            if (index < 0)
            {
                index += table.Rows.Count;
            }

            if ((index < 0) || (index >= table.Rows.Count))
            {
                throw new Exception("Strange Row Index '" + rowName + "', parsed '" + index + "'");
            }

            return (table, index);
        }

        /// <summary>
        /// get DataTable variable Row Index, and Column Index from variable name property and row, column name properties. If row index is empty, return value is current position.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="tableName"></param>
        /// <param name="rowName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static (DataTable table, int rowIndex, int columnIndex) GetDataTableVariableAndRowColumnIndeies(this ScriptCommand command, string tableName, string rowName, string columnTypeName, string columnName, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            (var table, var rowIndex) = command.GetDataTableVariableAndRowIndex(tableName, rowName, engine);
            (_, var columnIndex) = command.GetDataTableVariableAndColumnIndex(tableName, columnTypeName, columnName, engine);

            return (table, rowIndex, columnIndex);
        }


        public static void StoreInUserVariable(this DataTable value, Core.Automation.Engine.AutomationEngineInstance sender, string targetVariable)
        {
            ExtensionMethods.StoreInUserVariable(targetVariable, value, sender, false);
        }


        public static System.Data.DataTable CreateDataTable(string connection, string query)
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

        private static bool IsColumnExists(DataTable table, string columnName)
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
        private static bool IsColumnExists(DataTable table, int columnIndex)
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

        private static string GetColumnName(DataTable table, string columnName, Automation.Engine.AutomationEngineInstance engine)
        {
            string col = columnName.ConvertToUserVariable(engine);
            if (IsColumnExists(table, col))
            {
                return col;
            }
            else
            {
                throw new Exception("Strange Column Name " + columnName);
            }
        }

        private static int GetColumnIndex(DataTable table, string columnIndex, Automation.Engine.AutomationEngineInstance engine)
        {
            int index = new PropertyConvertTag(columnIndex, "Column Index").ConvertToUserVariableAsInteger(engine);
            if (index < 0)
            {
                index = table.Columns.Count + index;
            }
            if (IsColumnExists(table, index))
            {
                return index;
            }
            else
            {
                throw new Exception("Strange Column Index " + columnIndex + ", parse " + index);
            }
        }

        private static int GetColumnIndex(this ScriptCommand command, DataTable table, string columnTypeName, string columnName, Automation.Engine.AutomationEngineInstance engine)
        {
            string columnType = command.GetUISelectionValue(columnTypeName, "Column Type", engine);

            int columnIndex = 0;
            switch(columnType)
            {
                case "column name":
                    string targetColumnName = command.ConvertToUserVariable(columnName, "Column Name", engine);
                    columnIndex = GetColumnIndexFromName(table, targetColumnName, engine);
                    break;

                case "index":
                    string targetColumnIndex = command.ConvertToUserVariable(columnName, "Column Index", engine);
                    columnIndex = GetColumnIndex(table, targetColumnIndex, engine);
                    break;
            }
            return columnIndex;
        }

        private static int GetColumnIndexFromName(DataTable table, string columnName, Automation.Engine.AutomationEngineInstance engine)
        {
            string col = GetColumnName(table, columnName, engine);
            for (int i = table.Columns.Count - 1; i >= 0; i--)
            {
                if (table.Columns[i].ColumnName == col)
                {
                    return i;
                }
            }
            throw new Exception("Strange Column Name " + columnName);
        }

        /// <summary>
        /// return DataTable with the column names of argument table copied
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static DataTable CloneDataTableOnlyColumnName(DataTable table)
        {
            DataTable ret = new DataTable();
            int cols = table.Columns.Count;
            for (int i = 0; i < cols; i++)
            {
                ret.Columns.Add(table.Columns[i].ColumnName);
            }

            return ret;
        }

        public static string GetFieldValue(DataTable dt, string parameterName, string parameterColumnName = "ParameterName", string valueColumnName = "ParameterValue")
        {
            if ((!IsColumnExists(dt, parameterColumnName)) || (!IsColumnExists(dt, valueColumnName)))
            {
                throw new Exception("Parameter Column or Value Column does not exists");
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][parameterColumnName].ToString() == parameterName)
                {
                    //return dt.Rows[i][valueColumnName] == null ? "" : dt.Rows[i][valueColumnName].ToString();
                    return dt.Rows[i][valueColumnName]?.ToString() ?? "";
                }
            }
            return "";
        }

        public static string GetFieldValue(DataTable dt, int rowIndex, string columnName = "ParameterValue")
        {
            if (!IsColumnExists(dt, columnName))
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

            //return dt.Rows[rowIndex][columnName] == null ? "" : dt.Rows[rowIndex][columnName].ToString();
            return dt.Rows[rowIndex][columnName]?.ToString() ?? "";
        }

        public static Dictionary<string, string> GetFieldValues(DataTable dt, string parameterColumnName = "ParameterName", string valueColumnName = "ParameterValue", Automation.Engine.AutomationEngineInstance engine = null)
        {
            if ((!IsColumnExists(dt, parameterColumnName)) || (!IsColumnExists(dt, valueColumnName)))
            {
                throw new Exception("Parameter Column or Value Column does not exists");
            }

            Dictionary<string, string> dic = new Dictionary<string, string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][parameterColumnName] != null)
                {
                    //string value = dt.Rows[i][valueColumnName] == null ? "" : dt.Rows[i][valueColumnName].ToString();
                    string value = dt.Rows[i][valueColumnName]?.ToString() ?? "";
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
            if ((!IsColumnExists(dt, parameterColumnName)) || (!IsColumnExists(dt, valueColumnName)))
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

        public static bool IsParameterNamesExists(DataTable dt, List<string> parameterNames, string parameterNameColumn = "ParameterName")
        {
            if (!IsColumnExists(dt, parameterNameColumn))
            {
                return false;
            }
            if (dt.Rows.Count != parameterNames.Count)
            {
                return false;
            }
            else
            {
                foreach(string n in parameterNames)
                { 
                    bool isExists = false;
                    for (int i = dt.Rows.Count - 1; i >= 0; i--)
                    {
                        if (dt.Rows[i][parameterNameColumn].ToString() == n)
                        {
                            isExists = true;
                            break;
                        }
                    }
                    if (!isExists)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
