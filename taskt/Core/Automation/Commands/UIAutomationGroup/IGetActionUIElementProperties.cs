namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    /// <summary>
    /// Get from UIElement or UIElement Action commands properties
    /// </summary>
    public interface IGetActionUIElementProperties : IUIElementCoreProperties
    {
        /// <summary>
        /// variable name to store Window Name
        /// </summary>
        string v_WindowNameResult { get; set; }

        /// <summary>
        /// variable name to store Window Handle
        /// </summary>
        string v_WindowHandleResult { get; set; }
    }
}
