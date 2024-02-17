using System;
using Microsoft.Office.Interop.Excel;

namespace taskt.Core.Automation.Commands
{
    public static class EM_ExcelWorksheetExtensionMethods
    {
        /// <summary>
        /// check Correct Cell Location
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="range"></param>
        /// <param name="rg"></param>
        /// <returns></returns>
        public static bool CellLocationTryParse(this Worksheet sheet, string range, out Range rg)
        {
            try
            {
                rg = sheet.Range[range];
                return true;
            }
            catch
            {
                rg = null;
                return false;
            }
        }

        /// <summary>
        /// try parse RC Location
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="rg"></param>
        /// <returns></returns>
        public static bool RCLocationTryParse(this Worksheet sheet, int row, int column, out Range rg)
        {
            try
            {
                rg = sheet.Cells[row, column];
                return true;
            }
            catch
            {
                rg = null;
                return false;
            }
        }

        /// <summary>
        /// try parse RC row
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="r"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public static bool RowTryParse(this Worksheet sheet, int r, out int row)
        {
            if (sheet.RCLocationTryParse(r, 1, out _))
            {
                row = r;
                return true;
            }
            else
            {
                row = 0;
                return false;
            }
        }

        /// <summary>
        /// try parse RC column (index)
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="c"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static bool ColumnIndexTryParse(this Worksheet sheet, int c, out int column)
        {
            if (sheet.RCLocationTryParse(1, c, out _))
            {
                column = c;
                return true;
            }
            else
            {
                column = 0;
                return false;
            }
        }

        /// <summary>
        /// try parse coulmn name
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="c"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static bool ColumnNameTryParse(this Worksheet sheet, string c, out string column)
        {
            if (sheet.CellLocationTryParse($"{c}1", out _))
            {
                column = c;
                return true;
            }
            else
            {
                column = "";
                return false;
            }
        }

        /// <summary>
        /// convert column Name to column Index
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static int ToColumnIndex(this Worksheet sheet, string columnName)
        {
            //if (CheckCorrectColumnName(columnName, sheet))
            //{
            //    return ((Range)sheet.Columns[columnName]).Column;
            //}
            //else
            //{
            //    throw new Exception("Strange Column Name '" + columnName + "'");
            //}
            try
            {
                return ((Range)sheet.Columns[columnName]).Column;
            }
            catch
            {
                throw new Exception($"Strange Column Name '{columnName}'.");
            }
        }

        /// <summary>
        /// convert column Index to column Name
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public static string ToColumnName(this Worksheet sheet, int columnIndex)
        {
            //if (columnIndex < 1)
            //{
            //    return "";
            //}
            //else
            //{
            //    return ((Range)sheet.Cells[1, columnIndex]).Address.Split('$')[1];
            //}
            try
            {
                return ((Range)sheet.Cells[1, columnIndex]).Address.Split('$')[1];
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// convert RC Location to Cell Location
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string ToCellLocation(Worksheet sheet, int row, int column)
        {
            //if (CheckCorrectRC(row, column, sheet))
            //{
            //    return ((Range)sheet.Cells[row, column]).Address.Replace("$", "");
            //}
            //else
            //{
            //    throw new Exception("Strange Excel Location. Row: " + row + ", Column: " + column);
            //}
            try
            {
                return ((Range)sheet.Cells[row, column]).Address.Replace("$", "");
            }
            catch
            {
                throw new Exception($"Strange Excel RC Location. Row: '{row}', Column: '{column}'");
            }
        }

        /// <summary>
        /// get last index specified by column index
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="column">Column Index</param>
        /// <param name="startRow"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static int LastIndex(this Worksheet sheet, int column, int startRow, string targetType)
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
                    while ((string)(((Range)sheet.Cells[lastRow, column]).Text) != "")
                    {
                        lastRow++;
                    }
                    break;
            }
            return --lastRow;
        }
    }
}
