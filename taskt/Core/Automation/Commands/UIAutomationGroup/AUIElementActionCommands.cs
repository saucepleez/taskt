using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    /// <summary>
    /// UIElement action commands
    /// </summary>
    public abstract class AUIElementActionCommands : ADoSomethingUIElementCommands, IUIElementActionProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_ActivateWindow))]
        [PropertyParameterOrder(8000)]
        public virtual string v_ActivateWindowBeforeAction { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_WaitTimeBeforeAction))]
        [PropertyParameterOrder(9000)]
        public virtual string v_WaitTimeBeforeAction { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_WaitTimeAfterAction))]
        [PropertyParameterOrder(9000)]
        public virtual string v_WaitTimeAfterAction { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_WhenActionIsNotSupported))]
        [PropertyParameterOrder(11000)]
        public virtual string v_WhenActionIsNotSupported { get; set; }
    }
}
