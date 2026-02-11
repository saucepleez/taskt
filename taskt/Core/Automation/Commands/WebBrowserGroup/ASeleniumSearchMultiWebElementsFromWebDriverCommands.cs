using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    /// <summary>
    /// for search multi WebElements commands
    /// </summary>
    public abstract class ASeleniumSearchMultiWebElementsFromWebDriverCommands : ASeleniumGetFromWebDriverCommands, ISeleniumSearchMultiWebElementsParametersProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_SearchMethod))]
        [PropertyParameterOrder(6000)]
        public virtual string v_SearchMethod { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_SearchParameter))]
        [PropertyParameterOrder(6100)]
        public virtual string v_SearchParameter { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_WaitTimeForWebElement))]
        [PropertyParameterOrder(10000)]
        public virtual string v_WaitTimeForWebElement { get; set; }

        /// <summary>
        /// search multi WebElements action
        /// </summary>
        /// <param name="actionFunc"></param>
        /// <param name="engine"></param>
        protected void SearchMultiWebElementsAction(Action<List<IWebElement>> actionFunc, Engine.AutomationEngineInstance engine)
        {
            this.WebDriverActionCore(new System.Action<IWebDriver>(seleniumInsntance =>
            {
                var elems = this.SearchMultiWebElements(seleniumInsntance, engine);
                actionFunc(elems);
            }), engine);
        }
    }
}
