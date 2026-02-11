using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    public static class EM_SeleniumSearchParametersCorePropertiesExtensionMethods
    {
        /// <summary>
        /// get WebElement search func
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Func<ISearchContext, string, IReadOnlyCollection<IWebElement>> GetSearchMethodFunc(this ISeleniumSearchWebElementParametersCoreProperties command, AutomationEngineInstance engine)
        {
            switch (command.ToScriptCommand().ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_SearchMethod), engine))
            {
                case "find element by xpath":
                    return new Func<ISearchContext, string, IReadOnlyCollection<IWebElement>>((webDriver, parameter) =>
                    {
                        var elem = webDriver.FindElement(By.XPath(parameter));
                        return new ReadOnlyCollection<IWebElement>(new List<IWebElement>() { elem });
                    });

                case "find element by id":
                    return new Func<ISearchContext, string, IReadOnlyCollection<IWebElement>>((webDriver, parameter) =>
                    {
                        var elem = webDriver.FindElement(By.Id(parameter));
                        return new ReadOnlyCollection<IWebElement>(new List<IWebElement>() { elem });
                    });

                case "find element by name":
                    return new Func<ISearchContext, string, IReadOnlyCollection<IWebElement>>((webDriver, parameter) =>
                    {
                        var elem = webDriver.FindElement(By.Name(parameter));
                        return new ReadOnlyCollection<IWebElement>(new List<IWebElement>() { elem });
                    });

                case "find element by tag name":
                    return new Func<ISearchContext, string, IReadOnlyCollection<IWebElement>>((webDriver, parameter) =>
                    {
                        var elem = webDriver.FindElement(By.TagName(parameter));
                        return new ReadOnlyCollection<IWebElement>(new List<IWebElement>() { elem });
                    });

                case "find element by class name":
                    return new Func<ISearchContext, string, IReadOnlyCollection<IWebElement>>((webDriver, parameter) =>
                    {
                        var elem = webDriver.FindElement(By.ClassName(parameter));
                        return new ReadOnlyCollection<IWebElement>(new List<IWebElement>() { elem });
                    });

                case "find element by css selector":
                    return new Func<ISearchContext, string, IReadOnlyCollection<IWebElement>>((webDriver, parameter) =>
                    {
                        var elem = webDriver.FindElement(By.CssSelector(parameter));
                        return new ReadOnlyCollection<IWebElement>(new List<IWebElement>() { elem });
                    });

                case "find element by link text":
                    return new Func<ISearchContext, string, IReadOnlyCollection<IWebElement>>((webDriver, parameter) =>
                    {
                        var elem = webDriver.FindElement(By.LinkText(parameter));
                        return new ReadOnlyCollection<IWebElement>(new List<IWebElement>() { elem });
                    });

                case "find elements by xpath":
                    return new Func<ISearchContext, string, IReadOnlyCollection<IWebElement>>((webDriver, parameter) =>
                    {
                        return webDriver.FindElements(By.XPath(parameter));
                    });

                case "find elements by id":
                    return new Func<ISearchContext, string, IReadOnlyCollection<IWebElement>>((webDriver, parameter) =>
                    {
                        return webDriver.FindElements(By.Id(parameter));
                    });

                case "find elements by name":
                    return new Func<ISearchContext, string, IReadOnlyCollection<IWebElement>>((webDriver, parameter) =>
                    {
                        return webDriver.FindElements(By.Name(parameter));
                    });

                case "find elements by tag name":
                    return new Func<ISearchContext, string, IReadOnlyCollection<IWebElement>>((webDriver, parameter) =>
                    {
                        return webDriver.FindElements(By.TagName(parameter));
                    });

                case "find elements by class name":
                    return new Func<ISearchContext, string, IReadOnlyCollection<IWebElement>>((webDriver, parameter) =>
                    {
                        return webDriver.FindElements(By.ClassName(parameter));
                    });

                case "find elements by css selector":
                    return new Func<ISearchContext, string, IReadOnlyCollection<IWebElement>>((webDriver, parameter) =>
                    {
                        return webDriver.FindElements(By.CssSelector(parameter));
                    });

                case "find elements by link text":
                    return new Func<ISearchContext, string, IReadOnlyCollection<IWebElement>>((webDriver, parameter) =>
                    {
                        return webDriver.FindElements(By.LinkText(parameter));
                    });

                default:
                    throw new Exception($"Strange Search Method '{command.v_SearchMethod}'");
            }
        }

        /// <summary>
        /// search multi WebElements action
        /// </summary>
        /// <param name="command"></param>
        /// <param name="root"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<IWebElement> SearchMultiWebElementsAction(this ISeleniumSearchWebElementParametersCoreProperties command, ISearchContext root, AutomationEngineInstance engine)
        {
            var script = command.ToScriptCommand();
            var searchParameter = script.ExpandValueOrUserVariable(nameof(command.v_SearchParameter), "Search Parameter", engine);
            var waitTime = script.ExpandValueOrUserVariableAsInteger(nameof(command.v_WaitTimeForWebElement), "Wait Time", engine);

            var searchFunc = command.GetSearchMethodFunc(engine);

            var ret = WaitControls.WaitProcess(waitTime, "WebElement", new Func<(bool, object)>(() => {
                try
                {
                    var t = searchFunc(root, searchParameter);
                    if (t is ReadOnlyCollection<IWebElement> elems)
                    {
                        if (elems.Count > 0)
                        {
                            return (true, elems);
                        }
                        else
                        {
                            return (false, null);
                        }
                    }
                    else
                    {
                        return (false, null);
                    }
                }
                catch
                {
                    return (false, null);
                }
            }), engine);

            if (ret is ReadOnlyCollection<IWebElement> resultElems)
            {
                return resultElems.ToList();
            }
            else
            {
                throw new Exception($"WebElements not found. Search Method: '{command.v_SearchMethod}', Search Parameter: '{command.v_SearchParameter}'");
            }
        }
    }
}
