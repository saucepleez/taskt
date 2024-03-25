using System;
using System.Collections.Generic;

namespace taskt.Core.Automation.Commands
{
    public static class EM_CanHandleListExtensionMethods
    {
        /// <summary>
        /// expand user varaible as List
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<string> ExpandUserVariableAsList(this ICanHandleList command, string parameterName, Engine.AutomationEngineInstance engine)
        {
            var variableName = ((ScriptCommand)command).GetRawPropertyValueAsString(parameterName, "List Variable");
            var v = variableName.GetRawVariable(engine);
            if (v.VariableValue is List<string> list)
            {
                return list;
            }
            else
            {
                throw new Exception($"Variable '{variableName}' is not List");
            }
        }

        /// <summary>
        /// store list in User Variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="list"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        public static void StoreListInUserVariable(this ICanHandleList command, List<string> list, string parameterName, Engine.AutomationEngineInstance engine)
        {
            var variableName = ((ScriptCommand)command).GetRawPropertyValueAsString(parameterName, "List Variable");
            ExtensionMethods.StoreInUserVariable(variableName, list, engine);
        }
    }
}
