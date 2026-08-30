namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for to specifiy file path properties
    /// </summary>
    public interface IFilePathProperties : ICanHandleFilePath
    {
        /// <summary>
        /// target file path
        /// </summary>
        string v_TargetFilePath { get; set; }
    }
}
