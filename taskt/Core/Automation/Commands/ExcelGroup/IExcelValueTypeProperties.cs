namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// excel value type properties
    /// </summary>
    public interface IExcelValueTypeProperties : IExpandableProperties
    {
        /// <summary>
        /// value type
        /// </summary>
        string v_ValueType { get; set; }
    }
}
