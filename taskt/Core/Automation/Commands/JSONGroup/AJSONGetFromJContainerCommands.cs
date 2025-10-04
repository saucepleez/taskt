using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Get From JContainer(Object or Array) commands
    /// </summary>
    public abstract class AJSONGetFromJContainerCommands : AJSONGetFromJSONCommands, IJSONInputJContainer, IJSONJSONPathProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_JSONPath))]
        [PropertyParameterOrder(6000)]
        public virtual string v_JsonExtractor { get; set; }
    }
}
