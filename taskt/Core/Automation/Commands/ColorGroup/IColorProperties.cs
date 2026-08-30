namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// Color Properties
    /// </summary>
    public interface IColorProperties : ICanHandleColor
    {
        // TODO: atodeyaru
        /// <summary>
        /// Color Variable Name
        /// </summary>
        string v_Color { get; set; }
    }
}
