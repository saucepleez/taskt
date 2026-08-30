namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    /// <summary>
    /// UIElement Index properties
    /// </summary>
    public interface IUIElementIndexProperties : ISelectionMethodProperties, IExpandableProperties
    {
        /// <summary>
        /// UIElement Index
        /// </summary>
        string v_TargetUIElementIndex { get; set; }
    }
}
