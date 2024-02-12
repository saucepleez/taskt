namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// Math commands value-result
    /// </summary>
    public interface IMathValueResultProperties
    {
        /// <summary>
        /// value
        /// </summary>
        string v_Value { get; set; }

        /// <summary>
        /// variable name to store result
        /// </summary>
        string v_Result { get; set; }
    }
}
