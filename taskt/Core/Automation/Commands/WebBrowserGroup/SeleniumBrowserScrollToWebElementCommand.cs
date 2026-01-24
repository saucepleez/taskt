using System;
using taskt.Core.Automation.Commands.WebBrowserGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("WebElement Action")]
    [Attributes.ClassAttributes.CommandSettings("Scroll To WebElement")]
    [Attributes.ClassAttributes.Description("This command allows you to Scroll to WebElement.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Scroll to WebElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserScrollToWebElementCommand : ASeleniumWebElementActionCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        //public string v_InstanceName { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputWebElementName))]
        //public string v_WebElement { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        //[PropertyDescription("When Fail Scroll")]
        //[PropertyUISelectionOption("Error")]
        //[PropertyUISelectionOption("Ignore")]
        //[PropertyIsOptional(true, "Error")]
        //[PropertyDisplayText(false, "")]
        //public string v_WhenFailAction { get; set; }

        public SeleniumBrowserScrollToWebElementCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var seleniumInstance = v_InstanceName.ExpandValueOrUserVariableAsSeleniumBrowserInstance(engine);
            //var elem = v_WebElement.ExpandUserVariableAsWebElement("WebElement", engine);

            //try
            //{
            //    string scroll = string.Format("window.scroll(0, {0})", elem.Location.Y);
            //    //IJavaScriptExecutor js = seleniumInstance as IJavaScriptExecutor;
            //    //js.ExecuteScript(scroll);
            //    SeleniumBrowserControls.ExcecuteScript(seleniumInstance, scroll);

            //    // Debug
            //    //Console.WriteLine("JSJS");
            //    //Console.WriteLine(scroll);
            //}
            //catch
            //{
            //    if (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WhenFailAction), engine) == "error")
            //    {
            //        throw new Exception("Failed to Scroll To WebElement");
            //    }
            //}

            this.WebElementAction(
                new Action<OpenQA.Selenium.IWebElement, OpenQA.Selenium.IWebDriver>((el, dr) =>
                {
                    var script = $"window.scroll(0, {el.Location.Y})";
                    SeleniumBrowserControls.ExcecuteScript(dr, script);
                }), engine, new Action<Exception>(ex =>
                {
                    //throw new Exception("Failed to Scroll To WebElement");
                    throw new Exception(EM_SeleniumWebElementActionPropertiesExtensionMehtods.GetFailActionMessage("Scroll"));
                })
            );
        }
    }
}