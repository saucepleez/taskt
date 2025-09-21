using System;

namespace taskt.Core.Automation.Commands
{
    public static class EM_DateTimeResultPropertiesExtensionMethods
    {
        /// <summary>
        /// store DateTimre result in user variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="dt"></param>
        /// <param name="engine"></param>
        public static void StoreDateTimeInUserVariable(this IDateTimeResultProperties command, DateTime dt, Engine.AutomationEngineInstance engine)
        {
            command.StoreDateTimeInUserVariable(dt, nameof(command.v_Result), engine);
        }
    }
}
