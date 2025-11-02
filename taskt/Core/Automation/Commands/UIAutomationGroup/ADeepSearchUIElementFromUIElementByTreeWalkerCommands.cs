using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    /// <summary>
    /// for deep search UIElements commands
    /// </summary>
    public abstract class ADeepSearchUIElementsFromUIElementByTreeWalkerCommands : ACoreSearchUIElementsFromUIElementByTreeWalkerCommands, IUIElementDeepSearchParametersProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_MaxDepth))]
        [PropertyParameterOrder(7992)]
        public string v_MaxDepth { get; set; }
    }
}
