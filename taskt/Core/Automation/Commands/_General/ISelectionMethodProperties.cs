namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// general selection properties
    /// </summary>
    public interface ISelectionMethodProperties : IExpandableProperties
    {
        /// <summary>
        /// selection method
        /// </summary>
        string v_SelectionMethod { get; set; }

        // index property name is v_Target***Index
    }
}
