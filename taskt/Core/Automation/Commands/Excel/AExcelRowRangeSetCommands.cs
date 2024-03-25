using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    public abstract class AExcelRowRangeSetCommands : AExcelRowRangeCommands, IExcelRowRangeSetProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_WhenItemNotEnough))]
        [PropertyDescription("When Items Not Enough")]
        [PropertyParameterOrder(12000)]
        public abstract string v_WhenItemNotEnough { get; set; }
    }
}
