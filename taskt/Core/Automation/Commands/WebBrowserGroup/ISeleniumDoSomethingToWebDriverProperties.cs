namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    /// <summary>
    /// do something to WebDriver properties
    /// </summary>
    public interface ISeleniumDoSomethingToWebDriverProperties : ICanHandleWebDriver, IExpandableProperties
    {
        /// <summary>
        /// WebBrowser instance name
        /// </summary>
        string v_InstanceName { get; set; }
    }
}
