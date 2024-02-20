using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for commands that using Excel Cell
    /// </summary>
    public abstract class AExcelCellCommands : AExcelInstanceCommands, IExcelCellProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_CellRangeLocation))]
        [PropertyParameterOrder(6000)]
        public virtual string v_CellLocation { get; set; }
    }
}
