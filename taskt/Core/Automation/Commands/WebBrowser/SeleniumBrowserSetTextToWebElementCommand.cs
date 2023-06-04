using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.SubGruop("WebElement Action")]
    [Attributes.ClassAttributes.CommandSettings("Set Text To WebElement")]
    [Attributes.ClassAttributes.Description("This command allows you to Set Text in WebElement.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Set Text in WebElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SeleniumBrowserSetTextToWebElementCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputWebElementName))]
        public string v_WebElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_MultiLinesTextBox))]
        [PropertyDescription("Text To Set")]
        public string v_TextToSet { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionControls), nameof(SelectionControls.v_YesNoComboBox))]
        [PropertyDescription("Clear Text before Setting Text")]
        [PropertyIsOptional(true, "No")]
        public string v_ClearTextBeforeSetting { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionControls), nameof(SelectionControls.v_YesNoComboBox))]
        [PropertyDescription("Encrypted Text")]
        [PropertyIsOptional(true, "No")]
        public string v_EncryptedText { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("When the WebElement does not support Set Text")]
        [PropertyUISelectionOption("Error")]
        [PropertyUISelectionOption("Ignore")]
        [PropertyIsOptional(true, "Error")]
        [PropertyDisplayText(false, "")]
        public string v_WhenSetNotSupported { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_ScrollToElement))]
        [PropertySelectionChangeEvent(nameof(cmbScrollToElement_SelectionChange))]
        public string v_ScrollToElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        [PropertyIsOptional(true)]
        public string v_InstanceName { get; set; }

        public SeleniumBrowserSetTextToWebElementCommand()
        {
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            if (this.GetYesNoSelectionValue(nameof(v_ScrollToElement), engine))
            {
                var scroll = new SeleniumBrowserScrollToWebElementCommand
                {
                    v_InstanceName = this.v_InstanceName,
                    v_WebElement = this.v_WebElement,
                    v_WhenFailScroll = "ignore"
                };
                scroll.RunCommand(engine);
            }

            var elem = v_WebElement.ConvertToUserVariableAsWebElement("WebElement", engine);

            if (this.GetYesNoSelectionValue(nameof(v_ClearTextBeforeSetting), engine))
            {
                var clearText = new SeleniumBrowserClearTextInWebElementCommand
                {
                    v_WebElement = v_WebElement,
                    v_WhenClearNotSupported = "Ignore"
                };
                clearText.RunCommand(engine);
            }

            var textToSet = v_TextToSet.ConvertToUserVariable(engine);

            if (this.GetYesNoSelectionValue(nameof(v_EncryptedText), engine))
            {
                textToSet = EncryptionServices.DecryptString(textToSet, "TASKT");
            }

            try
            {
                elem.SendKeys(textToSet);
            }
            catch
            {
                if (this.GetUISelectionValue(nameof(v_WhenSetNotSupported), engine) == "error")
                {
                    throw new Exception("Fail Setting Text. TagName: '" + elem.TagName + "'");
                }
            }
        }

        private void cmbScrollToElement_SelectionChange(object sender, EventArgs e)
        {
            SeleniumBrowserControls.ScrollToWebElement_SelectionChange((ComboBox)sender, ControlsList, nameof(v_InstanceName));
        }
    }
}