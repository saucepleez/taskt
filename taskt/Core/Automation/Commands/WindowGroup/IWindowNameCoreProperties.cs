using taskt.Core.Automation.Commands.TextGroup;
using taskt.Core.Automation.Commands.WindowGroup;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// window name commands core properties
    /// </summary>
    public interface IWindowNameCoreProperties : IFromWindowNameResultsProperties, ICanHandleWindowName, ITextCheckProperties
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

        ///// <summary>
        ///// found window name
        ///// </summary>
        //string v_WindowNameResult { get; set; }

        ///// <summary>
        ///// found window handle
        ///// </summary>
        //string v_WindowHandleResult { get; set; }
    }
}
