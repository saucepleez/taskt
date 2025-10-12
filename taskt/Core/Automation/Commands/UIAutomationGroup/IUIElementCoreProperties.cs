namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    /// <summary>
    /// for UIElement action or get from UIElement properties
    /// </summary>
    public interface IUIElementCoreProperties : ICanHandleUIElement, IExpandableProperties
    {
        /// <summary>
        /// target UIElement variable name
        /// </summary>
        string v_TargetElement { get; set; }
    }
}
