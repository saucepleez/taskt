using taskt.Core.Automation.Commands._General;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// excel value type properties
    /// </summary>
    public interface ILExcelValueTypeProperties : ILExpandableProperties
    {
        /// <summary>
        /// value type
        /// </summary>
        string v_ValueType { get; set; }
    }
}
