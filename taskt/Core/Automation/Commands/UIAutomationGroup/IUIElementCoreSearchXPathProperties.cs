namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    /// <summary>
    /// UIElement core search XPath properties
    /// </summary>
    public interface IUIElementCoreSearchXPathProperties : IUIElementCoreSearchSomewayProperties, IUIElementWindowResultsFromUIElementProperties
    {
        /// <summary>
        /// UIElement search XPath
        /// </summary>
        string v_SearchXPath { get; set; }
    }
}
