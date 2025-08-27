namespace taskt.Core.Automation.Commands
{
    public interface IWindowWaitTimeBetweenFindAndActionProperties : IExpandableProperties
    {
        /// <summary>
        /// wait time between finding window and exection action
        /// </summary>
        string v_WaitTimeBetweenFindAndAction { get; set; }
    }
}
