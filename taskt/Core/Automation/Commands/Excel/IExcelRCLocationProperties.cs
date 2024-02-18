namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// cell RC row, column properties
    /// </summary>
    public interface IExcelRCLocationProperties : ILExcelInstanceProperties
    {
        /// <summary>
        /// cell row index
        /// </summary>
        string v_CellRow { get; set; }

        /// <summary>
        /// cell column index
        /// </summary>
        string v_CellColumn { get; set; }
    }
}
