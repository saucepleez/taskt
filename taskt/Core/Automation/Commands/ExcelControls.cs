using System;
using Microsoft.Office.Interop.Excel;
using taskt.Core.Automation.Commands;

namespace taskt.Core
{
    internal static class ExcelControls
    {
        //public static Application getExcelInstance(Automation.Engine.AutomationEngineInstance engine, string instanceName)
        //{
        //    var excelObject = engine.GetAppInstance(instanceName);

        //    return (Application)excelObject;
        //}

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

        //public static Worksheet getWorksheet(Automation.Engine.AutomationEngineInstance engine, Application excelInstance, string sheetName)
        //{
        //    if (sheetName == engine.engineSettings.CurrentWorksheetKeyword)
        //    {
        //        return (Worksheet)excelInstance.ActiveSheet;
        //    }
        //    else if (sheetName == engine.engineSettings.NextWorksheetKeyword)
        //    {
        //        return getNextWorksheet(excelInstance);
        //    }
        //    else if (sheetName == engine.engineSettings.PreviousWorksheetKeyword)
        //    {
        //        return getPreviousWorksheet(excelInstance);
        //    }
        //    else
        //    {
        //        try
        //        {
        //            return (Worksheet)excelInstance.Worksheets[sheetName];
        //        }
        //        catch
        //        {
        //            return null;
        //        }
        //    }
        //}

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
            return ((Range)sheet.Cells[row, column]).Address.Replace("$", "");
        }

        public static int getLastRowIndex(Worksheet sheet, string column, int startRow, string targetType)
        {
            return getLastRowIndex(sheet, GetColumnIndex(sheet, column), startRow, targetType);
        }

        public static int getLastRowIndex(Worksheet sheet, int column, int startRow, string targetType)
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

        public static int getLastColumnIndex(Worksheet sheet, int row, string startColum, string targetType)
        {
            return getLastColumnIndex(sheet, row, GetColumnIndex(sheet, startColum), targetType);
        }

        public static int getLastColumnIndex(Worksheet sheet, int row, int startColum, string targetType)
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

        public static string ConvertToUserVariableAsExcelRangeLocation(this string value, Automation.Engine.AutomationEngineInstance engine, Application excelInstance)
        {
            var location = value.ConvertToUserVariable(engine);
            //try
            //{
            //    var rg = excelInstance.Range[location]; // location validate test
            //    return location;
            //}
            //catch
            //{
            //    throw new Exception("Location '" + value + "' is not Range. Value: '" + location + "'.");
            //}
            if (CheckCorrectRange(location, excelInstance))
            {
                return location;
            }
            else
            {
                throw new Exception("Location '" + value + "' is not Range. Value: '" + location + "'.");
            }
        }

        public static (int row, int column) ConvertToUserVariableAsExcelRCLocation(this ((string rowValue, string rowName) row, (string columnValue, string columnName) column) location, Automation.Engine.AutomationEngineInstance engine, Application excelInstance, ScriptCommand command)
        {
            int row = location.row.rowValue.ConvertToUserVariableAsInteger(location.row.rowName, "Row", engine, command);
            int column = location.column.columnValue.ConvertToUserVariableAsInteger(location.column.columnName, "Column", engine, command);

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
            string pos = location.ConvertToUserVariableAsExcelRangeLocation(engine, excelInstance);
            return excelSheet.Range[pos];
        }

        public static Range GetExcelRange(this ((string rowName, string rowValue) row, (string columnName, string columnValue) column) location, Automation.Engine.AutomationEngineInstance engine, Application excelInstance, Worksheet excelSheet, ScriptCommand command)
        {
            var rc = location.ConvertToUserVariableAsExcelRCLocation(engine, excelInstance, command);
            return excelSheet.Cells[rc.row, rc.column];
        }

        public static bool CheckCorrectColumnName(string columnName, Application excelInstance)
        {
            try
            {
                var col = excelInstance.Columns[columnName];
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool CheckCorrectColumnName(string columnName, Worksheet excelSheet)
        {
            try
            {
                var col = excelSheet.Columns[columnName];
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool CheckCorrectColumnIndex(int columnIndex, Application excelInstance)
        {
            return CheckCorrectRC(1, columnIndex, excelInstance);
        }
        public static bool CheckCorrectColumnIndex(int columnIndex, Worksheet excelInstance)
        {
            try
            {
                var rg = excelInstance.Cells[1, columnIndex];
                return true;
            }
            catch
            {
                return false;
            }
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
    }
}
