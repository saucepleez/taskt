namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// Get List something From List Value
    /// </summary>
    public interface IListGetFromValueProperties : IListResultProperties
    {
        /// <summary>
        /// List Value
        /// </summary>
        string v_Value { get; set; }
    }
}
