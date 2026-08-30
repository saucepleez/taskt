namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// folder path properties
    /// </summary>
    public interface IFolderPathProperties: ICanHandleFolderPath
    {
        /// <summary>
        /// target folder path
        /// </summary>
        string v_TargetFolderPath { get; set; }
    }
}
