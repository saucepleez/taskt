namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// Json input properties
    /// </summary>
    public interface IJSONInputJSONProperties : ICanHandleJSON, IExpandableProperties
    {
        /// <summary>
        /// JSON Value or Variable Name
        /// </summary>
        string v_Json { get; set; }
    }
}
