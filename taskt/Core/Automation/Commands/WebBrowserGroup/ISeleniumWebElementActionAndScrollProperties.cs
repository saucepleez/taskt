namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    /// <summary>
    /// for WebElement Action supports scroll to WebElement commands
    /// </summary>
    public interface ISeleniumWebElementActionAndScrollProperties : ISeleniumWebElementActionProperties
    {
        /// <summary>
        /// scroll to WebElement before Action
        /// </summary>
        string v_ScrollToWebElement { get; set; }
    }
}
