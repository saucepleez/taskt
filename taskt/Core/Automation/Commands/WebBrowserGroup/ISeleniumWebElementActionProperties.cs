namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    /// <summary>
    /// for WebElement action commands properties
    /// </summary>
    public interface ISeleniumWebElementActionProperties : ISeleniumDoSomethingToWebElementProperties
    {
        /// <summary>
        /// behavior when Fail action
        /// </summary>
        string v_WhenFailAction { get; set; }
    }
}
