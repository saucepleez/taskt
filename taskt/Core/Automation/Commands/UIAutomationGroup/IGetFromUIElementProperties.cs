namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    /// <summary>
    /// get something from UIElement properties
    /// </summary>
    public interface IGetFromUIElementProperties : IDoSomethingUIElementProperties
    {
        /// <summary>
        /// when Value(s) can not retrieved
        /// </summary>
        string v_WhenValueCanNotRetrieved { get; set; }
    }
}
