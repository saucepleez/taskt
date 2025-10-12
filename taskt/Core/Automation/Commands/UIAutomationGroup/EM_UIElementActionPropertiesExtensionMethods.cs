using System;
using System.Windows.Automation;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    public static class EM_UIElementActionPropertiesExtensionMethods
    {
        /// <summary>
        /// wait process
        /// </summary>
        /// <param name="t"></param>
        private static void WaitProcess(decimal t)
        {
            if (t > 0)
            {
                int waitTime = (int)(t * 1000);
                System.Threading.Thread.Sleep(waitTime);
            }
        }

        /// <summary>
        /// wait before action
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        private static void WaitBeforeAction(this IUIElementActionProperties command, AutomationEngineInstance engine)
        {
            if (!string.IsNullOrEmpty(command.v_WaitTimeBeforeAction))
            {
                var wait = command.ToScriptCommand().ExpandValueOrUserVariableAsDecimal(nameof(command.v_WaitTimeBeforeAction), engine);
                WaitProcess(wait);
            }
        }

        /// <summary>
        /// wait after action
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        private static void WaitAfterAction(this IUIElementActionProperties command, AutomationEngineInstance engine)
        {
            if (!string.IsNullOrEmpty(command.v_WaitTimeAfterAction))
            {
                var wait = command.ToScriptCommand().ExpandValueOrUserVariableAsDecimal(nameof(command.v_WaitTimeAfterAction), engine);
                WaitProcess(wait);
            }
        }

        /// <summary>
        /// UIElement action and wait
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <param name="actionFunc"></param>
        public static void UIElementActionAndWait(this IUIElementActionProperties command, AutomationEngineInstance engine, Action<AutomationElement, IntPtr> actionFunc)
        {
            var targetElement = command.ExpandUserVariableAsUIElement(engine);

            // before action
            command.WaitBeforeAction(engine);

            // get window name and window handle
            (var windowName, var whnd) = EM_CanHandleUIElementExtentionMethods.GetWindowNameAndHandle(targetElement);

            // activate before action
            if (command.ToScriptCommand().ExpandValueOrUserVariableAsYesNo(nameof(command.v_ActivateWindowBeforeAction), engine))
            {
                var activateWindow = new ActivateWindowByWindowHandleCommand()
                {
                    v_WindowHandle = whnd.ToString(),
                };
                activateWindow.RunCommand(engine);
            }

            // core process
            actionFunc(targetElement, whnd);

            // wait after action
            command.WaitAfterAction(engine);

            // store window name, handle
            command.StoreWindowNameResultInUserVariable(windowName, engine);
            command.StoreWindowHandleResultInUserVariable(whnd, engine);
        }
    }
}
