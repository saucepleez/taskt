namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// column index/name specified properties
    /// </summary>
    public interface IExcelColumnSpecifiedProperties : IExcelRCRangeProperties
    {
        /// <summary>
        /// column index or Name
        /// </summary>
        string v_ColumnIndex { get; set; }
    }
}
