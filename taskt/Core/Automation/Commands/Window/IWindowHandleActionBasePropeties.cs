namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for window handle action
    /// </summary>
    public interface IWindowHandleActionBasePropeties : IWindowHandleProperties
    {
        /// <summary>
        /// wait time between finding window and exection action
        /// </summary>
        string v_WaitTimeBetweenFindAndAction { get; set; }
    }
}
