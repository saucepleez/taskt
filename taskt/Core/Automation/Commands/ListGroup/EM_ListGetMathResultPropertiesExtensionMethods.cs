using System;
using System.Collections.Generic;

namespace taskt.Core.Automation.Commands
{
    public static class EM_ListGetMathResultPropertiesExtensionMethods
    {
        /// <summary>
        /// Expand User Variable as Decimal (Numeric) List
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static List<decimal> ExpandUserVariableAsDecimalList(this IListGetMathResultFromListProperties command, Engine.AutomationEngineInstance engine)
        {
            var notNumeric = command.ToScriptCommand().ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_WhenValueIsNotNumeric), "When Not Numeric", engine);
            return command.ExpandUserVariableAsDecimalList(nameof(command.v_List), (notNumeric == "ignore"), engine);
        }

        /// <summary>
        /// Decimal (Numeric) List Math Process
        /// </summary>
        /// <param name="command"></param>
        /// <param name="mathFunc"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static decimal MathProcess(this IListGetMathResultFromListProperties command, Func<List<decimal>, decimal> mathFunc, Engine.AutomationEngineInstance engine)
        {
            return mathFunc(command.ExpandUserVariableAsDecimalList(engine));
        }
    }
}
