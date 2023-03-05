using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.SubGruop("Navigate")]
    [Attributes.ClassAttributes.CommandSettings("Navigate Back")]
    [Attributes.ClassAttributes.Description("This command allows you to navigate backwards in a Selenium web browser session.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to simulate a back click in the web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SeleniumBrowserNavigateBackCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        public SeleniumBrowserNavigateBackCommand()
        {
            //this.CommandName = "SeleniumBrowserNavigateBackCommand";
            //this.SelectionName = "Navigate Back";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var seleniumInstance = v_InstanceName.GetSeleniumBrowserInstance(engine);

            seleniumInstance.Navigate().Back();
        }
    }
}