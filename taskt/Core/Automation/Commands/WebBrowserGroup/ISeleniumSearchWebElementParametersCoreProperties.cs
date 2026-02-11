namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    public interface ISeleniumSearchWebElementParametersCoreProperties : IExpandableProperties
    {
        /// <summary>
        /// WebElement search method
        /// </summary>
        string v_SearchMethod { get; set; }

        /// <summary>
        /// WebElement search parameter
        /// </summary>
        string v_SearchParameter { get; set; }

        /// <summary>
        /// Wait time for WebElement
        /// </summary>
        string v_WaitTimeForWebElement { get; set; }
    }
}
