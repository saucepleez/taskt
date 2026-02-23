namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    /// <summary>
    /// for create WebDriver properties
    /// </summary>
    public interface ISeleniumCreateWebDriverProperties : ISeleniumDoSomethingToWebDriverProperties
    {
        /// <summary>
        /// WebBrowser type
        /// </summary>
        string v_BrowserType { get; set; }

        /// <summary>
        /// WebDriver binary path
        /// </summary>
        string v_WebDriverPath { get; set; }

        /// <summary>
        /// show/hide WebDriver terminal window
        /// </summary>
        string v_HideTerminalWindow { get; set; }
    }
}
