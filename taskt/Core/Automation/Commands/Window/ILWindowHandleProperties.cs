using taskt.Core.Automation.Commands._General;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// window handle command properties
    /// </summary>
    public interface ILWindowHandleProperties : ILExpandableProperties
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
