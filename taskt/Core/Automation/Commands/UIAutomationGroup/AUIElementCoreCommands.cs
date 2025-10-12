using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    /// <summary>
    /// for UIElement commands core (UIElement action or get from UIElement)
    /// </summary>
    public abstract class AUIElementCoreCommands : ScriptCommand, IUIElementCoreProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_InputUIElementName))]
        [PropertyParameterOrder(5000)]
        public virtual string v_TargetElement { get; set; }
    }
}
