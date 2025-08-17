using System.Collections.Generic;

namespace taskt.Core.Automation.Commands
{
    public static class EM_DictionaryPropertiesExtensionMethods
    {
        /// <summary>
        /// Expand user variable as Dictionary
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ExpandUserVariableAsDictionary(this IDictionaryProperties command, Engine.AutomationEngineInstance engine)
        {
            return command.ExpandUserVariableAsDictionary(nameof(command.v_Dictionary), engine);
        }
    }
}
