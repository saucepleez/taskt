namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// DataTable Column Position Properties
    /// </summary>
    public interface IDataTableColumnPositionProperties : IExpandableProperties
    {
        /// <summary>
        /// Column Type (Index or ColumnName)
        /// </summary>
        string v_ColumnType { get; set; }

        /// <summary>
        /// Column Index or Name
        /// </summary>
        string v_ColumnIndex { get; set; }
    }
}
