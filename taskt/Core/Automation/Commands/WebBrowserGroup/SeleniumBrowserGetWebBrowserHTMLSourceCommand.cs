using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.WebBrowserGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Get From Web Browser")]
    [Attributes.ClassAttributes.CommandSettings("Get Web Browser HTML Source")]
    [Attributes.ClassAttributes.Description("This command allows you to Get HTML Source from Web Browser.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Get HTML Source from Web Browser.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserGetWebBrowserHTMLSourceCommand : ASeleniumGetFromWebDriverCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        //public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyParameterOrder(6000)]
        public override string v_Result { get; set; }

        public SeleniumBrowserGetWebBrowserHTMLSourceCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.WebDriverActionCore(new Action<OpenQA.Selenium.IWebDriver, string>((seleniumInstance, _) =>
            {
                seleniumInstance.PageSource.StoreInUserVariable(engine, v_Result);
            }), engine);
        }
    }
}