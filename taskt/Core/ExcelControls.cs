using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskt.Core
{
    public class ExcelControls
    {
        public static Microsoft.Office.Interop.Excel.Application getExcelInstance(Core.Automation.Engine.AutomationEngineInstance engine, string instanceName)
        {
            var excelObject = engine.GetAppInstance(instanceName);

            return (Microsoft.Office.Interop.Excel.Application)excelObject;
        }

        public static Microsoft.Office.Interop.Excel.Worksheet getWorksheet(Core.Automation.Engine.AutomationEngineInstance engine, Microsoft.Office.Interop.Excel.Application excelInstance, string sheetName)
        {
            if (sheetName == engine.engineSettings.CurrentWorksheetKeyword)
            {
                return (Microsoft.Office.Interop.Excel.Worksheet)excelInstance.ActiveSheet;
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
                    return (Microsoft.Office.Interop.Excel.Worksheet)excelInstance.Worksheets[sheetName];
                }
                catch
                {
                    return null;
                }
            }
        }

        public static Microsoft.Office.Interop.Excel.Workbook getCurrentWorksheet(Microsoft.Office.Interop.Excel.Application excelInstance)
        {
            return (Microsoft.Office.Interop.Excel.Workbook)excelInstance.ActiveSheet;
        }

        public static Microsoft.Office.Interop.Excel.Worksheet getNextWorksheet(Microsoft.Office.Interop.Excel.Application excelInstance, Microsoft.Office.Interop.Excel.Worksheet mySheet = null)
        {
            Microsoft.Office.Interop.Excel.Worksheet currentSheet;
            if (mySheet == null)
            {
                currentSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelInstance.ActiveSheet;
            }
            else
            {
                currentSheet = mySheet;
            }

            int idx = 1;
            foreach (Microsoft.Office.Interop.Excel.Worksheet sht in excelInstance.Worksheets)
            {
                if (sht.Name == currentSheet.Name)
                {
                    break;
                }
                idx++;
            }
            if (idx < excelInstance.Worksheets.Count)
            {
                return (Microsoft.Office.Interop.Excel.Worksheet)excelInstance.Worksheets[idx + 1];
            }
            else
            {
                return null;
            }
        }
        public static Microsoft.Office.Interop.Excel.Worksheet getPreviousWorksheet(Microsoft.Office.Interop.Excel.Application excelInstance, Microsoft.Office.Interop.Excel.Worksheet mySheet = null)
        {
            Microsoft.Office.Interop.Excel.Worksheet currentSheet;
            if (mySheet == null)
            {
                currentSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelInstance.ActiveSheet;
            }
            else
            {
                currentSheet = mySheet;
            }

            int idx = 1;
            foreach (Microsoft.Office.Interop.Excel.Worksheet sht in excelInstance.Worksheets)
            {
                if (sht.Name == currentSheet.Name)
                {
                    break;
                }
                idx++;
            }
            if (idx > 1)
            {
                return (Microsoft.Office.Interop.Excel.Worksheet)excelInstance.Worksheets[idx - 1];
            }
            else
            {
                return null;
            }
        }

        public static Func<Microsoft.Office.Interop.Excel.Worksheet, int, int, string> getCellValueFunction(string valueType)
        {
            Func<Microsoft.Office.Interop.Excel.Worksheet, int, int, string> getFunc = null;
            switch (valueType)
            {
                case "cell":
                    getFunc = (sheet, column, row) =>
                    {
                        return (string)((Microsoft.Office.Interop.Excel.Range)sheet.Cells[row, column]).Text;
                    };
                    break;
                case "formula":
                    getFunc = (sheet, column, row) =>
                    {
                        return (string)((Microsoft.Office.Interop.Excel.Range)sheet.Cells[row, column]).Formula;
                    };
                    break;
                case "format":
                    getFunc = (sheet, column, row) =>
                    {
                        return (string)((Microsoft.Office.Interop.Excel.Range)sheet.Cells[row, column]).NumberFormatLocal;
                    };
                    break;
                case "fore color":
                    getFunc = (sheet, column, row) =>
                    {
                        return ((long)((Microsoft.Office.Interop.Excel.Range)sheet.Cells[row, column]).Font.Color).ToString();
                    };
                    break;
                case "back color":
                    getFunc = (sheet, column, row) =>
                    {
                        return ((long)((Microsoft.Office.Interop.Excel.Range)sheet.Cells[row, column]).Interior.Color).ToString();
                    };
                    break;
            }
            return getFunc;
        }

        public static Action<string, Microsoft.Office.Interop.Excel.Worksheet, int, int> setCellValueFunction(string valueType)
        {
            Action<string, Microsoft.Office.Interop.Excel.Worksheet, int, int> setFunc = null;
            switch (valueType)
            {
                case "cell":
                    setFunc = (value, sheet, column, row) =>
                    {
                        ((Microsoft.Office.Interop.Excel.Range)sheet.Cells[row, column]).Value = value;
                    };
                    break;
                case "formula":
                    setFunc = (value, sheet, column, row) =>
                    {
                        ((Microsoft.Office.Interop.Excel.Range)sheet.Cells[row, column]).Formula = value;
                    };
                    break;
                case "format":
                    setFunc = (value, sheet, column, row) =>
                    {
                        ((Microsoft.Office.Interop.Excel.Range)sheet.Cells[row, column]).NumberFormatLocal = value;
                    };
                    break;
                case "fore color":
                    setFunc = (value, sheet, column, row) =>
                    {
                        ((Microsoft.Office.Interop.Excel.Range)sheet.Cells[row, column]).Font.Color = long.Parse(value);
                    };
                    break;
                case "back color":
                    setFunc = (value, sheet, column, row) =>
                    {
                        ((Microsoft.Office.Interop.Excel.Range)sheet.Cells[row, column]).Interior.Color = long.Parse(value);
                    };
                    break;
            }

            return setFunc;
        }
        
        public static int getColumnIndex(Microsoft.Office.Interop.Excel.Worksheet sheet, string columnName)
        {
            return ((Microsoft.Office.Interop.Excel.Range)sheet.Columns[columnName]).Column;
        }

        public static string getAddress(Microsoft.Office.Interop.Excel.Worksheet sheet, int row, int column)
        {
            return ((Microsoft.Office.Interop.Excel.Range)sheet.Cells[row, column]).Address.Replace("$", "");
        }
    }
}
