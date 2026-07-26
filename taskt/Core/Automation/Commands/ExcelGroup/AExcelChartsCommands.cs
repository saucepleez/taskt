using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;

namespace taskt.Core.Automation.Commands.ExcelGroup
{
    /// <summary>
    /// do something to Excel Charts commands
    /// </summary>
    public abstract class AExcelChartsCommands : AExcelInstanceCommands, ICanHandleExcelCharts
    {
        /// <summary>
        /// excel charts action
        /// </summary>
        /// <param name="engine"></param>
        /// <param name="chartsFunc"></param>
        /// <param name="errorFunc"></param>
        protected void ExcelChartsAction(Engine.AutomationEngineInstance engine, Action<List<ChartObject>> chartsFunc, Action<Exception> errorFunc = null)
        {
            (_, var sht) = this.ExpandValueOrVariableAsExcelInstanceAndCurrentWorksheet(engine);
            var charts = this.GetChartsFromWorksheet(sht);
            try
            {
                chartsFunc(charts);
            }
            catch (Exception ex)
            {
                if (errorFunc != null)
                {
                    errorFunc(ex);
                }
                else
                {
                    throw ex;
                }
            }
        }
    }
}
