namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// window resize commands properties
    /// </summary>
    public interface IWindowResizeProperties : IWindowSizeProperties
    {
        /// <summary>
        /// behavior when window is maximized
        /// </summary>
        string v_WhenWindowIsMaximized { get; set; }
    }
}
