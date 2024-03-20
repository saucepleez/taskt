using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Input Dictionary commands
    /// </summary>
    public abstract class AInputDictionaryCommands : ScriptCommand, ILDictionaryProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_InputDictionaryName))]
        public virtual string v_Dictionary { get; set; }
    }
}
