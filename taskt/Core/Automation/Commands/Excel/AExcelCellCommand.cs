using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    public abstract class AExcelCellCommand : AExcelInstanceCommand, IExcelCellProperties
    {
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_CellRangeLocation))]
        [PropertyParameterOrder(6000)]
        public virtual string v_CellLocation { get; set; }
    }
}
