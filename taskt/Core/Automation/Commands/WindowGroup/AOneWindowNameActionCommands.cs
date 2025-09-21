using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for one window name action commands
    /// </summary>
    public abstract class AOneWindowNameActionCommands : AOneWindowNameCommands, IOneWindowNameActionProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WaitTimeBetweenFindAndAction))]
        [PropertyParameterOrder(10000)]
        public virtual string v_WaitTimeBetweenFindAndAction { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_ActivateBeforeAction))]
        [PropertyParameterOrder(10010)]
        public virtual string v_ActivateBeforeAction { get; set; }
    }
}
