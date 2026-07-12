namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// when fail execute some action behavior properties
    /// </summary>
    public interface IWhenFailActionBehaviorProperties : IExpandableProperties
    {
        /// <summary>
        /// behavior when Fail action
        /// </summary>
        string v_WhenFailAction { get; set; }
    }
}
