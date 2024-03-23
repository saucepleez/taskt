namespace taskt.Core.Automation.Commands
{
    public interface ILDataTableProperties : ILExpandableProperties, ICanHandleDataTable
    {
        /// <summary>
        /// DataTable variabe name
        /// </summary>
        string v_DataTable { get; set; }
    }
}
