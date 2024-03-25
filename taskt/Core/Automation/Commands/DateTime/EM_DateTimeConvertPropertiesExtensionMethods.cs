using System;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{
    public static class EM_DateTimeConvertPropertiesExtensionMethods
    {
        /// <summary>
        /// expand value or variable as DateTime
        /// </summary>
        /// <param name="command"></param>
        /// <param name="paramterName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static DateTime ExpandValueOrVariableAsDateTime(this IDateTimeConvertProperties command, AutomationEngineInstance engine)
        {
            return command.ExpandValueOrVariableAsDateTime(nameof(command.v_DateTime), engine);
        }
    }
}
