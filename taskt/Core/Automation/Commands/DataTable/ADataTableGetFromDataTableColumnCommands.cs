using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Get Something From DataTable Column commands
    /// </summary>
    public abstract class ADataTableGetFromDataTableColumnCommands : ADataTableColumnCommands, IResultProperties
    {
        [XmlAttribute]
        [PropertyParameterOrder(10000)]
        public abstract string v_Result { get; set; }
    }
}
