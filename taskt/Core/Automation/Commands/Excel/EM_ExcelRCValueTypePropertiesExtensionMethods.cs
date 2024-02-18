using System;
using Microsoft.Office.Interop.Excel;

namespace taskt.Core.Automation.Commands
{
    public static class EM_ExcelRCValueTypePropertiesExtensionMethods
    {
        /// <summary>
        /// expand value or variable as get cell value Function(sheet, row, column)
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static Func<Worksheet, int, int, string> ExpandValueOrVariableAsGetValueFunction(this IExcelRCValueTypeProperties command, Engine.AutomationEngineInstance engine)
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
        /// expand value or variable as set cell value Action(value, sheet, row, column)
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static Action<string, Worksheet, int, int> ExpandValueOrVaribleAsSetValueAction(this IExcelRCValueTypeProperties command, Engine.AutomationEngineInstance engine)
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

        /// <summary>
        /// expand value or variable as Check Range Value Function(sheet, row, column)
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static Func<Worksheet, int, int, bool> ExpandValueOrVariableAsCheckValueFunction(this IExcelRCValueTypeProperties command, Engine.AutomationEngineInstance engine)
        {
            Func<Worksheet, int, int, bool> func = null;
            switch (((ScriptCommand)command).ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_ValueType), "Value Type", engine))
            {
                case "cell":
                    func = (sheet, row, column) =>
                    {
                        return !string.IsNullOrEmpty(((Range)sheet.Cells[row, column]).Text);
                    };
                    break;

                case "formula":
                    func = (sheet, row, column) =>
                    {
                        return ((Range)sheet.Cells[row, column]).Formula.StartsWith("=");
                    };
                    break;

                case "back color":
                    func = (sheet, row, column) =>
                    {
                        return ((Range)sheet.Cells[row, column]).Interior.Color != 16777215;
                    };
                    break;
            }
            return func;
        }
    }
}
