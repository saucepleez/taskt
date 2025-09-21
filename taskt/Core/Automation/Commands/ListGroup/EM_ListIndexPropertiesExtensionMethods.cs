using System;
using System.Collections.Generic;

namespace taskt.Core.Automation.Commands
{
    public static class EM_ListIndexPropertiesExtensionMethods
    {
        /// <summary>
        /// Expand Value Or User Variable As List Index
        /// </summary>
        /// <param name="command"></param>
        /// <param name="list"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static int ExpandValueOrUserVariableAsListIndex(this IListIndexProperties command, List<string> list, Engine.AutomationEngineInstance engine)
        {
            var index = command.ToScriptCommand().ExpandValueOrUserVariableAsInteger(nameof(command.v_Index), engine);
            if (index < 0)
            {
                index += list.Count;
            }
            if (index >= 0 && index < list.Count)
            {
                return index;
            }
            else
            {
                throw new Exception($"Strange List Index Value. Value: '{command.v_Index}', Expand Value: '{index}'");
            }
        }

        /// <summary>
        /// Expand Value Or User Variable As List and List Index and List Value
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns>(list, index, value)</returns>
        public static (List<string>, int, string) ExpandValueOrUserVariableAsListAndIndexAndValue(this IListIndexProperties command, Engine.AutomationEngineInstance engine)
        {
            var list = command.ExpandUserVariableAsList(engine);
            var index = command.ExpandValueOrUserVariableAsListIndex(list, engine);
            return (list, index, list[index]);
        }
    }
}
