using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Get List something from List Value commands
    /// </summary>
    public abstract class AListGetFromValueCommands : AListGetFromListCommands, IListGetFromValueProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_SearchValue))]
        [PropertyParameterOrder(7000)]
        public string v_Value { get; set; }
    }
}
