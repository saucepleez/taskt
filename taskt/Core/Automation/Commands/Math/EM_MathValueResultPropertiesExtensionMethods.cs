using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{
    public static class EM_MathValueResultPropertiesExtensionMethods
    {
        /// <summary>
        /// expand value or variable as Value
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static double ExpandValueOrVariableAsValue(this IMathValueResultProperties command, AutomationEngineInstance engine)
        {
            return (double)((ScriptCommand)command).ExpandValueOrUserVariableAsDecimal("Value", engine);
        }
    }
}
