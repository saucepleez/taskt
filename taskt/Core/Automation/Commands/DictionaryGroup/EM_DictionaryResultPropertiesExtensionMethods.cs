using System.Collections.Generic;

namespace taskt.Core.Automation.Commands
{
    public static class EM_DictionaryResultPropertiesExtensionMethods
    {
        /// <summary>
        /// Store Dictionary in User Variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="dic"></param>
        /// <param name="engine"></param>
        public static void StoreDictionaryInUserVariable(this IDictionaryResultProperties command, Dictionary<string, string> dic, Engine.AutomationEngineInstance engine)
        {
            command.StoreDictionaryInUserVariable(dic, nameof(command.v_Result), engine);
        }
    }
}
