using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// Set Value of JContainer commands
    /// </summary>
    public abstract class AJSONSetValueOfJContainerCommands : AJSONAddInsertSetJContainerCommands
    {
        [XmlAttribute]
        [PropertyDescription("Value to Set")]
        public override string v_Value { get; set; }
    }
}
