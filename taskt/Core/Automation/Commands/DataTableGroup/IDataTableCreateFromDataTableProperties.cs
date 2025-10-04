namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// Create From DataTable properties
    /// </summary>
    public interface IDataTableCreateFromDataTableProperties : IExpandableProperties, ICanHandleDataTable
    {
        /// <summary>
        /// DataTable Variable Name to Create DataTable
        /// </summary>
        string v_TargetDataTable { get; set; }

        /// <summary>
        /// DataTable Variable Name to Store New DataTable
        /// </summary>
        string v_NewDataTable { get; set; }
    }
}
