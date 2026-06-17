using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.WebBrowserGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Search WebElement")]
    [Attributes.ClassAttributes.CommandSettings("Check WebElement Exists From WebElement")]
    [Attributes.ClassAttributes.Description("This command allows you to check WebElement existance from WebElement.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to check WebElement existance from WebElement.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserCheckWebElementExistsFromWebElementCommand : ASeleniumSearchWebElementFromWebElementCommands
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(BooleanControls), nameof(BooleanControls.v_Result))]
        [Remarks("When WebElement exists, Result is **True**")]
        [PropertyParameterOrder(8000)]
        public string v_Result { get; set; }

        [XmlAttribute]
        [PropertyValidationRule("Wait Time", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyIsOptional(true, "0")]
        [PropertyFirstValue("0")]
        public override string v_WaitTimeForWebElement { get; set; }

        public SeleniumBrowserCheckWebElementExistsFromWebElementCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            try
            {
                this.SearchWebElementAction(new Action<OpenQA.Selenium.IWebElement>(elem =>
                {
                    true.StoreInUserVariable(engine, v_Result);
                }), engine);
            }
            catch
            {
                false.StoreInUserVariable(engine, v_Result);
            }
        }
    }
}