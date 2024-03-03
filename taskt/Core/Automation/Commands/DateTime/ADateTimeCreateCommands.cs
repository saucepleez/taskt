using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Create DateTime variable commands
    /// </summary>
    public abstract class ADateTimeCreateCommands : ScriptCommand, IDateTimeCreateProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DateTimeControls), nameof(DateTimeControls.v_OutputDateTime))]
        [PropertyParameterOrder(5000)]
        public virtual string v_DateTime { get; set; }
    }
}
