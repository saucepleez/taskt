using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.WebBrowserGroup;

namespace taskt.Core.Automation.Commands
{
    public abstract class ASeleniumGetWebElementSelectorFromWebElementCommands : ASeleniumGetOneResultFromWebElementCommands
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyParameterOrder(6000)]
        public override string v_Result { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Believe ID Attribute")]
        [PropertyIsOptional(true, "No")]
        [PropertyDisplayText(false, "Believe ID")]
        [PropertyParameterOrder(7000)]
        public virtual string v_BeliveIDAttribute { get; set; }
    }
}