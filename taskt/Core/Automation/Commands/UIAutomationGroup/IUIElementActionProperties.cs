namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    public interface IUIElementActionProperties : IDoSomethingUIElementProperties
    {
        /// <summary>
        /// wait time before action
        /// </summary>
        string v_WaitTimeBeforeAction { get; set; }

        /// <summary>
        /// wait time after action
        /// </summary>
        string v_WaitTimeAfterAction { get; set; }

        /// <summary>
        /// activate window before action
        /// </summary>
        string v_ActivateWindowBeforeAction { get; set; }

        /// <summary>
        /// when UIElement is not supprted this action
        /// </summary>
        string v_WhenActionIsNotSupported { get; set; }
    }
}
