using System;
using taskt.Core.Automation.Commands.WebBrowserGroup;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Navigate")]
    [Attributes.ClassAttributes.CommandSettings("Navigate Forward")]
    [Attributes.ClassAttributes.Description("This command allows you to navigate forward a Selenium web browser session.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to simulate a forward click in the web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserNavigateForwardCommand : ASeleniumWebDriverActionCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        //public string v_InstanceName { get; set; }

        public SeleniumBrowserNavigateForwardCommand()
        {
            //this.CommandName = "WebBrowserNavigateCommand";
            //this.SelectionName = "Navigate Forward";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var seleniumInstance = v_InstanceName.ExpandValueOrUserVariableAsSeleniumBrowserInstance(engine);

            //seleniumInstance.Navigate().Forward();

            this.WebDriverAction(new Action<OpenQA.Selenium.IWebDriver>(seleniumInstance =>
            {
                seleniumInstance.Navigate().Forward();
            }), engine);
        }
    }
}