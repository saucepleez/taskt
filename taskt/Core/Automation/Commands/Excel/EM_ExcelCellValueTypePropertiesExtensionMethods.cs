using System;
using Microsoft.Office.Interop.Excel;

namespace taskt.Core.Automation.Commands
{
    public static class EM_ExcelCellValueTypePropertiesExtensionMethods
    {
        /// <summary>
        /// expand value or variable as Check Range Value Function
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static Func<Range, bool> ExpandValueOrVariableAsCheckRangeFunction(this IExcelCellValueTypeProperties command, Engine.AutomationEngineInstance engine)
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
        public static Func<Range, string> ExpandValueOrVariableAsGetRangeFunction(this IExcelCellValueTypeProperties command, Engine.AutomationEngineInstance engine)
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
        public static Action<Range, string> ExpandValueOrVariableAsSetRangeAction(this IExcelCellValueTypeProperties command, Engine.AutomationEngineInstance engine)
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
    }
}
