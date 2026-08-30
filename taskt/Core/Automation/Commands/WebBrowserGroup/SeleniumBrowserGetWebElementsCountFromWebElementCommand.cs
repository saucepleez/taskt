using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.WebBrowserGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Search WebElement From WebElement")]
    [Attributes.ClassAttributes.CommandSettings("Get WebElements Count From WebElement")]
    [Attributes.ClassAttributes.Description("This command allows you to count WebElements from WebElement.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to count WebElements from WebElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserGetWebElementsCountFromWebElementCommand : ASeleniumSearchMultiWebElementsFromWebElementCommands
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyParameterOrder(7000)]
        public string v_Result { get; set; }

        public SeleniumBrowserGetWebElementsCountFromWebElementCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            try
            {
                this.SearchMultiWebElementsAction(new Action<System.Collections.Generic.List<OpenQA.Selenium.IWebElement>>(elems =>
                {
                    elems.Count.StoreInUserVariable(engine, v_Result);
                }), engine);
            }
            catch
            {
                0.StoreInUserVariable(engine, v_Result);
            }
        }
    }
}