using OpenQA.Selenium;
using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.WebBrowserGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Search WebElement")]
    [Attributes.ClassAttributes.CommandSettings("Search Shadow DOM WebElement From WebElement")]
    [Attributes.ClassAttributes.Description("This command allows you to search Shadow DOM WebElement from WebElement.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get Shadow DOM WebElement from WebElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserSearchShadowDOMWebElementFromWebElementCommand : ASeleniumDoSomethingToWebElementCommands, ISeleniumSearchWebElementParametersProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_SearchMethod))]
        [PropertyUISelectionOptionBehavior(MultiAttributesBehavior.Overwrite)]
        [PropertyUISelectionOption("Find Elements By CSS Selector")]
        [PropertyUISelectionOption("Find Element By CSS Selector")]
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
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_OutputWebElementName))]
        [PropertyParameterOrder(7000)]
        public string v_Result { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_WaitTimeForWebElement))]
        [PropertyParameterOrder(10000)]
        public string v_WaitTimeForWebElement { get; set; }

        public SeleniumBrowserSearchShadowDOMWebElementFromWebElementCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.WebElementActionCore(new Action<IWebElement, IWebDriver>((elem, seleniumInstance) =>
            {
                try
                {
                    var shadow = elem.GetShadowRoot();

                    var findElem = this.SearchWebElement(shadow, engine);
                    this.StoreInUserVariable(findElem, seleniumInstance, engine, v_Result);
                }
                catch
                {
                    throw new Exception($"WebElement does not have Shadow-Root or WebElement does not Exist. WebElement: '{v_WebElement}', Parameter: '{v_SearchParameter}'");
                }
            }), engine);
        }
    }
}