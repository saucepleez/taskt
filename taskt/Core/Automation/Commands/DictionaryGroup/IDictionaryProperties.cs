namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// Dictionary properties
    /// </summary>
    public interface IDictionaryProperties : ICanHandleDictionary, IExpandableProperties
    {
        /// <summary>
        /// Dictionary variable name
        /// </summary>
        string v_Dictionary { get; set; }
    }
}
