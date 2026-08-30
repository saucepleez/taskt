namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    /// <summary>
    /// get from webElement properties
    /// </summary>
    public interface ISeleniumGetFromWebElementProperties : ISeleniumWebElementActionAndScrollProperties
    {
        /// <summary>
        /// when value(s) can not retrieved
        /// </summary>
        string v_WhenValueCanNotRetrieved { get;set; }
    }
}
