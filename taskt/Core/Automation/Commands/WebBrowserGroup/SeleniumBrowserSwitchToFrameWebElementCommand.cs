using System;
using taskt.Core.Automation.Commands.WebBrowserGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("WebElement Action")]
    [Attributes.ClassAttributes.CommandSettings("Switch To Frame WebElement")]
    [Attributes.ClassAttributes.Description("This command allows you to Switch to Frame WebElement.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Switch to Frame WebElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserSwitchToFrameWebElementCommand : ASeleniumWebElementActionAndScrollCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        //public string v_InstanceName { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputWebElementName))]
        //public string v_WebElement { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        //[PropertyDescription("When Fail Switch")]
        //[PropertyUISelectionOption("Error")]
        //[PropertyUISelectionOption("Ignore")]
        //[PropertyIsOptional(true, "Error")]
        //[PropertyDisplayText(false, "")]
        //public string v_WhenFailAction { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_ScrollToElement))]
        //public string v_ScrollToWebElement { get; set; }

        public SeleniumBrowserSwitchToFrameWebElementCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_ScrollToWebElement), engine))
            //{
            //    var scrollCommand = new SeleniumBrowserScrollToWebElementCommand()
            //    {
            //        //v_InstanceName = this.v_InstanceName,
            //        v_WebElement = this.v_WebElement,
            //        v_WhenFailAction = "ignore"
            //    };
            //    scrollCommand.RunCommand(engine);
            //}

            //var elem = v_WebElement.ExpandUserVariableAsWebElement("WebElement", engine);
            //switch (elem.TagName.ToLower())
            //{
            //    case "frame":
            //    case "iframe":
            //        break;
            //    default:
            //        throw new Exception("WebElement is not frame or iframe");
            //}

            //var seleniumInstance = v_InstanceName.ExpandValueOrUserVariableAsSeleniumBrowserInstance(engine);

            //try
            //{
            //    seleniumInstance.SwitchTo().Frame(elem);
            //}
            //catch
            //{
            //    if (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WhenFailAction), engine) == "error")
            //    {
            //        throw new Exception("Fail Switch to Frame.");
            //    }
            //}

            this.WebElementActionAndScroll(
                new Action<OpenQA.Selenium.IWebElement, OpenQA.Selenium.IWebDriver>((el, dr) =>
                {
                    switch (this.TagName(el))
                    {
                        case "frame":
                        case "iframe":
                            break;
                        default:
                            throw new Exception();
                    }
                    dr.SwitchTo().Frame(el);
                }), engine,
                new Action<Exception>(ex =>
                {
                    throw new Exception(EM_SeleniumWebElementActionPropertiesExtensionMehtods.GetFailActionMessage("Switch To Frame"));
                })
            );
        }
    }
}