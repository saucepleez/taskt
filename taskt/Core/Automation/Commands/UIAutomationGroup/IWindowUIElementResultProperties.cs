namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    /// <summary>
    /// window UIElement results
    /// </summary>
    public interface IWindowUIElementResultProperties : ICanHandleUIElement
    {
        /// <summary>
        /// Variable Name to Store Window UIElement
        /// </summary>
        string v_WindowUIElement { get; set; }
    }
}
