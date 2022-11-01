using System;
using System.Collections.Generic;

namespace taskt.Core.Automation.Commands
{
    internal static class ListControls
    {
        /// <summary>
        /// get List&lt;string&gt; variable from variable name
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<string> GetListVariable(this string variableName, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            Script.ScriptVariable v = variableName.GetRawVariable(engine);
            if (v.VariableValue is List<string> list)
            {
                return list;
            }
            else
            {
                throw new Exception("Variable " + variableName + " is not supported List");
            }
        }

        public static void StoreInUserVariable<Type>(this List<Type> value, Core.Automation.Engine.AutomationEngineInstance sender, string targetVariable)
        {
            ExtensionMethods.StoreInUserVariable(targetVariable, value, sender, false);
        }

        /// <summary>
        /// convert list&lt;string&gt; to List&lt;decimal&gt; from variable name
        /// </summary>
        /// <param name="listName"></param>
        /// <param name="ignoreNotNumeric"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<decimal> ConvertToDecimalList(string listName, bool ignoreNotNumeric, Engine.AutomationEngineInstance engine)
        {
            var list = listName.GetListVariable(engine);

            List<decimal> numList = new List<decimal>();
            foreach(var value in list)
            {
                if (decimal.TryParse(value, out decimal v))
                {
                    numList.Add(v);
                }
                else if (!ignoreNotNumeric)
                {
                    throw new Exception(listName + " has not numeric value.");
                }
            }

            return numList;
        }
    }
}
