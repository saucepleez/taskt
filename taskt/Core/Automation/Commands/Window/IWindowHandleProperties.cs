namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// window handle command properties
    /// </summary>
    public interface IWindowHandleProperties
    {
        /// <summary>
        /// window handle
        /// </summary>
        string v_WindowHandle { get; set; }

        /// <summary>
        /// wait time for window
        /// </summary>
        string v_WaitTimeForWindow { get; set; }
    }
}
