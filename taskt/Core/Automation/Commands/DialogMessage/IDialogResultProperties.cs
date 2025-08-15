namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for to get dialog result properties
    /// </summary>
    public interface IDialogResultProperties
    {
        /// <summary>
        /// behavior when user click cancel in Dialog
        /// </summary>
        string v_WhenCancel { get; set; }

        /// <summary>
        /// variable name to store dialog result
        /// </summary>
        string v_DialogResult { get; set; }
    }
}
