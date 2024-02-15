namespace taskt.Core.Automation.Commands
{
    public interface IExcelRCRangeProperties : IExcelInstanceProperties, IExcelValueTypeProperties
    {
        /// <summary>
        /// column type Name(Range) or Index
        /// </summary>
        string v_ColumnType { get; set; }
    }
}
