using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    /// <summary>
    /// get something from UIElement commands
    /// </summary>
    public abstract class AGetFromUIElementCommands : ADoSomethingUIElementCommands, IGetFromUIElementProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_ComboBoxHasErrorIgnoreSetEmpty))]
        [PropertyDescription("When the Value(s) can not Retrieved")]
        [PropertyDisplayText(true, "When can not Retrieved")]
        [PropertyIsOptional(true, "Error")]
        [PropertyValidationRule("When Value can not Retrieved", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyParameterOrder(10000)]
        public virtual string v_WhenValueCanNotRetrieved { get; set; }
    }
}
