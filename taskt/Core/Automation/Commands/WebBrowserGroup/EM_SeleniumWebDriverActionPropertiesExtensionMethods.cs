using OpenQA.Selenium;
using System;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    public static class EM_SeleniumWebDriverActionPropertiesExtensionMethods
    {
        /// <summary>
        /// WebDriver action
        /// </summary>
        /// <param name="command"></param>
        /// <param name="actionFunc">func(IWebDriver, profilePath)</param>
        /// <param name="engine"></param>
        /// <param name="errorFunc"></param>
        public static void WebDriverAction(this ISeleniumWebDriverActionProperties command, Action<IWebDriver, string> actionFunc, AutomationEngineInstance engine, Action<Exception> errorFunc = null)
        {
            try
            {
                command.WebDriverActionCore(actionFunc, engine, errorFunc);
            }
            catch (Exception ex)
            {
                switch (command.ToScriptCommand().ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_WhenFailAction), engine))
                {
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

                    case "ignore":
                        break;
                }
            }

            //try
            //{
            //    (var driver, string prof) = command.ExpandValueOrUserVariableAsWebBrowserInstanceAndProfilePath(engine);
            //    actionFunc(driver, prof);
            //}
            //catch (Exception ex) 
            //{
            //    switch(command.ToScriptCommand().ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_WhenFailAction), engine))
            //    {
            //        case "error":
            //            if (errorFunc != null)
            //            {
            //                errorFunc(ex);
            //            }
            //            else
            //            {
            //                throw ex;
            //            }
            //            break;

            //        case "ignore":
            //            break;
            //    }
            //}
        }

        /// <summary>
        /// WebDriver action, not used profile path
        /// </summary>
        /// <param name="command"></param>
        /// <param name="actionFunc"></param>
        /// <param name="engine"></param>
        /// <param name="errorFunc"></param>
        public static void WebDriverAction(this ISeleniumWebDriverActionProperties command, Action<IWebDriver> actionFunc, AutomationEngineInstance engine, Action<Exception> errorFunc = null)
        {
            command.WebDriverAction(new Action<IWebDriver, string>((driver, p) =>
            {
                actionFunc(driver);
            }), engine, errorFunc);
        }
    }
}
