namespace taskt.Core.Automation.Commands
{
    public interface IExcelValueSetProperties : IExpandableProperties
    {
        /// <summary>
        /// text to set
        /// </summary>
        string v_TextToSet { get; set; }
    }
}
