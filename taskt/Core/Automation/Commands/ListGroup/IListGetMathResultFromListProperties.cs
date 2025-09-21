namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// Get Math Result From List properties
    /// </summary>
    public interface IListGetMathResultFromListProperties : IListGetFromListProperties
    {
        /// <summary>
        /// Behavior when Value is Not Numeric
        /// </summary>
        string v_WhenValueIsNotNumeric { get; set; }
    }
}
