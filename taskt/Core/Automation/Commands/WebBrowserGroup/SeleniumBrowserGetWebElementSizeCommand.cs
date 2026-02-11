using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.WebBrowserGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Get From WebElement")]
    [Attributes.ClassAttributes.CommandSettings("Get WebElement Size")]
    [Attributes.ClassAttributes.Description("This command allows you to Get WebElement Size.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Get WebElement Size.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumWebElementSizeCommand : ASeleniumGetFromWebElementCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputWebElementName))]
        //public string v_WebElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Width")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Width", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Width")]
        [PropertyParameterOrder(6000)]
        public string v_Width { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Height")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Height", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Height")]
        [PropertyParameterOrder(6100)]
        public string v_Height { get; set; }

        public SeleniumWebElementSizeCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var elem = v_WebElement.ExpandUserVariableAsWebElement("WebElement", engine);

            //if (!string.IsNullOrEmpty(v_Width))
            //{
            //    elem.Size.Width.StoreInUserVariable(engine, v_Width);
            //}
            //if (!string.IsNullOrEmpty(v_Height))
            //{
            //    elem.Size.Height.StoreInUserVariable(engine, v_Height);
            //}

            this.GetFromWebElementAction(new Action<OpenQA.Selenium.IWebElement, OpenQA.Selenium.IWebDriver>((elem, seleniumInstance) =>
            {
                if (!string.IsNullOrEmpty(v_Width))
                {
                    elem.Size.Width.StoreInUserVariable(engine, v_Width);
                }
                if (!string.IsNullOrEmpty(v_Height))
                {
                    elem.Size.Height.StoreInUserVariable(engine, v_Height);
                }
            }), new Action<Engine.AutomationEngineInstance>(e =>
            {
                if (!string.IsNullOrEmpty(v_Width))
                {
                    string.Empty.StoreInUserVariable(engine, v_Width);
                }
                if (!string.IsNullOrEmpty(v_Height))
                {
                    string.Empty.StoreInUserVariable(engine, v_Height);
                }
            }), engine);
        }
    }
}