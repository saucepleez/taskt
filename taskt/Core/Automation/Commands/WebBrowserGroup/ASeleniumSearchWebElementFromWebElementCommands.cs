using OpenQA.Selenium;
using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    public abstract class ASeleniumSearchWebElementFromWebElementCommands : ASeleniumSearchMultiWebElementsFromWebElementCommands, ISeleniumSearchWebElementParametersProperties
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
            this.WebElementActionCore(new Action<IWebElement, IWebDriver>((el, dr) =>
            {
                var elem = this.SearchWebElement(el, engine);
                actionFunc(elem);
            }), engine);
        }
    }
}
