namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// exists folder path and path result properties
    /// </summary>
    public interface IFolderExistsFolderPathPathResultProperties : IFolderExistsFolderPathProperties
    {
        /// <summary>
        /// variable name to store result folder path
        /// </summary>
        string v_ResultPath { get; set; }
    }
}
