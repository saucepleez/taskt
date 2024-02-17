using Microsoft.Office.Interop.Excel;

namespace taskt.Core.Automation.Commands
{
    public static class EM_ExcelApplicationExtensionMethods
    {
        /// <summary>
        /// check Correct Cell Location
        /// </summary>
        /// <param name="excelInstance"></param>
        /// <param name="range"></param>
        /// <param name="rg"></param>
        /// <returns></returns>
        public static bool CellLocationTryParse(this Application excelInstance, string range, out Range rg)
        {
            try
            {
                rg = excelInstance.Range[range];
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
        /// <param name="excelInstance"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="rg"></param>
        /// <returns></returns>
        public static bool RCLocationTryParse(this Application excelInstance, int row, int column, out Range rg)
        {
            try
            {
                rg = excelInstance.Cells[row, column];
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
        /// <param name="excelInstance"></param>
        /// <param name="r"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public static bool RowTryParse(this Application excelInstance, int r, out int row) 
        {
            if (excelInstance.RCLocationTryParse(r, 1, out _))
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
        /// <param name="excelInstance"></param>
        /// <param name="c"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static bool ColumnIndexTryParse(this Application excelInstance, int c, out int column)
        {
            if (excelInstance.RCLocationTryParse(1, c, out _))
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
        /// <param name="excelInstance"></param>
        /// <param name="c"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static bool ColumnNameTryParse(this Application excelInstance, string c, out string column)
        {
            if (excelInstance.CellLocationTryParse($"{c}1", out _))
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
    }
}
