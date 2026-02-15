using OpenQA.Selenium;

namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    public static class EM_CanExecuteJavaScriptToWebDriverExtensionMethods
    {
        /// <summary>
        /// execute JavaScript
        /// </summary>
        /// <param name="command"></param>
        /// <param name="seleniumInstance"></param>
        /// <param name="script"></param>
        /// <returns></returns>
        public static object ExecuteJavaScript(this ICanExecuteJavaScriptToWebDriver command, IWebDriver seleniumInstance, string script)
        {
            var js = seleniumInstance as IJavaScriptExecutor;
            return js.ExecuteScript(script);
        }
    }
}
