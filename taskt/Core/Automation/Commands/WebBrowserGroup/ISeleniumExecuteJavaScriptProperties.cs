namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    /// <summary>
    /// selenium Execute JavaScript properties
    /// </summary>
    public interface ISeleniumExecuteJavaScriptProperties : ISeleniumWebDriverActionProperties, IResultProperties
    {
        /// <summary>
        /// arguments
        /// </summary>
        string v_Arguments { get; set; }
    }
}
