using System;
using Microsoft.Office.Interop.Excel;

namespace taskt.Core
{
    internal static class ExcelControls
    {
        public static Application getExcelInstance(Automation.Engine.AutomationEngineInstance engine, string instanceName)
        {
            var excelObject = engine.GetAppInstance(instanceName);

            return (Application)excelObject;
        }

        public static Microsoft.Office.Interop.Excel.Application getExcelInstance(this string instanceName, Automation.Engine.AutomationEngineInstance engine)
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

        public static Worksheet getWorksheet(Automation.Engine.AutomationEngineInstance engine, Application excelInstance, string sheetName)
        {
            if (sheetName == engine.engineSettings.CurrentWorksheetKeyword)
            {
                return (Worksheet)excelInstance.ActiveSheet;
            }
            else if (sheetName == engine.engineSettings.NextWorksheetKeyword)
            {
                return getNextWorksheet(excelInstance);
            }
            else if (sheetName == engine.engineSettings.PreviousWorksheetKeyword)
            {
                return getPreviousWorksheet(excelInstance);
            }
            else
            {
                try
                {
                    return (Worksheet)excelInstance.Worksheets[sheetName];
                }
                catch
                {
                    return null;
                }
            }
        }

        public static Worksheet getCurrentWorksheet(Application excelInstance)
        {
            return (Worksheet)excelInstance.ActiveSheet;
        }

        public static Worksheet getNextWorksheet(Application excelInstance, Worksheet mySheet = null)
        {
            Worksheet currentSheet;
            if (mySheet == null)
            {
                currentSheet = (Worksheet)excelInstance.ActiveSheet;
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
        public static Worksheet getPreviousWorksheet(Application excelInstance, Worksheet mySheet = null)
        {
            Worksheet currentSheet;
            if (mySheet == null)
            {
                currentSheet = (Worksheet)excelInstance.ActiveSheet;
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

        public static Func<Worksheet, int, int, string> getCellValueFunction(string valueType)
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

        public static Action<string, Worksheet, int, int> setCellValueFunction(string valueType)
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
        
        public static int getColumnIndex(Worksheet sheet, string columnName)
        {
            return ((Range)sheet.Columns[columnName]).Column;
        }

        public static string getColumnName(Worksheet sheet, int columnIndex)
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

        public static string getAddress(Worksheet sheet, int row, int column)
        {
            return ((Range)sheet.Cells[row, column]).Address.Replace("$", "");
        }

        public static int getLastRowIndex(Worksheet sheet, string column, int startRow, string targetType)
        {
            return getLastRowIndex(sheet, getColumnIndex(sheet, column), startRow, targetType);
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
            return getLastColumnIndex(sheet, row, getColumnIndex(sheet, startColum), targetType);
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
    }
}
