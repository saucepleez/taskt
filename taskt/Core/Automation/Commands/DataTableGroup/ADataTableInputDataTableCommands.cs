using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Input DataTable commands
    /// </summary>
    public abstract class ADataTableInputDataTableCommands : ScriptCommand, IDataTableProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_InputDataTableName))]
        [PropertyParameterOrder(5000)]
        public virtual string v_DataTable { get; set; }
    }
}
