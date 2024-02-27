using System;
using Microsoft.Office.Interop.Excel;

namespace taskt.Core.Automation.Commands
{
    public static class EM_ExcelColumnSpecifiedPropertiesExtensionMethods
    {
        /// <summary>
        /// expand value or variable as Excel Column Index
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static int ExpandValueOrVariableAsExcelColumnIndex(this IExcelColumnSpecifiedProperties command, Engine.AutomationEngineInstance engine)
        {
            (_, var sheet) = command.ExpandValueOrVariableAsExcelInstanceAndCurrentWorksheet(engine);

            string columnIndexValue = ((ScriptCommand)command).GetRawPropertyValueAsString(nameof(command.v_ColumnIndex), "Column Index");
            int columnIndex = 0;

            switch (((ScriptCommand)command).ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_ColumnType), "Column Type", engine))
            {
                case "range":
                    if (string.IsNullOrEmpty(columnIndexValue))
                    {
                        columnIndexValue = "A";
                    }
                    columnIndex = sheet.ToColumnIndex(columnIndexValue);
                    break;
                case "rc":
                    if (string.IsNullOrEmpty(columnIndexValue))
                    {
                        columnIndexValue = "1";
                    }
                    columnIndex = columnIndexValue.ExpandValueOrUserVariableAsInteger("Column Index", engine);
                    break;
            }

            if (!sheet.RCLocationTryParse(1, columnIndex, out _))
            {
                throw new Exception($"Invalid Column Index. Type: '{command.v_ColumnType}', Value: '{command.v_ColumnIndex}', Expand Value: '{columnIndex}'");
            }

            return columnIndex;
        }

        /// <summary>
        /// expand value or variable as Excel current worksheet and column index
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static (Worksheet, int) ExpandValueOrVariableAsExcelCurrentWorksheetAndColumnIndex(this IExcelColumnSpecifiedProperties command, Engine.AutomationEngineInstance engine)
        {
            (_, var sheet) = command.ExpandValueOrVariableAsExcelInstanceAndCurrentWorksheet(engine);
            var index = command.ExpandValueOrVariableAsExcelColumnIndex(engine);

            return (sheet, index);
        }
    }
}
