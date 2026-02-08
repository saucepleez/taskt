using OpenQA.Selenium;

namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    public static class EM_SeleniumWebElementResultPropertiesExtensionMethods
    {
        /// <summary>
        /// store WebElement in User variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="elem"></param>
        /// <param name="seleniumInstance"></param>
        /// <param name="engine"></param>
        public static void StoreWebElementInUserVariable(this ISeleniumWebElementResultProperties command, IWebElement elem, IWebDriver seleniumInstance, Engine.AutomationEngineInstance engine)
        {
            (elem, seleniumInstance).StoreInUserVariable(engine, command.v_Result);
        }
    }
}
