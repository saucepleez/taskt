using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    /// <summary>
    /// for deep search one UIElement from UIElement by TreeWalker commands
    /// </summary>
    public abstract class ADeepSearchOneUIElementFromUIElementByTreeWalkerCommands : ADeepSearchUIElementsFromUIElementByTreeWalkerCommands
    {
        [XmlAttribute]
        [PropertyIsOptional(true, "1")]
        public override string v_MaxNumberUIElements { get; set; }
    }
}
