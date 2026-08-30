using OpenQA.Selenium;
using System;

namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    public static class EM_CanHandleWebElementExtensionMethods
    {
        /// <summary>
        /// expand user variable as WebElement and WebDriver
        /// </summary>
        /// <param name="str"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static (IWebElement, IWebDriver) ExpandUserVariableAsWebElementAndWebDriver(this ICanHandleWebElement command, string str, string parameterName, Engine.AutomationEngineInstance engine)
        {
            var v = str.GetRawVariable(engine);

            if (IsWebElementAndWebDriverTuple(v.VariableValue, out (IWebElement, IWebDriver) webs))
            {
                return webs;
            }
            else
            {
                throw new Exception($"{parameterName} '{str}' is not a WebElement-WebDriver tuple.");
            }
        }

        /// <summary>
        /// expand user variable as WebElement
        /// </summary>
        /// <param name="str"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static IWebElement ExpandUserVariableAsWebElement(this ICanHandleWebElement command, string str, string parameterName, Engine.AutomationEngineInstance engine)
        {
            (var e, _) = command.ExpandUserVariableAsWebElementAndWebDriver(str, parameterName, engine);
            return e;
        }

        /// <summary>
        /// expand user variable as webDriver from WebElement
        /// </summary>
        /// <param name="str"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static IWebDriver ExpandUserVariableAsWebDriverFromWebElement(this ICanHandleWebElement command, string str, string parameterName, Engine.AutomationEngineInstance engine)
        {
            (_, var d) = command.ExpandUserVariableAsWebElementAndWebDriver(str, parameterName, engine);
            return d;
        }

        /// <summary>
        /// check value is WebElement-WebDriver
        /// </summary>
        /// <param name="v"></param>
        /// <param name="ret"></param>
        /// <returns></returns>
        private static bool IsWebElementAndWebDriverTuple(object v, out ValueTuple<IWebElement, IWebDriver> ret)
        {
            if (v is ValueTuple<IWebElement, IWebDriver> t)
            {
                ret = t;
                return true;
            }
            else
            {
                ret = (null, null);
                return false;
            }
        }

        /// <summary>
        /// store WebElement (and WebDriver) to user variable
        /// </summary>
        /// <param name="value"></param>
        /// <param name="engine"></param>
        /// <param name="targetVariable"></param>
        public static void StoreInUserVariable(this ICanHandleWebElement command, IWebElement elem, IWebDriver driver, Engine.AutomationEngineInstance engine, string targetVariable)
        {
            ExtensionMethods.StoreInUserVariable(targetVariable, (elem, driver), engine, false);
        }

        /// <summary>
        /// get WebElement TagName
        /// </summary>
        /// <param name="command"></param>
        /// <param name="elem"></param>
        public static string TagName(this ICanHandleWebElement command, IWebElement elem)
        {
            return elem.TagName.ToLower();
        }
    }
}
