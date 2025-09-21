namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// Input JSON and JSONPath properties
    /// </summary>
    public interface IJSONJSONPathProperties : IJSONInputJContainer
    {
        /// <summary>
        /// JSONPath
        /// </summary>
        string v_JsonExtractor { get; set; }
    }
}
