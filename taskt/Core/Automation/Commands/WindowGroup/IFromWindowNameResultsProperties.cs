namespace taskt.Core.Automation.Commands.WindowGroup
{
    /// <summary>
    /// window name & handle results properties
    /// </summary>
    public interface IFromWindowNameResultsProperties : IFromWindowHandleResultsProperties
    {
        /// <summary>
        /// found window handle
        /// </summary>
        string v_WindowHandleResult { get; set; }
    }
}
