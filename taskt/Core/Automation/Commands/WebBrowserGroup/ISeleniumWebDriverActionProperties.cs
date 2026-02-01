namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    public interface ISeleniumWebDriverActionProperties : ISeleniumDoSomethingToWebDriverProperties
    {
        /// <summary>
        /// behavior when fail action
        /// </summary>
        string v_WhenFailAction { get; set; }
    }
}
