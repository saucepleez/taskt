using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.WebBrowserGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Search WebElement")]
    [Attributes.ClassAttributes.CommandSettings("Check WebElement Exists")]
    [Attributes.ClassAttributes.Description("This command allows you to check WebElement existance.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to check WebElement existance.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserCheckWebElementExistsCommand : ASeleniumSearchWebElementFromWebDriverCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        //public string v_InstanceName { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_SearchMethod))]
        //public string v_SearchMethod { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_SearchParameter))]
        //public string v_SearchParameter { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_ElementIndex))]
        //public string v_WebElementIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(BooleanControls), nameof(BooleanControls.v_Result))]
        [Remarks("When WebElement exists, Result is **True**")]
        [PropertyParameterOrder(8000)]
        public override string v_Result { get; set; }

        [XmlAttribute]
        [PropertyValidationRule("Wait Time", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyIsOptional(true, "0")]
        [PropertyFirstValue("0")]
        public override string v_WaitTimeForWebElement { get; set; }

        public SeleniumBrowserCheckWebElementExistsCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //try
            //{
            //    SeleniumBrowserControls.ExpandValueOrUserVariableAsSeleniumBrowserInstanceAndWebElement(this, nameof(v_InstanceName), nameof(v_SearchMethod), nameof(v_SearchParameter), nameof(v_WebElementIndex), nameof(v_WaitTimeForWebElement), engine);
            //    true.StoreInUserVariable(engine, v_Result);
            //}
            //catch
            //{
            //    false.StoreInUserVariable(engine, v_Result);
            //}
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