using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for JSON and JSONPath commands
    /// </summary>
    public abstract class AJSONJSONPathCommands : AJSONInputJSONCommands, IJSONJSONPathProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_BothJSONName))]
        public override string v_Json { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_JSONPath))]
        [PropertyParameterOrder(6000)]
        public virtual string v_JsonExtractor { get; set; }
    }
}
