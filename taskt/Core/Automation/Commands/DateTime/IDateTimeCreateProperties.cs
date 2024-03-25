namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// DateTime create properties
    /// </summary>
    public interface IDateTimeCreateProperties : ICanHandleDateTime
    {
        /// <summary>
        ///  DateTime variable name
        /// </summary>
        string v_DateTime { get; set; }
    }
}
