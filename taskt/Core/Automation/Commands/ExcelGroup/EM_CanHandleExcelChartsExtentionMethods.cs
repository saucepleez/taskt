using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;

namespace taskt.Core.Automation.Commands.ExcelGroup
{
    public static class EM_CanHandleExcelChartsExtentionMethods
    {
        /// <summary>
        /// get charts from workhseet
        /// </summary>
        /// <param name="command"></param>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public static List<ChartObject> GetChartsFromWorksheet(this ICanHandleExcelCharts command, Worksheet sheet)
        {
            var charts = (ChartObjects)sheet.ChartObjects();

            var ret = new List<ChartObject>();
            foreach (ChartObject chart in charts)
            {
                ret.Add(chart);
            }
            return ret;
        }

        /// <summary>
        /// get chart from chart name
        /// </summary>
        /// <param name="command"></param>
        /// <param name="sheet"></param>
        /// <param name="chartName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static ChartObject GetChartFromName(this ICanHandleExcelCharts command, Worksheet sheet, string chartName)
        {
            var charts = GetChartsFromWorksheet(command, sheet);

            foreach (var chart in charts)
            {
                if (chart.Name == chartName)
                {
                    return chart;
                }
            }

            throw new Exception($"Chart Not Found. Chart Name: '{chartName}', Sheet Name: '{sheet.Name}'");
        }

        /// <summary>
        /// get chart from index
        /// </summary>
        /// <param name="command"></param>
        /// <param name="sheet"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static ChartObject GetChartFromIndex(this ICanHandleExcelCharts command, Worksheet sheet, int index)
        {
            var charts = GetChartsFromWorksheet(command, sheet);

            if (index < 0)
            {
                index += charts.Count;
            }
            if (index >= 0 && index < charts.Count)
            {
                return charts[index];
            }
            else
            {
                throw new Exception($"Chart Not Found. Chart Index: '{index}', Sheet Name: '{sheet.Name}'");
            }
        }
    }
}
