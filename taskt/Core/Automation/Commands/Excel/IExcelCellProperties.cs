namespace taskt.Core.Automation.Commands
{
    public interface IExcelCellProperties : IExcelInstanceProperties
    {
        /// <summary>
        /// cell location
        /// </summary>
        string v_CellLocation { get; set; }
    }
}
