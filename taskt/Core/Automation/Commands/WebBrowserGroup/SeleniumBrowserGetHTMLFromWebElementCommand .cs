using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.WebBrowserGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Get From WebElement")]
    [Attributes.ClassAttributes.CommandSettings("Get HTML From WebElement")]
    [Attributes.ClassAttributes.Description("This command allows you to Get HTML from WebElement.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Get HTML from WebElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserGetHTMLFromWebElementCommand : ASeleniumGetOneResultFromWebElementCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputWebElementName))]
        //public string v_WebElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyParameterOrder(6000)]
        public override string v_Result { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_ScrollToElement))]
        //public string v_ScrollToWebElement { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        //[PropertyIsOptional(true)]
        //public string v_InstanceName { get; set; }

        public SeleniumBrowserGetHTMLFromWebElementCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var getAttribute = new SeleniumBrowserGetAttributeFromWebElementCommand()
            {
                v_WebElement = this.v_WebElement,
                v_AttributeName = "outerHTML",
                v_Result = this.v_Result,
                v_ScrollToWebElement = this.v_ScrollToWebElement,
                v_WhenFailAction = this.v_WhenFailAction,
                v_WhenValueCanNotRetrieved = this.v_WhenValueCanNotRetrieved,
            };
            getAttribute.RunCommand(engine);
        }
    }
}