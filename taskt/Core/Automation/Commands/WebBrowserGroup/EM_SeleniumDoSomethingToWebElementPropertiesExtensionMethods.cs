using OpenQA.Selenium;
using System;

namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    public static class EM_SeleniumDoSomethingToWebElementPropertiesExtensionMethods
    {
        /// <summary>
        /// expand variable as WebElement-WebDriver
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static (IWebElement, IWebDriver) ExpandUserVariableAsWebElementAndWebDriver(this ISeleniumDoSomethingToWebElementProperties command, Engine.AutomationEngineInstance engine)
        {
            //var variableName = command.v_WebElement;
            //var v = variableName.GetRawVariable(engine);
            //// DBG
            ////Console.WriteLine(v.VariableValue.GetType().FullName);

            //if (IsWebElementAndWebDriverTuple(v.VariableValue, out ValueTuple<IWebElement, IWebDriver> elemDriv))
            //{
            //    return elemDriv;
            //}
            //else
            //{
            //    throw new Exception($"Variable '{variableName}' is not WebElement-WebDriver tuple");
            //}

            return command.ExpandUserVariableAsWebElementAndWebDriver(command.v_WebElement, "WebElement", engine);
        }

        ///// <summary>
        ///// expand user variable as WebElement
        ///// </summary>
        ///// <param name="command"></param>
        ///// <param name="engine"></param>
        ///// <returns></returns>
        //public static IWebElement ExpandUserVariableAsWebElement(this ISeleniumDoSomethingToWebElementProperties command, Engine.AutomationEngineInstance engine)
        //{
        //    (var e, _) = command.ExpandUserVariableAsWebElementAndWebDriver(engine);
        //    return e;
        //}

        ///// <summary>
        ///// expand user variable as WebDriver from WebElement
        ///// </summary>
        ///// <param name="command"></param>
        ///// <param name="engine"></param>
        ///// <returns></returns>
        //public static IWebDriver ExpandUserVariableAsWebDriverFromWebElement(this ISeleniumDoSomethingToWebElementProperties command, Engine.AutomationEngineInstance engine)
        //{
        //    (_, var d) = command.ExpandUserVariableAsWebElementAndWebDriver(engine);
        //    return d;
        //}

        ///// <summary>
        ///// check object is WebElement-WebDriver tuple
        ///// </summary>
        ///// <param name="v"></param>
        ///// <param name="ret"></param>
        ///// <returns></returns>
        //private static bool IsWebElementAndWebDriverTuple(object v, out ValueTuple<IWebElement, IWebDriver> ret)
        //{
        //    if (v is ValueTuple<IWebElement, IWebDriver> t)
        //    {
        //        ret = t;
        //        return true;
        //    }
        //    else
        //    {
        //        ret = (null, null);
        //        return false;
        //    }
        //}

        /// <summary>
        /// WebElement action (not use try-catch)
        /// </summary>
        /// <param name="command"></param>
        /// <param name="actionFunc"></param>
        /// <param name="engine"></param>
        public static void WebElementActionCore(this ISeleniumDoSomethingToWebElementProperties command, Action<IWebElement, IWebDriver> actionFunc, Engine.AutomationEngineInstance engine)
        {
            (var el, var dr) = command.ExpandUserVariableAsWebElementAndWebDriver(engine);

            actionFunc(el, dr);
        }
    }
}
