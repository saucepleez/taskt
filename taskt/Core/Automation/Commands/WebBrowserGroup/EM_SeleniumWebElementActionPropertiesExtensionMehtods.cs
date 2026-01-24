using OpenQA.Selenium;
using System;

namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    public static class EM_SeleniumWebElementActionPropertiesExtensionMehtods
    {
        /// <summary>
        /// WebElement Action
        /// </summary>
        /// <param name="command"></param>
        /// <param name="actionFunc"></param>
        /// <param name="engine"></param>
        /// <param name="errorFunc"></param>
        public static void WebElementAction(this ISeleniumWebElementActionProperties command, Action<IWebElement, IWebDriver> actionFunc, Engine.AutomationEngineInstance engine, Action<Exception> errorFunc = null)
        {
            command.WebElementActionCore(new Action<IWebElement, IWebDriver>((el, dr) =>
            {
                try
                {
                    actionFunc(el, dr);
                }
                catch (Exception ex)
                {
                    switch(command.ToScriptCommand().ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_WhenFailAction), engine))
                    {
                        case "ignore":
                            break;
                        case "error":
                            if (errorFunc != null)
                            {
                                errorFunc(ex);
                            }
                            else
                            {
                                throw ex;
                            }
                            break;
                    }
                }
            }), engine);
        }
    }
}
