using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskt.UI.CustomControls
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
    }
}
