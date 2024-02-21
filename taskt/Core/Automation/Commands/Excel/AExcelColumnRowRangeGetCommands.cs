using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Excel Column Row Range (2D-Range) Get commands
    /// </summary>
    public abstract class AExcelColumnRowRangeGetCommands : AExcelColumnRowRangeCommands, IExcelColumnRowRangeGetProperties
    {
        [XmlAttribute]
        [PropertyParameterOrder(11000)]
        public abstract string v_Result { get; set; }
    }
}
