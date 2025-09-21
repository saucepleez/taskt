namespace taskt.Core.Automation.Commands
{
    public interface IExcelWorksheetActionProperties : IExcelInstanceProperties
    {
        /// <summary>
        /// worksheet name for action
        /// </summary>
        string v_TargetSheetName { get; set; }
    }
}
