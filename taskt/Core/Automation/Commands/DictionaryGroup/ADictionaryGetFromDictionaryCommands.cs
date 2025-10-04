using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Get from Dictionary, or Dictionary convert to something commands
    /// </summary>
    public abstract class ADictionaryGetFromDictionaryCommands : ADictionaryInputDictionaryCommands, IDictionaryGetFromDictionaryProperties
    {
        [XmlAttribute]
        [PropertyParameterOrder(7000)]
        public abstract string v_Result { get; set; }
    }
}
