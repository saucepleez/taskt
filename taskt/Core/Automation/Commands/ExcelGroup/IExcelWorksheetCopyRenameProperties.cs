namespace taskt.Core.Automation.Commands
{
    public interface IExcelWorksheetCopyRenameProperties : IExcelWorksheetActionProperties
    {
        /// <summary>
        /// new sheet name
        /// </summary>
        string v_NewSheetName { get; set; }
    }
}
