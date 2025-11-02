using System.Data;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    /// <summary>
    /// UIElement search parameters
    /// </summary>
    public interface IUIElementCoreSearchParametersProperties : IExpandableProperties, IHaveDataTableElements
    {
        /// <summary>
        /// UIElement search parameters
        /// </summary>
        DataTable v_SearchParameters { get; set; }

        /// <summary>
        /// wait time for UIElement
        /// </summary>
        string v_WaitTimeForUIElement { get; set; }

        /// <summary>
        /// maximum number of sibling nodes to search
        /// </summary>
        string v_MaxSiblings { get; set; }

        /// <summary>
        /// Maxinum Number of UIElements to Search
        /// </summary>
        string v_MaxNumberUIElements { get; set; }
    }
}
