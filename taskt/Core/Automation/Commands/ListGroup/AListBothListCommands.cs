using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Both List Commands
    /// </summary>
    public abstract class AListBothListCommands : AListInputListCommands
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_BothListName))]
        public override string v_List { get; set; }
    }
}
