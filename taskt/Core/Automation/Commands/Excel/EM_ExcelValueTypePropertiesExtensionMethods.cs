using System;
using Microsoft.Office.Interop.Excel;

namespace taskt.Core.Automation.Commands
{
    public static class EM_ExcelValueTypePropertiesExtensionMethods
    {
        /// <summary>
        /// expand value or variable as Check Range Value Function
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static Func<Range, bool> ExpandValueOrVariableAsCheckRangeFunction(this IExcelValueTypeProperties command, Engine.AutomationEngineInstance engine)
        {
            Func<Range, bool> func = null;
            switch (((ScriptCommand)command).ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_ValueType), "Value Type", engine))
            {
                case "cell":
                    func = (rg) => 
                    {
                        return !string.IsNullOrEmpty((string)rg.Text); 
                    };
                    break;

                case "formula":
                    func = (rg) => 
                    {
                        return ((string)rg.Formula).StartsWith("="); 
                    };
                    break;

                case "back color":
                    func = (rg) => 
                    { 
                        return ((long)rg.Interior.Color) != 16777215; 
                    };
                    break;
            }
            return func;
        }

        /// <summary>
        /// expand value or variable as Get Range Value Function
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static Func<Range, string> ExpandValueOrVariableAsGetRangeFunction(this IExcelValueTypeProperties command, Engine.AutomationEngineInstance engine)
        {
            Func<Range, string> getFunc = null;
            switch (((ScriptCommand)command).ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_ValueType), "Value Type", engine))
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

        /// <summary>
        /// expand value or variable as Set Range Value Action
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Action<Range, string> ExpandValueOrVariableAsSetRangeAction(this IExcelValueTypeProperties command, Engine.AutomationEngineInstance engine)
        {
            Func<string, long> longConvert = (str) =>
            {
                if (long.TryParse(str, out long v))
                {
                    return v;
                }
                else
                {
                    throw new Exception($"Value '{str}' is not Color Value.");
                }
            };

            Action<Range, string> setFunc = null;
            switch (((ScriptCommand)command).ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_ValueType), "Value Type", engine))
            {
                case "cell":
                    setFunc = (rg, value) =>
                    {
                        rg.Value = value;
                    };
                    break;
                case "formula":
                    setFunc = (rg, value) =>
                    {
                        rg.Formula = value;
                    };
                    break;
                case "format":
                    setFunc = (rg, value) =>
                    {
                        rg.NumberFormatLocal = value;
                    };
                    break;
                case "fore color":
                    setFunc = (rg, value) =>
                    {
                        rg.Font.Color = longConvert(value);
                    };
                    break;
                case "back color":
                    setFunc = (rg, value) =>
                    {
                        rg.Interior.Color = longConvert(value);
                    };
                    break;
            }

            return setFunc;
        }

        /// <summary>
        /// expand value or variable as get cell value Function
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static Func<Worksheet, int, int, string> ExpandValueOrVariableAsGetCellValueFunction(this IExcelValueTypeProperties command, Engine.AutomationEngineInstance engine)
        {
            Func<Worksheet, int, int, string> getFunc = null;
            switch (((ScriptCommand)command).ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_ValueType), "Value Type", engine))
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

        /// <summary>
        /// expand value or variable as set cell value Action
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static Action<string, Worksheet, int, int> ExpandValueOrVaribleAsSetCellValueAction(this IExcelValueTypeProperties command, Engine.AutomationEngineInstance engine)
        {
            Action<string, Worksheet, int, int> setFunc = null;
            switch (((ScriptCommand)command).ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_ValueType), "Value Type", engine))
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
    }
}
