using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Create From DataTable commands
    /// </summary>
    public abstract class ADataTableCreateFromDataTableCommands : ScriptCommand, IDataTableCreateFromDataTableProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_InputDataTableName))]
        [PropertyParameterOrder(5000)]
        public abstract string v_TargetDataTable { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_NewOutputDataTableName))]
        [PropertyParameterOrder(10000)]
        public virtual string v_NewDataTable { get; set; }
    }
}
