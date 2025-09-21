using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Add/Insert/Set something to JContainer commands
    /// </summary>
    public abstract class AJSONAddInsertSetJContainerCommands : AJSONJSONPathCommands, IJSONValueActionProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_ValueToAdd))]
        [PropertyParameterOrder(10000)]
        public virtual string v_Value { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_ValueType))]
        [PropertyParameterOrder(11000)]
        public virtual string v_ValueType { get; set; }
    }
}
