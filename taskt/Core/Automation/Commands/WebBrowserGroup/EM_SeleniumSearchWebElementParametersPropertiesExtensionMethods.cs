using OpenQA.Selenium;
using System;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    public static class EM_SeleniumSearchWebElementParametersPropertiesExtensionMethods
    {
        /// <summary>
        /// search WebElement
        /// </summary>
        /// <param name="command"></param>
        /// <param name="root"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static IWebElement SearchWebElement(this ISeleniumSearchWebElementParametersProperties command, ISearchContext root, AutomationEngineInstance engine)
        {
            var script = command.ToScriptCommand();

            int index = 0;
            switch(script.ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_SelectionMethod), engine))
            {
                case "first":
                    break;
                case "last":
                    index = -1;
                    break;
                case "index":
                    if (string.IsNullOrEmpty(command.v_WebElementIndex))
                    {
                        command.v_WebElementIndex = "0";
                    }
                    index = script.ExpandValueOrUserVariableAsInteger(nameof(command.v_WebElementIndex), "Index", engine);
                    break;
            }

            var elems = command.SearchMultiWebElements(root, engine);
            if (elems.Count > 0)
            {
                if (index < 0)
                {
                    index += elems.Count;
                }
                if ((index >= 0) && (index < elems.Count))
                {
                    return elems[index];
                }
                else
                {
                    throw new Exception($"WebElement not found, out of index. Search Method: '{command.v_SearchMethod}', Search Parameter: '{command.v_SearchParameter}', Index: '{command.v_WebElementIndex}', Expand Index: '{index}'");
                }
            }
            else
            {
                throw new Exception($"WebElement not found. Search Method: '{command.v_SearchMethod}', Search Parameter: '{command.v_SearchParameter}', Index: '{command.v_WebElementIndex}', Expand Index: '{index}'");
            }
        }
    }
}
