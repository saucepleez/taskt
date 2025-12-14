namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    /// <summary>
    /// UIElement children search XPath properties
    /// </summary>
    public interface IUIElementChildrenSearchXPathProperties : IUIElementChildrenSearchSomewayProperties
    {
        /// <summary>
        /// UIElement search XPath
        /// </summary>
        string v_SearchXPath { get; set; }
    }
}
