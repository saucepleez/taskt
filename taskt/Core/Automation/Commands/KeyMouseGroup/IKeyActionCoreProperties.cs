namespace taskt.Core.Automation.Commands.KeyMouseGroup
{
    /// <summary>
    /// key action core properties
    /// </summary>
    public interface IKeyActionCoreProperties : IWindowActivateProperties
    {
        /// <summary>
        /// wait time after keys enter
        /// </summary>
        string v_WaitTimeAfterKeyEnter { get; set; }
    }
}
