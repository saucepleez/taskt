namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// Excel **Single** Cell properties
    /// </summary>
    public interface IExcelCellProperties : IExcelInstanceProperties
    {
        /// <summary>
        /// cell location
        /// </summary>
        string v_CellLocation { get; set; }
    }
}
