using Newtonsoft.Json.Linq;

namespace taskt.Core.Automation.Commands
{
    public static class EM_JSONInputJContainerExtensionMethods
    {
        /// <summary>
        /// Expand Value or User Variable as JSON
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns>(jsonText, JContainer, jsonType)</returns>
        public static (string, JContainer, string) ExpandValueOrUserVariableAsJSON(this IJSONInputJContainer command, Engine.AutomationEngineInstance engine)
        {
            return command.ExpandValueOrUserVariableAsJSON(nameof(command.v_Json), engine);
        }
    }
}
