using System.Data;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// Filter Value Properties
    /// </summary>
    public interface IFilterValueProperties : IExpandableProperties
    {
        /// <summary>
        /// Value Type (Text or Number)
        /// </summary>
        string v_ValueType { get; set; }

        /// <summary>
        /// Filter Action
        /// </summary>
        string v_FilterAction { get; set; }

        /// <summary>
        /// Filter Action Parameters
        /// </summary>
        DataTable v_FilterActionParameterTable { get; set; }
    }
}
