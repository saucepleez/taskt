namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    /// <summary>
    /// deep search UIElements and Get any-one UIElement properties
    /// </summary>
    public interface IUIElementDeepSearchAnyUIElementProperties : IUIElementDeepSearchParametersProperties
    {
        /// <summary>
        /// UIElement Index
        /// </summary>
        string v_TargetUIElementIndex { get; set; }
    }
}
