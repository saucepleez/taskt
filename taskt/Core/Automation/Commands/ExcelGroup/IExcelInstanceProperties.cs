namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// excel instance properties
    /// </summary>
    public interface IExcelInstanceProperties : IExpandableProperties
    {
        /// <summary>
        /// excel instance name
        /// </summary>
        string v_InstanceName { get; set; }
    }
}
