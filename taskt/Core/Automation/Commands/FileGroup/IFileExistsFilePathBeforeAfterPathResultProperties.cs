namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for exists file path and get before/after file path properties
    /// </summary>
    public interface IFileExistsFilePathBeforeAfterPathResultProperties : IFileExistsFilePathProperties
    {
        /// <summary>
        /// variable name to store file path before command process
        /// </summary>
        string v_BeforeFilePathResult { get; set; }

        /// <summary>
        /// variable name to store file path after command process
        /// </summary>
        string v_AfterFilePathResult { get; set; }
    }
}
