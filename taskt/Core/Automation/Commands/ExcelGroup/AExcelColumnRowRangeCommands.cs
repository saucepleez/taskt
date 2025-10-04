using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Excel Column Row Range (2D-Range) commands
    /// </summary>
    public abstract class AExcelColumnRowRangeCommands : AExcelInstanceCommands, IExcelColumnRowRangeProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnType))]
        [PropertyParameterOrder(6000)]
        public virtual string v_ColumnType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnStart))]
        [PropertyParameterOrder(7000)]
        public virtual string v_ColumnStart { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnEnd))]
        [PropertyParameterOrder(8000)]
        public virtual string v_ColumnEnd { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_RowStart))]
        [PropertyIsOptional(false)]
        [PropertyParameterOrder(9000)]
        public virtual string v_RowStart { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_RowEnd))]
        [PropertyParameterOrder(10000)]
        public virtual string v_RowEnd { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ValueType))]
        [PropertyParameterOrder(12000)]
        public virtual string v_ValueType { get; set; }
    }
}
