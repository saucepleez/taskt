using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Both Dictionary commands
    /// </summary>
    public abstract class ABothDictionaryCommands : AInputDictionaryCommands
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_BothDictionaryName))]
        public override string v_Dictionary { get; set; }
    }
}
