using System.Data;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    /// <summary>
    /// for UIElement Action from something by XPath commands
    /// </summary>
    public abstract class AUIElementActionFromSomethingByXPathCommands : AUIElementActionSomewayCommands, IUIElementCoreSearchXPathProperties, IUIElementDeepSearchXPathProperties
    {
        [XmlAttribute]
        [PropertyParameterOrder(5500)]
        public override string v_AutomationType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_SearchXPath))]
        [PropertyParameterOrder(6000)]
        public virtual string v_SearchXPath { get; set; }

        [XmlElement]
        [PropertyParameterOrder(6500)]
        public override DataTable v_UIAActionParameters { get; set; }
    }
}
