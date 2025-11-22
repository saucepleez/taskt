namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    /// <summary>
    /// for UIElement core search by someway properites
    /// </summary>
    public interface IUIElementCoreSearchSomewayProperties : IExpandableProperties
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
