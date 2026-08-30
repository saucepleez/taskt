namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for set/get window state commands properties
    /// </summary>
    public interface IWindowStateProperties : IExpandableProperties
    {
        /// <summary>
        /// window state string or number
        /// </summary>
        string v_WindowState { get; set; }
    }
}
