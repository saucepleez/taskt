using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for DataTable Column Commands
    /// </summary>
    public abstract class ADataTableColumnCommands : ADataTableInputDataTableCommands, IDataTableColumnProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_ColumnType))]
        [PropertyParameterOrder(6000)]
        public virtual string v_ColumnType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_ColumnNameIndex))]
        [PropertyParameterOrder(7000)]
        public virtual string v_ColumnIndex { get; set; }
    }
}
