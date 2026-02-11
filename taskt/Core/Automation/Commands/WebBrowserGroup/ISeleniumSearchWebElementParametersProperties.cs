namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    /// <summary>
    /// for WebElement search parameters
    /// </summary>
    public interface ISeleniumSearchWebElementParametersProperties : ISeleniumSearchMultiWebElementsParametersProperties, IExpandableProperties
    {
        ///// <summary>
        ///// WebElement search method
        ///// </summary>
        //string v_SearchMethod { get; set; }

        ///// <summary>
        ///// WebElement search parameter
        ///// </summary>
        //string v_SearchParameter { get; set; }

        /// <summary>
        /// WebElement index
        /// </summary>
        string v_WebElementIndex { get; set; }

        ///// <summary>
        ///// Wait time for WebElement
        ///// </summary>
        //string v_WaitTimeForWebElement { get; set; }
    }
}
