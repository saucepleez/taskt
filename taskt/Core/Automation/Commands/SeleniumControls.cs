using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace taskt.Core.Automation.Commands
{
    internal class SeleniumControls
    {
        public static IWebDriver getWebBrowserInstance(Automation.Engine.AutomationEngineInstance engine, string instanceName)
        {
            return (IWebDriver)engine.GetAppInstance(instanceName);
        }

        public static object findElement(IWebDriver seleniumInstance, string searchParameter, string searchMethod)
        {
            object element = null;

            switch (searchMethod.ToLower())
            {
                case "find element by xpath":
                    element = seleniumInstance.FindElement(By.XPath(searchParameter));
                    break;

                case "find element by id":
                    element = seleniumInstance.FindElement(By.Id(searchParameter));
                    break;

                case "find element by name":
                    element = seleniumInstance.FindElement(By.Name(searchParameter));
                    break;

                case "find element by tag name":
                    element = seleniumInstance.FindElement(By.TagName(searchParameter));
                    break;

                case "find element by class name":
                    element = seleniumInstance.FindElement(By.ClassName(searchParameter));
                    break;

                case "find element by css selector":
                    element = seleniumInstance.FindElement(By.CssSelector(searchParameter));
                    break;

                case "find element by link text":
                    element = seleniumInstance.FindElement(By.LinkText(searchParameter));
                    break;

                case "find elements by xpath":
                    element = seleniumInstance.FindElements(By.XPath(searchParameter));
                    break;

                case "find elements by id":
                    element = seleniumInstance.FindElements(By.Id(searchParameter));
                    break;

                case "find elements by name":
                    element = seleniumInstance.FindElements(By.Name(searchParameter));
                    break;

                case "find elements by tag name":
                    element = seleniumInstance.FindElements(By.TagName(searchParameter));
                    break;

                case "find elements by class name":
                    element = seleniumInstance.FindElements(By.ClassName(searchParameter));
                    break;

                case "find elements by css selector":
                    element = seleniumInstance.FindElements(By.CssSelector(searchParameter));
                    break;

                case "find elements by link text":
                    element = seleniumInstance.FindElements(By.LinkText(searchParameter));
                    break;

                default:
                    break;
            }

            return element;
        }

        public static string getAttribute(IWebElement element, string attributeName)
        {
            if (string.IsNullOrEmpty(attributeName))
            {
                throw new Exception("Attribute Name is empty.");
            }

            switch (attributeName.ToLower())
            {
                case "enabled":
                    return element.Enabled.ToString();
                    break;

                case "displayed":
                    return element.Displayed.ToString();
                    break;

                case "selected":
                    return element.Selected.ToString();
                    break;

                case "text":
                    return element.Text;
                    break;

                case "tag":
                case "tag name":
                case "tagname":
                    return element.TagName;
                    break;

                case "location":
                    System.Drawing.Point lc = element.Location;
                    return lc.X.ToString() + "," + lc.Y.ToString();
                    break;

                case "size":
                    System.Drawing.Size sz = element.Size;
                    return sz.Width.ToString() + "," + sz.Height.ToString();
                    break;

                default:
                    return element.GetAttribute(attributeName);
                    break;
            }
        }
    }
}
