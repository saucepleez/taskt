namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Excel Column-Row Range (2D-Range) Properties
    /// </summary>
    public interface IExcelColumnRowRangeProperties : ILExcelInstanceProperties, IExcelRCRangeProperties
    {
        /// <summary>
        /// column start index
        /// </summary>
        string v_ColumnStart { get; set; }
        
        /// <summary>
        /// column end index
        /// </summary>
        string v_ColumnEnd { get; set; }

        /// <summary>
        /// row start index
        /// </summary>
        string v_RowStart { get; set; } 

        /// <summary>
        /// row end index
        /// </summary>
        string v_RowEnd { get; set; }
    }
}
