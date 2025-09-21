namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// excel worksheet properties
    /// </summary>
    public interface IExcelWorksheetProperties : IExcelInstanceProperties
    {
        /// <summary>
        /// excel worksheet name
        /// </summary>
        string v_SheetName { get; set; }
    }
}
