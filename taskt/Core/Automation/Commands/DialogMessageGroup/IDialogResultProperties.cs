namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for to get dialog result properties
    /// </summary>
    public interface IDialogResultProperties : IExpandableProperties
    {
        /// <summary>
        /// variable name to store dialog result
        /// </summary>
        string v_DialogResult { get; set; }
    }
}
