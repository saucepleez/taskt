namespace taskt.Core.Automation.Commands.ExcelGroup
{
    /// <summary>
    /// chart name properties
    /// </summary>
    public interface IExcelChartNameProperties : IExcelInstanceProperties, ICanHandleExcelCharts
    {
        /// <summary>
        /// chart
        /// </summary>
        string v_ChartName { get; set; }
    }
}
