using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    /// <summary>
    /// for Get from UIElement or UIElement Action commands base
    /// </summary>
    public abstract class ADoSomethingUIElementCommands : AUIElementCoreCommands, IDoSomethingUIElementProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WindowNameResult))]
        [PropertyParameterOrder(10000)]
        public virtual string v_WindowNameResult { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_OutputWindowHandle))]
        [PropertyParameterOrder(10100)]
        public virtual string v_WindowHandleResult { get; set; }
    }
}
