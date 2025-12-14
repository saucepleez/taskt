namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    /// <summary>
    /// UIElement descendants search (someway) parameters
    /// </summary>
    public interface IUIElementDescendantsSearchSomewayProperties : IExpandableProperties
    {
        /// <summary>
        /// Max depth to Search UIElements
        /// </summary>
        string v_MaxDepth { get; set; }
    }
}
