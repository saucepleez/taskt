namespace taskt.Core.Automation.Commands.TextGroup
{
    /// <summary>
    /// text check properties
    /// </summary>
    public interface ITextCheckProperties : ICheckProperties
    {
        /// <summary>
        /// check method is case sensitive or not
        /// </summary>
        string v_CaseSensitive { get; set; }

        /// <summary>
        /// Trim Before check
        /// </summary>
        string v_TrimBeforeCheck { get; set; }
    }
}
