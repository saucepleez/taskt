using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for DataTable Both Row Commands
    /// </summary>
    public abstract class ADataTableBothRowCommands : ADataTableRowCommands
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_BothDataTableName))]
        public override string v_DataTable { get; set; }
    }
}
