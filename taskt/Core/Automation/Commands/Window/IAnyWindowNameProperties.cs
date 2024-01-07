namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// any window name commands properties
    /// </summary>
    public interface IAnyWindowNameProperties
    {
        /// <summary>
        /// window name
        /// </summary>
        string v_WindowName { get; set; }

        /// <summary>
        /// search method (contains, starts-with, ...)
        /// </summary>
        string v_SearchMethod { get; set; }

        /// <summary>
        /// wait time for window
        /// </summary>
        string v_WaitTime { get; set; }

        /// <summary>
        /// found window name
        /// </summary>
        string v_NameResult { get; set; }

        /// <summary>
        /// found window handle
        /// </summary>
        string v_HandleResult { get; set; }
    }
}
