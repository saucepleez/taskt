using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Input Dictionary commands
    /// </summary>
    public abstract class ADictionaryInputDictionaryCommands : ScriptCommand, IDictionaryProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_InputDictionaryName))]
        [PropertyParameterOrder(5000)]
        public virtual string v_Dictionary { get; set; }
    }
}
