using Microsoft.Office.Interop.Excel;
using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.ExcelGroup
{
    public abstract class AExcelDoSomethingToChartByChartName : AExcelChartsCommands, IExcelChartNameProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ChartName))]
        [PropertyParameterOrder(7000)]
        public virtual string v_ChartName { get; set; }

        /// <summary>
        /// one chart action by name
        /// </summary>
        /// <param name="engine"></param>
        /// <param name="chartFunc"></param>
        /// <param name="errorFunc"></param>
        protected void ExcelChartAction(Engine.AutomationEngineInstance engine, Action<ChartObject> chartFunc, Action<Exception> errorFunc = null)
        {
            (_, var sht) = this.ExpandValueOrVariableAsExcelInstanceAndCurrentWorksheet(engine);

            var chartName = this.ExpandValueOrUserVariable(nameof(v_ChartName), "Chart Name", engine);

            try
            {
                var chart = this.GetChartFromName(sht, chartName);
                chartFunc(chart);
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
