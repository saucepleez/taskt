namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// general check properties
    /// </summary>
    public interface ICheckProperties : IExpandableProperties
    {
        /// <summary>
        /// check method
        /// </summary>
        string v_CheckMethod { get; set; }
    }
}
