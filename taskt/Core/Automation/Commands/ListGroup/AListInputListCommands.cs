using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for List Input Commands
    /// </summary>
    public abstract class AListInputListCommands : ScriptCommand, IListProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_InputListName))]
        [PropertyParameterOrder(5000)]
        public virtual string v_List { get; set; }
    }
}
