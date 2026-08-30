using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    /// <summary>
    /// for descendants search UIELements From Window Handle by TreeWalker commands
    /// </summary>
    public abstract class ADescendantsSearchUIElementsFromWindowHandleByTreeWalkerCommands : ADescendantsSearchUIElementsFromSomethingByTreeWalkerCommands, IWindowHandleProperties, IUIElementSearchUIElementFromWindowSomethingByAnywayProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_InputWindowHandle))]
        [PropertyParameterOrder(5000)]
        
        public virtual string v_WindowHandle { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WaitTime))]
        [PropertyParameterOrder(8200)]
        public virtual string v_WaitTimeForWindow { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_WindowUIElementName))]
        [PropertyParameterOrder(10200)]
        public virtual string v_WindowUIElement { get; set; }
    }
}
