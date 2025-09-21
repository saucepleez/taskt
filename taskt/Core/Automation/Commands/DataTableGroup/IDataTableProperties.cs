namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// DataTable properties
    /// </summary>
    public interface IDataTableProperties : ICanHandleDataTable, IExpandableProperties
    {
        /// <summary>
        /// DataTable variabe name
        /// </summary>
        string v_DataTable { get; set; }
    }
}
