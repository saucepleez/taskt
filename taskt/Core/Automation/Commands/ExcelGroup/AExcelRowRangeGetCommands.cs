using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for row Range Get commands
    /// </summary>
    public abstract class AExcelRowRangeGetCommands : AExcelRowRangeCommands, IExcelRowRangeGetProperties
    {
        [XmlAttribute]
        [PropertyParameterOrder(10000)]
        public abstract string v_Result { get; set; }
    }
}
