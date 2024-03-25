namespace taskt.Core.Automation.Commands
{
    public interface ILDataTableProperties : ICanHandleDataTable
    {
        /// <summary>
        /// DataTable variabe name
        /// </summary>
        string v_DataTable { get; set; }
    }
}
