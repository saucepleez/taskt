using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.SubGruop("WebElement Action")]
    [Attributes.ClassAttributes.CommandSettings("Switch Frame To WebElement")]
    [Attributes.ClassAttributes.Description("This command allows you to Switch Frame to WebElement.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Switch Frame to WebElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SeleniumBrowserSwitchFrameToWebElementCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputWebElementName))]
        public string v_WebElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("When Fail Switch")]
        [PropertyUISelectionOption("Error")]
        [PropertyUISelectionOption("Ignore")]
        [PropertyIsOptional(true, "Error")]
        [PropertyDisplayText(false, "")]
        public string v_WhenFailSwitch { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_ScrollToElement))]
        public string v_ScrollToElement { get; set; }

        public SeleniumBrowserSwitchFrameToWebElementCommand()
        {
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            if (this.GetYesNoSelectionValue(nameof(v_ScrollToElement), engine))
            {
                var scrollCommand = new SeleniumBrowserScrollToWebElementCommand()
                {
                    v_InstanceName = this.v_InstanceName,
                    v_WebElement = this.v_WebElement,
                    v_WhenFailScroll = "ignore"
                };
                scrollCommand.RunCommand(engine);
            }

            var elem = v_WebElement.ConvertToUserVariableAsWebElement("WebElement", engine);
            switch (elem.TagName.ToLower())
            {
                case "frame":
                case "iframe":
                    break;
                default:
                    throw new Exception("WebElement is not frame or iframe");
            }

            var seleniumInstance = v_InstanceName.GetSeleniumBrowserInstance(engine);

            try
            {
                seleniumInstance.SwitchTo().Frame(elem);
            }
            catch
            {
                if (this.GetUISelectionValue(nameof(v_WhenFailSwitch), engine) == "error")
                {
                    throw new Exception("Fail Switch to Frame.");
                }
            }
        }
    }
}