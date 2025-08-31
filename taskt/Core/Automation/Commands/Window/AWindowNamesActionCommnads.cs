using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for window names action commands
    /// </summary>
    public abstract class AWindowNamesActionCommnads : AWindowNamesCommands, IWindowNamesActionProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WaitTimeBetweenFindAndAction))]
        [PropertyParameterOrder(10000)]
        public string v_WaitTimeBetweenFindAndAction { get; set; }
    }
}
