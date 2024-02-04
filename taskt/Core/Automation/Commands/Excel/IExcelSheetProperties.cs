namespace taskt.Core.Automation.Commands
{
    public interface IExcelSheetProperties : IExcelInstanceProperties
    {
        /// <summary>
        /// excel worksheet name
        /// </summary>
        string v_SheetName { get; set; }
    }
}
