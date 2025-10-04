namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for exists file path properties
    /// </summary>
    public interface IFileExistsFilePathProperties : IFilePathProperties
    {
        /// <summary>
        /// wait time for file (sec)
        /// </summary>
        string v_WaitTimeForFile { get; set; }
    }
}
