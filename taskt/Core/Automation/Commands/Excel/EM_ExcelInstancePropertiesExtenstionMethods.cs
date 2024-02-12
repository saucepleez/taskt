using System;
using Microsoft.Office.Interop.Excel;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{
    public static class EM_ExcelInstancePropertiesExtenstionMethods
    {
        /// <summary>
        /// expand value or variable as Excel Instance
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Application ExpandValueOrVariableAsExcelInstance(this IExcelInstanceProperties command, AutomationEngineInstance engine)
        {
            string ins = command.v_InstanceName.ExpandValueOrUserVariable(engine);
            var instanceObject = engine.GetAppInstance(ins);
            if (instanceObject is Application app)
            {
                return app;
            }
            else
            {
                throw new Exception($"Instance '{command.v_InstanceName}' is not Excel Instance");
            }
        }

        /// <summary>
        /// expand value or variable as Excel Instance and CurrentWorksheet
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static (Application, Worksheet) ExpandValueOrVariableAsExcelInstanceAndCurrentWorksheet(this IExcelInstanceProperties command, AutomationEngineInstance engine)
        {
            var ins = command.ExpandValueOrVariableAsExcelInstance(engine);
            if (ins.Worksheets.Count > 0)
            {
                return (ins, ins.ActiveSheet);
            }
            else
            {
                throw new Exception($"Instance '{command.v_InstanceName} has no Worksheets");
            }
        }

        /// <summary>
        /// get next worksheet
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="currentSheet"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static Worksheet GetNextWorksheet(Application instance, Worksheet currentSheet)
        {
            int idx = 1;
            foreach (Worksheet sht in instance.Worksheets)
            {
                if (sht.Name == currentSheet.Name)
                {
                    break;
                }
                idx++;
            }
            if (idx < instance.Worksheets.Count)
            {
                return (Worksheet)instance.Worksheets[idx + 1];
            }
            else
            {
                throw new Exception($"No Next Worksheet. CurrentSheet: '{currentSheet.Name}'");
            }
        }

        /// <summary>
        /// get previous worksheet
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="currentSheet"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static Worksheet GetPreviousWorksheet(Application instance, Worksheet currentSheet)
        {
            int idx = 1;
            foreach (Worksheet sht in instance.Worksheets)
            {
                if (sht.Name == currentSheet.Name)
                {
                    break;
                }
                idx++;
            }
            if (idx > 1)
            {
                return (Worksheet)instance.Worksheets[idx - 1];
            }
            else
            {
                throw new Exception($"No Previous Worksheet. CurrentSheet: '{currentSheet.Name}'");
            }
        }

        /// <summary>
        /// expand value or variable as Excel Worksheet
        /// </summary>
        /// <param name="command"></param>
        /// <param name="sheetName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Worksheet ExpandValueOrVariableAsExcelWorksheet(this IExcelInstanceProperties command, string sheetName, AutomationEngineInstance engine)
        {
            (var ins, var currentSheet) = command.ExpandValueOrVariableAsExcelInstanceAndCurrentWorksheet(engine);

            if ((sheetName == VariableNameControls.GetWrappedVariableName(SystemVariables.Excel_CurrentWorkSheet.VariableName, engine)) ||
                (sheetName == engine.engineSettings.CurrentWorksheetKeyword))
            {
                return currentSheet;
            }
            else if ((sheetName == VariableNameControls.GetWrappedVariableName(SystemVariables.Excel_NextWorkSheet.VariableName, engine)) ||
                    (sheetName == engine.engineSettings.NextWorksheetKeyword))
            {
                return GetNextWorksheet(ins, currentSheet);
            }
            else if ((sheetName == VariableNameControls.GetWrappedVariableName(SystemVariables.Excel_PreviousWorkSheet.VariableName, engine)) ||
                    (sheetName == engine.engineSettings.PreviousWorksheetKeyword))
            {
                return GetPreviousWorksheet(ins, currentSheet);
            }
            else
            {
                var expandSheetName = sheetName.ExpandValueOrUserVariable(engine);
                try
                {
                    return (Worksheet)ins.Worksheets[expandSheetName];
                }
                catch
                {
                    throw new Exception($"Worksheet '{expandSheetName}' does not exists.");
                }
            }
        }

        /// <summary>
        /// Expand value or variable as Worksheet Name
        /// </summary>
        /// <param name="command">not used</param>
        /// <param name="sheet"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string ExpandValueOrVariableAsExcelWorksheetName(this IExcelInstanceProperties command, string sheet, AutomationEngineInstance engine)
        {
            var newSheet = sheet.ExpandValueOrUserVariable(engine);

            if (string.IsNullOrEmpty(newSheet))
            {
                throw new Exception("Worksheet name is Empty.");
            }
            else if (newSheet.Length >= 31)
            {
                throw new Exception($"Worksheet name is too long. Must be less than 31 characters.");
            }

            foreach(var s in ExcelControls.Disallow_Contains_Worksheet_Charactors)
            {
                if (newSheet.Contains(s))
                {
                    throw new Exception($"Worksheet name contains invalid character '{s}'. Worksheet: '{newSheet}'");
                }
            }
            foreach(var s in ExcelControls.Disallow_Starts_Ends_Worksheet_Charactors)
            {
                if (newSheet.StartsWith(s) || newSheet.EndsWith(s))
                {
                    throw new Exception($"Worksheet name starts or ends with a character that is invalid '{s}'. Worksheet: '{newSheet}'");
                }
            }

            return newSheet;
        }
    }
}
