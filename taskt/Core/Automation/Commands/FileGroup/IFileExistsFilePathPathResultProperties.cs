namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for exists file path and get path result properties
    /// </summary>
    public interface IFileExistsFilePathPathResultProperties : IFileExistsFilePathProperties
    {
        /// <summary>
        /// variable name to store exists file path
        /// </summary>
        string v_ResultPath { get; set; }
    }
}
