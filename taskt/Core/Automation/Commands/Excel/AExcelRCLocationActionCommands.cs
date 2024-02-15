using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Excel RC Location Action commands
    /// </summary>
    public abstract class AExcelRCLocationActionCommands: AExcelRCLocationCommand, IExcelRCLocationActionProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ValueType))]
        [PropertyParameterOrder(9000)]
        public virtual string v_ValueType { get; set; }
    }
}
