using System.Data;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// Replace value properties
    /// </summary>
    public interface IReplaceValueProperties : IExpandableProperties
    {
        /// <summary>
        /// Value Type (Text or Number)
        /// </summary>
        string v_ValueType { get; set; }

        /// <summary>
        /// Replace Action
        /// </summary>
        string v_ReplaceAction { get; set; }

        /// <summary>
        /// Replace Action parameters
        /// </summary>
        DataTable v_ReplaceActionParameterTable { get; set; }

        /// <summary>
        /// New Value to Replace
        /// </summary>
        string v_NewValue { get; set; }
    }
}
