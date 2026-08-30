using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Get From DataTable Row commands
    /// </summary>
    public abstract class ADataTableGetFromDataTableRowCommands : ADataTableRowCommands, IResultProperties
    {
        [XmlAttribute]
        [PropertyParameterOrder(10000)]
        public abstract string v_Result { get; set; }
    }
}
