using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for DataTable Both Column Commands
    /// </summary>
    public abstract class ADataTableBothColumnCommands : ADataTableColumnCommands
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_BothDataTableName))]
        public override string v_DataTable { get; set; }
    }
}
