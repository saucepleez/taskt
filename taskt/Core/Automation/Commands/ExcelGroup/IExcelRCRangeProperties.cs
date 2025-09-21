namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// Range properties specified Row, Column
    /// </summary>
    public interface IExcelRCRangeProperties : IExcelInstanceProperties, IExcelRCValueTypeProperties
    {
        /// <summary>
        /// column type Name(Range) or Index
        /// </summary>
        string v_ColumnType { get; set; }
    }
}
