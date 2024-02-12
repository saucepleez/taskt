namespace taskt.Core.Automation.Commands
{
    public interface IExcelCellActionProperties : IExcelCellProperties
    {
        /// <summary>
        /// cell value type
        /// </summary>
        string v_ValueType { get; set; }
    }
}
