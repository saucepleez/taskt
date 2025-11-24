using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.WindowGroup;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    /// <summary>
    /// for core search UIELement from something by XPath commands
    /// </summary>
    public abstract class ACoreSearchUIElementsFromSomethingByXPathCommands : ScriptCommand, IUIElementCoreSearchXPathProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_SearchXPath))]
        [PropertyParameterOrder(6000)]
        public virtual string v_SearchXPath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_WaitTimeForUIElement))]
        [PropertyParameterOrder(7990)]
        public virtual string v_WaitTimeForUIElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_MaxSiblings))]
        [PropertyParameterOrder(7991)]
        public virtual string v_MaxSiblings { get; set; }

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
