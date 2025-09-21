namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// copy folder commands properties
    /// </summary>
    public interface IFolderCopyFolderProperties : IExpandableProperties
    {
        /// <summary>
        /// copy sub folder or not
        /// </summary>
        string v_CopySubFolder { get; set; }
    }
}
