namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// window size properties
    /// </summary>
    public interface IWindowSizeProperties : ISizeProperties, IWindowRECTProperties
    {
        /// <summary>
        /// when target window is minimized
        /// </summary>
        string v_WhenWindowIsMinimized { get; set; }
    }
}
