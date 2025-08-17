namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// text compare properties
    /// </summary>
    public interface ITextCompareProperties : ICompareProperties
    {
        /// <summary>
        /// Trim Before Compare
        /// </summary>
        string v_TrimBeforeCompare { get; set; }
    }
}
