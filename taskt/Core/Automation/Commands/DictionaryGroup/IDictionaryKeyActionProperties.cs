namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// Dictionary & Key action properties
    /// </summary>
    public interface IDictionaryKeyActionProperties : IDictionaryKeyProperties
    {
        /// <summary>
        /// When Key does not exists action
        /// </summary>
        string v_WhenKeyDoesNotExists { get; set; }
    }
}
