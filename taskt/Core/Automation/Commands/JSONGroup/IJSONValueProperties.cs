namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// JSON Value properties
    /// </summary>
    public interface IJSONValueProperties : IExpandableProperties
    {
        /// <summary>
        /// JSON Value
        /// </summary>
        string v_Value { get; set; }

        /// <summary>
        /// Type of JSON Value
        /// </summary>
        string v_ValueType { get; set; }
    }
}
