namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// one window name commands properties
    /// </summary>
    public interface IOneWindowNameProperties : IWindowNameCoreProperties
    {
        /// <summary>
        /// selection method (first, last, index)
        /// </summary>
        string v_SelectionMethod { get; set; }

        /// <summary>
        /// selection method index
        /// </summary>
        string v_TargetWindowIndex { get; set; }
    }
}
