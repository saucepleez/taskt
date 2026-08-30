namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// exists forlder path before after path result properties
    /// </summary>
    public interface IFolderExistsFolderPathBeforeAfterPathResultProperties : IFolderExistsFolderPathProperties
    {
        /// <summary>
        /// variable name to store folder path before process
        /// </summary>
        string v_BeforeFolderPathResult { get; set; }

        /// <summary>
        /// variable name to store folder path after process
        /// </summary>
        string v_AfterFolderPathResult { get; set; }
    }
}
