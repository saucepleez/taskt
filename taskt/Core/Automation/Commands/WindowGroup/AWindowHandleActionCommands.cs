using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// window handle action commands
    /// </summary>
    public abstract class AWindowHandleActionCommands : AWindowHandleCommands, IWindowHandleActionPropeties
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
