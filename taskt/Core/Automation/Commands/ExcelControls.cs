using System;
using System.Collections.Generic;
using Microsoft.Office.Interop.Excel;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for excel methods
    /// </summary>
    internal static class ExcelControls
    {
        /// <summary>
        /// excel instance property
        /// </summary>
        [PropertyDescription("Excel Instance Name")]
        [InputSpecification("Excel Instance Name", true)]
        [PropertyDetailSampleUsage("**RPAExcel**", PropertyDetailSampleUsage.ValueType.Value, "Excel Instance Name")]
        [PropertyDetailSampleUsage("**{{{vInstance}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Excel Instance Name")]
        [Remarks("Please specify the Excel Instance Name created by **Create Excel Instance** command in advance.")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Excel)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyFirstValue("%kwd_default_excel_instance%")]
        [PropertyValidationRule("Instance", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Instance")]
        public static string v_InputInstanceName { get; }

        /// <summary>
        /// sheet name property
        /// </summary>
        [PropertyDescription("Sheet Name")]
        [InputSpecification("Sheet Name", true)]
        [PropertyDetailSampleUsage("**mySheet**", PropertyDetailSampleUsage.ValueType.Value, "Sheet Name")]
        [PropertyDetailSampleUsage("**{{{vSheet}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Sheet Name")]
        [PropertyDetailSampleUsage("**%kwd_current_worksheet%**", "Specify Current Sheet Name")]
        [Remarks("")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Sheet", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Sheet")]
        [PropertyIntermediateConvert(nameof(ApplicationSettings.EngineSettings.convertToIntermediateExcelSheet), nameof(ApplicationSettings.EngineSettings.convertToRawExcelSheet))]
        public static string v_SheetName { get; }

        /// <summary>
        /// cell range location
        /// </summary>
        [PropertyDescription("Cell Location")]
        [InputSpecification("Cell Location", true)]
        [PropertyDetailSampleUsage("**A1**", PropertyDetailSampleUsage.ValueType.Value)]
        [PropertyDetailSampleUsage("**B10**", PropertyDetailSampleUsage.ValueType.Value)]
        [PropertyDetailSampleUsage("**{{{vAddress}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Cell Location")]
        [Remarks("")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Cell Location", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Cell")]
        public static string v_CellRangeLocation { get; }

        /// <summary>
        /// row location property
        /// </summary>
        [PropertyDescription("Row Location")]
        [InputSpecification("Row Location", true)]
        [PropertyDetailSampleUsage("**1**", "Specify the First Row")]
        [PropertyDetailSampleUsage("**2**", PropertyDetailSampleUsage.ValueType.Value, "Row Location")]
        [PropertyDetailSampleUsage("**{{{vRow}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Row Location")]
        [Remarks("")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Row", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.EqualsZero | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(true, "Row")]
        public static string v_RowLocation { get; }

        /// <summary>
        /// column location property
        /// </summary>
        [PropertyDescription("Column Location")]
        [InputSpecification("Column Location", true)]
        [PropertyDetailSampleUsage("**1**", "Specify the First Column")]
        [PropertyDetailSampleUsage("**2**", PropertyDetailSampleUsage.ValueType.Value, "Column Location")]
        [PropertyDetailSampleUsage("**{{{vColumn}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Column Location")]
        [Remarks("")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Column", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.EqualsZero | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(true, "Column")]
        public static string v_ColumnLocation { get; }

        /// <summary>
        /// cell value type property
        /// </summary>
        [PropertyDescription("Value Type")]
        [InputSpecification("", true)]
        [Remarks("")]
        [PropertyDetailSampleUsage("**Cell**", "Specify the Cell Value")]
        [PropertyDetailSampleUsage("**Formula**", "Specify the Cell Formula, like **=SUM(A1:A10)**")]
        [PropertyDetailSampleUsage("**Format**", "Specify the Cell Format")]
        [PropertyDetailSampleUsage("**Font Color**", "Specify the Cell Text Color")]
        [PropertyDetailSampleUsage("**Back Color**", "Specify the Cell Background Color")]
        [PropertyUISelectionOption("Cell")]
        [PropertyUISelectionOption("Formula")]
        [PropertyUISelectionOption("Format")]
        [PropertyUISelectionOption("Font Color")]
        [PropertyUISelectionOption("Back Color")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsOptional(true, "Cell")]
        [PropertyDisplayText(true, "Type")]
        public static string v_ValueType { get; }

        /// <summary>
        /// cell value type property for check cell commands
        /// </summary>
        [PropertyDescription("Value Type")]
        [InputSpecification("", true)]
        [Remarks("")]
        [PropertyUISelectionOption("Cell")]
        [PropertyUISelectionOption("Formula")]
        [PropertyUISelectionOption("Back Color")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsOptional(true, "Cell")]
        [PropertySecondaryLabel(true)]
        [PropertyAddtionalParameterInfo("Cell", "Check the Cell has Value or Not")]
        [PropertyAddtionalParameterInfo("Formula", "Check the Cell has Formula or Not")]
        [PropertyAddtionalParameterInfo("Back Color", "Check the Cell Background Color is Not White")]
        [PropertyDisplayText(true, "Type")]
        public static string v_CheckableValueType { get; }

        /// <summary>
        /// column type propertys
        /// </summary>
        [PropertyDescription("Column Type")]
        [InputSpecification("")]
        [Remarks("")]
        [PropertyDetailSampleUsage("**Range**", "Use Range, like **A**. It means first column.")]
        [PropertyDetailSampleUsage("**RC**", "Use Row-Column, like **1**. It means first column.")]
        [PropertyIsOptional(true, "Range")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Range")]
        [PropertyUISelectionOption("RC")]
        [PropertySelectionValueSensitive(false)]
        [PropertyDisplayText(true, "Column Type")]
        public static string v_ColumnType { get; }

        /// <summary>
        /// column location or index
        /// </summary>
        [PropertyDescription("Column Location or Index")]
        [InputSpecification("Column Location or Index", true)]
        [Remarks("")]
        [PropertyDetailSampleUsage("**A**", "Specify the First Column when **Range** is specified for Column Type.")]
        [PropertyDetailSampleUsage("**1**", "Specify the First Column when **RC** is specified for Column Type.")]
        [PropertyDetailSampleUsage("**{{{vColumn}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Column")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Column", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero | PropertyValidationRule.ValidationRuleFlags.EqualsZero)]
        [PropertyDisplayText(true, "Column")]
        public static string v_ColumnNameOrIndex { get; }

        /// <summary>
        /// start row
        /// </summary>
        [PropertyDescription("Start Row Index")]
        [InputSpecification("Start Row", true)]
        [PropertyDetailSampleUsage("**1**", "Specify the First Row Index for Start Row")]
        [PropertyDetailSampleUsage("**2**", PropertyDetailSampleUsage.ValueType.Value, "Start Row")]
        [PropertyDetailSampleUsage("**{{{vRow}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Start Row")]
        [Remarks("")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true, "1")]
        [PropertyDisplayText(true, "Start Row")]
        public static string v_RowStart { get; }

        /// <summary>
        /// end row
        /// </summary>
        [PropertyDescription("End Row Index")]
        [InputSpecification("End Row", true)]
        [PropertyDetailSampleUsage("**1**", "Specify the First Row Index for Start Row")]
        [PropertyDetailSampleUsage("**2**", PropertyDetailSampleUsage.ValueType.Value, "Start Row")]
        [PropertyDetailSampleUsage("**{{{vRow}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Start Row")]
        [Remarks("When End Row Index is Empty, Automatically specifies the Last Row where values are entered consecutively")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyIsOptional(true, "Last Row")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDisplayText(true, "End Row")]
        public static string v_RowEnd { get; }

        /// <summary>
        /// column start property
        /// </summary>
        [PropertyDescription("Start Column Location or Index")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Column Location or Index", true)]
        [PropertyDetailSampleUsage("**A**", "Specify the First Column when **Range** is specified for Column Type.")]
        [PropertyDetailSampleUsage("**1**", "Specify the First Column when **RC** is specified for Column Type.")]
        [PropertyDetailSampleUsage("**{{{vColumn}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Column")]
        [Remarks("")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Start Column", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero | PropertyValidationRule.ValidationRuleFlags.EqualsZero)]
        [PropertyDisplayText(true, "Start Column")]
        public static string v_ColumnStart { get; }

        /// <summary>
        /// column end property
        /// </summary>
        [PropertyDescription("End Column Location or Index")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Column Location or Index", true)]
        [PropertyDetailSampleUsage("**A**", "Specify the First Column when **Range** is specified for Column Type.")]
        [PropertyDetailSampleUsage("**1**", "Specify the First Column when **RC** is specified for Column Type.")]
        [PropertyDetailSampleUsage("**{{{vColumn}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Column")]
        [Remarks("")]
        [PropertyIsOptional(true, "Last Column")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDisplayText(true, "End Column")]
        public static string v_ColumnEnd { get; }

        /// <summary>
        /// when items not enough
        /// </summary>
        [PropertyDescription("When A Items Not Enough")]
        [InputSpecification("", true)]
        [PropertyDetailSampleUsage("**Ignore**", "Don't Set the Value")]
        [PropertyDetailSampleUsage("**Error**", "Rise a Error")]
        [Remarks("")]
        [PropertyIsOptional(true, "Ignore")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Error")]
        public static string v_WhenItemNotEnough { get; }

        /// <summary>
        /// excel file path
        /// </summary>
        [PropertyDescription("Workbook (Excel File) Path")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [InputSpecification("Excel File Path", true)]
        [PropertyDetailSampleUsage("**C:\\temp\\myfile.xlsx**", PropertyDetailSampleUsage.ValueType.Value, "File Path")]
        [PropertyDetailSampleUsage("**{{{vFilePath}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "File Path")]
        [Remarks("If file does not contain extension, supplement extensions supported by Excel.\nIf file does not contain folder path, file will be opened in the same folder as script file.")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("File", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "File")]
        public static string v_FilePath { get; }


        #region instance, worksheet methods
        public static Application GetExcelInstance(this string instanceName, Automation.Engine.AutomationEngineInstance engine)
        {
            string ins = instanceName.ConvertToUserVariable(engine);
            var instanceObject = engine.GetAppInstance(ins);
            if (instanceObject is Application)
            {
                return (Application)instanceObject;
            }
            else
            {
                throw new Exception("Instance '" + instanceName + "' is not Excel Instance");
            }
        }

        public static (Application instance, Worksheet sheet) GetExcelInstanceAndWorksheet(this string instanceName, Automation.Engine.AutomationEngineInstance engine)
        {
            var instanceObject = instanceName.GetExcelInstance(engine);
            return (instanceObject, GetCurrentWorksheet(instanceObject));
        }

        public static (Application instance, Worksheet sheet) GetExcelInstanceAndWorksheet(this (string instanceName, string sheetName) info, Automation.Engine.AutomationEngineInstance engine, bool returnNullIfSheetDoesNotExists = false)
        {
            var instanceObject = info.instanceName.GetExcelInstance(engine);
            var sheet = info.sheetName.GetExcelWorksheet(engine, instanceObject, returnNullIfSheetDoesNotExists);
            return (instanceObject, sheet);
        }

        public static Worksheet GetExcelWorksheet(this string sheetVariable, Automation.Engine.AutomationEngineInstance engine, Application excelInstance, bool returnNullIfSheetDoesNotExists = false)
        {
            var sheet = sheetVariable.ConvertToUserVariable(engine);
            if (sheet == engine.engineSettings.CurrentWorksheetKeyword)
            {
                try
                {
                    //return (Worksheet)excelInstance.ActiveSheet;
                    return GetCurrentWorksheet(excelInstance);
                }
                catch
                {
                    if (returnNullIfSheetDoesNotExists)
                    {
                        return null;
                    }
                    else
                    {
                        throw new Exception("No Worksheet exists.");
                    }
                }
            }
            else if (sheet == engine.engineSettings.NextWorksheetKeyword)
            {
                try
                {
                    return GetNextWorksheet(excelInstance);
                }
                catch
                {
                    if (returnNullIfSheetDoesNotExists)
                    {
                        return null;
                    }
                    else
                    {
                        throw new Exception("Next Worksheet not found.");
                    }
                }
            }
            else if (sheet == engine.engineSettings.PreviousWorksheetKeyword)
            {
                try
                {
                    return GetPreviousWorksheet(excelInstance);
                }
                catch
                {
                    if (returnNullIfSheetDoesNotExists)
                    {
                        return null;
                    }
                    else
                    {
                        throw new Exception("Previous Worksheet not found.");
                    }
                }
            }
            else
            {
                try
                {
                    return (Worksheet)excelInstance.Worksheets[sheet];
                }
                catch
                {
                    if (returnNullIfSheetDoesNotExists)
                    {
                        return null;
                    }
                    else
                    {
                        throw new Exception("Worksheet " + sheet + " does not exists.");
                    }
                }
            }
        }

        private static Worksheet GetCurrentWorksheet(Application excelInstance)
        {
            if (excelInstance.Sheets.Count == 0)
            {
                return null;
            }
            else
            {
                return excelInstance.ActiveSheet;
            }
        }

        private static Worksheet GetNextWorksheet(Application excelInstance, Worksheet mySheet = null)
        {
            Worksheet currentSheet;
            if (mySheet == null)
            {
                try
                {
                    //currentSheet = (Worksheet)excelInstance.ActiveSheet;
                    currentSheet = GetCurrentWorksheet(excelInstance);
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                currentSheet = mySheet;
            }

            int idx = 1;
            foreach (Worksheet sht in excelInstance.Worksheets)
            {
                if (sht.Name == currentSheet.Name)
                {
                    break;
                }
                idx++;
            }
            if (idx < excelInstance.Worksheets.Count)
            {
                return (Worksheet)excelInstance.Worksheets[idx + 1];
            }
            else
            {
                return null;
            }
        }
        private static Worksheet GetPreviousWorksheet(Application excelInstance, Worksheet mySheet = null)
        {
            Worksheet currentSheet;
            if (mySheet == null)
            {
                try
                {
                    //currentSheet = (Worksheet)excelInstance.ActiveSheet;
                    currentSheet = GetCurrentWorksheet(excelInstance);
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                currentSheet = mySheet;
            }

            int idx = 1;
            foreach (Worksheet sht in excelInstance.Worksheets)
            {
                if (sht.Name == currentSheet.Name)
                {
                    break;
                }
                idx++;
            }
            if (idx > 1)
            {
                return (Worksheet)excelInstance.Worksheets[idx - 1];
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Func methods
        public static Func<Range, bool> CheckCellValueFunctionFromRange(string valueType)
        {
            Func<Range, bool> func = null;
            switch (valueType)
            {
                case "cell":
                    func = (rg) => { return !String.IsNullOrEmpty((string)rg.Text); };
                    break;

                case "formula":
                    func = (rg) => { return ((string)rg.Formula).StartsWith("="); };
                    break;

                case "back color":
                    func = (rg) => { return ((long)rg.Interior.Color) != 16777215; };
                    break;
            }
            return func;
        }

        public static Func<Range, string> GetCellValueFunctionFromRange(string valueType)
        {
            Func<Range, string> getFunc = null;
            switch (valueType)
            {
                case "cell":
                    getFunc = (rg) =>
                    {
                        return (string)rg.Text;
                    };
                    break;
                case "formula":
                    getFunc = (rg) =>
                    {
                        return (string)rg.Formula;
                    };
                    break;
                case "format":
                    getFunc = (rg) =>
                    {
                        return (string)rg.NumberFormatLocal;
                    };
                    break;
                case "fore color":
                    getFunc = (rg) =>
                    {
                        return ((long)rg.Font.Color).ToString();
                    };
                    break;
                case "back color":
                    getFunc = (rg) =>
                    {
                        return ((long)rg.Interior.Color).ToString();
                    };
                    break;
            }
            return getFunc;
        }

        public static Func<Worksheet, int, int, string> GetCellValueFunction(string valueType)
        {
            Func<Worksheet, int, int, string> getFunc = null;
            switch (valueType)
            {
                case "cell":
                    getFunc = (sheet, column, row) =>
                    {
                        return (string)((Range)sheet.Cells[row, column]).Text;
                    };
                    break;
                case "formula":
                    getFunc = (sheet, column, row) =>
                    {
                        return (string)((Range)sheet.Cells[row, column]).Formula;
                    };
                    break;
                case "format":
                    getFunc = (sheet, column, row) =>
                    {
                        return (string)((Range)sheet.Cells[row, column]).NumberFormatLocal;
                    };
                    break;
                case "fore color":
                    getFunc = (sheet, column, row) =>
                    {
                        return ((long)((Range)sheet.Cells[row, column]).Font.Color).ToString();
                    };
                    break;
                case "back color":
                    getFunc = (sheet, column, row) =>
                    {
                        return ((long)((Range)sheet.Cells[row, column]).Interior.Color).ToString();
                    };
                    break;
            }
            return getFunc;
        }

        public static Action<string, Worksheet, Range> SetCellValueFunctionFromRange(string valueType)
        {
            Action<string, Worksheet, Range> setFunc = null;

            Func<string, long> longConvert = (str) =>
            {
                if (long.TryParse(str, out long v))
                {
                    return v;
                }
                else
                {
                    throw new Exception("Value '" + str + "' is not color.");
                }
            };

            switch (valueType)
            {
                case "cell":
                    setFunc = (value, sheet, rg) =>
                    {
                        rg.Value = value;
                    };
                    break;
                case "formula":
                    setFunc = (value, sheet, rg) =>
                    {
                        rg.Formula = value;
                    };
                    break;
                case "format":
                    setFunc = (value, sheet, rg) =>
                    {
                        rg.NumberFormatLocal = value;
                    };
                    break;
                case "fore color":
                    setFunc = (value, sheet, rg) =>
                    {
                        rg.Font.Color = longConvert(value);
                    };
                    break;
                case "back color":
                    setFunc = (value, sheet, rg) =>
                    {
                        rg.Interior.Color = longConvert(value);
                    };
                    break;
            }

            return setFunc;
        }

        public static Action<string, Worksheet, int, int> SetCellValueFunction(string valueType)
        {
            Action<string, Worksheet, int, int> setFunc = null;
            switch (valueType)
            {
                case "cell":
                    setFunc = (value, sheet, column, row) =>
                    {
                        ((Range)sheet.Cells[row, column]).Value = value;
                    };
                    break;
                case "formula":
                    setFunc = (value, sheet, column, row) =>
                    {
                        ((Range)sheet.Cells[row, column]).Formula = value;
                    };
                    break;
                case "format":
                    setFunc = (value, sheet, column, row) =>
                    {
                        ((Range)sheet.Cells[row, column]).NumberFormatLocal = value;
                    };
                    break;
                case "fore color":
                    setFunc = (value, sheet, column, row) =>
                    {
                        ((Range)sheet.Cells[row, column]).Font.Color = long.Parse(value);
                    };
                    break;
                case "back color":
                    setFunc = (value, sheet, column, row) =>
                    {
                        ((Range)sheet.Cells[row, column]).Interior.Color = long.Parse(value);
                    };
                    break;
            }

            return setFunc;
        }
        #endregion

        public static int GetColumnIndex(Worksheet sheet, string columnName)
        {
            if (CheckCorrectColumnName(columnName, sheet))
            {
                return ((Range)sheet.Columns[columnName]).Column;
            }
            else
            {
                throw new Exception("Strange Column Name '" + columnName + "'");
            }
        }

        public static string GetColumnName(Worksheet sheet, int columnIndex)
        {
            if (columnIndex < 1)
            {
                return "";
            }
            else
            {
                return ((Range)sheet.Cells[1, columnIndex]).Address.Split('$')[1];
            }
        }

        public static string GetAddress(Worksheet sheet, int row, int column)
        {
            if (CheckCorrectRC(row, column, sheet))
            {
                return ((Range)sheet.Cells[row, column]).Address.Replace("$", "");
            }
            else
            {
                throw new Exception("Strange Excel Location. Row: " + row + ", Column: " + column);
            }
        }

        public static int GetLastRowIndex(Worksheet sheet, string column, int startRow, string targetType)
        {
            return GetLastRowIndex(sheet, GetColumnIndex(sheet, column), startRow, targetType);
        }

        public static int GetLastRowIndex(Worksheet sheet, int column, int startRow, string targetType)
        {
            int lastRow = startRow;
            switch (targetType.ToLower())
            {
                case "formula":
                    while ((string)(((Range)sheet.Cells[lastRow, column]).Formula) != "")
                    {
                        lastRow++;
                    }
                    break;

                default:
                    while((string)(((Range)sheet.Cells[lastRow, column]).Text) != "")
                    {
                        lastRow++;
                    }
                    break;
            }
            return --lastRow;
        }

        public static int GetLastColumnIndex(Worksheet sheet, int row, string startColum, string targetType)
        {
            return GetLastColumnIndex(sheet, row, GetColumnIndex(sheet, startColum), targetType);
        }

        public static int GetLastColumnIndex(Worksheet sheet, int row, int startColum, string targetType)
        {
            int lastColumn = startColum;
            switch (targetType.ToLower())
            {
                case "formula":
                    while ((string)(((Range)sheet.Cells[row, lastColumn]).Formula) != "")
                    {
                        lastColumn++;
                    }
                    break;

                default:
                    while ((string)(((Range)sheet.Cells[row, lastColumn]).Text) != "")
                    {
                        lastColumn++;
                    }
                    break;
            }
            return --lastColumn;
        }

        public static (int columnIndex, int rowStartIndex, int rowEndIndex, string valueType) GetRangeIndeiesColumnDirection(string columnValueName, string columnTypeName, string rowStartName, string rowEndName, string valueTypeName, Automation.Engine.AutomationEngineInstance engine, Worksheet excelSheet, ScriptCommand command, object targetObject = null)
        {
            string columnType = command.GetUISelectionValue(columnTypeName, "Column Type", engine);

            int columnIndex = 0;
            switch (columnType)
            {
                case "range":
                    string col = command.ConvertToUserVariable(columnValueName, "Column", engine);
                    columnIndex = GetColumnIndex(excelSheet, col);
                    break;
                case "rc":
                    columnIndex = command.ConvertToUserVariableAsInteger(columnValueName, "Column", engine);
                    break;
            }

            string valueType = command.GetUISelectionValue(valueTypeName, "Value Type", engine);

            int rowStartIndex = command.ConvertToUserVariableAsInteger(rowStartName, "Start Row", engine);
            string rowEndValue = command.GetRawPropertyString(rowEndName, "End Row");
            int rowEndIndex;
            if (String.IsNullOrEmpty(rowEndValue))
            {
                if (targetObject == null)
                {
                    rowEndIndex = GetLastRowIndex(excelSheet, columnIndex, rowStartIndex, valueType);
                }
                else
                {
                    int size;
                    if (targetObject is List<string>)
                    {
                        size = ((List<string>)targetObject).Count;
                    }
                    else if (targetObject is Dictionary<string, string>)
                    {
                        size = ((Dictionary<string, string>)targetObject).Count;
                    }
                    else if (targetObject is System.Data.DataTable)
                    {
                        size = ((System.Data.DataTable)targetObject).Rows.Count;
                    }
                    else if (targetObject is int)
                    {
                        size = (int)targetObject;
                    }
                    else
                    {
                        throw new Exception("target object is strange data.");
                    }
                    rowEndIndex = rowStartIndex + size - 1;
                }
            }
            else
            {
                rowEndIndex = rowEndValue.ConvertToUserVariableAsInteger("End Row", engine);
            }

            if (rowStartIndex > rowEndIndex)
            {
                int t = rowStartIndex;
                rowStartIndex = rowEndIndex;
                rowEndIndex = t;
            }

            CheckCorrectRCRange(rowStartIndex, columnIndex, rowEndIndex, columnIndex, excelSheet);

            return (columnIndex, rowStartIndex, rowEndIndex, valueType);
        }

        public static (int rowIndex, int columnStartIndex, int columnEndIndex, string valueType) GetRangeIndeiesRowDirection(string rowValueName, string columnTypeName, string columnStartName, string columnEndName, string valueTypeName, Automation.Engine.AutomationEngineInstance engine, Worksheet excelSheet, ScriptCommand command, object targetObject = null)
        {
            int rowIndex = command.ConvertToUserVariableAsInteger(rowValueName, "Row Index", engine);

            string valueType = command.GetUISelectionValue(valueTypeName, "Value Type", engine);

            int columnStartIndex = 0;
            int columnEndIndex = 0;

            string columnStartValue = command.GetRawPropertyString(columnStartName, "Start Column");
            string columnEndValue = command.GetRawPropertyString(columnEndName, "End Column");

            Func<int> getLastRowFromObject = () =>
            {
                int size;
                if (targetObject is List<string>)
                {
                    size = ((List<string>)targetObject).Count;
                }
                else if (targetObject is Dictionary<string, string>)
                {
                    size = ((Dictionary<string, string>)targetObject).Count;
                }
                else if (targetObject is System.Data.DataTable)
                {
                    size = ((System.Data.DataTable)targetObject).Columns.Count;
                }
                else if (targetObject is int)
                {
                    size = (int)targetObject;
                }
                else
                {
                    throw new Exception("target object is strange data.");
                }
                return columnStartIndex + size - 1;
            };

            string columnType = command.GetUISelectionValue(columnTypeName, "Column Type", engine);
            switch (columnType)
            {
                case "range":
                    if (String.IsNullOrEmpty(columnStartValue))
                    {
                        columnStartValue = "A";
                    }
                    columnStartIndex = ExcelControls.GetColumnIndex(excelSheet, columnStartValue.ConvertToUserVariable(engine));

                    
                    if (String.IsNullOrEmpty(columnEndValue))
                    {
                        if (targetObject == null)
                        {
                            columnEndIndex = ExcelControls.GetLastColumnIndex(excelSheet, rowIndex, columnStartIndex, valueType);
                        }
                        else
                        {
                            columnEndIndex = getLastRowFromObject();
                        }
                    }
                    else
                    {
                        columnEndIndex = ExcelControls.GetColumnIndex(excelSheet, columnEndValue.ConvertToUserVariable(engine));
                        
                    }
                    break;

                case "rc":
                    if (String.IsNullOrEmpty(columnStartValue))
                    {
                        columnStartValue = "1";
                    }
                    columnStartIndex = columnStartValue.ConvertToUserVariableAsInteger("Start Column", engine);

                    if (String.IsNullOrEmpty(columnEndValue))
                    {
                        if (targetObject == null)
                        {
                            columnEndIndex = ExcelControls.GetLastColumnIndex(excelSheet, rowIndex, columnStartIndex, valueType);
                        }
                        else
                        {
                            columnEndIndex = getLastRowFromObject();
                        }
                    }
                    else
                    {
                        columnEndIndex = columnEndValue.ConvertToUserVariableAsInteger("Column End", engine);
                    }

                    break;
            }

            if (columnStartIndex > columnEndIndex)
            {
                int t = columnStartIndex;
                columnStartIndex = columnEndIndex;
                columnEndIndex = t;
            }

            CheckCorrectRCRange(rowIndex, columnStartIndex, rowIndex, columnEndIndex, excelSheet);

            return (rowIndex, columnStartIndex, columnEndIndex, valueType);
        }

        #region convert methods
        public static string GetoExcelRangeLocation(this string value, Automation.Engine.AutomationEngineInstance engine, Application excelInstance)
        {
            var location = value.ConvertToUserVariable(engine);
            if (CheckCorrectRange(location, excelInstance))
            {
                return location;
            }
            else
            {
                throw new Exception("Location '" + value + "' is not Range. Value: '" + location + "'.");
            }
        }

        public static (int row, int column) GetExcelRCLocation(this ScriptCommand command, string rowPropertyName, string columnPropertyName, Automation.Engine.AutomationEngineInstance engine, Application excelInstance)
        {
            int row = command.ConvertToUserVariableAsInteger(rowPropertyName, "Row", engine);
            int column = command.ConvertToUserVariableAsInteger(columnPropertyName, "Column", engine);
            if (CheckCorrectRC(row, column, excelInstance))
            {
                return (row, column);
            }
            else
            {
                throw new Exception("Invalid Location. Row: " + row + ", Column: " + column);
            }
        }

        public static Range GetExcelRange(this string location, Automation.Engine.AutomationEngineInstance engine, Application excelInstance, Worksheet excelSheet, ScriptCommand command)
        {
            string pos = location.GetoExcelRangeLocation(engine, excelInstance);
            return excelSheet.Range[pos];
        }

        public static Range GetExcelRange(this ScriptCommand command, string rowPropertyName, string columnPropertyName, Automation.Engine.AutomationEngineInstance engine, Application excelInstance, Worksheet excelSheet)
        {
            var rc = command.GetExcelRCLocation(rowPropertyName, columnPropertyName, engine, excelInstance);
            return excelSheet.Cells[rc.row, rc.column];
        }
        #endregion

        #region check methods
        public static bool CheckCorrectColumnName(string columnName, Worksheet excelSheet)
        {
            return CheckCorrectRange(columnName + "1", excelSheet);
        }

        public static bool CheckCorrectColumnIndex(int columnIndex, Worksheet excelSheet)
        {
            return CheckCorrectRC(1, columnIndex, excelSheet);
        }

        public static bool CheckCorrectRange(string range, Worksheet excelSheet)
        {
            try
            {
                var rg = excelSheet.Range[range];
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool CheckCorrectRC(int row, int column, Worksheet excelSheet)
        {
            try
            {
                var rc = excelSheet.Cells[row, column];
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool CheckCorrectColumnName(string columnName, Application excelInstance)
        {
            return CheckCorrectRange(columnName + "1", excelInstance);
        }

        public static bool CheckCorrectRowIndex(int rowIndex, Application excelInstance)
        {
            return CheckCorrectRC(rowIndex, 1, excelInstance);
        }

        public static bool CheckCorrectColumnIndex(int columnIndex, Application excelInstance)
        {
            return CheckCorrectRC(1, columnIndex, excelInstance);
        }

        public static bool CheckCorrectRange(this string range, Application excelInstance)
        {
            try
            {
                var rg = excelInstance.Range[range];
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool CheckCorrectRC(int row, int column, Application excelInstance)
        {
            try
            {
                var rc = excelInstance.Cells[row, column];
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool CheckCorrectRCRange(int startRow, int startColumn, int endRow, int endColumn, Application excelInstance, bool throwExceptionWhenInvalidRange = true)
        {
            if (!CheckCorrectRC(startRow, startColumn, excelInstance))
            {
                if (throwExceptionWhenInvalidRange)
                {
                    throw new Exception("Invalid Start Location. Row: " + startRow + ", Column: " + startColumn);
                }
                else
                {
                    return false;
                }
            }
            if (!CheckCorrectRC(endRow, endColumn, excelInstance))
            {
                if (throwExceptionWhenInvalidRange)
                {
                    throw new Exception("Invalid End Location. Row: " + endRow + ", Column: " + endColumn);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        public static bool CheckCorrectRCRange(int startRow, int startColumn, int endRow, int endColumn, Worksheet excelSheet, bool throwExceptionWhenInvalidRange = true)
        {
            if (!CheckCorrectRC(startRow, startColumn, excelSheet))
            {
                if (throwExceptionWhenInvalidRange)
                {
                    throw new Exception("Invalid Start Location. Row: " + startRow + ", Column: " + startColumn);
                }
                else
                {
                    return false;
                }
            }
            if (!CheckCorrectRC(endRow, endColumn, excelSheet))
            {
                if (throwExceptionWhenInvalidRange)
                {
                    throw new Exception("Invalid End Location. Row: " + endRow + ", Column: " + endColumn);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        #endregion
    }
}
