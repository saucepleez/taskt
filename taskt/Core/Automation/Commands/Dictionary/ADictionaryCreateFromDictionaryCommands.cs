using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Create Dictionary From Dictionary commands
    /// </summary>
    public abstract class ADictionaryCreateFromDictionaryCommands : ScriptCommand, IDictionaryCreateFromDictionary
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_InputDictionaryName))]
        [PropertyParameterOrder(5000)]
        public abstract string v_TargetDictionary { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_NewOutputDictionaryName))]
        [PropertyParameterOrder(10000)]
        public virtual string v_NewDictionary { get; set; }
    }
}
