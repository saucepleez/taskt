namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// extension methods for ILExpandableProperties
    /// </summary>
    public static class EM_ExpandablePropertiesExtensionMethods
    {
        /// <summary>
        /// convert to ScriptCommand
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ScriptCommand ToScriptCommand(this IExpandableProperties command)
        {
            // TODO: It will eventually go out of use.
            return (ScriptCommand)command;
        }
    }
}
