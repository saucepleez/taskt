using taskt.Core.Automation.Commands.TextGroup;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// window name commands core properties
    /// </summary>
    public interface IWindowNameCoreProperties : ICanHandleWindowName, ITextCheckProperties
    {
        /// <summary>
        /// window name
        /// </summary>
        string v_WindowName { get; set; }

        // memo: imple text-check interface
        ///// <summary>
        ///// compare method (contains, starts-with, ...)
        ///// </summary>
        //string v_CompareMethod { get; set; }

        /// <summary>
        /// wait time for window
        /// </summary>
        string v_WaitTimeForWindow { get; set; }

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
