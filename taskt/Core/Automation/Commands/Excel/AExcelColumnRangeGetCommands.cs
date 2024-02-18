using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for excel column range get commands
    /// </summary>
    public abstract class AExcelColumnRangeGetCommands : AExcelColumnRangeCommand, IExcelColumnRangeGetProperties
    {
        [XmlAttribute]
        [PropertyParameterOrder(10000)]
        public abstract string v_Result { get; set; }
    }
}
