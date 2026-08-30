using System.Data;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    /// <summary>
    /// for UIElement Action By/From someway properties
    /// </summary>
    public interface IUIElementUIElementActionSomewayProperties : IExpandableProperties, IHaveDataTableElements
    {
        /// <summary>
        /// UIElement Action type
        /// </summary>
        string v_AutomationType { get; set; }

        /// <summary>
        /// UIElement Action Parameters
        /// </summary>
        DataTable v_UIAActionParameters { get; set; }
    }
}
