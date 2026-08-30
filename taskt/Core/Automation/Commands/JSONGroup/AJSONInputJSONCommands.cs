using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Input JSON commands
    /// </summary>
    public abstract class AJSONInputJSONCommands : ScriptCommand, IJSONInputJSONProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_InputJSONName))]
        [PropertyParameterOrder(5000)]
        public virtual string v_Json { get; set; }
    }
}
