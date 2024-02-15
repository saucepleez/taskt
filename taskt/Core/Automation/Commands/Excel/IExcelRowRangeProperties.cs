namespace taskt.Core.Automation.Commands
{
    public interface IExcelRowRangeProperties : IExcelRCRangeProperties
    {
        /// <summary>
        /// row index
        /// </summary>
        string v_RowIndex { get; set; }

        /// <summary>
        /// column start
        /// </summary>
        string v_ColumnStart { get; set; }

        /// <summary>
        /// column end
        /// </summary>
        string v_ColumnEnd { get; set; }
    }
}
