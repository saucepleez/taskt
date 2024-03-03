namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// color create properties
    /// </summary>
    public interface IColorCreateProperties : ICanHandleColor
    {
        /// <summary>
        /// color variable name
        /// </summary>
        string v_Color { get; set; }
    }
}
