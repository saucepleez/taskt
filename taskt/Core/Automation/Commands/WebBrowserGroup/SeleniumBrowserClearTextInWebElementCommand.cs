using System;
using taskt.Core.Automation.Commands.WebBrowserGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("WebElement Action")]
    [Attributes.ClassAttributes.CommandSettings("Clear Text In WebElement")]
    [Attributes.ClassAttributes.Description("This command allows you to Clear Text in WebElement.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Clear Text in WebElement.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserClearTextInWebElementCommand : ASeleniumWebElementActionAndScrollCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputWebElementName))]
        //public string v_WebElement { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        //[PropertyDescription("When the WebElement does not support Clear Text")]
        //[PropertyUISelectionOption("Error")]
        //[PropertyUISelectionOption("Ignore")]
        //[PropertyIsOptional(true, "Error")]
        //[PropertyDisplayText(false, "")]
        //public string v_WhenFailAction { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_ScrollToElement))]
        //[PropertySelectionChangeEvent(nameof(cmbScrollToElement_SelectionChange))]
        //public string v_ScrollToWebElement { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        //[PropertyIsOptional(true)]
        //public string v_InstanceName { get; set; }

        public SeleniumBrowserClearTextInWebElementCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_ScrollToWebElement), engine))
            //{
            //    var scroll = new SeleniumBrowserScrollToWebElementCommand
            //    {
            //        //v_InstanceName = this.v_InstanceName,
            //        v_WebElement = this.v_WebElement,
            //        v_WhenFailAction = "ignore"
            //    };
            //    scroll.RunCommand(engine);
            //}

            //var elem = v_WebElement.ExpandUserVariableAsWebElement("WebElement", engine);

            //switch (elem.TagName.ToLower())
            //{
            //    case "input":
            //    case "textarea":
            //        elem.Clear();
            //        break;
            //    default:
            //        if (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WhenFailAction), engine) == "error")
            //        {
            //            throw new Exception("Specified WebElement does not support Clear Text. TagName: '" + elem.TagName + "'");
            //        }
            //        break;
            //}

            this.WebElementActionAndScroll(
                new Action<OpenQA.Selenium.IWebElement, OpenQA.Selenium.IWebDriver>((el, dr) =>
                {
                    switch (el.TagName.ToLower())
                    {
                        case "input":
                        case "textarea":
                            el.Clear();
                            break;
                        default:
                            //if (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WhenFailAction), engine) == "error")
                            //{
                            //    throw new Exception($"Specified WebElement does not support Clear Text. TagName: '" + elem.TagName + "'");
                            //}
                            throw new Exception();
                            //break;
                    }
                }), engine,
                new Action<Exception>(ex =>
                {
                    throw new Exception(EM_SeleniumWebElementActionPropertiesExtensionMehtods.GetFailActionMessage("Clear Text"));
                })
            );
        }

        //private void cmbScrollToElement_SelectionChange(object sender, EventArgs e)
        //{
        //    SeleniumBrowserControls.ScrollToWebElement_SelectionChange((ComboBox)sender, ControlsList, nameof(v_InstanceName));
        //}
    }
}