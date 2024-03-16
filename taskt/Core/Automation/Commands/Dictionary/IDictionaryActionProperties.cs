namespace taskt.Core.Automation.Commands.Dictionary
{
    /// <summary>
    /// Dictionary action properties
    /// </summary>
    public interface IDictionaryActionProperties : ICanHandleDictionary
    {
        /// <summary>
        /// Dictionary variable name
        /// </summary>
        string v_Dictionary { get; set; }
    }
}
