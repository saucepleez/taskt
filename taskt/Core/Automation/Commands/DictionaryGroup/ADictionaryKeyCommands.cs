using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Dictionary key commands
    /// </summary>
    public abstract class ADictionaryKeyCommands : ADictionaryInputDictionaryCommands, IDictionaryKeyProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_Key))]
        [PropertyParameterOrder(6000)]
        public virtual string v_Key { get; set; }
    }
}
