using taskt.Core.Automation.Commands._General;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// any window name commands properties
    /// </summary>
    public interface ILAnyWindowNameProperties : ILExpandableProperties
    {
        /// <summary>
        /// window name
        /// </summary>
        string v_WindowName { get; set; }

        /// <summary>
        /// compare method (contains, starts-with, ...)
        /// </summary>
        string v_CompareMethod { get; set; }

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
