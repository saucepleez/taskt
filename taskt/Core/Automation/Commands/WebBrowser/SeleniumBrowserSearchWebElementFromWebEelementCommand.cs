using OpenQA.Selenium;
using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.SubGruop("Search WebElement")]
    [Attributes.ClassAttributes.CommandSettings("Search WebElement From WebElement")]
    [Attributes.ClassAttributes.Description("This command allows you to search WebElement from WebElement.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get WebElement from WebElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SeleniumBrowserSearchWebElementFromWebElementCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputWebElementName))]
        public string v_WebElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_SearchMethod))]
        public string v_SeleniumSearchType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_SearchParameter))]
        public string v_SeleniumSearchParameter { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_ElementIndex))]
        public string v_ElementIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_OutputWebElementName))]
        public string v_Result { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_WaitTime))]
        public string v_WaitTime { get; set; }

        public SeleniumBrowserSearchWebElementFromWebElementCommand()
        {
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var targetElement = v_WebElement.ConvertToUserVariableAsWebElement("WebElement", engine);

            var searchMethod = this.GetUISelectionValue(nameof(v_SeleniumSearchType), engine);
            var searchFunc = GetWebElementSearchMethod(searchMethod);

            var searchParameter = v_SeleniumSearchParameter.ConvertToUserVariable(engine);
            var waitTime = v_WaitTime.ConvertToUserVariableAsInteger("Wait Time", engine);

            int index = 0;
            if (!string.IsNullOrEmpty(v_ElementIndex))
            {
                index = v_ElementIndex.ConvertToUserVariableAsInteger("Index", engine);
            }

            var ret = WaitControls.WaitProcess(waitTime, "WebElement", new Func<(bool, object)>(() => {
                try
                {
                    var t = searchFunc(targetElement, searchParameter);
                    if (t is IWebElement elem)
                    {
                        return (true, elem);
                    }
                    else if (t is ReadOnlyCollection<IWebElement> elems)
                    {
                        if (index < 0)
                        {
                            index += elems.Count;
                        }
                        if ((index >= 0) && (index < elems.Count))
                        {
                            return (true, elems[index]);
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

            if (ret is IWebElement resultElem)
            {
                resultElem.StoreInUserVariable(engine, v_Result);
            }
            else
            {
                throw new Exception("WebElement not found");
            }
        }

        private static Func<IWebElement, string, object> GetWebElementSearchMethod(string searchMethod)
        {
            switch (searchMethod.ToLower())
            {
                case "find element by xpath":
                    return new Func<IWebElement, string, object>((webDriver, parameter) =>
                    {
                        return webDriver.FindElement(By.XPath(parameter));
                    });

                case "find element by id":
                    return new Func<IWebElement, string, object>((webDriver, parameter) =>
                    {
                        return webDriver.FindElement(By.Id(parameter));
                    });

                case "find element by name":
                    return new Func<IWebElement, string, object>((webDriver, parameter) =>
                    {
                        return webDriver.FindElement(By.Name(parameter));
                    });

                case "find element by tag name":
                    return new Func<IWebElement, string, object>((webDriver, parameter) =>
                    {
                        return webDriver.FindElement(By.TagName(parameter));
                    });

                case "find element by class name":
                    return new Func<IWebElement, string, object>((webDriver, parameter) =>
                    {
                        return webDriver.FindElement(By.ClassName(parameter));
                    });

                case "find element by css selector":
                    return new Func<IWebElement, string, object>((webDriver, parameter) =>
                    {
                        return webDriver.FindElement(By.CssSelector(parameter));
                    });

                case "find element by link text":
                    return new Func<IWebElement, string, object>((webDriver, parameter) =>
                    {
                        return webDriver.FindElement(By.LinkText(parameter));
                    });

                case "find elements by xpath":
                    return new Func<IWebElement, string, object>((webDriver, parameter) =>
                    {
                        return webDriver.FindElements(By.XPath(parameter));
                    });

                case "find elements by id":
                    return new Func<IWebElement, string, object>((webDriver, parameter) =>
                    {
                        return webDriver.FindElements(By.Id(parameter));
                    });

                case "find elements by name":
                    return new Func<IWebElement, string, object>((webDriver, parameter) =>
                    {
                        return webDriver.FindElements(By.Name(parameter));
                    });

                case "find elements by tag name":
                    return new Func<IWebElement, string, object>((webDriver, parameter) =>
                    {
                        return webDriver.FindElements(By.TagName(parameter));
                    });

                case "find elements by class name":
                    return new Func<IWebElement, string, object>((webDriver, parameter) =>
                    {
                        return webDriver.FindElements(By.ClassName(parameter));
                    });

                case "find elements by css selector":
                    return new Func<IWebElement, string, object>((webDriver, parameter) =>
                    {
                        return webDriver.FindElements(By.CssSelector(parameter));
                    });

                case "find elements by link text":
                    return new Func<IWebElement, string, object>((webDriver, parameter) =>
                    {
                        return webDriver.FindElements(By.LinkText(parameter));
                    });

                default:
                    throw new Exception("Strange Search Method '" + searchMethod + "'");
            }
        }
    }
}