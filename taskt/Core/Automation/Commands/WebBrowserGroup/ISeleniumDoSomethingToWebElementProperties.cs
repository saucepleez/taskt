namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    /// <summary>
    /// do something to WebElement properties
    /// </summary>
    public interface ISeleniumDoSomethingToWebElementProperties : ICanHandleWebElement, IExpandableProperties
    {
        /// <summary>
        /// target WebElement variable name
        /// </summary>
        string v_WebElement { get; set; }
    }
}
