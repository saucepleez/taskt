using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for commands that using Excel Cell get, set, etc...
    /// </summary>
    public abstract class AExcelCellActionCommand : AExcelCellCommand, IExcelCellActionProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ValueType))]
        [PropertyParameterOrder(8000)]
        public string v_ValueType { get; set; }
    }
}
