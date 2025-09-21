namespace taskt.Core.Automation.Commands.KeyMouseGroup
{
    /// <summary>
    /// for enter keys properties
    /// </summary>
    public interface IEnterKeysProperties : IKeyActionCoreProperties
    {
        /// <summary>
        /// text or key strokes to send
        /// </summary>
        string v_TextToSend { get; set; }

        /// <summary>
        /// text encrypted
        /// </summary>
        string v_EncryptionOption { get; set; }

        /// <summary>
        /// use clipboard when text send
        /// </summary>
        string v_UserClipBoard { get; set; }

        /// <summary>
        /// clear clipboard after paste (when use clipboard)
        /// </summary>
        string v_ClearClipboardAfterPaste { get; set; }
    }
}
