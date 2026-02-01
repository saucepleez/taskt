using OpenQA.Selenium;

namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    public static class EM_SeleniumDoSomethingToWebBroswerInstancePropertiesExtensionMethods
    {
        /// <summary>
        /// expand value or user variable as WebBrowser Instance
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static IWebDriver ExpandValueOrUserVariableAsWebBrowserInstance(this ISeleniumDoSomethingToWebDriverProperties command, Engine.AutomationEngineInstance engine)
        {
            return command.GetWebBrowserIntance(command.v_InstanceName, engine);
        }

        /// <summary>
        /// expand value or user variable as WebBrowser Instance and profile path
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static (IWebDriver, string) ExpandValueOrUserVariableAsWebBrowserInstanceAndProfilePath(this ISeleniumDoSomethingToWebDriverProperties command, Engine.AutomationEngineInstance engine)
        {
            return command.GetWebBrowserInstanceAndProfilePath(command.v_InstanceName, engine);
        }

        /// <summary>
        /// create WebBrowser instance
        /// </summary>
        /// <param name="command"></param>
        /// <param name="driver"></param>
        /// <param name="profilePath"></param>
        /// <param name="engine"></param>
        public static void CreateWebBrowserInstance(this ISeleniumDoSomethingToWebDriverProperties command, IWebDriver driver, string profilePath, Engine.AutomationEngineInstance engine)
        {
            command.CreateWebBrowserInstance(command.v_InstanceName, driver, profilePath, engine);
        }
    }
}
