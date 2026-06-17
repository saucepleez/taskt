using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.WebBrowserGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Search WebElement From WebElement")]
    [Attributes.ClassAttributes.CommandSettings("Wait For WebElement To Exists From WebElement")]
    [Attributes.ClassAttributes.Description("This command allows you to Wait for WebElement exists from WebElement.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Wait for WebElement exists from WebElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserWaitForWebElementToExistsFromWebElementCommand : ASeleniumDoSomethingToWebElementCommands, ISeleniumSearchWebElementParametersProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_SearchMethod))]
        [PropertyParameterOrder(6000)]
        public string v_SearchMethod { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_SearchParameter))]
        [PropertyParameterOrder(6100)]
        public string v_SearchParameter { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_SelectionMethod))]
        [PropertyParameterOrder(6200)]
        public string v_SelectionMethod { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_WebElementIndex))]
        [PropertyParameterOrder(6300)]
        public string v_WebElementIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_WaitTimeForWebElement))]
        [PropertyParameterOrder(10000)]
        public string v_WaitTimeForWebElement { get; set; }

        public SeleniumBrowserWaitForWebElementToExistsFromWebElementCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.WebElementActionCore(new Action<OpenQA.Selenium.IWebElement, OpenQA.Selenium.IWebDriver>((el, dr) =>
            {
                this.SearchWebElement(el, engine);
            }), engine);
        }
    }
}