using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.WebBrowserGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Search WebElement")]
    [Attributes.ClassAttributes.CommandSettings("Search WebElement")]
    [Attributes.ClassAttributes.Description("This command allows you to search WebElement.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get WebElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserSearchWebElementCommand : ASeleniumSearchWebElementFromWebDriverCommands, ISeleniumSearchWebElementParametersProperties, ISeleniumWebElementResultProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        //public string v_InstanceName { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_SearchMethod))]
        //[PropertyParameterOrder(6000)]
        //public string v_SearchMethod { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_SearchParameter))]
        //[PropertyParameterOrder(6100)]
        //public string v_SearchParameter { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_WebElementIndex))]
        //[PropertyParameterOrder(6200)]
        //public string v_WebElementIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_OutputWebElementName))]
        [PropertyParameterOrder(7000)]
        public override string v_Result { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_WaitTime))]
        //[PropertyParameterOrder(8000)]
        //public string v_WaitTimeForWebElement { get; set; }

        public SeleniumBrowserSearchWebElementCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //(var ins, var trgElem) = SeleniumBrowserControls.ExpandValueOrUserVariableAsSeleniumBrowserInstanceAndWebElement(this, nameof(v_InstanceName), nameof(v_SearchMethod), nameof(v_SearchParameter), nameof(v_WebElementIndex), nameof(v_WaitTimeForWebElement), engine);

            //(trgElem, ins).StoreInUserVariable(engine, v_Result);

            // DBG
            //SeleniumBrowserControls.CreateXPath(trgElem);

            this.WebDriverActionCore(new Action<OpenQA.Selenium.IWebDriver>(seleniumInstance =>
            {
                var elem = this.SearchWebElement(seleniumInstance, engine);
                this.StoreWebElementInUserVariable(elem, seleniumInstance, engine);
            }), engine);
        }
    }
}