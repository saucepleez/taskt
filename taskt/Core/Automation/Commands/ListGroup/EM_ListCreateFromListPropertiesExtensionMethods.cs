using System.Collections.Generic;

namespace taskt.Core.Automation.Commands
{
    public static class EM_ListCreateFromListPropertiesExtensionMethods
    {
        /// <summary>
        /// Expand User Variable As List
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static List<string> ExpandUserVariableAsList(this IListCreateFromListProperties command, Engine.AutomationEngineInstance engine)
        {
            return command.ExpandUserVariableAsList(nameof(command.v_TargetList), engine);
        }

        /// <summary>
        /// Store List in User Variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="list"></param>
        /// <param name="engine"></param>
        public static void StoreListInUserVariable(this IListCreateFromListProperties command, List<string> list, Engine.AutomationEngineInstance engine)
        {
            command.StoreListInUserVariable(list, nameof(command.v_NewList), engine);
        }
    }
}
