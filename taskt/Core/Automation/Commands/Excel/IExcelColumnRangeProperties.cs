namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Excel Column Range properties
    /// </summary>
    public interface IExcelColumnRangeProperties : IExcelRCRangeProperties
    {
        /// <summary>
        /// column index
        /// </summary>
        string v_ColumnIndex { get; set; }

        /// <summary>
        /// row index start
        /// </summary>
        string v_RowStart { get; set; }

        /// <summary>
        /// row index end
        /// </summary>
        string v_RowEnd { get; set; }
    }
}
