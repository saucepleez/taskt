using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Dictionary Key Action commands
    /// </summary>
    public abstract class ADictionaryKeyActionCommands : ADictionaryKeyCommands, IDictionaryKeyActionProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_WhenKeyDoesNotExists))]
        [PropertyParameterOrder(10000)]
        public virtual string v_WhenKeyDoesNotExists { get; set; }
    }
}
