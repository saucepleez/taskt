using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    /// <summary>
    /// for WebDriver action commands
    /// </summary>
    public abstract class ASeleniumWebDriverActionCommands : ASeleniumDoSomethingToWebDriverCommands, ISeleniumWebDriverActionProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_InputInstanceName))]
        public override string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_ComboBoxHasErrorIgnore))]
        [PropertyDescription("When Fail Action")]
        [PropertyDisplayText(false, "When Fail Action")]
        [PropertyValidationRule("When Fail Action", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyIsOptional(true, "Error")]
        [PropertyParameterOrder(10000)]
        public virtual string v_WhenFailAction { get; set; }
    }
}
