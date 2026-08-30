namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// window position properties
    /// </summary>
    public interface IWindowPositionProperties : IPositionProperties, IWindowRECTProperties
    {
        /// <summary>
        /// when window is minimized
        /// </summary>
        string v_WhenWindowIsMinimized { get; set; }

        /// <summary>
        /// when window is maximized
        /// </summary>
        string v_WhenWindowIsMaximized { get; set; }
    }
}
