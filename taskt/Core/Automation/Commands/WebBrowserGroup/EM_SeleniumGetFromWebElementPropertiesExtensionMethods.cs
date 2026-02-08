using OpenQA.Selenium;
using System;

namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    public static class EM_SeleniumGetFromWebElementPropertiesExtensionMethods
    {
        /// <summary>
        /// get from WebElement action
        /// </summary>
        /// <param name="command"></param>
        /// <param name="actionFunc"></param>
        /// <param name="valueErrorFunc"></param>
        /// <param name="engine"></param>
        public static void GetFromWebElementAction(this ISeleniumGetFromWebElementProperties command, Action<IWebElement, IWebDriver> actionFunc, Action<Engine.AutomationEngineInstance> emptyValueFunc, Engine.AutomationEngineInstance engine)
        {
            try
            {
                command.WebElementActionAndScroll(actionFunc, engine);
            }
            catch (Exception ex)
            {
                var script = command.ToScriptCommand();
                switch(script.ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_WebElement), engine))
                {
                    case "set empty value":
                        emptyValueFunc(engine);
                        return;

                    case "error":
                        throw ex;
                }
                
                switch(script.ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_WhenFailAction), engine))
                {
                    case "error":
                        throw ex;
                }
            }
        }
    }
}
