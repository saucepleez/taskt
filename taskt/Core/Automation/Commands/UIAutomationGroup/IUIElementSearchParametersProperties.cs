using System.Data;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    /// <summary>
    /// UIElement search parameters
    /// </summary>
    public interface IUIElementSearchParametersProperties : IExpandableProperties, IHaveDataTableElements
    {
        /// <summary>
        /// UIElement search parameters
        /// </summary>
        DataTable v_SearchParameters { get; set; }

        /// <summary>
        /// wait time for UIElement
        /// </summary>
        string v_WaitTimeForUIElement { get; set; }
    }
}
