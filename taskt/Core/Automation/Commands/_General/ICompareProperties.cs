namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// general compare properties
    /// </summary>
    public interface ICompareProperties : IExpandableProperties
    {
        /// <summary>
        /// compare method
        /// </summary>
        string v_CompareMethod { get; set; }

        /// <summary>
        /// compare method is case sensitive or not
        /// </summary>
        string v_CaseSensitive { get; set; }
    }
}
