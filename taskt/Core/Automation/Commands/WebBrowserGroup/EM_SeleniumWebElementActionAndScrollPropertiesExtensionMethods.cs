using OpenQA.Selenium;
using System;

namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    public static class EM_SeleniumWebElementActionAndScrollPropertiesExtensionMethods
    {
        /// <summary>
        /// WebElement Action and Scroll
        /// </summary>
        /// <param name="command"></param>
        /// <param name="actionFunc"></param>
        /// <param name="engine"></param>
        /// <param name="errorFunc"></param>
        public static void WebElementActionAndScroll(this ISeleniumWebElementActionAndScrollProperties command, Action<IWebElement, IWebDriver> actionFunc, Engine.AutomationEngineInstance engine, Action<Exception> errorFunc = null)
        {
            command.WebElementAction(new Action<IWebElement, IWebDriver>((el, dr) =>
            {
                var cmd = command.ToScriptCommand();
                if (cmd.ExpandValueOrUserVariableAsYesNo(nameof(command.v_ScrollToWebElement), engine))
                {
                    var scroll = new SeleniumBrowserScrollToWebElementCommand()
                    {
                        v_WebElement = command.v_WebElement,
                        v_WhenFailAction = command.v_WebElement,
                    };
                    scroll.RunCommand(engine);
                }
                actionFunc(el, dr);
            }), engine, errorFunc);
        }
    }
}
