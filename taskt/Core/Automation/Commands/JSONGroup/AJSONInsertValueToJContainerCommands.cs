using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// Insert Value to JContainer commands
    /// </summary>
    public abstract class AJSONInsertValueToJContainerCommands : AJSONAddInsertSetJContainerCommands
    {
        [XmlAttribute]
        [PropertyDescription("Value to Insert")]
        public override string v_Value { get; set; }
    }
}
