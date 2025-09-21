using System.Collections.Generic;

namespace taskt.Core.Automation.Commands
{
    public static class EM_DicitionaryCreateFromDictionaryExtensionMethods
    {
        /// <summary>
        /// Expand user variable as Dictionary
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ExpandUserVariableAsDictionary(this IDictionaryCreateFromDictionary command, Engine.AutomationEngineInstance engine)
        {
            return command.ExpandUserVariableAsDictionary(nameof(command.v_TargetDictionary), engine);
        }

        /// <summary>
        /// store Dictionary in User Variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="dic"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        public static void StoreDictionaryInUserVariable(this IDictionaryCreateFromDictionary command, Dictionary<string, string> dic, Engine.AutomationEngineInstance engine)
        {
            command.StoreDictionaryInUserVariable(dic, nameof(command.v_NewDictionary), engine);
        }
    }
}
