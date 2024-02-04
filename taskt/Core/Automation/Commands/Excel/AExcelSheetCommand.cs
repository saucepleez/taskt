using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.Excel
{
    /// <summary>
    /// for commands that using Excel Sheet
    /// </summary>
    public abstract class AExcelSheetCommand : AExcelInstanceCommand, IExcelSheetProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_SheetName))]
        [PropertyParameterOrder(6000)]
        public virtual string v_SheetName { get; set; }
    }
}
