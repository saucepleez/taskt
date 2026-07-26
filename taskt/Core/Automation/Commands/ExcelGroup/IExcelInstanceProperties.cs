using taskt.Core.Automation.Commands.ExcelGroup;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// excel instance properties
    /// </summary>
    public interface IExcelInstanceProperties : ICanHandleExcelInstance, IExpandableProperties
    {
        /// <summary>
        /// excel instance name
        /// </summary>
        string v_InstanceName { get; set; }
    }
}
