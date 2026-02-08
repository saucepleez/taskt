using OpenQA.Selenium;
using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    /// <summary>
    /// for get Window And Tab information commands
    /// </summary>
    public abstract class ASeleniumGetWindowAndTabInformationCommands : ASeleniumGetFromWebDriverCommands
    {
        [XmlAttribute]
        [PropertyParameterOrder(6000)]
        public override abstract string v_Result { get; set; }

        /// <summary>
        /// selenium window and tab action
        /// </summary>
        /// <param name="driver">Action(handle)</param>
        protected static void SeleniumWindowAndTabAction(IWebDriver driver, Action<string> actionFunc)
        {
            var currentHandle = driver.CurrentWindowHandle;

            var handles = driver.WindowHandles;
            foreach (var handle in handles)
            {
                driver.SwitchTo().Window(handle);

                actionFunc(handle);
            }

            driver.SwitchTo().Window(currentHandle);
        }
    }
}
