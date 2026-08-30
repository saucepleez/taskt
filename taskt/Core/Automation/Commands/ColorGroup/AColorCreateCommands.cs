using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Color Create commands
    /// </summary>
    public abstract class AColorCreateCommands : ScriptCommand, IColorCreateProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ColorControls), nameof(ColorControls.v_InputColorVariableName))]
        [PropertyParameterOrder(5000)]
        public string v_Color { get; set; }
    }
}
