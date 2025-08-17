namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// List Index properties
    /// </summary>
    public interface IListIndexProperties : IListProperties
    {
        /// <summary>
        /// List Index
        /// </summary>
        string v_Index { get; set; }
    }
}
