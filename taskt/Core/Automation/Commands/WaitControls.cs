using System;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// general wait methods
    /// </summary>
    internal static class WaitControls
    {
        #region VirtualProperty

        /// <summary>
        /// Wait Time for something
        /// </summary>
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Wait Time for *** (sec)")]
        [InputSpecification("Number Greater than or Equal 0")]
        [PropertyDetailSampleUsage("**10**", PropertyDetailSampleUsage.ValueType.Value, "Wait Time")]
        [PropertyDetailSampleUsage("**{{{vWaitTime}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Wait Time")]
        [PropertyDisplayText(true, "Wait", "s")]
        [PropertyValidationRule("Wait Time", PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        public static string v_WaitTime { get; }

        #endregion

        /// <summary>
        /// general waiting process, this method get pauseTime from command property
        /// </summary>
        /// <param name="command"></param>
        /// <param name="waitTimeName"></param>
        /// <param name="targetName"></param>
        /// <param name="waitFunc"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static object WaitProcess(this ScriptCommand command, string waitTimeName, string targetName, Func<(bool, object)> waitFunc, Engine.AutomationEngineInstance engine)
        {
            int pauseTime = command.ExpandValueOrUserVariableAsInteger(waitTimeName, "Wait Time", engine);

            return WaitProcess(pauseTime, targetName, waitFunc, engine);
        }

        /// <summary>
        /// general waiting process
        /// </summary>
        /// <param name="pauseTime"></param>
        /// <param name="targetName"></param>
        /// <param name="waitFunc"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static object WaitProcess(int pauseTime, string targetName, Func<(bool, object)> waitFunc, Engine.AutomationEngineInstance engine)
        {
            var stopWaiting = DateTime.Now.AddSeconds(pauseTime);

            object ret = null;
            var isFound = false;

            // while target not been found
            while (!isFound)
            {
                (isFound, ret) = waitFunc();

                if (!isFound)
                {
                    // test if we should exit and throw exception
                    if (DateTime.Now > stopWaiting)
                    {
                        throw new Exception($"{targetName} was not found in time!");
                    }
                    else
                    {
                        // put thread to sleep before iterating
                        engine.ReportProgress($"{targetName} Not Yet Found... {(int)((stopWaiting - DateTime.Now).TotalSeconds)}s remain");
                        System.Threading.Thread.Sleep(1000);
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// wait process, this method can check timeout in callback Func
        /// </summary>
        /// <param name="pauseTime"></param>
        /// <param name="targetName"></param>
        /// <param name="waitFunc">1st arg Func<bool> is check timeout method</bool></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static object WaitProcess(int pauseTime, string targetName, Func<Func<bool>, (bool, object)> waitFunc, Engine.AutomationEngineInstance engine)
        {
            var stopWaiting = DateTime.Now.AddSeconds(pauseTime);
            
            // check timeout
            bool CheckTimeout()
            {
                return (DateTime.Now > stopWaiting);
            }

            object ret = null;
            var isFound = false;

            // while target not been found
            while (!isFound)
            {
                (isFound, ret) = waitFunc(CheckTimeout);

                if (!isFound)
                {
                    // test if we should exit and throw exception
                    if (CheckTimeout())
                    {
                        throw new Exception($"{targetName} was not found in time!");
                    }
                    else
                    {
                        // put thread to sleep before iterating
                        engine.ReportProgress($"{targetName} Not Yet Found... {(int)((stopWaiting - DateTime.Now).TotalSeconds)}s remain");
                        System.Threading.Thread.Sleep(1000);
                    }
                }
            }

            return ret;
        }
    }
}
