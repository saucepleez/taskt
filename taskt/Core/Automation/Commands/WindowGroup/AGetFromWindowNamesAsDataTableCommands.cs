using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Get***FromWindowNamesAsDataTable commands
    /// </summary>
    public abstract class GetFromWindowNamesAsDataTableCommands : AWindowNamesCommands, IDataTableResultProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        [PropertyParameterOrder(6500)]
        public virtual string v_Result { get; set; }
    }
}
