using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for DateTime value convert commands
    /// </summary>
    public abstract class ADateTimeConvertCommands : ScriptCommand, IDateTimeConvertProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DateTimeControls), nameof(DateTimeControls.v_InputDateTime))]
        [PropertyParameterOrder(5000)]
        public virtual string v_DateTime { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyParameterOrder(7000)]
        public abstract string v_Result { get; set; }
    }
}
