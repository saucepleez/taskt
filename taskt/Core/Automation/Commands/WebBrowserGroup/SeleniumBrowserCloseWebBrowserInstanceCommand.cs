using System;
using taskt.Core.Automation.Commands.WebBrowserGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Instance")]
    [Attributes.ClassAttributes.CommandSettings("Close Web Browser Instance")]
    [Attributes.ClassAttributes.Description("This command allows you to close a Selenium web browser session.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to close and end a web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserCloseWebBrowserInstanceCommand : ASeleniumWebDriverActionCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        //public string v_InstanceName { get; set; }

        public SeleniumBrowserCloseWebBrowserInstanceCommand()
        {
            //this.CommandName = "SeleniumBrowserCloseCommand";
            //this.SelectionName = "Close Browser";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var vInstance = v_InstanceName.ExpandValueOrUserVariable(engine);
            //var seleniumInstance = v_InstanceName.ExpandValueOrUserVariableAsSeleniumBrowserInstance(engine);

            //seleniumInstance.Quit();
            //seleniumInstance.Dispose();

            //engine.RemoveAppInstance(vInstance);

            this.WebDriverAction(new Action<OpenQA.Selenium.IWebDriver, string>((seleniumInstance, profilePath) =>
            {
                seleniumInstance.Quit();
                seleniumInstance.Dispose();

                var instanceName = this.GetInstanceNameFromWebBrowserInstance(seleniumInstance, engine);
                engine.RemoveAppInstance(instanceName);
            }), engine);
        }
    }
}