namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// exists folder path properties
    /// </summary>
    public interface IFolderExistsFolderPathProperties : IFolderPathProperties
    {
        /// <summary>
        /// wait time for folder
        /// </summary>
        string v_WaitTimeForFolder { get; set; }
    }
}
