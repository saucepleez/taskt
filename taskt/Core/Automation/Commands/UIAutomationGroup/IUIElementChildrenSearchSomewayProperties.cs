namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    /// <summary>
    /// for UIElement children search by someway properites
    /// </summary>
    public interface IUIElementChildrenSearchSomewayProperties : IUIElementWindowResultsFromUIElementProperties
    {
        /// <summary>
        /// wait time for UIElement
        /// </summary>
        string v_WaitTimeForUIElement { get; set; }

        /// <summary>
        /// maximum number of sibling nodes to search
        /// </summary>
        string v_MaxSiblings { get; set; }
    }
}
