using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.SubGruop("WebElement Action")]
    [Attributes.ClassAttributes.CommandSettings("Clear Text In WebElement")]
    [Attributes.ClassAttributes.Description("This command allows you to Clear Text in WebElement.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Clear Text in WebElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SeleniumBrowserClearTextInWebElementCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputWebElementName))]
        public string v_WebElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("When the WebElement does not support Clear Text")]
        [PropertyUISelectionOption("Error")]
        [PropertyUISelectionOption("Ignore")]
        [PropertyIsOptional(true, "Error")]
        [PropertyDisplayText(false, "")]
        public string v_WhenClearNotSupported { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionControls), nameof(SelectionControls.v_YesNoComboBox))]
        [PropertyDescription("Try Scroll To the WebElement")]
        [PropertyIsOptional(false, "No")]
        public string v_ScrollToElement { get; set; }

        public SeleniumBrowserClearTextInWebElementCommand()
        {
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            if (this.GetYesNoSelectionValue(nameof(v_ScrollToElement), engine))
            {
                var scroll = new SeleniumBrowserScrollToWebElementCommand
                {
                    v_WebElement = this.v_WebElement,
                    v_WhenFailScroll = "ignore"
                };
                scroll.RunCommand(engine);
            }

            var elem = v_WebElement.ConvertToUserVariableAsWebElement("WebElement", engine);

            switch (elem.TagName.ToLower())
            {
                case "input":
                case "textarea":
                    elem.Clear();
                    break;
                default:
                    if (this.GetUISelectionValue(nameof(v_WhenClearNotSupported), engine) == "error")
                    {
                        throw new Exception("Specified WebElement does not support Clear Text. TagName: '" + elem.TagName + "'");
                    }
                    break;
            }
        }
    }
}