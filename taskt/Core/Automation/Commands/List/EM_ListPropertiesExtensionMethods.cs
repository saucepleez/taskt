using System.Collections.Generic;

namespace taskt.Core.Automation.Commands
{
    public static class EM_ListPropertiesExtensionMethods
    {
        /// <summary>
        /// Expand User Variable as List
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static List<string> ExpandUserVariableAsList(this IListProperties command, Engine.AutomationEngineInstance engine)
        {
            return command.ExpandUserVariableAsList(nameof(command.v_List), engine);
        }
    }
}
