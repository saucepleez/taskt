using OpenQA.Selenium;
using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    /// <summary>
    /// for search one WebElement from WebDriver commands
    /// </summary>
    public abstract class ASeleniumSearchWebElementFromWebDriverCommands : ASeleniumSearchMultiWebElementsFromWebDriverCommands, ISeleniumSearchWebElementParametersProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_SelectionMethod))]
        [PropertyParameterOrder(7000)]
        public string v_SelectionMethod { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_WebElementIndex))]
        [PropertyParameterOrder(7100)]
        public string v_WebElementIndex { get; set; }

        /// <summary>
        /// search WebElement action
        /// </summary>
        /// <param name="actionFunc"></param>
        /// <param name="engine"></param>
        protected void SearchWebElementAction(Action<IWebElement> actionFunc, Engine.AutomationEngineInstance engine)
        {
            this.WebDriverActionCore(new System.Action<IWebDriver>(seleniumInsntance =>
            {
                var elem = this.SearchWebElement(seleniumInsntance, engine);
                actionFunc(elem);
            }), engine);
        }
    }
}
