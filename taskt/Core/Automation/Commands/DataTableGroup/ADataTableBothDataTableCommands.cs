using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Both(Input/Output) DataTable commands
    /// </summary>
    public abstract class ADataTableBothDataTableCommands : ADataTableInputDataTableCommands
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_BothDataTableName))]
        public override string v_DataTable { get; set; }
    }
}
