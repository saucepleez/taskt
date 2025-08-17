namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// Create List from List properties
    /// </summary>
    public interface IListCreateFromListProperties : ICanHandleList, IExpandableProperties
    {
        /// <summary>
        /// List variable name to Create New List
        /// </summary>
        string v_TargetList { get; set; }

        /// <summary>
        /// List variable name to Store New List
        /// </summary>
        string v_NewList { get; set; }
    }
}
