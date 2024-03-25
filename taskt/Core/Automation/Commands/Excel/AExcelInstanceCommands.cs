using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for commands that using Excel instance
    /// </summary>
    public abstract class AExcelInstanceCommands : ScriptCommand, ILExcelInstanceProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        [PropertyParameterOrder(5000)]
        public virtual string v_InstanceName { get; set; }
    }
}
