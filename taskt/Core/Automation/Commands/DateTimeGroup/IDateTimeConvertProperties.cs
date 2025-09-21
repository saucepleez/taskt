namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// DateTime convert properties
    /// </summary>
    public interface IDateTimeConvertProperties : ICanHandleDateTime
    {
        /// <summary>
        /// DateTime variable name
        /// </summary>
        string v_DateTime { get; set; }

        /// <summary>
        /// Variable Name to Store Result
        /// </summary>
        string v_Result { get; set; }
    }
}
