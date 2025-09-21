using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Create List From List commands
    /// </summary>
    public abstract class AListCreateFromListCommands : ScriptCommand, IListCreateFromListProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_InputListName))]
        [PropertyParameterOrder(5000)]
        public virtual string v_TargetList { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_NewOutputListName))]
        [PropertyParameterOrder(10000)]
        public virtual string v_NewList { get; set; }
    }
}
