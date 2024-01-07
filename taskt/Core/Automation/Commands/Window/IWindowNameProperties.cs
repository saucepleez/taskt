namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// window name commands properties
    /// </summary>
    public interface IWindowNameProperties : IAnyWindowNameProperties
    {
        /// <summary>
        /// match method (first, last, index)
        /// </summary>
        string v_MatchMethod { get; set; }

        /// <summary>
        /// match method index
        /// </summary>
        string v_TargetWindowIndex { get; set; }
    }
}
