using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for DataTable Row commands
    /// </summary>
    public abstract class ADataTableRowCommands : ADataTableInputDataTableCommands, IDataTableRowProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_RowIndex))]
        [PropertyParameterOrder(6000)]
        public virtual string v_RowIndex { get; set; }
    }
}
