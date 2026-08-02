namespace taskt.Core.Automation.Commands.ExcelGroup
{
    /// <summary>
    /// shape name properties
    /// </summary>
    public interface IExcelShapeNameProperties : IExcelInstanceProperties, ICanHandleExcelShapes
    {
        /// <summary>
        /// shape
        /// </summary>
        string v_ShapeName { get; set; }
    }
}
