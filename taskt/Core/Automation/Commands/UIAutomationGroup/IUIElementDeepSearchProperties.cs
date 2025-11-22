namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    /// <summary>
    /// UIElement deep search parameters
    /// </summary>
    public interface IUIElementDeepSearchProperties : IExpandableProperties
    {
        /// <summary>
        /// Max depth to Search UIElements
        /// </summary>
        string v_MaxDepth { get; set; }
    }
}
