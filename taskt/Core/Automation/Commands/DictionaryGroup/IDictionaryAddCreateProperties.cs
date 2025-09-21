using System.Data;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// Add Items to Dictionary or Create Dictionary properties
    /// </summary>
    public interface IDictionaryAddCreateProperties : IDictionaryProperties
    {
        /// <summary>
        /// Dictionary Keys & Values
        /// </summary>
        DataTable v_ColumnNameDataTable { get; set; }
    }
}
