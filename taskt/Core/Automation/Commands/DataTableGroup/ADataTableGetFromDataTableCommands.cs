using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Get something from DataTable commands
    /// </summary>
    public abstract class ADataTableGetFromDataTableCommands : ADataTableInputDataTableCommands, IDataTableGetFromDataTable
    {
        [XmlAttribute]
        [PropertyParameterOrder(10000)]
        public abstract string v_Result { get; set; }
    }
}
