using System.Collections.Generic;

namespace taskt.Core.Automation.Commands
{
    public static class EM_ListResultPropertiesExtensionMethods
    {
        /// <summary>
        /// Store List in User Variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="list"></param>
        /// <param name="engine"></param>
        public static void StoreListInUserVariable(this IListResultProperties command, List<string> list, Engine.AutomationEngineInstance engine)
        {
            command.StoreListInUserVariable(list, nameof(command.v_Result), engine);
        }
    }
}
