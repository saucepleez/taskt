using OpenQA.Selenium;
using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.SubGruop("WebElement Action")]
    [Attributes.ClassAttributes.CommandSettings("Scroll To WebElement")]
    [Attributes.ClassAttributes.Description("This command allows you to Scroll to WebElement.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Scroll to WebElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SeleniumBrowserScrollToWebElementCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputWebElementName))]
        public string v_WebElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("When Fail Scroll")]
        [PropertyUISelectionOption("Error")]
        [PropertyUISelectionOption("Ignore")]
        [PropertyIsOptional(true, "Error")]
        [PropertyDisplayText(false, "")]
        public string v_WhenFailScroll { get; set; }

        public SeleniumBrowserScrollToWebElementCommand()
        {
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var seleniumInstance = v_InstanceName.GetSeleniumBrowserInstance(engine);
            var elem = v_WebElement.ConvertToUserVariableAsWebElement("WebElement", engine);

            try
            {
                string scroll = string.Format("window.scroll(0, {0})", elem.Location.Y);
                //IJavaScriptExecutor js = seleniumInstance as IJavaScriptExecutor;
                //js.ExecuteScript(scroll);
                SeleniumBrowserControls.ExcecuteScript(seleniumInstance, scroll);

                // Debug
                //Console.WriteLine("JSJS");
                //Console.WriteLine(scroll);
            }
            catch
            {
                if (this.GetUISelectionValue(nameof(v_WhenFailScroll), engine) == "error")
                {
                    throw new Exception("Failed to Scroll To WebElement");
                }
            }
        }
    }
}