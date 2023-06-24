using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for DataTable methods
    /// </summary>
    internal static class DataTableControls
    {
        /// <summary>
        /// input DataTable property
        /// </summary>
        [PropertyDescription("DataTable Variable Name")]
        [InputSpecification("DataTable Variable Name", true)]
        [PropertyDetailSampleUsage("**vDataTable**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vDataTable}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("DataTable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "DataTable")]
        public static string v_InputDataTableName { get; }

        /// <summary>
        /// output DataTable property
        /// </summary>
        [PropertyDescription("DataTable Variable Name")]
        [InputSpecification("DataTable Variable Name", true)]
        [PropertyDetailSampleUsage("**vDataTable**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vDataTable}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        [PropertyValidationRule("DataTable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "DataTable")]
        public static string v_OutputDataTableName { get; }

        /// <summary>
        /// New output DataTable property
        /// </summary>
        [PropertyDescription("New DataTable Variable Name")]
        [InputSpecification("New DataTable Variable Name", true)]
        [PropertyDetailSampleUsage("**vNewDataTable**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vNewDataTable}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        [PropertyValidationRule("New DataTable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "New DataTable")]
        public static string v_NewOutputDataTableName { get; }

        /// <summary>
        /// input & output DataTable parameter
        /// </summary>
        [PropertyDescription("DataTable Variable Name")]
        [InputSpecification("DataTable Variable Name", true)]
        [PropertyDetailSampleUsage("**vDataTable**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vDataTable}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Both)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        [PropertyValidationRule("DataTable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "DataTable")]
        public static string v_BothDataTableName { get; }

        /// <summary>
        /// when column does not exists
        /// </summary>
        [PropertyDescription("When DataTable Column does not Exists")]
        [InputSpecification("", true)]
        [PropertyDetailSampleUsage("**Ignore**", "Do not add a Column")]
        [PropertyDetailSampleUsage("**Error**", "Rise a Error")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Error")]
        [PropertyIsOptional(true, "Ignore")]
        public static string v_WhenColumnNotExists { get; }

        /// <summary>
        /// column type
        /// </summary>
        [PropertyDescription("Column type")]
        [InputSpecification("", true)]
        [PropertyDetailSampleUsage("**Column Name**", "Specify the Column Name like **Name**")]
        [PropertyDetailSampleUsage("**Index**", "Specify the Column Index like **0** or **1**")]
        [Remarks("")]
        [PropertyUISelectionOption("Column Name")]
        [PropertyUISelectionOption("Index")]
        [PropertyIsOptional(true, "Column Name")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyDisplayText(true, "Column Type")]
        public static string v_ColumnType { get; }

        /// <summary>
        /// column name or index
        /// </summary>
        [PropertyDescription("Name or Index of the Column")]
        [InputSpecification("", true)]
        [PropertyDetailSampleUsage("**id**", PropertyDetailSampleUsage.ValueType.Value, "Column Name")]
        [PropertyDetailSampleUsage("**0**", PropertyDetailSampleUsage.ValueType.Value, "Column Index")]
        [PropertyDetailSampleUsage("**-1**", "Specify Last Column Index")]
        [PropertyDetailSampleUsage("**{{{vColumn}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Column Name or Index")]
        [Remarks("If **-1** is specified for Column Index, it means the last column.")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Column", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Column")]
        public static string v_ColumnNameIndex { get; }

        /// <summary>
        /// row index
        /// </summary>
        [PropertyDescription("Index of the Row")]
        [InputSpecification("Index of the Row")]
        [PropertyDetailSampleUsage("**0**", "Specify First Row Index")]
        [PropertyDetailSampleUsage("**-1**", "Specify Last Row Index")]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Row Index")]
        [PropertyDetailSampleUsage("**{{{vRowIndex}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Row Index")]
        [Remarks("**-1** means index of the last row.")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyIsOptional(true, "Current Row")]
        [PropertyDisplayText(true, "Row")]
        public static string v_RowIndex { get; }

        /// <summary>
        /// for set column values parameter
        /// </summary>
        [PropertyDescription("When there are Less Rows than *** to set")]
        [InputSpecification("", true)]
        [PropertyDetailSampleUsage("**Ignore**", "Do not Add New Rows")]
        [PropertyDetailSampleUsage("**Add Rows**", "Add New Rows")]
        [PropertyDetailSampleUsage("**Error**", "Rise a Error")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Add Rows")]
        [PropertyUISelectionOption("Error")]
        [PropertyIsOptional(true, "Ignore")]
        public static string v_WhenLessRows { get; }

        /// <summary>
        /// for set column values parameter
        /// </summary>
        [PropertyDescription("When there are Less Rows than DataTable to be Setted")]
        [InputSpecification("", true)]
        [PropertyDetailSampleUsage("**Ignore**", "Do not Set Value")]
        [PropertyDetailSampleUsage("**Error**", "Rise a Error")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Error")]
        [PropertyIsOptional(true, "Ignore")]
        public static string v_WhenGreaterRows { set; get; }

        /// <summary>
        /// get DataTable variable from variable name
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static DataTable GetDataTableVariable(this string variableName, Engine.AutomationEngineInstance engine)
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
        /// get DataTable variable from parameter name
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static DataTable ConvertToUserVariableAsDataTable(this ScriptCommand command, string parameterName, Engine.AutomationEngineInstance engine)
        {
            var prop = command.GetProperty(parameterName);
            var value = prop?.GetValue(command) ?? null;
            if (value is DataTable tbl)
            {
                return tbl;
            }
            else
            {
                throw new Exception("Property '" + parameterName + "' is not DataTable");
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
        public static (DataTable table, int columnIndex) GetDataTableVariableAndColumnIndex(this ScriptCommand command, string tableName, string columnTypeName, string columnName, Engine.AutomationEngineInstance engine)
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
        public static (DataTable table, int rowIndex) GetDataTableVariableAndRowIndex(this ScriptCommand command, string tableName, string rowName, Engine.AutomationEngineInstance engine)
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
        public static (DataTable table, int rowIndex, int columnIndex) GetDataTableVariableAndRowColumnIndeies(this ScriptCommand command, string tableName, string rowName, string columnTypeName, string columnName, Engine.AutomationEngineInstance engine)
        {
            (var table, var rowIndex) = command.GetDataTableVariableAndRowIndex(tableName, rowName, engine);
            (_, var columnIndex) = command.GetDataTableVariableAndColumnIndex(tableName, columnTypeName, columnName, engine);

            return (table, rowIndex, columnIndex);
        }


        public static void StoreInUserVariable(this DataTable value, Engine.AutomationEngineInstance sender, string targetVariable)
        {
            ExtensionMethods.StoreInUserVariable(targetVariable, value, sender, false);
        }


        public static DataTable CreateDataTable(string connection, string query)
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

        /// <summary>
        /// check column name exists
        /// </summary>
        /// <param name="table"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        private static bool IsColumnExists(DataTable table, string columnName)
        {
            return table.Columns.Contains(columnName);
        }

        /// <summary>
        /// check column index exists
        /// </summary>
        /// <param name="table"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
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

        /// <summary>
        /// get DataTable column name from value specified by argument
        /// </summary>
        /// <param name="table"></param>
        /// <param name="columnName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static string GetColumnName(DataTable table, string columnName, Engine.AutomationEngineInstance engine)
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

        /// <summary>
        /// get column index from value specified by argument
        /// </summary>
        /// <param name="table"></param>
        /// <param name="columnIndex"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static int GetColumnIndex(DataTable table, string columnIndex, Engine.AutomationEngineInstance engine)
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

        /// <summary>
        /// get column name or Index from value specified by arguments
        /// </summary>
        /// <param name="command"></param>
        /// <param name="table"></param>
        /// <param name="columnTypeName"></param>
        /// <param name="columnName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        private static int GetColumnIndex(this ScriptCommand command, DataTable table, string columnTypeName, string columnName, Engine.AutomationEngineInstance engine)
        {
            string columnType = command.GetUISelectionValue(columnTypeName, "Column Type", engine);

            int columnIndex = 0;
            switch (columnType)
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

        /// <summary>
        /// get column index from name
        /// </summary>
        /// <param name="table"></param>
        /// <param name="columnName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static int GetColumnIndexFromName(DataTable table, string columnName, Engine.AutomationEngineInstance engine)
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

        /// <summary>
        /// get DataTable Value, specify ParameterName, ParameterValue column name
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="parameterName"></param>
        /// <param name="parameterColumnName"></param>
        /// <param name="valueColumnName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string GetFieldValue(DataTable dt, string parameterName, string parameterColumnName = "ParameterName", string valueColumnName = "ParameterValue")
        {
            if ((!IsColumnExists(dt, parameterColumnName)) || (!IsColumnExists(dt, valueColumnName)))
            {
                throw new Exception("Parameter Column or Value Column does not exists");
            }

            foreach (DataRow row in dt.Rows)
            {
                if ((row.Field<string>(parameterColumnName) ?? "") == parameterName)
                {
                    return row.Field<string>(valueColumnName) ?? "";
                }
            }

            return "";
        }

        /// <summary>
        /// get DataTable Value, specify RowIndex, ParameterValue column name
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="rowIndex"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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

            return dt.Rows[rowIndex].Field<string>(columnName) ?? "";
        }

        /// <summary>
        /// get DataTable Values as Dictionary
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="parameterColumnName"></param>
        /// <param name="valueColumnName"></param>
        /// <param name="engine">if not null, expand variables in ValueColumn values</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Dictionary<string, string> GetFieldValues(DataTable dt, string parameterColumnName = "ParameterName", string valueColumnName = "ParameterValue", Engine.AutomationEngineInstance engine = null)
        {
            if ((!IsColumnExists(dt, parameterColumnName)) || (!IsColumnExists(dt, valueColumnName)))
            {
                throw new Exception("Parameter Column or Value Column does not exists");
            }

            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (DataRow row in dt.Rows)
            {
                var key = row.Field<string>(parameterColumnName) ?? "";
                if (key != "")
                {
                    dic.Add(key, row.Field<string>(valueColumnName) ?? "");
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

        /// <summary>
        /// get DataTable Values as Dictionary
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="parameterColumnName"></param>
        /// <param name="valueColumnName"></param>
        /// <param name="expandVaiable"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Dictionary<string, string> GetFieldValues(DataTable dt, string parameterColumnName = "ParameterName", string valueColumnName = "ParameterValue", bool expandVaiable = false, Engine.AutomationEngineInstance engine = null)
        {
            if ((!IsColumnExists(dt, parameterColumnName)) || (!IsColumnExists(dt, valueColumnName)))
            {
                throw new Exception("Parameter Column or Value Column does not exists");
            }

            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (DataRow row in dt.Rows)
            {
                var key = row.Field<string>(parameterColumnName) ?? "";
                if (key != "")
                {
                    dic.Add(key, row.Field<string>(valueColumnName) ?? "");
                }
            }

            if (expandVaiable)
            {
                if (engine == null)
                {
                    throw new Exception("No Engine Specified. Fail expand variable.");
                }
                var keys = dic.Keys.ToArray();
                foreach (string key in keys)
                {
                    dic[key] = dic[key].ConvertToUserVariable(engine);
                }
            }
            
            return dic;
        }

        /// <summary>
        /// set DataTable value specified by column names
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="newValue"></param>
        /// <param name="parameterName"></param>
        /// <param name="parameterColumnName"></param>
        /// <param name="valueColumnName"></param>
        /// <returns></returns>
        public static bool SetParameterValue(DataTable dt, string newValue, string parameterName, string parameterColumnName = "ParameterName", string valueColumnName = "ParameterValue")
        {
            if ((!IsColumnExists(dt, parameterColumnName)) || (!IsColumnExists(dt, valueColumnName)))
            {
                return false;
            }

            foreach (DataRow row in dt.Rows)
            {
                var key = row.Field<string>(parameterColumnName) ?? "";
                if (key == parameterName)
                {
                    row.SetField(valueColumnName, newValue);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// check parameter names exists
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterNameColumn"></param>
        /// <returns></returns>
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
                foreach (DataRow row in dt.Rows)
                {
                    var key = row.Field<string>(parameterNameColumn) ?? "";
                    if (!parameterNameColumn.Contains(key))
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        #region Event handlers
        public static void AllEditableDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var myDGV = (DataGridView)sender;
            if (e.RowIndex < 0)
            {
                return;
            }

            if (e.ColumnIndex >= 0)
            {
                var targetCell = myDGV.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (targetCell is DataGridViewTextBoxCell)
                {
                    myDGV.BeginEdit(false);
                }
                else if (targetCell is DataGridViewComboBoxCell)
                {
                    SendKeys.Send("{F4}");
                }
            }
            else
            {
                myDGV.EndEdit();
            }
        }

        public static void FirstColumnReadonlySubsequentEditableDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var myDGV = (DataGridView)sender;

            if (e.RowIndex < 0)
            {
                return;
            }

            if (e.ColumnIndex >= 0)
            {
                if (e.ColumnIndex >= 1)
                {
                    var targetCell = myDGV.Rows[e.RowIndex].Cells[1];
                    if (targetCell is DataGridViewTextBoxCell)
                    {
                        myDGV.BeginEdit(false);
                    }
                    else if (targetCell is DataGridViewComboBoxCell)
                    {
                        SendKeys.Send("{F4}");
                    }
                }
            }
            else
            {
                myDGV.EndEdit();
            }
        }
        public static void FirstColumnReadonlySubsequentEditableDataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                e.Cancel = true;
            }
        }
        #endregion

        #region Validate
        public static void BeforeValidate(DataGridView dgv, DataTable table)
        {
            if (dgv.IsCurrentCellDirty || dgv.IsCurrentRowDirty)
            {
                dgv.CommitEdit(DataGridViewDataErrorContexts.Commit);
                var newRow = table.NewRow();
                table.Rows.Add(newRow);
                table.AcceptChanges();
                int cols = table.Columns.Count;
                for (int i = table.Rows.Count - 1; i >= 0; i--)
                {
                    bool allEmpty = true;
                    for (int j = 0; j < cols; j++)
                    {
                        if ((table.Rows[i][j]?.ToString() ?? "") != "")
                        {
                            allEmpty = false;
                            break;
                        }
                    }
                    if (allEmpty)
                    {
                        table.Rows[i].Delete();
                    }
                }
                table.AcceptChanges();
            }
        }
        #endregion
    }
}
