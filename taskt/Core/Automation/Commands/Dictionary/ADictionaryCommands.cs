using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Dictionary commands
    /// </summary>
    public abstract class ADictionaryCommands : ScriptCommand, ILDictionaryProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_InputDictionaryName))]
        public virtual string v_Dictionary { get; set; }
    }
}
