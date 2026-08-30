namespace taskt.Core.Automation.Commands.KeyMouseGroup
{
    /// <summary>
    /// enter shortcut key properties
    /// </summary>
    public interface IEnterShortcutKeyProperties : IKeyActionCoreProperties
    {
        /// <summary>
        /// hot key (shortcut key)
        /// </summary>
        string v_Hotkey { get; set; }
    }
}
