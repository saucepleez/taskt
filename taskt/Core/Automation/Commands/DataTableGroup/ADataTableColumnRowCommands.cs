using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for DataTable Column Row specified commands
    /// </summary>
    public abstract class ADataTableColumnRowCommands : ADataTableInputDataTableCommands, IDataTableColumnRowPositionProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_ColumnType))]
        [PropertyParameterOrder(7000)]
        public virtual string v_ColumnType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_ColumnNameIndex))]
        [PropertyParameterOrder(8000)]
        public virtual string v_ColumnIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_RowIndex))]
        [PropertyParameterOrder(9000)]
        public virtual string v_RowIndex { get; set; }
    }
}
