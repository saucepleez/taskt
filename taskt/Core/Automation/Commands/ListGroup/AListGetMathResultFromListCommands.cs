using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Get Math Result from List commands
    /// </summary>
    public abstract class AListGetMathResultFromListCommands : AListGetFromListCommands, IListGetMathResultFromListProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public override string v_Result { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_WhenValueIsNotNumeric))]
        [PropertyParameterOrder(11000)]
        public virtual string v_WhenValueIsNotNumeric { get; set; }
    }
}
