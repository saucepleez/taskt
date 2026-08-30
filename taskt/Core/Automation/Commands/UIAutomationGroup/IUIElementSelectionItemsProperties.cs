namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    /// <summary>
    /// for UIElement Selection Items
    /// </summary>
    public interface IUIElementSelectionItemsProperties : IDoSomethingUIElementProperties
    {
        /// <summary>
        /// try expand combobox when selection items not found
        /// </summary>
        string v_ExpandWhenItemsNotFound { get; set; }

        /// <summary>
        /// wait time after expand selection items
        /// </summary>
        string v_WaitTimeAfterExpand { get; set; }
    }
}
