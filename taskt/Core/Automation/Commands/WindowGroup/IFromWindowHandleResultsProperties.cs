namespace taskt.Core.Automation.Commands.WindowGroup
{
    /// <summary>
    /// window name results properites
    /// </summary>
    public interface IFromWindowHandleResultsProperties : IExpandableProperties
    {
        /// <summary>
        /// found window name
        /// </summary>
        string v_WindowNameResult { get; set; }
    }
}
