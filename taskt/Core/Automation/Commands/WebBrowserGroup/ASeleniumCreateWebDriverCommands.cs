using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    /// <summary>
    /// for create WebDriver (WebBrowser instance) commands
    /// </summary>
    public abstract class ASeleniumCreateWebDriverCommands : ASeleniumDoSomethingToWebDriverCommands, ISeleniumCreateWebDriverProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_OutputInstanceName))]
        //[PropertyParameterOrder(5000)]
        public override string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Web Browser Type")]
        [PropertyUISelectionOption("Edge")]
        [PropertyUISelectionOption("Chrome")]
        [PropertyUISelectionOption("Firefox")]
        [PropertyUISelectionOptionBehavior(MultiAttributesBehavior.Merge)]
        [PropertyIsOptional(true, "Chrome")]
        [PropertyFirstValue("Chrome")]
        [PropertyDisplayText(true, "Web Browser Type")]
        [PropertyParameterOrder(6000)]
        public virtual string v_BrowserType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_OutputWindowHandle))]
        [PropertyParameterOrder(11000)]
        public virtual string v_Handle { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Web Driver Binary Path")]
        [InputSpecification("Web Driver Binary Path", true)]
        [PropertyDetailSampleUsage("**C:\\temp\\WebDriverPath.exe**", PropertyDetailSampleUsage.ValueType.Value, "WebDriver Path")]
        [PropertyDetailSampleUsage("**{{{vBrowserPath}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "WebDriver Path")]
        [Remarks("When path is Empty, taskt uses default WebDriver.\nIE is not supported.\nIf you use a fixed web browser version, use this parameter.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [PropertyIsOptional(true, "Empty")]
        [PropertyDisplayText(false, "Web Driver Binary")]
        [PropertyParameterOrder(13000)]
        public virtual string v_WebDriverPath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Hide Terminal Window")]
        [PropertyIsOptional(true, "No")]
        [PropertyFirstValue("No")]
        [PropertyDisplayText(false, "Hide Terminal")]
        [PropertyParameterOrder(14000)]
        public virtual string v_HideTerminalWindow { get; set; }
    }
}
