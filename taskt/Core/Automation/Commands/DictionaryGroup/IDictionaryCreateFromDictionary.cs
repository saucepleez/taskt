namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// Create Dictionary from Dictionary properties
    /// </summary>
    public interface IDictionaryCreateFromDictionary: IExpandableProperties, ICanHandleDictionary
    {
        /// <summary>
        /// Dictionary variable name to Create New Dictionary
        /// </summary>
        string v_TargetDictionary { get; set; }

        /// <summary>
        /// Dictionary variable name to Store New Dictionary
        /// </summary>
        string v_NewDictionary { get; set; }
    }
}
