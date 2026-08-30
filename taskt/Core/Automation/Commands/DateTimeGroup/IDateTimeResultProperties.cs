namespace taskt.Core.Automation.Commands
{
    public interface IDateTimeResultProperties : ICanHandleDateTime
    {
        /// <summary>
        /// User Variable Name to Store DateTime Result
        /// </summary>
        string v_Result { get; set; }
    }
}
