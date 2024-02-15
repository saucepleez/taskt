namespace taskt.Core.Automation.Commands
{
    public interface IExcelRowRangeGetProperties : IExcelRowRangeProperties
    {
        /// <summary>
        /// variable name to store result
        /// </summary>
        string v_Result { get; set; }
    }
}
