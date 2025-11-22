namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    /// <summary>
    /// UIElement deep search (someway) parameters
    /// </summary>
    public interface IUIElementDeepSearchSomewayProperties : IExpandableProperties
    {
        /// <summary>
        /// Max depth to Search UIElements
        /// </summary>
        string v_MaxDepth { get; set; }
    }
}
